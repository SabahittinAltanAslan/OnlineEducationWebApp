namespace OnlineEducationWebApp.Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SchNumber { get; set; }
        public DateTime BirthDay { get; set; }

        //Navigation Property
        public ICollection<StudentLesson> StudentLessons { get; set; }

    }
}
