using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Controllers;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using AttachmentType = AdminLteMvc.Utilities.AttachmentType;
using Status = AdminLteMvc.Utilities.Status;
using MailService2;
namespace AdminLteMvc.DDL
{
    public class Data
    {
        FtdContext db = new FtdContext();
        FTDEntities context = new FTDEntities();

        Results results = new Results();
        public bool ValidateIfDirector(int idEmployee)
        {
            var any = db.Department.Any(a => a.IdDirector == idEmployee);
            return any;
        }

        public List<EmployeeGroups> GetGroupMembers(int idGroup)
        {
            var query = db.EmployeeGroups.Where(a => a.IdGroup == idGroup).ToList();
            return query;
        }

        public DashboardViewModel GetDashboardViewModel(string username)
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            var employee = GetEmployee(username);
            var request = db.Request.Include(a=> a.Requestor).Include(a=> a.From).ToList();
            dashboardViewModel.CreatedByMe = request.Count(a => a.Requestor.Identification == username);
            dashboardViewModel.CreatedByMyDeparment = request.Count(a => a.IdFrom == employee.IdDepartment);
            dashboardViewModel.Completed = request.Count(a =>
                a.IdFrom == employee.IdDepartment && a.IdStatus == (int) Utilities.Status.Approved);
            dashboardViewModel.Pending = request.Count(a =>
                a.IdFrom == employee.IdDepartment && (a.IdStatus == (int)Utilities.Status.Pending || a.IdStatus == (int)Utilities.Status.Blocked || a.IdStatus == (int)Utilities.Status.OnlyAttachments));
            return dashboardViewModel;

        }

