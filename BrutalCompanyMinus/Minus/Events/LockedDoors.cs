using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using DunGen;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class LockedDoors : MEvent
    {
        public override string Name() => nameof(LockedDoors);

        public static LockedDoors Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "It's like a dungeon!", "You need to unlock every door", "Let's hope the keys spawn" };
            ColorHex = "#8B008B";
            Type = EventType.Insane;
            isSpecialEvent = true;
            isBetaEvent = true;

            //ScaleList.Add(ScaleType.ScrapValue, new Scale(1.35f, 0.0115f, 1.35f, 2.5f));
            ScaleList.Add(ScaleType.ScrapAmount, new Scale(2.35f, 0.0115f, 2.35f, 3.5f));

        }

        //public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");
        public override bool AddEventIfOnly() => (Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Beta Events?"));

        public override void Execute()
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);

            if (randomValue <= 0.93f)
            {
                //Manager.scrapValueMultiplier *= Getf(ScaleType.ScrapValue);  
                Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);

                scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(1.0f, 1.0f, 1.0f, 1.0f),
                new SpawnableItemWithRarity(Assets.GetItem("Key"), 100)
                );

                scrapTransmutationEvent.Execute();
            }

            Active = true;
        }

        public override void OnShipLeave() => Active = false;

        public override void OnGameStart() => Active = false;
    } 
}
