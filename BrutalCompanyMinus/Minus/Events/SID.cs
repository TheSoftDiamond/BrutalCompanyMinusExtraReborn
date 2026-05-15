using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SID : MEvent
    {
        public override string Name() => nameof(SID);

        public static SID Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Single Item Day", "Very Special Day" };
            ColorHex = "#00FF00";
            Type = EventType.VeryGood;

            EventsToRemove = new List<string>() { nameof(Clock), nameof(ControlPad), nameof(Dentures), nameof(Dustpans), nameof(FootballScrap), nameof(EasterEggs), nameof(GarbageLid), nameof(GoldenBars), nameof(GoldenFacility), nameof(Honk), nameof(HolidaySeason), nameof(MaskItem), nameof(SeveredBits), nameof(SussyPaintings), nameof(ToiletPaper), nameof(Train), nameof(ZedDog), nameof(CityOfGold), nameof(Bellcrab), nameof(Dice), nameof(BadDice), nameof(TakeyGokuBracken), nameof(TakeyGokuPlush), nameof(TakeyGokuPlushBig), nameof(TakeyPlush), nameof(Pickles) };
        }

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        public override void Execute()
        {
            Assets.ItemName[] scrapList = new Assets.ItemName[]
            {
                Assets.ItemName.LargeAxle, Assets.ItemName.V_TypeEngine, Assets.ItemName.PlasticFish,
                Assets.ItemName.MetalSheet, Assets.ItemName.LaserPointer, Assets.ItemName.BigBolt,
                Assets.ItemName.Bottles, Assets.ItemName.Ring, Assets.ItemName.SteeringWheel,
                Assets.ItemName.CookieMoldPan, Assets.ItemName.EggBeater, Assets.ItemName.JarOfPickles,
                Assets.ItemName.DustPan, Assets.ItemName.AirHorn, Assets.ItemName.ClownHorn,
                Assets.ItemName.CashRegister, Assets.ItemName.Candy, Assets.ItemName.GoldBar,
                Assets.ItemName.YieldSign, Assets.ItemName.HomemadeFlashbang, Assets.ItemName.Gift,
                Assets.ItemName.Flask, Assets.ItemName.ToyCube, Assets.ItemName.Remote,
                Assets.ItemName.ToyRobot, Assets.ItemName.MagnifyingGlass, Assets.ItemName.StopSign,
                Assets.ItemName.TeaKettle, Assets.ItemName.Mug, Assets.ItemName.RedSoda,
                Assets.ItemName.OldPhone, Assets.ItemName.HairDryer, Assets.ItemName.Brush,
                Assets.ItemName.Bell, Assets.ItemName.WhoopieCushion, Assets.ItemName.Comedy,
                Assets.ItemName.Tragedy, Assets.ItemName.RubberDucky, Assets.ItemName.ChemicalJug,
                Assets.ItemName.FancyLamp, Assets.ItemName.GoldenCup, Assets.ItemName.Painting,
                Assets.ItemName.Toothpaste, Assets.ItemName.PillBottle, Assets.ItemName.PerfumeBottle,
                Assets.ItemName.Teeth, Assets.ItemName.Magic7Ball, Assets.ItemName.EasterEgg,
                Assets.ItemName.ToyTrain, Assets.ItemName.ToiletPaper, Assets.ItemName.SoccerBall,
                Assets.ItemName.PlasticCup, Assets.ItemName.GarbageLid, Assets.ItemName.ControlPad,
                Assets.ItemName.Clock, Assets.ItemName.ZedDog, Assets.ItemName.BabyKiwiEgg,
                Assets.ItemName.SeveredThigh, Assets.ItemName.SeveredHand, Assets.ItemName.SeveredEar,
                Assets.ItemName.SeveredBone, Assets.ItemName.SeveredTongue, Assets.ItemName.SeveredHeart,
                Assets.ItemName.SeveredFoot, Assets.ItemName.SeveredBoneRib
            };

            Assets.ItemName chosenScrap = scrapList[new System.Random(StartOfRound.Instance.randomMapSeed).Next(0, scrapList.Length)];

            scrapTransmutationEvent = new ScrapTransmutationEvent(
            new Scale(1.0f, 1.0f, 1.0f, 1.0f),
            new SpawnableItemWithRarity(Assets.GetItem(chosenScrap), 100)
            );

            scrapTransmutationEvent.Execute();
        }
    }
}
