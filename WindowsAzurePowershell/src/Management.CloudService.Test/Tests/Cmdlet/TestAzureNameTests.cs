﻿// ----------------------------------------------------------------------------------
//
// Copyright 2011 Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.CloudService.Test.Tests.Cmdlet
{
    using CloudService.Cmdlet;
    using CloudService.Model;
    using Management.Test.Stubs;
    using Services;
    using TestData;
    using Utilities;
    using VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.WindowsAzure.ServiceManagement;

    [TestClass]
    public class TestAzureNameTests : TestBase
    {
        SimpleServiceManagement channel;
        FakeWriter writer;
        TestAzureNameCommand cmdlet;
        string subscriptionId = "my subscription Id";

        [TestInitialize]
        public void SetupTest()
        {
            channel = new SimpleServiceManagement();
            writer = new FakeWriter();
            cmdlet = new TestAzureNameCommand (channel) { Writer = writer };
            Management.Extensions.CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();
        }

        [TestMethod]
        public void TestAzureServiceNameAvailable()
        {
            string name = "test";
            channel.IsDNSAvailableThunk = idnsa => { return new AvailabilityResponse { Result = true }; };

            cmdlet.IsDNSAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestAzureServiceNameIsNotAvailable()
        {
            string name = "test";
            channel.IsDNSAvailableThunk = idnsa => { return new AvailabilityResponse { Result = false }; };

            cmdlet.IsDNSAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TestAzureStorageNameAvailable()
        {
            string name = "test";
            channel.IsStorageServiceAvailableThunk = idnsa => { return new AvailabilityResponse { Result = true }; };

            cmdlet.IsStorageServiceAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestAzureStorageNameIsNotAvailable()
        {
            string name = "test";
            channel.IsStorageServiceAvailableThunk = idnsa => { return new AvailabilityResponse { Result = false }; };

            cmdlet.IsStorageServiceAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TestAzureServiceBusNamespaceAvailable()
        {
            string name = "test";
            channel.IsServiceBusNamespaceAvailableThunk = idnsa => { return new ServiceBusNamespaceAvailabiliyResponse { Result = true }; };

            cmdlet.IsServiceBusNamespaceAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestAzureServiceBusNamespaceIsNotAvailable()
        {
            string name = "test";
            channel.IsServiceBusNamespaceAvailableThunk = idnsa => { return new ServiceBusNamespaceAvailabiliyResponse { Result = false }; };

            cmdlet.IsServiceBusNamespaceAvailable(subscriptionId, name);

            bool actual = (bool)writer.OutputChannel[0];
            
            Assert.IsFalse(actual);
        }
    }
}
