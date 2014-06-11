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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Internet)]
    public class HttpConnectionTest : Test
    {
        [MandatoryField]
        [Description("The URL to test.")]
        [Category("HTTP Connection Properties")]
        public string UrlToTest { get; set; }

        [MandatoryField]
        [Description("The expected HTTP status code response.")]
        [Category("HTTP Connection Properties")]
        public HttpStatusCode ExpectedResponse { get; set; }

        public override void Run()
        {
            var httpRequest = WebRequest.Create(UrlToTest);
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            AssertState.Equal(ExpectedResponse.ToString(), httpResponse.StatusCode.ToString(), false, String.Format("The Http response was {0}. The Expected response is {1}", httpResponse.StatusCode, ExpectedResponse));
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>{
                new HttpConnectionTest {UrlToTest="http://www.google.com", ExpectedResponse= HttpStatusCode.OK}
            };
        }
    }
}
