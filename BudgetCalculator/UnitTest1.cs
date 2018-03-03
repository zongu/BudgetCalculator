using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BudgetCalculator
{
    [TestClass]
    public class UnitTest1
    {
        private IRepository<Budget> repo = Substitute.For<IRepository<Budget>>();
        private Accountting target;

        [TestInitialize]
        public void TestInit()
        {
            target = new Accountting(repo);
        }

        [TestMethod]
        public void no_budget()
        {
            Givenbudget();

            TotalAmountSholdBe(0, new DateTime(2018, 3, 1), new DateTime(2018, 3, 1));
        }

        
        [TestMethod]
        public void no_budget_in_month()
        {
            Givenbudget(new Budget() { YearMonth = "201801", Amount = 62 } );

            TotalAmountSholdBe(0, new DateTime(2018, 2, 1), new DateTime(2018, 2, 15));
        }

        [TestMethod]
        public void illegal_date()
        {
            Givenbudget();

            Action actual = () => target.TotalAmount(new DateTime(2018, 3, 1), new DateTime(2018, 2, 1));
            actual.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void budget_in_month()
        {
            Givenbudget(
                new Budget() { YearMonth = "201801", Amount = 62 }
                );

            TotalAmountSholdBe(30, new DateTime(2018, 1, 1), new DateTime(2018, 1, 15));
        }

        [TestMethod]
        public void budget_full_month()
        {
            Givenbudget(
                new Budget() { YearMonth = "201801", Amount = 62 }
            );

            TotalAmountSholdBe(62, new DateTime(2018, 1, 1), new DateTime(2018, 1, 31));
        }

        [TestMethod]
        public void budget_empty_in_month()
        {
            Givenbudget(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201803", Amount = 62 }
            );

            TotalAmountSholdBe(82, new DateTime(2018, 1, 1), new DateTime(2018, 3, 10));
        }

        [TestMethod]
        public void budget_two_full_month()
        {
            Givenbudget(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 }
            );

            TotalAmountSholdBe(342, new DateTime(2018, 1, 1), new DateTime(2018, 2, 28));
        }

        [TestMethod]
        public void budget_cross_month()
        {
            Givenbudget(
                new Budget() { YearMonth = "201801", Amount = 62 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 62 }
            );

            TotalAmountSholdBe(362, new DateTime(2018, 1, 1), new DateTime(2018, 3, 10));
        }

        [TestMethod]
        public void budget_through_year()
        {
            Givenbudget(
                new Budget() { YearMonth = "201712", Amount = 310 },
                new Budget() { YearMonth = "201801", Amount = 310 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 310 }
            );

            TotalAmountSholdBe(1000, new DateTime(2017, 12, 1), new DateTime(2018, 3, 10));
        }

        private void Givenbudget(params Budget[] budgets)
        {
            repo.GetAll().Returns(budgets.ToList());
        }

        private void TotalAmountSholdBe(decimal expected, DateTime start, DateTime end)
        {
            this.target.TotalAmount(start, end).Should().Be(expected);
        }
    }
}