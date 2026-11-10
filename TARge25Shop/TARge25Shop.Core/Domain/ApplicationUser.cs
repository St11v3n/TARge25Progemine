using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TARge25Shop.Core.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public string City { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
