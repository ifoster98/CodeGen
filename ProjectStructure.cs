namespace CodeGen 
{
    public static class ProjectStructure
    {
        public static ProjectDirectories CreateDirectories(string baseDirectory, string moduleName)
        {
            var directories = new ProjectDirectories
            {
//                BLControllerDirectory = $"Bes4.{moduleName}.BL/Controllers",
                DomainDirectory = $"Ianf.{moduleName}.Domain",
//                FilterDirectory = $"Bes4.{moduleName}.Dal/Filter",
//                EntityDirectory = $"Bes4.{moduleName}.Dal/Entities",
//                InterfaceDirectory = $"Bes4.{moduleName}.Dal/Interfaces",
//                DtoDirectory = $"Bes4.{moduleName}.Dto/{entityName}",
//                ServiceDirectory = $"Bes4.{moduleName}.Service/Controllers",
//                RepositoryDirectory = $"Bes4.{moduleName}.RepositoryEF/Dal",
                TestDirectory = $"Ianf.{moduleName}.UnitTest"
            };

//            CreateDirectory(directories.BLControllerDirectory);
            CreateDirectory(directories.DomainDirectory);
//            CreateDirectory(directories.FilterDirectory);
//            CreateDirectory(directories.EntityDirectory);
//            CreateDirectory(directories.InterfaceDirectory);
//            CreateDirectory(directories.DtoDirectory);
//            CreateDirectory(directories.ServiceDirectory);
//            CreateDirectory(directories.RepositoryDirectory);
            CreateDirectory(directories.TestDirectory);

            return directories;

            void CreateDirectory(string dirName) => System.IO.Directory.CreateDirectory($"{baseDirectory}/{dirName}");
        }
    }

    public struct ProjectDirectories 
    {
        public string BLControllerDirectory { get; set; }
        public string DomainDirectory { get; set; }
        public string FilterDirectory { get; set; }
        public string EntityDirectory { get; set; }
        public string InterfaceDirectory { get; set; }
        public string DtoDirectory { get; set; }
        public string ServiceDirectory { get; set; }
        public string RepositoryDirectory { get; set; }
        public string TestDirectory { get; set; }
    }
}
