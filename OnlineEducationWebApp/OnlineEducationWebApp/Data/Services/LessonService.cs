using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;
using OnlineEducationWebApp.Models.DTO;

namespace OnlineEducationWebApp.Data.Services
{
    public class LessonService : ILessonService
    {
        private readonly ProjectContext _context;

        public LessonService(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await _context.Lessons.ToListAsync();
        }

        public async Task<Lesson> GetLessonByUrl(string url)
        {
            return await _context.Lessons.FirstOrDefaultAsync(x => x.Url == url);
        }

        public async Task<Lesson> CreateAsync(LessonCreateRequest request, int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher == null)
                return null;

            var lesson = new Lesson
            {
                Name = request.Name,
                Description = request.Description,
                LessonDate = request.LessonDate + request.LessonTime,
                TeacherId = teacherId,
                Url = Guid.NewGuid().ToString() + Guid.NewGuid().ToString("N")
            };

            try
            {
                await _context.Lessons.AddAsync(lesson);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return lesson;
        }

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Lessons.FindAsync(id);
            _context.Lessons.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Lesson>> GetTeacherLessonAsync(int teacherId)
        {
            return await _context.Lessons
                .Where(lesson => lesson.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetStudentLessonAsync(int studentId)
        {
            return await _context.StudentLessons
                .Where(x => x.StudentId == studentId)
                .Select(x => x.Lesson)
                .ToListAsync();
        }

        public async Task<Lesson> JoinLesson(string lessonUrl, int userId)
        {
            var lesson = await GetLessonByUrl(lessonUrl);
            if (lesson == null)
                return null;

            // check if lesson is started
            if (lesson.StartedAt == null)
                return null;

            // Derse öğretmen mi yoksa öğrenci mi katılmaya çalışıyor kontrol et
            if (await _context.Teachers.AnyAsync(x => x.Id == userId))
                return lesson;

            // Öğrenci derse kayıt olmuş mu kontrol et
            if (!await IsStudentSubscribedToLesson(lessonUrl, userId))
                return null;

            return lesson;
        }

        public async Task<bool> IsStudentSubscribedToLesson(string lessonUrl, int studentId)
        {
            return await _context.StudentLessons
                .AnyAsync(x => x.Lesson.Url == lessonUrl && x.StudentId == studentId);
        }

        public async Task<Lesson> StartLesson(string lessonUrl)
        {
            List<StudentLesson> lessonStudents = null;
            Lesson lesson = null;

            try
            {
                lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Url == lessonUrl);
                if (lesson == null)
                    return null;

                lesson.StartedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                lessonStudents = await _context.StudentLessons
                    .Where(x => x.LessonId == lesson.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

            // Tüm öğrencilerin mail adreslerini al
            //var studentEmails = lessonStudents.Select(x => x.Student.Email).ToList();

            // tüm öğrencilere mail gönder
            //MailService.SendMail(studentEmails, "reminder@onlineedu.com.tr", "Ders Başladı", $"<h1>Dersiniz başlamıştır. Lütfen dersinize katılın.</h1>\r\n<a href=\"https://localhost:1234/class/{lesson.Url}\">Derse git</a>");

            return lesson;
        }

        public async Task<Lesson> FinishLesson(int lessonId)
        {
            Lesson lesson = null;
            try
            {
                lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
                if (lesson == null)
                    return null;

                lesson.FinishedAt = DateTime.Now;
                lesson.IsCompleted = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

            return lesson;
        }

        public async Task Subscribe(int studentId, int lessonId)
        {
            if (lessonId == 0 || studentId == 0)
            {
                return;
            }

            StudentLesson studentLesson = new StudentLesson
            {
                LessonId = lessonId,
                StudentId = studentId
            };

            if (await _context.StudentLessons.ContainsAsync(studentLesson))
            {
                return;
            }

            await _context.StudentLessons.AddAsync(studentLesson);
            await _context.SaveChangesAsync();
        }
    }
}