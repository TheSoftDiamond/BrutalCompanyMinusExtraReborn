# Changelog

<details>
  <summary>0.22.1</summary>

## Additions

   - Player scaling difficulty. See readme for more info. This should support mods that expand player lobbies by default.
   - Lethal Playtime Events were added. Critters, PlaytimeBig, ItsPlaytime.
   - GiantsOutside. Basically GiantShowdown for those without the mod that adds more Giants.
   - Two surfaced Events. Bellcrab, and Nemo.

## Changes

   - Dust pans were put under bad but are treated as slightly bad and rare.
   - Reduced Amount of Tree spawns to half the amount. As well as their weight a bit.

</details>
<details>
  <summary>0.22.0</summary>

## Additions

   - ShipInventory support for difficulty. (It should no longer softlock if the mod is not present)

## Changes

   - ScrapValue applies to more scrap. Bee Hives, Grabbable Turrets, Grabbable Landmines, Apparatice will properly scale with ScrapValue multiplier state. Thanks bmnr!
   - TransmuteScrapBig.cs and TransmuteScrapSmall.cs will pick an item based on rarity instead of an at randomly picking them from list of items. For example, With LCGoldScrap mod, it kept picking gold scrap all the time, because there are a lot of scrap variations, though they're very rare.

</details>
<details>
  <summary>0.21.10</summary>

## Additions

   - Nightmare Foxy Event

## Changes

   - Apparently Football Event pointed to wrong dependency

</details>
<details>
  <summary>0.21.9</summary>

## Additions

   - SkullEnemy Event
   - ManStalker Event

## Changes

   - Spawn cycle fix on Ignored Moons
   - If Super Eclipse mod is present, the game should no longer roll ShipCoreFailure/LeverFailure Events

</details>

<details>
  <summary>0.21.8</summary>

## Changes

   - Apparently the Bad Dice and regular Dice event was broken, but it has been fixed. If you previously have generated files from BCMER, you will need to either delete the Modded Events Config File, or manually change the MysteryDiceItem to GamblerItem for changes to reflect.

</details>
<details>
  <summary>0.21.7</summary>

## Additions

   - You can blacklist moons from causing events, see the readme for more info.
   - Multipliers for scrap amount and value based on Event Types active are added. *Thank you bmnr for the assistance there!*

</details>
<details>
  <summary>0.21.6</summary>

## Changes

   - Temporarily removed support for ShipInventory due to softlock issue.

</details>
<details>
  <summary>0.21.5</summary>

## Additions

   - Support for Cruiser and Ship Inventory in determining difficulty from scrap items present.

## Changes

   - Fixed some various typos

</details>
<details>
  <summary>0.21.4</summary>

## Additions

   - Seamine
   - Bertha
   - YeetBomb

## Changes

   - Typos here and there on some stuff

</details>
<details>
  <summary>0.21.3</summary>

## Additions

   - EasterEggs and MaskItem event. Don't trust your troll friends with this one!

</details>
<details>
  <summary>0.21.2</summary>

## Changes

   - Apparently commenting those two lines BROKE everything. Well... bug fix update.

</details>
<details>
  <summary>0.21.1</summary>

## Additions

   - Inverse Teleport Event (Very Bad -- TRUST ME)

## Changes

   - Asset Bundles are no longer accidentally packed with the .dll file

</details>
<details>
  <summary>0.21.0</summary>

## Changes

   - The folder for configs is now in the BrutalCompanyMinusExtraReborn Directory, and as a result will generate new files in that directory for the mod to use. If you have any changes from the mod on previous versions in the config files, you may have to input data manually or drag your files from the old folder.

</details>
<details>
  <summary>0.20.5</summary>

## Added
   - Shiba Event (Bad)
   - Facility Meltdown (Very Bad)

## Changes

   - RGBShipLighting no longer occurs for the time being

</details>
<details>
  <summary>0.20.4</summary>

Mostly a bug fix update because I made some mistakes by accident in the previous version. You may need to delete your ModdedEvents.cfg file for the changes to the Baldi event to be present.

## Changes

   - Fixed some more missed typos from files
   - Fixed a issue involving Baldi and MoaiEnemy Mod that could potentially cause the game to softlock or crash because of wrong variable referenced. oops!
   - Baldi Event is no longer VeryBad, but Bad. (Compares to a Barber)

</details>
<details>
  <summary>0.20.3</summary>

## New

   - Added Baldi Event

## Changes

   - Updated README
   - Fixed changelog from previous version update

</details>
<details>
  <summary>0.20.2</summary>

## New

   - Added Goku Bracken Event
   - Added Moai Enemy Event

