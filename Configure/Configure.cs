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
    public IEnumerable<Memory<char>> Players {get; set;}
    public IEnumerable<Memory<char>> Synches {get; set;}
}

public class Configure:IConfigure
{
    public Memory<char> ComponentsFile {get; init;}

    public Config ConfigSheet {get; set;}

    public Configure()
    {}

    public Configure(Memory<char> componentsFile)
    {
        ComponentsFile = componentsFile;
        ParseAsync().Wait();
    }

    public void Parse()
    {        
        using FileStream openStream = File.OpenRead(ComponentsFile.ToString());
        ConfigSheet = JsonSerializer.Deserialize<Config>(openStream);
    }

    public async Task ParseAsync()
    {
        using FileStream openStream = File.OpenRead(ComponentsFile.ToString());
        ConfigSheet = await JsonSerializer.DeserializeAsync<Config>(openStream);
    }
}
