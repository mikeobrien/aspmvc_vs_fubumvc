using System.Linq;
using Core.Domain;
using FubuMvc.Directory;
using FubuMvc.Directory.Entries;
using NUnit.Framework;
using Should;

namespace Tests.Fubu
{
    [TestFixture]
    public class DirectoryEntriesGetAllHandlerTests
    {
        [Test]
        public void Should_Return_Filtered_Directory_Results()
        {
            var repository = new MemoryRepository<DirectoryEntry>(new [] {"keith", "kevin", "kiner"}.
                                        Select(x => new DirectoryEntry {Name = x }));
            var handler = new MultiplePublicGetHandler(repository);
            
            var viewModel = handler.Execute(new GetAllRequest { Query = "ke" });

            viewModel.Count.ShouldEqual(2);
            viewModel.Any(x => x.name == "keith").ShouldBeTrue();
            viewModel.Any(x => x.name == "kevin").ShouldBeTrue();
        }

        [Test]
        public void Should_Return_First_Page_By_Default()
        {
            var repository = new MemoryRepository<DirectoryEntry>(Enumerable.Range(1, 30).
                            Select(x => new DirectoryEntry { Name = x.ToString("00") }));
            var handler = new MultiplePublicGetHandler(repository);

            var viewModel = handler.Execute(new GetAllRequest());

            viewModel.Count.ShouldEqual(MultiplePublicGetHandler.PageSize);
            viewModel.Min(x => int.Parse(x.name)).ShouldEqual(1);
            viewModel.Max(x => int.Parse(x.name)).ShouldEqual(20);
        }

        [Test]
        public void Should_Return_First_Page()
        {
            var repository = new MemoryRepository<DirectoryEntry>(Enumerable.Range(1, 30).
                            Select(x => new DirectoryEntry { Name = x.ToString("00") }));
            var handler = new MultiplePublicGetHandler(repository);

            var viewModel = handler.Execute(new GetAllRequest {Index = 1});

            viewModel.Count.ShouldEqual(MultiplePublicGetHandler.PageSize);
            viewModel.Min(x => int.Parse(x.name)).ShouldEqual(1);
            viewModel.Max(x => int.Parse(x.name)).ShouldEqual(20);
        }

        [Test]
        public void Should_Return_Second_Page()
        {
            var repository = new MemoryRepository<DirectoryEntry>(Enumerable.Range(1, 26).
                            Select(x => new DirectoryEntry { Name = x.ToString("00") }));
            var handler = new MultiplePublicGetHandler(repository);

            var viewModel = handler.Execute(new GetAllRequest { Index = 2 });

            viewModel.Count.ShouldEqual(6);
            viewModel.Min(x => int.Parse(x.name)).ShouldEqual(21);
            viewModel.Max(x => int.Parse(x.name)).ShouldEqual(26);
        }
    }
}