## Changes

   - Updated README

</details>
<details>
  <summary>0.20.1</summary>

## New

   - Added VeryLateShip Event

## Changes

   - Tweaked some events a slight bit
   - Updated README

</details>
<details>
  <summary>0.20.0</summary>

## New

   - Added VeryEarlyShip Event

## Changes

   - Modified various event descriptions
   - Some bug fixes
   - Updated README (Some work still to do though)
   - Begin development of BrutalCompanyMinusExtra Reborn

</details>

------------- BEGIN FORK ------------- 
<details>
  <summary>0.19.3</summary>

## New

   - Last update for this mod

## Changes

   - Updated README

</details>
<details>
  <summary>0.19.2</summary>

## New

   - None

## Changes

   - Updated README

</details>
<details>
  <summary>0.19.1</summary>

## New

   - Added Security feature to prevent players from using debug commands
   - If debug commands get activated, the host will be alerted
   - Added Lights Magenta command conversion to Lights Purple
   - Added support for other modders to add their own events with their own extension mod

## Changes

   - Removed TakeyPlush and Zombies plush mods from dependencies list
   - Removed ZombiesApocalypse event due to removal request from its owner
   - Removed ZombiesPlush event due to mod conflict and will be moved to BCME - ExternalModule
   - Removed TakeyGokuPlush event due to mod conflict and will be moved to BCME - ExternalModule
   - Removed TakeyGokuPlushBig event due to mod conflict and will be moved to BCME - ExternalModule
   - Removed StreamerEventsControl function due to streamer events being moved to BCME - ExternalModule
   - Fixed Lights command desync
   - Rewrote event registry, events should now generate in the right config file
   - Fixed missing event entries in event registry(*for some reason all modded events werent registered*)
   - Removed LethalModDataLib from dependencies list since its nolonger required

</details>
<details>
  <summary>0.19.0</summary>
   
## WARNING! Deletele VanillaEvents.cfg and ModdedEvents.cfg so it can regenerate with new values, if you dont do that then fixes in "Changes" section wont work!
 

