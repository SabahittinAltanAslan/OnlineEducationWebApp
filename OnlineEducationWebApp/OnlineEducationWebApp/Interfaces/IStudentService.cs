using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IStudentService
    {
        public Task<List<Student>> GetAllStudentsAsync();
        public Task<Student> GetStudentByIdAsync(int id);
        public Task<Student> CreateAsync(Student student);

        public Task UpdateAsync(Student student);
        public Task DeleteAsync(int id);

    }
}
