namespace BessPressPortal.Api.Controllers
{
    using global::BessPressPortal.Api.Entities;
    using global::BessPressPortal.Api.Services;
    using global::BessPressPortal.Shared.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace BessPressPortal.Server.Controllers // Use the server's controller namespace
    {
        [ApiController]
        [Route("api/[controller]")] // This means endpoints will be /api/Notes
        public class NotesController : ControllerBase
        {
            private readonly NotesService _notesService;

            public NotesController(NotesService notesService)
            {
                _notesService = notesService;
            }

            [HttpGet]
            public async Task<ActionResult<List<NoteEntity>>> GetNotes() // <-- Returns NoteEntity
            {
                var notes = await _notesService.GetNotesByPartitionAsync("AllNotes");
                return Ok(notes); // Sending NoteEntity directly
            }

            [HttpPost]
            public async Task<ActionResult> AddNote([FromBody] CreateNoteDto createNoteDto) // <-- Accept CreateNoteDto
            {
                // ASP.NET Core's [ApiController] will automatically handle ModelState.IsValid and return 400
                // if [Required] attributes on CreateNoteDto are violated.
                // No need for 'if (createNoteDto == null)' here usually, as model binding would handle it.

                var noteEntity = new NoteEntity
                {
                    Title = createNoteDto.Title,
                    Content = createNoteDto.Content,
                    CreatedDate = DateTime.UtcNow, // Server sets these dates
                    ExpirationDate = DateTime.UtcNow.AddMonths(6), // Server sets these dates
                    PartitionKey = "AllNotes", // Server sets PartitionKey
                    RowKey = Guid.NewGuid().ToString() // Server generates RowKey
                };

                await _notesService.AddNoteAsync(noteEntity);
                return StatusCode(201);
            }

            [HttpDelete("{partitionKey}/{rowKey}")]
            public async Task<ActionResult> DeleteNote(string partitionKey, string rowKey)
            {
                await _notesService.DeleteNoteAsync(partitionKey, rowKey);
                return NoContent(); // 204 No Content
            }

            // You could add Update and Get single note endpoints as well
        }
    }
}
