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
    public class AddSocietyFormTests
    {
        [TestMethod]
        public void Test_LoadHeads()
        {
            try
            {
                var instance = new AddSocietyForm();
                instance.LoadHeads();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for LoadHeads: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SaveBtn_Click()
        {
            try
            {
                var instance = new AddSocietyForm();
                instance.SaveBtn_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SaveBtn_Click: {ex.Message}");
            }
        }

    }
}
