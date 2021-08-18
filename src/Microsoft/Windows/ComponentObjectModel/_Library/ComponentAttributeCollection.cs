using System;
using System.Collections.Generic;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Manages custom <see cref="Attribute"/> instances.
    /// </summary>
    public class ComponentAttributeCollection {

        Dictionary<string,List<Attribute>> _attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentAttributeCollection"/> class.
        /// </summary>
        public ComponentAttributeCollection() {
            _attributes = new Dictionary<string, List<Attribute>>();
        }

        /// <summary>
        /// Adds an attribute to the collection.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void Add( Attribute attribute ) {

            string name = attribute.GetType().FullName;
            if ( false == _attributes.TryGetValue( name, out List<Attribute> list ) ) {
                list = new List<Attribute>();
                _attributes.Add( name, list );
            }

            list.Add( attribute );
        }

        /// <summary>
        /// Gets a typed attribute from the collection.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <returns>if found within the collection, The typed attribute. Otherwise, default.</returns>
        public T Get<T>() where T : Attribute {
            if ( false == _attributes.TryGetValue( typeof( T ).FullName, out List<Attribute> list ) ) {
                return default;
            }

            if ( list.Count == 0 ) {
                return default;
            }

            if ( list.Count > 1 ) {
                throw new InvalidOperationException();
            }

            return (T)list[0];
        }

        /// <summary>
        /// Returns a List of typed attributes contained in this collection.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <returns>A list of typed attributes, as specified by T or default, when attribute is not found in the collection.</returns>
        public List<T> ToList<T>() where T : Attribute {

            if ( false == _attributes.TryGetValue( typeof( T ).FullName, out List<Attribute> list ) ) {
                return default;
            }

            List<T> result = new List<T>(list.Count);

            for ( int i = 0 ; i < list.Count ; i++ ) {
                result.Add( (T)list[i] );
            }

            return result;
        }

        /// <summary>
        /// Tries to get a typed attribute from this collection.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="attribute">[Out] parameter, containing the attribute, or null, when not found in the collection.</param>
        /// <returns>true, if the attribute was found. Otherwise, false.</returns>
        public bool TryGet<T>( out T attribute ) where T : Attribute {

            if ( false == _attributes.TryGetValue( typeof( T ).FullName, out List<Attribute> list ) ) {
                attribute = default;
                return false;
            }

            if ( list.Count == 0 ) {
                attribute = default;
                return false;
            }

            if ( list.Count > 1 ) {
                throw new InvalidOperationException();
            }

            attribute = (T)list[0];
            return true;

        }
    }
}
