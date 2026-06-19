using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    public class GrabbableTurret : GrabbableObject
    {
        public Transform turretTransform = null!;

        private static int seed = 0;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Turret), nameof(Turret.Start))]
        private static void onTurretStart(Turret __instance)
        {
            if (Events.GrabbableTurrets.Active)
            {
                GameObject terminalObjectRoot = __instance.transform.parent != null ? __instance.transform.parent.gameObject : __instance.gameObject;
                DisableTerminalAccessibleObjects(terminalObjectRoot);

                if (!RoundManager.Instance.IsHost) return;

                __instance.StartCoroutine(DestroySelfAndReplace(__instance));
            }
            else
            {
                if (!RoundManager.Instance.IsHost) return;

                seed++;
                System.Random rng = new(seed);
                __instance.StartCoroutine(syncTerminalCodeWhenReady(__instance, rng));
            }

            static IEnumerator DestroySelfAndReplace(Turret turret)
            {
                MEvent _event = Events.GrabbableTurrets.Instance;

                float rarity = _event.Getf(MEvent.ScaleType.Rarity);

                seed++;
                System.Random rng = new(StartOfRound.Instance.randomMapSeed + seed);
                if (rng.NextDouble() <= rarity)
                {
                    GameObject grabbableTurret = Instantiate(Assets.grabbableTurret.spawnPrefab, RoundManager.Instance.GetRandomNavMeshPositionInBoxPredictable(turret.transform.position, 30.0f, RoundManager.Instance.navHit, rng), turret.transform.rotation);
                    NetworkObject netObject = grabbableTurret.GetComponent<NetworkObject>();
                    netObject.Spawn();
                    Net.Instance.GenerateAndSyncTerminalCodeServerRpc(netObject, rng.Next(RoundManager.Instance.possibleCodesForBigDoors.Length));

                    int scrapValue = UnityEngine.Random.Range(Assets.grabbableTurret.minValue, Assets.grabbableTurret.maxValue + 1);
                    int multipliedScrapValue = Mathf.RoundToInt(scrapValue * RoundManager.Instance.scrapValueMultiplier * Manager.scrapValueMultiplier);
                    Net.Instance.SyncScrapValueServerRpc(netObject, multipliedScrapValue);

                    yield return new WaitForSeconds(5.0f);

                    NetworkObject vanillaNetObject = turret.transform.parent != null ? turret.transform.parent.GetComponent<NetworkObject>() : turret.NetworkObject;
                    if (vanillaNetObject != null && vanillaNetObject.IsSpawned)
                    {
                        vanillaNetObject.Despawn(destroy: true);
                    }
                }
            }
        }

        private static void DisableTerminalAccessibleObjects(GameObject rootObject)
        {
            if (rootObject == null)
            {
                return;
            }

            foreach (TerminalAccessibleObject terminalAccessibleObject in rootObject.GetComponentsInChildren<TerminalAccessibleObject>(true))
            {
                if (terminalAccessibleObject == null)
                {
                    continue;
                }

                // Stops the bugged vanilla update after round ends
                terminalAccessibleObject.enabled = false;
            }
        }

        private static IEnumerator syncTerminalCodeWhenReady(Turret turret, System.Random rng)
        {
            float waitTime = 0f;
            while (turret != null && (turret.NetworkObject == null || !turret.NetworkObject.IsSpawned) && waitTime < 2f)
            {
                waitTime += Time.deltaTime;
                yield return null;
            }

            if (turret == null || turret.NetworkObject == null || !turret.NetworkObject.IsSpawned)
            {
                yield break;
            }

            Net.Instance.GenerateAndSyncTerminalCodeServerRpc(turret.NetworkObject, rng.Next(RoundManager.Instance.possibleCodesForBigDoors.Length));
        }

        public override void Start()
        {
            base.Start();
            TerminalAccessibleObject terminalAccessibleObject = GetComponentInChildren<TerminalAccessibleObject>();
            if (terminalAccessibleObject != null)
            {
                // Should be done on the prefab if this isn't
                terminalAccessibleObject.terminalCodeEvent = new InteractEvent();
                terminalAccessibleObject.terminalCodeEvent.AddListener(DisableTurretFromTerminal);
                terminalAccessibleObject.terminalCodeCooldownEvent = new InteractEvent();
                terminalAccessibleObject.terminalCodeCooldownEvent.AddListener(EnableTurretFromTerminal);
            }
            DisableTerminalAccessibleObjects(gameObject);
            if (IsServer)
            {
                DisableTerminalAccessibleObjectsClientRpc();
            }
            StartCoroutine(UpdateTransform(11.0f, new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));

            IEnumerator UpdateTransform(float time, Vector3 rotation)
            {
                yield return new WaitForSeconds(time);
                if (RoundManager.Instance.IsHost) syncRotationServerRpc(rotation);
            }
        }

        private void DisableTurretFromTerminal(PlayerControllerB playerWhoTriggered)
        {
            Turret turret = GetComponent<Turret>();
            turret?.ToggleTurretEnabled(false);
        }

        private void EnableTurretFromTerminal(PlayerControllerB playerWhoTriggered)
        {
            Turret turret = GetComponent<Turret>();
            turret?.ToggleTurretEnabled(true);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void DisableTerminalAccessibleObjectsClientRpc()
        {
            DisableTerminalAccessibleObjects(gameObject);
        }

        public override void OnDestroy()
        {
            DisableTerminalAccessibleObjects(gameObject);
            base.OnDestroy();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void syncRotationServerRpc(Vector3 eulerAngle) => syncRotationClientRpc(eulerAngle);

        [Rpc(SendTo.ClientsAndHost)]
        private void syncRotationClientRpc(Vector3 eulerAngle) => turretTransform.eulerAngles += eulerAngle;
    }
}
