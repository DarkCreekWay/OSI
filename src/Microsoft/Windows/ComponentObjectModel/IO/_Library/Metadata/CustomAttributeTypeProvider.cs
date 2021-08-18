using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata {

    /// <summary>
    /// Provides methods for converting metadata to types.
    /// </summary>
    public class CustomAttributeTypeProvider
    : ICustomAttributeTypeProvider<Type> {

        Dictionary<string,Type> _referencedTypeMap;
        Dictionary<Type,PrimitiveTypeCode> _typeToTypeCodeMap;

        /// <summary>
        /// Creates a new instance of the <see cref="CustomAttributeTypeProvider"/>
        /// </summary>
        public CustomAttributeTypeProvider() {

            CreateTypeToTypeCodeMap();
            CreateReferencedTypeMap();
            AddAdditionalReferenceTypesToMap();
        }

        /// <summary>
        /// Provide additional reference types to be used for conversion between <see cref="TypeReferenceHandle"/> and <see cref="System.Type"/> instances.
        /// </summary>
        /// <returns>When overriden by derieved types, an <see cref="IEnumerable{T}"/> where T is <see cref="System.Type"/>. Otherwise, null.</returns>
        protected virtual IEnumerable<Type> GetAdditionalReferenceTypes() {
            return null;
        }

        /// <summary>
        /// Test, if a given Type is a System Type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true, if <paramref name="type"/> value is a system type. Otherwise, false.</returns>
        public bool IsSystemType( Type type ) {

            return false;
        }

        /// <summary>
        /// >Returns a <see cref="System.Type"/>, that is underlying a given Enum Type.
        /// </summary>
        /// <param name="type">The Enum Type.</param>
        /// <returns>The underlying <see cref="System.Type"/> of an Enum type.</returns>
        public PrimitiveTypeCode GetUnderlyingEnumType( Type type ) {
            return GetPrimitiveTypeCode( Enum.GetUnderlyingType( type ) );
        }

        /// <summary>
        /// Returns a <see cref="System.Type"/> for a given <see cref="PrimitiveTypeCode"/>
        /// </summary>
        /// <param name="typeCode">The <see cref="PrimitiveTypeCode"/></param>
        /// <returns>A <see cref="System.Type"/> corresponding to the <paramref name="typeCode"/> value.</returns>
        public Type GetPrimitiveType( PrimitiveTypeCode typeCode ) {

            switch ( typeCode ) {
                case PrimitiveTypeCode.Boolean:
                    return typeof( bool );

                case PrimitiveTypeCode.Byte:
                    return typeof( byte );

                case PrimitiveTypeCode.Char:
                    return typeof( char );

                case PrimitiveTypeCode.Double:
                    return typeof( double );

                case PrimitiveTypeCode.Int16:
                    return typeof( short );

                case PrimitiveTypeCode.Int32:
                    return typeof( int );

                case PrimitiveTypeCode.Int64:
                    return typeof( long );

                case PrimitiveTypeCode.IntPtr:
                    return typeof( IntPtr );

                case PrimitiveTypeCode.Object:
                    return typeof( object );

                case PrimitiveTypeCode.SByte:
                    return typeof( sbyte );

                case PrimitiveTypeCode.Single:
                    return typeof( float );

                case PrimitiveTypeCode.String:
                    return typeof( string );

                case PrimitiveTypeCode.TypedReference:
                    return typeof( TypedReference );

                case PrimitiveTypeCode.UInt16:
                    return typeof( ushort );

                case PrimitiveTypeCode.UInt32:
                    return typeof( uint );

                case PrimitiveTypeCode.UInt64:
                    return typeof( ulong );

                case PrimitiveTypeCode.UIntPtr:
                    return typeof( UIntPtr );

                case PrimitiveTypeCode.Void:
                    return null;

                default:
                    throw new ArgumentOutOfRangeException( nameof( typeCode ), typeCode.ToString() );
            }
        }

        /// <summary>
        /// Gets the TType representation for Type.
        /// </summary>
        /// <returns><see cref="System.Type"/></returns>
        /// <exception cref="System.NotImplementedException"/>
        public Type GetSystemType() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a <see cref="TypeDefinitionHandle"/> to a type.
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/></param>
        /// <param name="handle">The <see cref="TypeDefinitionHandle"/></param>
        /// <param name="rawTypeKind">The raw type kind.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"/>
        public Type GetTypeFromDefinition( MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="System.Type"/> identified by a serialized type name.
        /// </summary>
        /// <param name="name">The serialized name of the Type.</param>
        /// <returns>The <see cref="System.Type"/> identified by a serialized type name.</returns>
        /// <exception cref="System.NotImplementedException"/>
        public Type GetTypeFromSerializedName( string name ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a <see cref="TypeReferenceHandle"/> to a type.
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/>.</param>
        /// <param name="handle">The <see cref="TypeReferenceHandle"/>.</param>
        /// <param name="rawTypeKind">The raw type kind.</param>
        /// <returns>The converted Type on success. Otherwise, null.</returns>
        public Type GetTypeFromReference( MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind ) {

            // This code does not work with nested types and needs some revision.

            TypeReference typeReference = reader.GetTypeReference( handle );
            string typeNamespace = reader.GetString( typeReference.Namespace );
            string typeName      = reader.GetString( typeReference.Name );
            string fullTypeName  = typeNamespace + "." + typeName;

            if ( true == _referencedTypeMap.TryGetValue( fullTypeName, out Type type ) ) {
                return type;
            }

            return null;
        }

        /// <summary>
        /// Gets the type for a single-dimensional array of the given element type with a lower bounds of zero.
        /// </summary>
        /// <param name="elementType">The element type.</param>
        /// <returns>The type.</returns>
        public Type GetSZArrayType( Type elementType ) {
            return Array.CreateInstance( elementType, 0 ).GetType();
        }

        void CreateTypeToTypeCodeMap() {

            _typeToTypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>() {
                { typeof( bool )           , PrimitiveTypeCode.Boolean        },
                { typeof( byte )           , PrimitiveTypeCode.Byte           },
                { typeof( char )           , PrimitiveTypeCode.Char           },
                { typeof( double )         , PrimitiveTypeCode.Double         },
                { typeof( short )          , PrimitiveTypeCode.Int16          },
                { typeof( int )            , PrimitiveTypeCode.Int32          },
                { typeof( long )           , PrimitiveTypeCode.Int64          },
                { typeof( IntPtr )         , PrimitiveTypeCode.IntPtr         },
                { typeof( object )         , PrimitiveTypeCode.Object         },
                { typeof( sbyte )          , PrimitiveTypeCode.SByte          },
                { typeof( float )          , PrimitiveTypeCode.Single         },
                { typeof( string )         , PrimitiveTypeCode.String         },
                { typeof( TypedReference ) , PrimitiveTypeCode.TypedReference },
                { typeof( ushort )         , PrimitiveTypeCode.UInt16         },
                { typeof( uint )           , PrimitiveTypeCode.UInt32         },
                { typeof( ulong )          , PrimitiveTypeCode.UInt64         },
                { typeof( UIntPtr )        , PrimitiveTypeCode.UIntPtr        },
            };
        }

        PrimitiveTypeCode GetPrimitiveTypeCode( Type type ) {

            if ( null == type ) {
                return PrimitiveTypeCode.Void;
            }

            return _typeToTypeCodeMap[type];

        }

        void CreateReferencedTypeMap() {

            _referencedTypeMap = new Dictionary<string, Type>() {
                { typeof(ComponentServerType).FullName, typeof(ComponentServerType) },
                { typeof(ComponentThreadingModel).FullName, typeof(ComponentThreadingModel) },
            };

        }

        void AddAdditionalReferenceTypesToMap() {

            IEnumerable<Type> enumerable = GetAdditionalReferenceTypes();
            if ( null == enumerable ) {
                return;
            }

            foreach ( Type type in enumerable ) {
                _referencedTypeMap.Add( type.FullName, type );
            }
        }
    }
}

