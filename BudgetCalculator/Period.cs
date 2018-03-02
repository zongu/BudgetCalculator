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
    }
}