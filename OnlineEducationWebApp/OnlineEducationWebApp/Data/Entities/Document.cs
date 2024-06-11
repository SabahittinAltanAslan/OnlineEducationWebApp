namespace OnlineEducationWebApp.Data.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; } 
        public string OriginalFileName { get; set; } 
        public string Description { get; set; }
        public string FilePath { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
