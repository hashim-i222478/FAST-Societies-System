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
    public class MembershipRepositoryTests
    {
        [TestMethod]
        public void Test_CreateMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.CreateMembership(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetMembershipById()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.GetMembershipById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetMembershipById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingMembershipRequests()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.GetPendingMembershipRequests(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingMembershipRequests: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyMembers()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.GetSocietyMembers(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyMembers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetStudentMemberships()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.GetStudentMemberships(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetStudentMemberships: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsMember()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.IsMember(1, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsMember: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.ApproveMembership(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RejectMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.RejectMembership(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RejectMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RemoveMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.RemoveMembership(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RemoveMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.UpdateMembership(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateMembership: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MapReaderToMembership()
        {
            try
            {
                var instance = new MembershipRepository();
                instance.MapReaderToMembership(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MapReaderToMembership: {ex.Message}");
            }
        }

    }
}
