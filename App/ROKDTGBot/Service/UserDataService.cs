using ROTGBot.Contract.Model;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using User = Telegram.BotAPI.AvailableTypes.User;

namespace ROTGBot.Service
{
    public class UserDataService(IRepository<Db.Model.User> userRepo,
        IRepository<Role> roleRepo,
        IRepository<UserRole> userRoleRepo) : IUserDataService
    {
        private readonly IRepository<Db.Model.User> _userRepo = userRepo;
        private readonly IRepository<Role> _roleRepo = roleRepo;
        private readonly IRepository<UserRole> _userRoleRepo = userRoleRepo;

        public async Task<Contract.Model.User> GetOrAddUser(User tguser, long chatId, CancellationToken cancellationToken)
        {
            var user = (await _userRepo.GetAsync(new Filter<Db.Model.User>()
            {
                Selector = s => s.TGId == tguser.Id
            }, cancellationToken)).FirstOrDefault();

            if (user == null)
            {
                user = await _userRepo.AddAsync(new Db.Model.User()
                {
                    Id = Guid.NewGuid(),
                    Description = $"{tguser.FirstName} {tguser.LastName} (@{tguser.Username})",
                    IsDeleted = false,
                    Name = $"{tguser.FirstName} {tguser.LastName} (@{tguser.Username})",
                    TGLogin = tguser.Username,
                    TGId = tguser.Id,
                    ChatId = chatId
                }, true, cancellationToken);

                var userRole = (await _roleRepo.GetAsync(new Filter<Role>() { Selector = s => s.Name == "user" }, cancellationToken)).First();

                await _userRoleRepo.AddAsync(new UserRole()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    RoleId = userRole.Id,
                    UserId = user.Id
                }, true, cancellationToken);
            }
            else
            {
                user.ChatId = chatId;
                await _userRepo.UpdateAsync(user, true, cancellationToken);
            }
            return await Map(user, cancellationToken);
        }

        private async Task<Contract.Model.User> Map(Db.Model.User user, CancellationToken cancellationToken)
        {
            var roles = (await GetUserRoles(user.Id, cancellationToken)).Select(s => Enum.Parse<RoleEnum>(s))?.ToList() ?? [RoleEnum.user];
            return new Contract.Model.User()
            {
                ChatId = user.ChatId,
                Description = user.Description,
                Id = user.Id,
                IsNotify = user.IsNotify,
                Name = user.Name,
                Roles = roles,
                TGId = user.TGId,
                TGLogin = user.TGLogin
            };
        }

        private async Task<string[]> GetUserRoles(Guid userId, CancellationToken token)
        {
            string[] roles = [];
            var userRoles = (await _userRoleRepo.GetAsync(new Filter<UserRole>() { Selector = s => s.UserId == userId }, token)).Select(s => s.RoleId).Distinct().ToArray();
            if (userRoles.Length != 0)
            {
                roles = [.. (await _roleRepo.GetAsync(new Filter<Role>() { Selector = s => userRoles.Contains(s.Id) }, token)).Select(s => s.Name)];
            }

            return roles;
        }

        public async Task<IEnumerable<Contract.Model.User>> GetNotifyModerators(CancellationToken token)
        {
            var result = await _userRepo.GetAsync(new Filter<Db.Model.User>()
            {
                Selector = s => !s.IsDeleted && s.IsNotify
            }, token);

            var users = new List<Contract.Model.User>();

            foreach(var res in result)
            {
                users.Add(await Map(res, token));
            }

            return users.Where(s => s.IsModerator);
        }

        public async Task SetRole(string login, RoleEnum role, CancellationToken token)
        {
            var user = (await _userRepo.GetAsync(new Filter<Db.Model.User>()
            {
                Selector = s => s.TGLogin != null && s.TGLogin == login
            }, token)).FirstOrDefault();

            if (user != null)
            {
                var newRole = (await _roleRepo.GetAsync(new Filter<Role>() { Selector = s => s.Name == Enum.GetName(typeof(RoleEnum), role) }, token)).First();

                await _userRoleRepo.AddAsync(new UserRole()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    RoleId = newRole.Id,
                    UserId = user.Id
                }, true, token);
            }
        }

        public async Task<bool> SwitchUserNotify(Guid userId, CancellationToken token)
        {
            var user = await _userRepo.GetAsync(userId, token);
            user.IsNotify = !user.IsNotify;
            await _userRepo.UpdateAsync(user, true, token);
            return user.IsNotify;
        }
    }
}
