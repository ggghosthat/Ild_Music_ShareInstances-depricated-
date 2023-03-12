using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record TrackV2(string pathway, string name, string description) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Pathway {get; set;} = pathway;
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;
    public string AvatarBase64 {get; set;}

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
    public void SetAvatar(string path)
    {
        if(File.Exists(path))
        {}
    }
    #endregion
}