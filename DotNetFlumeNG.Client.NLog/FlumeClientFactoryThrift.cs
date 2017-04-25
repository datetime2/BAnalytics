﻿// 
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
using System.Globalization;
using System.Linq;
using DotNetFlumeNG.Client.Core;
using DotNetFlumeNG.Client.LegacyThrift;

namespace DotNetFlumeNG.Client
{
    internal static partial class FlumeClientFactory
    {
        public static IFlumeClient CreateClient(ClientType clientType, string host, int port)
        {
            var server = _server.FirstOrDefault(t => t.ClientType == clientType && t.Host == host && t.Port == port);
            if (server == null)
            {
                server = new ServerInfo()
                {
                    ClientType = clientType,
                    Host = host,
                    Port = port
                };
                _server.Add(server);
            }
            if (server.FlumeClient == null || server.FlumeClient.IsClosed)
            {
                switch (server.ClientType)
                {
                    case ClientType.LegacyThrift:
                        server.FlumeClient = new LegacyThriftClient(server.Host, server.Port);
                        break;

                    case ClientType.Thrift:
                        server.FlumeClient = new Thrift.ThriftClient(server.Host, server.Port);
                        break;

                    default:
                        throw new NotSupportedException(
                            string.Format(CultureInfo.InvariantCulture,
                                "The client type [{0}] is not supported. The only supported type is Thrift.",
                                _clientType));
                }
            }
            return server.FlumeClient;
        }
    }
}