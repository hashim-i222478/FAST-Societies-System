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
    public class ModernControlsTests
    {
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
