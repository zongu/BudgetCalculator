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

        public int EffectiveDays(Budget budget)
        {
            var effectiveStartDate = this.Start;
            if (this.Start < budget.FirstDay)
            {
                effectiveStartDate = budget.FirstDay;
            }

            var effectiveEndDate = this.End;
            if (this.End > budget.LastDay)
            {
                effectiveEndDate = budget.LastDay;
            }
            return (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
        }
    }
}