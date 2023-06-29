using System;

namespace ShareInstances.Instances.Map;
public record TrackMap(byte[] Buid,
                       string Pathway,
                       string Name,
                       string Description,
                       byte[] Avatar,
                       bool IsValid,
                       TimeSpan Duration);