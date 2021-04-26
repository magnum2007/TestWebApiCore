using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.IServices;
using Test.Models;

namespace Test.Tests.Services
{
    public class LecturerServiceFake : ILecturerService
    {
        private readonly IList<Lecturer> _lecturers;
        private int maxId;
        public LecturerServiceFake()
        {
            _lecturers = new List<Lecturer>
            {
                new Lecturer
                {
                    Id = 1,
                    Name = "Asad",
                    LecturerStudents = new List<LecturerStudent>
                    {
                        new LecturerStudent{
                            LecturerId = 1,
                            Student = new Student{
                                Id = 1,
                                Name = "Ali"
                            }
                        },
                        new LecturerStudent{
                            LecturerId = 1,
                            Student = new Student{
                                Id = 2,
                                Name = "Ali2"
                            }
                        }
                    }
                },
                new Lecturer
                {
                    Id = 2,
                    Name = "Asad2",
                    LecturerStudents = new List<LecturerStudent>
                    {
                        new LecturerStudent{
                            LecturerId = 2,
                            Student = new Student{
                                Id = 2,
                                Name = "Ali2"
                            }
                        }
                    }
                }
            };
            maxId = _lecturers.Max(l => l.Id);
        }
        public async Task<int> Add(Lecturer lecturer)
        {
            lecturer.Id = ++maxId;
            _lecturers.Add(lecturer);
            return lecturer.Id;
        }

        public async Task<IList<int>> AddMany(IList<Lecturer> lecturers)
        {
            var lecturerIds = new List<int>();
            foreach (var lecturer in lecturers)
            {
                lecturerIds.Add(await Add(lecturer));
            }
            return lecturerIds;
        }

        public async Task<int> Delete(int id)
        {
            var l = await Get(id);
            if (l != null)
            {
                //_lecturers.Remove(l);
                return l.Id;
            }
            return 0;
        }

        public async Task<IList<int>> DeleteMany(IList<int> ids)
        {
            var lecturerIds = new List<int>();
            foreach (var id in ids)
            {
                lecturerIds.Add(await Delete(id));
            }
            return lecturerIds.Where(id => id != 0).ToList();
        }

        public void Dispose()
        {
            //TODO: Do something at least
        }

        public async Task<Lecturer> Get(int id)
        {
            return _lecturers.Where(l => l.Id == id).FirstOrDefault();
        }

        public async Task<IList<Lecturer>> GetAll()
        {
            return _lecturers;
        }

        public async Task<IList<Lecturer>> GetMany(IList<int> ids)
        {
            return _lecturers.Where(l => ids.Contains(l.Id)).ToList();
        }

        public async Task<IList<Lecturer>> Sync(IList<Lecturer> lecturers)
        {
            var newLecturers = lecturers.Where(l => l.Id == 0).ToList();
            var existingLecturers = lecturers.Where(l => l.Id != 0).ToList();
            var newLecturerIds = await AddMany(newLecturers);
            var existingLecturerIds = await UpdateMany(existingLecturers, true);
            return await GetMany(newLecturerIds.Union(existingLecturerIds).ToList());
        }

        public async Task<int> Update(Lecturer lecturer, bool sync = false)
        {
            var l = await Get(lecturer.Id);
            if (l != null)
            {
                l.Name = lecturer.Name;
                if (sync)
                    l.LecturerStudents.Clear();
                foreach (var lecturerStudent in lecturer.LecturerStudents)
                {
                    if (l.LecturerStudents.Select(l1 => l1.Student).Any(s => s.Id == lecturerStudent?.StudentId))
                        continue;
                    l.LecturerStudents.Add(new LecturerStudent
                    {
                        LecturerId = l.Id,
                        Student = lecturerStudent.Student
                    });
                }
                return l.Id;
            }
            return 0;
        }

        public async Task<IList<int>> UpdateMany(IList<Lecturer> lecturers, bool sync = false)
        {
            var lecturerIds = new List<int>();
            foreach (var lecturer in lecturers)
            {
                lecturerIds.Add(await Update(lecturer));
            }
            return lecturerIds;
        }
    }
}
