// Copyright 2005-2010 Gallio Project - http://www.gallio.org/
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

using System.Drawing;
using Aga.Controls.Tree;
using Gallio.Icarus.Models;
using Gallio.Icarus.Properties;
using Gallio.Model;

namespace Gallio.Icarus.Controls
{
    public class TestStatusNodeIcon : NodeIcon<TestTreeNode>
    {
        public TestStatusNodeIcon()
            : base(ttn => ttn.TestStatus)
        { }

        protected override Image GetIcon(TreeNodeAdv node)
        {
            var value = GetValue(node);

            if (value == null)
                return null;
             
            return GetTestStatusIcon((TestStatus)value);
        }

        private static Image GetTestStatusIcon(TestStatus status)
        {
            switch (status)
            {
                case TestStatus.Failed:
                    return Resources.cross;
                case TestStatus.Passed:
                    return Resources.tick;
                case TestStatus.Inconclusive:
                    return Resources.error;
            }
            return null;
        }
    }
}
