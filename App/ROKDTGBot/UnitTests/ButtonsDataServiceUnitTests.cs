using Microsoft.Extensions.Configuration;
using ROTGBot.Service;
using ROTGBot.Db.Interface;
using ROTGBot.Db.Model;
using Moq;

namespace XUnitTests
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

        /// <summary>
        /// 0.0.5.2.2
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddNewButton_NoButtons_Async()
        {
            var _repoMock = new Mock<IRepository<NewsButton>>();

            _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<NewsButton>()));

            _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new NewsButton()));

            var buttonsService = new ButtonsDataService(_repoMock.Object);

            var result = await buttonsService.AddNewButton(1, 1, "chat", "chat", new CancellationToken());

            Assert.True(result);
        }

        [Fact]
        public async Task AddNewButton_ButtonExists_Async()
        {
            var _repoMock = new Mock<IRepository<NewsButton>>();

            _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<NewsButton>()
                {
                    new NewsButton()
                    {
                        ButtonName = "chat",
                        ButtonNumber = 1,
                        ChatId = 1,
                        ChatName = "chat",
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        ThreadId = 1,
                        ThreadName = "chat",
                        ToSend = true
                    }
                }));

            _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new NewsButton()));

            var buttonsService = new ButtonsDataService(_repoMock.Object);

            var result = await buttonsService.AddNewButton(1, 1, "chat", "chat", new CancellationToken());

            Assert.False(result);
        }

        [Fact]
        public async Task AddNewButton_ChatButtonIsNull_Error_Async()
        {
            var _repoMock = new Mock<IRepository<NewsButton>>();

            _repoMock.Setup(s => s.GetAsync(It.IsAny<Filter<NewsButton>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<NewsButton>()
                {
                    new()
                    {
                        ButtonName = "chat",
                        ButtonNumber = 1,
                        ChatId = 1,
                        ChatName = "chat",
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        ThreadId = 1,
                        ThreadName = "chat",
                        ToSend = true
                    }
                }));

            _repoMock.Setup(s => s.AddAsync(It.IsAny<NewsButton>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new NewsButton()));

            var buttonsService = new ButtonsDataService(_repoMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => buttonsService.AddNewButton(1, 1, null, "chat", new CancellationToken()));
        }
    }
}