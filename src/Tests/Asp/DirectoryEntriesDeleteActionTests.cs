using System;
using System.Linq;
using AspMvc.Controllers;
using Core.Domain;
using NUnit.Framework;
using Should;

namespace Tests.Asp
{
    [TestFixture]
    public class DirectoryEntriesDeleteActionTests
    {
        [Test]
        public void Should_Delete_Entry()
        {
            var id = Guid.NewGuid();
            var repository = new MemoryRepository<DirectoryEntry>(new[] { Guid.NewGuid(), id, Guid.NewGuid() }.
                                    Select(x => new DirectoryEntry { Id = x }));
            var controller = new DirectoryController(repository);
            
            // This is delete? Hard to tell isn't it...
            controller.Entries(id);

            repository.Count().ShouldEqual(2);
            repository.Any(x => x.Id == id).ShouldBeFalse();
        }
    }
}