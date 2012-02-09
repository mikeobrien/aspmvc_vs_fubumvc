using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;
using FubuMvc.Behaviors;

namespace FubuMvc.Directory.Entries
{
    public class GetAllRequest
    {
        public string Query { get; set; }
    }

    public class PublicGetAllHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PublicGetAllHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        [OverrideTransactionScope]
        public List<EntryModel> Execute(GetAllRequest request)
        {
            return _directoryRepository.
                        OrderBy(x => x.Name).
                        Where(x => x.Name.StartsWith(request.Query, true, CultureInfo.InvariantCulture)).
                        Take(20).
                        Select(Mapper.Map<EntryModel>).
                        ToList();
        }
    }
}