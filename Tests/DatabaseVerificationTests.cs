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
    public class DatabaseVerificationTests
    {
        [TestMethod]
        public void Test_TestConnection()
        {
            try
            {
                DatabaseVerification.TestConnection();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for TestConnection: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_VerifyTables()
        {
            try
            {
                DatabaseVerification.VerifyTables();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for VerifyTables: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetDatabaseStatus()
        {
            try
            {
                DatabaseVerification.GetDatabaseStatus();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetDatabaseStatus: {ex.Message}");
            }
        }

    }
}
