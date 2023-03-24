using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record Track : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Pathway {get; set;}
	public string Name {get; set;}
	public string Description {get; set;}
    public string AvatarBase64 {get; set;}

	public TimeSpan Duration => ExtraxtDuration();

    #region Const
    public Track(string pathway,
                 string name,
                 string description,
                 string avatar = null)
    {
        Pathway = pathway;
        Name = name;
        Description = description;
        AvatarBase64 = SetAvatar(avatar);
    }
    #endregion


    #region Extraction Methods
    private TimeSpan ExtraxtDuration()
    {
    	if (File.Exists(Pathway))
        {
            using( var taglib = TagLib.File.Create(Pathway))
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
            return Convert.FromBase64String(AvatarBase64);
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    public void DefineAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                string result = Convert.ToBase64String(file); 
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;
            }
        }
    }

    public string SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                string result = Convert.ToBase64String(file); 
                return result;
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