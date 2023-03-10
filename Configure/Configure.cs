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
        Parse();
    }

    public void Parse(){}

    public async Task ParseAsync()
    {
        using FileStream openStream = File.OpenRead(ComponentsFile);
        ConfigSheet = await JsonSerializer.DeserializeAsync<Config>(openStream);
    }
}