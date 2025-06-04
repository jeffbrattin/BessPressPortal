namespace BessPressPortal.Api.Services
{
    using Azure.Data.Tables;
    using Azure;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using global::BessPressPortal.Api.Entities;

        public class NotesService
        {
            private readonly TableClient _tableClient;
            private readonly string _tableName = "Notes"; // Your table name

            public NotesService(IConfiguration configuration)
            {
                string connectionString = configuration["AzureStorage:TableConnectionString"];
                _tableClient = new TableClient(connectionString, _tableName);
                _tableClient.CreateIfNotExists(); // Create the table if it doesn't exist
            }

            public async Task AddNoteAsync(NoteEntity note)
            {
                if (string.IsNullOrEmpty(note.PartitionKey))
                {
                    note.PartitionKey = "AllNotes"; // Example: Fixed partition key, or use User ID from Identity
                }
                if (string.IsNullOrEmpty(note.RowKey))
                {
                    note.RowKey = Guid.NewGuid().ToString(); // Ensure unique row key
                }
                note.CreatedDate = DateTime.UtcNow;
                note.ExpirationDate = DateTime.UtcNow.AddMonths(6); // Set expiration

                await _tableClient.AddEntityAsync(note);
            }

            public async Task<NoteEntity> GetNoteAsync(string partitionKey, string rowKey)
            {
                try
                {
                    Response<NoteEntity> response = await _tableClient.GetEntityAsync<NoteEntity>(partitionKey, rowKey);
                    return response.Value;
                }
                catch (RequestFailedException ex) when (ex.Status == 404)
                {
                    return null; // Entity not found
                }
            }

            public async Task<List<NoteEntity>> GetNotesByPartitionAsync(string partitionKey)
            {
                List<NoteEntity> notes = new List<NoteEntity>();
                // Use .AsPages() for efficient pagination on large results, or just QueryAsync for smaller sets
                await foreach (NoteEntity entity in _tableClient.QueryAsync<NoteEntity>(filter: $"PartitionKey eq '{partitionKey}'"))
                {
                    notes.Add(entity);
                }
                return notes;
            }

            public async Task UpdateNoteAsync(NoteEntity note)
            {
                await _tableClient.UpdateEntityAsync(note, note.ETag, TableUpdateMode.Replace);
            }

            public async Task DeleteNoteAsync(string partitionKey, string rowKey)
            {
                await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            }
        }
    }
