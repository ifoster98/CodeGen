///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Deploy");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
   CleanDirectory($"./bin");
   CleanDirectory($"./obj");
});

Task("Build")
.IsDependentOn("Clean")
.Does(() => {
   DotNetCoreBuild("./CodeGen.csproj", new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
});

Task("Deploy")
.IsDependentOn("Build")
.Does(() => {
   var settings = new DotNetCorePublishSettings
   {
      Configuration = "Release",
      OutputDirectory = "/Users/ianfoster/.local/bin"
   };

   DotNetCorePublish("./CodeGen.csproj", settings);
});

RunTarget(target);