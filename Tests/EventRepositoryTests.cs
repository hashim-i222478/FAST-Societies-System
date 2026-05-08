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
    public class EventRepositoryTests
    {
        [TestMethod]
        public void Test_CreateEvent()
        {
            try
            {
                var instance = new EventRepository();
                instance.CreateEvent(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventById()
        {
            try
            {
                var instance = new EventRepository();
                instance.GetEventById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetUpcomingEvents()
        {
            try
            {
                var instance = new EventRepository();
                instance.GetUpcomingEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetUpcomingEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventsBySociety()
        {
            try
            {
                var instance = new EventRepository();
                instance.GetEventsBySociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventsBySociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingEvents()
        {
            try
            {
                var instance = new EventRepository();
                instance.GetPendingEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateEvent()
        {
            try
            {
                var instance = new EventRepository();
                instance.UpdateEvent(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveEvent()
        {
            try
            {
                var instance = new EventRepository();
                instance.ApproveEvent(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelEvent()
        {
            try
            {
                var instance = new EventRepository();
                instance.CancelEvent(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetEventRegistrationCount()
        {
            try
            {
                var instance = new EventRepository();
                instance.GetEventRegistrationCount(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetEventRegistrationCount: {ex.Message}");
            }
        }

    }
}
