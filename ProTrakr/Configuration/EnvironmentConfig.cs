using System;
namespace ProTrakr.Configuration
{

    public class EnvironmentConfig : IEnvironmentConfig
    {
        private static IEnvironmentConfig _instance;
        public static IEnvironmentConfig Instance => _instance ?? (_instance = new EnvironmentConfig());

        public string ServerAddress => "192.168.86.183";
        public Uri AuthURL => new Uri($"http://{ServerAddress}:9080");
        public Uri ServerURL => new Uri($"realm://{ServerAddress}:9080/~/default");
    }
}