## New

 Theese events were possible to make thanks to help from [Zehs](https://thunderstore.io/c/lethal-company/p/Zehs/)
   - Added (Good) TakeyGokuPlush event
   - Added (Neutral+) TakeyGokuPlushBig event
   - Added (Neutral) Clock event
   - Added (Neutral-) SussyPaintings event
   - Added (Neutral) Train event
   - Added (Good) ZombiesPlush event
   - Added description to "Lights" command
   - Added description to "StreamerEvents info" command¨
   - Added (VeryBad) Dweller event
   - Added (Neutral--) ControlPads event
   - Added (Good) ZedDog event
   - Added (Neutral) PlasticCup event
   - Added (Neutral--) ToiletPaper event
   - Added (Neutral--) SoccerBall event
   - Added (Neutral--) GarbageLid event
   - Added (Bad) MeteorShower event

## Changes

   - Fixed MantiSlayers spawning inside in MantiSlayers event
   - Fixed MantiToils spawning inside in MantiToils event
   - Return of MantiToils event
   - Return of NoMantiToils event
   - Return of TurretsEverywhere event
   - Adjusted Scale values is NutSlayers event
   - NutSlayers now only spawn outside in NutSlayers event
   - Adjusted Scale values in ZombieApocalypse event to make it less annoying

## Known issues

   - For some reason it is required that ZombiesPlush and TakeyPlush mods are present
   - Added temporarily Zombies Plush and TakeyPlush mods as dependencies until issues are resolved

</details>
<details>
  <summary>0.18.5</summary>
   
## New

   - Rebuild the mod with v62 game files
   - Added CrowdControl checks to disable events deemed as "incompatible"

## Changes

   - Decreased scrap value multiplier from 7 to 4.5 in NutSlayers event
   - Changed door closing time from 10am to 3pm

## Known issues

   - None

</details>
<details>
  <summary>0.18.4</summary>

## Changes

   - Removed code preventing the mod from loading

</details>
<details>
  <summary>0.18.3</summary>
   
## New

   - Added (Fun) RGBShipLights event as requested
   - Implemented new function StreamerEventsControl that allows you to disable all streamer events without the need of restarting the game
   - Added LethalModDataLib as dependency to make sure StreamerEventsControl is working properly

## Changes

   - Increased scrap value in NutSlayers event to make it "fair", also decreased NutSlayer enemy spawnrate
   - Improved TerminalFailure event to completelly break the terminal instead of just preventing purchases
   - Improved ShipLightsFailure event to kill the lights and disable the lightswitch instead of just disabling the lightswitch
   - Fixed DoorCircuitFailure not spawning due to code oversight

## Known issues

   - Disabled Mantitoils event due to mantitoils spawning inside
   - Disabled TurretsEverywhere event due to its relation with Mantitoils event
   - Disabled NoMantitoils event due to its relation with Mantitoils event
   - Disabled MaskedHorde event dut to it spawning mimics without the event being selected

</details>
<details>
  <summary>0.18.0</summary>
   
   - Added (Extreme) NutSlayers
   - Added (Bad) JetpackFailure, Note: Not sorry Takey <img src="https://imgur.com/u6zqX6q.png" width="50px"> 
   - Added (Bad) FlashLigtsFailure
   - Added GokuBracken dependency to make sure TakeyGokuBracken is only added when all essential mods are present
   - Attempt fix for some of ToilHead related events calling Execute method regardless of the event not being added
   - Attempt fix to prevent MantiToils from spawning inside
   - Attempt fix for TerminalFailure not showing error message on the screen after attemping purchase
   - Fixed code oversight that allowed TurretsEverywhere event to spawn regardless of ToilHead mod not being 
     present and making the game stuck because of that
   - Fixed ToilHead mod dependency list missing related events entries
   - Updated README file

</details>
<details>
  <summary>0.17.16</summary>
   
   - *DATA REMOVED*

</details>
<details>
  <summary>0.17.15</summary>
   
   - Added (VeryBad)DoorCircuitFailure event
   - Added (Bad) TargetingFailure event
   - Added v55 compatibility, but be warned there still might be undiscovered bugs

## Warning

   - As of v55 all mod integrations are deemed incompatible, 
   i recommend that u do your own research on what works and what doesnt, 
   as soon as the mods are updated for v55, i will test them and fix a few things if required

</details>
<details>
  <summary>0.17.10</summary>
   
   - Fixed minor issue

</details>
<details>
  <summary>0.17.9</summary>
   
   - Added(Good)DoorOverdrive event
   - Added(Bad)TakeyGokuBracken event  
   - Added personal library file (Essential for the mod to work)
   - *missing changelog entry*
   - <img src="https://i.imgur.com/6TWu85g.gif" width="50px">
   - Added TerminalAPI dependency in order to improve TerminalFailure event functionality
   - EmergencyDice mod is unsupported, 
   so if any events regarding this mod will break i will not fix them and probably just remove them
   
   Note: Make sure to delete "VanillaEvents" so it regenerates

## Known issues
   - Event Crazy is not working at all
   - Sometimes manticoils spawn inside the facility without an event involving them
</details>
<details>
  <summary>0.17.4-Emergency patch</summary>
   
   - Rewrote the code of events added in 1.16.9
   - Fixed code issue where effects of the events would only apply to the host
   - Added(Very Bad) ToilSlayer event
   - Fixed ZombieApocalypse event to not trigger that often
   - Changed ZombieApocalypse category from Extreme to Very Bad
   - Reduced the chance of streamer special events so they are rare Note: This was requested
   - Updated API references for ZombieApocalypse, ToilHeads and MantiToils
   - Added(Very Bad) MantiSlayer event
   - Added(Very Bad) AllSlayers event
   - Added(Remove) NoToilSlayer event
   - Added(Remove) NoMantiSlayer event
   - Added(Remove) NoSlayers event
   - Added LethalNetworkAPI dependency *This enry was added later*

Note: *NO DATA* is a reference to an unscheduled update

</details>
<details>
  <summary>0.16.9</summary>

   - NO DATA

</details>
<details>
  <summary>0.15.3</summary>

   - Added (Bad)TurretsEverywhere event
   - Added (Bad)Lasso event(NonShippingAuthorisation required for the event to occur)
   - Reduced Dust pan item spawn rate in Dust pans event
   - Added (Even worse then Very Bad)ZombieApocalypse
   - Changed WelcomeToTheFactory category event from Neutral to Bad
   - Removed unwanted code from NoMantitoils event

</details>
<details>
  <summary>0.14.2</summary>

   - Fixed NoMantiToils event with the help of Zehs <3
   - Added (Neutral)WelcomeToTheFactory event
   - Added (Neutral)DustPans event
   - Added (Very Bad)MaskedHorde event
   - Slightlly increased spawn weight of specific events to make sure they are not super rare

</details>
<details>
  <summary>0.13.7</summary>

   - Added TakeyPlush integration
   - Added (Good)TakeyPlush event
   - Added (Very Bad)MantiToil event
   - Added (Remove)NoMantiToil event
   - Added (Very Bad)Doors event

</details>
