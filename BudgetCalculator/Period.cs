using System;

namespace BudgetCalculator
{
    internal class Period
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
            var effectiveStartDate = this.Start;
            if (this.Start < period.Start)
            {
                effectiveStartDate = period.Start;
            }

            var effectiveEndDate = this.End;
            if (this.End > period.End)
            {
                effectiveEndDate = period.End;
            }
            return (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
        }
    }
}