using System;
using Core.Domain;
using Core.Infrastructure.Data;

namespace FubuMvc.Directory.Entries
{
    public class DeleteRequestModel
    {
        public Guid Id { get; set; }
    }

    public class DeleteHandler
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public DeleteHandler(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public void Execute_Id(DeleteRequestModel request)
        {
            _directoryRepository.Delete(request.Id);
        }
    }
}