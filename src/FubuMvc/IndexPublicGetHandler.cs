using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;
using FubuMvc.Directory.Entries;

namespace FubuMvc
{
    public class IndexRequest
    {
        public string UserAgent { get; set; }
    }

    public class IndexResponse
    {
        public bool IsChromeBrowser { get; set; }
        public List<EntryModel> Results { get; set; }
    }

    public class IndexPublicGetHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public IndexPublicGetHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public IndexResponse Execute(IndexRequest request)
        {
            if (request.UserAgent.Contains("MSIE")) throw new Exception("IE broke me!!!");
            return new IndexResponse
                {
                    IsChromeBrowser = request.UserAgent.Contains("Chrome"),
                    Results = _directoryRepository.
                                    OrderBy(x => x.Name).
                                    Take(MultiplePublicGetHandler.PageSize).
                                    Select(Mapper.Map<EntryModel>).
                                    ToList()
                };
        }
    }
}