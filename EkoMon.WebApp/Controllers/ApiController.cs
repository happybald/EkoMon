using EkoMon.DomainModel.Db;
using EkoMon.WebApp.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace EkoMon.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        private readonly EntityContext context;
        public ApiController(EntityContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Dev()
        {
            var location = new Location("ПРАТ «ММК ім. Ілліча»", 47.14034067673211, 37.59423930750801)
            {
                Address = "Донецька обл., 87504, м. Маріуполь. вул. Левченка ,1",
                LocationParameters = new List<LocationParameter>()
                {
                    new LocationParameter(7, 0.001),
                    new LocationParameter(6, 0.02),
                    new LocationParameter(5, 0.05),
                    new LocationParameter(4, 2),
                    new LocationParameter(3, 0.02),
                    new LocationParameter(2, 0.03),
                    new LocationParameter(1, 0.5),
                    new LocationParameter(11, 2),
                    new LocationParameter(13, 150),
                    new LocationParameter(12, 50),
                    new LocationParameter(15, 0.5),
                    new LocationParameter(14, 0.2),
                    new LocationParameter(30, 5000),
                    new LocationParameter(31, 10000),
                    new LocationParameter(32, 5000),
                    new LocationParameter(33, 500),
                }
            };

            var location1 = new Location("ПРАТ \"АКХЗ\"", 48.166174810099854, 37.70565747262548)
            {
                Address = "Індустріальний просп., 1, Авдіївка, Донецька область",
                LocationParameters = new List<LocationParameter>()
                {
                    new LocationParameter(7, 0.02),
                    new LocationParameter(6, 0.05),
                    new LocationParameter(5, 0.03),
                    new LocationParameter(4, 2.5),
                    new LocationParameter(3, 0.02),
                    new LocationParameter(2, 0.1),
                    new LocationParameter(1, 0.1),
                    new LocationParameter(11, 2.5),
                    new LocationParameter(13, 150),
                    new LocationParameter(12, 50),
                    new LocationParameter(15, 0.5),
                    new LocationParameter(14, 0.2),
                    new LocationParameter(24, 343310000),
                }
            };
            var location2 = new Location("ПРАТ \"НОВОТРОЇЦЬКЕ РУ\"", 47.71482350583813, 37.55547806345076)
            {
                Address = "вул. Совєтська, Новотроїцьке, Донецька область",
                LocationParameters = new List<LocationParameter>()
                {
                    new LocationParameter(7, 0.001),
                    new LocationParameter(6, 0.02),
                    new LocationParameter(5, 0.03),
                    new LocationParameter(4, 2),
                    new LocationParameter(3, 0.001),
                    new LocationParameter(2, 0.2),
                    new LocationParameter(1, 0.1),
                    new LocationParameter(11, 2),
                    new LocationParameter(13, 100),
                    new LocationParameter(12, 50),
                    new LocationParameter(15, 0.5),
                    new LocationParameter(14, 0.3),
                    new LocationParameter(31, 500),
                    new LocationParameter(32, 100),
                    new LocationParameter(24, 29338000),
                }
            };

            context.Add(location);
            context.Add(location1);
            context.Add(location2);

            await context.SaveChangesAsync();

            var locations = context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).Select(o => new
            {
                Name = o.Title,
                Address = o.Address,
                Lat = o.Latitude,
                Lon = o.Longitude,
                Parameters = o.LocationParameters.Select(v => new
                {
                    ParameterId = v.ParameterId,
                    Value = v.Value,
                })
            });

            var result = System.Text.Json.JsonSerializer.Serialize(locations);
            var parameters = context.Parameters.Where(t => t.Type == ParameterType.Measurable).Include(u => u.Unit).Select(o => new
            {
                Id = o.Id,
                Name = o.Title,
                Unit = o.Unit.Title
            });
            var result1 = System.Text.Json.JsonSerializer.Serialize(parameters);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryModel>>> GetCategories()
        {
            return await context.Categories.Select(o => new CategoryModel(o)).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationShortModel>>> GetLocations(int? categoryId = null)
        {
            if (categoryId.HasValue)
                return await context.Locations.Where(lp => lp.LocationParameters.Any(p => p.Parameter.CategoryId == categoryId.Value)).Select(o => new LocationShortModel(o)).ToListAsync();
            return await context.Locations.Select(o => new LocationShortModel(o)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LocationModel>> GetLocation(int id)
        {
            var location = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == id);

            if (location == null)
                return NotFound();

            return new LocationModel(location);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LocationParameterModel>> GetLocationParameter(int id)
        {
            var locationParameter = await context.LocationParameters.Include(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == id);

            if (locationParameter == null)
                return NotFound();

            return new LocationParameterModel(locationParameter);
        }

        [HttpPost]
        public async Task<ActionResult<LocationParameterModel>> UpsertLocationParameter(LocationParameterModel locationParameterModel, int locationId)
        {
            var dbLocationParameter = await context.LocationParameters.Include(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == locationParameterModel.Id);
            if (dbLocationParameter == null)
                dbLocationParameter = new LocationParameter(locationParameterModel.Parameter.Id, locationParameterModel.Value);

            dbLocationParameter.LocationId = locationId;
            dbLocationParameter.DateTime = locationParameterModel.DateTime;
            dbLocationParameter.Value = locationParameterModel.Value;

            if (dbLocationParameter.Id == 0)
                context.LocationParameters.Add(dbLocationParameter);

            await context.SaveChangesAsync();
            dbLocationParameter = await context.LocationParameters.Include(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == dbLocationParameter.Id);
            return new LocationParameterModel(dbLocationParameter);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationParameter(int id)
        {
            var locationParameter = await context.LocationParameters.FindAsync(id);
            if (locationParameter == null)
            {
                return NotFound();
            }

            context.LocationParameters.Remove(locationParameter);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LocationModel>> UpsertLocation(LocationModel location)
        {
            var dbLocation = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == location.Id);
            if (dbLocation == null)
                dbLocation = new Location(location.Title, location.Latitude, location.Longitude);

            dbLocation.Title = location.Title;
            dbLocation.Address = location.Address;
            dbLocation.Latitude = location.Latitude;
            dbLocation.Longitude = location.Longitude;

            if (dbLocation.Id == 0)
                context.Locations.Add(dbLocation);

            await context.SaveChangesAsync();
            dbLocation = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == dbLocation.Id);
            return new LocationModel(dbLocation!);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            context.Locations.Remove(location);
            await context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<ParameterModel>>> GetParameters()
        {
            return await context.Parameters.Include(u => u.Unit).Select(p => new ParameterModel(p)).ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<List<UnitModel>>> GetUnits()
        {
            return await context.Units.Select(p => new UnitModel(p)).ToListAsync();
        }
    }
}
