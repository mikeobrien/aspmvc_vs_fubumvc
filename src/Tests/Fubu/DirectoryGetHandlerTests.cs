using System;
using System.Linq;
using Core.Domain;
using FubuMvc;
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
            var handler = new IndexPublicGetHandler(repository);
            
            var viewModel = handler.Execute(new IndexRequest {UserAgent = "" });

            viewModel.Results.Count.ShouldEqual(20);
        }

        [Test]
        public void Should_Indicate_That_The_User_Is_Using_Chrome()
        {
            var handler = new IndexPublicGetHandler(new MemoryRepository<DirectoryEntry>());

            var viewModel = handler.Execute(new IndexRequest { UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7" });

            viewModel.IsChromeBrowser.ShouldBeTrue();
        }

        [Test]
        public void Should_Indicate_That_The_User_Is_Not_Using_Chrome()
        {
            var handler = new IndexPublicGetHandler(new MemoryRepository<DirectoryEntry>());

            var viewModel = handler.Execute(new IndexRequest { UserAgent = "Mozilla/6.0 (Macintosh; I; Intel Mac OS X 11_7_9; de-LI; rv:1.9b4) Gecko/2012010317 Firefox/10.0a4" });

            viewModel.IsChromeBrowser.ShouldBeFalse();
        }

        [Test]
        public void Should_Blow_Up_If_User_Is_Using_Internet_Explorer()
        {
            var handler = new IndexPublicGetHandler(new MemoryRepository<DirectoryEntry>());

            Assert.Throws<Exception>(() => handler.Execute(new IndexRequest { UserAgent = "Mozilla/5.0 (Windows; U; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)" }));
        }
    }
}