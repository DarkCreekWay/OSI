using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Specifies the <seealso cref="ComponentServerType"/>.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public sealed class ComponentServerTypeAttribute
    : Attribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentServerTypeAttribute"/>.
        /// </summary>
        public ComponentServerTypeAttribute()
        : this( ComponentServerType.InprocServer32 ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentServerTypeAttribute"/>.
        /// </summary>
        /// <param name="serverType">The server type.</param>
        public ComponentServerTypeAttribute( ComponentServerType serverType ) {

            if ( serverType == ComponentServerType.Undefined ) {
                throw new ArgumentOutOfRangeException( nameof( serverType ) );
            }

            ServerType = serverType;
        }

        /// <summary>
        /// Gets the server type.
        /// </summary>
        public ComponentServerType ServerType {
            get;
        }

        /// <summary>
        /// Get the <seealso cref="ComponentServerTypeAttribute"/> from a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <seealso cref="ComponentServerTypeAttribute"/> if found. Otherwise, null.</returns>
        public static ComponentServerTypeAttribute GetAttribute( Type type ) {

            return type.GetCustomAttribute<ComponentServerTypeAttribute>( true );

        }
    }
}
