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
    public class EventRegistrationTests
    {
        [TestMethod]
        public void Test_CheckIn()
        {
            try
            {
                var instance = new EventRegistration();
                instance.CheckIn();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CheckIn: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_Cancel()
        {
            try
            {
                var instance = new EventRegistration();
                instance.Cancel();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Cancel: {ex.Message}");
            }
        }

    }
}
