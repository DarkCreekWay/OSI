using System;
using System.Collections.Generic;

namespace DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory {

    ///<inheritdoc/>
    public class InMemoryRegistry
    : IRegistry {

        BaseKeys _32Bit = new BaseKeys();
        BaseKeys _64Bit = new BaseKeys();
        BaseKeys _default = null;

        RegistryView _defaultView;

        /// <summary>
        /// Initializes a new instance of the InMemoryRegistry class.
        /// </summary>
        /// <param name="defaultView">The <see cref="RegistryView"/> to be used as default view type.</param>
        public InMemoryRegistry( RegistryView defaultView ) {

            switch ( defaultView ) {

                case RegistryView.Default:
                    throw new ArgumentOutOfRangeException( nameof( defaultView ), "Please specify a concrete RegistryView (32/64 bit) as default" );

                case RegistryView.Registry32:
                    _default = _32Bit;
                    break;

                case RegistryView.Registry64:
                    _default = _64Bit;
                    break;

                default:
                    throw new ArgumentOutOfRangeException( nameof( defaultView ) );

            }
            _defaultView = defaultView;

            RegistryHive[] hives = (RegistryHive[])Enum.GetValues(typeof(RegistryHive));

            foreach ( RegistryHive hive in hives ) {
                _32Bit.Keys.Add( hive, new InMemoryRegistryKey( null, GetNameforHive( hive ) ) );
                _64Bit.Keys.Add( hive, new InMemoryRegistryKey( null, GetNameforHive( hive ) ) );
            }
        }

        ///<inheritdoc/>
        public IRegistryKey OpenBaseKey( RegistryHive hKey, RegistryView view = RegistryView.Default ) {

            BaseKeys keys = GetKeysForView(view);
            return keys.Keys[hKey];

        }

        BaseKeys GetKeysForView( RegistryView view ) {

            switch ( view ) {

                case RegistryView.Default:
                    return _default;

                case RegistryView.Registry32:
                    return _32Bit;

                case RegistryView.Registry64:
                    return _64Bit;

                default:
                    throw new ArgumentOutOfRangeException( nameof( view ) );
            }
        }

        string GetNameforHive( RegistryHive hive ) {

            switch ( hive ) {
                case RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";
                case RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";
                case RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";
                case RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";
                case RegistryHive.PerformanceData:
                    return "HKEY_PERFORMANCE_DATA";
                case RegistryHive.Users:
                    return "HKEY_USERS";
                default:
                    throw new ArgumentOutOfRangeException( nameof( hive ) );
            }
        }

        /// <summary>
        /// Internal class for managing base registry keys.
        /// </summary>
        class BaseKeys {

            public Dictionary<RegistryHive,IRegistryKey> Keys = new Dictionary<RegistryHive, IRegistryKey>();

        }

    }
}

// TODO: Add type for parsing a .reg file. The parsed .reg file can be used for (pre-loading) building an InMemory-Registry from a .reg file.
// TODO: Add type for writing a .reg gile. An InMemory-Registry can be dumped to a .reg file
// https://stackoverflow.com/questions/334207/regedit-file-format
// https://docs.microsoft.com/en-us/windows/win32/sysinfo/registry-files
// https://de.wikipedia.org/wiki/Registrierungsdatenbank
// https://support.microsoft.com/de-de/help/310516/how-to-add-modify-or-delete-registry-subkeys-and-values-by-using-a-reg
// https://archive.is/n4sj6