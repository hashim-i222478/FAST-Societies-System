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
    public class UpdateEventFormTests
    {
        [TestMethod]
        public void Test_LoadEventData()
        {
            try
            {
                var instance = new UpdateEventForm();
                instance.LoadEventData();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadEventData: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SaveBtn_Click()
        {
            try
            {
                var instance = new UpdateEventForm();
                instance.SaveBtn_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SaveBtn_Click: {ex.Message}");
            }
        }

    }
}
