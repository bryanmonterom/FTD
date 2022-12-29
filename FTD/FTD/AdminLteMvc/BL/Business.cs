using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;
using AdminLteMvc.DDL;
using Microsoft.Owin.Security.Provider;
using Status = AdminLteMvc.Utilities.Status;

namespace AdminLteMvc.BL
{
    public class Business
    {
        DDL.Data data = new Data();

        public List<Request> RequestFilter(int? idStatus, string identifier, string requestor)
        {
            return data.RequestFilter(idStatus, identifier, requestor);
        }
        public List<Request> RequestFilter(int? idStatus, string identifier, string requestor, string username)
        {
            return data.RequestFilter(idStatus, identifier, requestor, username);
        }

        public bool IsSecretary(string username)
        {
            return data.IsSecretary(username);
        }

        public Results AccessOrNot(string idEmployee, int idPostHistories)
        {
            try
            {
                var postHistories = data.GetRequestHistory(idPostHistories);
                var employee = data.GetEmployee(idEmployee);
                var flowstep = postHistories.FlowSteps;

                if (flowstep == null)
                {
                    return Utilities.Utilities.SetResults(false, "No steps found to validate the access");
                }
                var groupMembers = data.GetGroupMembers((int)flowstep.IdGroup).Where(a => a.IdEmployee == employee.Id).ToList();
                if ((!groupMembers.Any() || groupMembers == null || groupMembers.Count() == 0) && postHistories.IdEmployee != employee.Id)
                {
                    return Utilities.Utilities.SetResults(false, "You don't have access to edit this resource'");
                }
            }
            catch (Exception e)
            {
                return Utilities.Utilities.SetResults(false, "You don't have access to edit this resource'");
            }

            return Utilities.Utilities.SetResults(true);
        }

        public Results EndRequest(RequestHistory request, string username)
        {
            return data.EndRequest(request, username);
        }

        public DashboardViewModel GetDashboardViewModel(string username)
        {
            return data.GetDashboardViewModel(username);
        }

        public bool ValidateIfDirector(int idEmployee, int idDepartment)
        {
            return data.ValidateIfDirector(idEmployee, idDepartment);
        }

        public bool ValidateIfDirector(int idEmployee)
        {
            return data.ValidateIfDirector(idEmployee);
        }

        public string RequestQueryBuilder(DateTime? from, DateTime? to, int? IdFlowType, int? idStatus, int? idDepartment)
        {
            string query=" ";
            idStatus = idStatus == 0 ? (int)Utilities.Status.Approved : idStatus;
            if (idStatus != 0)
            {
                query += string.Format(" [IdStatus]={0}", idStatus);
            }
            if (from !=null && to !=null)
            {
            query += $"AND [Date] between (#{@from:yyyy-MM-dd}#,#{to:yyyy-MM-dd}#)";
            }

            if (IdFlowType !=0)
            {
                query += string.Format(" AND [IdFlowType] ={0}", IdFlowType);
            }
       if (idDepartment != 0)
            {
                query += string.Format(" AND [IdFrom] ={0}", idDepartment);
            }

            return query;
        }

        public List<AuditTrail> AuditTrailsFilter(string perfomedBy, string action)
        {
            return data.AuditTrailsFilter(perfomedBy, action);
        }


        public Results AuditTrailQueryBuilder(DateTime? ToDate, DateTime? fromDateTime,string performedBy = "", string action = "")
        {
            string query = " ";
            action = action == "" ? "I" : action;
            if (action != "")
            {
                query += string.Format(" [Action]='{0}'", action);
            }
            if (performedBy != "")
            {
                query += string.Format(" AND [PerformedBy]='{0}'", performedBy);
            }
            if (fromDateTime != null && ToDate != null)
            {
                query += $" AND [Date] between (#{fromDateTime:yyyy-MM-dd}#,#{ToDate:yyyy-MM-dd}#)";
            }

            return Utilities.Utilities.SetResults(true, query);
        }

