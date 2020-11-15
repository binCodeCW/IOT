using System;
using System.Collections.Generic;
using SettingsProviderNet;

namespace WHC.MVCWebMis.Controllers
{
    internal class DatabaseStorage : ISettingsStorage
    {
        private string creator;

        public DatabaseStorage(string creator)
        {
            this.creator = creator;
        }

        public Dictionary<string, string> Load(string key)
        {
            throw new NotImplementedException();
        }

        public void Save(string key, Dictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }
    }
}