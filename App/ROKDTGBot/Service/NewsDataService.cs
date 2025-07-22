using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ROTGBot.Service
{
    public class NewsDataService(
        IRepository<News> newsRepo,
        IRepository<NewsMessage> newsMessageRepo) : INewsDataService
    {
        private readonly IRepository<News> _newsRepo = newsRepo;
        private readonly IRepository<NewsMessage> _newsMessageRepo = newsMessageRepo;

        public async Task AddNewMessageForNews(long messageId, Guid userNewsId, string text, CancellationToken cancellationToken)
        {
            await _newsMessageRepo.AddAsync(new NewsMessage()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                NewsId = userNewsId,
                TGMessageId = messageId,
                TextValue = text
            }, true, cancellationToken);
        }

        public async Task<Contract.Model.News?> GetCurrentNews(Guid userId, CancellationToken cancellationToken)
        {
            var result =  (await _newsRepo.GetAsync(new Filter<News>()
            {
                Selector = s => s.UserId == userId
            }, cancellationToken)).FirstOrDefault(s => s.State == "create");
            return Map(result);
        }

        private static Contract.Model.News? Map(News? result)
        {
            if (result == null) return null;

            return new Contract.Model.News()
            {
                ChatId = result.ChatId,
                CreatedDate = result.CreatedDate,
                Description = result.Description,
                GroupId = result.GroupId,
                Id = result.Id,
                State = result.State,
                ThreadId = result.ThreadId,
                Title = result.Title,
                Type = result.Type,
                UserId = result.UserId
            };
        }

        private static Contract.Model.NewsMessage? Map(NewsMessage? result)
        {
            if (result == null) return null;

            return new Contract.Model.NewsMessage()
            {
                Id = result.Id,
                NewsId = result.NewsId,
                TextValue = result.TextValue,
                TGMessageId = result.TGMessageId
            };
        }

        public async Task<Contract.Model.News?> GetNewsById(Guid id, CancellationToken token)
        {
            return Map(await _newsRepo.GetAsync(id, token));
        }

        public async Task<List<Contract.Model.News>> GetNewsForApprove(CancellationToken token)
        {
            return Map((await _newsRepo.GetAsync(new Filter<News>()
            {
                Selector = s => s.State == "accepted" && s.Type == "news"
            }, token)).OrderBy(s => s.CreatedDate));
        }

        private static List<Contract.Model.News> Map(IEnumerable<Db.Model.News> news)
        {
            List<Contract.Model.News> result = [];
            foreach(var item in news)
            {
                var map = Map(item);
                if (map != null)
                    result.Add(map);
            }
            return result;
        }

        private static List<Contract.Model.NewsMessage> Map(IEnumerable<Db.Model.NewsMessage> news)
        {
            List<Contract.Model.NewsMessage> result = [];
            foreach (var item in news)
            {
                var map = Map(item);
                if (map != null)
                    result.Add(map);
            }
            return result;
        }

        public async Task<List<Contract.Model.NewsMessage>> GetNewsMessages(Guid newsId, CancellationToken token)
        {
            return Map((await _newsMessageRepo.GetAsync(new Filter<NewsMessage>()
            {
                Selector = s => s.NewsId == newsId
            }, token)));
        }

        public async Task SetNewsAccepted(Guid id, CancellationToken token)
        {
            await SetNewsStatus(id, null, "accepted", false, token);
        }

        private async Task SetNewsStatus(Guid id, Guid? moderatorId, string state, bool toDelete, CancellationToken token)
        {
            var userNews = await _newsRepo.GetAsync(id, token);
            userNews.State = state;
            if (moderatorId.HasValue) userNews.ModeratorId = moderatorId;
            if (toDelete) userNews.IsDeleted = true;
            await _newsRepo.UpdateAsync(userNews, true, token);
        }

        public async Task SetNewsApproved(Guid id, Guid moderatorId, CancellationToken token)
        {
            await SetNewsStatus(id, moderatorId, "approved", false, token);            
        }

        public async Task SetNewsDeclined(Guid id, Guid moderatorId, CancellationToken token)
        {
            await SetNewsStatus(id, moderatorId, "declined", false, token);            
        }

        public async Task SetNewsDeleted(Guid id, CancellationToken token)
        {
            await SetNewsStatus(id, null, "deleted", true, token);           
        }

        public Task CreateNews(long chatId, Guid userId, long? groupId, long? threadId, string type, string title, CancellationToken token)
        {
            return _newsRepo.AddAsync(new News()
            {
                IsDeleted = false,
                Id = Guid.NewGuid(),
                UserId = userId,
                State = "create",
                Title = title,
                ChatId = chatId,
                Description = title,
                Type = type,
                GroupId = groupId,
                ThreadId = threadId,
                CreatedDate = DateTime.Now
            }, true, token);
        }

        public async Task<string> GetUserReport(Guid userId, CancellationToken token)
        {
            string result = string.Empty;

            var allNews = (await _newsRepo.GetAsync(new Filter<News>() { 
                Selector = s => s.UserId == userId
            }, token)).OrderBy(s => s.CreatedDate);

            foreach(var byYear in allNews.GroupBy(s => s.CreatedDate.Year))
            {
                result += $"{byYear.Key} год:\r\n";

                foreach (var byMonth in allNews.GroupBy(s => s.CreatedDate.Month))
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(byMonth.Key);

                    result += $"{monthName}: отправлено {byMonth.Count()}," +
                        $" подтверждено: {byMonth.Count(s => s.State == "approved")}, " +
                        $"отклонено: {byMonth.Count(s => s.State == "declined")} обращений;";    
                }
            }

            result += $"\r\n\r\nВсего: отправлено {allNews.Count()}, " +
                $"принято: {allNews.Count(s => s.State == "approved")}, " +
                $"отклонено: {allNews.Count(s => s.State == "declined")}, " +
                $"в очереди на подтверждение: {allNews.Count(s => s.State == "approved")} обращений.";

            return result;
        }

        public async Task<string> GetModeratorReport(Guid userId, CancellationToken token)
        {
            string result = string.Empty;

            var allNews = (await _newsRepo.GetAsync(new Filter<News>()
            {
                Selector = s => s.ModeratorId == userId
            }, token)).OrderBy(s => s.CreatedDate);

            foreach (var byYear in allNews.GroupBy(s => s.CreatedDate.Year))
            {
                result += $"{byYear.Key} год:\r\n";

                foreach (var byMonth in allNews.GroupBy(s => s.CreatedDate.Month))
                {
                    result += $"{byMonth.Key} месяц: всего {byMonth.Count()}," +
                        $" подтверждено: {byMonth.Count(s => s.State == "approved")}, " +
                        $"отклонено: {byMonth.Count(s => s.State == "declined")} обращений;";
                }
            }

            result += $"\r\n\r\nВсего: отправлено {allNews.Count()}, " +
                $"подтверждено: {allNews.Count(s => s.State == "approved")}, " +
                $"отклонено: {allNews.Count(s => s.State == "declined")} обращений.";

            return result;
        }
    }
}
