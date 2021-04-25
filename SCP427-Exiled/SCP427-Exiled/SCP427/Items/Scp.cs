using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.CustomItems.API.Spawn;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.ComponentModel;

namespace SCP427.Items
{
    public class Scp : CustomItem
    {
        public override uint Id { get; set; } = 12;

        public override string Name { get; set; } = "SCP-427";

        public override string Description { get; set; } = "Its Scp-427";

        private List<CoroutineHandle> Coroutines { get; } = new List<CoroutineHandle>();

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint
                {
                    Chance = 100,
                    Location = SpawnLocation.InsideHczArmory,
                },
            },
        };

        [Description("Delay in treatment at the first stage (in seconds)")]
        public float Deley { get; set; } = 0.5f;

        [Description("Delay in treatment at the second stage (in seconds)")]
        public float Deley2 { get; set; } = 1f;

        [Description("Delay of treatment at the third stage (in seconds)")]
        public float Deley3 { get; set; } = 1.5f;

        [Description("Delay of treatment at the fourth stage (in seconds)")]
        public float Deley4 { get; set; } = 0.5f;

        [Description("Delay of mutation (in seconds)")]
        public float Mutation { get; set; } = 30;

        [Description("SCP-427-1 Health Amount")]
        public int Scp427Health { get; set; } = 700;

        [Description("SCP-427-1 Armor Quantity")]
        public int Scp427AHealth { get; set; } = 100;

        [Description("Message displayed to the player when they release SCP-427")]
        public string TakeOffMessage { get; set; } = "You have removed the amulet";

        [Description("The message shown to the player during the first stage of SCP-427")]
        public string HealingMessage { get; set; } = "You feel all your wounds are healing";

        [Description("The message shown to the player during the second stage of SCP-427")]
        public string HealingMessage2 { get; set; } = "You feel the arrival of new forces";

        [Description("Message displayed to the player during SCP-427 mutation")]
        public string HealingMessage3 { get; set; } = "You feel a mutation in you";

        [Description("What effects should be given to the player when using SCP-427")]
        public List<string> Scp427Effects { get; set; } = new List<string>
        {
            "Burned",
        };

        public static List<int> ShPlayers = new List<int>();

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;

            base.UnsubscribeEvents();
        }

        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                ev.Player.ShowHint(TakeOffMessage);

            }

            base.OnDropping(ev);
        }

        public void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Check(ev.NewItem))
            {
                ev.Player.DisableAllEffects();

                if (ev.Player.ReferenceHub.TryGetComponent(out SCP008X.Scp008 scp008))
                {
                    UnityEngine.Object.Destroy(scp008);
                }

                UnityEngine.Object.Destroy(scp008);

                Coroutines.Add(Timing.RunCoroutine(Healing(ev), "Healing"));
            }
            else

            if (Check(ev.OldItem))
            {
                Timing.KillCoroutines("Healing");
                if (!string.IsNullOrEmpty(TakeOffMessage))
                    ev.Player.ShowHint(TakeOffMessage);

                ev.Player.ReferenceHub.playerStats.artificialHpDecay = 1f;

            }
        }

        public IEnumerator<float> Healing(ChangingItemEventArgs ev)
        {
            ev.Player.DisableAllEffects();
            while (ev.Player.Health < 30)
            {
                 ev.Player.ShowHint(HealingMessage);

                 yield return Timing.WaitForSeconds(Deley);

                 ev.Player.Health++;
            }

            while (ev.Player.Health >= 30 & ev.Player.Health < 80)
            {
                 ev.Player.ShowHint(HealingMessage2);

                 yield return Timing.WaitForSeconds(Deley2);

                 ev.Player.Health++;
            }

            if (ev.Player.Health >= 80 & ev.Player.Health < ev.Player.MaxHealth)
            {
                ev.Player.ShowHint(HealingMessage2);
                foreach (string effect in Scp427Effects)
                 {
                      ev.Player.ReferenceHub.playerEffectsController.EnableByString(effect, 999f, false);
                 }
                 while (ev.Player.Health >= 80 & ev.Player.Health < ev.Player.MaxHealth)
                 {
                       yield return Timing.WaitForSeconds(Deley3);

                       ev.Player.Health++;
                 }
            }

            if (ev.Player.Health == ev.Player.MaxHealth & ev.Player.ArtificialHealth < Mutation)
            {
                ev.Player.ShowHint(HealingMessage3);
                foreach (string effect in Scp427Effects)
                 {
                     ev.Player.ReferenceHub.playerEffectsController.EnableByString(effect, 999f, false);
                 }
                 while (ev.Player.Health == ev.Player.MaxHealth & ev.Player.ArtificialHealth < Mutation)
                 {
                     ev.Player.ReferenceHub.playerStats.artificialHpDecay = 0f;
                     yield return Timing.WaitForSeconds(Deley4);

                     ev.Player.ArtificialHealth++;
                 }
            }

            if (ev.Player.ArtificialHealth == Mutation)
            {
                foreach (string effect in Scp427Effects)
                {
                    ev.Player.ReferenceHub.playerEffectsController.ChangeByString(effect, 0);
                }

                ev.Player.SetRole(RoleType.Scp0492, true);
                ev.Player.MaxHealth = Scp427Health;
                ev.Player.Health = ev.Player.MaxHealth;
                ev.Player.MaxArtificialHealth = Scp427AHealth;
                ev.Player.ArtificialHealth = ev.Player.MaxArtificialHealth;
            }
        }
    }
}
