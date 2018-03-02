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
            return _repo.GetAll().Sum(b => b.EffectiveAmount(period));
        }
    }
}