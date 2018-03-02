using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        private DateTime FirstDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            }
        }

        private DateTime LastDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + TotalDays, "yyyyMMdd", null);
            }
        }

        private int TotalDays
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        public int EffectiveAmount(Period period)
        {
            return this.DailyAmount() * period.EffectiveDays(new Period(this.FirstDay, this.LastDay));
        }

        private int DailyAmount()
        {
            return (Amount / TotalDays);
        }
    }
}