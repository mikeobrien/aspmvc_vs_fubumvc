using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class PostHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PostHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public EntryModel Execute(EntryModel request)
        {
            return Mapper.Map<EntryModel>(_directoryRepository.Add(Mapper.Map<DirectoryEntry>(request)));
        }
    }
}