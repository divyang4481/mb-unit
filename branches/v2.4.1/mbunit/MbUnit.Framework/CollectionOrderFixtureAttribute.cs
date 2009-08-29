//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;
using System.Reflection;

namespace MbUnit.Framework 
{
	using MbUnit.Core;
	using MbUnit.Core.Invokers;
	using MbUnit.Framework.Testers;
	using System.Collections;
	using MbUnit.Core.Runs;
	
	/// <summary>
	/// Different collection order
	/// </summary>
	public enum CollectionOrderTest
	{
		/// <summary>Tests ascending order collection</summary>
		OrderedAscending,
		/// <summary>Tests ascending order collection</summary>
		OrderedDescending
	}
	
	/// <summary>
	/// Collection Order Pattern implementations.
	/// </summary>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/remarkss/remarks[@name='CollectionSortFixtureAttribute']"/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class CollectionOrderFixtureAttribute : TestFixturePatternAttribute
    {
		private CollectionOrderTest order;
        private IComparer comparer;

        public CollectionOrderFixtureAttribute(CollectionOrderTest order)
        {
            this.comparer = Comparer.Default;
        }
        public CollectionOrderFixtureAttribute(
			CollectionOrderTest order,
			Type comparerType
			)
		:base()
		{
			if (comparerType==null)
				throw new ArgumentNullException("comparerType");

			this.order = order;
            this.comparer = (IComparer)Activator.CreateInstance(comparerType);
        }
		
		public override IRun GetRun()
		{
			SequenceRun runs = new SequenceRun();

			// set up
			OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute),false);			
			runs.Runs.Add( setup );

			// collection providers			
			MethodRun provider = new MethodRun(
			                                   typeof(ProviderAttribute),
			                                   typeof(ArgumentFeederRunInvoker),
			                                   false,
			                                   true
			                                   );
			runs.Runs.Add(provider);
			
			
			// fill
			MethodRun fill = new MethodRun(typeof(FillAttribute),
			                               false,
			                               true
			                               );
			runs.Runs.Add(fill);

			// add tester for the order
			CustomRun orderTest = new CustomRun(
				typeof(CollectionOrderTester),
				typeof(TestAttribute),
				true, // it test
				this.order, // constructor arguments
				comparer
				);
			runs.Runs.Add( orderTest );

			// tear down
			OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute),false);
			runs.Runs.Add(tearDown);

						
			return runs;						
		}
	}
}