using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;

namespace FTIService
{
    class Utilities
    {
        Entities1  db = new Entities1();
        AdminLteMvc.Models.Employee employee = new AdminLteMvc.Models.Employee();
        FtdContext db1 = new FtdContext();
        public void PostPendings(AdminLteMvc.Utilities.Status status)
        {
        
            RequestHistories item1 = new RequestHistories();

            try
            {
                var query = db.RequestHistories.Where(a => a.IdStatus == (int)status).ToList();

                foreach (var item in query)
                {
                    employee = db1.Employee.FirstOrDefault(a => a.Id == item.IdEmployee);
                    item1 = item;
                    AdminLteMvc.Utilities.Utilities.MailSender(employee, item.IdRequest, EmailSubject.Pending);
                    AdminLteMvc.Utilities.Utilities.PrepareAuditTrail("Correo enviado a: " + employee.Identification + " relacionado a: " + item.Requests.Identifier, "System", AuditTrailAction.System);
                }
            }
            catch (Exception e)
            {
                AdminLteMvc.Utilities.Utilities.PrepareAuditTrail(e.Message + "Inner: " + e.InnerException.Message, "System", AuditTrailAction.System);
                AdminLteMvc.Utilities.Utilities.PrepareAuditTrail("Fallo el envio del correo  a: " + employee.Identification + "relacionado a: " + item1.IdRequest + "Inner: " + e.InnerException.Message, "System", AuditTrailAction.System);
                Console.WriteLine();
            }

        }
    }
}
