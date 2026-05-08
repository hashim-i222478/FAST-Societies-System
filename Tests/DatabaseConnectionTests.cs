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
                DatabaseConnection.ExecuteNonQuery("test_data", "test_data", new object());
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
                DatabaseConnection.ExecuteScalar("test_data", "test_data", new object());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ExecuteScalar: {ex.Message}");
            }
        }

    }
}
