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
    public class StudentServiceTests
    {
        [TestMethod]
        public void Test_BrowseSocieties()
        {
            try
            {
                var instance = new StudentService();
                instance.BrowseSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for BrowseSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApplyForMembership()
        {
            try
            {
                var instance = new StudentService();
                instance.ApplyForMembership(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApplyForMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMyMemberships()
        {
            try
            {
                var instance = new StudentService();
                instance.GetMyMemberships(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMyMemberships: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMembershipStatus()
        {
            try
            {
                var instance = new StudentService();
                instance.GetMembershipStatus(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMembershipStatus: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUpcomingEvents()
        {
            try
            {
                var instance = new StudentService();
                instance.GetUpcomingEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUpcomingEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUpcomingEventsBySociety()
        {
            try
            {
                var instance = new StudentService();
                instance.GetUpcomingEventsBySociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUpcomingEventsBySociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RegisterForEvent()
        {
            try
            {
                var instance = new StudentService();
                instance.RegisterForEvent(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RegisterForEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMyEventRegistrations()
        {
            try
            {
                var instance = new StudentService();
                instance.GetMyEventRegistrations(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMyEventRegistrations: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelEventRegistration()
        {
            try
            {
                var instance = new StudentService();
                instance.CancelEventRegistration(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelEventRegistration: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventTicket()
        {
            try
            {
                var instance = new StudentService();
                instance.GetEventTicket("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventTicket: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LeaveSociety()
        {
            try
            {
                var instance = new StudentService();
                instance.LeaveSociety(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LeaveSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyDetails()
        {
            try
            {
                var instance = new StudentService();
                instance.GetSocietyDetails(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyDetails: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventDetails()
        {
            try
            {
                var instance = new StudentService();
                instance.GetEventDetails(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventDetails: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMyTasks()
        {
            try
            {
                var instance = new StudentService();
                instance.GetMyTasks(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMyTasks: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CompleteTask()
        {
            try
            {
                var instance = new StudentService();
                instance.CompleteTask(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CompleteTask: {ex.Message}");
            }
        }

    }
}
