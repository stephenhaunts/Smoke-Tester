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
using System.Text;

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class ASMXServiceSoapTestTests
    {

        [TestMethod]
        [TestCategory("Integration")]
        public void CheckServiceReturnsResponse()
        {
            var test = new ASMXServiceSoapTest { TestName = "Loan Service Soap Test" };
            test.ServiceAddress = "http://ukqaoftsoa03:90/CreditSearchProxyService/Ambernet.asmx";
            test.DefaultNamespace = "http://www.ambernet.dfguk.com/";
            test.SoapRequestAction = "\"http://www.ambernet.dfguk.com/GetCTPCreditLimit\"";

            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("<GetCTPCreditLimit xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.ambernet.dfguk.com/\">");
            requestBody.Append("<pUserId>0</pUserId>");
            requestBody.Append("<pCustomerCode>UK05073809696X</pCustomerCode>");
            requestBody.Append("<IsExperianCheckRequired>false</IsExperianCheckRequired>");
            requestBody.Append("<IsBankAccountCheckRequired>false</IsBankAccountCheckRequired>");
            requestBody.Append("</GetCTPCreditLimit>");
            test.SoapRequestBody = requestBody.ToString();

            StringBuilder responseBody = new StringBuilder();
            responseBody.Append("<GetCTPCreditLimitResponse xmlns=\"http://www.ambernet.dfguk.com/\">");
            responseBody.Append("<GetCTPCreditLimitResult>");
            responseBody.Append("<ConfirmationDTO>");
            responseBody.Append("<Response>wrSuccess</Response>");
            responseBody.Append("<FailureEnum>frNone</FailureEnum>");
            responseBody.Append("<Reason>Call Successful</Reason>");
            responseBody.Append("<ConfirmationDate>2013-03-28T15:46:34.9946292+00:00</ConfirmationDate>");
            responseBody.Append("</ConfirmationDTO>");
            responseBody.Append("<CustomerCode>UK05073809696X</CustomerCode>");
            responseBody.Append("<CreditLimit>-1</CreditLimit>");
            responseBody.Append("<IsExperianCheckSuccessful>0</IsExperianCheckSuccessful>");
            responseBody.Append("<IsBankAccountCheckSuccessful>0</IsBankAccountCheckSuccessful>");
            responseBody.Append("<RepeatCheck>false</RepeatCheck>");
            responseBody.Append("</GetCTPCreditLimitResult></GetCTPCreditLimitResponse>");
            test.ExpectedSoapResponseBody = responseBody.ToString();
            test.XPathNodesToBeRemoved = "//response:ConfirmationDate";

            test.Run();
        }
    }
}
