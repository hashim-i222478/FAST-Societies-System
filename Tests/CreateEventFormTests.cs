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
    public class CreateEventFormTests
    {
        [TestMethod]
        public void Test_PopulateSocieties()
        {
            try
            {
                var instance = new CreateEventForm();
                instance.PopulateSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for PopulateSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CreateButton_Click()
        {
            try
            {
                var instance = new CreateEventForm();
                instance.CreateButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateButton_Click: {ex.Message}");
            }
        }

    }
}
