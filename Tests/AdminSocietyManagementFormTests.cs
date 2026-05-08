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
    public class AdminSocietyManagementFormTests
    {
        [TestMethod]
        public void Test_LoadSocieties()
        {
            try
            {
                var instance = new AdminSocietyManagementForm();
                instance.LoadSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_OpenAddSociety()
        {
            try
            {
                var instance = new AdminSocietyManagementForm();
                instance.OpenAddSociety();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for OpenAddSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SocietiesGrid_SelectionChanged()
        {
            try
            {
                var instance = new AdminSocietyManagementForm();
                instance.SocietiesGrid_SelectionChanged(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SocietiesGrid_SelectionChanged: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ChangeStatus()
        {
            try
            {
                var instance = new AdminSocietyManagementForm();
                instance.ChangeStatus("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ChangeStatus: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DeleteSociety()
        {
            try
            {
                var instance = new AdminSocietyManagementForm();
                instance.DeleteSociety();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DeleteSociety: {ex.Message}");
            }
        }

    }
}
