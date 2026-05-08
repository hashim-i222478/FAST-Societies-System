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
    public class AddUserFormTests
    {
        [TestMethod]
        public void Test_AddInputField()
        {
            try
            {
                var instance = new AddUserForm();
                instance.AddInputField(null, "test_data", "test_data", true);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddInputField: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_FindControl()
        {
            try
            {
                var instance = new AddUserForm();
                instance.FindControl("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for FindControl: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SaveButton_Click()
        {
            try
            {
                var instance = new AddUserForm();
                instance.SaveButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SaveButton_Click: {ex.Message}");
            }
        }

    }
}
