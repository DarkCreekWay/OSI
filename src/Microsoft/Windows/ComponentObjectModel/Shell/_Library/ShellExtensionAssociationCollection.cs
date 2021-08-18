using System;
using System.Collections;
using System.Collections.Generic;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// Manages a collection of progids for associating them with a shell extension.
    /// </summary>
    public class ShellExtensionAssociationCollection
    : IEnumerable<string> {

        List<string> _list = new List<string>();

        /// <summary>
        /// Gets or sets the progId at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>The progId at the specificed index.</returns>
        public string this[int index] {
            get => _list[index];
        }

        /// <summary>
        /// Gets the number of progIds.
        /// </summary>
        public int Count {
            get => _list.Count;
        }

        /// <summary>
        /// Adds a progid to the collection.
        /// </summary>
        /// <param name="progId">The progId.</param>
        public void Add( string progId ) {

            _list.Add( progId );
        }

        /// <summary>
        /// Adds a <seealso cref="PredefinedShellObject"/> to the collection.
        /// </summary>
        /// <param name="predefinedShellObject">The <seealso cref="PredefinedShellObject"/>.</param>
        public void Add( PredefinedShellObject predefinedShellObject ) {
            _list.Add( predefinedShellObject.ProgId );
        }

        /// <summary>
        /// Adds a collection of progIds.
        /// </summary>
        /// <param name="collection">The collection of progIds.</param>

        public void AddRange( IEnumerable<string> collection ) {

            _list.AddRange( collection );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns><seealso cref="IEnumerator{T}"/>, where T is string.</returns>
        public IEnumerator<string> GetEnumerator() {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns><seealso cref="IEnumerator"/></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
