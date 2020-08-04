using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null) { return NotFound(); }
            var setellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            celestialObject.Satellites = setellites;
            return Ok(celestialObject);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name.Contains( name)).ToList();
            if (celestialObjects.Count == 0) { return NotFound(); }

            foreach (var item in celestialObjects)
            {
                var setellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
                item.Satellites = setellites;
            }
           
            return Ok(celestialObjects);

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var item in celestialObjects)
            {
                var setellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
                item.Satellites = setellites;
            }

            return Ok(celestialObjects);
        }
    }
}
