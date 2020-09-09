using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using Win32 = Microsoft.Win32;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry {

    [TestClass]
    [TestCategory( "OSI.Microsoft.Windows.Registry" )]
    public class Win32_Registry_Tests {

        [TestMethod]
        public void Win32Registry_ValueType_Detection_Behavior() {

            // Delete and recreate the test key.
            Win32.Registry.CurrentUser.DeleteSubKeyTree( "RegistrySetValueExample", false );
            Win32.RegistryKey rk = Win32.Registry.CurrentUser.CreateSubKey("RegistrySetValueExample");

            rk.CreateSubKey( "AnotherSubKey" );
            Win32.Registry.CurrentUser.DeleteSubKey( "RegistrySetValueExample\\AnotherSubKey", false );
            // Create name/value pairs.

            // Default Value Handling
            rk.SetValue( null, "<NULL>" );
            Assert.AreEqual( "<NULL>", rk.GetValue( "" ) );

            // Numeric values that cannot be interpreted as DWord (int) values
            // are stored as strings.
            rk.SetValue( "LargeNumberValue1", (long)42 );
            rk.SetValue( "LargeNumberValue2", 42000000000 );

            rk.SetValue( "DWordValue", 42 );
            rk.SetValue( "MultipleStringValue", new string[] { "One", "Two", "Three" } );
            rk.SetValue( "BinaryValue", new byte[] { 10, 43, 44, 45, 14, 255 } );

            // This overload of SetValue does not support expanding strings. Use
            // the overload that allows you to specify RegistryValueKind.
            rk.SetValue( "StringValue", "The path is %PATH%" );

            // Display all name/value pairs stored in the test key, with each
            // registry data type in parentheses.
            //
            string[] valueNames = rk.GetValueNames();
            foreach ( string s in valueNames ) {
                Win32.RegistryValueKind rvk = rk.GetValueKind(s);
                switch ( rvk ) {
                    case Win32.RegistryValueKind.MultiString:
                        string[] values = (string[]) rk.GetValue(s);
                        Console.Write( "\r\n {0} ({1}) = \"{2}\"", s, rvk, values[0] );

                        for ( int i = 1 ; i < values.Length ; i++ ) {
                            Console.Write( ", \"{0}\"", values[i] );
                        }

                        Console.WriteLine();
                        break;

                    case Win32.RegistryValueKind.Binary:
                        byte[] bytes = (byte[]) rk.GetValue(s);
                        Console.Write( "\r\n {0} ({1}) = {2:X2}", s, rvk, bytes[0] );

                        for ( int i = 1 ; i < bytes.Length ; i++ ) {
                            // Display each byte as two hexadecimal digits.
                            Console.Write( " {0:X2}", bytes[i] );
                        }

                        Console.WriteLine();
                        break;

                    default:
                        Console.WriteLine( "\r\n {0} ({1}) = {2}", s, rvk, rk.GetValue( s ) );
                        break;
                }
            }
        }

        [TestMethod]
        [ExpectedException( typeof( UnauthorizedAccessException ) )]
        public void Win32Registry_OpenSameKeyInDifferentModes_Behavior() {

            Win32.RegistryKey hkcu = Win32.RegistryKey.OpenBaseKey(Win32.RegistryHive.CurrentUser, Win32.RegistryView.Default);

            Win32.RegistryKey softwareKeyInReadOnlyMode = hkcu.OpenSubKey( "Software" );
            Win32.RegistryKey softwareKeyInWritableMode = hkcu.OpenSubKey( "Software", true);

            ;

            // Should not throw
            softwareKeyInWritableMode.SetValue( "Test write access", "true" );

            // Does the readonly regkey reflect the change by the writable reg key ?
            Assert.AreEqual( softwareKeyInWritableMode.ValueCount, softwareKeyInReadOnlyMode.ValueCount );

            softwareKeyInWritableMode.DeleteValue( "Test write access" );

            Assert.AreEqual( softwareKeyInWritableMode.ValueCount, softwareKeyInReadOnlyMode.ValueCount );

            // Should throw
            softwareKeyInReadOnlyMode.SetValue( "Test write access", "true" );
            softwareKeyInReadOnlyMode.DeleteValue( "Test write access" );

        }
    }
}
