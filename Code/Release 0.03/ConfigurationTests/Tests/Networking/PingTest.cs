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
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ComponentModel;
using Common.Boolean;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Networking)]
    public class PingTest : Test
    {
        [Description("Host name to check, eg www.google.com")]
        [Category("Ping Properties")]
        public string HostName { get; set; }

        private bool _shouldExist = true;

        [DefaultValue(true)]
        [Description("Does host exist?")]
        [Category("Ping Properties")]
        public bool ShouldExist
        {
            get { return _shouldExist; }
            set { _shouldExist = value; }
        }

        public override void Run()
        {
            var result = WasSuccessful(PingHost(HostName));
            AssertState.Equal(ShouldExist, result, string.Format("Host is {0}contactable.", ShouldExist.IfTrue("not ")));
        }

        private static string PingHost(string host)
        {            
            var returnMessage = string.Empty;

            try
            {                
                var address = GetIpFromHost(host);             
                var pingOptions = new PingOptions(128, true);                
                var ping = new Ping();                
                var buffer = new byte[32];
                
                for (var i = 0; i < 4; i++)
                {
                    try
                    {
                        var pingReply = ping.Send(address, 1000, buffer, pingOptions);
                        
                        if (pingReply != null)
                        {
                            switch (pingReply.Status)
                            {
                                case IPStatus.Success:
                                    returnMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}",
                                        pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime,
                                        pingReply.Options.Ttl);
                                    break;
                                case IPStatus.TimedOut:
                                    returnMessage = "Connection has timed out...";
                                    break;
                                default:
                                    returnMessage = string.Format("Ping failed: {0}", pingReply.Status);
                                    break;
                            }
                        }
                        else
                            returnMessage = "Connection failed for an unknown reason...";
                    }
                    catch (PingException ex)
                    {
                        returnMessage = string.Format("Connection Error: {0}", ex.Message);
                    }
                    catch (SocketException ex)
                    {
                        returnMessage = string.Format("Connection Error: {0}", ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Connection failed for an unknown reason...");
            }

            return returnMessage;
        }
        private static IPAddress GetIpFromHost(string host)
        {
            var address = Dns.GetHostEntry(host).AddressList[0];
            return address;
        }

        private static bool WasSuccessful(string reply)
        {
            return reply.Contains("Reply from");
        }

        public override List<Test> CreateExamples()
        {
           return new List<Test>
            {
                new IISInstalledTest(){ShouldExist = true, TestName = "Check that the IIS Webserver is intalled."}
            };
        }
    }
}