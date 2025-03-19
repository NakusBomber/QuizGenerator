﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizGenerator.DAL;

#nullable disable

namespace QuizGenerator.DAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("QuizGenerator.Model.Entities.AnswerDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("QuestionDetailId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionDetailId");

                    b.ToTable("AnswerDetails");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.PracticeSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("QuizId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("PracticeSessions");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("EvaluationPrice")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ListNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("QuizId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.QuestionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionDetails");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTimeChanged")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateTimeLastPractice")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan?>("IntervalPractice")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.UserAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AnswerDetailId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PracticeSessionId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("QuestionDetailId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AnswerDetailId");

                    b.HasIndex("PracticeSessionId");

                    b.HasIndex("QuestionDetailId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.AnswerDetail", b =>
                {
                    b.HasOne("QuizGenerator.Model.Entities.QuestionDetail", "QuestionDetail")
                        .WithMany("AnswerDetails")
                        .HasForeignKey("QuestionDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("QuestionDetail");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.PracticeSession", b =>
                {
                    b.HasOne("QuizGenerator.Model.Entities.Quiz", "Quiz")
                        .WithMany()
                        .HasForeignKey("QuizId");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.Question", b =>
                {
                    b.HasOne("QuizGenerator.Model.Entities.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.QuestionDetail", b =>
                {
                    b.HasOne("QuizGenerator.Model.Entities.Question", "Question")
                        .WithMany("QuestionDetails")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.UserAnswer", b =>
                {
                    b.HasOne("QuizGenerator.Model.Entities.AnswerDetail", "AnswerDetail")
                        .WithMany()
                        .HasForeignKey("AnswerDetailId");

                    b.HasOne("QuizGenerator.Model.Entities.PracticeSession", "PracticeSession")
                        .WithMany("UserAnswers")
                        .HasForeignKey("PracticeSessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizGenerator.Model.Entities.QuestionDetail", "QuestionDetail")
                        .WithMany()
                        .HasForeignKey("QuestionDetailId");

                    b.Navigation("AnswerDetail");

                    b.Navigation("PracticeSession");

                    b.Navigation("QuestionDetail");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.PracticeSession", b =>
                {
                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.Question", b =>
                {
                    b.Navigation("QuestionDetails");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.QuestionDetail", b =>
                {
                    b.Navigation("AnswerDetails");
                });

            modelBuilder.Entity("QuizGenerator.Model.Entities.Quiz", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
