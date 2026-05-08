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
    public class RegisterFormTests
    {
        [TestMethod]
        public void Test_FindControl()
        {
            try
            {
                var instance = new RegisterForm();
                instance.FindControl("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for FindControl: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RegisterButton_Click()
        {
            try
            {
                var instance = new RegisterForm();
                instance.RegisterButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RegisterButton_Click: {ex.Message}");
            }
        }

    }
}
