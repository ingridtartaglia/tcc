﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TrainingSystem.Data;

namespace TrainingSystem.Migrations
{
    [DbContext(typeof(TrainingSystemContext))]
    partial class TrainingSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TrainingSystem.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TrainingSystem.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Instructor")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("CourseId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("TrainingSystem.Models.CourseSubscription", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("EmployeeId");

                    b.HasKey("CourseId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("CourseSubscription");
                });

            modelBuilder.Entity("TrainingSystem.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<string>("Occupation")
                        .IsRequired();

                    b.HasKey("EmployeeId");

                    b.HasIndex("AppUserId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("TrainingSystem.Models.Exam", b =>
                {
                    b.Property<int>("ExamId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LessonId");

                    b.HasKey("ExamId");

                    b.HasIndex("LessonId")
                        .IsUnique();

                    b.ToTable("Exam");
                });

            modelBuilder.Entity("TrainingSystem.Models.Keyword", b =>
                {
                    b.Property<int>("KeywordId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("KeywordId");

                    b.HasIndex("CourseId");

                    b.ToTable("Keyword");
                });

            modelBuilder.Entity("TrainingSystem.Models.Lesson", b =>
                {
                    b.Property<int>("LessonId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("LessonId");

                    b.HasIndex("CourseId");

                    b.ToTable("Lesson");
                });

            modelBuilder.Entity("TrainingSystem.Models.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("MaterialId");

                    b.HasIndex("CourseId");

                    b.ToTable("Material");
                });

            modelBuilder.Entity("TrainingSystem.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ExamId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("QuestionId");

                    b.HasIndex("ExamId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("TrainingSystem.Models.QuestionChoice", b =>
                {
                    b.Property<int>("QuestionChoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsCorrect");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("QuestionId");

                    b.HasKey("QuestionChoiceId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionChoice");
                });

            modelBuilder.Entity("TrainingSystem.Models.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int>("CourseId");

                    b.Property<int>("Name");

                    b.HasKey("RatingId");

                    b.HasIndex("CourseId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("TrainingSystem.Models.Video", b =>
                {
                    b.Property<int>("VideoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<int>("LessonId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("VideoId");

                    b.HasIndex("LessonId");

                    b.ToTable("Video");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TrainingSystem.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TrainingSystem.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TrainingSystem.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TrainingSystem.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.CourseSubscription", b =>
                {
                    b.HasOne("TrainingSystem.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TrainingSystem.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Employee", b =>
                {
                    b.HasOne("TrainingSystem.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TrainingSystem.Models.Exam", b =>
                {
                    b.HasOne("TrainingSystem.Models.Lesson", "Lesson")
                        .WithOne("Exam")
                        .HasForeignKey("TrainingSystem.Models.Exam", "LessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Keyword", b =>
                {
                    b.HasOne("TrainingSystem.Models.Course", "Course")
                        .WithMany("Keywords")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Lesson", b =>
                {
                    b.HasOne("TrainingSystem.Models.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Material", b =>
                {
                    b.HasOne("TrainingSystem.Models.Course", "Course")
                        .WithMany("Materials")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Question", b =>
                {
                    b.HasOne("TrainingSystem.Models.Exam", "Exam")
                        .WithMany("Questions")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.QuestionChoice", b =>
                {
                    b.HasOne("TrainingSystem.Models.Question", "Question")
                        .WithMany("QuestionChoices")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Rating", b =>
                {
                    b.HasOne("TrainingSystem.Models.Course", "Course")
                        .WithMany("Ratings")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TrainingSystem.Models.Video", b =>
                {
                    b.HasOne("TrainingSystem.Models.Lesson", "Lesson")
                        .WithMany("Videos")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
