using Microsoft.Extensions.Configuration;
using Moq;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using ROTGBot.Service;

namespace UnitTests
{
    public class GroupsDataServiceUnitTests
    {
        private IConfiguration configuration;

        public GroupsDataServiceUnitTests()
        {
            ConfigurationBuilder builder = new();
            builder.AddJsonFile("appsettings.json");
            configuration = builder.Build();
        }

        /// <summary>
        /// 0.0.5.2.1
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddGroupIfNotExists_GroupExists_Success_Async()
        {
            var _repoMock = new Mock<IRepository<Groups>>();

            _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<Groups>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<Groups>()
                {
                    new()
                    {
                        ChatId = 1,
                        Description = "test",
                        Id = new Guid(),
                        IsDeleted = false,
                        SendNews = true,
                        ThreadId = 1,
                        Title = "test",
                    }
                }));
            _repoMock.Setup(s => s.AddAsync(It.IsAny<Groups>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Groups()));

            var service = new GroupsDataService(_repoMock.Object);

            var result = await service.AddGroupIfNotExists(1, "test", "test", new CancellationToken());

            Assert.True(result);
        }

        //[Fact]
        //public async Task AddGroupIfNotExists_GroupNotExists_Success_Async()
        //{
        //    var _repoMock = new Mock<IRepository<Groups>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<Groups>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<Groups>()));
        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<Groups>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new Groups()));

        //    var service = new GroupsDataService(_repoMock.Object);

        //    var result = await service.AddGroupIfNotExists(1, "test", "test", new CancellationToken());

        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task AddGroupIfNotExists_Error_TitleIsNull_Async()
        //{
        //    var _repoMock = new Mock<IRepository<Groups>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<Groups>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<Groups>()));
        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<Groups>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new Groups()));

        //    var service = new GroupsDataService(_repoMock.Object);

        //    await Assert.ThrowsAsync<ArgumentException>(() => service.AddGroupIfNotExists(1, null, "test", new CancellationToken()));
        //}
    }
}