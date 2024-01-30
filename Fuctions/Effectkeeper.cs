using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions
{
    internal class Effectkeeper
    {
        public static void OnEscaping(EscapingEventArgs ev)
        {
            var effects = new List<Effect>();
            foreach (var effect in ev.Player.ActiveEffects)
            {
                if(effect.GetEffectType() == Exiled.API.Enums.EffectType.MovementBoost)
                {
                    continue;
                }
                effects.Add(new Effect(effect.GetEffectType(), effect.Duration));
            }
            Timing.RunCoroutine(CoroutineEffect(ev.Player, effects));
        }
        private static IEnumerator<float> CoroutineEffect(Player player, List<Effect> effects)
        {
            yield return Timing.WaitForSeconds(2);

            player.EnableEffects(effects);
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
        }
    }
}
