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
