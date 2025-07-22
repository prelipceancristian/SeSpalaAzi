using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SeSpalaAzi3.Models;
using SeSpalaAzi3.Services;
using StackExchange.Redis;

namespace SeSpalaAzi3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IDatabase _cache;
    private readonly IScrapingService _scrapingService;

    public HomeController(ILogger<HomeController> logger, IConnectionMultiplexer cache, IScrapingService scrapingService)
    {
        _logger = logger;
        _cache = cache.GetDatabase();
        _scrapingService = scrapingService;
    }

    public IActionResult Index([FromQuery] DateTime? date)
    {
        var key = (date ?? DateTime.Now).ToString("dd-MM-yyyy");

        var cacheContent = _cache.StringGet(key);
        if (cacheContent.HasValue)
        {
            var cachedModel = JsonSerializer.Deserialize<HolidayStatusModel>(cacheContent.ToString());
            return View(cachedModel);
        }

        var holidayStatusModel = _scrapingService.GetHolidayStatus(DateTime.Now);
        _cache.StringSet(key, JsonSerializer.Serialize(holidayStatusModel));

        return View(holidayStatusModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
