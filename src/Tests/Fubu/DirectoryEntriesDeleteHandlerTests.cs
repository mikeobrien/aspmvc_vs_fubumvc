using System;
using System.Linq;
using Core.Domain;
using FubuMvc.Directory;
using FubuMvc.Directory.Entries;
using NUnit.Framework;
using Should;

namespace Tests.Fubu
{
    [TestFixture]
    public class DirectoryEntriesDeleteHandlerTests
    {
        [Test]
        public void Should_Delete_Entry()
        {
            var id = Guid.NewGuid();
            var repository = new MemoryRepository<DirectoryEntry>(new[] { Guid.NewGuid(), id, Guid.NewGuid() }.
                                    Select(x => new DirectoryEntry { Id = x }));
            var handler = new DeleteHandler(repository);
            
            handler.Execute_id(new DeleteRequest { id = id });

            repository.Count().ShouldEqual(2);
            repository.Any(x => x.Id == id).ShouldBeFalse();
        }
    }
}