using AutoMapper;
using Bottles;
using Core.Domain;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuMvc.Directory.Entries;
using FubuMvc.app_start;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStartFubuMVC), "Start")]

namespace FubuMvc.app_start
{
    public static class AppStartFubuMVC
    {
        public static void Start()
        {
            FubuApplication.For<Conventions>()
                .StructureMap(new Container(new Registry()))
                .Bootstrap();
			PackageRegistry.AssertNoFailures();

            Mapper.CreateMap<DirectoryEntry, EntryModel>();
            Mapper.CreateMap<EntryModel, DirectoryEntry>();
        }
    }
}