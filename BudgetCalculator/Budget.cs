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
                var firstDay = this.YearMonth + "01";
                return DateTime.ParseExact(firstDay, "yyyyMMdd", null);
            }
        }

        private DateTime LastDay
        {
            get
            {
                return new DateTime(FirstDay.Year, FirstDay.Month, TotalDays);
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
            return period.EffectiveDays(new Period(this.FirstDay, this.LastDay)) * this.DailyAmount();
        }

        private int DailyAmount()
        {
            return Amount / TotalDays;
        }
    }
}