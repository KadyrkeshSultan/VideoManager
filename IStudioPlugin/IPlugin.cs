using System;

namespace IStudioPlugin
{
    public interface IPlugin
    {
        string PluginName { get; }

        string Version { get; }

        string Company { get; }

        void SetAccount(Guid Id);

        void Init();
    }
}
