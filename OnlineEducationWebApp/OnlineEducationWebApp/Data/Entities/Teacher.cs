namespace OnlineEducationWebApp.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

    }
}
