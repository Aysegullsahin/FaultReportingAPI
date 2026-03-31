using System.ComponentModel.DataAnnotations.Schema;

namespace FaultReportingAPI.Core.Models.Base
{

    public class CoreBaseEntity
    {
       // public long Id { get; set; }
    }
    public class BaseEntity : CoreBaseEntity
    {
        public long Id { get; set; }

        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedUser { get; set; }


        [ForeignKey(nameof(UpdatedById))]
        public User? UpdatedUser { get; set; }

        [ForeignKey(nameof(DeletedById))]
        public User? DeletedUser { get; set; }
    }
}
