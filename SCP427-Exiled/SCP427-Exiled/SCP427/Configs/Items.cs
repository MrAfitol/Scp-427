using System.Collections.Generic;
using System.ComponentModel;
using SCP427.Items;

namespace SCP427.Configs
{
    public class Items
    {
        [Description("The list of Scp427s.")]
        public List<Scp> Scp427s { get; private set; } = new List<Scp>
        {
            new Scp { Type = ItemType.Coin },
        };
    }
}
