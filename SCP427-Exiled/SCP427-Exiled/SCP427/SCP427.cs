using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Server = Exiled.Events.Handlers.Server;

namespace SCP427
{
    using Events;
    using global::SCP427.Items;
    using MEC;

    public class SCP427 : Plugin<Config>
    {
        public override string Name => "SCP-427";

        public override string Author => "MrAfitol";

        public override Version Version => new Version(1, 0, 0);

        public override Version RequiredExiledVersion => new Version(2, 10, 0);

        private static readonly SCP427 InstanceValue = new SCP427();

        private ServerHandler serverHandler;

        private Scp scp;

        private SCP427()
        {
        }

        public static SCP427 Instance => InstanceValue;

        public override void OnEnabled()
        {
            serverHandler = new ServerHandler();
            scp = new Scp();

            Config.LoadItems();
            RegisterItems();

            Server.ReloadedConfigs += serverHandler.OnReloadingConfigs;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {

            try
            {
                Timing.KillCoroutines("Heal");
            }
            catch { }

            UnregisterItems();

            Server.ReloadedConfigs -= serverHandler.OnReloadingConfigs;

            serverHandler = null;

            base.OnDisabled();
        }

        private void RegisterItems()
        {
            Instance.Config.ItemConfigs.Scp427s?.Register();
        }
        private void UnregisterItems()
        {
            Instance.Config.ItemConfigs.Scp427s?.Unregister();
        }
    }
}
