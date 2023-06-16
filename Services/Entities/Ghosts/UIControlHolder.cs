using System;
using ShareInstances.Services.Interfaces;

namespace ShareInstances.Services.Entities;
public class UIControlHolder<T>
{
    public ReadOnlyMemory<char> ServiceName {get; init;} = "ControlsHolder".AsMemory();

    private int size;
    private T[] controls;

    public UIControlHolder(int size = 5)
    {
        this.size = size;
        controls = new T[size];
    }

    public void AddControl(T control)
    {
        var index = controls.Length - 1 ;            
        if (index < 0)
        {
            index = 0;
            controls[index] = control;
        }
    }

    public void Dispose() =>
        controls = default;   
}
