using SeSpalaAzi3.Models;

namespace SeSpalaAzi3.Services
{
    public interface IScrapingService
    {
        public HolidayStatusModel GetHolidayStatus(DateTime date);
    }
}