using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using EkoMon.DomainModel.Services;
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
        private readonly AzureOpenAIClient azureOpenAiClient;
        public ApiController(EntityContext context, AirPollutionIndicator airPollutionIndicator, WaterPollutionIndicator waterPollutionIndicator, EarthPollutionIndicator earthPollutionIndicator, RadiationPollutionIndicator radiationPollutionIndicator, TrashPollutionIndicator trashPollutionIndicator, EconomyIndicator economyIndicator, HealthIndicator healthIndicator, EnergyIndicator energyIndicator, AzureOpenAIClient azureOpenAiClient)
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
            this.azureOpenAiClient = azureOpenAiClient;
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
            dbLocation.Area = location.Area;
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

        
        [HttpGet("{locationId}")] 
        public async Task<string> GetAdvice(int locationId)
        {
            var location = await context.Locations.Include(l => l.LocationParameters).ThenInclude(p => p.Parameter).ThenInclude(u => u.Unit).FirstOrDefaultAsync(i => i.Id == locationId);
            var categoryNameById = context.Categories.AsEnumerable().ToDictionary(k => k.Id, v => v.Title);
            if (location == null)
                return null;

            var indicators = CalculateIndicators(location);
            
            var text = @"Мій обєкт: {{LocationName}}
Мої індекси:
{{Indexes}}

Обери мені можливі заходи, які можна використати щоб покращити показники індексів на моєму обєкті, використвоючи список захоів наведений нище.
Можливо вигадай свої варіанти.
Також додай можливий бюджет у тисячах гривень до обраних заходів, якщо бюджет обєкту становить {{Money}} тисяч гривень.
Не обовязково робити для кожного індексу, надай пріорітети найгіршим.

**Початок списку заходів**

Заходи щодо Атмосферного повітря:
1. мінімізацію та запобігання викидів шкідливих речовин в атмосферу шляхом застосування промисловими підприємствами екологічних фільтрів;
2. перехід на експлуатацію екологічного транспорту та побутової техніки;
3. контрольована утилізація сміття, особливо це стосується спалення побутових відходів;
3. впровадження комплексних «зелених» альтернатив, які б були корисні не лише для повітря, а і для здоров’я людини (наприклад, мотивувати людей використовувати велосипеди, оскільки це корисно і для екології, і для самопочуття);
4. розробка екологічно орієнтованого законодавства та програми.

Енергетичні заходи:
1. забезпечення належного рівня енергетичної ефективності будівель відповідно до технічних регламентів, норм і правил та будівельних норм;
2. стимулювання зменшення споживання енергії у будівлях;
3. забезпечення скорочення викидів парникових газів у атмосферу;
4. створення умов для залучення інвестицій з метою здійснення енергоефективних заходів;
5. забезпечення термомодернізації будівель, стимулювання використання відновлюваних джерел енергії;
6. розроблення та реалізація національного плану щодо збільшення кількості будівель з близьким до нульового рівнем споживання енергії та стратегії термомодернізації будівель;
7. стимулювання до збільшення кількості будівель з близьким до нульового рівнем споживання енергії, зокрема шляхом нового будівництва та термомодернізації будівель.

Заходи щодо грунтів:
1. Зміна погляду на ґрунт.
2. Оптимізація обробітку ґрунту.
3. Планування сівозмін.
4. Застосування сидератів та багаторічних трав.
5. Застосування біологічних препаратів для захисту рослин.
6. Внесення гноєвих компостів.
7. Відновлення полезахисних лісосмуг.
8. Використання сільськогосподарських угідь згідно технологічних груп земель залежно від крутизни схилів.
9. Робота з ґрунтами в комплексі.
10. Робота з рослинними рештками.

Заходи щодо водних ресурсів:
1. запровадження інтегрованого управління водними ресурсами, 
2. прискорення переходу до управління водокористуванням за басейним принципом; 
3. поліпшення екологічного стану річок та підземних вод України, якості питної води.
4. будівництво очисних споруд для очищення зливових стоків і талих вод;
5. забезпечення ефективної діяльності гідротехнічних споруд;
6. впорядкування  та розчищення русла річки;
7. припинення несанкціонованого скидання каналізаційно-побутових стоків з прилеглої території приватної забудови та скидів зливових вод з території підприємств без очистки;
8. контроль за дотриманням природоохоронного законодавства  у водоохоронній зоні річки та впорядкування прибережної території та річкової заплави.

Заходи щодо відходів:
1. відмовитися від поліетиленових пакетів – екологічно та економічно;
2. відмовитися від одноразових пляшок та посуду;
3. сортувати сміття і віддавати на переробку;
**Кінець списку заходів**
";
            text = text.Replace("{{LocationName}}", location.Title);
            var indexesText = string.Join(";\n", indicators.Select(t => $"{categoryNameById[t.CategoryId]}: {t.Value}({GetRankUkrString(t.Rank)})"));
            text = text.Replace("{{Indexes}}", indexesText);
            text = text.Replace("{{Money}}", $"{100}");

            string GetRankUkrString(IndexRank indexRank)
            {
                return indexRank switch
                {
                    IndexRank.VeryBad => "Дуже погано",
                    IndexRank.Bad => "Погано",
                    IndexRank.Medium => "Середньо",
                    IndexRank.Good => "Добре",
                    IndexRank.VeryGood => "Дуже добре",
                    _ => throw new ArgumentOutOfRangeException(nameof(indexRank), indexRank, null)
                };
            }

            var response = await azureOpenAiClient.GetResponse(text);
            return response;

        }
    }
}
