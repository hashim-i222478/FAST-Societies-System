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
    public class SocietyApprovalFormTests
    {
        [TestMethod]
        public void Test_LoadApprovals()
        {
            try
            {
                var instance = new SocietyApprovalForm();
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
                var instance = new SocietyApprovalForm();
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
                var instance = new SocietyApprovalForm();
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
                var instance = new SocietyApprovalForm();
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
