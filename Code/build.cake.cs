#addin nuget:?package=Cake.Scripty
#tool nuget:?package=Scripty

var target = Argument("target", "default");

////
//Scripty
////

Task("scripty-run-task")
.Does(() => {
    Scripty(@"PathToProject.csproj")
        .Evaluate(@"PathToScriptyFile.csx");
});

////
//Default
////

Task("default")
.IsDependentOn("scripty-run-task")

RunTarget(target);