using System;
using System.Collections.Generic;

namespace SBSC.Wallet.BusinessCore.DbModels;

public partial class User
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public bool IsDeleted { get; set; }

    public long CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DateUpdated { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual ICollection<Wallet> Wallets { get; } = new List<Wallet>();
}
