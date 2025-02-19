using System.Collections.Generic;

namespace Paper.Models
{
    public class Content
    {
        public int ContentId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Summary { get; set; }
        public string CreatedAt { get; set; }
        public List<Flashcard> Flashcards { get; set; }
    }
}
