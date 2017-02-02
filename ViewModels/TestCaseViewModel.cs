using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;

namespace TCM.ViewModels
{
    public class TestCaseViewModel
    {
        [Required]
        [StringLength (100, MinimumLength = 5)]
        public string Name { get; set; }
        public string Preconditions { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        

    }
}
