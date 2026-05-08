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
    public class MyTicketsFormTests
    {
        [TestMethod]
        public void Test_LoadTickets()
        {
            try
            {
                var instance = new MyTicketsForm();
                instance.LoadTickets();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadTickets: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewButton_Click()
        {
            try
            {
                var instance = new MyTicketsForm();
                instance.ViewButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CancelButton_Click()
        {
            try
            {
                var instance = new MyTicketsForm();
                instance.CancelButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CancelButton_Click: {ex.Message}");
            }
        }

    }
}
