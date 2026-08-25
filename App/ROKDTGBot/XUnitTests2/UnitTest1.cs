using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Moq;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using ROTGBot.Service;

namespace XUnitTests2
{
    public class ButtonsDataServiceUnitTests
    {
        private IConfiguration configuration;

        public ButtonsDataServiceUnitTests()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            configuration = builder.Build();
        }

        //[Fact]
        //public async Task AddNewButton_NoButtons_Async()
        //{
        //    var _repoMock = new Mock<IRepository<NewsButton>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<NewsButton>()));

        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new NewsButton()));

        //    var buttonsService = new ButtonsDataService(_repoMock.Object);

        //    var result = await buttonsService.AddNewButton(1, 1, "chat", "chat", new CancellationToken());

        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task AddNewButton_ButtonExists_Async()
        //{
        //    var _repoMock = new Mock<IRepository<NewsButton>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<NewsButton>()
        //        {
        //            new NewsButton()
        //            {
        //                ButtonName = "chat",
        //                ButtonNumber = 1,
        //                ChatId = 1,
        //                ChatName = "chat",
        //                Id = Guid.NewGuid(),
        //                IsDeleted = false,
        //                ThreadId = 1,
        //                ThreadName = "chat",
        //                ToSend = true
        //            }
        //        }));

        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new NewsButton()));

        //    var buttonsService = new ButtonsDataService(_repoMock.Object);

        //    var result = await buttonsService.AddNewButton(1, 1, "chat", "chat", new CancellationToken());

        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task AddNewButton_ChatButtonIsNull_Error_Async()
        //{
        //    var _repoMock = new Mock<IRepository<NewsButton>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<NewsButton>()
        //        {
        //            new()
        //            {
        //                ButtonName = "chat",
        //                ButtonNumber = 1,
        //                ChatId = 1,
        //                ChatName = "chat",
        //                Id = Guid.NewGuid(),
        //                IsDeleted = false,
        //                ThreadId = 1,
        //                ThreadName = "chat",
        //                ToSend = true
        //            }
        //        }));

        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new NewsButton()));

        //    var buttonsService = new ButtonsDataService(_repoMock.Object);

        //    await Assert.ThrowsAsync<ArgumentException>(() => buttonsService.AddNewButton(1, 1, null, "chat", new CancellationToken()));
        //}

        ///// <summary>
        ///// 0.0.14.2.4
        ///// </summary>
        ///// <returns></returns>
        //[Fact]
        //public async Task AddParentButton_Success_Async()
        //{
        //    var _repoMock = new Mock<IRepository<NewsButton>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new List<NewsButton>()));

        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new NewsButton()));

        //    var buttonsService = new ButtonsDataService(_repoMock.Object);

        //    var result = await buttonsService.AddParentButton("chat", null, new CancellationToken());

        //    Assert.True(result);
        //}

        /// <summary>
        /// 0.0.3.2.1
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllButtons_Success_Async()
        {
            var _repoMock = new Mock<IRepository<NewsButton>>();

            _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<NewsButton>()
                {
                    new()
                    {
                        ButtonName = "Button1",
                        ButtonNumber = 1,
                        ChatId = 1,
                        ChatName = "ChatName",
                        Id = Guid.NewGuid(),
                        IsDeleted = false,                      
                        ThreadId = 1,
                        ThreadName = "ThreadName",
                        ToSend = true
                    },
                    new()
                    {
                        ButtonName = "Button2",
                        ButtonNumber = 2,
                        ChatId = 1,
                        ChatName = "ChatName",
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        ThreadId = 2,
                        ThreadName = "ThreadName2",
                        ToSend = true
                    }
                }));

            var buttonsService = new ButtonsDataService(_repoMock.Object);

            var result = await buttonsService.GetAllButtons(new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        ///// <summary>
        ///// 0.0.14.2.2
        ///// </summary>
        ///// <returns></returns>
        //[Fact]
        //public async Task AddNewButton_RepoError_Async()
        //{
        //    var _repoMock = new Mock<IRepository<NewsButton>>();

        //    _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
        //        .Returns(GenerateRepoError);

        //    _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult(new NewsButton()));

        //    var buttonsService = new ButtonsDataService(_repoMock.Object);

        //    await Assert.ThrowsAsync<RepositoryException>(() => buttonsService.AddNewButton(1, 1, "chat", "chat", new CancellationToken()));
        //}

        private static Task<List<NewsButton>> GenerateRepoError()
        {
            throw new RepositoryException("some error");
        }
    }
}