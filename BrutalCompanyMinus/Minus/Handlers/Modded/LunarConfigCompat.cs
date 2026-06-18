using System;
using System.Collections.Generic;
using HarmonyLib;

namespace BrutalCompanyMinus.Minus.Handlers.Modded;

/// <summary>
/// Applies current day enemy pool after DawnLib/LunarConfig changes the moon enemy pools
/// </summary>
[HarmonyPatch]
internal class LunarConfigCompat
{
    private static readonly List<EnemyPool> enemyPools = [];
    private static bool applying;

    [HarmonyPatch(typeof(Manager), "DoAddEnemyToPoolWithRarity")]
    [HarmonyPostfix]
    private static void DoAddEnemyToPoolWithRarityPostfix(ref List<SpawnableEnemyWithRarity> list, EnemyType enemy, int rarity)
    {
        RoundManager roundManager = RoundManager.Instance;

        if (applying || !Compatibility.DawnLibPresent || roundManager == null || roundManager.currentLevel == null || list == null || enemy == null || enemy.enemyPrefab == null)
            return;

        SelectableLevel currentLevel = roundManager.currentLevel;
        EnemyPoolList poolList;
        if (ReferenceEquals(list, currentLevel.Enemies))
        {
            poolList = EnemyPoolList.Inside;
        }
        else if (ReferenceEquals(list, currentLevel.OutsideEnemies))
        {
            poolList = EnemyPoolList.Outside;
        }
        else if (ReferenceEquals(list, currentLevel.DaytimeEnemies))
        {
            poolList = EnemyPoolList.Daytime;
        }
        else
        {
            return;
        }

        enemyPools.Add(new EnemyPool(EnemyPoolType.Add, poolList, enemy, rarity, string.Empty));
    }

    [HarmonyPatch(typeof(Manager), "DoRemoveSpawn")]
    [HarmonyPostfix]
    private static void DoRemoveSpawnPostfix(string Name)
    {
        RoundManager roundManager = RoundManager.Instance;

        if (applying || !Compatibility.DawnLibPresent || roundManager == null || roundManager.currentLevel == null || string.IsNullOrWhiteSpace(Name))
            return;

        enemyPools.Add(new EnemyPool(EnemyPoolType.Remove, EnemyPoolList.Inside, null!, 0, Name));
    }

    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.RefreshEnemiesList))]
    [HarmonyPostfix]
    private static void RefreshEnemiesListPostfix()
    {
        RoundManager roundManager = RoundManager.Instance;

        if (!Compatibility.DawnLibPresent || roundManager == null || !roundManager.IsHost || roundManager.currentLevel == null || enemyPools.Count == 0)
            return;

        SelectableLevel currentLevel = roundManager.currentLevel;

        try
        {
            applying = true;

            foreach (EnemyPool enemyPool in enemyPools)
            {
                if (enemyPool.PoolType == EnemyPoolType.Remove)
                {
                    Manager.RemoveSpawn(enemyPool.EnemyName);
                    continue;
                }

                List<SpawnableEnemyWithRarity> target = enemyPool.PoolList switch
                {
                    EnemyPoolList.Inside => currentLevel.Enemies,
                    EnemyPoolList.Outside => currentLevel.OutsideEnemies,
                    EnemyPoolList.Daytime => currentLevel.DaytimeEnemies,
                    _ => throw new ArgumentOutOfRangeException(nameof(enemyPool.PoolList), enemyPool.PoolList, null),
                };
                Manager.AddEnemyToPoolWithRarity(ref target, enemyPool.EnemyType, enemyPool.Rarity);
            }
        }
        catch (Exception exception)
        {
            Log.LogWarning("Failed to apply enemy pool changes after LunarConfig/DawnLib: " + exception);
        }
        finally
        {
            applying = false;
        }
    }

    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.OnLocalDisconnect))]
    [HarmonyPostfix]
    private static void CleanupPostfix()
    {
        enemyPools.Clear();
    }

    private enum EnemyPoolType
    {
        Add,
        Remove
    }

    private enum EnemyPoolList
    {
        Inside,
        Outside,
        Daytime
    }

    private readonly struct EnemyPool
    {
        internal readonly EnemyPoolType PoolType;
        internal readonly EnemyPoolList PoolList;
        internal readonly EnemyType EnemyType;
        internal readonly int Rarity;
        internal readonly string EnemyName;

        internal EnemyPool(EnemyPoolType poolType, EnemyPoolList poolList, EnemyType enemyType, int rarity, string enemyName)
        {
            PoolType = poolType;
            PoolList = poolList;
            EnemyType = enemyType;
            Rarity = rarity;
            EnemyName = enemyName;
        }
    }
}
