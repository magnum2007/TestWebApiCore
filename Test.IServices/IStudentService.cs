using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test.IServices
{
    public interface IStudentService : IDisposable
    {
        Task<int> Add(Student student);
        Task<IList<int>> AddMany(IList<Student> students);
        Task<int> Update(Student student);
        Task<IList<int>> UpdateMany(IList<Student> students);
        Task<int> Delete(int id);
        Task<IList<int>> DeleteMany(IList<int> ids);
        Task<Student> Get(int id);
        Task<IList<Student>> GetMany(IList<int> ids);
    }
}
