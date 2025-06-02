namespace BessPressPortal.Api.Entities
{
    using Azure;
    using Azure.Data.Tables; 
    public class NoteEntity : ITableEntity // <--- This class *does* implement ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
