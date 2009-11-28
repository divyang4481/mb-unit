// Copyright 2005-2009 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Threading;

namespace Gallio.UI.Common.Synchronization
{
    ///<summary>
    /// Wrapper for System.Threading.SynchronizationContext to improve testability.
    ///</summary>
    public interface ISynchronizationContext
    {
        /// <summary>
        /// Dispatches an asynchronous message.
        /// </summary>
        /// <param name="sendOrPostCallback">The SendOrPostCallback delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        void Post(SendOrPostCallback sendOrPostCallback, object state);

        /// <summary>
        /// Dispatches a synchronous message. 
        /// </summary>
        /// <param name="sendOrPostCallback">The SendOrPostCallback delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        void Send(SendOrPostCallback sendOrPostCallback, object state);
    }
}