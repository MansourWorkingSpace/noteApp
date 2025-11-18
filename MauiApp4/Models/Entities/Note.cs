using System;

namespace MauiApp4.Models.Entities
{
    // Plain POCO entity for Note. We'll map fields manually in the DAO.
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        // Relation to User
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
