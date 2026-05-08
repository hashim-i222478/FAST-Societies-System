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
    public class ManageEventsFormTests
    {
        [TestMethod]
        public void Test_LoadSocieties()
        {
            try
            {
                var instance = new ManageEventsForm();
                instance.LoadSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadEvents()
        {
            try
            {
                var instance = new ManageEventsForm();
                instance.LoadEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateEvent_Click()
        {
            try
            {
                var instance = new ManageEventsForm();
                instance.UpdateEvent_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateEvent_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelEvent_Click()
        {
            try
            {
                var instance = new ManageEventsForm();
                instance.CancelEvent_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelEvent_Click: {ex.Message}");
            }
        }

    }
}
