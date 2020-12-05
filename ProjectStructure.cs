namespace CodeGen 
{
    public static class ProjectStructure
    {
        public static ProjectDirectories CreateDirectories(AllOptions opts)
        {
            var dirName = $"{opts.Business}.{opts.Application}.{opts.Module}";
            var directories = new ProjectDirectories
            {
                DomainDirectory = $"{dirName}/Domain",
                TestDirectory = $"{dirName}.UnitTest/Domain"
            };

            CreateDirectory(directories.DomainDirectory);
            CreateDirectory(directories.TestDirectory);

            return directories;

            void CreateDirectory(string dirName) => System.IO.Directory.CreateDirectory($"{opts.OutputDirectory}/{dirName}");
        }
    }

    public struct ProjectDirectories 
    {
        public string DomainDirectory { get; set; }
        public string TestDirectory { get; set; }
    }
}
