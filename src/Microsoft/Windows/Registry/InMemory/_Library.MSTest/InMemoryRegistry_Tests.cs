using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory {

    [TestClass]
    [TestCategory( "OSI.Microsoft.Windows.Registry" )]
    [TestCategory( "OSI.Microsoft.Windows.Registry.InMemory" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0059:Remove unnecessary value assignment", Justification = "Not relevant for Unit Tests" )]
    public class InMemoryRegistry_Tests {

        [TestMethod]
        public void InMemoryRegistryKey_Values_Properties_and_Methods() {

            IRegistry registry = new InMemoryRegistry( RegistryView.Registry64 );
            IRegistryKey hklm = registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default );

            // ValueCount Property - before adding/setting a value
            Assert.AreEqual( 0, hklm.ValueCount );

            // SetValue Methods
            hklm.SetValue( "MyFirstValue", "Some meaningful value", RegistryValueKind.String );

            // ValueCount Property - after adding/setting a value
            Assert.AreEqual( 1, hklm.ValueCount );

            // GetValueKind Method
            Assert.AreEqual( RegistryValueKind.String, hklm.GetValueKind( "MyFirstValue" ) ); // Test on existing value
            // TODO: Add tests for calling the method for non-existent value (Should throw)

            string[] valueNames = hklm.GetValueNames();
            Assert.AreEqual( hklm.ValueCount, valueNames.Length );

            // GetValue Methods

            // Accessing non-existant value returns null
            object regVal01 = hklm.GetValue( "Non-Existant-Registry-Value-should-return-null" );
            Assert.IsNull( regVal01 );

            // Accessing existing value with different casing returns correct value. Registry Value Names are case-insensitive
            object regVal02 = hklm.GetValue( "myfirstvalue" );
            Assert.AreEqual( regVal02, "Some meaningful value" );

            // Update stored value with new value and retrieve it

            // DeleteValue Methods

            hklm.DeleteValue( "myfirstvalue" );
            Assert.AreEqual( 0, hklm.ValueCount );

            // Default Value handling

            hklm.SetValue( "", "", RegistryValueKind.String );
            Assert.AreEqual( "", hklm.GetValue( null ) );

        }

        [TestMethod]
        public void InMemoryRegistryKey_SetValues_DetectValueKind() {
            IRegistry registry = new InMemoryRegistry( RegistryView.Registry64 );

            IRegistryKey hklm = registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default );

            hklm.SetValue( "StringValue", "Hello World" );
            hklm.SetValue( "MultiStringValue", new string[] { "a", "b", "c" } );
            hklm.SetValue( "BinaryValue", new byte[] { 0x00, 0x01, 0x02 } );
            hklm.SetValue( "IntValue", 42 );
            hklm.SetValue( "LongValue", 42L );

            Assert.AreEqual( RegistryValueKind.String, hklm.GetValueKind( "StringValue" ) );
            Assert.AreEqual( RegistryValueKind.MultiString, hklm.GetValueKind( "MultiStringValue" ) );
            Assert.AreEqual( RegistryValueKind.Binary, hklm.GetValueKind( "BinaryValue" ) );
            Assert.AreEqual( RegistryValueKind.DWord, hklm.GetValueKind( "IntValue" ) );
            Assert.AreEqual( RegistryValueKind.String, hklm.GetValueKind( "LongValue" ) );

        }

        [TestMethod]
        public void InMemoryRegistryKey_Subkey_Properties_and_Methods() {

            IRegistry registry = new InMemoryRegistry( RegistryView.Registry64 );

            IRegistryKey hklm = registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default );

            IRegistryKey key01 = hklm.CreateSubKey( @"Software", true );
            IRegistryKey key02 = hklm.CreateSubKey( @"Software\Microsoft", true );

            IRegistryKey key03 = hklm.CreateSubKey( @"SYSTEM\CurrentControlSet", false );
            Assert.IsInstanceOfType( key03, typeof( ReadOnlyInMemoryRegistryKey ) );

            IRegistryKey key04 = hklm.OpenSubKey( @"SYSTEM\CurrentControlSet", true );
            Assert.IsInstanceOfType( key04, typeof( InMemoryRegistryKey ) );

            IRegistryKey key05 = hklm.OpenSubKey( @"Software", false );
            Assert.IsInstanceOfType( key05, typeof( ReadOnlyInMemoryRegistryKey ) );

            // Delete Subkey addressed over 2 levels
            hklm.DeleteSubKey( @"Software\Microsoft", true );

            // Delete SubKeyTree addressed over multiple levels
            // TODO: Validate this behavior with MS implementation

            // Create a deeper tree first.
            IRegistryKey key06 = hklm.CreateSubKey( @"SOFTWARE\Classes\Directory\shellEx\ContextMenuHandlers\MyHandler" );

            // Now delete part of the tree, addressed over multiple levels.
            hklm.DeleteSubKeyTree( @"SOFTWARE\Classes\Directory\shellEx" );

            // Validate results
            Assert.IsNotNull( hklm.OpenSubKey( @"SOFTWARE\Classes\Directory" ) );
            Assert.IsNull( hklm.OpenSubKey( @"SOFTWARE\Classes\Directory\shellEx" ) );
        }
    }
}
