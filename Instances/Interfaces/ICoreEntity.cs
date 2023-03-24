using System;

namespace ShareInstances.Instances.Interfaces
{
    public interface ICoreEntity
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarBase64 {get; set;}
        public byte[]? GetAvatar();
        public void DefineAvatar(string path);
        public string SetAvatar(string path);
    }
}