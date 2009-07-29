﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Library.Properties.Resources", typeof(Resources).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to Exception: {0}.
        /// </summary>
        internal static string ExceptionException {
            get {
                return ResourceManager.GetString("ExceptionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Press Back to Exit.
        /// </summary>
        internal static string ExceptionExitPrompt {
            get {
                return ResourceManager.GetString("ExceptionExitPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to **Crash Log **.
        /// </summary>
        internal static string ExceptionHeader {
            get {
                return ResourceManager.GetString("ExceptionHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stack Trace: {0}.
        /// </summary>
        internal static string ExceptionTrace {
            get {
                return ResourceManager.GetString("ExceptionTrace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No storage device was selected. You can continue without a device, but you will not be able to save. Would you like to select a storage device?.
        /// </summary>
        internal static string StoragePromptReselectCancelled {
            get {
                return ResourceManager.GetString("StoragePromptReselectCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The storage device was disconnected. You can continue without a device, but you will not be able to save. Would you like to select a storage device?\n\nNote: if you only have one remaining storage device on the system (e.g. only a hard drive) the device will be selected automatically and no prompt will appear..
        /// </summary>
        internal static string StoragePromptReselectDisconnected {
            get {
                return ResourceManager.GetString("StoragePromptReselectDisconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No. Continue without device.
        /// </summary>
        internal static string StoragePromptReselectNo {
            get {
                return ResourceManager.GetString("StoragePromptReselectNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reselect Storage Device?.
        /// </summary>
        internal static string StoragePromptReselectTitle {
            get {
                return ResourceManager.GetString("StoragePromptReselectTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes. Select new device..
        /// </summary>
        internal static string StoragePromptReselectYes {
            get {
                return ResourceManager.GetString("StoragePromptReselectYes", resourceCulture);
            }
        }
    }
}
