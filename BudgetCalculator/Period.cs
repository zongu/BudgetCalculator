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

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int EffectiveDays(Period periodOfBudget)
        {
            if (EndDate < periodOfBudget.StartDate)
            {
                return 0;
            }

            if (StartDate > periodOfBudget.EndDate)
            {
                return 0;
            }
            var effectiveEndDate = EndDate;
            if (EndDate > periodOfBudget.EndDate)
            {
                effectiveEndDate = periodOfBudget.EndDate;
            }

            var effectiveStartDate = StartDate;
            if (StartDate < periodOfBudget.StartDate)
            {
                effectiveStartDate = periodOfBudget.StartDate;
            }

            var effectiveDays = (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
            return effectiveDays;
        }
    }
}