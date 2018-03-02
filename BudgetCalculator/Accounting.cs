using System;
using System.Collections.Generic;
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

            var budgets = this._repo.GetAll();
            if (budgets.Any())
            {
                var totalAmount = 0m;
                foreach (var budget in budgets)
                {
                    totalAmount += GetOneMonthAmount(period, budget);
                }
                return totalAmount;
                return IsSameMonth(period)
                    ? GetOneMonthAmount(period, budgets.Get(period.Start))
                    : GetRangeMonthAmount(period);
            }
            return 0;
        }

        private decimal GetRangeMonthAmount(Period period)
        {
            var budgets = this._repo.GetAll();
            var monthCount = period.TotalMonths();
            var total = 0;
            for (var index = 0; index <= monthCount; index++)
            {
                var monthPeriod = GetPeriod(period, index, monthCount);

                var budget = budgets.Get(monthPeriod.Start);
                total += GetOneMonthAmount(monthPeriod, budget);
            }
            return total;
        }

        private static Period GetPeriod(Period period, int index, int monthCount)
        {
            if (index == 0)
            {
                return new Period(period.Start, period.Start.LastDate());
            }
            else if (index == monthCount)
            {
                return new Period(period.End.FirstDate(), period.End);
            }
            else
            {
                var now = period.Start.AddMonths(index);
                return new Period(now.FirstDate(), now.LastDate());
            }
        }

        private bool IsSameMonth(Period period)
        {
            return period.Start.Year == period.End.Year && period.Start.Month == period.End.Month;
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