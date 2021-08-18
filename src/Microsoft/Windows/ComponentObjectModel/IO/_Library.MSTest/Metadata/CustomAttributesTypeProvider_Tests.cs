using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata {

    [TestClass]
    public class CustomAttributesTypeProvider_Tests {

        [TestMethod]
        [TestCategory( ".NET" )]
        public void EnumBaseType() {

            Type enumType = typeof(ComponentServerType);
            Type underlyingType = Enum.GetUnderlyingType(enumType );
            Assert.AreEqual( typeof( int ), underlyingType );
            ;
        }
    }
}
