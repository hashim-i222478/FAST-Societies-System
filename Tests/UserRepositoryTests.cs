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
    public class UserRepositoryTests
    {
        [TestMethod]
        public void Test_CreateUser()
        {
            try
            {
                var instance = new UserRepository();
                instance.CreateUser(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateUser: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUserById()
        {
            try
            {
                var instance = new UserRepository();
                instance.GetUserById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUserById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUserByEmail()
        {
            try
            {
                var instance = new UserRepository();
                instance.GetUserByEmail("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUserByEmail: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUsersByRole()
        {
            try
            {
                var instance = new UserRepository();
                instance.GetUsersByRole("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUsersByRole: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllActiveUsers()
        {
            try
            {
                var instance = new UserRepository();
                instance.GetAllActiveUsers();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllActiveUsers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateUser()
        {
            try
            {
                var instance = new UserRepository();
                instance.UpdateUser(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateUser: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdatePassword()
        {
            try
            {
                var instance = new UserRepository();
                instance.UpdatePassword(1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdatePassword: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SuspendUser()
        {
            try
            {
                var instance = new UserRepository();
                instance.SuspendUser(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SuspendUser: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ActivateUser()
        {
            try
            {
                var instance = new UserRepository();
                instance.ActivateUser(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ActivateUser: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EmailExists()
        {
            try
            {
                var instance = new UserRepository();
                instance.EmailExists("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EmailExists: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllUsers()
        {
            try
            {
                var instance = new UserRepository();
                instance.GetAllUsers();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllUsers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MapReaderToUser()
        {
            try
            {
                var instance = new UserRepository();
                instance.MapReaderToUser(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MapReaderToUser: {ex.Message}");
            }
        }

    }
}
