using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;
using FubuMvc.Behaviors;

namespace FubuMvc.Directory.Entries
{
    public class GetAllRequest
    {
        public string Query { get; set; }
        public int Index { get; set; }
    }

    public class PublicGetAllHandler
    {
        public const int PageSize = 20;
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public PublicGetAllHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        [OverrideTransactionScope]
        public List<EntryModel> Execute(GetAllRequest request)
        {
            switch (request.Query)
            {
                case "blowup": throw new Exception("Bad things happening!");
                case "secure": throw new AuthorizationException();
                case "invalid": throw new ValidationException("The search text you entered is invalid.");
            }
            if (request.Query == "slow")
            {
                Thread.Sleep(2000);
                request.Query = "";
            }
            return Mapper.Map<List<EntryModel>>(
                _directoryRepository.
                        OrderBy(x => x.Name).
                        Where(x => request.Query == null || x.Name.StartsWith(request.Query, true, CultureInfo.InvariantCulture)).
                        Skip((Math.Max(request.Index, 1) - 1) * PageSize).
                        Take(PageSize).
                        ToList());
        }
    }
}