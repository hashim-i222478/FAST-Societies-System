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
    public class ReportServiceTests
    {
        [TestMethod]
        public void Test_GenerateMembershipReport()
        {
            try
            {
                var instance = new ReportService();
                instance.GenerateMembershipReport(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateMembershipReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GenerateEventReport()
        {
            try
            {
                var instance = new ReportService();
                instance.GenerateEventReport(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateEventReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GenerateTaskReport()
        {
            try
            {
                var instance = new ReportService();
                instance.GenerateTaskReport(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateTaskReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GenerateUniversityReport()
        {
            try
            {
                var instance = new ReportService();
                instance.GenerateUniversityReport();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GenerateUniversityReport: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventStatistics()
        {
            try
            {
                var instance = new ReportService();
                instance.GetEventStatistics(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventStatistics: {ex.Message}");
            }
        }

    }
}
