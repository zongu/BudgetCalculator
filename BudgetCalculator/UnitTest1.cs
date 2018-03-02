using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    [TestClass]
    public class UnitTest1
    {
        private Accounting _accounting;
        private IRepository<Budget> _budgetRepository = Substitute.For<IRepository<Budget>>();

        [TestInitialize]
        public void TestInit()
        {
            _accounting = new Accounting(_budgetRepository);
        }

        /// <summary>
        /// 資料庫沒有預算
        /// </summary>
        [TestMethod]
        public void no_budgets()
        {
            GivenBudgets();
            TotalAmountShouldBe(new DateTime(2018, 3, 1), new DateTime(2018, 3, 1), 0);
        }

        /// <summary>
        /// 找不到這個月的預算_拿到0
        /// </summary>
        [TestMethod]
        public void no_effective_days_period_after_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 2, 1), new DateTime(2018, 2, 15), 0);
        }

        /// <summary>
        /// 時間起訖不合法
        /// </summary>
        [TestMethod]
        public void invalid_period()
        {
            Action actual = () => _accounting.TotalAmount(new DateTime(2018, 3, 1), new DateTime(2018, 2, 1));
            actual.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// 當一月預算為62_一月一號到一月十五號_預算拿到30
        /// </summary>
        [TestMethod]
        public void fifteen_effective_days_peirod_included_by_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 15), 30);
        }

        /// <summary>
        /// 當一月預算為62_一月一號到一月三十一號_預算拿到62
        /// </summary>
        [TestMethod]
        public void period_equal_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 62 });
            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 31), 62);
        }

        /// <summary>
        /// 當一月預算為62_二月預算為0_三月預算為62_一月一號到三月十號_預算拿到82
        /// </summary>
        [TestMethod]
        public void multiple_budgets()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 0 },
                new Budget() { YearMonth = "201803", Amount = 62 });

            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 3, 10), 82);
        }

        /// <summary>
        /// 當一月預算為62_二月預算為280_一月一號到二月二十八號_預算拿到342
        /// </summary>
        [TestMethod]
        public void period_is_equal_to_two_budget_months()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 });

            TotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 2, 28), 342);
        }

        [TestMethod]
        public void 當一月預算為62_二月預算為280_三月預算為62_一月一號到三月十號_預算拿到362()
        {
            var target = BudgetCalculat(new List<Budget>()
            {
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 62 },
            });
            var start = new DateTime(2018, 1, 1);
            var end = new DateTime(2018, 3, 10);

            var actual = target.TotalAmount(start, end);

            actual.Should().Be(362);
        }

        [TestMethod]
        public void 當十二月預算為310一月預算為310_二月預算為280_三月預算為310_十二月一號到三月十號_預算拿到1000()
        {
            var target = BudgetCalculat(new List<Budget>()
            {
                new Budget() { YearMonth = "201712", Amount = 310 },
                new Budget() { YearMonth = "201801", Amount = 310 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 310 },
            });
            var start = new DateTime(2017, 12, 1);
            var end = new DateTime(2018, 3, 10);

            var actual = target.TotalAmount(start, end);

            actual.Should().Be(1000);
        }

        private Accounting BudgetCalculat(List<Budget> budgets)
        {
            IRepository<Budget> repo = Substitute.For<IRepository<Budget>>();
            repo.GetAll().Returns(budgets);

            return new Accounting(repo);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _budgetRepository.GetAll().Returns(budgets.ToList());
        }

        private void TotalAmountShouldBe(DateTime start, DateTime end, int expected)
        {
            _accounting.TotalAmount(start, end).Should().Be(expected);
        }
    }
}