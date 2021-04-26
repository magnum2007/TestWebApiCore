using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Test.IServices;
using Test.Web.Extensions;
using Test.Web.ViewModels;

namespace Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase, IDisposable
    {
        private readonly ILecturerService _lecturerService;
        private readonly IStudentService _studentService;
        public TestController(ILecturerService lecturerService, IStudentService studentService)
        {
            _lecturerService = lecturerService;
            _studentService = studentService;
        }
        [HttpPost("Add")]
        public async Task<ActionResult> Add(LecturerViewModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _lecturerService.Add(model.ToLecturer());
            return Ok(result);
        }
        [HttpPost("AddMany")]
        public async Task<ActionResult> AddMany(IList<LecturerViewModel> model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _lecturerService.AddMany(model.ToLecturers());
            return Ok(result);
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(LecturerViewModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _lecturerService.Update(model.ToLecturer());
            return Ok(result);
        }
        [HttpPost("UpdateMany")]
        public async Task<ActionResult> UpdateMany(IList<LecturerViewModel> model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _lecturerService.UpdateMany(model.ToLecturers());
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _lecturerService.Delete(id);
            return Ok(result);
        }
        [HttpPost("Sync")]
        public async Task<ActionResult> Sync(SyncRequest request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _lecturerService.Sync(request.Lecturers.ToLecturers());
            return Ok(new SyncResponse { Lecturers = result.ToLecturerViewModels() });
        }
        [HttpGet("Download")]
        public async Task<ActionResult> Download()
        {
            var result = await _lecturerService.GetAll();
            return Ok(result.ToLecturerViewModels());
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _lecturerService.Dispose();
                    _studentService.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TestController() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}