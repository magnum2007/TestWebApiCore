using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Web.ViewModels
{
    public class SyncRequest
    {
        public SyncRequest()
        {
            Lecturers = new List<LecturerViewModel>();
        }
        [Required]
        public IList<LecturerViewModel> Lecturers { get; set; }
    }
}
