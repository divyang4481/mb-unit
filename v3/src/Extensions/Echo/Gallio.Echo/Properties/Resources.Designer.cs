﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gallio.Echo.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gallio.Echo.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Icon GallioEcho {
            get {
                object obj = ResourceManager.GetObject("GallioEcho", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gallio Echo - Version {0}.{1}.{2} build {3}.
        /// </summary>
        internal static string MainClass_ApplicationTitle {
            get {
                return ResourceManager.GetString("MainClass_ApplicationTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Available options:.
        /// </summary>
        internal static string MainClass_AvailableOptionsHeader {
            get {
                return ResourceManager.GetString("MainClass_AvailableOptionsHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Canceled!.
        /// </summary>
        internal static string MainClass_Canceled {
            get {
                return ResourceManager.GetString("MainClass_Canceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: {0}.
        /// </summary>
        internal static string MainClass_CommandLineArgumentErrorMessageFormat {
            get {
                return ResourceManager.GetString("MainClass_CommandLineArgumentErrorMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A fatal exception occurred..
        /// </summary>
        internal static string MainClass_FatalException {
            get {
                return ResourceManager.GetString("MainClass_FatalException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get the latest version at .
        /// </summary>
        internal static string MainClass_GetTheLatestVersionBanner {
            get {
                return ResourceManager.GetString("MainClass_GetTheLatestVersionBanner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.mbunit.com/.
        /// </summary>
        internal static string MainClass_MbUnitUrl {
            get {
                return ResourceManager.GetString("MainClass_MbUnitUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Opening reports..
        /// </summary>
        internal static string MainClass_OpeningReports {
            get {
                return ResourceManager.GetString("MainClass_OpeningReports", resourceCulture);
            }
        }
    }
}
