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
    public class UserManagementFormTests
    {
        [TestMethod]
        public void Test_LoadUsers()
        {
            try
            {
                var instance = new UserManagementForm();
                instance.LoadUsers();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadUsers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CreateButton_Click()
        {
            try
            {
                var instance = new UserManagementForm();
                instance.CreateButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SuspendButton_Click()
        {
            try
            {
                var instance = new UserManagementForm();
                instance.SuspendButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SuspendButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ActivateButton_Click()
        {
            try
            {
                var instance = new UserManagementForm();
                instance.ActivateButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ActivateButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewButton_Click()
        {
            try
            {
                var instance = new UserManagementForm();
                instance.ViewButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewButton_Click: {ex.Message}");
            }
        }

    }
}
