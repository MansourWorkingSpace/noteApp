using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Web;

namespace MauiApp4.Infrastructure.WebServices
{
    // Lightweight consumer for https://jsonplaceholder.typicode.com used for demo/sync
    public class NotesApiService : INotesApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com";

        public NotesApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        private class PostDto { public int userId { get; set; } public int id { get; set; } public string title { get; set; } = string.Empty; public string body { get; set; } = string.Empty; }
        private class UserDto { public int id { get; set; } public string name { get; set; } = string.Empty; public string email { get; set; } = string.Empty; }

        public async Task<List<Note>> GetNotesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/posts");
                if (!response.IsSuccessStatusCode) return new List<Note>();
                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<PostDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PostDto>();
                var notes = new List<Note>();
                foreach (var p in posts)
                {
                    notes.Add(new Note
                    {
                        Id = p.id,
                        Title = p.title ?? string.Empty,
                        Content = p.body ?? string.Empty,
                        UserId = p.userId,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                return notes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.GetNotesAsync error: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<Note?> GetNoteAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/posts/{id}");
                if (!response.IsSuccessStatusCode) return null;
                var json = await response.Content.ReadAsStringAsync();
                var p = JsonSerializer.Deserialize<PostDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (p is null) return null;
                return new Note { Id = p.id, Title = p.title ?? string.Empty, Content = p.body ?? string.Empty, UserId = p.userId, CreatedAt = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.GetNoteAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<Note?> CreateNoteAsync(Note note)
        {
            try
            {
                var dto = new PostDto { userId = note.UserId, title = note.Title ?? string.Empty, body = note.Content ?? string.Empty };
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/posts", content);
                if (!response.IsSuccessStatusCode) return null;
                var responseJson = await response.Content.ReadAsStringAsync();
                var created = JsonSerializer.Deserialize<PostDto>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (created is null) return null;
                return new Note { Id = created.id, Title = created.title ?? string.Empty, Content = created.body ?? string.Empty, UserId = created.userId, CreatedAt = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.CreateNoteAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<Note?> UpdateNoteAsync(int id, Note note)
        {
            try
            {
                var dto = new PostDto { userId = note.UserId, title = note.Title ?? string.Empty, body = note.Content ?? string.Empty, id = id };
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/posts/{id}", content);
                if (!response.IsSuccessStatusCode) return null;
                var responseJson = await response.Content.ReadAsStringAsync();
                var updated = JsonSerializer.Deserialize<PostDto>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (updated is null) return null;
                return new Note { Id = updated.id, Title = updated.title ?? string.Empty, Content = updated.body ?? string.Empty, UserId = updated.userId, CreatedAt = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.UpdateNoteAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/posts/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.DeleteNoteAsync error: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserAsync(int userId = 1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/users/{userId}");
                if (!response.IsSuccessStatusCode) return null;
                var json = await response.Content.ReadAsStringAsync();
                var u = JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (u is null) return null;
                return new User { Id = u.id, Name = u.name ?? string.Empty, Email = u.email ?? string.Empty, CreatedAt = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesApiService.GetUserAsync error: {ex.Message}");
                return null;
            }
        }
    }
}
