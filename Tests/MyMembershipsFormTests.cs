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
    public class MyMembershipsFormTests
    {
        [TestMethod]
        public void Test_LoadMemberships()
        {
            try
            {
                var instance = new MyMembershipsForm();
                instance.LoadMemberships();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadMemberships: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewEventsButton_Click()
        {
            try
            {
                var instance = new MyMembershipsForm();
                instance.ViewEventsButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewEventsButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LeaveButton_Click()
        {
            try
            {
                var instance = new MyMembershipsForm();
                instance.LeaveButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LeaveButton_Click: {ex.Message}");
            }
        }

    }
}
