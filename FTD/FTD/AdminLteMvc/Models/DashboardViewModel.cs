using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models
{
    public class DashboardViewModel
    {
        public int CreatedByMe { get; set; }
        public int CreatedByMyDeparment { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }



    }
}