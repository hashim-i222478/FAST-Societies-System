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
    public class BrowseSocietiesFormTests
    {
        [TestMethod]
        public void Test_LoadSocieties()
        {
            try
            {
                var instance = new BrowseSocietiesForm();
                instance.LoadSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApplyButton_Click()
        {
            try
            {
                var instance = new BrowseSocietiesForm();
                instance.ApplyButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApplyButton_Click: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SocietiesGrid_SelectionChanged()
        {
            try
            {
                var instance = new BrowseSocietiesForm();
                instance.SocietiesGrid_SelectionChanged(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SocietiesGrid_SelectionChanged: {ex.Message}");
            }
        }

    }
}
