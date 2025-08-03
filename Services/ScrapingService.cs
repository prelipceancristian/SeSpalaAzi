using HtmlAgilityPack;
using SeSpalaAzi3.Models;

namespace SeSpalaAzi3.Services
{
    public class ScrapingService : IScrapingService
    {
        private const string rootUrl = @"https://doxologia.ro/calendar-ortodox";

        public HolidayStatusModel GetHolidayStatus(DateTime date)
        {
            var htmlDocument = GetHtmlDocument(date);
            var holidayContainer = GetDayContainerFromDocument(date, htmlDocument);
            var result = BuildHolidayStatusModel(holidayContainer);

            return result;
        }

        private static HolidayStatusModel BuildHolidayStatusModel(HtmlNode dayContainer)
        {
            var result = new HolidayStatusModel();
            if ((dayContainer.Attributes["class"]?.Value.Split().Contains("rosu")) != true)
            {
                return result;
            }

            // there are holidays in this container
            var holidays = dayContainer.ChildNodes.Select(node => new Holiday
            {
                Name = node.InnerText,
                HolidayLevel = DetermineHolidayLevel(node)
            });
            result.Holidays = [.. holidays];

            return result;
        }

        private static HtmlNode GetDayContainerFromDocument(DateTime date, HtmlDocument doc)
        {
            // there is a single month view div in the page
            var monthNode = doc.DocumentNode.SelectNodes("//*[contains(@class, 'month-view')]")[0];
            // there is a wrapper div for each day 
            var dayNodes = monthNode.ChildNodes.Where(n => n.Name == "div").ToArray();
            // selecting based on the given day
            var specificDayNode = dayNodes[date.Day - 1];
            // The day node is something like this:
            // <div>
            //     Actual date
            // </div>
            // <div>
            //     <div class="calendar-zi rosu">
            //         <a href="/...">...</a>  <!-- simple holiday -->
            //         <a href="/..." class="rosub"> ✝) ... </a> <!-- holy day -->
            //         <a href="/..." class="rosub"> (✝) ... </a> <!-- extra holy day -->
            //     </div>
            // </div>
            var dayContainer = specificDayNode.ChildNodes[1].ChildNodes[0];
            return dayContainer;
        }

        private static HolidayLevel DetermineHolidayLevel(HtmlNode holidayContainer)
        {
            var holidayLevel = HolidayLevel.SimpleEntry;
            if (holidayContainer.Attributes["class"]?.Value.Split().Contains("rosub") == true)
            {
                holidayLevel = HolidayLevel.Holy;
                if (holidayContainer.InnerHtml.Contains("(✝)"))
                {
                    holidayLevel = HolidayLevel.ExtraHoly;
                }
            }

            return holidayLevel;
        }

        private static HtmlDocument GetHtmlDocument(DateTime date)
        {
            var web = new HtmlWeb();
            var formattedDate = date.ToString("yyyyMM");
            var fullUrl = $"{rootUrl}/{formattedDate}";
            var doc = web.Load(fullUrl);
            return doc;
        }
    }
}