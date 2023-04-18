using ShareInstances.Instances.Interfaces;
using ShareInstances.Filer;
using ShareInstances.Services.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
namespace ShareInstances.Instances;
public static class Extesions
{
    public static T ToEntity<T>(this Guid guid, IList<T> store) where T : ICoreEntity
    {
        return store.ToList().FirstOrDefault(i => guid.Equals(i.Id));
    }

	public static List<T> ToEntity<T>(this IList<Guid> guids, IList<T> store, bool full_return = false ) where T : ICoreEntity
	{
        if (!full_return)
    		return store.ToList()
                        .FindAll(delegate(T item) { return guids.Contains(item.Id); });
        else
        {
            return store.ToList();   
        }
	}

    public static Track MusicFileConvertTrack(this MusicFile musicFile)
    {
        string picture64Base = null;
        if (musicFile.Picture is not null)
        {
            picture64Base = Convert.ToBase64String(musicFile.Picture);
        }
        return new Track(musicFile.FilePath,
                     musicFile.FileName,
                     "",
                     picture64Base);
    }


    //Here supporter null value for "temporary playing".
    //And we need pass supporter when we wanna save temporary instances (MusicFile).
    public static Playlist MusicFileConvertPlaylist(this IEnumerable<MusicFile> musicFiles,
                                                         SupporterService supporter = null)
    {
        //playlist initialization
        var playlist = new Playlist(name:DateTime.Now.ToString(), 
                                    description:"");

        //tracks initialization & playlist population
        musicFiles.ToList().ForEach(mf => 
        {
            var track = mf.MusicFileConvertTrack();
            if (supporter is not null) supporter.AddInstance(track);
            playlist.AddTemp(track);
        });   

        return playlist;
    }
}