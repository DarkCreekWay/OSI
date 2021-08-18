using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata {

    [TestClass]
    [TestCategory("OSI.Microsoft.Windows.ComponentObjectModel.IO")]
    public class ComponentLibraryMetadataReader_Tests {

        [TestMethod]
        [DeploymentItem( @"..\..\..\Assets\", "Assets" )]
        public void ComponentLibraryMetadataReader_Read() {

            // TODO: Validate, that inspected assemblies do net get loaded into the AppDomain

            string[] componentFolders = Directory.GetDirectories( @".\Assets" );
            ComponentLibraryMetadataReader  reader = new ComponentLibraryMetadataReader ();

            for ( int i = 0 ; i < componentFolders.Length ; i++ ) {

                // Name of folder is name of component (by convention)

                // GetFileName returns the last element of any given path.
                // Calling it with a path to a folder returns the folder name only.
                string componentName   = Path.GetFileName(componentFolders[i]);
                string componentFile   = componentName + ".dll";
                string componentFolder = Path.GetFullPath(componentFolders[i]);
                string componentPath   = Path.Combine(componentFolder, componentFile);

                ComponentLibraryMetadata metadata = reader.Read( componentPath );

                Console.WriteLine( $"Image Path: {metadata.Path}" );
                Console.WriteLine( $"Image Type: {metadata.ImageType}" );
                Console.WriteLine();

                Console.WriteLine( $"Assembly Name                       : {metadata.AssemblyName}" );
                Console.WriteLine( $"Assembly Target Framework           : {metadata.AssemblyTargetFramework}" );
                Console.WriteLine( $"Assembly Target Framework Version   : {metadata.AssemblyTargetFrameworkVersion}" );
                Console.WriteLine();
                ;
            }
        }

        [TestMethod]
        public void Filename_Tests() {

            List<string> filenames = new List<string>() {
             "NetCore31Component.dll",
             "NetCore31Component.comhost.dll",
             "NetCore31Component.deps.json",
             "NetCore31Component.runtimeconfig.json",
             "NetCore31Component.runtimeconfig.dev.json",
            };

            for ( int i = 0 ; i < filenames.Count ; i++ ) {

                string filename = filenames[i];
                string basename = Path.GetFileNameWithoutExtension(filename);
                string ext1     = Path.GetExtension(filename);
                string ext2     = Path.GetExtension(basename);

                Console.WriteLine( filename );
                Console.WriteLine( basename );
                Console.WriteLine( ext1 );
                Console.WriteLine( ext2 );
            }
        }
    }
}
