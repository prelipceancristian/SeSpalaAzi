public class HolidayStatusModel(Holiday[] holidays)
{
    public Holiday[] Holidays = holidays;
    public bool CanWash { get => Holidays.Length == 0; }
}


public class Holiday(string name, DateTimeOffset date, HolidayLevel holidayLevel)
{
    public string Name { get; init; } = name;
    public DateTimeOffset Date { get; init; } = date;
    public HolidayLevel HolidayLevel { get; init; } = holidayLevel;
}


public enum HolidayLevel
{
    Normal,
    Holy,
    ExtraHoly
}