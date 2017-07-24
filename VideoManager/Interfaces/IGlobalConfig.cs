using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    interface IGlobalConfig : IDisposable
    {
        string GetConfigValue(string Key);

        string GetConfigValue(string Key, string Default);

        GlobalConfig GetConfigRecord(string Key);

        List<GlobalConfig> GetConfigList();

        List<GlobalConfig> GetConfigListByDesc();

        void SaveUpdate(GlobalConfig rec);
    }
}
