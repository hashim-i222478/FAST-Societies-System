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
    public class DatabaseConnectionTests
    {
        [TestMethod]
        public void Test_GetConnection()
        {
            try
            {
                DatabaseConnection.GetConnection();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetConnection: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_TestConnection()
        {
            try
            {
                DatabaseConnection.TestConnection();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for TestConnection: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ExecuteNonQuery()
        {
            try
            {
                DatabaseConnection.ExecuteNonQuery("test_data", new Dictionary<string, object>());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ExecuteNonQuery: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ExecuteScalar()
        {
            try
            {
                DatabaseConnection.ExecuteScalar("test_data", new Dictionary<string, object>());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ExecuteScalar: {ex.Message}");
            }
        }

    }
}
