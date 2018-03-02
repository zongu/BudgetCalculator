using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public DateTime FirstDay
        {
            get
            {
                var firstDay = this.YearMonth + "01";
                return DateTime.ParseExact(firstDay, "yyyyMMdd", null);
            }
        }

        public string YearMonth { get; set; }

        public int Amount { get; set; }

        public DateTime LastDay
        {
            get
            {
                return new DateTime(FirstDay.Year,FirstDay.Month,TotalDays);
            }
        }

        public int TotalDays
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        public int DailyAmount()
        {
            var dailyAmount = Amount / TotalDays;
            return dailyAmount;
        }

        public int EffectiveAmount(Period period)
        {
            return period.EffectiveDays(new Period(this.FirstDay, this.LastDay)) * this.DailyAmount();
        }
    }
}