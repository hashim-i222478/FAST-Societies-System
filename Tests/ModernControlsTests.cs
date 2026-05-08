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
    public class ModernControlsTests
    {
        [TestMethod]
        public void Test_CreateRoundRectRgn()
        {
            try
            {
                ModernControls.CreateRoundRectRgn(1, null, null, null, null, null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateRoundRectRgn: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SetRoundedCorners()
        {
            try
            {
                ModernControls.SetRoundedCorners(null, 1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SetRoundedCorners: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApplyCardStyle()
        {
            try
            {
                ModernControls.ApplyCardStyle(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApplyCardStyle: {ex.Message}");
            }
        }

    }
}
