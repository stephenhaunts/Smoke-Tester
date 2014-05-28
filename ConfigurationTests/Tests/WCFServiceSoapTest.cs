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
    public class WCFServiceSoapTest : SoapTest
    {
        private const string DEFAULT_CONTENT_TYPE = "application/soap+xml; charset=utf-8";
        private const string DEFAULT_METHOD = "POST";

        public override void Run()
        {
            StringBuilder soapRequest = new StringBuilder();
            soapRequest.Append("<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">");
            soapRequest.Append("<s:Header>");
            soapRequest.Append(string.Concat("<a:Action s:mustUnderstand=\"1\">", SoapRequestAction, "</a:Action>"));
            soapRequest.Append("<a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo>");
            soapRequest.Append(string.Concat("<a:To s:mustUnderstand=\"1\">", ServiceAddress, "</a:To>"));
            soapRequest.Append("</s:Header>");
            soapRequest.Append("<s:Body>");
            soapRequest.Append(SoapRequestBody);
            soapRequest.Append("</s:Body>");
            soapRequest.Append("</s:Envelope>");

            UTF8Encoding encodedSoapRequest = new UTF8Encoding();
            byte[] bytesToWrite = encodedSoapRequest.GetBytes(soapRequest.ToString());

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(ServiceAddress);
            webRequest.Timeout = (ConnectionTimeout ?? 10) * 1000;
            webRequest.Method = !string.IsNullOrWhiteSpace(Method) ? Method : DEFAULT_METHOD;
            webRequest.ContentLength = bytesToWrite.Length;
            webRequest.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : DEFAULT_CONTENT_TYPE;

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                newStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            StringBuilder soapResponse = new StringBuilder();
            soapResponse.Append("<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">");
            soapResponse.Append("<s:Header>");
            soapResponse.Append(string.Concat("<a:Action s:mustUnderstand=\"1\">", ExpectedSoapResponseAction, "</a:Action>"));
            soapResponse.Append("</s:Header>");
            soapResponse.Append("<s:Body>");
            soapResponse.Append(ExpectedSoapResponseBody);
            soapResponse.Append("</s:Body>");
            soapResponse.Append("</s:Envelope>");

            XmlDocument expectedSoapResponse = new XmlDocument();
            expectedSoapResponse.LoadXml(soapResponse.ToString());

            if (!string.IsNullOrWhiteSpace(XPathNodesToBeRemoved))
            {
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(expectedSoapResponse.NameTable);
                namespaceManager.AddNamespace("response", DefaultNamespace);

                XmlNode root = expectedSoapResponse.DocumentElement;
                XmlNode nodeToBeRemoved = root.SelectSingleNode(XPathNodesToBeRemoved, namespaceManager);
                nodeToBeRemoved.ParentNode.RemoveChild(nodeToBeRemoved);
            }

            XmlDocument actualSoapResponse = new XmlDocument();
            string responseFromServer = reader.ReadToEnd();
            actualSoapResponse.LoadXml(responseFromServer);

            if (!string.IsNullOrWhiteSpace(XPathNodesToBeRemoved))
            {
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(actualSoapResponse.NameTable);
                namespaceManager.AddNamespace("response", DefaultNamespace);

                XmlNode root = actualSoapResponse.DocumentElement;
                XmlNode nodeToBeRemoved = root.SelectSingleNode(XPathNodesToBeRemoved, namespaceManager);
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
                                         SoapRequestAction = "http://tempuri.org/IService/IService",
                                         SoapRequestBody = "<MethodName xmlns=\"http://tempuri.org/\" />",
                                         ExpectedSoapResponseAction = "http://tempuri.org/IService/ServiceResponse",
                                         ExpectedSoapResponseBody = "<MethodNameResponse xmlns=\"http://tempuri.org/\"><MethodNameResult>result is here</MethodNameResult></MethodNameResponse>"
                                     }
                             };
        }
    }
}
