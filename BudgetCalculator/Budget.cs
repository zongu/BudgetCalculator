using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public string YearMonth { get; set; }

        public int Amount { get; set; }

        public DateTime FirstDate
        {
            get => new DateTime(
                int.Parse(YearMonth.Substring(0, 4)),
                int.Parse(YearMonth.Substring(4, 2)),
                1);
        }

        public DateTime LastDate
        {
            get => FirstDate.LastDate();
        }

        public int DayilyAmount
        {
            get => Amount / DateTime.DaysInMonth(
                int.Parse(YearMonth.Substring(0, 4)),
                int.Parse(YearMonth.Substring(4, 2)));
        }
    }
}