// 
//    Copyright 2013 Mark Lamley
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client
{
    public class ServerInfo
    {
        public ClientType ClientType { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public IFlumeClient FlumeClient { get; set; }
    }

    internal static partial class FlumeClientFactory
    {
        private static ClientType _clientType;

        private static IFlumeClient _client;

        private static IList<ServerInfo> _server = new List<ServerInfo>();

        public static void Init(ClientType clientType, string host, int port)
        {
            if (host == null) throw new ArgumentNullException("host");

            _clientType = clientType;
            if (!_server.Any(t => t.ClientType == clientType && t.Host == host && t.Port == port))
            {
                _server.Add(new ServerInfo()
                {
                    ClientType = clientType,
                    Host = host,
                    Port = port
                });
            }
        }

        public static void Close()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}