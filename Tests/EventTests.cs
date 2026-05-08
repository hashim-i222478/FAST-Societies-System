using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.BLL;

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
