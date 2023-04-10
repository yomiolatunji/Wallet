using System;
using System.Collections.Generic;

namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class Audit
{
    public long Id { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? PrimaryKey { get; set; }

    public string? UserId { get; set; }

    public string? Type { get; set; }

    public string? TableName { get; set; }

    public DateTime? Date { get; set; }

    public string? AffectedColumn { get; set; }
}
