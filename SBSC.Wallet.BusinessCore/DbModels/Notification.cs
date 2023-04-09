using System;
using System.Collections.Generic;

namespace SBSC.Wallet.BusinessCore.DbModels;

public partial class Notification
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string UserType { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateRead { get; set; }

    public DateTime? DateDeleted { get; set; }
}
