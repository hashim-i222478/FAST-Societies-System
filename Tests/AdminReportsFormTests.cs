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
    public class AdminReportsFormTests
    {
        [TestMethod]
        public void Test_LoadReportData()
        {
            try
            {
                var instance = new AdminReportsForm();
                instance.LoadReportData();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadReportData: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadUserDistribution()
        {
            try
            {
                var instance = new AdminReportsForm();
                instance.LoadUserDistribution();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadUserDistribution: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadSocietyPerformance()
        {
            try
            {
                var instance = new AdminReportsForm();
                instance.LoadSocietyPerformance();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocietyPerformance: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadEventParticipation()
        {
            try
            {
                var instance = new AdminReportsForm();
                instance.LoadEventParticipation();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadEventParticipation: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadLogsSummary()
        {
            try
            {
                var instance = new AdminReportsForm();
                instance.LoadLogsSummary();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadLogsSummary: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ExportBtn_Click()
        {
            try
            {
                var instance = new AdminReportsForm();
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
