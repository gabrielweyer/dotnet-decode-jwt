#tool dotnet:?package=GitVersion.Tool&version=5.8.2

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

var artifactsDir = MakeAbsolute(Directory("artifacts"));
var packagesDir = artifactsDir.Combine(Directory("packages"));

var solutionPath = "./dotnet-decode-jwt.sln";

Task("Clean")
    .Does(() =>
    {
        var settings = new DotNetCleanSettings
        {
            Configuration = configuration
        };

        DotNetClean(solutionPath, settings);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetRestore();
    });

Task("SemVer")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var gitVersion = GitVersion();

        assemblyVersion = gitVersion.AssemblySemVer;
        packageVersion = gitVersion.NuGetVersion;

        Information($"AssemblySemVer: {assemblyVersion}");
        Information($"NuGetVersion: {packageVersion}");
    });

Task("SetAppVeyorVersion")
    .IsDependentOn("Semver")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(packageVersion);
    });

Task("Build")
    .IsDependentOn("SetAppVeyorVersion")
    .Does(() =>
    {
        var settings = new DotNetBuildSettings
        {
            Configuration = configuration,
            NoIncremental = true,
            NoRestore = true,
            MSBuildSettings = new DotNetMSBuildSettings()
                .SetVersion(assemblyVersion)
                .WithProperty("FileVersion", packageVersion)
                .WithProperty("InformationalVersion", packageVersion)
                .WithProperty("nowarn", "7035")
        };

        DotNetBuild(solutionPath, settings);
    });


Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true
        };

        GetFiles("./tests/*/*-tests.csproj")
            .ToList()
            .ForEach(f => DotNetTest(f.FullPath, settings));
    });

Task("Pack")
    .IsDependentOn("Test")
    .WithCriteria(() => HasArgument("pack"))
    .Does(() =>
    {
        var settings = new DotNetPackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            OutputDirectory = packagesDir,
            MSBuildSettings = new DotNetMSBuildSettings()
                .WithProperty("PackageVersion", packageVersion)
        };

        GetFiles("./src/*/*.csproj")
            .ToList()
            .ForEach(f => DotNetPack(f.FullPath, settings));
    });

Task("PublishAppVeyorArtifacts")
    .IsDependentOn("Pack")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        CopyFiles($"{packagesDir}/*.nupkg", MakeAbsolute(Directory("./")), false);

        GetFiles($"./*.nupkg")
            .ToList()
            .ForEach(f => AppVeyor.UploadArtifact(f, new AppVeyorUploadArtifactsSettings { DeploymentName = "packages" }));
    });

Task("Default")
    .IsDependentOn("PublishAppVeyorArtifacts");

RunTarget(target);
