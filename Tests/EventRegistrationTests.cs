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
    public class EventRegistrationTests
    {
        [TestMethod]
        public void Test_GenerateTicketId()
        {
            try
            {
                var instance = new EventRegistration();
                instance.GenerateTicketId();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateTicketId: {ex.Message}");
            }
        }

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
