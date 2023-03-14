using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShareInstances.Configure;
public record Config()
{
    public IEnumerable<string> Players {get; set;}
    public IEnumerable<string> Synches {get; set;}
}

public class Configure:IConfigure
{
    public string ComponentsFile {get; init;}

    public Config ConfigSheet {get; set;}

    public Configure()
    {}

    public Configure(string componentsFile)
    {
        ComponentsFile = componentsFile;
        ParseAsync().Wait();
    }

    public void Parse()
    {        
        string openStream = File.ReadAllText(ComponentsFile);
        ConfigSheet = JsonSerializer.Deserialize<Config>(openStream);
    }

    public async Task ParseAsync()
    {
        using FileStream openStream = File.OpenRead(ComponentsFile);
        ConfigSheet = JsonSerializer.Deserialize<Config>(openStream);
    }
}