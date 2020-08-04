namespace Barker.Stewart.Bpdts.Test.LocationApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Barker.Stewart.Bpdts.Test.LocationApi.Bpdts;
    using Barker.Stewart.Bpdts.Test.LocationApi.Helpers;
    using Barker.Stewart.Bpdts.Test.LocationApi.Json;
    using Barker.Stewart.Bpdts.Test.Models;
    using Barker.Stewart.Bpdts.Test.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class UserLocationController : ControllerBase
    {
        private readonly ILogger<UserLocationController> _logger;
        private readonly ILocationService _locationService;
        private readonly IBpdtsClient _bpdtsClient;

        public UserLocationController(ILogger<UserLocationController> logger, ILocationService locationService, IBpdtsClient bpdtsClient)
        {
            _logger = logger;
            _locationService = locationService;
            _bpdtsClient = bpdtsClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var usersLivingInLondon = this.DeserialiseUsers(await _bpdtsClient.GetUsersInCity("London"));

                var usersLivingWithin50MilesOfLondon = await this.GetUsersLivingWithin50MilesOfLondon();

                return Ok(usersLivingInLondon.Union(usersLivingWithin50MilesOfLondon, new UserDistinctComparer()).ToList());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        private async Task<IEnumerable<User>> GetUsersLivingWithin50MilesOfLondon()
        {
            ILocation london = new Location { Latitude = 51.509865, Longitude = -0.118092 };

            var allUsers = DeserialiseUsers(await _bpdtsClient.GetAllUsers());
            var milesRadius = 50;
            
            return allUsers.Where(a => _locationService.IsWithinMilesOfLocation(sourceLocation:a, miles:milesRadius, targetLocation:london));
        }

        private IEnumerable<User> DeserialiseUsers(string users)
        {
            var serializeOptions = new JsonSerializerOptions();

            serializeOptions.Converters.Add(new TextToDoubleConverter());

            return JsonSerializer.Deserialize<List<User>>(users, serializeOptions);
        }
    }
}