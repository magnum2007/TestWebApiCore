using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Test.IServices;
using Test.Tests.Services;
using Test.Web.Controllers;
using Test.Web.ViewModels;
using Xunit;

namespace Test.Tests
{
    public class TestControllerTest
    {
        private readonly ILecturerService _lecturerService;
        private readonly TestController _testController;
        public TestControllerTest()
        {
            _lecturerService = new LecturerServiceFake();
            _testController = new TestController(_lecturerService, null);
        }
        [Fact]
        public void Add_InvalidObject_ReturnsBadRequest()
        {
            //Act
            var result = _testController.Add(null).Result;
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Add_ValidObject_ReturnsOk()
        {
            //Act
            var result = _testController.Add(new Web.ViewModels.LecturerViewModel
            {
                Name = "Test Lecturer",
                Students = new System.Collections.Generic.List<Web.ViewModels.StudentViewModel>
                {
                    new Web.ViewModels.StudentViewModel{Name = "Test Student" }
                }
            }).Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void AddMany_ValidObject_ReturnsOk()
        {
            //Act
            var result = _testController.AddMany(new List<LecturerViewModel>
            {
                new Web.ViewModels.LecturerViewModel
                {
                    Name = "Test Lecturer",
                    Students = new System.Collections.Generic.List<Web.ViewModels.StudentViewModel>
                    {
                        new Web.ViewModels.StudentViewModel{Name = "Test Student" }
                    }
                }
            }).Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Update_InvalidObject_ReturnsBadRequest()
        {
            //Act
            var result = _testController.Update(null).Result;
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Update_ValidObject_ReturnsOk()
        {
            //Act
            var result = _testController.Update(new Web.ViewModels.LecturerViewModel
            {
                Id = 1,
                Name = "Test Lecturer",
                Students = new System.Collections.Generic.List<Web.ViewModels.StudentViewModel>
                {
                    new Web.ViewModels.StudentViewModel
                    {
                        Id = 1,
                        Name = "Test Student"
                    }
                }
            }).Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void UpdateMany_ValidObject_ReturnsOk()
        {
            //Act
            var result = _testController.UpdateMany(new List<LecturerViewModel> {
                new Web.ViewModels.LecturerViewModel
                {
                    Id = 1,
                    Name = "Test Lecturer",
                    Students = new System.Collections.Generic.List<Web.ViewModels.StudentViewModel>
                    {
                        new Web.ViewModels.StudentViewModel
                        {
                            Id = 1,
                            Name = "Test Student"
                        }
                    }
                }
            }).Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Delete_InvalidId_ReturnsZero()
        {
            //Act
            var result = _testController.Delete(20).Result;
            //Assert
            Assert.Equal(0, (result as OkObjectResult).Value);
        }
        [Fact]
        public void Delete_ValidId_ReturnsId()
        {
            //Act
            var result = _testController.Delete(1).Result;
            //Assert
            Assert.Equal(1, (result as OkObjectResult).Value);
        }
        [Fact]
        public void Sync_ValidObject_ReturnsResponseObject()
        {
            //Act
            var result = _testController.Sync(new SyncRequest
            {
                Lecturers = new List<LecturerViewModel>
                {
                    new Web.ViewModels.LecturerViewModel
                    {
                        Id = 1,
                        Name = "Test Lecturer",
                        Students = new System.Collections.Generic.List<Web.ViewModels.StudentViewModel>
                        {
                            new Web.ViewModels.StudentViewModel
                            {
                                Id = 1,
                                Name = "Test Student"
                            }
                        }
                    }
                }
            }).Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void Download_ReturnsResponse()
        {
            //Act
            var result = _testController.Download().Result;
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
