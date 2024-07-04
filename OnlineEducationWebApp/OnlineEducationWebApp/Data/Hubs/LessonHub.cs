using Microsoft.AspNetCore.SignalR;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using System.Collections.Concurrent;

namespace OnlineEducationWebApp.Data.Hubs
{
    public class LessonHub : Hub
    {
        private readonly ProjectContext _context;

        public LessonHub(ProjectContext context)
        {
            _context = context;
        }

        private static ConcurrentDictionary<string, List<UserMessage>> lessonMessages = new ConcurrentDictionary<string, List<UserMessage>>();
        private static ConcurrentDictionary<string, List<string>> lessonUsers = new ConcurrentDictionary<string, List<string>>();

        // Bir ders için mesaj gönderme işlemini gerçekleştiren metot
        public async Task SendMessage(string lessonId, string user, string message)
        {
            var timestamp = DateTime.UtcNow;

            var msg = new UserMessage
            {
                User = user,
                Content = message,
                Timestamp = timestamp
            };

            if (lessonMessages.TryGetValue(lessonId, out var messages))
            {
                messages.Add(msg);
            }
            else
            {
                lessonMessages.TryAdd(lessonId, new List<UserMessage> { msg });
            }

            await Clients.Group(lessonId).SendAsync("ReceiveMessage", Context.ConnectionId, user, message, timestamp);
        }

        public async Task FinishLesson(string lessonId)
        {
            lessonMessages.TryRemove(lessonId, out _);

            foreach (var user in lessonUsers[lessonId])
            {
                await Groups.RemoveFromGroupAsync(user, lessonId);
            }

            lessonUsers.TryRemove(lessonId, out _);

            await Clients.Group(lessonId).SendAsync("FinishLesson");
        }

        // Kullanıcının kamerasını açıp kapama işlemini gerçekleştiren metot
        public async Task ToggleCamera(string lessonId, string userId, bool isCameraOn)
        {
            await Clients.Group(lessonId).SendAsync("ToggleCamera", userId, isCameraOn);
        }

        public async Task SendOffer(string lessonId, string senderId, string targetId, string sdp)
        {
            await Clients.Client(targetId).SendAsync("ReceiveOffer", senderId, sdp);
        }

        public async Task SendAnswer(string lessonId, string senderId, string targetId, string sdp)
        {
            await Clients.Client(targetId).SendAsync("ReceiveAnswer", senderId, sdp);
        }

        public async Task SendCandidate(string lessonId, string senderId, string targetId, string candidate)
        {
            await Clients.Client(targetId).SendAsync("ReceiveCandidate", senderId, candidate);
        }

        // Kullanıcının bir ders odasına katılmasını sağlayan metot
        public async Task UserJoinedToLesson(string lessonUrl, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lessonUrl.ToString());

            var lesson = _context.Lessons.FirstOrDefault(x => x.Url == lessonUrl);

            // Kullanıcıyı derse ekliyoruz
            lessonUsers.AddOrUpdate(lessonUrl.ToString(), new List<string> { Context.ConnectionId }, (key, existingList) =>
            {
                existingList.Add(Context.ConnectionId);
                return existingList;
            });

            // Mevcut kullanıcıların listesini yeni kullanıcıya gönderiyoruz
            var users = lessonUsers[lessonUrl.ToString()].Where(id => id != Context.ConnectionId).ToList();
            await Clients.Caller.SendAsync("ExistingUsers", users);

            // Diğer kullanıcılara yeni bir kullanıcının derse katıldığını bildiriyoruz
            await Clients.Group(lessonUrl.ToString()).SendAsync("UserJoined", Context.ConnectionId);

            // Kullanıcıya önceki mesajları yüklüyoruz
            if (lessonMessages.TryGetValue(lessonUrl.ToString(), out var messages))
            {
                await Clients.Caller.SendAsync("LoadMessages", messages);
            }

            var studentLesson = _context.StudentLessons.FirstOrDefault(x => x.LessonId == lesson.Id && x.StudentId == int.Parse(userId));
            if (studentLesson != null)
            {
                var student = _context.Students.FirstOrDefault(x => x.Id == int.Parse(userId));

                studentLesson.ConnectionId = Context.ConnectionId;

                // Kullanıcının ders odasına katıldığını bildiriyoruz
                await SendMessage(lessonUrl.ToString(), "System", $"{Context.ConnectionId} - {student.Name} {student.Surname} joined the lesson.");
            }

            var teacher = _context.Teachers.FirstOrDefault(x => x.Id == int.Parse(userId));
            if (teacher != null)
            {
                // Kullanıcının ders odasına katıldığını bildiriyoruz
                await SendMessage(lessonUrl.ToString(), "System", $"{Context.ConnectionId} - {teacher.Name} {teacher.Surname} joined the lesson.");
            }

            await base.OnConnectedAsync();
        }

        // Kullanıcının bir ders odasına katılmasını sağlayan metot
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"New user connected. ConnectionId: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        // Kullanıcının bir ders odasından ayrılmasını sağlayan metot
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var lessonId = lessonUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key;

            if (!string.IsNullOrEmpty(lessonId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lessonId);

                if (lessonUsers.TryGetValue(lessonId, out var users))
                {
                    users.Remove(Context.ConnectionId);
                    if (users.Count == 0)
                    {
                        lessonUsers.TryRemove(lessonId, out _);
                    }
                }

                await Clients.Group(lessonId).SendAsync("UserLeft", Context.ConnectionId);

                var studentLesson = _context.StudentLessons.FirstOrDefault(x => x.LessonId == int.Parse(lessonId) && x.ConnectionId == Context.ConnectionId);
                if (studentLesson != null)
                {
                    var student = _context.Students.FirstOrDefault(x => x.Id == studentLesson.StudentId);

                    // Kullanıcının ders odasına katıldığını bildiriyoruz
                    await SendMessage(lessonId, "System", $"{Context.ConnectionId} - {student.Name} {student.Surname} left the lesson.");
                }

                await SendMessage(lessonId, "System", $"{Context.ConnectionId} left the lesson.");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}