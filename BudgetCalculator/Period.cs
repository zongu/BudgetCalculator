using System;

namespace BudgetCalculator
{
    internal class Period
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

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int EffectiveDays(Budget budget)
        {
            if (EndDate < budget.FirstDay)
            {
                return 0;
            }

            if (StartDate > budget.LastDay)
            {
                return 0;
            }
            var effectiveEndDate = EndDate;
            if (EndDate > budget.LastDay)
            {
                effectiveEndDate = budget.LastDay;
            }

            var effectiveStartDate = StartDate;
            if (StartDate < budget.FirstDay)
            {
                effectiveStartDate = budget.FirstDay;
            }

            var effectiveDays = (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
            return effectiveDays;
        }
    }
}