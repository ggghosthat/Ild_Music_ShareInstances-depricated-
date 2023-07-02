using System;
using System.Collections.Generic;

namespace ShareInstances.Instances.Map;
public struct Store<T>
{
    public int Tag {get; set;}
    public ICollection<Pair> Pairs {get; set;} = new List<Pair>();

    public Store(int tag)
    {
        Tag = tag;
    }
}