using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject newcelestialObject)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (celestialObject == null) { return NotFound(); }
            celestialObject.Name = newcelestialObject.Name;
            celestialObject.OrbitalPeriod = newcelestialObject.OrbitalPeriod;
            celestialObject.OrbitedObjectId = newcelestialObject.OrbitedObjectId;

            _context.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (celestialObject == null) { return NotFound(); }
            celestialObject.Name = name;
            _context.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
           if (celestialObject == null) { return NotFound(); }
            var setellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
          
            var celestialObjects = new List<CelestialObject>();
            celestialObjects.Add(celestialObject);
            celestialObjects.AddRange(setellites);
            if (celestialObjects.Count == 0) { return NotFound(); }

            _context.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();

        }
    }
}
