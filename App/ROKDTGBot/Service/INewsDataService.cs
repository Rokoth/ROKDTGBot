using ROTGBot.Contract.Model;

namespace ROTGBot.Service
{
    public interface INewsDataService
    {
        Task AddNewMessageForNews(long messageId, Guid userNewsId, string text, CancellationToken cancellationToken);
        Task CreateNews(long chatId, Guid id, long? groupId, long? threadId, string type, string title, CancellationToken token);
        Task<News?> GetCurrentNews(Guid userId, CancellationToken cancellationToken);
        Task<string> GetModeratorReport(Guid id, CancellationToken token);
        Task<News?> GetNewsById(Guid id, CancellationToken token);
        Task<List<News>> GetNewsForApprove(CancellationToken token);
        Task<List<NewsMessage>> GetNewsMessages(Guid newsId, CancellationToken token);
        Task<string> GetUserReport(Guid id, CancellationToken token);
        Task SetNewsAccepted(Guid id, CancellationToken token);
        Task SetNewsApproved(Guid id, Guid moderatorId, CancellationToken token);
        Task SetNewsDeclined(Guid id, Guid moderatorId, CancellationToken token);
        Task SetNewsDeleted(Guid id, CancellationToken token);
    }
}