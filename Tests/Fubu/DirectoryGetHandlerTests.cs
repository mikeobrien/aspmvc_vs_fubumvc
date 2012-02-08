﻿using System.Linq;
using Core.Domain;
using FubuMvc.Directory;
using NUnit.Framework;
using Should;

namespace Tests.Fubu
{
    [TestFixture]
    public class DirectoryGetHandlerTests
    {
        [Test]
        public void Should_Return_The_First_Twenty_Directory_Results()
        {
            var repository = new MemoryRepository<DirectoryEntry>(Enumerable.Range(0, 30).Select(x => new DirectoryEntry()));
            var handler = new GetHandler(repository);
            
            var viewModel = handler.Execute(new GetRequestModel {UserAgent = "" });

            viewModel.Results.Count.ShouldEqual(20);
        }

        [Test]
        public void Should_Indicate_That_The_User_Is_Using_Chrome()
        {
            var handler = new GetHandler(new MemoryRepository<DirectoryEntry>());

            var viewModel = handler.Execute(new GetRequestModel { UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7" });

            viewModel.IsChromeBrowser.ShouldBeTrue();
        }

        [Test]
        public void Should_Indicate_That_The_User_Is_Not_Using_Chrome()
        {
            var handler = new GetHandler(new MemoryRepository<DirectoryEntry>());

            var viewModel = handler.Execute(new GetRequestModel { UserAgent = "Mozilla/5.0 (Windows; U; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)" });

            viewModel.IsChromeBrowser.ShouldBeFalse();
        }
    }
}