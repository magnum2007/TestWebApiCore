using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test.IServices
{
    public interface ILecturerService : IDisposable
    {
        Task<int> Add(Lecturer lecturer);
        Task<IList<int>> AddMany(IList<Lecturer> lecturers);
        Task<int> Update(Lecturer lecturer, bool sync = false);
        Task<IList<int>> UpdateMany(IList<Lecturer> lecturers, bool sync = false);
        Task<int> Delete(int id);
        Task<IList<int>> DeleteMany(IList<int> ids);
        Task<Lecturer> Get(int id);
        Task<IList<Lecturer>> GetMany(IList<int> ids);
        Task<IList<Lecturer>> GetAll();
        Task<IList<Lecturer>> Sync(IList<Lecturer> lecturers);
    }
}
