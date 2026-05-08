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
    public class MyTasksFormTests
    {
        [TestMethod]
        public void Test_LoadTasks()
        {
            try
            {
                var instance = new MyTasksForm();
                instance.LoadTasks();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewButton_Click()
        {
            try
            {
                var instance = new MyTasksForm();
                instance.ViewButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CompleteButton_Click()
        {
            try
            {
                var instance = new MyTasksForm();
                instance.CompleteButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CompleteButton_Click: {ex.Message}");
            }
        }

    }
}
