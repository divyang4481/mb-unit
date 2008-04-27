﻿// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
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
using System.Reflection;
using Gallio.Model;
using Gallio.Reflection;
using Gallio.MSTestAdapter.Model;
using Gallio.MSTestAdapter.TestResources;
using Gallio.MSTestAdapter.TestResources.Metadata;
using Gallio.Tests.Model;
using MbUnit.Framework;

namespace Gallio.MSTestAdapter.Tests.Model
{
    [TestFixture]
    [TestsOn(typeof(MSTestFramework))]
    [Author("Julian", "julian.hidalgo@gallio.org")]
    public class MSTestFrameworkTest : BaseTestFrameworkTest
    {
        protected override Assembly GetSampleAssembly()
        {
            return typeof(SimpleTest).Assembly;
        }

        protected override ITestFramework CreateFramework()
        {
            return new MSTestFramework();
        }

        [Test]
        public void NameIsMSTest()
        {
            Assert.AreEqual("MSTest", framework.Name);
        }

        [Test]
        public void PopulateTestTree_CapturesTestStructureAndBasicMetadata()
        {
            PopulateTestTree();

            RootTest rootTest = testModel.RootTest;
            Assert.IsNull(rootTest.Parent);
            Assert.AreEqual(TestKinds.Root, rootTest.Kind);
            Assert.IsNull(rootTest.CodeElement);
            Assert.IsFalse(rootTest.IsTestCase);
            Assert.AreEqual(1, rootTest.Children.Count);

            BaseTest frameworkTest = (BaseTest)rootTest.Children[0];
            Assert.AreSame(testModel.RootTest, frameworkTest.Parent);
            Assert.AreEqual(TestKinds.Framework, frameworkTest.Kind);
            Assert.IsNull(frameworkTest.CodeElement);
            Assert.AreEqual("MSTest v9.0.0.0", frameworkTest.Name);
            Assert.IsFalse(frameworkTest.IsTestCase);
            Assert.AreEqual(1, frameworkTest.Children.Count);

            MSTestAssembly assemblyTest = (MSTestAssembly)frameworkTest.Children[0];
            Assert.AreSame(frameworkTest, assemblyTest.Parent);
            Assert.AreEqual(TestKinds.Assembly, assemblyTest.Kind);
            Assert.AreEqual(CodeReference.CreateFromAssembly(sampleAssembly), assemblyTest.CodeElement.CodeReference);
            Assert.AreEqual(sampleAssembly.GetName().Name, assemblyTest.Name);
            Assert.IsFalse(assemblyTest.IsTestCase);
            Assert.GreaterEqualThan(assemblyTest.Children.Count, 2);

            MSTest fixtureTest = (MSTest)GetDescendantByName(assemblyTest, "SimpleTest");
            Assert.AreSame(assemblyTest, fixtureTest.Parent);
            Assert.AreEqual(TestKinds.Fixture, fixtureTest.Kind);
            Assert.AreEqual(new CodeReference(sampleAssembly.FullName, "Gallio.MSTestAdapter.TestResources", "Gallio.MSTestAdapter.TestResources.SimpleTest", null, null),
                fixtureTest.CodeElement.CodeReference);
            Assert.AreEqual("SimpleTest", fixtureTest.Name);
            Assert.IsFalse(fixtureTest.IsTestCase);
            Assert.AreEqual(2, fixtureTest.Children.Count);

            MSTest passTest = (MSTest)GetDescendantByName(fixtureTest, "Pass");
            MSTest failTest = (MSTest)GetDescendantByName(fixtureTest, "Fail");

            Assert.IsNotNull(passTest, "Cannot find test case 'Pass'");
            Assert.IsNotNull(failTest, "Cannot find test case 'Fail'");

            Assert.AreSame(fixtureTest, passTest.Parent);
            Assert.AreEqual(TestKinds.Test, passTest.Kind);
            Assert.AreEqual(new CodeReference(sampleAssembly.FullName, "Gallio.MSTestAdapter.TestResources", "Gallio.MSTestAdapter.TestResources.SimpleTest", "Pass", null),
                passTest.CodeElement.CodeReference);
            Assert.AreEqual("Pass", passTest.Name);
            Assert.IsTrue(passTest.IsTestCase);
            Assert.AreEqual(0, passTest.Children.Count);

            Assert.AreSame(fixtureTest, failTest.Parent);
            Assert.AreEqual(TestKinds.Test, failTest.Kind);
            Assert.AreEqual(new CodeReference(sampleAssembly.FullName, "Gallio.MSTestAdapter.TestResources", "Gallio.MSTestAdapter.TestResources.SimpleTest", "Fail", null),
                failTest.CodeElement.CodeReference);
            Assert.AreEqual("Fail", failTest.Name);
            Assert.IsTrue(failTest.IsTestCase);
            Assert.AreEqual(0, failTest.Children.Count);
        }

