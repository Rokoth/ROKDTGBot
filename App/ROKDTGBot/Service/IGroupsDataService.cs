
namespace ROTGBot.Service
{
    public interface IGroupsDataService
    {
        Task AddGroupIfNotExists(long chatId, string? title, string description, CancellationToken token);
    }
}