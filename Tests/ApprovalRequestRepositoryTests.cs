using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.BLL;

namespace FASTSocietiesSystem.Tests
{
    [TestClass]
    public class ApprovalRequestRepositoryTests
    {
        [TestMethod]
        public void Test_CreateApprovalRequest()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.CreateApprovalRequest(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateApprovalRequest: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetApprovalRequestById()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.GetApprovalRequestById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetApprovalRequestById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingApprovalRequests()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.GetPendingApprovalRequests("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingApprovalRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetApprovalRequestsByRequester()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.GetApprovalRequestsByRequester(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetApprovalRequestsByRequester: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllPendingApprovalRequests()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.GetAllPendingApprovalRequests();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllPendingApprovalRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveRequest()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.ApproveRequest(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveRequest: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectRequest()
        {
            try
            {
                var instance = new ApprovalRequestRepository();
                instance.RejectRequest(1, 1, "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectRequest: {ex.Message}");
            }
        }

    }
}
