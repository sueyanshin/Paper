namespace Paper.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsFlipped { get; set; }
    }
}
