using System.Text.Json.Serialization;

namespace SeSpalaAzi3.Models
{
    public class HolidayStatusModel
    {
        public HolidayStatusModel()
        {
            Holidays = [];
        }

        [JsonPropertyName("Holidays")]
        public Holiday[] Holidays { get; set; }

        [JsonIgnore]
        public bool CanWash { get => Holidays.Length == 0; }
    }


    public class Holiday
    {
        public Holiday() { }

        [JsonPropertyName("Name")]
        public required string Name { get; init; }

        [JsonIgnore]
        public DateTimeOffset Date { get; init; }

        [JsonIgnore]
        public HolidayLevel HolidayLevel { get; init; }
    }


    public enum HolidayLevel
    {
        Normal,
        Holy,
        ExtraHoly
    }
}