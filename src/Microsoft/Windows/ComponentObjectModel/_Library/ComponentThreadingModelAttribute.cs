using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Specifies the <seealso cref="ComponentThreadingModel"/>.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public sealed class ComponentThreadingModelAttribute
    : Attribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentThreadingModelAttribute"/> class
        /// for the ThreadingModel <see cref="ComponentThreadingModel.Both"/>
        /// </summary>
        public ComponentThreadingModelAttribute()
        : this( ComponentThreadingModel.Both ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentThreadingModelAttribute"/> class.
        /// </summary>
        /// <param name="threadingModel">The threading model.</param>
        public ComponentThreadingModelAttribute( ComponentThreadingModel threadingModel ) {

            if ( threadingModel == ComponentThreadingModel.Undefined ) {
                throw new ArgumentOutOfRangeException( nameof( threadingModel ) );
            }

            ThreadingModel = threadingModel;
        }

        /// <summary>
        /// Gets the threading model.
        /// </summary>
        public ComponentThreadingModel ThreadingModel {
            get;
        }

        /// <summary>
        /// Get the <seealso cref="ComponentThreadingModelAttribute"/> from a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="ComponentThreadingModelAttribute"/> if found. Otherwise, null.</returns>

        public static ComponentThreadingModelAttribute GetAttribute( Type type ) {

            return type.GetCustomAttribute<ComponentThreadingModelAttribute>( true );
        }
    }
}
