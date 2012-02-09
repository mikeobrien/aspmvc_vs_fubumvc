using AspMvc.Models;
using AutoMapper;
using Core.Domain;
using NUnit.Framework;

namespace Tests.Asp
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