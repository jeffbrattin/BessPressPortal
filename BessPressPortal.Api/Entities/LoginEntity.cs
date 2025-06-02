using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace BessPressPortal.Api.Entities
{
    public class LoginEntity : ITableEntity // <--- This class *does* implement ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail{ get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords, not plain text

        public string VerificationCode { get; set; }

        public DateTime CreatedDate { get; set; }
       // public DateTime LastLogin { get; set; }
    }

}
