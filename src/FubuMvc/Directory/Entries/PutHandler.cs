using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class PutHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PutHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public void Execute_id(EntryModel request)
        {
            _directoryRepository.Modify(Mapper.Map<DirectoryEntry>(request));
        }
    }
}