using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.CoreObject.Enumerables;

namespace YomiOlatunji.Wallet.BusinessCore.AuditLogs
{
    public class AuditHelper
    {
        public AuditHelper(EntityEntry entity)
        {
            Entity = entity;
        }

        public EntityEntry Entity { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();

        public Audit ToAudit()
        {
            Audit audit = new();
            audit.UserId = UserId;
            audit.Type = AuditType.ToString();
            audit.TableName = TableName;
            audit.Date = DateTime.Now;
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumn = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return audit;
        }
    }
}