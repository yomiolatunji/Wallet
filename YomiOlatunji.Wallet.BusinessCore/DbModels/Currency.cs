namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class Currency
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public string? CurrencyLogoUrl { get; set; }

    public bool IsDeleted { get; set; }

    public long CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DateUpdated { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime? DateDeleted { get; set; }
}