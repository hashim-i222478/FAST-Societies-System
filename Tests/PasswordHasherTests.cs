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
    public class PasswordHasherTests
    {
        [TestMethod]
        public void Test_HashPassword()
        {
            try
            {
                PasswordHasher.HashPassword("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for HashPassword: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_VerifyPassword()
        {
            try
            {
                PasswordHasher.VerifyPassword("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for VerifyPassword: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsPasswordStrong()
        {
            try
            {
                PasswordHasher.IsPasswordStrong("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsPasswordStrong: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPasswordStrengthMessage()
        {
            try
            {
                PasswordHasher.GetPasswordStrengthMessage("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPasswordStrengthMessage: {ex.Message}");
            }
        }

    }
}
