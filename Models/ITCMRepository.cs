using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TCM.Models;

namespace TCM.Models
{
    public interface ITCMRepository
    {
        IEnumerable<TestCase> GetAllTestCases();

        TestCase GetTestCaseById(int testCaseId);

        void AddTestCase(TestCase testCase);
        void AddStep(int testCaseId, Step newStep);

        Task<bool> SaveChangesAsync();

    }
}