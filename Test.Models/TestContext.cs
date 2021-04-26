using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LecturerStudent>().HasKey(ls => new { ls.LecturerId, ls.StudentId });
            modelBuilder.Entity<LecturerStudent>()
                .HasOne<Lecturer>(ls => ls.Lecturer)
                .WithMany(l => l.LecturerStudents)
                .HasForeignKey(ls => ls.LecturerId);
            modelBuilder.Entity<LecturerStudent>()
                .HasOne<Student>(ls => ls.Student)
                .WithMany(s => s.LecturerStudents)
                .HasForeignKey(ls => ls.StudentId);
        }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<LecturerStudent> LecturerStudents { get; set; }
    }
}
