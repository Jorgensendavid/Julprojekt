using Logic;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Framework
{
    public class CustomUserStore : UserStore<ApplicationUser>
    {
        public CustomUserStore(ApplicationDbContext context) : base(context)
        {

        }
    }
}