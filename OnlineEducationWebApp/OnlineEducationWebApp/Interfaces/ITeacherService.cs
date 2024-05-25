using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface ITeacherService
    {
        public Task<List<Teacher>> GetAllAsync();
        public Task<Teacher> GetByIdAsync(int id);
        public Task<Teacher> CreateAsync(Teacher teacher);
        public Task UpdateAsync(Teacher teacher);
        public Task DeleteAsync(int id);

    }
}
