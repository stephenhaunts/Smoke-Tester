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

using System.ComponentModel;

namespace ConfigurationTests.Tests
{
    public abstract class SoapTest : ConfigurationTest
    {
        [Category("Web Service Properties")]
        public string ServiceAddress { get; set; }

        [Category("Web Service Properties")]
        public string ExpectedSoapResponseBody { get; set; }

        [Category("Web Service Properties")]
        public string ExpectedSoapResponseAction { get; set; }

        [Category("Web Service Properties")]
        public string SoapRequestBody { get; set; }

        [Category("Web Service Properties")]
        public string SoapRequestAction { get; set; }

        [Category("Web Service Properties")]
        public string DefaultNamespace { get; set; }

        [Category("Web Service Properties")]
        public string XPathNodesToBeRemoved { get; set; }

        [Category("Web Service Properties")]
        public int? ConnectionTimeout{ get; set; }

        [Category("Web Service Properties")]
        public string ContentType { get; set; }

        [Category("Web Service Properties")]
        public string Method { get; set; }        
    }
}
