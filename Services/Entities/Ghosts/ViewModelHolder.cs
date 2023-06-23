using ShareInstances.Services.Interfaces;

using System;
using System.Linq;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public class ViewModelHolder<T> : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "HolderService".AsMemory();

    private static IDictionary<Memory<char>, T> holder = new Dictionary<Memory<char>, T>();

    public ViewModelHolder()
    {}

    public static void Add(Memory<char> name,T viewModel) =>
        holder[name] = viewModel;
    
    public static T Get(Memory<char> name) =>
        (holder.ContainsKey(name))?holder[name]:default(T);

    public static T Peek() =>
        holder.ToList().Last().Value;

    public static void Clear() =>
        holder.Clear();
}
