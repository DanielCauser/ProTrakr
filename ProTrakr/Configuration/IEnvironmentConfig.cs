using System;
namespace ProTrakr.Configuration
{
    public interface IEnvironmentConfig
    {
        string ServerAddress { get; }
        Uri AuthURL { get; }
        Uri ServerURL { get; }
    }
}
