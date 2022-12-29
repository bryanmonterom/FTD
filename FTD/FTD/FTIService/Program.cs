using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTIService
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities utilities = new Utilities();

            utilities.PostPendings(AdminLteMvc.Utilities.Status.Pending);
        }
    }
}
