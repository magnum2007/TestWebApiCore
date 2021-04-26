using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Services
{
    public class BaseService
    {
        private readonly TestContext _context;
        public BaseService(TestContext context)
        {
            _context = context;
        }
        public int SaveChange()
        {
            var savedData = false;
            int result = 0;
            while (!savedData)
            {
                try
                {
                    // Save the changes to the database
                    result = _context.SaveChanges();
                    savedData = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Student)
                        {
                            var currentValues = entry.CurrentValues; //Just to keep if needed
                            var dbValues = entry.GetDatabaseValues();
                            var student = (Student)entry.Entity;
                            student.Name = (string)dbValues["Name"];
                        }
                        else if (entry.Entity is Lecturer)
                        {
                            var currentValues = entry.CurrentValues; //Just to keep if needed
                            var dbValues = entry.GetDatabaseValues();
                            var lecturer = (Lecturer)entry.Entity;
                            lecturer.Name = (string)dbValues["Name"];
                        }
                        else
                        {
                            throw new NotSupportedException("Don’t know handling of concurrency conflict " + entry.Metadata.Name);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> SaveChangesAsync()
        {
            var savedData = false;
            int result = 0;
            while (!savedData)
            {
                try
                {
                    // Save the changes to the database
                    result = await _context.SaveChangesAsync();
                    savedData = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Student)
                        {
                            var currentValues = entry.CurrentValues; //Just to keep if needed
                            var dbValues = await entry.GetDatabaseValuesAsync();
                            var student = (Student)entry.Entity;
                            student.Name = (string)dbValues["Name"];
                        }
                        else if (entry.Entity is Lecturer)
                        {
                            var currentValues = entry.CurrentValues; //Just to keep if needed
                            var dbValues = await entry.GetDatabaseValuesAsync();
                            var lecturer = (Lecturer)entry.Entity;
                            lecturer.Name = (string)dbValues["Name"];
                        }
                        else
                        {
                            throw new NotSupportedException("Don’t know handling of concurrency conflict " + entry.Metadata.Name);
                        }
                    }
                }
            }
            return result;
        }
    }
}
