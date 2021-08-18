using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Provides useful utility methods.
    /// </summary>
    public static class Utils {

        /// <summary>
        /// Convert a <see cref="Guid"/> to a clsid string.
        /// </summary>
        /// <param name="guid">The <see cref="Guid"/> to convert.</param>
        /// <returns>A clsid string.</returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string GuidToClsid( Guid guid ) {
            return guid.ToString( "B" );
        }

        /// <summary>
        /// Converts a clsid string to a <see cref="Guid"/>
        /// </summary>
        /// <param name="clsid">The clsid string to convert.</param>
        /// <returns>A <see cref="Guid"/>.</returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Guid ClsidToGuid( string clsid ) {
            return Guid.Parse( clsid );
        }

        /// <summary>
        /// Constructs the path to the dll from any given path.
        /// </summary>
        /// <param name="path">The input path.</param>
        /// <returns>The path to the dll.</returns>
        /// <remarks>
        /// This method can construct the path to the dll from pathes in the registry like the ComHost path of .NET core.
        /// </remarks>
        public static string GetLibraryPath( string path ) {

            return GetBasePath( path ) + ".dll";

        }

        /// <summary>
        /// Constructs the path to the ComHost dll for a given path.
        /// </summary>
        /// <param name="path">The input path.</param>
        /// <returns>The path to the comhost.dll</returns>
        public static string GetComHostPath( string path ) {
            return GetBasePath( path ) + ".comhost.dll";
        }

        /// <summary>
        /// Constructs a base path from a given path.
        /// </summary>
        /// <param name="path">The input path.</param>
        /// <returns>The base path.</returns>
        /// <remarks>
        /// The base path can be constructed from pathes to a comhost.dll.
        /// </remarks>
        public static string GetBasePath( string path ) {

            string basePath        = Path.GetDirectoryName( Path.GetFullPath( path) );
            string filename        = Path.GetFileName( path );
            string[] filenameParts = filename.Split( '.' );

            if ( filenameParts.Length < 2 ) {
                throw new FormatException();
            }

            int filenamePartsCount = 0;

            // Dynamic Link Libraries (*.dll)
            // <library>.dll
            // <library>.comhost.dll

            if ( filenameParts[filenameParts.Length - 1].Equals( "dll", StringComparison.OrdinalIgnoreCase ) ) {

                filenamePartsCount = filenameParts.Length - 1;

                if ( filenameParts.Length >= 3 ) {
                    if ( filenameParts[filenameParts.Length - 2].Equals( "comhost", StringComparison.OrdinalIgnoreCase ) ) {
                        filenamePartsCount -= 1;
                    }
                }
            }

            // .json config files
            if ( filenameParts[filenameParts.Length - 1].Equals( "json", StringComparison.OrdinalIgnoreCase ) ) {
                throw new NotImplementedException();
            }

            return Path.Combine( basePath, string.Join( ".", filenameParts, 0, filenamePartsCount ) );
        }
    }
}
