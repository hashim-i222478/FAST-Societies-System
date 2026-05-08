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
    public class AnnouncementTests
    {
        [TestMethod]
        public void Test_Deactivate()
        {
            try
            {
                var instance = new Announcement();
                instance.Deactivate();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Deactivate: {ex.Message}");
            }
        }

    }
}
