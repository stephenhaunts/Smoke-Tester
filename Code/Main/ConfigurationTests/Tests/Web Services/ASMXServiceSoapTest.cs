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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ConfigurationTests.Tests
{

    /// <summary>
    /// Class used to test the soap response of a ASMX service.
    /// </summary>
    public class ASMXServiceSoapTest : SoapTest
    {
        private const string DEFAULT_CONTENT_TYPE = "text/xml; charset=utf-8";
        private const string DEFAULT_METHOD = "POST";

        public override void Run()
        {
            var soapRequest = new StringBuilder();
            soapRequest.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            soapRequest.Append("<s:Body>");
            soapRequest.Append(SoapRequestBody);
            soapRequest.Append("</s:Body>");
            soapRequest.Append("</s:Envelope>");

            var encodedSoapRequest = new UTF8Encoding();
            byte[] bytesToWrite = encodedSoapRequest.GetBytes(soapRequest.ToString());

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(ServiceAddress);
            webRequest.Timeout = (ConnectionTimeout ?? 10) * 1000;
            webRequest.Method = !string.IsNullOrWhiteSpace(Method) ? Method : DEFAULT_METHOD;
            webRequest.ContentLength = bytesToWrite.Length;
            webRequest.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : DEFAULT_CONTENT_TYPE;
            webRequest.Headers.Add("SOAPAction", SoapRequestAction);

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                newStream.Close();
            }

            var response = (HttpWebResponse)webRequest.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);

            var soapResponse = new StringBuilder();
            soapResponse.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            soapResponse.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            soapResponse.Append("<soap:Body>");
            soapResponse.Append(ExpectedSoapResponseBody);
            soapResponse.Append("</soap:Body>");
            soapResponse.Append("</soap:Envelope>");

            var expectedSoapResponse = new XmlDocument();
            expectedSoapResponse.LoadXml(soapResponse.ToString());

            if (!string.IsNullOrWhiteSpace(XPathNodesToBeRemoved))
            {
                var namespaceManager = new XmlNamespaceManager(expectedSoapResponse.NameTable);
                namespaceManager.AddNamespace("response", DefaultNamespace);

                var root = expectedSoapResponse.DocumentElement;
                var nodeToBeRemoved = root.SelectSingleNode(XPathNodesToBeRemoved, namespaceManager);
                nodeToBeRemoved.ParentNode.RemoveChild(nodeToBeRemoved);
            }

            var actualSoapResponse = new XmlDocument();
            string responseFromServer = reader.ReadToEnd();
            actualSoapResponse.LoadXml(responseFromServer);

            if (!string.IsNullOrWhiteSpace(XPathNodesToBeRemoved))
            {
                var namespaceManager = new XmlNamespaceManager(actualSoapResponse.NameTable);
                namespaceManager.AddNamespace("response", DefaultNamespace);

                var root = actualSoapResponse.DocumentElement;
                var nodeToBeRemoved = root.SelectSingleNode(XPathNodesToBeRemoved, namespaceManager);
                nodeToBeRemoved.ParentNode.RemoveChild(nodeToBeRemoved);
            }

            AssertState.Equal(expectedSoapResponse.OuterXml, actualSoapResponse.OuterXml, "While attempting to get a soap response");
            webRequest.Abort();
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                             {
                                  new WCFServiceSoapTest
                                     {
                                         TestName = "Example Service Endpoint",
                                         ServiceAddress = "http://hostname/location/Service.svc/Service",
                                         DefaultNamespace = "http://www.fakehost.com/",
                                         SoapRequestAction = "http://tempuri.org/IClassBase/MethodOnInterface",
                                         SoapRequestBody = "<MethodName xmlns=\"http://tempuri.org/\" />",
                                         ExpectedSoapResponseAction = "http://tempuri.org/IService/ServiceResponse",
                                         ExpectedSoapResponseBody = "<MethodNameResponse xmlns=\"http://tempuri.org/\"><MethodNameResult>result is here</MethodNameResult></MethodNameResponse>",
                                         ConnectionTimeout = 30
                                     },
                                  new WCFServiceSoapTest
                                     {
                                         TestName = "Example HTTP Endpoint 2",
                                         ServiceAddress = "http://hostname/location/Service.svc/Service",
                                         DefaultNamespace = "http://www.fakehost.com/",
                                         SoapRequestAction = "http://tempuri.org/IService/IService",
                                         SoapRequestBody = "<MethodName xmlns=\"http://tempuri.org/\" />",
                                         ExpectedSoapResponseAction = "http://tempuri.org/IService/ServiceResponse",
                                         ExpectedSoapResponseBody = "<MethodNameResponse xmlns=\"http://tempuri.org/\"><MethodNameResult>result is here</MethodNameResult></MethodNameResponse>",
                                         XPathNodesToBeRemoved = "//response:DateElement"
                                     }
                             };
        }
    }
}
