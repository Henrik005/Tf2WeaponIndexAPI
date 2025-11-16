using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tf2WeaponIndexAPI.Models;
using Tf2WeaponIndexAPI.Services;

namespace Tf2WeaponIndexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponsController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly DataMapper _dataMapper;

        public WeaponsController(MongoDBService mongoDBService, DataMapper dataMapper)
        {
            _mongoDBService = mongoDBService;
            _dataMapper = dataMapper;
        }

        [HttpGet("getAllWeapons")]
        public IActionResult Get()
        {
            var items = _dataMapper.TfItem.Values.ToList();
            var formattedItems = _dataMapper.FormatItem(items);

            var attributes = _dataMapper.attributeDefs.Values.ToList();
            var formattedAttributes = new List<string>();

            foreach (var item in items)
            {
                if (item.attributes != null)
                {
                    foreach (var attribute in item.attributes)
                    {
                        var attributeDef = attributes.FirstOrDefault(attrDef => attrDef.Name == attribute.Name);
                        if (attributeDef != null)
                        {
                            var formattedAttribute = _dataMapper.FormatAttribute(attribute, attributeDef);
                            formattedAttributes.Add(formattedAttribute);
                        }
                    }
                }
            }

            if (formattedAttributes.Count > 0 || items.Count > 0)
            {
                return Ok(new
                {
                    formattedItems,
                    formattedAttributes
                });
            }

            return NotFound("No weapons found.");
        }
    }
}
