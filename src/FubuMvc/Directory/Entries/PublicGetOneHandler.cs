using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class PublicGetOneHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PublicGetOneHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public EntryModel Execute_id(EntryModel request)
        {
            return Mapper.Map<EntryModel>(_directoryRepository.Get(request.id));
        }
    }
}