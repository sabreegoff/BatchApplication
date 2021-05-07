# BatchApplication

**Running application on Windows or mac**:
 -If you have are running on windows and have visual studio installed, you should be able to pull the code and run it.
 -If not, you can install VSCode at: https://code.visualstudio.com/download
 -After you've installed VSCode, download the .NET Core SDK: https://dotnet.microsoft.com/download
    -To be sure its installed, run "dotnet" in your cmd window. 
 -Once you have verified it is installed, use the terminal to run: dotnet run
 -You should get a prompt to enter a mixtape file (drag and drop is fine)
 -You should then get a prompt to enter a changes file (drag and drop is fine)
 -An output.json file should appear in your BatchApplication folder, I also pretty printed the json. 
 -I also added the .exe file to the github repo but I understand if you dont want to run that.
 
** Handling Extra large Files**
Under the circumstances the input or changes files happen to be extra large, the code would need to be modified. We could first make the Mixtape.Read() and the ChangeList.ReadChanges() methods async. We would need to await an use JsonSerializer.DeserializeAsync. Same goes for Serialization, we could also make that asyncronous. This way, the program wouldn't have to wait for the input file to deserialize before starting on the changes file. A better way would be to write the code in a way that can ingest the changes file in a way that could make the changes concurrently.  

 
 
 
