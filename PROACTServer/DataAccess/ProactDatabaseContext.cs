using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Entities.MedicalTeams;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Entities.Users;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Proact.Services {
    public class ProactDatabaseContext : DbContext {
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<InstituteAdmin> InstituteAdmins { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ProjectHtmlContent> ProjectHtmlContents { get; set; }
        public DbSet<ProjectProtocol> ProjectProtocols { get; set; }
        public DbSet<UserProtocol> UserProtocols { get; set; }
        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectProperties> ProjectProperties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<NotificationSettings> NotificationSettings { get; set; }
        public DbSet<MedicalTeam> MedicalTeams { get; set; }
        public DbSet<MedicsMedicalTeamRelation> MedicsMedicalTeamRelations { get; set; }
        public DbSet<NursesMedicalTeamRelation> NursesMedicalTeamRelations { get; set; }
        public DbSet<DataManagersMedicalTeamRelation> DataManagersMedicalTeamRelations { get; set; }
        public DbSet<ResearchersMedicalTeamRelation> ResearchersMedicalTeamRelation { get; set; }
        public DbSet<Medic> Medics { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<DataManager> DataManagers { get; set; }
        public DbSet<MedicAdmin> MedicAdmins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<TreatmentHistory> TreatmentsHistory { get; set; }
        public DbSet<MessageRecipient> MessageRecipients { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageData> MessagesData { get; set; }
        public DbSet<MessageReplies> MessagesReplies { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<ProactSystemInfo> ProactSystemInfo { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveysQuestionsRelation> SurveysQuestionsRelations { get; set; }
        public DbSet<SurveyQuestionsAnswersRelation> QuestionsAnswersRelations { get; set; }
        public DbSet<SurveysAssignationRelation> SurveysAssignationsRelations { get; set; }
        public DbSet<SurveyUsersQuestionsAnswersRelation> SurveyUsersQuestionsAnswersRelations { get; set; }
        public DbSet<SurveyUserQuestionAnswer> SurveyUserQuestionAnswers { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyQuestionsSet> SurveyQuestionsSets { get; set; }
        public DbSet<SurveyAnswersBlock> SurveyAnswersBlocks { get; set; }
        public DbSet<SurveyScheduler> SurveyScheduler { get; set; }
        public DbSet<Lexicon> Lexicons { get; set; }
        public DbSet<LexiconLabel> LexiconLabels { get; set; }
        public DbSet<LexiconCategory> LexiconCategories { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        public DbSet<MobileAppsInfo> MobileAppsInfo { get; set; }

        public ProactDatabaseContext( DbContextOptions<ProactDatabaseContext> options ) : base( options ) {
        }

        public void SaveChangesWithEntityTracking( Guid userId ) {
            PerformOnSavingChanges( userId );
            SaveChanges();
        }

        private void PerformOnSavingChanges( Guid userId ) {
            var entities = ChangeTracker.Entries<IEntity>();
            var addedEntities = entities
                .Where( e => e.State == EntityState.Added ).ToList();
            var modifiedEntities = entities
                .Where( e => e.State == EntityState.Modified ).ToList();

            addedEntities.Where( e => e.Entity.Id == Guid.Empty )
                .ToList().ForEach( e => {
                    e.Entity.Id = Guid.NewGuid();
                } );

            var entityTracker = new EntityTracker();
            
            entityTracker.SetTrackInfo( 
                userId, addedEntities.Select( e => e.Entity ), EntityState.Added );
            
            entityTracker.SetTrackInfo( 
                userId, modifiedEntities.Select( e => e.Entity ), EntityState.Modified );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<Institute>()
                .HasMany( x => x.Projects )
                .WithOne( x => x.Institute )
                .HasForeignKey( x => x.InstituteId );

            modelBuilder.Entity<Institute>()
                .HasMany( x => x.Admins )
                .WithOne( x => x.Institute )
                .HasForeignKey( x => x.InstituteId );

            modelBuilder.Entity<Institute>()
                .HasMany( x => x.Documents )
                .WithOne( x => x.Institute )
                .HasForeignKey( x => x.InstituteId );

            modelBuilder.Entity<Project>()
                .HasOne( x => x.ProjectProperties )
                .WithOne( x => x.Project )
                .HasForeignKey<ProjectProperties>( x => x.ProjectId );

            modelBuilder.Entity<Patient>()
                .HasOne( x => x.User );

            modelBuilder.Entity<Patient>()
                .HasIndex( x => x.UserId )
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasOne( x => x.MedicalTeam );

            modelBuilder.Entity<Medic>()
                .HasIndex( x => x.UserId )
                .IsUnique();

            modelBuilder.Entity<Medic>()
                .HasMany( x => x.MedicalTeamRelations )
                .WithOne( x => x.Medic )
                .HasForeignKey( x => x.MedicId );

            modelBuilder.Entity<MedicalTeam>()
                .HasMany( x => x.MedicsRelation )
                .WithOne( x => x.MedicalTeam )
                .HasForeignKey( x => x.MedicalTeamId );

            modelBuilder.Entity<Nurse>()
                .HasMany( x => x.MedicalTeamRelations )
                .WithOne( x => x.Nurse )
                .HasForeignKey( x => x.NurseId );

            modelBuilder.Entity<MedicalTeam>()
                .HasMany( x => x.NursesRelation )
                .WithOne( x => x.MedicalTeam )
                .HasForeignKey( x => x.MedicalTeamId );

            modelBuilder.Entity<Nurse>()
                .HasIndex( x => x.UserId )
                .IsUnique();

            modelBuilder.Entity<DataManager>()
                .HasMany( x => x.DataManagerTeamRelations )
                .WithOne( x => x.DataManager )
                .HasForeignKey( x => x.DataManagerId );

            modelBuilder.Entity<MedicalTeam>()
                .HasMany( x => x.DataManagersRelation )
                .WithOne( x => x.MedicalTeam )
                .HasForeignKey( x => x.MedicalTeamId );

            modelBuilder.Entity<Device>()
                .HasIndex( x => x.PlayerId )
                .IsUnique();

            modelBuilder.Entity<NotificationSettings>()
                .HasMany( x => x.Devices )
                .WithOne( x => x.NotificationSettings )
                .HasForeignKey( x => x.NotificationSettingsId );

            modelBuilder.Entity<MedicAdmin>()
                .HasKey( x => new { x.UserId, x.MedicalTeamId } );

            modelBuilder.Entity<Message>()
                .HasMany( u => u.Recipients )
                .WithOne( u => u.Message ).IsRequired()
                .HasForeignKey( u => u.MessageId );

            modelBuilder.Entity<Message>()
                .HasOne( x => x.MessageAttachment )
                .WithOne( x => x.Message )
                .HasForeignKey<MessageAttachment>( x => x.MessageId );

            modelBuilder.Entity<Message>()
                .HasMany( m => m.Replies )
                .WithOne( m => m.OriginalMessage )
                .HasForeignKey( m => m.OriginalMessageId );

            modelBuilder.Entity<Message>()
                .HasOne( m => m.MessageData )
                .WithOne( m => m.Message )
                .HasForeignKey<MessageData>( x => x.MessageId );

            modelBuilder.Entity<MessageReplies>()
                .HasOne( m => m.OriginalMessage )
                .WithMany( m => m.Replies )
                .HasForeignKey( m => m.OriginalMessageId );

            modelBuilder.Entity<MessageReplies>()
                .HasKey( m => new { m.OriginalMessageId, m.Id } );

            modelBuilder.Entity<MessageRecipient>()
                .HasKey( m => new { m.MessageId, m.UserId } );

            modelBuilder.Entity<MessageRecipient>()
                .HasOne( m => m.Message )
                .WithMany( m => m.Recipients )
                .OnDelete( DeleteBehavior.Restrict );

            modelBuilder.Entity<Project>()
                .HasMany( p => p.MedicalTeams )
                .WithOne( p => p.Project )
                .HasForeignKey( p => p.ProjectId );

            modelBuilder.Entity<SurveysAssignationRelation>()
                .HasMany( x => x.UserAnswers )
                .WithOne( x => x.AssignmentRelation )
                .HasForeignKey( x => x.AssignmentId );
                
            modelBuilder.Entity<Message>()
                .HasMany( x => x.Analysis )
                .WithOne( x => x.Message )
                .HasForeignKey( x => x.MessageId );

            modelBuilder.Entity<Analysis>()
                .HasMany( x => x.AnalysisResults )
                .WithOne( x => x.Analysis )
                .HasForeignKey( x => x.AnalysisId );

            modelBuilder.Entity<Survey>()
                .HasMany( x => x.AssignationRelations )
                .WithOne( x => x.Survey )
                .HasForeignKey( x => x.SurveyId );

            base.OnModelCreating( modelBuilder );
        }
    }
}
