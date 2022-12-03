job(".NET Core GPNA.WebApiTemplate build,test and publish"){
    container(image = "mcr.microsoft.com/dotnet/sdk:5.0"){
        env["FEED_URL"] = "https://nuget.pkg.jetbrains.space/gpna/p/gpna-projecttemplates/nuget/v3/index.json"
        shellScript {
            content = """
                echo RUN BULD...
                dotnet build  
                echo PUBLISH NUGET PACKAGE...
                chmod +x publish.sh
                ./publish.sh
            """
        }
    }
}
