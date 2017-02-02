using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.ViewModels
{
    public class StepViewModel
    {
        [Required]
        public int Order { get; set; }

        [Required]
        [StringLength(4096, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(4096, MinimumLength = 5)]
        public string Result { get; set; }

        public string Status { get; set; }
    }
}
