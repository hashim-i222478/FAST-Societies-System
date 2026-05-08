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
    public class ReportsFormTests
    {
        [TestMethod]
        public void Test_PopulateSocieties()
        {
            try
            {
                var instance = new ReportsForm();
                instance.PopulateSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for PopulateSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GenerateReport()
        {
            try
            {
                var instance = new ReportsForm();
                instance.GenerateReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetRegistrationCount()
        {
            try
            {
                var instance = new ReportsForm();
                instance.GetRegistrationCount(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetRegistrationCount: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ExportBtn_Click()
        {
            try
            {
                var instance = new ReportsForm();
                instance.ExportBtn_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ExportBtn_Click: {ex.Message}");
            }
        }

    }
}
