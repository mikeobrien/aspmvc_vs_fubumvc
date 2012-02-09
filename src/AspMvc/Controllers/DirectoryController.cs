using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AspMvc.ActionFilters;
using AspMvc.Models;
using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Data;

namespace AspMvc.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly IRepository<DirectoryEntry> _directoryRepository;

        public DirectoryController(IRepository<DirectoryEntry> directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        [Public]
        public ActionResult Index()
        {
            if (Request.UserAgent.Contains("MSIE")) throw new Exception("IE broke me!!!");
            return View(new ViewModel
            {
                IsChromeBrowser = Request.UserAgent.Contains("Chrome"),
                Results = _directoryRepository.
                                OrderBy(x => x.Name).
                                Take(20).
                                Select(Mapper.Map<EntryModel>).
                                ToList()
            });
        }

        [Public]
        [HttpGet]
        [OverrideTransactionScope]
        public JsonResult Entries(string query)
        {
            return Json(_directoryRepository.
                        OrderBy(x => x.Name).
                        Where(x => x.Name.StartsWith(query, true, CultureInfo.InvariantCulture)).
                        Take(20).
                        Select(Mapper.Map<EntryModel>).
                        ToList(), JsonRequestBehavior.AllowGet);
        }

        // Were breaking our RESTful urls here so we'll have to deal with this in MVC or in backbone
        [Public]
        [HttpGet]
        public JsonResult EntriesGet(Guid id)
        {
            return Json(Mapper.Map<EntryModel>(_directoryRepository.Get(id)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Entries(EntryModel request)
        {
            return Json(Mapper.Map<EntryModel>(_directoryRepository.Add(Mapper.Map<DirectoryEntry>(request))));
        }

        [HttpPut]
        public void Entries(Guid id, EntryModel model)
        {
            _directoryRepository.Modify(Mapper.Map<DirectoryEntry>(model));
        }

        [HttpDelete]
        public void Entries(Guid id)
        {
            _directoryRepository.Delete(id);
        }
    }
}
