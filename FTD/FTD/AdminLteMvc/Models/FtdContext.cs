using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace AdminLteMvc.Models
{
    public class FtdContext : DbContext
    {
        public FtdContext() : base ("DefaultConnection")
        {
                
        }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<AttachmentType> AttachmentType { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeGroups> EmployeeGroups { get; set; }
        public DbSet<FlowSteps> FlowSteps { get; set; }
        public DbSet<FlowType> FlowType { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<RequestHistory> RequestHistory { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ReasonToReject> RejectReasons { get; set; }








        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Attachments
            modelBuilder.Entity<Attachments>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Attachments>().HasRequired(a => a.Request).WithMany(a => a.Attachments).HasForeignKey(a => a.IdRequest);
            modelBuilder.Entity<Attachments>().HasOptional(a => a.AttachmentType).WithMany(a => a.Attachments).HasForeignKey(a => a.IdAttachmentType);

            #endregion

            #region AttachmentType
            modelBuilder.Entity<AttachmentType>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            #endregion

            #region Deparment
            modelBuilder.Entity<Department>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Department>().HasKey(a => a.IdDirector).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            #endregion

            #region Employee
            modelBuilder.Entity<Employee>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Employee>().HasOptional(a => a.Employees).WithMany(a => a.Supervised).HasForeignKey(a => a.IdSupervisor);
            modelBuilder.Entity<Employee>().HasOptional(a => a.Department).WithMany(a => a.Employees)
                .HasForeignKey(a => a.IdDepartment);
            #endregion

            #region EmployeeGroups
            modelBuilder.Entity<EmployeeGroups>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmployeeGroups>().HasRequired(a => a.Employee).WithMany(a => a.EmployeeGroups).HasForeignKey(a => a.IdEmployee);
            modelBuilder.Entity<EmployeeGroups>().HasRequired(a => a.Group).WithMany(a => a.EmployeeGroups).HasForeignKey(a => a.IdGroup);

            #endregion

            #region FlowSteps
            modelBuilder.Entity<FlowSteps>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<FlowSteps>().HasRequired(a => a.FlowType).WithMany(a => a.FlowSteps).HasForeignKey(a => a.IdFlowType);
            modelBuilder.Entity<FlowSteps>().HasOptional(a => a.AttachmentType).WithMany(a => a.FlowSteps).HasForeignKey(a => a.IdAttachmentType);
            modelBuilder.Entity<FlowSteps>().HasOptional( a => a.Group).WithMany(a => a.FlowSteps).HasForeignKey(a => a.IdGroup);
            modelBuilder.Entity<FlowSteps>().HasRequired(a => a.Employee).WithMany(a => a.FlowSteps).HasForeignKey(a => a.IdEmployee);
            modelBuilder.Entity<FlowSteps>().HasOptional(a => a.GroupToInform).WithMany(a => a.FlowSteps1)
                .HasForeignKey(a => a.IdGroupToInform);
            #endregion

            #region FlowType
            modelBuilder.Entity<FlowType>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            #endregion

            #region Group
            modelBuilder.Entity<Group>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            #endregion

            #region Request
            modelBuilder.Entity<Request>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Request>().HasRequired(a => a.From).WithMany(a => a.Requests).HasForeignKey(a => a.IdFrom);
            modelBuilder.Entity<Request>().HasRequired(a => a.To).WithMany(a => a.Requests).HasForeignKey(a => a.IdTo);
            modelBuilder.Entity<Request>().HasRequired(a => a.From).WithMany(a => a.Requests2).HasForeignKey(a => a.IdFrom).WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>().HasRequired(a => a.Requestor).WithMany(a => a.Request).HasForeignKey(a => a.IdRequestor);
            modelBuilder.Entity<Request>().HasRequired(a => a.Status).WithMany(a => a.Requests).HasForeignKey(a => a.IdStatus);
            modelBuilder.Entity<Request>().HasRequired(a => a.FlowSteps).WithMany(a => a.Requests).HasForeignKey(a => a.IdFlowSteps).WillCascadeOnDelete(false);
            modelBuilder.Entity<Request>().HasRequired(a => a.FlowType).WithMany(a => a.Requests).HasForeignKey(a => a.IdFlowType);






            #endregion

            #region RequestHistory
            modelBuilder.Entity<RequestHistory>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RequestHistory>().HasRequired(a => a.Status).WithMany(a => a.RequestHistory).HasForeignKey(a => a.IdStatus);
            modelBuilder.Entity<RequestHistory>().HasRequired(a => a.Request).WithMany(a => a.RequestHistory).HasForeignKey(a => a.IdRequest).WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestHistory>().HasRequired(a => a.Employee).WithMany(a => a.RequestHistory).HasForeignKey(a => a.IdEmployee);
            modelBuilder.Entity<RequestHistory>().HasRequired(a => a.FlowSteps).WithMany(a => a.RequestHistory).HasForeignKey(a => a.IdFlowSteps).WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestHistory>().HasOptional(a => a.RejectReasons).WithMany(a => a.RequestHistores)
                .HasForeignKey(a => a.IdRejectReasons);
            #endregion

            #region RequestType
            modelBuilder.Entity<RequestType>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            #endregion

            #region Status
            modelBuilder.Entity<Status>().HasKey(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            #endregion

            #region RejectReasons

            modelBuilder.Entity<ReasonToReject>().HasKey(a => a.Id).Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            #endregion


            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<AdminLteMvc.Models.RequestType> RequestTypes { get; set; }
    }
}