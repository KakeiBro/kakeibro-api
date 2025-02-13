var target = Argument("target", "CopyGitHooks"); // It will run up until the target Task
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./src/KakeiBro.API/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./KakeiBro.API.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("./KakeiBro.API.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

// Custom task for copying git-hooks
Task("CopyGitHooks")
    .IsDependentOn("Test")
    .Does(() =>
{
    var sourcePath = "../git-hooks";
    var destinationPath = "../.git/hooks";

    // Ensure the destination path exists (if it exists it doesn't do anything)
    CreateDirectory(destinationPath);

    // Copy all content from source to destination
    CopyFiles($"{sourcePath}/**", destinationPath);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
