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

namespace Microsoft.WindowsAzure.Management.Test.Tests.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Management.Cmdlets;
    using Management.Extensions;
    using Management.Services;
    using Management.Utilities;
    using Stubs;
    using TestData;
    using VisualStudio.TestTools.UnitTesting;
    using XmlSchema;

    [TestClass]
    public class SetSubscriptionTest
    {
        [TestInitialize]
        public void SetupTest()
        {
            CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();

            GlobalPathInfo.GlobalSettingsDirectory = Data.AzureAppDir;
        }

        [TestMethod]
        public void TestSetDefaultSubscription()
        {
            var globalComponents = GlobalComponents.CreateFromPublishSettings(GlobalPathInfo.GlobalSettingsDirectory, null, Data.ValidPublishSettings.First());

            var setSubscriptionCommand = new SetSubscriptionCommandStub();

            // Check that current subscription is the default one
            Assert.AreEqual("Windows Azure Sandbox 9-220", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);

            // Change it and make sure it got changed
            setSubscriptionCommand.SetSubscriptionProcess("DefaultSubscription", null, null, null, null, "TestSubscription1", null, null);
            Assert.AreEqual("TestSubscription1", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);
            Assert.AreEqual("TestSubscription1", setSubscriptionCommand.GetSubscriptions(null).Values.Single(sub => sub.IsDefault).SubscriptionName);

            // Clean
            globalComponents.DeleteGlobalComponents();
        }

        [TestMethod]
        public void TestResetDefaultSubscription()
        {
            var globalComponents = GlobalComponents.CreateFromPublishSettings(GlobalPathInfo.GlobalSettingsDirectory, null, Data.ValidPublishSettings.First());

            var setSubscriptionCommand = new SetSubscriptionCommandStub();

            // Check that current subscription is the default one
            Assert.AreEqual("Windows Azure Sandbox 9-220", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);

            // Change it and make sure it got changed
            setSubscriptionCommand.SetSubscriptionProcess("DefaultSubscription", null, null, null, null, "TestSubscription1", null, null);
            Assert.AreEqual("TestSubscription1", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);
            Assert.AreEqual("TestSubscription1", setSubscriptionCommand.GetSubscriptions(null).Values.Single(sub => sub.IsDefault).SubscriptionName);

            setSubscriptionCommand.SetSubscriptionProcess("ResetDefaultSubscription", null, null, null, null, null, null, null);
            Assert.AreEqual("TestSubscription1", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);
            Assert.AreEqual("Windows Azure Sandbox 9-220", setSubscriptionCommand.GetSubscriptions(null).Values.Single(sub => sub.IsDefault).SubscriptionName);

            // Clean
            globalComponents.DeleteGlobalComponents();
        }

        [TestMethod]
        public void TestUpdateSubscription()
        {
            var globalComponents = GlobalComponents.CreateFromPublishSettings(GlobalPathInfo.GlobalSettingsDirectory, null, Data.ValidPublishSettings.First());

            var setSubscriptionCommand = new SetSubscriptionCommandStub();

            // Check that current subscription is the default one
            Assert.AreEqual("Windows Azure Sandbox 9-220", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);

            // Change it and make sure it got changed
            setSubscriptionCommand.SetSubscriptionProcess("CommonSettings", "TestSubscription1", "newSubscriptionId", null, "newEndpoint", null, null, null);
            var updatedSubscription = setSubscriptionCommand.GetSubscriptions(null).Values.First(subscription => subscription.SubscriptionName == "TestSubscription1");
            Assert.AreEqual("newSubscriptionId", updatedSubscription.SubscriptionId);
            Assert.AreEqual("newEndpoint", updatedSubscription.ServiceEndpoint);

            // Clean
            globalComponents.DeleteGlobalComponents();
        }

        [TestMethod]
        public void TestCreateSubscription()
        {
            var globalComponents = GlobalComponents.CreateFromPublishSettings(GlobalPathInfo.GlobalSettingsDirectory, null, Data.ValidPublishSettings.First());

            var setSubscriptionCommand = new SetSubscriptionCommandStub();

            // Check that current subscription is the default one
            Assert.AreEqual("Windows Azure Sandbox 9-220", setSubscriptionCommand.GetCurrentSubscription().SubscriptionName);

            // Change it and make sure it got changed
            setSubscriptionCommand.SetSubscriptionProcess("CommonSettings", "newSubscription", "newSubscriptionId", null, "newEndpoint", null, "storage", null);
            var updatedSubscription = setSubscriptionCommand.GetSubscriptions(null).Values.First(subscription => subscription.SubscriptionName == "newSubscription");
            Assert.AreEqual("newSubscriptionId", updatedSubscription.SubscriptionId);
            Assert.AreEqual("newEndpoint", updatedSubscription.ServiceEndpoint);
            Assert.AreEqual("storage", updatedSubscription.CurrentStorageAccount);

            // Clean
            globalComponents.DeleteGlobalComponents();
        }

        [TestMethod]
        public void TestCreateSubscriptionNoImport()
        {
            var setSubscriptionCommand = new SetSubscriptionCommandStub();

            // Check that we cant get a current subscription as there is no working directory
            Assert.IsNull(setSubscriptionCommand.GetCurrentSubscription());

            // Create a new subscription and a new working directory implicitly.
            var publishSettings = General.DeserializeXmlFile<PublishData>(Data.ValidPublishSettings.First(), string.Empty);
            var certificate = new X509Certificate2(Convert.FromBase64String(publishSettings.Items[0].ManagementCertificate), string.Empty);

            setSubscriptionCommand.SetSubscriptionProcess("CommonSettings", "newSubscription", "newSubscriptionId", certificate, "newEndpoint", null, null, null);
            var updatedSubscription = setSubscriptionCommand.GetSubscriptions(null).Values.First(subscription => subscription.SubscriptionName == "newSubscription");
            Assert.AreEqual("newSubscriptionId", updatedSubscription.SubscriptionId);
            Assert.AreEqual("newEndpoint", updatedSubscription.ServiceEndpoint);

            // Clean
            var globalComponents = GlobalComponents.Load(GlobalPathInfo.GlobalSettingsDirectory);
            globalComponents.DeleteGlobalComponents();
        }
    }

    public class SetSubscriptionCommandStub : SetAzureSubscriptionCommand
    {
        public readonly IList<string> Messages = new List<string>();

        protected override void WriteMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
