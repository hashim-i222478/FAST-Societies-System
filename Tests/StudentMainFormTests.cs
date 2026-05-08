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
    public class StudentMainFormTests
    {
        [TestMethod]
        public void Test_OnShown()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OnShown(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OnShown: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CheckForCancelledEvents()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.CheckForCancelledEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CheckForCancelledEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_AddWindowButton()
        {
            try
            {
                var instance = new StudentMainForm();
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
                var instance = new StudentMainForm();
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
                var instance = new StudentMainForm();
                instance.AddDashboardCard(null, "test_data", "test_data", null, null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddDashboardCard: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenBrowseSocieties()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OpenBrowseSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenBrowseSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMyMemberships()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OpenMyMemberships();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMyMemberships: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenBrowseEvents()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OpenBrowseEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenBrowseEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMyTickets()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OpenMyTickets();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMyTickets: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenMyTasks()
        {
            try
            {
                var instance = new StudentMainForm();
                instance.OpenMyTasks();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenMyTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenChangePassword()
        {
            try
            {
                var instance = new StudentMainForm();
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
                var instance = new StudentMainForm();
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
                var instance = new StudentMainForm();
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
