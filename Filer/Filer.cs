using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ShareInstances.Filer;
public class Filer
{
	public Filer()
	{}

	public ConcurrentBag<MusicFile> MusicFiles {get; private set;} = new();

	public async Task BrowseFiles(IEnumerable<string> inputPaths)
	{
		Parallel.ForEach(inputPaths, ReadFile);
	}

	//containing file-format restriction.
    //in the nearest release will be allow mp3 format only!!!
	private void ReadFile(string file)
	{
		if(File.Exists(file))
		{
			var ext = Path.GetExtension(file);
    	    if (ext.Equals("mp3"))
        	{
        		MusicFiles.Add(new MusicFile(file, new FileInfo(file).Length));
       		}
       	}
	}
}

public record MusicFile(string filepath, long size)
{
	public string FilePath {get; private set;} = filepath;
	public string FileName => ExtractTitle();
	public long Size {get; private set;}


	private string ExtractTitle()
	{
		using( var taglib = TagLib.File.Create(FilePath))
        return taglib.Tag.Title ?? "Unknown";
	}
}