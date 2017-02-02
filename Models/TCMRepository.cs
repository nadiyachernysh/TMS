using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;

namespace TCM.Models
{
    public class TCMRepository : ITCMRepository
    {
        private TCMContext _contex;

        public TCMRepository (TCMContext context)
        {
            _contex = context;
        }

        public void AddStep(int testCaseId, Step newStep)
        {
            var testCase = GetTestCaseById(testCaseId);

            if(testCase != null)
            {
                //here the foreign key is being set
                testCase.Steps.Add(newStep);
                //here we are actually adding new step to context
                _contex.Steps.Add(newStep);
                //we need both these methods to happen for the step to be related entity and to be saved correctly
            }
        }

        public void AddTestCase(TestCase testCase)
        {
            //this line is pushing it in the context as new object
            _contex.Add(testCase);
        }

        public IEnumerable<TestCase> GetAllTestCases()
        {
            return _contex.TestCases.ToList();
        }

        public TestCase GetTestCaseById(int testCaseId)
        {
            return _contex.TestCases
                //specifies related entities to include in a query result. but why t=>? if we needed more then one property - we can add more includes
                .Include(t => t.Steps)
                //filters the data based on a predicate
                .Where(t => t.Id == testCaseId)
                //return first value in sequence or default value
                .FirstOrDefault();
        }

        //asynchronous call - task that wraps a bool
        public async Task<bool> SaveChangesAsync()
        {
            //methods SaveChanges and SaveChangesAsync return an interger that represents number of rows affected
            return (await _contex.SaveChangesAsync()) > 0;
        }
    }
}
