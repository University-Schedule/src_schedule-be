using Microsoft.EntityFrameworkCore;
using Schedule.Models.Bot;
using Schedule.Models.Parser;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Schedule.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class ScheduleDbContext :
    AbpDbContext<ScheduleDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    
    // Parser
    public virtual DbSet<PeriodModel> Periods { get; set; }
    public virtual DbSet<DaysDefModel> DaysDefs { get; set; }
    public virtual DbSet<WeeksDefModel> WeeksDefs { get; set; }
    public virtual DbSet<TermsDefModel> TermsDefs { get; set; }
    public virtual DbSet<SubjectModel> Subjects { get; set; }
    public virtual DbSet<TeacherModel> Teachers { get; set; }
    public virtual DbSet<BuildingModel> Buildings { get; set; }
    public virtual DbSet<ClassRoomModel> Classrooms { get; set; }
    public virtual DbSet<GradeModel> Grades { get; set; }
    public virtual DbSet<ClassModel> Classes { get; set; }
    public virtual DbSet<GroupModel> Groups { get; set; }
    public virtual DbSet<StudentModel> Students { get; set; }
    public virtual DbSet<StudentSubjectsModel> StudentSubjects { get; set; }
    public virtual DbSet<LessonModel> Lessons { get; set; }
    public virtual DbSet<CardModel> Cards { get; set; }
    
    // Bot
    public virtual DbSet<TelegramUser> TelegramUsers { get; set; }

    #endregion

    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureGdpr();
        
        /*builder.Entity<CardModel>().HasNoKey();
        builder.Entity<CardModel>(b =>
        {
            b.Ignore(p => p.Id);
        });

        builder.Entity<GradeModel>().HasNoKey();
        builder.Entity<GradeModel>(b =>
        {
            b.Ignore(p => p.Id);
        });

        builder.Entity<PeriodModel>().HasNoKey();
        builder.Entity<PeriodModel>(b =>
        {
            b.Ignore(p => p.Id);
        });

        builder.Entity<StudentSubjectsModel>().HasNoKey();
        builder.Entity<StudentSubjectsModel>(b =>
        {
            b.Ignore(p => p.Id);
        });*/
    }
}
