using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Web.ViewModels
{
    public class LecturerViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<StudentViewModel> Students { get; set; }
    }
}
