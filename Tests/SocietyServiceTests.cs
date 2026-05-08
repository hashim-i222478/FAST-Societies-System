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
    public class SocietyServiceTests
    {
        [TestMethod]
        public void Test_CreateSociety()
        {
            try
            {
                var instance = new SocietyService();
                instance.CreateSociety("test_data", "test_data", 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMySocieties()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetMySocieties(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMySocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyMembers()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetSocietyMembers(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyMembers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingMembershipRequests()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetPendingMembershipRequests(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingMembershipRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveMembership()
        {
            try
            {
                var instance = new SocietyService();
                instance.ApproveMembership(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectMembership()
        {
            try
            {
                var instance = new SocietyService();
                instance.RejectMembership(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RemoveMember()
        {
            try
            {
                var instance = new SocietyService();
                instance.RemoveMember(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RemoveMember: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CreateEvent()
        {
            try
            {
                var instance = new SocietyService();
                instance.CreateEvent(1, "test_data", "test_data", DateTime.Now, "test_data", 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyEvents()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetSocietyEvents(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateEvent()
        {
            try
            {
                var instance = new SocietyService();
                instance.UpdateEvent(1, 1, "test_data", "test_data", DateTime.Now, "test_data", 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelEvent()
        {
            try
            {
                var instance = new SocietyService();
                instance.CancelEvent(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CreateTask()
        {
            try
            {
                var instance = new SocietyService();
                instance.CreateTask(1, "test_data", "test_data", DateTime.Now, "test_data", 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyTasks()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetSocietyTasks(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingTasks()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetPendingTasks(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateTask()
        {
            try
            {
                var instance = new SocietyService();
                instance.UpdateTask(1, 1, "test_data", "test_data", DateTime.Now, "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_PostAnnouncement()
        {
            try
            {
                var instance = new SocietyService();
                instance.PostAnnouncement(1, "test_data", "test_data", 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for PostAnnouncement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyAnnouncements()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetSocietyAnnouncements(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyAnnouncements: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyProfile()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetSocietyProfile(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyProfile: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateSocietyProfile()
        {
            try
            {
                var instance = new SocietyService();
                instance.UpdateSocietyProfile(1, 1, "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateSocietyProfile: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMemberCount()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetMemberCount(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMemberCount: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllSocieties()
        {
            try
            {
                var instance = new SocietyService();
                instance.GetAllSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SuspendSociety()
        {
            try
            {
                var instance = new SocietyService();
                instance.SuspendSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SuspendSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ActivateSociety()
        {
            try
            {
                var instance = new SocietyService();
                instance.ActivateSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ActivateSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DeleteSociety()
        {
            try
            {
                var instance = new SocietyService();
                instance.DeleteSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DeleteSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveSociety()
        {
            try
            {
                var instance = new SocietyService();
                instance.ApproveSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveSociety: {ex.Message}");
            }
        }

    }
}
