using System;
using System.Web.Mvc;
using AspMvc.ActionFilters;
using Core.Infrastructure.Data;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Asp
{
    [TestFixture]
    public class TransactionScopeFilterTests
    {
        [Test]
        public void Should_Start_A_Transaction()
        {
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var transactionScopeFilter = new TransactionScopeFilter(unitOfWork);

            transactionScopeFilter.OnAuthorization(Substitute.For<AuthorizationContext>());

            unitOfWork.Received().BeginTransaction();
        }
        [Test]
        public void Should_Not_Start_Or_Commit_A_Transaction_If_It_Is_Disabled()
        {
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var transaction = Substitute.For<ITransaction>();

            unitOfWork.BeginTransaction().Returns(transaction);
            var transactionScopeFilter = new TransactionScopeFilter(unitOfWork);
            transactionScopeFilter.Enabled = false;

            transactionScopeFilter.OnAuthorization(Substitute.For<AuthorizationContext>());
            transactionScopeFilter.OnActionExecuted(Substitute.For<ActionExecutedContext>());

            unitOfWork.DidNotReceiveWithAnyArgs().BeginTransaction();
            transaction.DidNotReceiveWithAnyArgs().Commit();
        }

        [Test]
        public void Should_Commit_A_Transaction_When_There_Are_No_Exceptions()
        {
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var transaction = Substitute.For<ITransaction>();

            unitOfWork.BeginTransaction().Returns(transaction);

            var transactionScopeFilter = new TransactionScopeFilter(unitOfWork);

            transactionScopeFilter.OnAuthorization(Substitute.For<AuthorizationContext>());
            transactionScopeFilter.OnActionExecuted(Substitute.For<ActionExecutedContext>());

            transaction.Received().Commit();
            transaction.DidNotReceive().Rollback();
        }

        [Test]
        public void Should_Rollback_A_Transaction_When_There_Is_An_Exception()
        {
            var actionExecutedContext = Substitute.For<ActionExecutedContext>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var transaction = Substitute.For<ITransaction>();

            actionExecutedContext.Exception = new Exception("bad things happning");
            unitOfWork.BeginTransaction().Returns(transaction);

            var transactionScopeFilter = new TransactionScopeFilter(unitOfWork);
            transactionScopeFilter.OnAuthorization(Substitute.For<AuthorizationContext>());
            transactionScopeFilter.OnActionExecuted(actionExecutedContext);

            transaction.DidNotReceive().Commit();
            transaction.Received().Rollback();
        }
    }
}