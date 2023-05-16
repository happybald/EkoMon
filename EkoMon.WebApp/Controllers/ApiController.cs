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
            var parameters = context.Parameters.Where(t=>t.Type == ParameterType.Measurable).Include(u => u.Unit).Select(o => new
            {
                Id = o.Id,
                Name = o.Title,
                Unit = o.Unit.Title
            });
            var result1 = System.Text.Json.JsonSerializer.Serialize(parameters);
            
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationShortModel>>> GetLocations()
        {
            return await context.Locations.Select(o => new LocationShortModel(o)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LocationModel>> GetLocation(int id)
        {
            var location = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            return new LocationModel(location);
        }

        [HttpPost]
        public async Task<ActionResult<LocationModel>> UpdateLocation(LocationModel location)
        {
            var dbLocation = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == location.Id);
            if (dbLocation == null)
                return NotFound();

            dbLocation.Title = location.Title;
            dbLocation.Address = location.Address;
            dbLocation.Latitude = location.Latitude;
            dbLocation.Longitude = location.Longitude;

            var parametersToRemove = dbLocation.LocationParameters
                .Where(db => location.LocationParameters.All(lp2 => lp2.Id != db.Id))
                .ToList();

            foreach (var parameter in parametersToRemove)
            {
                dbLocation.LocationParameters.Remove(parameter);
                context.LocationParameters.Remove(parameter);
            }

            // Update existing objects and add new objects
            foreach (var locationParameterModel in location.LocationParameters)
            {
                if (locationParameterModel.Id == 0)
                {
                    var dbLocationParameter = new LocationParameter();
                    dbLocationParameter.Value = locationParameterModel.Value;
                    var parameterModel = locationParameterModel.Parameter;
                    if (parameterModel.Id == 0)
                    {
                        var dbParameter = new Parameter(parameterModel.Title);
                        var unitModel = parameterModel.Unit;
                        if (unitModel != null)
                        {
                            if (unitModel.Id == 0)
                            {
                                var dbUnit = new Unit(unitModel.Title);
                                dbParameter.Unit = dbUnit;
                            }
                            else
                            {
                                dbParameter.UnitId = unitModel.Id;
                            }
                        }
                        dbLocationParameter.Parameter = dbParameter;
                    }
                    else
                    {
                        dbLocationParameter.ParameterId = parameterModel.Id;
                    }
                    dbLocation.LocationParameters.Add(dbLocationParameter);
                }
                else
                {
                    var dbLocationParameter = dbLocation.LocationParameters.First(i => i.Id == locationParameterModel.Id);
                    dbLocationParameter.Value = locationParameterModel.Value;
                    var parameterModel = locationParameterModel.Parameter;
                    if (parameterModel.Id == 0)
                    {
                        var dbParameter = new Parameter(parameterModel.Title);
                        var unitModel = parameterModel.Unit;
                        if (unitModel != null)
                        {
                            if (unitModel.Id == 0)
                            {
                                var dbUnit = new Unit(unitModel.Title);
                                dbParameter.Unit = dbUnit;
                            }
                            else
                            {
                                dbParameter.UnitId = unitModel.Id;
                            }
                        }
                        dbLocationParameter.Parameter = dbParameter;
                    }
                    else
                    {
                        dbLocationParameter.ParameterId = parameterModel.Id;
                    }
                }
            }

            await context.SaveChangesAsync();
            dbLocation = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstAsync(i => i.Id == location.Id);
            return new LocationModel(dbLocation);
        }

        [HttpPost]
        public async Task<ActionResult<LocationShortModel>> PostLocation(LocationShortModel location)
        {
            var dbLocation = new Location(location.Title, location.Latitude, location.Longitude);
            context.Locations.Add(dbLocation);
            await context.SaveChangesAsync();

            return new LocationShortModel(dbLocation);
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
