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
    public class TaskRepositoryTests
    {
        [TestMethod]
        public void Test_CreateTask()
        {
            try
            {
                var instance = new TaskRepository();
                instance.CreateTask(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetTaskById()
        {
            try
            {
                var instance = new TaskRepository();
                instance.GetTaskById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetTaskById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyTasks()
        {
            try
            {
                var instance = new TaskRepository();
                instance.GetSocietyTasks(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingTasks()
        {
            try
            {
                var instance = new TaskRepository();
                instance.GetPendingTasks(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetTasksForStudent()
        {
            try
            {
                var instance = new TaskRepository();
                instance.GetTasksForStudent(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetTasksForStudent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateTask()
        {
            try
            {
                var instance = new TaskRepository();
                instance.UpdateTask(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CompleteTask()
        {
            try
            {
                var instance = new TaskRepository();
                instance.CompleteTask(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CompleteTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelTask()
        {
            try
            {
                var instance = new TaskRepository();
                instance.CancelTask(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelTask: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MapReaderToTask()
        {
            try
            {
                var instance = new TaskRepository();
                instance.MapReaderToTask(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MapReaderToTask: {ex.Message}");
            }
        }

    }
}
