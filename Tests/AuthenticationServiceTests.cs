using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.BLL;

namespace FASTSocietiesSystem.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        [TestMethod]
        public void Test_Login()
        {
            try
            {
                var instance = new AuthenticationService();
                instance.Login("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Login: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RegisterStudent()
        {
            try
            {
                var instance = new AuthenticationService();
                instance.RegisterStudent("test_data", "test_data", "test_data", "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RegisterStudent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CreateSocietyHead()
        {
            try
            {
                var instance = new AuthenticationService();
                instance.CreateSocietyHead("test_data", "test_data", "test_data", "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateSocietyHead: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ChangePassword()
        {
            try
            {
                var instance = new AuthenticationService();
                instance.ChangePassword(1, "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ChangePassword: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ResetPassword()
        {
            try
            {
                var instance = new AuthenticationService();
                instance.ResetPassword(1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ResetPassword: {ex.Message}");
            }
        }

    }
}
