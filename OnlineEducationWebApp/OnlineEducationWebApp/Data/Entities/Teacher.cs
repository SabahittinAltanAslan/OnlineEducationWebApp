namespace OnlineEducationWebApp.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public List<Lesson> Lessons { get; set; }

    }
}
