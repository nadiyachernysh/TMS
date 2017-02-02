using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.Models
{
    public class TCMUser : IdentityUser
    {
        public string Position { get; set; }
    }
}
