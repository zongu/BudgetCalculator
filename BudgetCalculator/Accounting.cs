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

            return budget.DailyAmount() * period.EffectiveDays(new Period(budget.FirstDay,budget.LastDay));
        }
    }
}