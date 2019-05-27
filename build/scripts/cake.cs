var gitRemote = Argument<string>("gitRemote");
var srcDir = Argument<string>("srcDir");
var artifactsDir = Argument<string>("artifactsDir");
var target = Argument<string>("target", "Test");
var buildConfiguration = Argument<string>("buildConfiguration", "Release");
var buildVerbosity = (DotNetCoreVerbosity)Enum.Parse(typeof(DotNetCoreVerbosity), Argument<string>("buildVerbosity", "Minimal"));
var softwareVersion = Argument<string>("softwareVersion", string.Empty);
var buildNumber = Argument<int>("buildNumber", 0);
var commitHash = Argument<string>("commitHash");

const string testSuiteDirNameMarker = "TestSuite";

var srcDirInfo = new DirectoryInfo(srcDir);
var childDirInfos = srcDirInfo.GetDirectories();
var csprojFileInfos = childDirInfos.SelectMany(x => x.GetFiles("*.csproj")).ToList();
var csprojFiles = csprojFileInfos.Select(x => x.FullName).ToList();
var toBuildFileInfos = csprojFileInfos.Where(x => x.Directory.GetFiles("*.cs").Length != 0);
var toBuildFiles = toBuildFileInfos.Select(x => x.FullName).ToList();
var toTestFiles = toBuildFiles.Where(x => System.IO.File.ReadAllText(x).Contains("\"xunit\"")).ToList();

var perProjectMsBuildSettings = new Dictionary<string, DotNetCoreMSBuildSettings>();

Task("MSBuildSettings")
    .Does(() =>
    {
        var providedVersion = softwareVersion.Split(new [] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (providedVersion.Length == 1)
            providedVersion = new [] { providedVersion[0], ""};

        foreach (var project in csprojFiles)
        {
            var content = System.IO.File.ReadAllText(project);
            var document = new System.Xml.XmlDocument();
            document.LoadXml(content);
            var csprojProps = document.DocumentElement["PropertyGroup"];
            var csprojVersionXmlElem = csprojProps["InformationalVersion"] ?? csprojProps["Version"];
            var csprojVersion = (csprojVersionXmlElem?.InnerText ?? "0.1.0-alpha-01-commitHash").Split(new [] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var versionPrefix = providedVersion.Length == 0 ? csprojVersion[0].Split('.') : providedVersion[0].Split('.');
            var versionSuffix = (providedVersion.Length == 0 ? csprojVersion[1] : providedVersion[1]).Replace("commitHash", commitHash);
            if (versionSuffix.Length > 0)
                versionSuffix = "-" + versionSuffix;

            // AssemblyVersion
            // 1.0.0.0
            //
            // AssemblyFileVersion
            // 1.2.3.<BUILD_NUMBER>
            //
            // AssemblyInformationalVersion
            // 1.2.3(-alpha-commitHash)
            var assemblyVersion = String.Format("{0}.{1}.{1}.{1}", versionPrefix[0], 0);
            var assemblyFileVersion = String.Format("{0}.{1}.{2}.{3}", versionPrefix[0], versionPrefix[1], versionPrefix[2], buildNumber);
            var assemblyInformationalVersion = String.Format("{0}.{1}.{2}{3}", versionPrefix[0], versionPrefix[1], versionPrefix[2], versionSuffix);
            var packageReleaseNotesUrl = String.Format("{0}/releases/tag/v{1}", gitRemote, assemblyInformationalVersion);

            Information("Project: {0}", project);
            Information("AssemblyVersion: {0}", assemblyVersion);
            Information("AssemblyFileVersion: {0}", assemblyFileVersion);
            Information("AssemblyInformationalVersion/NuGet package version: {0}", assemblyInformationalVersion);
            Information("Package release notes URL: {0}{1}", packageReleaseNotesUrl, Environment.NewLine);

            perProjectMsBuildSettings[project] = new DotNetCoreMSBuildSettings { NoLogo = true }
                .WithProperty("AssemblyVersion", assemblyVersion)
                .WithProperty("FileVersion", assemblyFileVersion)
                .WithProperty("InformationalVersion", assemblyInformationalVersion)
                .WithProperty("Version", assemblyInformationalVersion)
                .WithProperty("PackageReleaseNotes", packageReleaseNotesUrl);
        }
    });

Task("Clean")
    .Does(() =>
    {
        var deleteDirectorySettings = new DeleteDirectorySettings
        {
            Recursive = true,
            Force = true
        };
        if (DirectoryExists(artifactsDir))
            DeleteDirectory(artifactsDir, deleteDirectorySettings);

        var toBuildFolders = toBuildFileInfos.Select(x => x.Directory.FullName).Distinct();
        var cleanSettings = new DotNetCoreCleanSettings
        {
            MSBuildSettings = new DotNetCoreMSBuildSettings { NoLogo = true },
            Verbosity = buildVerbosity
        };
        foreach (var folder in toBuildFolders)
        {
            Information(folder);
            DotNetCoreClean(folder, cleanSettings);

            var binFolder = System.IO.Path.Combine(folder, "bin");
            if (DirectoryExists(binFolder))
                DeleteDirectory(binFolder, deleteDirectorySettings);
            var objFolder = System.IO.Path.Combine(folder, "obj");
            if (DirectoryExists(objFolder))
                DeleteDirectory(objFolder, deleteDirectorySettings);
        }
    });

Task("Build")
    .IsDependentOn("MSBuildSettings")
    .Does(() =>
    {
        foreach (var projectToBuild in toBuildFiles)
        {
            var buildSettings = new DotNetCoreBuildSettings
            {
                MSBuildSettings = perProjectMsBuildSettings[projectToBuild],
                Configuration = buildConfiguration,
                Verbosity = buildVerbosity
            };
            DotNetCoreBuild(projectToBuild, buildSettings);
        }
    });

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach (var projectToTest in toTestFiles)
        {
            var testSettings = new DotNetCoreTestSettings
            {
                NoBuild = true,
                Configuration = buildConfiguration,
                Verbosity = buildVerbosity
            };
            DotNetCoreTest(projectToTest, testSettings);
        }
    });

RunTarget(target);