using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public struct Track
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Pathway {get; private set;} = string.Empty.AsMemory();
	public ReadOnlyMemory<char> Name {get; private set;} = string.Empty.AsMemory();
	public ReadOnlyMemory<char> Description {get; set;} = string.Empty.AsMemory();
    public ReadOnlyMemory<char> AvatarBase64 {get; private set;} = string.Empty.AsMemory();

    public bool IsValid {get; private set;} = false;
	public TimeSpan Duration {get; set; } = TimeSpan.FromSeconds(0);

    #region Const
    public Track(ReadOnlyMemory<char> pathway,
                 ReadOnlyMemory<char> name,
                 ReadOnlyMemory<char> description,
                 ReadOnlyMemory<char> avatarPath)
    {
        if(System.IO.File.Exists(pathway.ToString()))
        { 
            Pathway = pathway;
            Name = name;
            Description = description;
            AvatarBase64 = avatarPath;           
            IsValid = true;
        }
    }
    #endregion

    #region Avatar Manipulation
    public byte[]? GetAvatar()
    {
        try
        {
            return Convert.FromBase64String(AvatarBase64.ToString());
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    
    public string SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(file); 
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;   
            }            
        }
        else return null;
    }
    #endregion
}
