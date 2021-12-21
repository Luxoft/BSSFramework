namespace Framework.DomainDriven
{
    public struct Parameters
    {

        public Parameters(string outputPath, string assemblyPath, string domainObjectBaseName,
            string databaseServerName, string databaseName, string generatedAssemblyName) : this()
        {
            this.GeneratedAssemblyName = generatedAssemblyName;
            this.OutputPath = outputPath;
            this.AssemblyPath = assemblyPath;
            this.DomainObjectBaseName = domainObjectBaseName;
            this.DatabaseServerName = databaseServerName;
            this.DatabaseName = databaseName;
        }

        /// <summary>
        /// ѕуть генерации cs файлов
        /// </summary>
        public string OutputPath { get; private set; }
        public string AssemblyPath { get; private set; }
        public string DomainObjectBaseName { get; private set; }
        public string DatabaseServerName { get; private set; }
        public string DatabaseName { get; private set; }
        public string GeneratedAssemblyName { get; private set; }
    }

}