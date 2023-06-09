﻿namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class Wallet
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string WalletNumber { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public decimal AcyBalance { get; set; }

    public decimal LcyBalance { get; set; }

    public decimal AvailableBalance { get; set; }

    public decimal LienAmount { get; set; }

    public decimal InterestReceiveable { get; set; }

    public bool? DebitRestricted { get; set; }

    public bool? CreditRestricted { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public long CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DateUpdated { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual ICollection<InterestPayable> InterestPayables { get; } = new List<InterestPayable>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}