/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Xml;

namespace ConfigurationTests.Tests.Unit
{
    public interface ITwice { }
    public class Cod : ITwice { }

    [TestClass]
    public class IoCResolutionTestTests
    {
        private IoCResolutionTest _IoCResolutionTest;
        public interface IFish { }
        public interface ITwice { }
        public class Cod : IFish { }
        public interface IComputer { }
        public class AppleMacintosh : IComputer { }

        [TestInitialize]
        public void MyTestInitialize()
        {
            _IoCResolutionTest = new IoCResolutionTest();            
        }

        [TestMethod]
        public void TestInterfaceNotFound()
        {
            _IoCResolutionTest.Interface = "IDave";

            try
            {
                _IoCResolutionTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual(ex.Message, "No interface found with the name IDave");
            }
        }

        [TestMethod]
        public void TestInterfaceDefinedTwice()
        {
            _IoCResolutionTest.Interface = "ITwice";
            try
            {
                _IoCResolutionTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual(ex.Message, "Interface ITwice defined multiple times");
            }
        }

        [TestMethod]
        public void TestExpectedTypeNotFound()
        {
            _IoCResolutionTest.Interface = "IFish";
            _IoCResolutionTest.ExpectedType = "Trout";
            try
            {
                _IoCResolutionTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual(ex.Message, "No Type found with the name Trout");
            }
        }

        [TestMethod]
        public void TestExpectedTypeDefinedTwice()
        {
            _IoCResolutionTest.Interface = "IFish";
            _IoCResolutionTest.ExpectedType = "Cod";
            try
            {
                _IoCResolutionTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual(ex.Message, "Type Cod defined multiple times");
            }
        }

        [TestMethod]
        public void TestSuccessfulIoCResolution()
        {
            _IoCResolutionTest.Interface = "IComputer";
            _IoCResolutionTest.ExpectedType = "AppleMacintosh";
            _IoCResolutionTest.Run();
        }
    }
}
