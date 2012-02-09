using AutoMapper;
using Core.Domain;
using FubuMvc.Directory.Entries;
using NUnit.Framework;

namespace Tests.Fubu
{
    [SetUpFixture]
    public class SetupFixture
    {
        [SetUp]
        public void Setup()
        {
            Mapper.CreateMap<DirectoryEntry, EntryModel>();
            Mapper.CreateMap<EntryModel, DirectoryEntry>();
        }
    }
}