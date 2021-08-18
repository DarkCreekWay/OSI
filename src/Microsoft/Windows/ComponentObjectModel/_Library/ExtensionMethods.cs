using System;
using System.Diagnostics;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Provides useful extension methods.
    /// </summary>
    public static class ExtensionMethods {

        /// <summary>
        /// Gets a custom attribute from a given type.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute.</typeparam>
        /// <param name="type">The type to be searched.</param>
        /// <param name="inherit">true to search the inheritance chain to find the custom attribute. Otherwise, set to false.</param>
        /// <returns>If found, the custom attribute of Type {T}. Otherwise, null.</returns>
        [DebuggerStepThrough]
        public static T GetCustomAttribute<T>( this Type type, bool inherit = false ) where T : Attribute {

            object[] attributes = type.GetCustomAttributes( typeof( T ), inherit );

            if( null == attributes ) {
                return null;
            }

            if( attributes.Length == 0 ) {
                return null;
            }

            return attributes[0] as T;
        }
    }
}