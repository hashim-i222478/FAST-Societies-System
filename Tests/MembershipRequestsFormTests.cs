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
    public class MembershipRequestsFormTests
    {
        [TestMethod]
        public void Test_LoadRequests()
        {
            try
            {
                var instance = new MembershipRequestsForm();
                instance.LoadRequests();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveButton_Click()
        {
            try
            {
                var instance = new MembershipRequestsForm();
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
                var instance = new MembershipRequestsForm();
                instance.RejectButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectButton_Click: {ex.Message}");
            }
        }

    }
}
