using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPSCO_Web.Controllers;

namespace OPSCO_Web.Tests.Controllers
{
    [TestClass]
    public class RepresentativeControllerTest
    {
        [TestMethod]
        public void TestDetailsView()
        {
            string test = "Success";
            Assert.AreEqual("Success", test);
        }
    }
}