        public Results RequestQueryBuilder(int? idPost, string user)
        {
            var employee = data.GetEmployee(user);
            var post = data.GetRequest(idPost);

            if (idPost == 0)
            {
                return Utilities.Utilities.SetResults(false, "El campo Id no puede ser nulo");
            }
            if (post == null)
            {
                return Utilities.Utilities.SetResults(false, "Solicitud no encontrada");
            }
            var historic = data.GetRequestHistoryForFlow(post.Id).Where(a => a.IdEmployee == employee.Id);

            if (!historic.Any())
            {
                return Utilities.Utilities.SetResults(false, "No puedes acceder a esta solicitud.");

            }
        

            string query;
            query = $"[Id] = {idPost}";
            return Utilities.Utilities.SetResults(true, message:query);
        }

        public List<Attachments> AttachmentsFilter(int? IdRequest, string identifier = "")
        {
            return data.AttachmentsFilter(IdRequest, identifier);
        }

        public List<FlowSteps> FlowStepsFilter(string employee, int? flowType)
        {
            return data.FlowStepsFilter(employee, flowType);
        }

        public List<Employee> EmployeeFilter(string card = "", string name = "")
        {
            return data.EmployeeFilter(card, name);
        }
        public List<RequestHistory> GetRequestHistoryForFlow(int id)
        {
            return data.GetRequestHistoryForFlow(id);
        }

        public List<UserViewModels> UsersFilter(string username = "", string role = "")
        {
            return data.UsersFilter(username, role);
        }

        public Results Add(FlowSteps flowSteps)
        {
            return data.Add(flowSteps);
        }

        public Results Add(Request request)
        {
            return data.Add(request);
        }
        public Results IsCompleted(int id)
        {
            return data.IsCompleted(id);
        }

        public Results Update(HttpPostedFileBase file, int idfile, bool IsSecretary)
        {
            byte[] bytes;
            var attachment = data.GetAttachments(idfile);
            var transfer = data.GetRequest(attachment.IdRequest);
            if (transfer.IsCompleted == true)
            {
                return Utilities.Utilities.SetResults(false,
                    "No puedes agregar un adjunto a esta solicitud, la misma se encuentra marcada como completada");
            }
            if (transfer.IdStatus == (int)AdminLteMvc.Utilities.Status.Blocked && IsSecretary)
            {

                return Utilities.Utilities.SetResults(false, "Esta solicitud se encuentra bloqueada para la edicion por secretarias.");
            }
            if (file.ContentLength == null)
            {
                return Utilities.Utilities.SetResults(false, "El archivo de firmas no puede estar en blanco");
            }
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                bytes = br.ReadBytes(file.ContentLength);
            }

            //if (file.ContentType.ToLower() == "image/jpeg")
            //{
            var vectpr = Utilities.Utilities.VectorGenerator();

           Attachments attachments = new Attachments()
            {
                Name = Path.GetFileName(file.FileName),
                ContentType = file.ContentType,
                Vector = vectpr,
                ByteContent = Utilities.Utilities.EncryptStringToBytes_Aes(bytes, vectpr),
                IdRequest = attachment.IdRequest,
                IdAttachmentType = 1,
                Id =  attachment.Id
            };
            return data.Update(attachments);
            //}


        }

        public Results Add(HttpPostedFileBase file, int idTransfer, bool IsFromShopping)
        {
            byte[] bytes;
            var transfer = data.GetRequest(idTransfer);
            if (transfer.IsCompleted == true)
            {
              return  Utilities.Utilities.SetResults(false,
                    "No puedes agregar un adjunto a esta solicitud, la misma se encuentra marcada como completada");
            }
            if (transfer.IdStatus == (int)AdminLteMvc.Utilities.Status.Blocked && IsFromShopping)
            {

                return Utilities.Utilities.SetResults(false, "Esta solicitud se encuentra bloqueada para la edicion por secretarias.");
            }
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                bytes = br.ReadBytes(file.ContentLength);
            }

