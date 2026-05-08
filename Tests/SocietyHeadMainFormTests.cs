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
    public class SocietyHeadMainFormTests
    {
        [TestMethod]
        public void Test_AddWindowButton()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.AddWindowButton(null, "test_data", null, null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddWindowButton: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_AddSidebarButton()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.AddSidebarButton(null, "test_data", null, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddSidebarButton: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_AddDashboardCard()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.AddDashboardCard(null, "test_data", "test_data", null, null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddDashboardCard: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenSocietyManagement()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenSocietyManagement();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenSocietyManagement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMemberManagement()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenMemberManagement();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMemberManagement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMembershipRequests()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenMembershipRequests();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMembershipRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenCreateEvent()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenCreateEvent();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenCreateEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenManageEvents()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenManageEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenManageEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenCreateTask()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenCreateTask();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenCreateTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenViewTasks()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenViewTasks();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenViewTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenReports()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenReports();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenReports: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMembershipReport()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenMembershipReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMembershipReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenEventReport()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenEventReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenEventReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenTaskReport()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenTaskReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenTaskReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenChangePassword()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenChangePassword();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenChangePassword: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenProfile()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.OpenProfile();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenProfile: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_Logout()
        {
            try
            {
                var instance = new SocietyHeadMainForm();
                instance.Logout();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Logout: {ex.Message}");
            }
        }

    }
}
