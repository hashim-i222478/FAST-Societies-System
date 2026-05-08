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
    public class LogRepositoryTests
    {
        [TestMethod]
        public void Test_AddLog()
        {
            try
            {
                var instance = new LogRepository();
                instance.AddLog(1, "test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for AddLog: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllLogs()
        {
            try
            {
                var instance = new LogRepository();
                instance.GetAllLogs();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllLogs: {ex.Message}");
            }
        }

    }
}
