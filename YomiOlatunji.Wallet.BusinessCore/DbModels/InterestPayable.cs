using System;
using System.Collections.Generic;

namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class InterestPayable
{
    public long Id { get; set; }

    public long WalletId { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime ValueDate { get; set; }

    public DateTime RunDate { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
