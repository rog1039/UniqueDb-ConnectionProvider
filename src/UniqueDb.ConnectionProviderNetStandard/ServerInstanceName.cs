using System;

namespace UniqueDb.ConnectionProvider
{
    public class ServerInstanceName
    {
        public string ServerName { get; set; }
        public string InstanceName { get; set; }

        private ServerInstanceName(string serverName, string instanceName)
        {
            ServerName = serverName;
            InstanceName = instanceName;
        }

        public static ServerInstanceName Parse(string name)
        {
            if (name.Contains("\\"))
            {
                var slashIndex = name.IndexOf("\\", StringComparison.Ordinal);
                var serverNameLength = slashIndex;
                var instanceNameLength = name.Length - (slashIndex + 1);
                var serverName = name.Substring(0, slashIndex);
                var instanceName = name.Substring(slashIndex + 1, instanceNameLength);
                return new ServerInstanceName(serverName, instanceName);
            }
            else
            {
                return new ServerInstanceName(name, string.Empty);
            }
        }
    }
}