using System;
using System.Collections.Generic;
using Gallio.Collections;
using Gallio.Framework.Pattern;
using Gallio.Reflection;
using NBehave.Core;

namespace NBehave.Spec.Framework
{
    /// <summary>
    /// When applied to a method of a context class, declares a setup action to be performed before
    /// evaluating all of its specifications.
    /// </summary>
    [AttributeUsage(PatternAttributeTargets.ContributionMethod, AllowMultiple = false, Inherited = true)]
    public class ContextTearDownAttribute : ContributionMethodPatternAttribute
    {
        /// <inheritdoc />
        protected override void DecorateContainingScope(PatternEvaluationScope containingScope, IMethodInfo method)
        {
            containingScope.Test.TestInstanceActions.TearDownTestInstanceChain.After(
                delegate(PatternTestInstanceState testInstanceState)
                {
                    testInstanceState.InvokeFixtureMethod(method, EmptyArray<KeyValuePair<ISlotInfo, object>>.Instance);
                });
        }

        /// <inheritdoc />
        protected override void Validate(PatternEvaluationScope containingScope, IMethodInfo method)
        {
            base.Validate(containingScope, method);

            if (!containingScope.IsTestDeclaration
                || containingScope.Test.Kind != NBehaveTestKinds.Context)
                throw new PatternUsageErrorException("The [ContextTearDown] attribute can only appear on a method within a context class.");
        }
    }
}