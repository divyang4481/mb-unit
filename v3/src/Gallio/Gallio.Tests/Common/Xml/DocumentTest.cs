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

using System;
using Gallio.Model.Schema;
using Gallio.Common.Reflection;
using Gallio.Runner.Reports.Schema;
using Gallio.Tests;
using MbUnit.Framework;
using Gallio.Common.Xml;

namespace Gallio.Tests.Common.Xml
{
    [TestFixture]
    [TestsOn(typeof(Document))]
    public class DocumentTest
    {
        [Test]
        [ExpectedArgumentNullException]
        public void Constructs_with_null_root_should_throw_exception()
        {
            new Document(new Declaration(AttributeCollection.Empty), null);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void Constructs_with_null_declaration_should_throw_exception()
        {
            new Document(null, Null.Instance);
        }

        [Test]
        public void Constructs_empty()
        {
            var declaration = new Declaration(AttributeCollection.Empty);
            var document = new Document(declaration, Null.Instance);
            Assert.AreSame(declaration, document.Declaration);
            Assert.IsFalse(document.IsNull);
            Assert.IsEmpty(document.ToXml());
        }

        [Test]
        public void Constructs_empty_with_declaration()
        {
            var attribute = new Gallio.Common.Xml.Attribute("name", "value");
            var declaration = new Declaration(new AttributeCollection(new[] { attribute }));
            var document = new Document(declaration, Null.Instance);
            Assert.IsFalse(document.IsNull);
            Assert.AreEqual("<?xml name=\"value\"?>", document.ToXml());
        }

        [Test]
        public void Constructs_ok()
        {
            var attribute = new Gallio.Common.Xml.Attribute("name", "value");
            var declaration = new Declaration(new AttributeCollection(new[] { attribute }));
            var element = new Element(Null.Instance, "Planet", String.Empty, AttributeCollection.Empty);
            var document = new Document(declaration, element);
            Assert.IsFalse(document.IsNull);
            Assert.AreEqual("<?xml name=\"value\"?><Planet/>", document.ToXml());
        }

        private static Document GetDocument()
        {
            var parser = new Parser(
                "<?xml version='1.0' encoding='UTF-8'?>" +
                "<Composers>" +
                "  <Composer>" +
                "     <Name>Gustav Mahler</Name>" +
                "     <BirthDate>07/07/1860</BirthDate>" +
                "     <Song>abc</Song>" +
                "     <Song>def</Song>" +
                "     <Song>ghi</Song>" +
                "  </Composer>" +
                "</Composers>");

            return parser.Run(Options.IgnoreComments);
        }

        [Test]
        public void CountAt_with_null_path_should_throw_exception()
        {
            var document = GetDocument();
            Assert.Throws<ArgumentNullException>(() => document.CountAt(null, null, Options.None));
        }

        [Test]
        public void CountAt_should_find_unique_element()
        {
            var document = GetDocument();
            int count = document.CountAt((XmlPathClosed)XmlPath.Element("Composers").Element("Composer").Element("Name"), null, Options.None);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void CountAt_should_find_multiple_elements()
        {
            var document = GetDocument();
            int count = document.CountAt((XmlPathClosed)XmlPath.Element("Composers").Element("Composer").Element("Song"), null, Options.None);
            Assert.AreEqual(3, count);
        }

        [Test]
        public void CountAt_case_insensitive_should_find_unique_element()
        {
            var document = GetDocument();
            int count = document.CountAt((XmlPathClosed)XmlPath.Element("CoMPOSErs").Element("coMpoSER").Element("namE"), null, Options.IgnoreElementsNameCase);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void CountAt_should_find_zero_element_when_it_does_not_exist()
        {
            var document = GetDocument();
            int count = document.CountAt((XmlPathClosed)XmlPath.Element("Composers").Element("Composer").Element("DoesNotExist"), null, Options.None);
            Assert.AreEqual(0, count);
        }

        [Test]
        public void CountAt_case_sensitive_should_find_zero_element_when_it_exists_with_another_case()
        {
            var document = GetDocument();
            int count = document.CountAt((XmlPathClosed)XmlPath.Element("CoMPOSErs").Element("coMpoSER").Element("namE"), null, Options.None);
            Assert.AreEqual(0, count);
        }
    }
}