        [Test]
        public void MetadataImport_XmlDocumentation()
        {
            PopulateTestTree();

            MSTest test = (MSTest)GetDescendantByName(testModel.RootTest, typeof(SimpleTest).Name);
            MSTest passTest = (MSTest)GetDescendantByName(test, "Pass");
            MSTest failTest = (MSTest)GetDescendantByName(test, "Fail");

            Assert.AreEqual("<summary>\nA simple test fixture.\n</summary>", test.Metadata.GetValue(MetadataKeys.XmlDocumentation));
            Assert.AreEqual("<summary>\nA passing test.\n</summary>", passTest.Metadata.GetValue(MetadataKeys.XmlDocumentation));
            Assert.AreEqual("<summary>\nA failing test.\n</summary>", failTest.Metadata.GetValue(MetadataKeys.XmlDocumentation));
        }

        [Test]
        public void MetadataImport_IgnoreReason()
        {
            PopulateTestTree();

            MSTest fixture = (MSTest)GetDescendantByName(testModel.RootTest, typeof(MetadataSample).Name);
            MSTest test = (MSTest)fixture.Children[0];
            Assert.AreEqual("Ignored", test.Metadata.GetValue(MetadataKeys.IgnoreReason));
        }

        [Test]
        [Ignore("The test metadata is not being added yet")]
        public void MetadataImport_Property()
        {
            PopulateTestTree();

            MSTest fixture = (MSTest)GetDescendantByName(testModel.RootTest, typeof(MetadataSample).Name);
            MSTest test = (MSTest)fixture.Children[0];
            Assert.AreEqual("Julian", test.Metadata.GetValue("Owner"));
            Assert.AreEqual("1", test.Metadata.GetValue("Priority"));
        }

        [Test]
        public void MetadataImport_AssemblyAttributes()
        {
            PopulateTestTree();

            ITest frameworkTest = testModel.RootTest.Children[0];
            ITest assemblyTest = frameworkTest.Children[0];

            Assert.AreEqual("MbUnit Project", assemblyTest.Metadata.GetValue(MetadataKeys.Company));
            Assert.AreEqual("Test", assemblyTest.Metadata.GetValue(MetadataKeys.Configuration));
            StringAssert.Contains(assemblyTest.Metadata.GetValue(MetadataKeys.Copyright), "Gallio Project");
            Assert.AreEqual("A sample test assembly for MSTest.", assemblyTest.Metadata.GetValue(MetadataKeys.Description));
            Assert.AreEqual("Gallio", assemblyTest.Metadata.GetValue(MetadataKeys.Product));
            Assert.AreEqual("Gallio.MSTestAdapter.TestResources", assemblyTest.Metadata.GetValue(MetadataKeys.Title));
            Assert.AreEqual("Gallio", assemblyTest.Metadata.GetValue(MetadataKeys.Trademark));

            Assert.AreEqual("1.2.3.4", assemblyTest.Metadata.GetValue(MetadataKeys.InformationalVersion));
            StringAssert.IsNonEmpty(assemblyTest.Metadata.GetValue(MetadataKeys.FileVersion));
            StringAssert.IsNonEmpty(assemblyTest.Metadata.GetValue(MetadataKeys.Version));
        }
    }
}
