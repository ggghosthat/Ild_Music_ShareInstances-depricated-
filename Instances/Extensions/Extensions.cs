using ShareInstances.Instances.Interfaces;

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
}