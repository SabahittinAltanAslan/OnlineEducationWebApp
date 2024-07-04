namespace OnlineEducationWebApp.Data.Entities
{
    public class StudentLesson
    {
        public int StudentId { get; set; }
        public string? ConnectionId { get; set; }
        public Student Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}