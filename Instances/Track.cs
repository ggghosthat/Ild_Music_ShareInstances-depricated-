using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record Track(string pathway, string name, string description) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Pathway {get; set;} = pathway;
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;
    public char[]? AvatarBase64 {get; set;}

	public TimeSpan Duration => ExtraxtDuration();

    
    #region Extraction Methods
    private TimeSpan ExtraxtDuration()
    {
    	if (File.Exists(pathway))
        {
            using( var taglib = TagLib.File.Create(pathway))
            return taglib.Properties.Duration;
        }
        return TimeSpan.FromSeconds(1);
    }
    #endregion

    #region Avatar Manipulation
    public byte[]? GetAvatar()
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
}