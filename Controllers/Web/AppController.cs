using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TCM.Models;
using TCM.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TCM.Controllers
{
    public class AppController : Controller
    {
        private ITCMRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(ITCMRepository repository, ILogger<AppController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult TestCases()
        {
            //context will convert this line into query to database
            var data = _repository.GetAllTestCases();
            return View(data);
        }

        public IActionResult Create()
        {
            //ViewData["Message"] = "Test Case Creation form is going to be here.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCase model)
        {
            if (ModelState.IsValid)
            {
                //var newTestCase = Mapper.Map<TestCase>(vm);
                _repository.AddTestCase(model);
                ModelState.Clear();

                if (await _repository.SaveChangesAsync())
                {
                    ViewBag.UserMessage = "Test Case was created";
                    //return Redirect ($"/{model.Name}/addsteps");
                    return RedirectToAction("Edit", new { id = model.Id });
                }

            }
            ViewBag.UserMessage = "Something went wrong. Please try again.";
            return View();

        }

        //[HttpGet("/{testCaseId}")]
        public IActionResult Edit(int Id)
        {
            TestCase testCase = _repository.GetTestCaseById(Id);
            return View(testCase);
        }

        [HttpGet]
        public IActionResult AddSteps(int testCaseId)
        {
            TestCase testCase = _repository.GetTestCaseById(testCaseId);
            
            return View();
        }

        [HttpPost]
        public IActionResult AddSteps(int testCaseId, Step model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.AddStep(testCaseId, model);
                    return RedirectToAction("Details", new { id = testCaseId });
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to add step: {0}", ex);
                    
                }
            }
            return View();
        }

        [HttpGet]
        //  Route("/{testCaseId}")
        public IActionResult Details(int testCaseId)
        {
            //testCaseId = 1;
            var testCase = _repository.GetTestCaseById(testCaseId);
            return View(testCase);
        }
    }
}
