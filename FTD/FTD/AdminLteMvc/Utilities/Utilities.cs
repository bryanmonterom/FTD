using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using AdminLteMvc.Controllers;
using AdminLteMvc.DDL;
using  MailService2;

namespace AdminLteMvc.Utilities
{
    #region Enums
    public enum ErrorsCode
    {
        ItemDoesNotExist,
        ItemAlreadyExists,
        AnErrorOcurred,
        CantDelete,
        NoError
    }


    public enum AttachmentType
    {
        PopularForm = 1,
        RequestFiles=2
    }
    public enum AuditTrailAction
    {
        Insert,
        Update,
        Delete,
        System,
        Error
    }
 
    public enum NotificationType
    {
        error,
        success,
        warning,
        info
    }
    public enum Status
    {
        Approved =1,
        Rejected=2,
        Pending=3,
        Completed = 4,
        Blocked =5,
        OnlyAttachments = 6,
        Canceled =7

    }
    public enum EmailSubject
    {
        Assigned,
        Rejected,
        Pending,
        Approved,
        AlreadyApproved
    }
 

    #endregion

    public static class Utilities 
    {
        static DDL.Data data = new AdminLteMvc.DDL.Data();
        public const int pageSize = 25;

        public enum RejectReasons
        {
            Documentation = 1,
            AmountDiscrepancy = 2,
            ErrorInCurrency = 3,
            NotEnoughFunds = 4
        }

     

        public  static byte[] EncryptStringToBytes_Aes(byte[] data, byte[] vector)
        {
            // Check arguments.
         
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Key = key;
                aesAlg.IV = vector;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


            public static byte[] Decrypt(byte[] data, byte[] vector)
            {
                Data db = new Data();
                iv = vector;
            
            using (var aes = Aes.Create())
                {
                    aes.KeySize = 128;
                    aes.BlockSize = 128;
                    aes.Padding = PaddingMode.Zeros;

                    aes.Key = key;
                    aes.IV = vector;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        return PerformCryptography(data, decryptor);
                    }
                }
            }

            private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
            {
                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();

                    return ms.ToArray();
                }
            }

       internal static byte[] key = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        internal static byte[] iv = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


       
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameter"></param>
        /// <param name="Message"></param>
        /// <param name="user"></param>
        /// <param name="action"></param>
        public static void PrepareAuditTrail( string Message, string user = "System",AuditTrailAction action = AuditTrailAction.System)
        {
            AuditTrail adt = new AuditTrail();
                    adt = new AuditTrail
                    {
                        Action = ActionToString(action),
                        Date = DateTime.Now,
                        PerformedBy = user,
                        ActionMessage =  Message
                    };
            data.Add(adt);
        }

        public static string ActionToString(AuditTrailAction Action)
        {
            switch (Action)
            {
                case AuditTrailAction.Insert:
                    return "I";
                case AuditTrailAction.Update:
                    return "U";
                case AuditTrailAction.Delete:
                    return "D";
                case AuditTrailAction.System:
                    return "S";
                default:
                    return "E";
            }
        }

        public static string ActionToMessage(AuditTrailAction Action)
        {
            switch (Action)
            {
                case AuditTrailAction.Insert:
                    return "I";
                case AuditTrailAction.Update:
                    return "U";
                case AuditTrailAction.Delete:
                    return "D";
                case AuditTrailAction.System:
                    return "S";
                default:
                    return "E";
            }
        }

        public static byte[] VectorGenerator()
        {
            Random random = new Random();
            byte[] bytes = new byte[16];
            random.NextBytes(bytes);
            return bytes;
        }


        public static string ActionToString(EmailSubject subject)
        {
            switch (subject)
            {
                case EmailSubject.Assigned:
                    return "Se te ha asignado una nueva solicitud";
                case EmailSubject.Rejected:
                    return "An Advance Payment request submitted by you has been Rejected";
                case EmailSubject.Pending:
                    return "You have some Advance Payment Request waiting for your approval";
                case EmailSubject.Approved:
                    return "La solicitud sometida por ti ha completado el proceso de aprobaciones";
                default:
                    return "Tu solicitud ha sido: ";
            }
        }

