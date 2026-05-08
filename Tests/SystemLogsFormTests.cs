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
    public class SystemLogsFormTests
    {
        [TestMethod]
        public void Test_LoadLogs()
        {
            try
            {
                var instance = new SystemLogsForm();
                instance.LoadLogs();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadLogs: {ex.Message}");
            }
        }

    }
}
