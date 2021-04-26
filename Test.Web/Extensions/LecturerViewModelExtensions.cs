using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;
using Test.Web.ViewModels;

namespace Test.Web.Extensions
{
    public static class LecturerViewModelExtensions
    {
        public static IList<Lecturer> ToLecturers(this IList<LecturerViewModel> obj)
        {
            return obj.Select(l => l.ToLecturer()).ToList();
        }
        public static IList<LecturerViewModel> ToLecturerViewModels(this IList<Lecturer> obj)
        {
            return obj.Select(l => l.ToLecturerViewModel()).ToList();
        }
        public static Lecturer ToLecturer(this LecturerViewModel obj)
        {
            return new Lecturer
            {
                Id = obj.Id,
                Name = obj.Name,
                LecturerStudents = obj.Students != null ? (obj.Students.Select(s => new LecturerStudent
                {
                    LecturerId = obj.Id,
                    Student = new Student
                    {
                        Id = s.Id,
                        Name = s.Name
                    }
                }).ToList()) : new List<LecturerStudent>()
            };
        }
        public static LecturerViewModel ToLecturerViewModel(this Lecturer obj)
        {
            return new LecturerViewModel
            {
                Id = obj.Id,
                Name = obj.Name,
                Students = obj.LecturerStudents != null ? (obj.LecturerStudents.Select(ls => new StudentViewModel
                {
                    Id = ls.Student?.Id ?? 0,
                    Name = ls.Student?.Name
                }).ToList()) : new List<StudentViewModel>()
            };
        }
    }
}
