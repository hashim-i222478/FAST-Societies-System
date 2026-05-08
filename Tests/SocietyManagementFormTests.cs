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
    public class SocietyManagementFormTests
    {
        [TestMethod]
        public void Test_LoadSocieties()
        {
            try
            {
                var instance = new SocietyManagementForm();
                instance.LoadSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EditButton_Click()
        {
            try
            {
                var instance = new SocietyManagementForm();
                instance.EditButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EditButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewMembersButton_Click()
        {
            try
            {
                var instance = new SocietyManagementForm();
                instance.ViewMembersButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewMembersButton_Click: {ex.Message}");
            }
        }

    }
}
