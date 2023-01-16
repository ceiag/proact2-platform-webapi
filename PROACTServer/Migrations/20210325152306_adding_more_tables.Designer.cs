﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proact.Services;

namespace Proact.Services.Migrations
{
    [DbContext(typeof(ProactDatabaseContext))]
    [Migration("20210325152306_adding_more_tables")]
    partial class adding_more_tables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Proact.Services.Entities.DrugTarget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("DrugTargets");
                });

            modelBuilder.Entity("Proact.Services.Entities.DrugTargetPatient", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MedicalTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DrugTargetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "MedicalTeamId", "DrugTargetId");

                    b.HasIndex("DrugTargetId");

                    b.ToTable("DrugTargetPatient");
                });

            modelBuilder.Entity("Proact.Services.Entities.Medic", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MedicalTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "MedicalTeamId");

                    b.HasIndex("MedicalTeamId");

                    b.ToTable("Medics");
                });

            modelBuilder.Entity("Proact.Services.Entities.MedicAdmin", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MedicalTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "MedicalTeamId");

                    b.HasIndex("MedicalTeamId");

                    b.ToTable("MedicAdmins");
                });

            modelBuilder.Entity("Proact.Services.Entities.MedicalTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RegionCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOrProvince")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("MedicalTeams");
                });

            modelBuilder.Entity("Proact.Services.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ChatMessageType")
                        .HasColumnType("int");

                    b.Property<int?>("ContentStatus")
                        .HasColumnType("int");

                    b.Property<string>("ContentUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DrmProtectedContentUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrmProtectedMediaAssetId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<int?>("Emotion")
                        .HasColumnType("int");

                    b.Property<bool>("IsBroadcast")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHandled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMarked")
                        .HasColumnType("bit");

                    b.Property<bool?>("Landscape")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("MediaAssetId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("MedicalTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OriginalMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("RecordedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UploadedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("MedicalTeamId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Proact.Services.Entities.MessageRecipient", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ReadTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId", "RecipientId");

                    b.HasIndex("RecipientId");

                    b.ToTable("MessageRecipients");
                });

            modelBuilder.Entity("Proact.Services.Entities.Patient", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MedicalTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BirthYear")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ECode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PatientDataAssetId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ShowBroadcastMessages")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("TreatmentStartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "MedicalTeamId");

                    b.HasIndex("MedicalTeamId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Proact.Services.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Proact.Services.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Proact.Services.Entities.WelcomeMessage", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MessageId", "EntityId");

                    b.ToTable("WelcomeMessages");
                });

            modelBuilder.Entity("Proact.Services.Entities.DrugTarget", b =>
                {
                    b.HasOne("Proact.Services.Entities.Project", "Project")
                        .WithMany("DrugTargets")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Proact.Services.Entities.DrugTargetPatient", b =>
                {
                    b.HasOne("Proact.Services.Entities.DrugTarget", "DrugTarget")
                        .WithMany("DrugTargetPatients")
                        .HasForeignKey("DrugTargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Proact.Services.Entities.Patient", "Patient")
                        .WithMany("DrugTargetPatients")
                        .HasForeignKey("UserId", "MedicalTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DrugTarget");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Proact.Services.Entities.Medic", b =>
                {
                    b.HasOne("Proact.Services.Entities.MedicalTeam", "MedicalTeam")
                        .WithMany("Medics")
                        .HasForeignKey("MedicalTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Proact.Services.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalTeam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Proact.Services.Entities.MedicAdmin", b =>
                {
                    b.HasOne("Proact.Services.Entities.MedicalTeam", "MedicalTeam")
                        .WithMany("Admins")
                        .HasForeignKey("MedicalTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Proact.Services.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalTeam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Proact.Services.Entities.MedicalTeam", b =>
                {
                    b.HasOne("Proact.Services.Entities.Project", "Project")
                        .WithMany("MedicalTeams")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Proact.Services.Entities.Message", b =>
                {
                    b.HasOne("Proact.Services.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("Proact.Services.Entities.MedicalTeam", "MedicalTeam")
                        .WithMany()
                        .HasForeignKey("MedicalTeamId");

                    b.Navigation("Author");

                    b.Navigation("MedicalTeam");
                });

            modelBuilder.Entity("Proact.Services.Entities.MessageRecipient", b =>
                {
                    b.HasOne("Proact.Services.Entities.Message", "Message")
                        .WithMany("Recipients")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proact.Services.Entities.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Proact.Services.Entities.Patient", b =>
                {
                    b.HasOne("Proact.Services.Entities.MedicalTeam", "MedicalTeam")
                        .WithMany("Patients")
                        .HasForeignKey("MedicalTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Proact.Services.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalTeam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Proact.Services.Entities.Project", b =>
                {
                    b.HasOne("Proact.Services.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Proact.Services.Entities.WelcomeMessage", b =>
                {
                    b.HasOne("Proact.Services.Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("Proact.Services.Entities.DrugTarget", b =>
                {
                    b.Navigation("DrugTargetPatients");
                });

            modelBuilder.Entity("Proact.Services.Entities.MedicalTeam", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Medics");

                    b.Navigation("Patients");
                });

            modelBuilder.Entity("Proact.Services.Entities.Message", b =>
                {
                    b.Navigation("Recipients");
                });

            modelBuilder.Entity("Proact.Services.Entities.Patient", b =>
                {
                    b.Navigation("DrugTargetPatients");
                });

            modelBuilder.Entity("Proact.Services.Entities.Project", b =>
                {
                    b.Navigation("DrugTargets");

                    b.Navigation("MedicalTeams");
                });
#pragma warning restore 612, 618
        }
    }
}
