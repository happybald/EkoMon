using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using EkoMon.DomainModel.Services;
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
        private readonly AirPollutionIndicator airPollutionIndicator;
        private readonly WaterPollutionIndicator waterPollutionIndicator;
        private readonly EarthPollutionIndicator earthPollutionIndicator;
        private readonly RadiationPollutionIndicator radiationPollutionIndicator;
        private readonly TrashPollutionIndicator trashPollutionIndicator;
        private readonly EconomyIndicator economyIndicator;
        private readonly HealthIndicator healthIndicator;
        private readonly EnergyIndicator energyIndicator;
        public ApiController(EntityContext context, AirPollutionIndicator airPollutionIndicator, WaterPollutionIndicator waterPollutionIndicator, EarthPollutionIndicator earthPollutionIndicator, RadiationPollutionIndicator radiationPollutionIndicator, TrashPollutionIndicator trashPollutionIndicator, EconomyIndicator economyIndicator, HealthIndicator healthIndicator, EnergyIndicator energyIndicator)
        {
            this.context = context;
            this.airPollutionIndicator = airPollutionIndicator;
            this.waterPollutionIndicator = waterPollutionIndicator;
            this.earthPollutionIndicator = earthPollutionIndicator;
            this.radiationPollutionIndicator = radiationPollutionIndicator;
            this.trashPollutionIndicator = trashPollutionIndicator;
            this.economyIndicator = economyIndicator;
            this.healthIndicator = healthIndicator;
            this.energyIndicator = energyIndicator;
        }

        [HttpGet]
        public async Task<ActionResult> Dev()
        {
            var airParameters = context.Parameters.Where(i => i.CategoryId == 1).ToList();
            var waterParameters = context.Parameters.Where(i => i.CategoryId == 2).ToList();
            var earthParameters = context.Parameters.Where(i => i.CategoryId == 3).ToList();
            var radiationParameter = context.Parameters.First(i => i.CategoryId == 4);
            var trashParameters = context.Parameters.Where(i => i.CategoryId == 5).ToList();
            var location = new Location("ПРАТ «ММК ім. Ілліча»", 47.14034067673211, 37.59423930750801, 3000)
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

            var location3 = new Location("ВАТ \"Вінницький завод тракторних агрегатів\"", 49.24287265024275, 28.508196984655054, 1000)
            {
                Address = "вулиця Батозька, 16-Б, Вінниця, Вінницька область",
                LocationParameters = new List<LocationParameter>()
            };
            location3.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.1));
            location3.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1.3));
            location3.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.5));
            location3.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 1.6));

            var location4 = new Location("АТ «ДНІПРОАЗОТ»", 48.48757601416153, 34.6668094104297, 2000)
            {
                Address = "вулиця Горобця, 1, Кам’янське, Дніпропетровська область",
                LocationParameters = new List<LocationParameter>()
            };
            location4.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.8));
            location4.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1.7));
            location4.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.8));
            location4.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 1.7));

            var location5 = new Location("ВАТ \"ДНІПРОШИНА\"", 48.39884789953596, 34.97813279061553, 2000)
            {
                Address = "вулиця Горобця, 1, Кам’янське, Дніпропетровська область",
                LocationParameters = new List<LocationParameter>()
            };
            location5.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 2));
            location5.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 2));
            location5.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.5));
            location5.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 2));

            var location6 = new Location("АТ «ДАЗ»", 48.46604520592015, 34.99232713461423, 800)
            {
                Address = "вулиця Щепкіна, 53, Дніпро, Дніпропетровська область",
                LocationParameters = new List<LocationParameter>()
            };
            location6.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1));
            location6.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1));
            location6.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.1));
            location6.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 1));

            var location7 = new Location("ПРАТ «ІНГЗК»", 47.68591260681167, 33.17590369814871, 100)
            {
                Address = "м. Кривий Ріг, вул. Рудна, 47",
                LocationParameters = new List<LocationParameter>()
            };
            location7.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 2.1));
            location7.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 2.2));
            location7.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.8));
            location7.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 0.8));

            var location8 = new Location("ПРАТ \"ПРОЖЕКТОР\"", 50.77151958221609, 29.25911839628483, 200)
            {
                Address = "Малин, Житомирська область",
                LocationParameters = new List<LocationParameter>()
            };
            location8.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 2));
            location8.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 1.2));
            location8.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.1));
            location8.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 1.7));

            var location9 = new Location("АТ «МОТОР СІЧ»", 47.82863853585648, 35.194812761374386, 3000)
            {
                Address = "проспект Моторобудівників, 60/1, Запоріжжя, Запорізька область",
                LocationParameters = new List<LocationParameter>()
            };
            location9.LocationParameters.AddRange(GenerateValuesByIndex(airParameters, 1.9));
            location9.LocationParameters.AddRange(GenerateValuesByIndex(earthParameters, 2.2));
            location9.LocationParameters.AddRange(GenerateValuesByIndex(waterParameters, 1.7));
            location9.LocationParameters.AddRange(GenerateTrash(trashParameters, location.Area, 2));


            context.Add(location);
            context.Add(location1);
            context.Add(location2);
            context.Add(location3);
            context.Add(location4);
            context.Add(location5);
            context.Add(location6);
            context.Add(location7);
            context.Add(location8);
            context.Add(location9);

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
                {
                    var radiationPollutionIndicatorResult = radiationPollutionIndicator.Calculate(location.Id);
                    if (radiationPollutionIndicatorResult != null)
                        indicators.Add(radiationPollutionIndicatorResult);
                }
                if (TrashPollutionIndicator.CategoryId == categoryId)
                    indicators.Add(trashPollutionIndicator.Calculate(location.Id));
                if (EconomyIndicator.CategoryId == categoryId)
                {
                    var economyIndicatorResult = economyIndicator.Calculate(location.Id);
                    if (economyIndicatorResult != null)
                        indicators.Add(economyIndicatorResult);
                }
                if (HealthIndicator.CategoryId == categoryId)
                {
                    var healthIndicatorResult = healthIndicator.Calculate(location.Id);
                    if (healthIndicatorResult != null)
                        indicators.Add(healthIndicatorResult);
                }
                if (EnergyIndicator.CategoryId == categoryId)
                {
                    var energyIndicatorResult = energyIndicator.Calculate(location.Id);
                    if (energyIndicatorResult != null)
                        indicators.Add(energyIndicatorResult);
                }
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
