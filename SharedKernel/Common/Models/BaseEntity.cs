using System.ComponentModel.DataAnnotations;

namespace NexoraEnterprise.SharedKernel.Common.Models
{
    public interface  IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

       

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? DeletedBy { get; set; }
    }
    
}
