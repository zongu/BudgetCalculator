using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;

namespace BudgetCalculator
{
    [TestClass]
    public class UnitTest1
    {
        private Accounting _accounting;
        private IRepository<Budget> _repository = Substitute.For<IRepository<Budget>>();

        [TestInitialize]
        public void TestInit()
        {
            _accounting = new Accounting(_repository);
        }

        [TestMethod]
        public void no_budget()
        {
            GivenBudgets();
            TotalAmountShouldBe(new DateTime(2018, 3, 1), new DateTime(2018, 3, 1), 0);
        }

        [TestMethod]
        public void no_effective_days_period_before_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 2, 1), new DateTime(2018, 2, 15), 0);
        }

        [TestMethod]
        public void invalid_period()
        {
            Action actual = () => _accounting.Calculate(new DateTime(2018, 3, 1), new DateTime(2018, 2, 1));
            actual.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void fifteen_effective_days_period_between_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 15), 30);
        }

        [TestMethod]
        public void period_equals_to_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 31), 62);
        }

        [TestMethod]
        public void period_cross_3_budget_months()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 0 },
                new Budget() { YearMonth = "201803", Amount = 62 });

            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 3, 10), 82);
        }

        [TestMethod]
        public void period_equals_to_2_budget_month()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 }
                );

            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 2, 28), 342);
        }

        [TestMethod]
        public void period_cross_multiple_budgets()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 62 }
                );

            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 3, 10), 362);
        }

        [TestMethod]
        public void period_cross_4_budgets()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201712", Amount = 310 },
                new Budget() { YearMonth = "201801", Amount = 310 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 310 }
                );

            TotalAmountShouldBe(new DateTime(2017, 12, 1), new DateTime(2018, 3, 10), 1000);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _repository.GetAll().Returns(budgets.ToList());
        }

        private void TotalAmountShouldBe(DateTime start, DateTime end, int expected)
        {
            _accounting.Calculate(start, end).Should().Be(expected);
        }
    }
}