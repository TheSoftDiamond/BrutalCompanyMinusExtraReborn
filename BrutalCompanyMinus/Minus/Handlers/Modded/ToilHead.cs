using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using System.Collections;
using BepInEx.Bootstrap;
using System.Xml.Linq;
using UnityEngine.Bindings;
using BrutalCompanyMinus.Minus.Events;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class ToilHeadHandler
    {
        private static bool isToilHeadPresent()
        {
            return Chainloader.PluginInfos.ContainsKey("com.github.zehsteam.ToilHead");
        }

        public static void AttemptToDisableToilHeadTurrets(ref System.Random rng)
        {
            var turretType = Type.GetType("com.github.zehsteam.ToilHead.MonoBehaviours.ToilHeadTurretBehaviour, ToilHead");
            if (turretType == null) return;
            var turrets = GameObject.FindObjectsOfType(turretType);
            foreach (var turret in turrets)
            {
                if (Convert.ToBoolean(rng.Next(2)))
                    RoundManager.Instance.StartCoroutine(DisableToilHeadTurret(turret));
            }
        }

        public static IEnumerator DisableToilHeadTurret(object toilheadTurretobj)
        {
            var turretType = Type.GetType("com.github.zehsteam.ToilHead.MonoBehaviours.ToilHeadTurretBehaviour");
            if (turretType == null) yield break;

            var toggleMethod = turretType.GetMethod("ToggleTurretEnabled", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (toggleMethod == null) yield break;

            toggleMethod.Invoke(toilheadTurretobj, new object[] { false });
            yield return new WaitForSeconds(7.0f);
            toggleMethod.Invoke(toilheadTurretobj, new object[] { true });
        }

        public static void AttemptToRageToilHeadTurrets(ref System.Random rng)
        {
            var turretType = Type.GetType("com.github.zehsteam.ToilHead.MonoBehaviours.ToilHeadTurretBehaviour, ToilHead");
            if (turretType == null) return;
            var turrets = GameObject.FindObjectsOfType(turretType);

            foreach (var turretObj in turrets)
            {
                if (rng.NextDouble() <= FacilityGhost.rageTurretsChance)
                {
                    dynamic turret = turretObj;

                    if (turret.turretMode == TurretMode.Berserk || turret.turretMode == TurretMode.Firing || !turret.turretActive) continue;

                    turret.turretMode = TurretMode.Berserk;
                    turret.EnterBerserkModeServerRpc();
                }
            }
        }
    }
}
