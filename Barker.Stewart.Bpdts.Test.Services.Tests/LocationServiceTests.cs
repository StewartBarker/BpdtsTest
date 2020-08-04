using Barker.Stewart.Bpdts.Test.Models;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Barker.Stewart.Bpdts.Test.Services.Tests
{
    public class LocationServiceTests
    {
        public class IsWithinMilesOfLocation
        {
            private readonly ILocationService locationService = new LocationService();

            [Test]
            public void ReturnTrue_WhenTargetLocationIsWithinRadiousOfSourceLocation()
            {
                // Arrange
                var london = new Location { Latitude = 51.509865, Longitude = -0.118092 };
                var preston = new Location { Latitude = 53.765762, Longitude = -2.692337 };

                // Act
                var result = locationService.IsWithinMilesOfLocation(london, 500, preston);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void ReturnFalse_WhenTargetLocationIsNotWithinRadiousOfSourceLocation()
            {
                // Arrange
                var london = new Location { Latitude = 51.509865, Longitude = -0.118092 };
                var preston = new Location { Latitude = 53.765762, Longitude = -2.692337 };

                // Act
                var result = locationService.IsWithinMilesOfLocation(london, 100, preston);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void ThrowsArgumentNullException_WhenSourceLocationIsNull()
            {
                // Arrange
                Location london = null;
                var preston = new Location { Latitude = 53.765762, Longitude = -2.692337 };

                // Act
                // Assert
                Assert.Throws<ArgumentNullException>(() => locationService.IsWithinMilesOfLocation(london, 500, preston));
            }

            [Test]
            public void ThrowsArgumentNullException_WhenTargetLocationIsNull()
            {
                // Arrange
                var london = new Location { Latitude = 51.509865, Longitude = -0.118092 };
                Location preston = null;

                // Act
                // Assert
                Assert.Throws<ArgumentNullException>(() => locationService.IsWithinMilesOfLocation(london, 500, preston));
            }
        }
    }
}