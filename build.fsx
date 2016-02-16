// include Fake lib
#r @"packages\FAKE.4.20.0\tools\FakeLib.dll"
open Fake
open Fake.Testing
open Fake.AssemblyInfoFile

RestorePackages()

// Properties
let buildDir = "./build/"
let testDir  = "./testOutput/"
let deployDir = "./deploy/"

// version info
let version = "0.0.1.10"  // or retrieve from CI server

let runTests () =
    tracefn "Running tests..."
    !! (testDir @@ "*.Tests.dll")
       |> xUnit2 (fun p -> { p with HtmlOutputPath = Some (testDir @@ "xunit.html") })

Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; deployDir]
)

Target "BuildApp" (fun _ ->
    UpdateAttributes "./src/app/XmlConvert/Properties/AssemblyInfo.cs"
        [Attribute.Title "ConvertXml"
         Attribute.Description "Tool for convert from a .Net object to XML"
         Attribute.Guid "A539B42C-CB9F-4a23-8E57-AF4E7CEE5BAA"
         Attribute.Product "ConvertXml"]

    UpdateAttributes "./src/test/XmlConvert.Tests/Properties/AssemblyInfo.cs"
        [Attribute.Title "ConvertXml Unit Tests"
         Attribute.Description "Unit Tests of the tool for convert from a .Net object to XML"
         Attribute.Guid "A539B42C-CB9F-4a23-8E57-AF4E7CEE5BAA"
         Attribute.Product "ConvertXml Tests"]

    !! "src/**/*.csproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  !! "src/test/**/*.csproj"
    |> MSBuildDebug testDir "Build"
    |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
    runTests()
)

Target "FxCop" (fun () ->  
    !! (buildDir + @"\**\XmlConvert*.dll") 
    ++ (buildDir + @"\**\*.exe") 
    |> FxCop 
        (fun p -> 
            {p with 
              // override default parameters
              ReportFileName = testDir + "FXCopResults.xml"
              ToolPath = "./tools/FxCop/FxCopCmd.exe"})
)

let fullDir = System.IO.Path.GetFullPath "src/test/XmlConvert.Tests/bin/Debug"
//let fullDir = System.IO.Path.GetFullPath "src/test/"
Target "Watch" (fun _ ->
    use watcher = !! fullDir |> WatchChanges (fun changes -> 
        runTests()
    )
    System.Console.ReadLine() |> ignore 
    watcher.Dispose()
)

Target "Zip" (fun _ ->
    System.Console.WriteLine(deployDir + "XmlConvert." + version + ".zip")
    CreateDir deployDir
    !! (buildDir + "/**/*.*")
        -- "*.zip"
            |> Zip buildDir (deployDir + "XmlConvert." + version + ".zip")
)


//Dependencies
"Clean" 
  ==> "BuildApp"
  ==> "FxCop"
  ==> "BuildTest"
  ==> "Test"
  ==> "Zip"
  //==> "Watch"
  

// start build
RunTargetOrDefault "Zip"