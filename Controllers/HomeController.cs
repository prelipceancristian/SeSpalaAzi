using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeSpalaAzi3.Models;
using StackExchange.Redis;

namespace SeSpalaAzi3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IDatabase _cache;

    public HomeController(ILogger<HomeController> logger, IConnectionMultiplexer cache)
    {
        _logger = logger;
        _cache = cache.GetDatabase();
    }

    public IActionResult Index()
    {
        var holidayStatusModel = new HolidayStatusModel([new Holiday("asd", DateTimeOffset.Now, HolidayLevel.Normal)]);
        var isInCache = _cache.StringGet("key");
        if (isInCache.HasValue)
        {
            _cache.KeyDelete("key");

            return View(holidayStatusModel);
        }
        _cache.StringSet("key", "value");

        return View(new HolidayStatusModel([]));
        // check cache
        // if found, return
        // request using web crawler
        // update cache
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
