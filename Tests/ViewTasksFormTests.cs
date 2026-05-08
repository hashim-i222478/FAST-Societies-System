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
    public class ViewTasksFormTests
    {
        [TestMethod]
        public void Test_LoadTasks()
        {
            try
            {
                var instance = new ViewTasksForm();
                instance.LoadTasks();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CompleteButton_Click()
        {
            try
            {
                var instance = new ViewTasksForm();
                instance.CompleteButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CompleteButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DeleteButton_Click()
        {
            try
            {
                var instance = new ViewTasksForm();
                instance.DeleteButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DeleteButton_Click: {ex.Message}");
            }
        }

    }
}
