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

using System;
using Gallio.Runtime.Logging;

namespace Gallio.Runtime.ProgressMonitoring
{
    /// <summary>
    /// Displays progress by writing a series of messages to an <see cref="ILogger" /> as the name
    /// of the current task changes.
    /// </summary>
    public class LogProgressMonitorProvider : BaseProgressMonitorProvider
    {
        private readonly ILogger logger;

        /// <summary>
        /// Creates a log progress monitor provider.
        /// </summary>
        /// <param name="logger">The logger to which messages should be written.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public LogProgressMonitorProvider(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(@"logger");

            this.logger = logger;
        }

        /// <inheritdoc />
        protected override IProgressMonitorPresenter GetPresenter()
        {
            return new LogProgressMonitorPresenter(logger);
        }
    }
}