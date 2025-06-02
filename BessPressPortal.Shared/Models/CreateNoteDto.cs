// BessPressPortal.Shared/Models/CreateNoteDto.cs
using System;
using System.ComponentModel.DataAnnotations; // You can add [Required] here for client validation

namespace BessPressPortal.Shared.Models
{
    public class CreateNoteDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        // No Id, PartitionKey, RowKey, Timestamp, ETag here.
    }
}