using System;

namespace BudgetCalculator
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException();
            }
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime EndDate { get; private set; }
        public DateTime StartDate { get; private set; }

        public int EffectiveDays(Period period)
        {
            if (EndDate < period.StartDate)
            {
                return 0;
            }

            if (StartDate > period.EndDate)
            {
                return 0;
            }

            return (EffectiveEndDate(period).AddDays(1) - EffectiveStartDate(period)).Days;
        }

        private DateTime EffectiveEndDate(Period period)
        {
            return EndDate > period.EndDate
                ? period.EndDate
                : EndDate;
        }

        private DateTime EffectiveStartDate(Period period)
        {
            return StartDate < period.StartDate
                ? period.StartDate
                : StartDate;
        }
    }
}