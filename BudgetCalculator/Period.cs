using System;

namespace BudgetCalculator
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException();
            }
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int EffectiveDays(Period period)
        {
            return (EffectiveEndDate(period).AddDays(1) - EffectiveStartDate(period)).Days;
        }

        private DateTime EffectiveEndDate(Period period)
        {
            return End > period.End
                ? period.End
                : End;
        }

        private DateTime EffectiveStartDate(Period period)
        {
            return Start < period.Start
                ? period.Start
                : Start;
        }
    }
}