using BeMarketer.Models;

namespace BeMarketer.ViewModels
{
    public class EmployeeStats
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int LeadCount { get; set; }
        public int NewLeads { get; set; }
        public int ContactedLeads { get; set; }
        public int QualifiedLeads { get; set; }
        public int LostLeads { get; set; }
        public double ConversionRate => LeadCount > 0 ? Math.Round((double)QualifiedLeads / LeadCount * 100, 1) : 0;
    }

    public class PeriodStats
    {
        public int TotalLeads { get; set; }
        public int NewLeads { get; set; }
        public int ContactedLeads { get; set; }
        public int QualifiedLeads { get; set; }
        public int LostLeads { get; set; }
        public double ConversionRate => TotalLeads > 0 ? Math.Round((double)QualifiedLeads / TotalLeads * 100, 1) : 0;
    }

    public class DailyLeadCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class StatisticsViewModel
    {
        // Filtry
        public int SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }
        public int? CompareYear { get; set; }
        public int? CompareMonth { get; set; }

        // Statystyki głównego okresu
        public PeriodStats CurrentPeriodStats { get; set; } = new();
        public PeriodStats? ComparePeriodStats { get; set; }

        // Najlepszy pracownik w wybranym okresie
        public EmployeeStats? TopEmployee { get; set; }

        // Lista wszystkich pracowników (dla tabeli rankingowej)
        public List<EmployeeStats> EmployeeRanking { get; set; } = new();

        // Dane dla wykresu dziennego (tylko gdy wybrany miesiąc)
        public List<DailyLeadCount> DailyData { get; set; } = new();
        public List<DailyLeadCount> CompareDailyData { get; set; } = new();

        // Dane miesięczne (gdy wybrany tylko rok)
        public List<(int Month, int Count)> MonthlyData { get; set; } = new();
        public List<(int Month, int Count)> CompareMonthlyData { get; set; } = new();

        // Pomocnicze
        public List<int> AvailableYears { get; set; } = new();
        public string PeriodLabel => SelectedMonth.HasValue
            ? $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(SelectedMonth.Value)} {SelectedYear}"
            : SelectedYear.ToString();

        public string? ComparePeriodLabel => CompareMonth.HasValue && CompareYear.HasValue
            ? $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CompareMonth.Value)} {CompareYear}"
            : CompareYear.HasValue ? CompareYear.ToString() : null;

        public bool HasComparison => ComparePeriodStats != null;
    }
}
