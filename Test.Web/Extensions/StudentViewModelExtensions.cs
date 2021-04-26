using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;
using Test.Web.ViewModels;

namespace Test.Web.Extensions
{
    public static class StudentViewModelExtensions
    {
        public static IList<Student> ToStudents(this IList<StudentViewModel> obj)
        {
            return obj.Select(l => l.ToStudent()).ToList();
        }
        public static IList<StudentViewModel> ToStudentViewModels(this IList<Student> obj)
        {
            return obj.Select(l => l.ToStudentViewModel()).ToList();
        }
        public static Student ToStudent(this StudentViewModel obj)
        {
            return new Student
            {
                Id = obj.Id,
                Name = obj.Name
            };
        }
        public static StudentViewModel ToStudentViewModel(this Student obj)
        {
            return new StudentViewModel
            {
                Id = obj.Id,
                Name = obj.Name
            };
        }
    }
}
