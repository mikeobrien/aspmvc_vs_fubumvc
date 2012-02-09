using System.Collections.Generic;
using System.Linq;
using AspMvc.Controllers;
using AspMvc.Models;
using Core.Domain;
using NUnit.Framework;
using Should;

namespace Tests.Asp
{
    [TestFixture]
    public class DirectoryEntriesGetAllActionTests
    {
        [Test]
        public void Should_Return_Filtered_Directory_Results()
        {
            var repository = new MemoryRepository<DirectoryEntry>(new [] {"keith", "kevin", "kiner"}.
                                        Select(x => new DirectoryEntry {Name = x }));
            var controller = new DirectoryController(repository);
            
            var result = controller.Entries("ke");

            var data = (IEnumerable<EntryModel>) result.Data;
            data.Count().ShouldEqual(2);
            data.Any(x => x.name == "keith").ShouldBeTrue();
            data.Any(x => x.name == "kevin").ShouldBeTrue();
        }
    }
}