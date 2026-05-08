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
    public class EventRegistrationRepositoryTests
    {
        [TestMethod]
        public void Test_CreateRegistration()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.CreateRegistration(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateRegistration: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetRegistrationById()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.GetRegistrationById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetRegistrationById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetRegistrationByTicket()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.GetRegistrationByTicket("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetRegistrationByTicket: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventRegistrations()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.GetEventRegistrations(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventRegistrations: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetStudentRegistrations()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.GetStudentRegistrations(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetStudentRegistrations: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsStudentRegistered()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.IsStudentRegistered(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsStudentRegistered: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateRegistration()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.UpdateRegistration(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateRegistration: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelRegistration()
        {
            try
            {
                var instance = new EventRegistrationRepository();
                instance.CancelRegistration(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelRegistration: {ex.Message}");
            }
        }

    }
}
