using OnlineTechnicalSupport.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Context
{
    public class OTSContext : DbContext
    {
        public OTSContext() : base("OTSContext")
        {
        }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductComputerDetail> ProductComputerDetails { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<ComputerSkill> ComputerSkills { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignApplication> CampaignApplications { get; set; }
        public DbSet<AssetWarrantyDetail> AssetWarrantyDetails { get; set; }
        public DbSet<ServiceRequestStatus> ServiceRequestSituations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<IssueReportStatus> IssueReportSituations { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ChatStatus> ChatSituations { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<AccountSecurityRequest> AccountSecurityRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TextMessage>()
            //    .HasRequired(i => i.Chat)
            //    .WithMany(w => w.TextMessages)
            //    .HasForeignKey(i => i.ChatId)
            //    .WillCascadeOnDelete(false);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}