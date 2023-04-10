namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class Admin
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public long CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DateUpdated { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime? DateDeleted { get; set; }
}