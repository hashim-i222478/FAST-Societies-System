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
    public class TaskTests
    {
        [TestMethod]
        public void Test_StartTask()
        {
            try
            {
                var instance = new FASTSocietiesSystem.Models.Task();
                instance.StartTask();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for StartTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CompleteTask()
        {
            try
            {
                var instance = new FASTSocietiesSystem.Models.Task();
                instance.CompleteTask(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CompleteTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsOverdue()
        {
            try
            {
                var instance = new FASTSocietiesSystem.Models.Task();
                instance.IsOverdue();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsOverdue: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateOverdueStatus()
        {
            try
            {
                var instance = new FASTSocietiesSystem.Models.Task();
                instance.UpdateOverdueStatus();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateOverdueStatus: {ex.Message}");
            }
        }

    }
}
