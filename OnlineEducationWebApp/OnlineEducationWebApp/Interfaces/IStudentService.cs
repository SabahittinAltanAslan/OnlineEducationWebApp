using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentForLessonAsync(int id);
        public Task<Student> GetStudentByIdAsync(int id);
        public Task<Student> CreateAsync(Student student);
        public Task DeleteAsync(int id);

    }
}
