using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;
using TCM.ViewModels;

namespace TCM.Controllers.Api
{
    [Route("/api/testcases")]
    public class TestCasesController : Controller
    {
        private ILogger<TestCasesController> _logger;
        private ITCMRepository _repository;

        public TestCasesController(ITCMRepository repository, ILogger<TestCasesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllTestCases();
                //here we will return a collection of trips for read-only, so IEnumerable type is fine
                //mapper will convert this collection of test cases into collection of test case view models
                return Ok(Mapper.Map<IEnumerable<TestCaseViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get test cases: {0}", ex);
            }
            return BadRequest("Error occured.");
        }

        [HttpPost("")]
        //to make action asynchronous - wrap the return type with async Task <>
        public async Task<IActionResult> Post([FromBody]TestCaseViewModel testcase)
        {
            if (ModelState.IsValid)
            {
                var newTestCase = Mapper.Map<TestCase>(testcase);
                _repository.AddTestCase(newTestCase);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/testcases/{testcase.Name}", Mapper.Map<TestCaseViewModel>(newTestCase));
                }              
            }
            return BadRequest("Failed to save test case.");
        }
    }
}
