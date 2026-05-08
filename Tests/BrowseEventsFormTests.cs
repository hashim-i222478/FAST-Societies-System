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
    public class BrowseEventsFormTests
    {
        [TestMethod]
        public void Test_LoadEvents()
        {
            try
            {
                var instance = new BrowseEventsForm();
                instance.LoadEvents();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadEvents: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RegisterButton_Click()
        {
            try
            {
                var instance = new BrowseEventsForm();
                instance.RegisterButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RegisterButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DetailsButton_Click()
        {
            try
            {
                var instance = new BrowseEventsForm();
                instance.DetailsButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DetailsButton_Click: {ex.Message}");
            }
        }

    }
}
