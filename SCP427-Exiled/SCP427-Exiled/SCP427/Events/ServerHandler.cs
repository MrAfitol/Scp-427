

namespace SCP427.Events
{
    using static SCP427;

    public class ServerHandler
    {
        public void OnReloadingConfigs()
        {
            Instance.Config.LoadItems();
        }
    }
}
