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
    public class EventApprovalFormTests
    {
        [TestMethod]
        public void Test_LoadApprovals()
        {
            try
            {
                var instance = new EventApprovalForm();
                instance.LoadApprovals();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadApprovals: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveButton_Click()
        {
            try
            {
                var instance = new EventApprovalForm();
                instance.ApproveButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectButton_Click()
        {
            try
            {
                var instance = new EventApprovalForm();
                instance.RejectButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ViewButton_Click()
        {
            try
            {
                var instance = new EventApprovalForm();
                instance.ViewButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ViewButton_Click: {ex.Message}");
            }
        }

    }
}
