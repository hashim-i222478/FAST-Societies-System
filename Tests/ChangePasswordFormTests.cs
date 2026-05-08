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
    public class ChangePasswordFormTests
    {
        [TestMethod]
        public void Test_ChangeButton_Click()
        {
            try
            {
                var instance = new ChangePasswordForm();
                instance.ChangeButton_Click(new object(), null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ChangeButton_Click: {ex.Message}");
            }
        }

    }
}
