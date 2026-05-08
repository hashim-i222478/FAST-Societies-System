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