        public bool ValidateIfDirector(int idEmployee, int idDepartment)
        {
            var any = db.Department.Any(a => a.IdDirector == idEmployee && a.Id != idDepartment);
            return any;
        }
        public void SendEmailToGroup(int idGroup, int idPost, EmailSubject emailSubject, Status requestStatus, int currentStep)
        {
            var post = GetRequest(idPost);
            //var query = db.EmployeeGroups.Include(a => a.Employee).Where(a => a.IdGroup == idGroup);

            var query = db.EmployeeGroups.Include(a => a.Employee).Where(a => a.IdGroup == idGroup && a.Employee.IdDepartment == post.IdFrom);
            foreach (var item in query)
            {
                Utilities.Utilities.MailSender(item.Employee, idPost, EmailSubject.AlreadyApproved, requestStatus, currentStep);
            }
        }
        public Results Update(AspNetUsers aspNetUser)
        {

            var exists = GetEmployee(aspNetUser.UserName);
            var user = context.AspNetUsers.FirstOrDefault(a => a.UserName == aspNetUser.UserName && a.Id != aspNetUser.Id);

            if (exists == null)
            {
                return Utilities.Utilities.SetResults(false, "El usuario provisto no pertenece a ningun empleado");
            }
            if (user != null)
            {
                return Utilities.Utilities.SetResults(false, "Este empleado ya posee un usuario registrado");
            }

            try
            {
                var AspUser = context.AspNetUsers.AsNoTracking().FirstOrDefault(a => a.Id == aspNetUser.Id);
                context.Entry(aspNetUser).State = EntityState.Modified;
                aspNetUser.Email = aspNetUser.Email;
                aspNetUser.UserName = aspNetUser.UserName;
                aspNetUser.PasswordHash = AspUser.PasswordHash;
                context.SaveChanges();
                return Utilities.Utilities.SetResults(true);

            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }

        public bool IsSecretary(string username)
        {
            var query = GetEmployee(username).IsSupervisor;
            if (query)
            {
                return false;
            }
            return true;
        }

        public List<Request> RequestFilter(int? idStatus, string identifier, string requestor, string employee)
        {
            var employeeToFilter = GetEmployee(employee);
            var query = db.Request.Include("FlowSteps").Include("FlowType").Include(a => a.Requestor).Include("Status").Include(a=> a.From).Include(a=> a.To).Where(a =>
                a.Identifier.Contains(identifier == "" ? a.Identifier : identifier)
                && a.Requestor.Name.Contains(requestor == "" ? a.Requestor.Name : requestor)
                && a.IdStatus == (idStatus == null ? a.IdStatus : idStatus)
                && a.IdFrom == employeeToFilter.IdDepartment ).ToList();
            return query;
        }

        public List<Request> RequestFilter(int? idStatus, string identifier, string requestor)
        {
            var query = db.Request.Include("FlowSteps").Include("FlowType").Include(a => a.Requestor).Include("Status").Where(a =>
                a.Identifier.Contains(identifier == "" ? a.Identifier : identifier)
                && a.Requestor.Name.Contains(requestor == "" ? a.Requestor.Name : requestor)
                && a.IdStatus == (idStatus == null ? a.IdStatus : idStatus)).ToList();
            return query;
        }

        public List<Models.AttachmentType> GetAttachmentTypes()
        {
            var query = db.AttachmentType.OrderBy(a => a.Name).ToList();
            return query;
        }

        public Attachments GetFiles(int idPost, int? type)
        {
            var query = db.Attachments.FirstOrDefault(a => a.IdRequest == idPost && a.IdAttachmentType == type);
            return query;
        }

        public List<FlowSteps> FlowStepsFilter(string employee, int? flowType)
        {
            var query = db.FlowSteps.Include(a => a.Employee).Include(a => a.FlowType).Include(a=> a.Group).Where(a =>
                a.IdFlowType == (flowType == null ? a.IdFlowType : flowType)
                && a.Employee.Name.Contains(employee == "" ? a.Employee.Name : employee)).ToList();
            return query;
        }


        public List<Employee> EmployeeFilter(string card = "", string name = "")
        {
            var query = db.Employee
                .Where(a => a.Identification.Contains(card == string.Empty ? a.Identification : card) &&
                            a.Name.Contains(name == string.Empty ? a.Name : name)).ToList();
            return query;
        }


        public List<RequestHistory> GetRequestHistoryForFlow(int id)
        {
            var query = db.RequestHistory.Include(a=> a.Employee).Include(a=> a.FlowSteps).Include(a=> a.Status).Where(a => a.IdRequest == id).ToList();
            return query;
        }

        public List<UserViewModels> UsersFilter(string username = "", string role = "")
        {

            var users = context.AspNetUsers.Include(a => a.AspNetRoles)
                .Where(a => a.UserName.Contains(username == "" ? a.UserName : username)
                            && a.AspNetRoles.FirstOrDefault().Id ==
                            (role == "" ? a.AspNetRoles.FirstOrDefault().Id : role)).ToList();

            var employee = db.Employee.ToList();
            var query = (from user in users
                join empy in employee on user.UserName equals empy.Identification
                select new
                {
                    ID = user.Id,
                    User = user.UserName,
                    Role = user.AspNetRoles,
                    Name = empy.Name
                }).ToList().Select(b => new UserViewModels()
            {
                Id = b.ID,
                Employee = b.Name,
                Email = b.User,
                Role = b.Role,
            }).ToList();

            return query;


        }


        public List<AspNetRoles> GetRoles()
        {
            var query = context.AspNetRoles.OrderBy(A => A.Name).ToList();
            return query;
        }

        public Results Remove(Attachments attachments)
        {

            var query = db.Attachments.Find(attachments.Id);
            try
            {
                db.Attachments.Remove(query);
                db.SaveChanges();
                return Utilities.Utilities.SetResults();
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }

        }

        public Attachments GetAttachment(int id)
        {
            var query = db.Attachments.FirstOrDefault(a => a.Id == id);
            if (query != null)
            {

                query.ByteContent = Utilities.Utilities.Decrypt(query.ByteContent, query.Vector);

            }
            return query;
        }

        public Attachments GetAttachments(int idRequest)
        {
            var query = db.Attachments.FirstOrDefault(a => a.IdRequest == idRequest);
            if (query !=null)
            {

                query.ByteContent = Utilities.Utilities.Decrypt(query.ByteContent, query.Vector);

            }
            return query;
        }
        public Results Save(Attachments files)
        {
            try
            {
                db.Attachments.Add(files);
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }

        public Results EndRequest(RequestHistory requests, string username)
        {
            var results = new Results();
            var employee = GetEmployee(username);
            var request = GetRequest(requests.IdRequest);
            if (request.IsCompleted)
            {
                return Utilities.Utilities.SetResults(false,
                    "Esta solicitud esta marcada como completada, no se puede cancelar");
            }
            var pendingTaks = db.RequestHistory.FirstOrDefault(a => a.IdRequest == requests.IdRequest && a.IdStatus == (int)Status.Pending);
            if (pendingTaks != null)
            {
                results = UpdateRequestHistory(pendingTaks,Utilities.Status.Canceled);
                if (!results.IsSuccess)
                {
                    return Utilities.Utilities.SetResults(results.IsSuccess, results.Message);
                }
            }

            request.IdStatus = (int)Utilities.Status.Canceled;
            request.IsCompleted = true;
            results = Update(request);

            if (!results.IsSuccess)
            {
                return Utilities.Utilities.SetResults(results.IsSuccess, results.Message);
            }
            AddRequestHistoryCancel(requests);

            return Utilities.Utilities.SetResults();
        }


        public Results Update(Request request)
        {
            try
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }


        public Results Update(Attachments files)
            {
            try
            {
                var att = db.Attachments.Where(a => a.Id == files.Id).FirstOrDefault();
                att.ByteContent = files.ByteContent;
                att.ContentType = files.ContentType;
                att.Vector = files.Vector;
                att.Name = files.Name;
                db.Entry(att).State = EntityState.Modified;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }


        public List<Department> GetDepartments()
        {
            var query = db.Department.AsNoTracking().ToList();
            return query;
        }
        public List<Group> GetGroups()
        {
            var query = db.Group.AsNoTracking().OrderBy(a=> a.Name).ToList();
            return query;
        }
        public int GetPendingRequests(string username)
        {
            var query = db.RequestHistory.Include(a =>
                a.Employee).Count(a => a.IdStatus == (int)Status.Pending && a.Employee.Identification == username);
            return query;
        }

        public int GetPendingRequestTotal(string username)
        {
            var query = GetGroupPendingRequests(username) + GetPendingRequests(username);
            return query;
        }


        public List<RequestHistory> PostHistoriesFilter(int? approvals, int? status, string employee, string request)
        {
            var user = GetEmployee(employee);
            List<RequestHistory> query = new List<RequestHistory>();
            switch (approvals)
            {
                case 0:
                    query = db.RequestHistory.Include(a=> a.Request).Include(a=> a.Employee).Include(a=> a.Status)
                        .Include(a=> a.FlowSteps)
                        .Where(a => a.Employee.Identification == employee && a.IdStatus == (status == null ? a.IdStatus : status)
                                                                && a.Request.Identifier.Contains(request == "" ? a.Request.Identifier : request) ).ToList();
                    return query;
                case 1:
                    int g = user.EmployeeGroups.Count();
                    if (g != 0)
                    {
                        var empg = user.EmployeeGroups.ToList();
                        foreach (var x in empg)
                        {
                            var item = db.RequestHistory.Include(a=>a.Request).Include(a=> a.Employee).Include(a=> a.Status)
                                  .Include(a=> a.FlowSteps).FirstOrDefault(a => a.FlowSteps.IdGroup == x.IdGroup &&
                                                                            a.IdStatus == (status == null ? a.IdStatus : status)
                                                                            && a.Request.Identifier.Contains(request == "" ? a.Request.Identifier : request));
                            if (item != null)
                            {
                                query.Add(item);
                            }
                        }
                    }
                    return query;

            }

            return null;

        }

        public List<RequestHistory> PostHistoriesFilter(int? status, string employee, string request)
        {
            var query = db.RequestHistory.Include(a=> a.Request).Include(a=>a.Employee).Include(a=> a.Status)
                .Include(a=> a.FlowSteps)
                .Where(a => a.Employee.Name.Contains(employee == "" ? a.Employee.Name : employee)
                            && a.Request.Identifier.Contains(request == "" ? a.Request.Identifier : request)
                        && a.IdStatus == (status == null ? a.IdStatus : status)).ToList();
            return query;
        }

        public int GetGroupPendingRequests(string username)
        {
            var Groups = db.EmployeeGroups.Include(a => a.Employee).Where(a => a.Employee.Identification == username).ToList();
            int total=0;
            foreach (var item in Groups)
            {
                var query = db.RequestHistory.Include(a =>
                    a.Employee).Include(a => a.FlowSteps).Count(a =>
                    a.IdStatus == (int) Status.Pending && a.FlowSteps.IdGroup == item.IdGroup);

                total += query;
            }
            return total;
        }

        public List<EmployeeGroups> GetEmployeeGroups(int id)
        {
            var query = db.EmployeeGroups.Include(a => a.Employee).Include(a => a.Group).Where(a => a.IdEmployee == id)
                .OrderBy(a => a.Group.Name).ToList();
            return query;
        }


        public List<FlowSteps> GetFlowSteps(int idFlowType)
        {
            var query = db.FlowSteps.Where(a => a.IdFlowType == idFlowType).ToList();
            return query;
        }

        public List<FlowType> GetFlowTypes()
        {
            var query = db.FlowType.AsNoTracking().ToList();
            return query;
        }

        public List<Employee> GetEmployees()
        {
            var query = db.Employee.AsNoTracking().OrderBy(a=> a.Name).ToList();
            return query;
        }

        public Employee GetEmployee(int id)
        {
            var query = db.Employee.Include(a=> a.Department).FirstOrDefault(a => a.Id == id);
            return query;
        }

        public Department GetDeparmentByEmployee(int id)
        {

            var query = db.Employee.Include(a => a.Department).FirstOrDefault(a => a.Id == id).Department;
            return query;
        }

        public Department GetToDepartment(int idFlowType)
        {

            var steps = db.FlowSteps.Where(a=> a.IdFlowType == idFlowType).ToList();
           steps= steps.OrderByDescending(a => a.StepNumber).ToList();
           var employee = steps.FirstOrDefault().IdEmployee;
            var department = GetDeparmentByEmployee((int)employee);
            return department;
        }
        public Results IsCompleted(int idPost)
        {
            var post = GetRequest(idPost);
            if (post.IsCompleted == true)
            {
                return Utilities.Utilities.SetResults(true, "Esta solicitud esta completada, no puedes editarla");
            }

            return Utilities.Utilities.SetResults(false);
        }
        public Results Add(Request request)
        {
            try
            {
                var FlowStep = GetNextStep(request.IdFlowType,request.IdRequestor ,0);
                if (FlowStep == null)
                {
                    return Utilities.Utilities.SetResults(false, "No se encontraron pasos para este flujo");
                }

                FlowStep.IdEmployee = request.IdRequestor;
                request.IsCompleted = false;
                request.Date = DateTime.Now;
                request.FlowSteps = FlowStep;
                request.LastModified = DateTime.Now;
                request.IdStatus = (int)AdminLteMvc.Utilities.Status.Pending;
                request.IdFrom = GetDeparmentByEmployee(request.IdRequestor).Id;
                request.IdTo = GetToDepartment(request.IdFlowType).Id;
                db.Request.Add(request);
                db.SaveChanges();
                db.Entry(request).State = EntityState.Modified;
                request.Identifier = GetEmployee(request.IdRequestor).Department.Identifier + request.Id;
                db.SaveChanges();
                var results = AddRequestHistory(request.Id, request.FlowSteps.IdEmployee, Utilities.Status.Pending);
                Utilities.Utilities.MailSender(FlowStep.Employee, request.Id, EmailSubject.Assigned);
                if (results.IsSuccess == false)
                {
                    db.Request.Remove(request);
                    db.SaveChanges();
                }

                return results.IsSuccess == false ? results : Utilities.Utilities.SetResults(parameter: request.Id);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(isSuccess: false, message:e.Message);
            }

        }
        public Results IsApproved(int id)
        {
            var query = GetRequestHistory(id);
            if (query.IdStatus == (int)Utilities.Status.Pending)
            {
                return Utilities.Utilities.SetResults(false);
            }

            return Utilities.Utilities.SetResults(true, "Esto ya ha sido aprobado, no puedes editarlo.");
        }
        public RequestHistory GetCurrentStep(int id)
        {
            var query = db.RequestHistory.Include(a=> a.Request).Include(a=> a.Employee).Include(a=> a.Status)
                .Where(a => a.IdRequest == id).OrderByDescending(a => a.DateApproval).FirstOrDefault();
            return query;
        }

        public List<Attachments> AttachmentsFilter(int? IdRequest, string identifier ="")
        {
            var query = db.Attachments.Include(a => a.Request).Where(a =>
                a.IdRequest == (IdRequest == 0 ? a.IdRequest : IdRequest)
                && a.Request.Identifier.Contains(identifier == "" ? a.Request.Identifier : identifier)).ToList();
            return query;
        }

        public FlowSteps GetStep(int? idFlowStep)
        {
            var query = db.FlowSteps.Where(a => a.Id == idFlowStep).FirstOrDefault();
            return query;
        }

        public bool ValidateCompletition(int id)
        {
            var requestHistory = GetCurrentStep(id);
            var step = GetStep(requestHistory.Request.IdFlowSteps);
            var attachments = db.Attachments.Where(a => a.IdRequest == requestHistory.Request.Id).ToList();

            if (step.IsAttachmentNeed)
            {
                if (attachments.FirstOrDefault(a => a.IdAttachmentType == step.IdAttachmentType) == null)
                {
                    return false;
                }
            }
            else if (step.IsTransferNeed)
            {
                if (attachments == null)
                {
                    return false;
                }
            }

            return true;
        }

        public Results UpdateRequestHistory(RequestHistory requestHistory)
        {
            try
            {
                RequestHistory postHistory = GetCurrentStep(requestHistory.IdRequest);
                db.RequestHistory.Attach(postHistory);
                postHistory.Comments = requestHistory.Comments;
                postHistory.DateApproval = DateTime.Now;
                postHistory.IdEmployee = requestHistory.IdEmployee;
                postHistory.IdStatus = requestHistory.IdStatus;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }

        public Results UpdateRequestHistory(RequestHistory requestHistory, Utilities.Status status)
        {
            try
            {
                RequestHistory postHistory = GetCurrentStep(requestHistory.IdRequest);
                db.RequestHistory.Attach(postHistory);
                postHistory.Comments = requestHistory.Comments;
                postHistory.DateApproval = DateTime.Now;
                postHistory.IdEmployee = requestHistory.IdEmployee;
                postHistory.IdStatus = (int)status;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }


        public RequestHistory GetRequestHistory(int id)
        {
            var query = db.RequestHistory.Include(a=> a.FlowSteps).FirstOrDefault(a=> a.Id == id);
            return query;
        }


        public Results Add(AuditTrail auditTrail)
        {
            try
            {
                db.AuditTrail.Add(auditTrail);
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }

        //        public List<PostHistory> GetFlowsHistory(int idPost)
        //        {
        //            List<FlowViewModel> flows = new List<FlowViewModel>();
        //            var b = GetPostsHistory(idPost);
        //            return b;
        //        }


        //        public List<PostHistory> GetFlowHistory(int? idPost)
        //        {
        //            List<FlowViewModel> flows = new List<FlowViewModel>();
        //            var b = GetPostHistory(idPost);
        //            return b;
        //        }

        public Results MoveForward(int idPost)
        {
            try
            {
                FlowSteps steps = new FlowSteps();
                Request request = GetRequest(idPost);
                var currentStep = request.FlowSteps;
                List<FlowSteps> stepsList = GetSteps(request.IdFlowType);
                if (request.IsCompleted == false)
                {
                    try
                    {
                        if (currentStep.StepNumber + 1 > stepsList.LastOrDefault().StepNumber)
                        {
                            request.IsCompleted = true;
                            request.IdStatus = (int)AdminLteMvc.Utilities.Status.Approved;
                            db.SaveChanges();
                            Utilities.Utilities.MailSenderCompleted(steps.Employee,idPost, EmailSubject.Approved);
                            return results = Utilities.Utilities.SetResults(true);
                        }

                        steps = GetNextStep(request.Id);
                        if (steps == null)
                        {
                            return results = Utilities.Utilities.SetResults(false, "No se encontraron pasos");
                        }

                        var stepBlocker = db.FlowSteps
                            .Where(a => a.IdFlowType == request.IdFlowType && a.IsBuyersBlocker).FirstOrDefault();
                        if (stepBlocker != null)
                        {
                            if (steps.StepNumber >= stepBlocker.StepNumber)
                            {
                                request.IdStatus = (int)AdminLteMvc.Utilities.Status.Blocked;
                            }
                        }
                        //if (steps.IsBuyersBlocker)
                     
                        if ((bool)currentStep.SendEmailWhenCompleted)
                        {
                            SendEmailToGroup((int)currentStep.IdGroupToInform, idPost, EmailSubject.AlreadyApproved, Status.Approved, currentStep.StepNumber);
                        }


                        request.FlowSteps = steps;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();
                        results = AddRequestHistory(idPost, steps.IdEmployee, Utilities.Status.Pending);
                        Utilities.Utilities.MailSender(steps.Employee, idPost, EmailSubject.Assigned);

                        if (results.IsSuccess == false)
                        {
                            return results;
                        }


                        return results = Utilities.Utilities.SetResults(true);

                    }
                    catch (Exception e)
                    {
                        return results = Utilities.Utilities.SetResults(false, e.Message);
                    }
                }

                return results = Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return results = Utilities.Utilities.SetResults(false, e.InnerException.Message.Replace("'", " "));
            }
        }

        public Results Add(EmployeeGroups employeeGroups)
        {
            var exists = db.EmployeeGroups.Any(a =>
                a.IdEmployee == employeeGroups.IdEmployee && a.IdGroup == employeeGroups.IdGroup);
            if (exists)
            {
                return Utilities.Utilities.SetResults(false, "Este empleado ya pertenece a este grupo.");
            }

            try
            {
                db.EmployeeGroups.Add(employeeGroups);
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }
        public List<ReasonToReject> GetRejectReasons()
        {
            var query = db.RejectReasons.AsNoTracking().OrderBy(a => a.Name).ToList();
            return query;
        }
        public Results ReStartFlow(RequestHistory postHistory)
        {
            var results = new Results();
            var post = GetRequest(postHistory.IdRequest);
            var step = GetNextStep(post.Id);
            if (post.Requestor.IsSupervisor)
            {
                step = db.RequestHistory.Include(a=> a.FlowSteps).Where(a => a.IdRequest == postHistory.IdRequest).OrderBy(a => a.DateApproval)
                    .FirstOrDefault().FlowSteps;
                step.IdEmployee = post.IdRequestor;
            }
            post.IdFlowSteps = step.Id;
            post.LastModified = DateTime.Now;
            db.Request.Add(post);
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            var employee = GetEmployee(post.IdRequestor);
            try
            {
                results = AddRequestHistory(post.Id, employee.Id, Status.Pending);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }

            return results;
        }
        public Results MoveBackward(int id)
        {
            try
            {
                FlowSteps steps = new FlowSteps();

                Request request = GetRequest(id);
                List<FlowSteps> stepsList = GetSteps(request.IdFlowType);

                steps = GetPreviousStep(id);
                var stepsBlock = db.FlowSteps.FirstOrDefault(a => a.IdFlowType == request.IdFlowType && a.IsBuyersBlocker == true);
                if (stepsBlock != null)
                {
                    if (steps.StepNumber <= stepsBlock.StepNumber)
                        request.IdStatus = (int)AdminLteMvc.Utilities.Status.Pending;
                    else
                        request.IdStatus = (int)AdminLteMvc.Utilities.Status.Blocked;

                }

                //if (steps.IsBuyersBlocker)
                //{
                //    request.IdStatus = (int)AdminLteMvc.Utilities.Status.Pending;


                //}
                //else
                //{
                //    var query = db.FlowSteps.Any(a => a.IdFlowType == request.IdFlowType && a.IsBuyersBlocker == true);
                //    if(query)
                //        request.IdStatus = (int)AdminLteMvc.Utilities.Status.Blocked;
                //    else
                //        request.IdStatus = (int)AdminLteMvc.Utilities.Status.Pending;

                //}
                if ((bool)request.FlowSteps.SendEmailWhenCompleted)
                {
                    SendEmailToGroup((int)request.FlowSteps.IdGroupToInform, id, EmailSubject.AlreadyApproved, Status.Rejected, steps.StepNumber);
                }
                request.FlowSteps = steps;
                db.Request.Add(request);
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                Employee employee1;
                employee1 = steps.Employee;
                Utilities.Utilities.MailSender(steps.Employee, request.Id, EmailSubject.Assigned);


                results = AddRequestHistory(id, employee1.Id, Utilities.Status.Pending);
                if (results.IsSuccess == true)
                {
                    //Utilities.Utilities.MailSender(steps.Employee, idPost, EmailSubject.Assigned);
                    return results = Utilities.Utilities.SetResults(true);
                }
                else
                {
                    db.Request.Remove(request);
                    db.SaveChanges();
                    return results = Utilities.Utilities.SetResults(false, results.Message);
                }


            }
            catch (Exception e)
            {
                return results = Utilities.Utilities.SetResults(false, e.InnerException.Message.Replace("'", " "));
            }
        }

        public Results Update(Request request, Utilities.Status status)
        {

            try
            {

                db.Entry(request).State = EntityState.Modified;
                request.IdStatus =  (int)status;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);

            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.InnerException.Message);
            }

         
        }
        public List<FlowSteps> GetSteps(int? idFlowType)
        {
            var query = db.FlowSteps.Include("Employee").Where(a => a.IdFlowType == idFlowType)
                .OrderBy(a => a.StepNumber).ToList();
            return query;
        }

        public Request GetRequest(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var query = db.Request.Include(a=> a.FlowSteps).Include("Requestor").Include(a => a.FlowType)
               .FirstOrDefault(a => a.Id == id);
            return query;
        }


        public Results AddRequestHistory(int idRequest, int? idEmployee, Utilities.Status status)
        {
            var request = GetRequest(idRequest);
            try
            {
                db.RequestHistory.Add(new RequestHistory
                {
                    IdEmployee = idEmployee,
                    IdRequest = idRequest,
                    IdStatus = (int)status,
                    DateApproval = DateTime.Now,
                    Comments = "Sin comentarios",
                    FlowSteps = request.FlowSteps
                });
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);

            }
        }

        public Results AddRequestHistoryCancel(RequestHistory request)
        {
            db.RequestHistory.Add(request);
            db.SaveChanges();

            var requests = GetRequest(request.IdRequest);
            try
            {
                db.RequestHistory.Add(new RequestHistory
                {
                    IdEmployee = request.IdEmployee,
                    IdRequest = request.IdRequest,
                    IdStatus = (int)request.IdStatus,
                    DateApproval = DateTime.Now,
                    Comments = "Cancelado "+request.Comments,
                    FlowSteps = request.FlowSteps
                });
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }




        public FlowSteps GetNextStep(int? idFlowType, int IdRequestor,int currentStep = 0)
        {
            FlowSteps steps = new FlowSteps();
            var employee = GetEmployee(IdRequestor);
            List<FlowSteps> stepsList = GetSteps(idFlowType);

            if (!employee.IsSupervisor)
                steps = stepsList.FirstOrDefault(a => a.StepNumber == currentStep + 1);
            else
                steps = stepsList.FirstOrDefault(a => a.IsBeginPointWhenSupervisor);
            return steps;
        }

        public FlowSteps GetNextStep(int idPost)
        {
            FlowSteps steps = new FlowSteps();
            Request post = GetRequest(idPost);
            List<FlowSteps> stepsList = GetSteps(post.IdFlowType);
            steps = stepsList.FirstOrDefault(a => a.StepNumber == post.FlowSteps.StepNumber + 1);

            if (post.IdStatus == (int)Utilities.Status.OnlyAttachments)
            {
                steps = db.FlowSteps.Include(a => a.Employee)
                    .FirstOrDefault(a => a.IdFlowType == post.IdFlowType && a.IsRestorePoint);

            }
            if (steps.IsSupervisorNeed)
            {
                steps.Employee = GetSupervisor(post.IdRequestor);
            }

            return steps;

        }

        public FlowSteps GetPreviousStep(int idFlowType, int currentStep)
        {
            FlowSteps steps = new FlowSteps();
            List<FlowSteps> stepsList = GetSteps(idFlowType);
            steps = stepsList.Where(a => a.StepNumber == currentStep + 1).FirstOrDefault();
            return steps;

        }

        public FlowSteps GetPreviousStep(int id)
        {
            FlowSteps steps = new FlowSteps();
            Request request = GetRequest(id);
            List<FlowSteps> stepsList = GetSteps(request.IdFlowType);
            if (stepsList.FirstOrDefault().Id == request.IdFlowSteps)
            {
                var stp = stepsList.FirstOrDefault();
                if (stp.IsSupervisorNeed)
                {
                    stp.Employee = GetSupervisor(request.IdRequestor);
                }
                stp.Employee = GetEmployee(request.Requestor.Identification);
                return stp;
            }

            steps = stepsList.FirstOrDefault(a => a.StepNumber == request.FlowSteps.StepNumber - 1);
           

            if (steps.IsSupervisorNeed)
            {
                steps.Employee = GetSupervisor(request.IdRequestor);
            }

            return steps;

        }

        //        //public Employee GetCurrentApprover(int idPost)
        //        //{
        //        //    Posts post = GetPost(idPost);
        //        //    var steps = GetSteps(post.IdFlowType);

        //        //}


        //        public Employee GetNextApprover(Posts post)
        //        {
        //            FlowSteps steps = new FlowSteps();
        //            steps = GetStep(post.IdFlowSteps);
        //            Employee employee;
        //            if (steps.IsMatrixNeeded)
        //            {
        //                SignatureLevels level = GetSignatureLevels(steps.IdFlowType).FirstOrDefault(a =>
        //                    a.AmmountFrom >= post.TotalAmount && a.AmmountTo <= post.TotalAmount);
        //                employee = db.Employee.Find(level.IdEmployee);
        //                return employee;
        //            }

        //            if (steps.IsSupervisorNeed)
        //            {
        //                employee = GetSupervisor(post.BuyerId);
        //                return employee;
        //            }

        //            return steps.Employee;
        //        }

        public Employee GetSupervisor(int idEmployee)
        {
                    Employee employee = db.Employee.Include("Employees").FirstOrDefault(a => a.Id == idEmployee);
                    return employee.Employees;
                }

     

        public List<Models.Status> GetStatus()
        {
            var query = db.Status.AsNoTracking().OrderBy(a => a.Name).ToList();
            return query;
        }





        public Employee GetEmployee(string identification)
        {
            var employee = db.Employee.Include(a => a.EmployeeGroups).FirstOrDefault(a => a.Identification == identification);
            return employee;
        }


        public Results Save(ApplicationUser aspNetUser)
        {

            var exists = GetEmployee(aspNetUser.UserName);
            var user = context.AspNetUsers.FirstOrDefault(a => a.UserName == aspNetUser.UserName);
            if (exists == null)
            {
                return Utilities.Utilities.SetResults(false, "El usuario provisto no pertenece a ningun empleado.");
            }

            if (user != null)
            {
                return Utilities.Utilities.SetResults(false, "Este empleado ya posee un usuario registrado");
            }

            try
            {
                var AspUser = context.AspNetUsers.AsNoTracking().FirstOrDefault(a => a.Id == aspNetUser.Id);
                context.Entry(aspNetUser).State = EntityState.Modified;
                AspUser.Email = aspNetUser.Email;
                AspUser.UserName = aspNetUser.UserName;
                context.SaveChanges();
                return Utilities.Utilities.SetResults(true);

            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }




        public Results Add(FlowSteps flowSteps)
        {
            var exists = ValidateIfStepExists(flowSteps.IdFlowType, flowSteps.StepNumber);

            if (exists == true)
            {
                return Utilities.Utilities.SetResults(false,
                    "Ya existe un paso con este numero, por favor revise todos los pasos primero");
            }

            try
            {
                db.FlowSteps.Add(flowSteps);
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);

            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message + " "+ e.InnerException.InnerException);
            }
        }

        public bool ValidateIfStepExists(int idFlowType, int StepNumber)
        {
            var query = db.FlowSteps.Any(a => a.IdFlowType == idFlowType && a.StepNumber == StepNumber);
            return query;
        }

        public bool ValidateIfStepExists(int idFlowType, int StepNumber, int IdStep)
        {
            var query = db.FlowSteps.Any(
                a => a.IdFlowType == idFlowType && a.StepNumber == StepNumber && a.Id != IdStep);
            return query;
        }
        public Results Update(FlowSteps flowSteps)
        {
            var exists = ValidateIfStepExists(flowSteps.IdFlowType, flowSteps.StepNumber, flowSteps.Id);
            if (exists == true)
            {
                return Utilities.Utilities.SetResults(false,
                    "Ya existe un paso con asociado a este flujo con este numero, por favor revisar e intentar de nuevo");
            }

            try
            {
                db.Entry(flowSteps).State = EntityState.Modified;
                db.SaveChanges();
                return Utilities.Utilities.SetResults(true);
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, e.Message);
            }
        }

        public Results Find(Employee employee, bool alreadyExists)
        {
            var query = db.Employee.ToList();
            bool exists;

            if (!alreadyExists)
            {
                exists = query.Any(a => a.Identification == employee.Identification);
                if (exists)
                {
                    return Utilities.Utilities.SetResults(false, "Ya existe un empleado con esta ficha.");
                }

                exists = query.Any(a => a.Email == employee.Email);
                if (exists)
                {
                    return Utilities.Utilities.SetResults(false, "Ya existe un empleado con este correo.");
                }
            }
            else
            {
                exists = query.Any(a => a.Identification == employee.Identification && a.Id != employee.Id);
                if (exists)
                {
                    return Utilities.Utilities.SetResults(false, "Ya existe un empleado con esta ficha.");
                }

                exists = query.Any(a => a.Email == employee.Email && a.Id != employee.Id);
                if (exists)
                {
                    return Utilities.Utilities.SetResults(false, "Ya existe un empleado con este correo.");
                }
            }

            return Utilities.Utilities.SetResults(true);
        }

        public List<AuditTrail> AuditTrailsFilter(string perfomedBy, string action)
        {
            var query = db.AuditTrail.Where(a => a.PerformedBy.Contains(perfomedBy == "" ? a.PerformedBy : perfomedBy) && a.Action ==(action == ""? a.Action : action))
                .ToList();
            return query;
        }
    }

    //    }
}