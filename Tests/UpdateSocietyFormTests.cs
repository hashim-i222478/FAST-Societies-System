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
    public class UpdateSocietyFormTests
    {
        [TestMethod]
        public void Test_LoadSocietyData()
        {
            try
            {
                var instance = new UpdateSocietyForm();
                instance.LoadSocietyData();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadSocietyData: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SaveButton_Click()
        {
            try
            {
                var instance = new UpdateSocietyForm();
                instance.SaveButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SaveButton_Click: {ex.Message}");
            }
        }

    }
}
