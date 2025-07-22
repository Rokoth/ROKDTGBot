using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;

namespace ROTGBot.Service
{
    public class GroupsDataService(IRepository<Groups> groupsRepo) : IGroupsDataService
    {
        private readonly IRepository<Groups> _groupsRepo = groupsRepo;

        public async Task AddGroupIfNotExists(long chatId, string? title, string description, CancellationToken token)
        {
            var existsGroups = (await _groupsRepo.GetAsync(new Filter<Groups>()
            {
                Selector = s => s.ChatId == chatId
            }, token)).FirstOrDefault();

            if (existsGroups == null)
            {
                await _groupsRepo.AddAsync(new Groups()
                {
                    ChatId = chatId,
                    Description = description,
                    Title = title,
                    IsDeleted = false,
                    Id = Guid.NewGuid(),
                    SendNews = false
                }, true, token);
            }
        }
    }
}
