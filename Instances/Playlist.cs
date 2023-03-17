using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;


namespace ShareInstances.Instances;
public record Playlist(string name, string description, IList<Guid> tracks = null) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;
    public char[]? AvatarBase64 {get; set;}      

	public IList<Guid> Tracks = new List<Guid>(tracks);
    public int Count => Tracks.Count;
	public Guid Head {get; private set;}
	public Guid Tail {get; private set;}

	public int CurrentIndex {get; set;}
	public bool IsOrdered { get; private set; } = false;

    #region Collection Manipulation Methods
    public void AddTrack(Track track)
    {        
    	Tracks.Add(track.Id);
    }

    public void RemoveTrack(Track track)
    {        
    	if(Tracks.Contains(track.Id))
    	{
    		Tracks.Remove(track.Id);
    	}
    }
    #endregion

    #region Shuffle
    public void Shuffle() 
    {
        IList<Guid> shuffledList = Tracks.OrderBy(i => Guid.NewGuid()).ToList();
        Tracks = shuffledList;
    }
    #endregion

    #region Avatar Manipulation
    public byte[] GetAvatar()
    {
        try
        {
            byte[] result;
            return Convert.FromBase64CharArray(AvatarBase64, 0, AvatarBase64.Length);
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    public async void SetAvatar(string path)
    {
        if(File.Exists(path))
        {
            try
            {
                byte[]? fileBytes;
                using (FileStream fileStream = File.Open(path, FileMode.Open))
                {
                    fileBytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(fileBytes, 0, (int)fileStream.Length);
                }
                if (fileBytes != null)
                {
                    AvatarBase64 = null;
                    AvatarBase64 = new char[fileBytes.Length];
                    Convert.ToBase64CharArray(fileBytes, 0, fileBytes.Length, AvatarBase64, 0);
                }
                fileBytes = null;
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                return;
            }
        }
    }
    #endregion

    public IList<Track> GetTracks()
    {
        return null;
    }
}