using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;

namespace TCM.Controllers.Web
{
    public class TestCaseController : Controller
    {
        private ILogger<TestCaseController> _logger;
        private ITCMRepository _repository;

        public TestCaseController(ITCMRepository repository, ILogger<TestCaseController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestCase model)
        {
            if (ModelState.IsValid)
            {
                _repository.AddTestCase(model);
                ModelState.Clear();

                if(await _repository.SaveChangesAsync())
                {
                    ViewBag.UserMessage = "Test Case was created";
                    return View();
                }                  
                
            }
            ViewBag.UserMessage = "Something went wrong. Please try again.";
            return View();

        }
    }
}