        public static string SubjectToStatus(EmailSubject subject)
        {
            switch (subject)
            {
                case EmailSubject.Assigned:
                    return "Se te ha asignado una nueva solicitud";
                case EmailSubject.Rejected:
                    return "An Advance Payment request submitted by you has been Rejected";
                case EmailSubject.Pending:
                    return "You have pending approvals";
                case EmailSubject.Approved:
                    return "La solicitud sometida por ti ha completado el proceso de aprobaciones";
                case EmailSubject.AlreadyApproved:
                    return "Se ha aprobado una solicitud relacionada contigo";
                default:
                    return "La solicitud sometida por ti ha completado el proceso de aprobaciones";
            }
        }

        public static string ErrorHandling(string model ="", string message="", string parameterName="" , ErrorsCode error = ErrorsCode.NoError )
        {
            string ErrorMessage = "";
            switch (error)
            {
                case ErrorsCode.ItemAlreadyExists:
                    ErrorMessage = string.Format("Ya existe un {0} con est@ {1} {2} ", model, parameterName, message);
                    break;
                case ErrorsCode.AnErrorOcurred:
                    ErrorMessage = string.Format("Ha ocurrido un error {0}, comunicate con el departamento de IT", message);
                    break;
                case ErrorsCode.CantDelete:
                    ErrorMessage = string.Format("No puedes eliminar este {0}, existe algunos otros vinculados al mismo", model);
                    break;
                default:
                    ErrorMessage = string.Format("La operacion se ha completado con exito!");
                    break;
            }
            return ErrorMessage;
        }

        public static string BuildMailDetails(string identifier, string requestor,  string type, string dateAssigned,  string action)
        {
            
            string detail = string.Format("<tr><td> {0} </td><td> {1} </td><td> {2} </td><td> {3} </td><td> {4} </td></tr>", identifier,requestor,type, dateAssigned, action);
            return detail;
        }

        public static string BuildMailDetailsForCompleted(string identifier, string requestor, string buyer, string vendor, string type, string dateAssigned)
        {

            string detail = string.Format("<tr><td> {0} </td><td> {1} </td><td> {2} </td><td> {3} </td><td> {4} </td><td> {5} </td></tr>", identifier, requestor, buyer, vendor, type, dateAssigned);
            return detail;
        }


