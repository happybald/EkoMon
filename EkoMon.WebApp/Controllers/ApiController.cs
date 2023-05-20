using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using EkoMon.DomainModel.Services;
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
        private readonly AirPollutionIndicator airPollutionIndicator;
        private readonly WaterPollutionIndicator waterPollutionIndicator;
        private readonly EarthPollutionIndicator earthPollutionIndicator;
        private readonly RadiationPollutionIndicator radiationPollutionIndicator;
        private readonly TrashPollutionIndicator trashPollutionIndicator;
        public ApiController(EntityContext context, AirPollutionIndicator airPollutionIndicator, WaterPollutionIndicator waterPollutionIndicator, EarthPollutionIndicator earthPollutionIndicator, RadiationPollutionIndicator radiationPollutionIndicator, TrashPollutionIndicator trashPollutionIndicator)
        {
            this.context = context;
            this.airPollutionIndicator = airPollutionIndicator;
            this.waterPollutionIndicator = waterPollutionIndicator;
            this.earthPollutionIndicator = earthPollutionIndicator;
            this.radiationPollutionIndicator = radiationPollutionIndicator;
            this.trashPollutionIndicator = trashPollutionIndicator;
        }

        [HttpGet]
        public async Task<ActionResult> Dev()
        {
            var airParameters = context.Parameters.Where(i => i.CategoryId == 1).ToList();
            var waterParameters = context.Parameters.Where(i => i.CategoryId == 2).ToList();
            var earthParameters = context.Parameters.Where(i => i.CategoryId == 3).ToList();
            var radiationParameter = context.Parameters.First(i => i.CategoryId == 4);
            var trashParameters = context.Parameters.Where(i => i.CategoryId == 5).ToList();
            var location = new Location("ПРАТ «ММК ім. Ілліча»", 47.14034067673211, 37.59423930750801, 1000 )
            {
                Address = "Донецька обл., 87504, м. Маріуполь. вул. Левченка ,1",
                LocationParameters = new List<LocationParameter>()
            };
            location.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.0));
            location.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.3, DateTime.Now.AddDays(-3)));
            location.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.4, DateTime.Now.AddDays(-5)));
            location.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.3, DateTime.Now.AddDays(-7)));
            location.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.2, DateTime.Now.AddDays(-9)));
            location.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1.6));
            location.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.5));
            location.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 1.7));
            location.LocationParameters.Add(new LocationParameter(radiationParameter, 0.6));
            
            var location1 = new Location("ПРАТ \"АКХЗ\"", 48.166174810099854, 37.70565747262548, 100)
            {
                Address = "Індустріальний просп., 1, Авдіївка, Донецька область",
                LocationParameters = new List<LocationParameter>()
            };
            location1.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 0.8));
            location1.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1.1));
            location1.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.05));
            location1.LocationParameters.Add(new LocationParameter(radiationParameter, 0.4));
            location1.LocationParameters.AddRange(GenerateTrash(trashParameters, location1.Area, 1));
            
            var location2 = new Location("ПРАТ \"НОВОТРОЇЦЬКЕ РУ\"", 47.71482350583813, 37.55547806345076, 1000)
            {
                Address = "вул. Совєтська, Новотроїцьке, Донецька область",
                LocationParameters = new List<LocationParameter>()
            };
            location2.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1));
            location2.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 0.93));
            location2.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.5));
            location2.LocationParameters.Add(new LocationParameter(radiationParameter, 0.1));
            location2.LocationParameters.AddRange(GenerateTrash(trashParameters, location2.Area, 0.5));


            context.Add(location);
            context.Add(location1);
            context.Add(location2);

            await context.SaveChangesAsync();
            return Ok();

            IEnumerable<LocationParameter> GenerateValuesByIndex(List<Parameter> initial, double desiredIndex, DateTime? date = null)
            {
                date ??= DateTime.UtcNow;
                var locationParameters = new List<LocationParameter>();
                var oneParam = desiredIndex / initial.Count;
                foreach (var param in initial)
                    locationParameters.Add(new LocationParameter(param, Math.Round(oneParam * param.Limit!.Value, 6)) { DateTime = date.Value });

                return locationParameters;
            }
            
            IEnumerable<LocationParameter> GenerateTrash(List<Parameter> initial, int area, double desiredIndex, DateTime? date = null)
            {
                date ??= DateTime.UtcNow;
                var locationParameters = new List<LocationParameter>();
                var oneParam = desiredIndex * area / initial.Count;
                foreach (var param in initial)
                    locationParameters.Add(new LocationParameter(param, Math.Round(oneParam / param.Koef!.Value, 6)) { DateTime = date.Value });

                return locationParameters;
            }

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

            var indicators = CalculateIndicators(location);

            return new LocationModel(location, indicators);
        }
        private List<IndicatorModel> CalculateIndicators(Location location)
        {

            var indicators = new List<IndicatorModel>();
            foreach (var categoryId in location.LocationParameters.Select(p => p.ParameterId).Distinct())
            {
                if (AirPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(airPollutionIndicator.Calculate(location.Id));
                if (WaterPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(waterPollutionIndicator.Calculate(location.Id));
                if (EarthPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(earthPollutionIndicator.Calculate(location.Id));
                if (RadiationPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(radiationPollutionIndicator.Calculate(location.Id));
                if (TrashPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(trashPollutionIndicator.Calculate(location.Id));
            }
            return indicators;
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
                dbLocation = new Location(location.Title, location.Latitude, location.Longitude, location.Area);

            dbLocation.Title = location.Title;
            dbLocation.Address = location.Address;
            dbLocation.Latitude = location.Latitude;
            dbLocation.Longitude = location.Longitude;

            if (dbLocation.Id == 0)
                context.Locations.Add(dbLocation);

            await context.SaveChangesAsync();
            dbLocation = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == dbLocation.Id);

            var indicators = CalculateIndicators(dbLocation);

            return new LocationModel(dbLocation!, indicators);
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
