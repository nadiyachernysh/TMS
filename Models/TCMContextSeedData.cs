using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;

namespace TCM.Models
{
    public class TCMContextSeedData
    {
        private TCMContext _context;
        private UserManager<TCMUser> _usermanager;

        public TCMContextSeedData(TCMContext context, UserManager<TCMUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        public async Task EnsureSeedData()
        {
            if(await _usermanager.FindByEmailAsync("nadiyachernysh@gmail.com") == null)
            {
                var user = new TCMUser()
                {
                    UserName = "nadiya",
                    Email = "nadiyachernysh@gmail.com"
                };

                await _usermanager.CreateAsync(user, "P@ssw0rd!");
            }
            
            //method .Any returns bool telling if there are any elements in database
            if (!_context.TestCases.Any())
            {
                //creating an instance of the testcase
                var testCase = new TestCase()
                {
                    Name = "Sample test case.",
                    Preconditions = "Preconditions for sample test case.",
                    Steps = new List<Step>
                    {
                        new Step() {Order = 0, Name = "First step for first test case.", Result = "Expected to pass.", Status = "Passed" },
                        new Step() {Order = 1, Name = "Second step for first test case.", Result = "Expected to pass.", Status = "Passed" },
                    },
                    DateCreated = DateTime.UtcNow,
                    UserName = "nadiya",
                };
                //adding testcase to the context
                _context.TestCases.Add(testCase);
                //will add steps to separate table
                _context.Steps.AddRange(testCase.Steps);

                var nextTestCase = new TestCase()
                {
                    Name = "Next sample test case.",
                    Preconditions = "Preconditions for next sample test case.",
                    Steps = new List<Step>
                    {
                        new Step() {Order = 0, Name = "First step for second test case.", Result = "Expected to pass.", Status = "Passed" },
                        new Step() {Order = 1, Name = "Second step for second test case.", Result = "Expected to pass.", Status = "Passed" },
                    },
                    DateCreated = DateTime.UtcNow,
                    UserName = "nadiya",
                };

                _context.TestCases.Add(nextTestCase);

                _context.Steps.AddRange(nextTestCase.Steps);

                //and now we are saving all the added changes
                await _context.SaveChangesAsync();
            }
        }
    }
}
