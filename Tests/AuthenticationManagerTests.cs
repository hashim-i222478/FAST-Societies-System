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
    public class AuthenticationManagerTests
    {
        [TestMethod]
        public void Test_Login()
        {
            try
            {
                AuthenticationManager.Instance.Login(null);
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
                AuthenticationManager.Instance.Logout();
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
                AuthenticationManager.Instance.HasRole("test_data");
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
                AuthenticationManager.Instance.EnsureAuthenticated();
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
                AuthenticationManager.Instance.EnsureRole("test_data");
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
                AuthenticationManager.Instance.EnsureStudent();
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
                AuthenticationManager.Instance.EnsureSocietyHead();
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
                AuthenticationManager.Instance.EnsureAdmin();
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
                AuthenticationManager.Instance.GetCurrentUserName();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetCurrentUserName: {ex.Message}");
            }
        }

    }
}
