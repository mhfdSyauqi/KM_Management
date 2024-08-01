namespace KM_Management.Helper;

public record FilteredDate(DateTime StartDate, DateTime EndDate);

public class DateFilter
{
    public static FilteredDate GenerateFilterDate(string Filter, DateTime? StartDate, DateTime? EndDate)
    {
        var currDate = DateTime.Now;
        var startDate = new DateTime(currDate.Year, currDate.Month, currDate.Day, 0, 0, 0);
        var endDate = startDate.AddDays(1).AddMinutes(-1);

        switch (Filter)
        {
            case "Custom":
                startDate = StartDate.Value;
                endDate = EndDate.Value;
                break;
            case "Yesterday":
                startDate = startDate.AddDays(-1);
                endDate = endDate.AddDays(-1);
                break;
            case "Last 7 Days":
                startDate = startDate.AddDays(-6);
                break;
            case "Last 30 Days":
                startDate = startDate.AddDays(-29);
                break;
            case "Last 3 Months":
                startDate = startDate.AddMonths(-3).AddDays(1);
                break;
            case "Last 1 Year":
                startDate = startDate.AddYears(-1).AddDays(1);
                break;
            default:
                break;
        }

        return new FilteredDate(startDate, endDate);
    }
}
