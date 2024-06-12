namespace OnlineEducationWebApp.Data.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        
        //Navigation Property
        public ICollection<StudentLesson> StudentLessons { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public List<Document> Documents { get; set; }

    }
}
