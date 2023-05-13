using EkoMon.DomainModel.Db;
using EkoMon.WebApp.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return await context.Parameters.Include(u=>u.Unit).Select(p => new ParameterModel(p)).ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<List<UnitModel>>> GetUnits()
        {
            return await context.Units.Select(p => new UnitModel(p)).ToListAsync();
        }
    }
}
