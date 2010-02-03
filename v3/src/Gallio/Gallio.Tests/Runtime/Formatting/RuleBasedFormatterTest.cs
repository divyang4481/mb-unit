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
using Gallio.Common.Collections;
using Gallio.Runtime.Formatting;
using MbUnit.Framework;
using Rhino.Mocks;

namespace Gallio.Tests.Runtime.Formatting
{
    [TestFixture]
    [TestsOn(typeof(RuleBasedFormatter))]
    public class RuleBasedFormatterTest : BaseTestWithMocks
    {
        [Test, ExpectedArgumentNullException]
        public void ConstructorThrowsIfRuntimeIsNull()
        {
            new RuleBasedFormatter(null);
        }

        [Test]
        public void NullValueProducesStringContainingNull()
        {
            var formatter = new RuleBasedFormatter(EmptyArray<IFormattingRule>.Instance);
            Assert.AreEqual("null", formatter.Format(null));
        }

        [Test]
        [Column(true, false)]
        public void FormatterSubstitutesNameOfTypeIfRuleReturnsNullOrEmpty(bool useNull)
        {
            IFormattingRule rule;
            using (Mocks.Record())
            {
                rule = Mocks.StrictMock<IFormattingRule>();
                Expect.Call(rule.GetPriority(typeof(int))).Return(0);
                Expect.Call(rule.Format(null, null)).IgnoreArguments().Return(useNull ? null : "");
            }

            var formatter = new RuleBasedFormatter(new IFormattingRule[] { rule });
            Assert.AreEqual("{System.Int32}", formatter.Format(42));
        }

        [Test]
        public void FormatterSubstitutesNameOfTypeIfRuleThrows()
        {
            IFormattingRule rule;
            using (Mocks.Record())
            {
                rule = Mocks.StrictMock<IFormattingRule>();
                Expect.Call(rule.GetPriority(typeof(int))).Return(0);
                Expect.Call(rule.Format(null, null)).IgnoreArguments().Throw(new ApplicationException("Boom!"));
            }

            var formatter = new RuleBasedFormatter(new IFormattingRule[] { rule });
            Assert.AreEqual("{System.Int32}", formatter.Format(42));
        }

        [Test]
        public void FormatterChoosesTheBestRuleAndCachesLookups()
        {
            IFormattingRule rule1;
            IFormattingRule rule2;
            using (Mocks.Record())
            {
                rule1 = Mocks.StrictMock<IFormattingRule>();
                Expect.Call(rule1.GetPriority(typeof(int))).Return(0);

                rule2 = Mocks.StrictMock<IFormattingRule>();
                Expect.Call(rule2.GetPriority(typeof(int))).Return(1);

                Expect.Call(rule2.Format(null, null)).IgnoreArguments().Return("42");
                Expect.Call(rule2.Format(null, null)).IgnoreArguments().Return("53");
            }

            var formatter = new RuleBasedFormatter(new IFormattingRule[] { rule1, rule2 });
            Assert.AreEqual("42", formatter.Format(42));
            Assert.AreEqual("53", formatter.Format(53));
        }

        internal class Foo
        {
            private readonly int value;

            public int Value
            {
                get
                {
                    return value;
                }
            }

            public Foo(int value)
            {
                this.value = value;
            }
        }

        [Test]
        public void Custom_formatting()
        {
            var formatter = new RuleBasedFormatter(EmptyArray<IFormattingRule>.Instance);
            CustomFormatters.Register<Foo>(x => String.Format("Foo's value is {0}.", x.Value));

            try
            {
                string output = formatter.Format(new Foo(123));
                Assert.AreEqual("Foo's value is 123.", output);
            }
            finally
            {
                CustomFormatters.Unregister<Foo>();
            }
        }
    }
}