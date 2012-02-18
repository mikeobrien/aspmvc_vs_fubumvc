using System;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class GetOneRequest
    {
        public Guid id { get; set; }
    }

    public class PublicGetOneHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PublicGetOneHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public EntryModel Execute_id(GetOneRequest request)
        {
            return Mapper.Map<EntryModel>(_directoryRepository.Get(request.id));
        }
    }
}