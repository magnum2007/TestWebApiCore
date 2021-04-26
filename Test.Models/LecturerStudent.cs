using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models
{
    public class LecturerStudent
    {
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
