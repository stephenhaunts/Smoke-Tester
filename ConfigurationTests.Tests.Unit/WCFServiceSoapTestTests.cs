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

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class WCFServiceSoapTestTests
    {

        [TestMethod]
        [TestCategory("Integration")]
        public void CheckServiceReturnsResponseWithElementRemoved()
        {
            var test = new WCFServiceSoapTest { TestName = "Loan Service Soap Test" };
            test.ServiceAddress = "http://Server/Services/TestService.svc/TestService/basic";
            test.DefaultNamespace = "http://tempuri.org/";
            test.SoapRequestAction = "http://tempuri.org/IPing/Ping";
            test.SoapRequestBody = "<Ping xmlns=\"http://tempuri.org/\" />";
            test.ExpectedSoapResponseAction = "http://tempuri.org/IPing/PingResponse";
            test.ExpectedSoapResponseBody = "<PingResponse xmlns=\"http://tempuri.org/\"><PingResult>22/03/2013 16:56:35</PingResult></PingResponse>";
            test.XPathNodesToBeRemoved = "//response:PingResult";

            test.Run();
        }
    }
}
