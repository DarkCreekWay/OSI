namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// The registration state of a shell extension.
    /// </summary>
    public enum ShellExtensionRegistrationState {

        /// <summary>
        /// The registration state is unkown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The shell extension is not registered.
        /// </summary>
        NotRegistered,

        /// <summary>
        /// The shell extension is partially registered.
        /// </summary>
        /// <remarks>
        /// This state is used, when a shell extension demands a set of associations, but not all of them are found in the registry.
        /// </remarks>
        PartiallyRegistered,

        /// <summary>
        /// The shell extension is registered.
        /// </summary>
        Registered
    }
}
