using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(Turret))]
    internal class BerserkTurrets
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void TurretsHellMode(Turret __instance)
        {
            if (Events.BerserkTurrets.Instance.Active)
            {
                __instance.turretMode = TurretMode.Berserk;
            }
        }
    }
}
