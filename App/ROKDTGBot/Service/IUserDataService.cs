using ROTGBot.Contract.Model;

namespace ROTGBot.Service
{
    public interface IUserDataService
    {
        Task<IEnumerable<Contract.Model.User>> GetNotifyModerators(CancellationToken token);
        Task<Contract.Model.User> GetOrAddUser(Telegram.BotAPI.AvailableTypes.User tguser, long chatId, CancellationToken cancellationToken);
        Task SetRole(string login, RoleEnum role, CancellationToken token);
        Task<bool> SwitchUserNotify(Guid userId, CancellationToken token);
    }
}