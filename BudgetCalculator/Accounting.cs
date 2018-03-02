using System;
using System.Linq;

namespace BudgetCalculator
{
    internal class Accounting
    {
        private readonly IRepository<Budget> _repo;

        public Accounting(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal Calculate(DateTime start, DateTime end)
        {
            var period = new Period(start, end);
            return _repo.GetAll().Sum(b => GetOneMonthAmount(period, b));
        }

        private int GetOneMonthAmount(Period period, Budget budget)
        {
            if (budget == null)
            {
                return 0;
            }

            var validDays = EffectiveDays(period, budget);
            return budget.DailyAmount() * validDays;
        }

        private int EffectiveDays(Period period, Budget budget)
        {
            var effectiveStartDate = period.Start;
            if (period.Start < budget.FirstDay)
            {
                effectiveStartDate = budget.FirstDay;
            }

            var effectiveEndDate = period.End;
            if (period.End > budget.LastDay)
            {
                effectiveEndDate = budget.LastDay;
            }
            return (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
        }
    }
}