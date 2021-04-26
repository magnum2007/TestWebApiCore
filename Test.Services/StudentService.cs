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
    public class StudentService : BaseService, IStudentService, IDisposable
    {
        private readonly TestContext _context;
        public StudentService(TestContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> Add(Student student)
        {
            var s = new Student { Name = student.Name };
            _context.Students.Add(s);
            await base.SaveChangesAsync();
            return s.Id;
        }

        public async Task<IList<int>> AddMany(IList<Student> students)
        {
            var sList = new List<Student>();
            Student s = null;
            foreach (var student in students)
            {
                s = new Student { Name = student.Name };
                sList.Add(s);
                _context.Students.Add(s);
            }
            await base.SaveChangesAsync();
            return sList.Select(s1 => s1.Id).ToList();
        }

        public async Task<int> Delete(int id)
        {
            var student = _context.Students.Where(s => s.Id == id).FirstOrDefault();
            if (student != null)
            {
                _context.Students.Remove(student);
                await SaveChangesAsync();
                return id;
            }
            return 0;
        }

        public async Task<IList<int>> DeleteMany(IList<int> ids)
        {
            var idList = new List<int>();
            foreach (var id in ids)
                idList.Add(await Delete(id));
            return idList.Where(id => id != 0).ToList();
        }

        public async Task<Student> Get(int id)
        {
            return await _context.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Student>> GetMany(IList<int> ids)
        {
            return await _context.Students.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public async Task<int> Update(Student student)
        {
            var s1 = _context.Students.Where(s => s.Id == student.Id).FirstOrDefault();
            if (s1 != null)
            {
                s1.Name = student.Name ?? s1.Name;
                await base.SaveChangesAsync();
                return student.Id;
            }
            return 0;
        }

        public async Task<IList<int>> UpdateMany(IList<Student> students)
        {
            var sIdList = new List<int>();
            foreach (var s in students)
            {
                sIdList.Add(await Update(s));
            }
            await SaveChangesAsync();
            return sIdList.Where(id => id != 0).ToList();
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
        // ~StudentService() {
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
