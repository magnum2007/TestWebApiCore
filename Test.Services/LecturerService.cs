using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.IServices;
using Test.Models;

namespace Test.Services
{
    public class LecturerService : BaseService, ILecturerService, IDisposable
    {
        private readonly TestContext _context;
        private readonly IStudentService _studentService;
        public LecturerService(TestContext context, IStudentService studentService) : base(context)
        {
            _context = context;
            _studentService = studentService;
        }
        public async Task<int> Add(Lecturer lecturer)
        {
            var l = new Lecturer
            {
                Name = lecturer.Name
            };
            _context.Lecturers.Add(l);
            await SaveChangesAsync();
            var newStudents = lecturer.LecturerStudents.Select(ls => ls.Student).Where(s => s.Id == 0).ToList();
            var existingStudents = lecturer.LecturerStudents.Select(ls => ls.Student).Where(s => s.Id != 0).ToList();
            var newStudentIds = await _studentService.AddMany(newStudents);
            var existingStudentIds = await _studentService.UpdateMany(existingStudents);
            foreach (var studentId in newStudentIds.Union(existingStudentIds))
            {
                _context.LecturerStudents.Add(new LecturerStudent
                {
                    LecturerId = l.Id,
                    StudentId = studentId
                });
            }
            await SaveChangesAsync();
            return l.Id;
        }

        public async Task<IList<int>> AddMany(IList<Lecturer> lecturers)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lecturerIds = new List<int>();
                foreach (var lecturer in lecturers)
                {
                    lecturerIds.Add(await Add(lecturer));
                }
                transaction.Commit();
                return lecturerIds;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> Delete(int id)
        {
            var l = await Get(id);
            if (l != null)
            {
                foreach (var ls in l.LecturerStudents)
                {
                    _context.LecturerStudents.Remove(ls);
                }
                //await SaveChangesAsync();
                _context.Lecturers.Remove(l);
                await SaveChangesAsync();
                return id;
            }
            return 0;
        }

        public async Task<IList<int>> DeleteMany(IList<int> ids)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lecturerIds = new List<int>();
                foreach (var id in ids)
                {
                    lecturerIds.Add(await Delete(id));
                }
                transaction.Commit();
                return lecturerIds.Where(id => id != 0).ToList();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Lecturer> Get(int id)
        {
            return await _context.Lecturers.Where(l1 => l1.Id == id).Include(l1 => l1.LecturerStudents).ThenInclude(ls => ls.Student).FirstOrDefaultAsync();
        }

        public async Task<IList<Lecturer>> GetMany(IList<int> ids)
        {
            return await _context.Lecturers.Where(l1 => ids.Contains(l1.Id)).Include(l1 => l1.LecturerStudents).ThenInclude(ls => ls.Student).ToListAsync();
        }

        public async Task<IList<Lecturer>> GetAll()
        {
            return await _context.Lecturers.Include(l1 => l1.LecturerStudents).ThenInclude(ls => ls.Student).ToListAsync();
        }

        public async Task<int> Update(Lecturer lecturer, bool sync = false)
        {
            var l = await Get(lecturer.Id);
            if (l != null)
            {
                l.Name = lecturer.Name ?? l.Name;
                var newStudents = lecturer.LecturerStudents.Select(ls => ls.Student).Where(s => s.Id == 0).ToList();
                var existingStudents = lecturer.LecturerStudents.Select(ls => ls.Student).Where(s => s.Id != 0).ToList();
                var newStudentIds = await _studentService.AddMany(newStudents);
                var existingStudentIds = await _studentService.UpdateMany(existingStudents);
                //if (sync)
                //    foreach (var ls in l.LecturerStudents)
                //        _context.LecturerStudents.Remove(ls);
                foreach (var studentId in newStudentIds.Union(existingStudentIds).Except(l.LecturerStudents.Select(ls => ls.StudentId)))
                {
                    _context.LecturerStudents.Add(new LecturerStudent
                    {
                        LecturerId = l.Id,
                        StudentId = studentId
                    });
                }
                await SaveChangesAsync();
                return l.Id;
            }
            return 0;
        }

        public async Task<IList<int>> UpdateMany(IList<Lecturer> lecturers, bool sync = false)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lecturerIds = new List<int>();
                foreach (var lecturer in lecturers)
                {
                    lecturerIds.Add(await Update(lecturer, sync));
                }
                transaction.Commit();
                return lecturerIds.Where(id => id != 0).ToList();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IList<Lecturer>> Sync(IList<Lecturer> lecturers)
        {
            var newLecturers = lecturers.Where(l => l.Id == 0).ToList();
            var existingLecturers = lecturers.Where(l => l.Id != 0).ToList();
            var newLecturerIds = await AddMany(newLecturers);
            var existingLecturerIds = await UpdateMany(existingLecturers, true);
            return await GetMany(newLecturerIds.Union(existingLecturerIds).ToList());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LecturerService() {
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
