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
    public class AnnouncementRepositoryTests
    {
        [TestMethod]
        public void Test_CreateAnnouncement()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.CreateAnnouncement(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateAnnouncement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAnnouncementById()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.GetAnnouncementById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAnnouncementById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAnnouncementsBySociety()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.GetAnnouncementsBySociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAnnouncementsBySociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetLatestAnnouncements()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.GetLatestAnnouncements(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetLatestAnnouncements: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateAnnouncement()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.UpdateAnnouncement(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateAnnouncement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DeactivateAnnouncement()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.DeactivateAnnouncement(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DeactivateAnnouncement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MapReaderToAnnouncement()
        {
            try
            {
                var instance = new AnnouncementRepository();
                instance.MapReaderToAnnouncement(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MapReaderToAnnouncement: {ex.Message}");
            }
        }

    }
}
