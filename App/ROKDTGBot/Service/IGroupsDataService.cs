
namespace ROTGBot.Service
{
    public interface IGroupsDataService
    {
        Task<bool> AddGroupIfNotExists(long chatId, string? title, string description, CancellationToken token);
    }
}