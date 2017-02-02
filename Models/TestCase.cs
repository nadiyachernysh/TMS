using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.Models
{
    public class TestCase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Preconditions { get; set; }

        //Entity Framework will know about this relationship and will create a foreign key TestCaseId
        public ICollection<Step> Steps { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;


    }
}
