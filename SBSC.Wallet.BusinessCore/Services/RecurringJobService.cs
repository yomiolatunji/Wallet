using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using SBSC.Wallet.BusinessCore.DbModels;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class RecurringJobService
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            RecurringJob.AddOrUpdate("AccruedInterestJob", () => ComputeInterest(serviceProvider), Cron.Daily(2, 0));
            RecurringJob.AddOrUpdate("PayInterestJob", () => PayInterest(serviceProvider), Cron.Monthly(1, 1, 0));
        }
        public static void ComputeInterest(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WalletContext>();
                if (context == null)
                    return;
                var interestRate = 3.75M;
                var daysInYear = 365;
                var wallets = context.Wallets.Where(a => a.IsActive.GetValueOrDefault() && a.AcyBalance > 0).ToList();
                foreach (var wallet in wallets)
                {
                    using (var dbtransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var interest = wallet.AcyBalance * 1 * interestRate / (100 * daysInYear);
                            var interestPayable = new InterestPayable
                            {
                                Amount = interest,
                                Currency = wallet.Currency,
                                WalletId = wallet.Id,
                                ValueDate = DateTime.Today.AddDays(-1),
                                RunDate= DateTime.Now
                            };
                            context.InterestPayables.Add(interestPayable);
                            context.SaveChanges();

                            wallet.InterestReceiveable += interest;

                            context.Update(wallet);
                            context.SaveChanges();
                            dbtransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbtransaction.Rollback();
                        }
                    }
                }
            }
        }
        public static void PayInterest(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WalletContext>();
                if (context == null)
                    return;

                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                var startDate = month.AddMonths(-1);
                var endDate = month.AddDays(-1);

                var transactions = context.InterestPayables.Where(a => a.ValueDate >= startDate && a.ValueDate <= endDate)
                    .GroupBy(a => new { a.WalletId, a.Currency })
                    .Select(a => new { a.Key.WalletId, a.Key.Currency, Amount = a.Sum(z => z.Amount) })
                    .Where(a => a.Amount > 0).ToList();
                foreach (var transaction in transactions)
                {
                    using (var dbtransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var wallet = context.Wallets.FirstOrDefault(a => a.Id == transaction.WalletId);
                            if (wallet == null) continue;

                            wallet.InterestReceiveable = 0;
                            wallet.AcyBalance += transaction.Amount;

                            context.SaveChanges();
                            var interestPayable = new InterestPayable
                            {
                                Amount = -transaction.Amount,
                                Currency = wallet.Currency,
                                WalletId = wallet.Id,
                                ValueDate = DateTime.Today.AddDays(-1),
                                RunDate=DateTime.Now
                            };
                            context.InterestPayables.Add(interestPayable);
                            context.SaveChanges();

                            var transactionDto = new Transaction
                            {
                                Amount = transaction.Amount,
                                Currency = wallet.Currency,
                                WalletId = wallet.Id,
                                Narration = $"Interest for the month {month.ToString("MMM - yyyy")}",
                                TransactionCode = "INT",
                                TransactionType = "D",
                                TransactionDate = DateTime.Now
                            };
                            context.Transactions.Add(transactionDto);
                            context.SaveChanges();

                            dbtransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbtransaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
