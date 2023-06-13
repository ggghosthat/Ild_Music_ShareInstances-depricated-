using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ShareInstances.Filer;
//Filer is a special class wich perfome temporary file system loading
//Filer just read special files(music formats, such as .mp3)
public class Filer
{	
	private static ConcurrentBag<MusicFile> usicFiles = new();
    private static ConcurrentDictionary<string, MusicFile> MusicFiles = new ();

	public event Action OnMuted;	

    private static Memory<byte> buffer;

	public Filer()
	{}

    public static async Task<Memory<byte>> ReadFileAsync(string path)
    {
        if(File.Exists(path))
        {
            using(FileStream fs = File.OpenRead(path))
            {
                buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer);
            }
            return buffer;
        }
        
        throw new FileNotFoundException($"Could not find file: {path}");
    }

	public async Task BrowseFiles(IEnumerable<string> inputPaths)
	{
		Parallel.ForEach(inputPaths,
						 new ParallelOptions { MaxDegreeOfParallelism = 4 },
						 (string file) =>
        {            
			//containing file-format restriction.
    		//in the nearest release will be allow mp3 format only!!!
            if(File.Exists(file))
			{
				var ext = Path.GetExtension(file);
	    	    if (ext.Equals(".mp3"))
	        	{
	        		var mFile = new MusicFile(file, new FileInfo(file).Length);
		            MusicFiles.AddOrUpdate(mFile.FilePath, mFile, (string key, MusicFile oldValue) => mFile);
	       		}
	       	}
        });
	}

	public IList<MusicFile> GetMusicFiles()
	{
		return MusicFiles.Values.ToList();
	}

	public void CleanFiler()
	{
		MusicFiles.Clear();
	}
}

//MusicFile is a special instance which represent audio in file system.
//It mustn't be droped to player instance for playing.
//It just store necessary data to keep tracking audio file.
public record MusicFile(string filepath, long size)
{
	public DateTime Token {get; private set;} = DateTime.Now;
	public string FilePath {get; private set;} = filepath;
	public string FileName => ExtractTitle();
	public TimeSpan Duration => ExtractDuration();
	public byte[] Picture => ExtractPicture();
	public long Size {get; private set;}
	
	private string ExtractTitle()
	{
		using( var taglib = TagLib.File.Create(FilePath))
		{
        	return taglib.Tag.Title ?? "Unknown";
    	}
	}

	private byte[] ExtractPicture()
	{
		using( var taglib = TagLib.File.Create(FilePath))
		{
			if(taglib.Tag.Pictures.Length > 0)
			{
				return taglib.Tag.Pictures[0].Data.Data;
			}
			return null;
    	}
	}

	private TimeSpan ExtractDuration()
	{
		using( var taglib = TagLib.File.Create(FilePath))
		{
			return taglib.Properties.Duration;
    	}
	}
}
