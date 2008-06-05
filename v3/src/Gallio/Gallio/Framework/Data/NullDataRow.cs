// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
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

using Gallio.Model;

namespace Gallio.Framework.Data
{
    /// <summary>
    /// A null data row simply returns a null value on each request.
    /// It has no metadata and it ignores disposal.
    /// </summary>
    public sealed class NullDataRow : BaseDataRow
    {
        /// <summary>
        /// Gets the singleton null data row instance.
        /// </summary>
        public static readonly NullDataRow Instance = new NullDataRow();

        private NullDataRow()
        {
        }

        /// <inheritdoc />
        protected override object GetValueImpl(DataBinding binding)
        {
            return null;
        }

        /// <inheritdoc />
        protected override void PopulateMetadataImpl(MetadataMap map)
        {
        }

        /// <inheritdoc />
        public override bool IsDynamic
        {
            get { return false; }
        }
    }
}
