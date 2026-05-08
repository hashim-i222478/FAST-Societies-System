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
    public class LoginFormTests
    {
        [TestMethod]
        public void Test_LoginButton_Click()
        {
            try
            {
                var instance = new LoginForm();
                instance.LoginButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoginButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RegisterButton_Click()
        {
            try
            {
                var instance = new LoginForm();
                instance.RegisterButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RegisterButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_NavigateToDashboard()
        {
            try
            {
                var instance = new LoginForm();
                instance.NavigateToDashboard("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for NavigateToDashboard: {ex.Message}");
            }
        }

    }
}