        public static void MailSender(Employee employee, int idPost, EmailSubject emailSubject)
        {
            FtdContext db = new FtdContext();
            Request post = db.Request.Include("FlowType").Include("Requestor").FirstOrDefault(a => a.Id == idPost);
            var step = db.FlowSteps.Find(post.IdFlowSteps);
            string detail = "";
            string status = SubjectToStatus(emailSubject);
            string subject = ActionToString(emailSubject);
            string action = step.StepName;
            try
            {
                detail = BuildMailDetails(post.Identifier, post.Requestor.Name, post.FlowType.Name, post.Date.ToShortDateString(), post.FlowSteps.StepName);
                string template = System.IO.File.ReadAllText
                    (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\Template\\Assigned.tpl").LocalPath);
                template = template.Replace("[%Approver]", employee.Name);
                template = template.Replace("[%TRS]", detail);
                template = template.Replace("[%ActionToBeTaken]", action);

               var mail=  MailService2.Services.MailSender(subject,template,employee.Email);
               if (mail.IsSuccess == false)
               {
                   PrepareAuditTrail(mail.Message);
                }


            }
            catch (Exception e)
            {
                PrepareAuditTrail(e.Message);

            }
        }

        public static void MailSenderCompleted(Employee employee, int idPost, EmailSubject emailSubject)
        {
            FtdContext db = new FtdContext();
            Request post = db.Request.Include("FlowType").Include("Requestor").FirstOrDefault(a => a.Id == idPost);
            employee = db.Employee.Where(a => a.Id == post.IdRequestor).FirstOrDefault();
            var step = db.FlowSteps.Find(post.IdFlowSteps);
            string detail = "";
            string status = SubjectToStatus(emailSubject);
            string subject = ActionToString(emailSubject);
            string action = step.StepName;
            try
            {
                detail = BuildMailDetails(post.Identifier, post.Requestor.Name, post.FlowType.Name, post.Date.ToShortDateString(), post.FlowSteps.StepName);
                string template = System.IO.File.ReadAllText
                    (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\Template\\Completed.tpl").LocalPath);
                template = template.Replace("[%Approver]", employee.Name);
                template = template.Replace("[%TRS]", detail);
                template = template.Replace("[%ActionToBeTaken]", action);

                var mail = MailService2.Services.MailSender(subject, template, employee.Email);
                if (mail.IsSuccess == false)
                {
                    PrepareAuditTrail(mail.Message);
                }


            }
            catch (Exception e)
            {
                PrepareAuditTrail(e.Message);

            }
        }

        public static string ActionToString(Status status)
        {
            switch (status)
            {
                case Status.Approved:
                    return "Aprobada";
                case Status.Rejected:
                    return "Rechazada";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
        public static void MailSender(Employee employee, int idPost, EmailSubject emailSubject, Status requestStatus,
            int currentStep)
        {
            FtdContext db = new FtdContext();
            Request post = db.Request.Include("FlowType").Include("Requestor").FirstOrDefault(a => a.Id == idPost);
            var step = db.FlowSteps.Find(post.IdFlowSteps);
            string detail = "";
            string status = SubjectToStatus(emailSubject);
            string Approved = ActionToString(requestStatus);
            string subject = ActionToString(emailSubject) + " "+Approved;
            string action = step.StepName;


            try
            {
                detail = BuildMailDetails(post.Identifier, post.Requestor.Name, post.FlowType.Name, post.Date.ToShortDateString(), post.FlowSteps.StepName);
                string template = System.IO.File.ReadAllText
                    (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\Template\\Approved.tpl").LocalPath);
                template = template.Replace("[%Approver]", employee.Name);
                template = template.Replace("[%TRS]", detail);
                template = template.Replace("[%ActionToBeTaken]", action);
                template = template.Replace("[%Status]", Approved);


                var mail = MailService2.Services.MailSender(subject, template, employee.Email);
                if (mail.IsSuccess == false)
                {
                    PrepareAuditTrail(mail.Message);
                }


            }
            catch (Exception e)
            {
                PrepareAuditTrail(e.Message);

            }
        }


            public static Results SetResults(bool isSuccess =true, string message ="Transaccion completada con exito", int parameter = 0)
        {
            Results results = new Results()
            {

                IsSuccess = isSuccess,
                Message = message,
                Parameter = parameter
            };

            return results;
        }

        public static SelectListItem FirstItem()
        {
            var firstItem = new SelectListItem
            {
                Text = "Selecciona una opcion",
                Value = null
            };
            return firstItem;
        }

        public static SelectList GetActions()
        {
            var fist = new SelectListItem
            {
                Text = "Selecciona una opcion",
                Value = null
            };
            var firstItem = new SelectListItem
            {
                Text = "Insercion",
                Value = "I"
            };
            var second = new SelectListItem
            {
                Text = "Actualizacion",
                Value = "U"
            };
            var third = new SelectListItem
            {
                Text = "Eliminacion",
                Value = "D"
            };
            var system = new SelectListItem
            {
                Text = "Sistema",
                Value = "S"
            };

            List<SelectListItem> a = new List<SelectListItem>();
            a.Insert(0, fist);
            a.Insert(1, firstItem);
            a.Insert(2, second);
            a.Insert(3, third);
            a.Insert(4, system);

            SelectList s1 = new SelectList(a, "Value", "Text");
            return s1;
        }


        public static SelectList Approvals()
        {

            var firstItem = new SelectListItem
            {
                Text = "Select an Item",
                Value = null
            };

            var secondItem = new SelectListItem
            {
                Text = "Mis aprobaciones",
                Value = "0"
            };
            var thirdItem = new SelectListItem
            {
                Text = "Aprobaciones de mi grupos",
                Value = "1"
            };

            List<SelectListItem> a = new List<SelectListItem>();
            a.Insert(0, firstItem);
            a.Insert(1, secondItem);
            a.Insert(2, thirdItem);
            SelectList s1 = new SelectList(a, "Value", "Text");
            return s1;



        }

        public static SelectList GetDepartments()
        {
            var status = data.GetDepartments().OrderBy(a=> a.Name).
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }


        public static SelectList GetStatus()
        {
            var status = data.GetStatus().
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }

        public static SelectList GetRoles()
        {
            var status = data.GetRoles().
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }

        public static SelectList GetFlowTypes()
        {
            var status = data.GetFlowTypes().
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }
        public static SelectList GetRejectReasons()
        {
            var status = data.GetRejectReasons().
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }


        public static SelectList GetEmployees()
        {
            var status = data.GetEmployees().
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }
        public static SelectList GetGroups()
        {
            var status = data.GetGroups().OrderBy(a=> a.Name).
                Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }).ToList();

            status.Insert(0, FirstItem());
            SelectList s1 = new SelectList(status, "Value", "Text");
            return s1;

        }

        #endregion
    }
}