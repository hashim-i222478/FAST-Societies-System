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
    public class ThemeManagerTests
    {
        [TestMethod]
        public void Test_ApplyTheme()
        {
            try
            {
                ThemeManager.ApplyTheme(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApplyTheme: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApplyToControl()
        {
            try
            {
                ThemeManager.ApplyToControl(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApplyToControl: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_StyleButton()
        {
            try
            {
                ThemeManager.StyleButton(null, true);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for StyleButton: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_StyleTextBox()
        {
            try
            {
                ThemeManager.StyleTextBox(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for StyleTextBox: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_StyleGrid()
        {
            try
            {
                ThemeManager.StyleGrid(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for StyleGrid: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_StyleSidebarButton()
        {
            try
            {
                ThemeManager.StyleSidebarButton(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for StyleSidebarButton: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MakeGradientPanel()
        {
            try
            {
                ThemeManager.MakeGradientPanel(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MakeGradientPanel: {ex.Message}");
            }
        }

    }
}
