using System.ComponentModel;

namespace OnlineEducationWebApp.Data.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        [DisplayName("Ders")]
        public string Name { get; set; }

        public string Url { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Tarih")]
        public DateTime LessonDate { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }
        public bool IsCompleted { get; set; }

        //Navigation Property
        public ICollection<StudentLesson> StudentLessons { get; set; }

        [DisplayName("Öğretmen")]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }
        public List<Document> Documents { get; set; }
    }
}