using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tf2WeaponIndexAPI.Services;

namespace Tf2WeaponIndexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponsController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public WeaponsController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
    }
}
