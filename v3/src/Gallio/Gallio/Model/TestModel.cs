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
using System.Collections.Generic;
using Gallio.Common.Collections;

namespace Gallio.Model
{
    /// <summary>
    /// The test model provides access to the contents of the test tree
    /// generated from a test package by the test enumeration process.
    /// </summary>
    public sealed class TestModel
    {
        private readonly TestExplorationContext testExplorationContext;
        private readonly RootTest rootTest;
        private readonly List<Annotation> annotations;

        /// <summary>
        /// Creates a test model with a new empty root test.
        /// </summary>
        /// <param name="testExplorationContext">The test exploration context.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testExplorationContext"/> is null.</exception>
        public TestModel(TestExplorationContext testExplorationContext)
            : this(testExplorationContext, new RootTest())
        {
        }

        /// <summary>
        /// Creates a test model.
        /// </summary>
        /// <param name="testExplorationContext">The test package from which the model was created.</param>
        /// <param name="rootTest">The root test.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testExplorationContext" /> or <paramref name="rootTest"/> is null.</exception>
        public TestModel(TestExplorationContext testExplorationContext, RootTest rootTest)
        {
            if (testExplorationContext == null)
                throw new ArgumentNullException("testPackage");
            if (rootTest == null)
                throw new ArgumentNullException(@"rootTest");

            this.testExplorationContext = testExplorationContext;
            this.rootTest = rootTest;
            annotations = new List<Annotation>();
        }

        /// <summary>
        /// Gets the test exploration context.
        /// </summary>
        public TestExplorationContext TestExplorationContext
        {
            get { return testExplorationContext; }
        }

        /// <summary>
        /// Gets the root test in the model.
        /// </summary>
        public RootTest RootTest
        {
            get { return rootTest; }
        }

        /// <summary>
        /// Recursively enumerates all tests including the root test.
        /// </summary>
        public IEnumerable<ITest> AllTests
        {
            get
            {
                if (rootTest == null)
                    return EmptyArray<ITest>.Instance;

                return TreeUtils.GetPreOrderTraversal<ITest>(rootTest, GetChildren);
            }
        }

        /// <summary>
        /// Gets the read-only list of annotations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An annotation is an informational, warning or error message associated with
        /// a code element in the test model.
        /// </para>
        /// <para>
        /// Test frameworks publish annotations on the test model that describe usage errors
        /// or warnings about problems that may prevent tests from running, such as using a
        /// custom attribute incorrectly.  They may also emit informational annotations to
        /// draw the user's attention, such as by flagging ignored or pending tests.
        /// </para>
        /// <para>
        /// The presentation of annotations is undefined.  A command-line test runner might
        /// simply log them whereas an IDE plugin could generate new task items to incorporate
        /// them into the UI.
        /// </para>
        /// </remarks>
        public IList<Annotation> Annotations
        {
            get { return annotations.AsReadOnly(); }
        }

        /// <summary>
        /// Adds an annotation.
        /// </summary>
        /// <param name="annotation">The annotation to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="annotation"/> is null.</exception>
        /// <seealso cref="Annotations"/>
        public void AddAnnotation(Annotation annotation)
        {
            if (annotation == null)
                throw new ArgumentNullException("annotation");

            annotations.Add(annotation);
        }

        private static IEnumerable<ITest> GetChildren(ITest node)
        {
            return node.Children;
        }
    }
}