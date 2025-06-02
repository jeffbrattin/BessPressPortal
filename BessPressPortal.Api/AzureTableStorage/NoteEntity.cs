namespace BessPressPortal.Api.AzureTableStorage
{
    using Azure;
    using Azure.Data.Tables;
    using System;

    public class NoteEntity : ITableEntity
    {
        // PartitionKey: Groups related entities. Good for queries.
        // For notes, UserId or a combination of UserId and a category could work.
        public string PartitionKey { get; set; }

        // RowKey: Unique identifier within a partition.
        // For notes, a unique ID like a GUID or timestamp works well.
        public string RowKey { get; set; }

        // Your custom properties
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; } // For your 6-month lifespan

        // ITableEntity properties (required)
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

}
