using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class GetRequestModel
    {
        public string Query { get; set; }
    }

    public class GetHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public GetHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public List<EntryModel> Execute(GetRequestModel request)
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