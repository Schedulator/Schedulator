using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Schedulator.Models;
using System.Web.Mvc;
using System.Net;

namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class ProgramDirectorControllerTest
    {
        /*--------------------Details Method Testing--------------------*/
        [TestMethod]
        public void TestDetailsNullID()
        {
           //Call the details method with a null ID
            
            
            ProgramDirector pd1 = new ProgramDirector();

            ActionResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
    }
}
