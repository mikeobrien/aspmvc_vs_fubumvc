using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;
using FubuMvc.Directory.Entries;

namespace FubuMvc.Directory
{
    public class GetRequest
    {
        public string UserAgent { get; set; }
    }

    public class ViewModel
    {
        public bool IsChromeBrowser { get; set; }
        public List<EntryModel> Results { get; set; }
    }

    public class PublicGetHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PublicGetHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public ViewModel Execute(GetRequest request)
        {
            if (request.UserAgent.Contains("MSIE")) throw new Exception("IE broke me!!!");
            return new ViewModel
                {
                    IsChromeBrowser = request.UserAgent.Contains("Chrome"),
                    Results = _directoryRepository.
                                    OrderBy(x => x.Name).
                                    Take(PublicGetAllHandler.PageSize).
                                    Select(Mapper.Map<EntryModel>).
                                    ToList()
                };
        }
    }
}