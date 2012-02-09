using System.Linq;
using Core.Domain;
using FubuMvc.Directory.Entries;
using NUnit.Framework;
using Should;

namespace Tests.Fubu
{
    [TestFixture]
    public class DirectoryEntriesGetHandlerTests
    {
        [Test]
        public void Should_Return_Filtered_Directory_Results()
        {
            var repository = new MemoryRepository<DirectoryEntry>(new [] {"keith", "kevin", "kiner"}.
                                        Select(x => new DirectoryEntry {Name = x }));
            var handler = new PublicGetAllHandler(repository);
            
            var viewModel = handler.Execute(new GetAllRequest { Query = "ke" });

            viewModel.Count.ShouldEqual(2);
            viewModel.Any(x => x.name == "keith").ShouldBeTrue();
            viewModel.Any(x => x.name == "kevin").ShouldBeTrue();
        }
    }
}