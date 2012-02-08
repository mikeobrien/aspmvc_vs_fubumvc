using System;
using Core.Infrastructure.Data;
using FubuMVC.Core.Behaviors;
using FubuMvc.Behaviors;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Fubu
{
    [TestFixture]
    public class TransactionScopeBehaviorTests
    {
        [Test]
        public void Should_Start_A_Transaction()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var transactionScopeBehavior = new TransactionScopeBehavior(innerBehavior, unitOfWork);

            transactionScopeBehavior.Invoke();

            unitOfWork.Received().BeginTransaction();
        }

        [Test]
        public void Should_Commit_A_Transaction_When_There_Are_No_Exceptions()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var transaction = Substitute.For<ITransaction>();

            unitOfWork.BeginTransaction().Returns(transaction);

            var transactionScopeBehavior = new TransactionScopeBehavior(innerBehavior, unitOfWork);

            transactionScopeBehavior.Invoke();

            transaction.Received().Commit();
            transaction.DidNotReceive().Rollback();
        }

        [Test]
        public void Should_Rollback_A_Transaction_When_There_Is_An_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var transaction = Substitute.For<ITransaction>();

            innerBehavior.When(x => x.Invoke()).Do(x => { throw new Exception("bad things happning"); });
            unitOfWork.BeginTransaction().Returns(transaction);

            var transactionScopeBehavior = new TransactionScopeBehavior(innerBehavior, unitOfWork);

            Assert.Throws<Exception>(transactionScopeBehavior.Invoke);

            transaction.DidNotReceive().Commit();
            transaction.Received().Rollback();
        }
    }
}