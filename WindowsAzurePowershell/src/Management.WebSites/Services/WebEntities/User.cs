// ----------------------------------------------------------------------------------
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

namespace Microsoft.WindowsAzure.Management.Websites.Services.WebEntities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Utilities;

    [DataContract(Namespace = UriElements.ServiceNamespace)]
    public class User
    {

        [DataMember(IsRequired = false)]
        [PIIValue]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string PublishingUserName { get; set; }

        [DataMember(IsRequired = false)]
        [PIIValue]
        public string PublishingPassword { get; set; }
    }

    /// <summary>
    /// Collection of users
    /// </summary>
    [CollectionDataContract(Namespace = UriElements.ServiceNamespace)]
    public class Users : List<User>
    {

        /// <summary>
        /// Empty collection
        /// </summary>
        public Users() { }

        /// <summary>
        /// Initialize collection
        /// </summary>
        /// <param name="users"></param>
        public Users(List<User> users) : base(users) { }
    }
}
