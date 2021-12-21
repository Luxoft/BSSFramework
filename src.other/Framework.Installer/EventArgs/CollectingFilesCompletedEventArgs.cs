namespace Framework.Installer.EventArgs
{
    /// <summary>
    /// Represents arguments for event CollectingFilesCompleted
    /// </summary>
    public class CollectingFilesCompletedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Absolute path to folder with files
        /// </summary>
        public string PathToTempFolder { get; set; }

        /// <summary>
        /// Version of application
        /// </summary>
        public string ApplicationVersion { get; set; }
    }
}