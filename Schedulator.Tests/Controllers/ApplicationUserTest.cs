using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Controllers;
using Schedulator.Models;
using System.Data.Entity;
using System.Linq; 

namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class ApplicationUserTest
    {
        [TestMethod]
        public void TestRegister()
        {
            AccountController accountControler = new AccountController();
            ApplicationDbContext db = new ApplicationDbContext();
            
            RegisterViewModel registerViewModel = new RegisterViewModel { FirstName = "Test", LastName ="Test", Email = "Test@gmail.com", Password = "Password@123", ConfirmPassword = "Password@123", SelectedProgramId = db.Program.FirstOrDefault().ProgramId};

            accountControler.Register();
        }
    }
}
