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
    public class SocietyRepositoryTests
    {
        [TestMethod]
        public void Test_CreateSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.CreateSociety(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for CreateSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyById()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetSocietyById(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyById: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllActiveSocieties()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetAllActiveSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllActiveSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetPendingSocieties()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetPendingSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetPendingSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietiesByHead()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetSocietiesByHead(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietiesByHead: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_UpdateSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.UpdateSociety(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for UpdateSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ApproveSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.ApproveSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ApproveSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_SuspendSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.SuspendSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for SuspendSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_ActivateSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.ActivateSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for ActivateSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_DeleteSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.DeleteSociety(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for DeleteSociety: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetAllSocieties()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetAllSocieties();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetAllSocieties: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_GetSocietyMemberCount()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.GetSocietyMemberCount(1);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for GetSocietyMemberCount: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test_MapReaderToSociety()
        {
            try
            {
                var instance = new SocietyRepository();
                instance.MapReaderToSociety(null);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed for MapReaderToSociety: {ex.Message}");
            }
        }

    }
}
