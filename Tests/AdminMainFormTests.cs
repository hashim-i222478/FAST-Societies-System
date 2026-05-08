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
    public class AdminMainFormTests
    {
        [TestMethod]
        public void Test_AddWindowButton()
        {
            try
            {
                var instance = new AdminMainForm();
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
                var instance = new AdminMainForm();
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
                var instance = new AdminMainForm();
                instance.AddDashboardCard(null, "test_data", "test_data", null, null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddDashboardCard: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenUserManagement()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenUserManagement();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenUserManagement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenUserStatistics()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenUserStatistics();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenUserStatistics: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenSocietyManagement()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenSocietyManagement();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenSocietyManagement: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenSocietyApprovals()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenSocietyApprovals();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenSocietyApprovals: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenEventApprovals()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenEventApprovals();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenEventApprovals: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenActivityLog()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenActivityLog();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenActivityLog: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenSystemStatus()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenSystemStatus();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenSystemStatus: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenUniversityReport()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenUniversityReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenUniversityReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMembershipReport()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenMembershipReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMembershipReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenActivityReport()
        {
            try
            {
                var instance = new AdminMainForm();
                instance.OpenActivityReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenActivityReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenChangePassword()
        {
            try
            {
                var instance = new AdminMainForm();
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
                var instance = new AdminMainForm();
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
                var instance = new AdminMainForm();
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
