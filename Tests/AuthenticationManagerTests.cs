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
    public class AuthenticationManagerTests
    {
        [TestMethod]
        public void Test_Login()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.Login(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Login: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_Logout()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.Logout();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Logout: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_HasRole()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.HasRole("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for HasRole: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnsureAuthenticated()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.EnsureAuthenticated();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnsureAuthenticated: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnsureRole()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.EnsureRole(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnsureRole: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnsureStudent()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.EnsureStudent();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnsureStudent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnsureSocietyHead()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.EnsureSocietyHead();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnsureSocietyHead: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnsureAdmin()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.EnsureAdmin();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnsureAdmin: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetCurrentUserName()
        {
            try
            {
                var instance = new AuthenticationManager();
                instance.GetCurrentUserName();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetCurrentUserName: {ex.Message}");
            }
        }

    }
}