            //if (file.ContentType.ToLower() == "image/jpeg")
            //{
            var vector = Utilities.Utilities.VectorGenerator();
                Attachments attachments = new Attachments()
                {
                    Name = Path.GetFileName(file.FileName),
                    ContentType = file.ContentType,
                    Vector = vector,
                    ByteContent =Utilities.Utilities.EncryptStringToBytes_Aes(bytes, vector),
                    IdRequest = idTransfer,
                    IdAttachmentType = 1
                };
                return data.Save(attachments);
            //}

            //return Utilities.Utilities.SetResults(false, "Invalid File type, it must be .JPG");
        }

        public Attachments GetAttachment(int idRequest)
        {
            return data.GetAttachments(idRequest);
        }


        public Results IsApproved(int id)
        {
            return data.IsApproved(id);
        }
        public Results SetApproval(RequestHistory requestHistory)
        {

            bool Completed = data.ValidateCompletition(requestHistory.IdRequest);
            var request = data.GetRequest(requestHistory.IdRequest);

            if (Completed == false && requestHistory.IdStatus == (int)Utilities.Status.Approved)
            {
                return Utilities.Utilities.SetResults(false, "El paso aun no esta completado, tienes acciones pendientes por realizar");
            }
            var results = data.UpdateRequestHistory(requestHistory);

            if (results.IsSuccess == true)
            {
                switch (requestHistory.IdStatus)
                {
                    case (int)Utilities.Status.Approved:
                        data.Update(request, Status.Pending);
                        results = data.MoveForward(requestHistory.IdRequest);

                        if (results.IsSuccess)
                        {
                            return Utilities.Utilities.SetResults(true);
                        }
                        else
                        {
                            requestHistory.IdStatus = (int)Utilities.Status.Pending;
                            data.UpdateRequestHistory(requestHistory);
                            return results;
                        }

                    case (int)Utilities.Status.Rejected:
                        if (requestHistory.IdRejectReasons == (int)Utilities.Utilities.RejectReasons.Documentation)
                        {
                            data.Update(request, Utilities.Status.OnlyAttachments);
                            data.UpdateRequestHistory(requestHistory);
                            return data.ReStartFlow(requestHistory);
                        }
                        results = data.MoveBackward(requestHistory.IdRequest);
                        if (results.IsSuccess)
                        {
                            return results.IsSuccess ? Utilities.Utilities.SetResults(true) : Utilities.Utilities.SetResults(false, results.Message);
                        }
                        else
                            return results;
                    default:
                        return Utilities.Utilities.SetResults(true);
                }
            }
            return Utilities.Utilities.SetResults(false, "El paso aun no esta completado, tienes acciones pendientes por realizar");
        }

        public List<EmployeeGroups> GetEmployeeGroups(int id)
        {
            return data.GetEmployeeGroups(id);
        }
        public Results Add(EmployeeGroups employeeGroups)
        {
            return data.Add(employeeGroups);
        }

        public int GetGroupPendingRequests(string username)
        {
            return data.GetGroupPendingRequests(username);

        }


        public int GetPendingRequests(string username)
        {
            return data.GetPendingRequests(username);
        }

        public int GetTotal(string username)
        {
            return data.GetPendingRequestTotal(username);
        }
        public Results Update(AspNetUsers aspuser)
        {
            return data.Update(aspuser);
        }


        public Results Update(FlowSteps flowSteps)
        {
            return data.Update(flowSteps);
        }

        public Results Find(Employee employee, bool alreadyExists)
        {
            return data.Find(employee, alreadyExists);
        }

        public Employee GetEmployee(string username)
        {
            return data.GetEmployee(username);
        }

        public Employee GetEmployee(int id)
        {
            return data.GetEmployee(id);
        }

        public List<FlowSteps> GetFlowSteps(int idFlowType)
        {
            return data.GetFlowSteps(idFlowType);
        }

        public Results Save(ApplicationUser aspNetUsers)
        {
            return data.Save(aspNetUsers);
        }
        //public Results SetApproval(PostHistory postHistory, bool isFromShopping)
        //{

        //    bool Completed = data.ValidateCompletition(postHistory.IdPost);
        //    var posts = data.GetPost(postHistory.IdPost);



        //    if (Completed == false && postHistory.IdStatus== (int)Utilities.Status.Approved)
        //    {
        //        return Utilities.Utilities.SetResults(false, "Step not completed, you some pending actions to do");
        //    }
        //    var results = data.UpdatePostHistory(postHistory);

        //        if (results.IsSuccess == true)
        //        {
        //            switch (postHistory.IdStatus)
        //            {
        //                case (int) Utilities.Status.Approved:

        //                    results = data.MoveForward(postHistory.IdPost);

        //                    if (results.IsSuccess)
        //                    {
        //                        return Utilities.Utilities.SetResults(true);
        //                    }
        //                    else
        //                    {
        //                        postHistory.IdStatus = (int) Utilities.Status.Pending;
        //                        data.UpdatePostHistory(postHistory);
        //                        return results;
        //                    }

        //                case (int)Utilities.Status.Rejected:
        //                    results = data.MoveBackward(postHistory.IdPost);
        //                    if (results.IsSuccess)
        //                    {

        //                        return results.IsSuccess ? Utilities.Utilities.SetResults(true) : Utilities.Utilities.SetResults(false, results.Message);
        //                    }
        //                    else
        //                        return results;
        //                default:
        //                    return Utilities.Utilities.SetResults(true);
        //            }
        //        }
        //    return Utilities.Utilities.SetResults(false, "The step is not completed");
        //}

        //public Results Add(Posts post)
        //{
        //    if (post.PoAmount < post.TotalAmount)
        //    {
        //        return Utilities.Utilities.SetResults(false, "The Payment Amount cannot be higher than the PO Amount");
        //    }
        //    return data.Add(post);
        //}

        //public Results Add(Transfer transfer)
        //{
        //    var posts = data.GetPostWithoutTracking(transfer.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "This file associated to this transfer is already completed you cannot edit it");
        //    }
        //    return data.Add(transfer);

        //}

        //public Results Add(Files files)
        //{
        //    var posts = data.GetPostWithoutTracking(files.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "This file associated to this transfer is already completed you cannot edit it");
        //    }
        //    return data.Add(files);

        //}

        //public List<PostHistory> GetFlowsViewModel(int idPost)
        //{
        //    return data.GetFlowsHistory(idPost);
        //}

        //public List<PostHistory> GetFlowViewModel(int? idPost)
        //{
        //    return data.GetFlowHistory(idPost);
        //}

        //public Posts GetPost(int? id)
        //{
        //    return data.GetPost(id);
        //}

        //public Results Add(HttpPostedFileBase file, int idTransfer)
        //{
        //    byte[] bytes;
        //    var transfer = data.FindTransfer(idTransfer);
        //    if (transfer.Post.IsCompleted == true)
        //    {
        //        Utilities.Utilities.SetResults(false,
        //            "You cant update this attachment, the request related to it is mark as completed");
        //    }
        //    using (BinaryReader br = new BinaryReader(file.InputStream))
        //    {
        //        bytes = br.ReadBytes(file.ContentLength);
        //    }

        //    if (file.ContentType.ToLower() == "image/jpeg")
        //    {

        //        Attachments attachments = new Attachments()
        //        {
        //            Name = Path.GetFileName(file.FileName),
        //            ContentType =  file.ContentType,
        //            ByteContent = bytes,
        //            IdTransfer = idTransfer
        //        };
        //        return data.Save(attachments);
        //    }

        //    return Utilities.Utilities.SetResults(false, "Invalid File type, it must be .JPG");

        //}

        //public Results Upload(int idPost, HttpPostedFileBase file,  int idType, bool IsFromShopping)
        //{
        //    byte[] bytes;
        //    var post = GetPost(idPost);
        //    if (post.IsCompleted == true)
        //    {
        //      return  Utilities.Utilities.SetResults(false,
        //            "You cant update this file, the request related to it is mark as completed");
        //    }
        //    if (post.IdStatus == (int)ADVP.Utilities.Status.Blocked && IsFromShopping)
        //    {

        //        return Utilities.Utilities.SetResults(false, "This item has been blocked for editing by Finance, you cannot edit it");
        //    }
        //    if (file == null)
        //    {
        //        var files = data.GeFiles(idPost);
        //        if (files == null)
        //        {
        //            return Utilities.Utilities.SetResults(false,
        //                "The file should not be empty");
        //        }

        //        return data.Update(files, idType);
        //    }
        //    using (BinaryReader br = new BinaryReader(file.InputStream))
        //    {
        //        bytes = br.ReadBytes(file.ContentLength);
        //    }

        //    if (file.ContentType.ToLower() == "application/pdf")
        //    {

        //        Files files = new Files()
        //        {
        //            Name = Path.GetFileName(file.FileName),
        //            ContentType = file.ContentType,
        //            ByteContent = bytes,
        //            IdPost = idPost,
        //            idAttachmentType = idType
        //        };
        //        return data.Save(files);
        //    }

        //    return Utilities.Utilities.SetResults(false, "Invalid File type, it must be .PDF");

        //}

        //public Transfer GetTransfer(int idPost)
        //{
        //    return data.GetTransfer(idPost);
        //}

        //public Files GetFiles(int idPost, int? type)
        //{
        //    return data.GetFiles(idPost, type);
        //}

        //public Attachments GetAttachment(int idTransfer)
        //{
        //    return data.GetAttachments(idTransfer);
        //}

        //public List<EmployeeGroups> GetEmployeeGroups(int id)
        //{
        //    return data.GetEmployeeGroups(id);
        //}

        //public Results Add(EmployeeGroups employeeGroups)
        //{
        //    return data.Add(employeeGroups);
        //}

        //public List<UserViewModel> GetUsers()
        //{
        //    return data.GetUsers();
        //}

        //public Employee GetEmployee(string employeeId)
        //{
        //    return data.GetEmployee(employeeId);
        //}

        //public Results Save(ApplicationUser aspuser)
        //    {
        //        return data.Add(aspuser);
        //    }

        //public Results Update(AspNetUser aspuser)
        //{
        //    return data.Update(aspuser);
        //}

        //public Results IsCompleted(int id)
        //{
        //    return data.IsCompleted(id);
        //}

        //public Results IsApproved(int id)
        //{
        //    return data.IsApproved(id);
        //}


        //public Results Remove(PostHistory post)
        //{
        //    var posts = data.GetPostWithoutTracking(post.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "This request is already completed you cannot removed it");
        //    }
        //    return data.Remove(post);
        //}

        //public Results Update(Posts post, bool IsShopping)
        //{
        //    var posts = data.GetPostWithoutTracking(post.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "This request is already completed you cannot edit it");
        //    }
        //    if (IsShopping == true)
        //    {
        //        if (posts.IdStatus == (int)ADVP.Utilities.Status.Blocked)
        //        {
        //            return Utilities.Utilities.SetResults(false, "This item has been blocked for editing by Finance, you cannot edit it");

        //        }
        //    }
        //    return data.Update(post);
        //}

        //public Results Update(Transfer transfer)
        //{
        //    var posts = data.GetPostWithoutTracking(transfer.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "This request associated to this transfer is already completed you cannot edit it");
        //    }
        //    return data.Update(transfer);
        //}



        //public Results Remove(Transfer transfer)
        //{
        //    var posts = data.GetPostWithoutTracking(transfer.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "The Request form associated to this transfer is already completed you cannot delete it");
        //    }
        //    return data.Remove(transfer);
        //}

        //public Results Remove(Files files)
        //{
        //    var posts = data.GetPostWithoutTracking(files.IdPost);
        //    if (posts.IsCompleted == true)
        //    {
        //        return Utilities.Utilities.SetResults(false,
        //            "The Request associated to this transfer is already completed you cannot delete it");
        //    }
        //    return data.Remove(files);
        //}

        //public Vendors GetVendor(string id)
        //{
        //    return data.FindVendor(id);
        //}

        //public List<Attachments> AttachmentFilter(string transfer)
        //{
        //    return data.AttachmentFilter(transfer);
        //}

        //public List<Bank> BankFilter(int? city, string bank)
        //{
        //    return data.BankFilter(city,bank);
        //}

        //public List<Employee> EmployeeFilter(string name, string card)
        //{
        //    return data.EmployeeFilter(card, name);
        //}

        //public List<Files> FilesFilter( string card)
        //{
        //    return data.FilesFilter(card);
        //}
        //public List<FlowSteps> FlowStepFilter(string employee, int? flowType, int? IdLocation)
        //{
        //    return data.FlowStepsFilter(employee,flowType, IdLocation);
        //}


        //public List<Posts> PostFilter(string post, int? location, int? currency, string poNumber, string buyer)
        //{
        //    return data.PostFilter(post, location, currency, poNumber, buyer);
        //}

        //public string TransferQueryBuilder(int post)
        //{
        //    string query;
        //    query = $"[PostId] = {post}";
        //    return query;
        //}

        //public string RequestQueryBuilder(int post)
        //{
        //    string query;
        //    query = $"[IdPost] = {post}";
        //    return query;
        //}

        //public string RequestQueryBuilder(DateTime from, DateTime to)
        //{
        //    string query;
        //    query = string.Format("[DateCreated] between (#{0:yyyy-MM-dd}#,#{1:yyyy-MM-dd}#)", from,
        //        to);
        //    //query = "[ExpectedDate] Between(2019-10-23, 2019-10-23)";
        //    return query;
        //}

        public List<RequestHistory> GetPostHistoryByEmployee(int? approvals, int? IdStatus, string request, string employee = "")
        {
            return data.PostHistoriesFilter(approvals, IdStatus, employee, request);
        }

        public List<RequestHistory> GetPostHistoryByEmployee(int? status, string idEmployee = "", string request = "")
        {
            return data.PostHistoriesFilter(status, idEmployee, request);
        }


        //public Results Add(FlowSteps flowSteps)
        //{
        //    return data.Add(flowSteps);
        //}

        //public Results Update(FlowSteps flowSteps)
        //{
        //    return data.Update(flowSteps);
        //}

        //public List<Transfer> TransfersFilter(int? transferType, string post, string transferIdentifier)
        //{
        //    return data.TransfersFilter(transferType, post, transferIdentifier);
        //}

        //public Results GenerateTransferReport(string postId)
        //{
        //    if (postId == "")
        //    {
        //        return Utilities.Utilities.SetResults(false, "The field cannot be empty");
        //    }

        //    var post = data.GetPost(postId);
        //    if (post == null)
        //    {
        //        return Utilities.Utilities.SetResults(false, "Request not found");
        //    }
        //    //if (post.IsCompleted == false)
        //    //{
        //    //    return Utilities.Utilities.SetResults(false, "You can only generate this report when the request is completed'");
        //    //}


        //    return Utilities.Utilities.SetResults(true, post.IdPost.ToString());
        //}

        //public Results Find(Employee employee, bool alreadyExists)
        //{
        //    return data.Find(employee, alreadyExists);
        //}

        //public Results Find(Vendors vendors,bool alreadyExists)
        //{
        //    return data.Find(vendors, alreadyExists);
        //}

        //public Results CloneFlow(FlowType flowType)
        //{
        //    return data.CloneFlow(flowType);
        //}

        //public List<Vendors> VendorsFilter(string name, int? currency, string number, int? bank)
        //{
        //   return data.VendorsFilter(name, currency, number, bank);
        //}

        //public int GetPendingRequests(string user)
        //{
        //  return data.GetPendingRequests(user);
        //}

        //public List<UserViewModel> UsersFilter(string username="", string role="")
        //{
        //    return data.UsersFilter(username, role);
        //}
    }
}
