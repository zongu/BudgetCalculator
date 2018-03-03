using System;

namespace BudgetCalculator
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int EffectiveDays
        {
            get => (End - Start).Days + 1;
        }

        public bool IsSameMonth()
        {
            return this.Start.Year == this.End.Year && this.Start.Month == this.End.Month;
        }

        public int MonthCount()
        {
            return End.MonthDifference(Start);
        }
    }
}