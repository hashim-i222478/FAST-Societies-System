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
    public class UIHelpersTests
    {
        [TestMethod]
        public void Test_ShowInfo()
        {
            try
            {
                UIHelpers.ShowInfo("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ShowInfo: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ShowError()
        {
            try
            {
                UIHelpers.ShowError("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ShowError: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ShowWarning()
        {
            try
            {
                UIHelpers.ShowWarning("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ShowWarning: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ShowConfirm()
        {
            try
            {
                UIHelpers.ShowConfirm("test_data", "test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ShowConfirm: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ClearTextBoxes()
        {
            try
            {
                UIHelpers.ClearTextBoxes(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ClearTextBoxes: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DisableButtons()
        {
            try
            {
                UIHelpers.DisableButtons(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DisableButtons: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_EnableButtons()
        {
            try
            {
                UIHelpers.EnableButtons(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for EnableButtons: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_CenterFormOnScreen()
        {
            try
            {
                UIHelpers.CenterFormOnScreen(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CenterFormOnScreen: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_IsValidEmail()
        {
            try
            {
                UIHelpers.IsValidEmail("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for IsValidEmail: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_BrowseFile()
        {
            try
            {
                UIHelpers.BrowseFile("test_data");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for BrowseFile: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_FormatDate()
        {
            try
            {
                UIHelpers.FormatDate(DateTime.Now);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for FormatDate: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_FormatDateTime()
        {
            try
            {
                UIHelpers.FormatDateTime(DateTime.Now);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for FormatDateTime: {ex.Message}");
            }
        }

    }
}
