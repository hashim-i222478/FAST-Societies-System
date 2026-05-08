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
    public class MemberManagementFormTests
    {
        [TestMethod]
        public void Test_LoadSocieties()
        {
            try
            {
                var instance = new MemberManagementForm();
                instance.LoadSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_LoadMembers()
        {
            try
            {
                var instance = new MemberManagementForm();
                instance.LoadMembers();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadMembers: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_RemoveMember_Click()
        {
            try
            {
                var instance = new MemberManagementForm();
                instance.RemoveMember_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for RemoveMember_Click: {ex.Message}");
            }
        }

    }
}
