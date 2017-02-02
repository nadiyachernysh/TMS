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
    [Route("/api/testcases/{testCaseName}/steps")]
    public class StepsController : Controller
    {
        private ILogger<StepsController> _logger;
        private ITCMRepository _repository;

        public StepsController(ITCMRepository repository, ILogger<StepsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get(int testCaseId)
        {
            try
            {
                var testCase = _repository.GetTestCaseById(testCaseId);

                //need to map the collections of steps to the collection of step view models, so we will not return raw step
                return Ok(Mapper.Map<IEnumerable<StepViewModel>>(testCase.Steps.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get steps: {0}", ex);
            }
            return BadRequest("Failer to get steps");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(int testCaseId, [FromBody]StepViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStep = Mapper.Map<Step>(vm);
                    _repository.AddStep(testCaseId, newStep);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/testcases/{testCaseId}/steps/{newStep.Order}", Mapper.Map<StepViewModel>(newStep));
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new step: {0}", ex);
            }
            return BadRequest("Failed to save new step");
        }
    }
}
