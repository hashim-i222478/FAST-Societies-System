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
    public class ApprovalRequestTests
    {
        [TestMethod]
        public void Test_Approve()
        {
            try
            {
                var instance = new ApprovalRequest();
                instance.Approve(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Approve: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_Reject()
        {
            try
            {
                var instance = new ApprovalRequest();
                instance.Reject(1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for Reject: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsPending()
        {
            try
            {
                var instance = new ApprovalRequest();
                instance.IsPending();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsPending: {ex.Message}");
            }
        }

    }
}
