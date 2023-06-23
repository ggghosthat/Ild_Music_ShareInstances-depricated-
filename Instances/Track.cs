using System;
using System.IO;
using System.Collections.Generic;

namespace ShareInstances.Instances;
public struct Track
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Pathway {get; set;} = string.Empty.AsMemory();
	public ReadOnlyMemory<char> Name {get; set;} = string.Empty.AsMemory();
	public ReadOnlyMemory<char> Description {get; set;} = string.Empty.AsMemory();
    public ReadOnlyMemory<byte> AvatarSource {get; set;} = new byte[0];

    public bool IsValid {get; private set;} = false;
	public TimeSpan Duration {get; set; } = TimeSpan.FromSeconds(0);

    #region Const
    public Track(ReadOnlyMemory<char> pathway,
                 ReadOnlyMemory<char> name,
                 ReadOnlyMemory<char> description,
                 ReadOnlyMemory<byte> avatarSource,
                 TimeSpan duration)
    {
        if(System.IO.File.Exists(pathway.ToString()))
        { 
            Pathway = pathway;
            Name = name;
            Description = description;
            AvatarSource = avatarSource;      
            Duration = duration;
            IsValid = true;
        }
    }
    #endregion

    #region Avatar Manipulation
    public byte[]? GetAvatar()
    {
        try
        {
            return AvatarSource.ToArray();
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    //here is a bottleneck, please do not still use this method
    public void SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                AvatarSource = file; 
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;   
            }            
        }
    }
    #endregion
}
