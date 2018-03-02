using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    internal class BudgetCalculat
    {
        private readonly IRepository<Budget> _repo;

        public BudgetCalculat(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal Calculate(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException();
            }

            var list = this._repo.GetAll();
            if (list.Any() == false)
            {
                return 0;
            }

            if (IsSameMonth(start, end))
            {
                return GetOneMonthAmount(start, end);
            }
            else
            {
                return GetRangeMonthAmount(start, end);
            }
        }

        private decimal GetRangeMonthAmount(DateTime start, DateTime end)
        {
            var monthCount = end.MonthDifference(start);
            var total = 0;
            for (int index = 0; index <= monthCount; index++)
            {
                if (index == 0)
                {
                    total += GetOneMonthAmount(start, GetLastDate(start));
                }
                else if (index == monthCount)
                {
                    total += GetOneMonthAmount(GetFirstDate(end), end);
                }
                else
                {
                    var now = start.AddMonths(index);
                    total += GetOneMonthAmount(GetFirstDate(now), GetLastDate(now));
                }
            }
            return total;
        }

        private static bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.Year == end.Year && start.Month == end.Month;
        }

        private DateTime GetLastDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        private DateTime GetFirstDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        private int GetOneMonthAmount(DateTime start, DateTime end)
        {
            var list = this._repo.GetAll();
            var budget = list.Get(start)?.Amount ?? 0;

            var days = DateTime.DaysInMonth(start.Year, start.Month);
            var validDays = GetValidDays(start, end);

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
    }
}