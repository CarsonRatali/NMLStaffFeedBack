using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NMLStaffFeedback.Startup))]

namespace NMLStaffFeedback
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // return RedirectToAction("Index", "Feedback");
            //   ConfigureAuth(app);
        }
    }
}
