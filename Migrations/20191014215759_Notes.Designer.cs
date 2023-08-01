﻿// <auto-generated />
using Learning_Outcomes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Learning_Outcomes.Migrations
{
    [DbContext(typeof(Learning_OutcomesContext))]
    [Migration("20191014215759_Notes")]
    partial class Notes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Learning_Outcomes.Models.CourseInstances", b =>
                {
                    b.Property<int>("CourseInstancesID");

                    b.Property<string>("CourseName")
                        .IsRequired();

                    b.Property<string>("Dept")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("InstructorInstancesID");

                    b.Property<string>("Semester")
                        .IsRequired();

                    b.Property<int>("Year");

                    b.HasKey("CourseInstancesID");

                    b.HasIndex("InstructorInstancesID");

                    b.ToTable("CourseInstances");
                });

            modelBuilder.Entity("Learning_Outcomes.Models.CourseNoteInstances", b =>
                {
                    b.Property<int>("CourseNoteInstancesID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseInstancesID");

                    b.Property<string>("CourseNote");

                    b.HasKey("CourseNoteInstancesID");

                    b.HasIndex("CourseInstancesID")
                        .IsUnique();

                    b.ToTable("CourseNoteInstances");
                });

            modelBuilder.Entity("Learning_Outcomes.Models.InstructorInstances", b =>
                {
                    b.Property<int>("InstructorInstancesID");

                    b.Property<string>("InstructorUserName")
                        .IsRequired();

                    b.HasKey("InstructorInstancesID");

                    b.ToTable("InstructorInstances");
                });

            modelBuilder.Entity("Learning_Outcomes.Models.LearningOutcomeInstances", b =>
                {
                    b.Property<int>("LearningOutcomeInstancesID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseInstancesID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("LearningOutcomeInstancesID");

                    b.HasIndex("CourseInstancesID");

                    b.ToTable("LearningOutcomeInstances");
                });

            modelBuilder.Entity("Learning_Outcomes.Models.LearningOutcomeNoteInstances", b =>
                {
                    b.Property<int>("LearningOutcomeNoteInstancesID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseInstancesID");

                    b.Property<int>("LearningOutcomeInstancesID");

                    b.Property<string>("Note");

                    b.HasKey("LearningOutcomeNoteInstancesID");

                    b.HasIndex("LearningOutcomeInstancesID");

                    b.ToTable("LearningOutcomeNoteInstances");
                });

            modelBuilder.Entity("Learning_Outcomes.Models.CourseInstances", b =>
                {
                    b.HasOne("Learning_Outcomes.Models.InstructorInstances", "InstructorInstances")
                        .WithMany("Courses")
                        .HasForeignKey("InstructorInstancesID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Learning_Outcomes.Models.CourseNoteInstances", b =>
                {
                    b.HasOne("Learning_Outcomes.Models.CourseInstances", "Course")
                        .WithOne("Note")
                        .HasForeignKey("Learning_Outcomes.Models.CourseNoteInstances", "CourseInstancesID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Learning_Outcomes.Models.LearningOutcomeInstances", b =>
                {
                    b.HasOne("Learning_Outcomes.Models.CourseInstances", "CourseInstances")
                        .WithMany("LearningOutcomes")
                        .HasForeignKey("CourseInstancesID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Learning_Outcomes.Models.LearningOutcomeNoteInstances", b =>
                {
                    b.HasOne("Learning_Outcomes.Models.LearningOutcomeInstances", "LearningOutcome")
                        .WithMany()
                        .HasForeignKey("LearningOutcomeInstancesID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
