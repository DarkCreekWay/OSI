namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Defines the Threading model of a component.
    /// </summary>
    /// <remarks>
    /// <seealso href="https://docs.microsoft.com/en-us/windows/win32/cossdk/threading-model-attribute">Threading Model Attribute - Microsoft Docs</seealso>
    /// </remarks>
    public enum ComponentThreadingModel {

        /// <summary>
        /// The threading model is undefined.
        /// </summary>
        Undefined,

        /// <summary>
        /// Single threaded apartment.
        /// </summary>
        Apartment,

        /// <summary>
        /// Single-threaded or multithreaded apartment
        /// </summary>
        Both,

        /// <summary>
        /// Multithreaded apartment.
        /// </summary>
        Free,

        /// <summary>
        /// Neutral apartment
        /// </summary>
        Neutral,
    }
}
