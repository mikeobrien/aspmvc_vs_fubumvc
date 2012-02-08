﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;
using FubuMvc.Directory.Entries;

namespace FubuMvc.Directory
{
    public class GetRequestModel
    {
        public string UserAgent { get; set; }
    }

    public class ViewModel
    {
        public bool IsChromeBrowser { get; set; }
        public List<EntryModel> Results { get; set; }
    }

    public class GetHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public GetHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public ViewModel Execute(GetRequestModel request)
        {
            return new ViewModel
                {
                    IsChromeBrowser = request.UserAgent.Contains("Chrome"),
                    Results = _directoryRepository.
                                    OrderBy(x => x.Name).
                                    Take(20).
                                    Select(Mapper.Map<EntryModel>).
                                    ToList()
                };
        }
    }
}