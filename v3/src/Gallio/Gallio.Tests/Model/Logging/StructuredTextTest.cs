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

using System;
using Gallio.Collections;
using Gallio.Model.Logging;
using Gallio.Model.Logging.Tags;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Gallio.Tests.Model.Logging
{
    [TestsOn(typeof(StructuredText))]
    [VerifyEqualityContract(typeof(StructuredText))]
    public class StructuredTextTest : IEquivalenceClassProvider<StructuredText>
    {
        private static readonly StructuredText Example = new StructuredText(new BodyTag()
        {
            Contents =
                {
                    new TextTag("a"), 
                    new SectionTag("blah") { Contents = { new TextTag("bc") }},
                    new MarkerTag(Marker.AssertionFailure) { Contents = { new TextTag("def")}},
                    new EmbedTag("attachment"),
                    new TextTag("ghij")
                }
        }, new Attachment[] { new TextAttachment("attachment", MimeTypes.PlainText, "text") });

        public EquivalenceClassCollection<StructuredText> GetEquivalenceClasses()
        {
            return EquivalenceClassCollection<StructuredText>.FromDistinctInstances(
                new StructuredText("lalalala"),
                new StructuredText(new BodyTag() { Contents = { new TextTag("blah") }}),
                new StructuredText(new BodyTag() { Contents = { new TextTag("blah") }},
                    new[] { new TextAttachment("abc", MimeTypes.PlainText, "blah") })
                );
        }

        [Test, ExpectedArgumentNullException]
        public void ConstructorWithTextThrowsIfStringIsNull()
        {
            new StructuredText((string)null);
        }

        [Test, ExpectedArgumentNullException]
        public void ConstructorWithBodyTagThrowsIfBodyTagIsNull()
        {
            new StructuredText((BodyTag)null);
        }

        [Test, ExpectedArgumentNullException]
        public void ConstructorWithBodyTagAndAttachmentThrowsIfBodyTagIsNull()
        {
            new StructuredText(null, EmptyArray<Attachment>.Instance);
        }

        [Test, ExpectedArgumentNullException]
        public void ConstructorWithBodyTagAndAttachmentThrowsIfAttachmentListIsNull()
        {
            new StructuredText(new BodyTag(), null);
        }

        [Test]
        public void ConstructorWithTextInitializesProperties()
        {
            StructuredText text = new StructuredText("blah");
            Assert.AreEqual(0, text.Attachments.Count);

            NewAssert.AreEqual(new BodyTag() { Contents = { new TextTag("blah") } }, text.BodyTag);
        }

        [Test]
        public void ConstructorWithBodyTagInitializesProperties()
        {
            StructuredText text = new StructuredText(Example.BodyTag);
            Assert.AreEqual(0, text.Attachments.Count);

            NewAssert.AreEqual(Example.BodyTag, text.BodyTag);
        }

        [Test]
        public void ConstructorWithBodyTagAndAttachmentsInitializesProperties()
        {
            StructuredText text = new StructuredText(Example.BodyTag, Example.Attachments);

            NewAssert.AreEqual(Example.BodyTag, text.BodyTag);
            NewAssert.AreEqual(Example.Attachments, text.Attachments);
        }

        [Test]
        public void GetTextLength()
        {
            Assert.AreEqual(10, Example.GetTextLength());
        }
        
        [Test, ExpectedArgumentNullException]
        public void WriteToThrowsIfWriterIsNull()
        {
            Example.WriteTo(null);
        }

        [Test, ExpectedArgumentNullException]
        public void TruncatedWriteToThrowsIfWriterIsNull()
        {
            Example.TruncatedWriteTo(null, 0);
        }

        [Test, ExpectedArgumentOutOfRangeException]
        public void TruncatedWriteToThrowsIfMaxLengthIsNegative()
        {
            Example.TruncatedWriteTo(new StructuredTextWriter(), -1);
        }

        [Test]
        public void WriteToRecreatesTheStructuredText()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Example.WriteTo(writer);
            NewAssert.AreEqual(Example, writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToRecreatesTheStructuredTextWhenMaxLengthEqualsTextLength()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsFalse(Example.TruncatedWriteTo(writer, Example.GetTextLength()));
            NewAssert.AreEqual(Example, writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToRecreatesTheStructuredTextWhenMaxLengthExceedsTextLength()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsFalse(Example.TruncatedWriteTo(writer, Example.GetTextLength() + 1));
            NewAssert.AreEqual(Example, writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToTruncatesWhenLengthIsInsufficient0()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsTrue(Example.TruncatedWriteTo(writer, 0));
            NewAssert.AreEqual(new StructuredText(new BodyTag(),
                new Attachment[] { new TextAttachment("attachment", MimeTypes.PlainText, "text") }),
                writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToTruncatesWhenLengthIsInsufficient1()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsTrue(Example.TruncatedWriteTo(writer, 1));
            NewAssert.AreEqual(new StructuredText(new BodyTag()
            {
                Contents =
                    {
                        new TextTag("a"), 
                        new SectionTag("blah")
                    }
            }, new Attachment[] { new TextAttachment("attachment", MimeTypes.PlainText, "text") }),
            writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToTruncatesWhenLengthIsInsufficient5()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsTrue(Example.TruncatedWriteTo(writer, 5));
            NewAssert.AreEqual(new StructuredText(new BodyTag()
            {
                Contents =
                    {
                        new TextTag("a"),
                        new SectionTag("blah") { Contents = { new TextTag("bc") }},
                        new MarkerTag(Marker.AssertionFailure) { Contents = { new TextTag("de")}}
                    }
            }, new Attachment[] { new TextAttachment("attachment", MimeTypes.PlainText, "text") }),
            writer.ToStructuredText());
        }

        [Test]
        public void TruncatedWriteToTruncatesWhenLengthIsInsufficient8()
        {
            StructuredTextWriter writer = new StructuredTextWriter();
            Assert.IsTrue(Example.TruncatedWriteTo(writer, 8));
            NewAssert.AreEqual(new StructuredText(new BodyTag()
            {
                Contents =
                    {
                        new TextTag("a"),
                        new SectionTag("blah") { Contents = { new TextTag("bc") }},
                        new MarkerTag(Marker.AssertionFailure) { Contents = { new TextTag("def")}},
                        new EmbedTag("attachment"),
                        new TextTag("gh")
                    }
            }, new Attachment[] { new TextAttachment("attachment", MimeTypes.PlainText, "text") }),
            writer.ToStructuredText());
        }

        [Test]
        public void ToStringEqualsBodyTagToString()
        {
            Assert.AreEqual(Example.BodyTag.ToString(), Example.ToString());
        }
    }
}
