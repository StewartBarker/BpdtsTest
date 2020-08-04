using Barker.Stewart.Bpdts.Test.LocationApi.Bpdts;
using Barker.Stewart.Bpdts.Test.LocationApi.Controllers;
using Barker.Stewart.Bpdts.Test.Models;
using Barker.Stewart.Bpdts.Test.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barker.Stewart.Bpdts.Test.LocationApi.Tests.Controllers
{
    public class UserLocationControllerTests
    {
        public class Get
        {
            private readonly Location london = new Location { Latitude = 51.509865, Longitude = -0.118092 };
            private ILogger<UserLocationController> logger;
            private ILocationService locationService;
            private IBpdtsClient bpdtsClient;
            private UserLocationController userLocationController;

            [SetUp]
            public void Setup()
            {
                logger = Substitute.For<ILogger<UserLocationController>>();
                locationService = Substitute.For<ILocationService>();
                bpdtsClient = Substitute.For<IBpdtsClient>();
                userLocationController = new UserLocationController(logger, locationService, bpdtsClient);
            }

            [Test]
            public async Task ShouldReturnLondonUserOnly_WhenLocationServiceReturnsNoUsers()
            {
                var testUsersInLondon = "[{ \"longitude\": 24.12, \"latitude\": 12.1111 }]";
                var testAllUsers = "[{ \"longitude\": 24.12, \"latitude\": 12.1111 }, { \"longitude\": -0.118092, \"latitude\": 51.509865 }]";

                bpdtsClient.GetUsersInCity("London").Returns(Task.FromResult(testUsersInLondon));
                bpdtsClient.GetAllUsers().Returns(Task.FromResult(testAllUsers));

                // Act
                var result = await userLocationController.Get();

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okObjectResult = result.Result as OkObjectResult;
                var users = okObjectResult.Value as List<User>;
                Assert.AreEqual(1, users.Count);
                var user = users.First();
                Assert.AreEqual(12.1111, user.Latitude);
                Assert.AreEqual(24.12, user.Longitude);
            }

            [Test]
            public async Task ShouldReturnUsersWithin50MilesOfLondon_WhenNoUsersReturnedInLondon()
            {
                var testUsersInLondon = "[]";
                var testAllUsers = "[{ \"id\": 1, \"longitude\": 24.12, \"latitude\": 12.1111 }, { \"id\": 2, \"longitude\": 10, \"latitude\": 20 }]";
                var location = Substitute.For<ILocation>();
                location.Longitude.Returns(10);
                location.Latitude.Returns(20);
                    
                locationService.IsWithinMilesOfLocation(Arg.Any<ILocation>(), Arg.Any<int>(), Arg.Any<ILocation>()).Returns(true);

                bpdtsClient.GetUsersInCity("London").Returns(Task.FromResult(testUsersInLondon));
                bpdtsClient.GetAllUsers().Returns(Task.FromResult(testAllUsers));

                // Act
                var result = await userLocationController.Get();

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okObjectResult = result.Result as OkObjectResult;
                var users = okObjectResult.Value as List<User>;
                Assert.AreEqual(2, users.Count);
            }

            [Test]
            public async Task ShouldNotDuplicateResults_WhenUserIsInLondonAndAlsoWithin50MilesOfLondon()
            {
                var testUsersInLondon = "[{ \"id\": 1, \"longitude\": 24.12, \"latitude\": 12.1111 }]";
                var testAllUsers = "[{ \"id\": 1, \"longitude\": 24.12, \"latitude\": 12.1111 }, { \"id\": 2, \"longitude\": 10, \"latitude\": 20 }]";
                locationService.IsWithinMilesOfLocation(Arg.Any<ILocation>(), Arg.Any<int>(), Arg.Any<ILocation>()).ReturnsForAnyArgs(true);

                bpdtsClient.GetUsersInCity("London").Returns(Task.FromResult(testUsersInLondon));
                bpdtsClient.GetAllUsers().Returns(Task.FromResult(testAllUsers));

                // Act
                var result = await userLocationController.Get();

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okObjectResult = result.Result as OkObjectResult;
                var users = okObjectResult.Value as List<User>;
                Assert.AreEqual(2, users.Count);
            }
        }
    }
}
