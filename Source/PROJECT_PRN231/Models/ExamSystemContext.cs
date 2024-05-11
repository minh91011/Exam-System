using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PROJECT_PRN231.Models
{
    public partial class ExamSystemContext : DbContext
    {
        public ExamSystemContext()
        {
        }

        public ExamSystemContext(DbContextOptions<ExamSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserExamQuestionAnswer> UserExamQuestionAnswers { get; set; } = null!;
        public virtual DbSet<UserExamResult> UserExamResults { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server =(local); database = ExamSystem; uid=sa;pwd=12345678; TrustServerCertificate=True;Encrypt=False");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerId)
                    .HasColumnName("answer_id");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.ExamName)
                    .HasMaxLength(255)
                    .HasColumnName("exam_name");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.ToTable("ExamQuestion");

                entity.Property(e => e.ExamQuestionId).HasColumnName("exam_question_id");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.QuestionOrder).HasColumnName("question_order");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_ExamQuestion_Exam");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_ExamQuestion_Question");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.DifficultyLevel).HasColumnName("difficulty_level");

                entity.Property(e => e.QuestionText).HasColumnName("question_text");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.OtpCode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("otpCode");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(500)
                    .HasColumnName("passwordHash");

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(500)
                    .HasColumnName("passwordSalt");

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.Verified).HasColumnName("verified");
            });

            modelBuilder.Entity<UserExamQuestionAnswer>(entity =>
            {
                entity.ToTable("UserExamQuestionAnswer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnswerId).HasColumnName("answer_id");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.UserExamQuestionAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_UserExamQuestionAnswer_Answer");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.UserExamQuestionAnswers)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_UserExamQuestionAnswer_Exam");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserExamQuestionAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_UserExamQuestionAnswer_Question");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserExamQuestionAnswers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserExamQuestionAnswer_Users");
            });

            modelBuilder.Entity<UserExamResult>(entity =>
            {
                entity.HasKey(e => e.ResultId)
                    .HasName("PK__UserExam__AFB3C3169FDD9DCF");

                entity.ToTable("UserExamResult");

                entity.Property(e => e.ResultId).HasColumnName("result_id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.UserExamResults)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_UserExamResult_Exam");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserExamResults)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserExamResult_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}