using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator;
using Schedulator.Controllers;
using Microsoft.AspNet.Identity;
using Schedulator.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
