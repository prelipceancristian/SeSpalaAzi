using HtmlAgilityPack;
using SeSpalaAzi3.Models;

namespace SeSpalaAzi3.Services
{
    public class ScrapingService : IScrapingService
    {
        // don't ban me lol
        private const string rootUrl = @"https://doxologia.ro/calendar-ortodox";

        public HolidayStatusModel GetHolidayStatus(DateTime date)
        {
            var web = new HtmlWeb();

            //TODO: can use async overload
            var formattedDate = date.ToString("yyyyMM");
            var fullUrl = $"{rootUrl}/{formattedDate}";
            var doc = web.Load(fullUrl);

            // there is a single month view div in the page
            var monthNode = doc.DocumentNode.SelectNodes("//*[contains(@class, 'month-view')]")[0];
            // there is a wrapper div for each day 
            var dayNodes = monthNode.ChildNodes.Where(n => n.Name == "div").ToArray();
            // I am selecting the given day
            var specificDayNode = dayNodes[date.Day];
            // Selecting the wrapper that contains the list of holidays for that day
            //TODO: here something goes wrong
            var dayHolidayContainer = specificDayNode.ChildNodes[1].SelectNodes("//*[contains(@class, 'calendar-zi')]")[0];

            var result = new HolidayStatusModel();
            if (dayHolidayContainer.Attributes["class"]?.Value.Split().Contains("rosu") == true)
            {
                //TODO: parse the actual lines for the holidays
                var holiday = new Holiday
                {
                    Name = "testHoliday",
                    Date = DateTime.Now,
                    HolidayLevel = HolidayLevel.Holy
                };
                result.Holidays = [holiday];
            }

            return result;
        }
    }
}