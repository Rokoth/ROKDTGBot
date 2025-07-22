using ROTGBot.Contract.Model;
using Telegram.BotAPI.AvailableTypes;

namespace ROTGBot.Service
{
    public interface IButtonsDataService
    {
        Task AddNewButton(Message message, CancellationToken cancellationToken);
        Task<List<NewsButton>> GetActiveButtons(CancellationToken token);
        Task<List<NewsButton>> GetAllButtons(CancellationToken token);
        Task<NewsButton?> GetButtonByNumber(int buttonNumber, CancellationToken token);
        Task<NewsButton?> GetButtonByThreadId(long? groupId, long? threadId, CancellationToken token);
        Task RemoveButtonSend(Guid id, CancellationToken token);
        Task SetButtonSend(Guid id, string? name, CancellationToken token);
    }
}