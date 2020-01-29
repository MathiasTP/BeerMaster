using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Java.Lang.Reflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSubstitute;
using NUnit.Framework;
using Org.Apache.Http.IO;
using BeerBong;
using BeerBong.Models;
using BeerBong.ViewModel;
using BeerBong.Views;
using System.Net.Http;
using System.Threading;
using BeerBong.Services;
using Moq.Protected;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BeerMasterTestProject
{
    [TestClass]
    public class BeerMasterTest
    {
        [TestMethod]
        public void TestOpretBrugerViewModelPasswordCheck()
        {

            // Arrange
            var opretBrugerViewModel = new OpretBrugerViewModel();

            //act

            opretBrugerViewModel.OnClickOpretBruger.Execute(null);

            //Asserts

            Assert.IsTrue(opretBrugerViewModel.PasswordCheck());
            
           
        }

        [Fact]
        public async Task TestLoginBrugerSuccesApiKald()
        {
            //Arrange
            var service = Substitute.For<IRestService>();

            LoginUser user = new LoginUser()
            {
                passWord = "zpw63mth",
                userName = "test2"
            };


            // act
            service.GetLoginDataAsync(user).Returns(Task.FromResult(true));
            var target = new RestService();
            var id = await target.GetLoginDataAsync(user);


            //Assert
            Assert.IsTrue(id);
        }

        [Fact]
        public async Task TestOpretBrugerrSuccesApiKald()
        {
            //Arrange
            var service = Substitute.For<IRestService>();

            RegisterUser user = new RegisterUser()
            {
                passWord = "zpw63mth",
                userName = "test22222222"
            };


            // act
            service.SaveOpretBrugerAsync(user).Returns(Task.FromResult(true));
            var target = new RestService();
            var id = await target.SaveOpretBrugerAsync(user);


            //Assert
            Assert.IsTrue(id);
        }

        [Fact]
        public async Task TestLeaderboardApi()
        {
            //Arrange
            var service = Substitute.For<IRestService>();
            List<OnlineLeaderboard> leaderboard = new List<OnlineLeaderboard>(); 


            // act
            service.RefreshDataAsync().Returns(Task.FromResult(leaderboard));
            var target = new RestService();
            var id = await target.RefreshDataAsync();


            //Assert
            Assert.IsNotNull(id);
        }

        [Fact]
        public async Task TestCreateGameApiKald()
        {
            //Arrange
            var service = Substitute.For<IRestService>();

            Game game = new Game()
            {
                gameId = 171,
                players = null
            };
            LoginUser user = new LoginUser()
            {
                passWord = "zpw63mth",
                userName = "test2"
            };

            // act
            service.CreateGame(game).Returns(Task.FromResult(game));
            var target = new RestService();
            await target.GetLoginDataAsync(user);
            var id = await target.CreateGame(game);


            //Assert
            Assert.AreEqual(game.gameId, id.gameId);
        }

        [Fact]
        public async Task TestGetGameResultApiKald()
        {
            //Arrange
            var service = Substitute.For<IRestService>();

            List<GameResult> gameresult = new List<GameResult>();
            GameResult game = new GameResult()
            {
                dateTime = DateTime.Now,
                time = 20,
                playerid = 65
            };
            gameresult.Add(game);

            LoginUser user = new LoginUser()
            {
                passWord = "zpw63mth",
                userName = "test2"
            };

            // act
            service.GetGameResult(80).Returns(Task.FromResult(gameresult));
            var target = new RestService();
            await target.GetLoginDataAsync(user);
            var id = await target.GetGameResult(80);


            //Assert
            Assert.AreEqual(gameresult[0].playerid, id[0].playerid);
        }
    }
}
