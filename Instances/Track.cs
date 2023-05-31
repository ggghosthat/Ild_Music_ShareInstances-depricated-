using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public struct Track
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Pathway {get; private set;}
	public ReadOnlyMemory<char> Name {get; private set;}
	public ReadOnlyMemory<char> Description {get; set;}
    public ReadOnlyMemory<char> AvatarBase64 {get; private set;}

	public TimeSpan Duration {get; private set; }

    #region Const
    public Track(ReadOnlyMemory<char> pathway,
                 ReadOnlyMemory<char> name,
                 ReadOnlyMemory<char> description,
                 ReadOnlyMemory<char> avatar)
    {
        ExtractData(pathway, name, avatar);
        Description = description;
    }
    #endregion


    #region Extraction Methods
    private void ExtractData(ReadOnlyMemory<char> pathway, 
                             ReadOnlyMemory<char> name,
                             ReadOnlyMemory<char> avatar)
    {
        if(File.Exists(pathway.ToString()))
        {
            Pathway = pathway;
            using( var taglib = TagLib.File.Create(pathway.ToString()))
            {
                if(name.Length == 0)
                    Name = (taglib.Tag.Title ?? "Unknown").AsMemory();
                else
                    Name = name;

                Duration = taglib.Properties.Duration;

                if (string.IsNullOrEmpty(avatar.ToString()))
                {
                    if(taglib.tag.Pictures.Length > 0)
                        AvatarBase64 = Convert.ToBase64String(taglib.Tag.Pictures[0].Data.Data);
                    else AvatarBase64 = avatar;
                }
                else AvatarBase64 = avatar;
            }
        }

    }

    #endregion

    #region Avatar Manipulation
    public byte[]? GetAvatar()
    {
        try
        {
            return Convert.FromBase64String(AvatarBase64);
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
                return result = Convert.ToBase64String(file); 
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
