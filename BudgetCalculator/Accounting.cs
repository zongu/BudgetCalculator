using System;
using System.Collections.Generic;
using System.Linq;

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

        public int MonthsBetweenPeriod()
        {
            var monthsBetweenPeriod = EndDate.MonthDifference(StartDate);
            return monthsBetweenPeriod;
        }
    }

    internal class Accounting
    {
        private readonly IRepository<Budget> _repo;

        public Accounting(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime start, DateTime end)
        {
            var period = new Period(start, end);

            var budgets = _repo.GetAll();
            var totalAmount = 0m;
            foreach (var budget in budgets)
            {
                totalAmount += EffectiveAmountOfBudget(period, budget);
            }
            return totalAmount;
        }

        private int EffectiveAmountOfBudget(Period period, Budget budget)
        {
            if (period.EndDate < budget.FirstDay)
            {
                return 0;
            }

            if (period.StartDate > budget.LastDay)
            {
                return 0;
            }

            var effectiveEndDate = period.EndDate;
            if (period.EndDate > budget.LastDay)
            {
                effectiveEndDate = budget.LastDay;
            }

            var effectiveStartDate = period.StartDate;
            if (period.StartDate < budget.FirstDay)
            {
                effectiveStartDate = budget.FirstDay;
            }

            var effectiveDays = (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;

            var dailyAmount = budget.Amount / budget.TotalDays;
            return effectiveDays * dailyAmount;
        }

        private bool IsSameMonth(Period period)
        {
            return period.StartDate.Year == period.EndDate.Year && period.StartDate.Month == period.EndDate.Month;
        }

        private int GetOneMonthAmount(Period period)
        {
            var list = this._repo.GetAll();
            var budget = list.Get(period.StartDate)?.Amount ?? 0;

            var days = DateTime.DaysInMonth(period.StartDate.Year, period.StartDate.Month);
            var validDays = GetValidDays(period.StartDate, period.EndDate);

            return (budget / days) * validDays;
        }

        private int GetValidDays(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }
    }

    public static class BudgetExtension
    {
        public static Budget Get(this List<Budget> list, DateTime date)
        {
            return list.FirstOrDefault(r => r.YearMonth == date.ToString("yyyyMM"));
        }
    }

    public static class DateTimeExtension
    {
        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        public static DateTime LastDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime FirstDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}