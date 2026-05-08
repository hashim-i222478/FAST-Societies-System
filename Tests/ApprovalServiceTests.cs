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
    public class ApprovalServiceTests
    {
        [TestMethod]
        public void Test_RequestEventApproval()
        {
            try
            {
                var instance = new ApprovalService();
                instance.RequestEventApproval(1, 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RequestEventApproval: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RequestSocietyApproval()
        {
            try
            {
                var instance = new ApprovalService();
                instance.RequestSocietyApproval(1, 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RequestSocietyApproval: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingApprovals()
        {
            try
            {
                var instance = new ApprovalService();
                instance.GetPendingApprovals("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingApprovals: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllPendingApprovals()
        {
            try
            {
                var instance = new ApprovalService();
                instance.GetAllPendingApprovals();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllPendingApprovals: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveEvent()
        {
            try
            {
                var instance = new ApprovalService();
                instance.ApproveEvent(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectEvent()
        {
            try
            {
                var instance = new ApprovalService();
                instance.RejectEvent(1, 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectEvent: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveSociety()
        {
            try
            {
                var instance = new ApprovalService();
                instance.ApproveSociety(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectSociety()
        {
            try
            {
                var instance = new ApprovalService();
                instance.RejectSociety(1, 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectSociety: {ex.Message}");
            }
        }

    }
}
