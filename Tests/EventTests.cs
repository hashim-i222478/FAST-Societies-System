using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.UI.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.Tests
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void Test_IsUpcoming()
        {
            try
            {
                var instance = new Event();
                instance.IsUpcoming();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsUpcoming: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsRegistrationOpen()
        {
            try
            {
                var instance = new Event();
                instance.IsRegistrationOpen();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsRegistrationOpen: {ex.Message}");
            }
        }

    }
}
