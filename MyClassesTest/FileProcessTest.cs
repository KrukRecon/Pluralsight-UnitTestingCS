using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;

namespace MyClassesTest
{
    [TestClass]
    public class FileProcessTest : TestBase
    {
        private const string BAD_FILE_NAME = @"C:\Windows\Bogus.exe";

        [ClassInitialize()]
        public static void ClassInitialize(TestContext tc)
        {
            tc.WriteLine("In ClassInitialize() method");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestContext.WriteLine("In TestInitialize() method");
            WriteDescription(this.GetType());

            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                SetGoodFileName();
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine($"Creating file: {_GoodFileName}");

                    File.AppendAllText(_GoodFileName, "Some text");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestContext.WriteLine("In TestCleanup() method");

            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                if (File.Exists(_GoodFileName))
                {
                    File.Delete(_GoodFileName);
                }
            }
        }

        [TestMethod]
        [Description("Check to se if file exists")]
        [Owner("Majki")]
        [Priority(1)]
        public void FileNameDoesExist()
        {
            FileProcess fp = new();
            bool fromCall;

            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [DataRow(1, 1, DisplayName = "First")]
        [DataRow(42, 42, DisplayName = "Second")]
        public void AreNumbersEqual(int x, int y)
        {
            Assert.AreEqual(x, y);
        }

        //[TestMethod]
        //[DeploymentItem("FileToDeploy.txt")]
        //[DataRow("", DisplayName = "First")]
        //[DataRow("", DisplayName = "Second")]
        //Bla bla input method that copies from file in data row to deployment item or whatever

        [TestMethod]
        [Timeout(3000)]
        public void SimulateTimeout()
        {
            System.Threading.Thread.Sleep(4000);
            //Sleep was longer than time limit 
        }

        [TestMethod]
        [Description("Check to se if file does not exists")]
        [Owner("Majki")]
        [Priority(1)]
        public void FileNameDoesNotExist()
        {
            FileProcess fp = new();
            bool fromCall;

            TestContext.WriteLine($"Checking File {BAD_FILE_NAME}");

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Fajki")]
        [Priority(2)]
        public void FileNameNullOrEmpty_UsingAttribute()
        {
            FileProcess fp = new();

            TestContext.WriteLine(@"Checking for a null file");

            fp.FileExists("");
        }

        [TestMethod]
        [Owner("Fajki")]
        [Priority(2)]
        public void FileNameNullOrEmpty_UsingTryCatch()
        {
            FileProcess fp = new();

            TestContext.WriteLine(@"Checking for a null file");

            try {
                fp.FileExists("");
            }
            catch (ArgumentNullException) {
                // Test was a success
                return;
            }

            // Fail the test
            Assert.Fail("Call to FileExists() did NOT throw an ArgumentNullException.");
        }
    }
}
