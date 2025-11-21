using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
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
        public IActionResult GetWeapons()
        {
            var items = _dataMapper.TfItem.Values.ToList();
            var attributeDefs = _dataMapper.attributeDefs.Values.ToList();

            var result = new List<object>();

            foreach (var item in items)
            {
                var formattedItem = _dataMapper.FormatItem(new List<Tf2Item> { item }).FirstOrDefault();
                var formattedAttributes = new List<string>();

                if (item.attributes != null)
                {
                    foreach (var attribute in item.attributes)
                    {
                        var matchingAttributeDef = attributeDefs.FirstOrDefault(attrDef => attrDef.Name == attribute.Name);
                        if (matchingAttributeDef != null)
                        {
                            var formattedAttribute = _dataMapper.FormatAttribute(attribute, matchingAttributeDef);
                            formattedAttributes.Add(formattedAttribute);
                        }
                    }
                }

                result.Add(new
                {
                    Item = formattedItem,
                    Attributes = formattedAttributes
                });
            }

            if (result.Count > 0)
            {
                return Ok(result);
            }

            return NotFound("No weapons found.");
        }

        [HttpGet("attributeTest")]
        public IActionResult attrTest()
        {
            foreach (var items in _dataMapper.TfItem.Values)
            {
                if (items.attributes != null)
                {
                    foreach (var attribute in items.attributes)
                    {
                        Console.WriteLine(attribute.Name);
                    }
                }
            }
            return Ok(_dataMapper.TfItem.Values);
        }
    }
}
