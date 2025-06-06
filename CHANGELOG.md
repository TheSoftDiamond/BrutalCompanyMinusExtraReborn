# Changelog

<details>

  <summary>1.25.1</summary>

This update brings BCMER into v70 finally! Bugs and crashes may occur. Versions prior to this may not be compatible with v70.

# Additions

  - LockedEntrance, requires Special Event to be enabled

# Fixes

  - 

# Changes

  - Modified values for kiwi bird event.

</details>
<details>

  <summary>1.25.0</summary>

This update brings BCMER into v70 finally! Bugs and crashes may occur. Versions prior to this may not be compatible with v70.

# Additions

  - Added GiantKiwi and its egg to BCMER's register.
  - Added 'KiwiBird' event to BCMER, but currently depends on Special Events being enabled. Currently not tested fully.

# Fixes

  - PhonesOut event will now only harmony patch if LethalPhones is present
  - Desync/Despawn bugs related to outside objects and hazards should properly despawn and sync up to all clients now.
  - Enemy behavior in the _EnemyAI.cs file broke on some modded moons. While it has been removed from BCM, I have made is so it only will be present if StarLancerAI is not present. This is so I can try to monitor and study the behavior of this code more closely, but I highly recommend the use of StarLancerAI fix if enemy behavior should break.
    Project updated for v70.

</details>
<details>

  <summary>0.24.10</summary>

# Fixes

  - Small fix related to RPC method on the PhonesOut event.

</details>
<details>

  <summary>0.24.9</summary>

# Fixes

  - PhonesOut event will now only harmony patch if LethalPhones is present
  - Desync/Despawn bugs related to outside objects and hazards should properly despawn and sync up to all clients now.

</details>
<details>

  <summary>0.24.8</summary>

Just a small update that adds one new event. Though.. I also want to add more "good" or "VeryGood" rated events too.
# Additions

  - Added PhonesOut event, which requires Lethal Phones.

</details>
<details>
  <summary>0.24.7</summary>

# Additions

  - Readded ShipLightsFailure
  - Added Compatibility Check to SelfSortingStorage. Future Items checks will be added.

# Changes

  - ShipLightsFailure should no longer crash and works perfectly for both the host and clients
  - FlashlightsFailure event should work properly for both the host and client correctly. Grabbing and charging flashlights in general should fail to work. Upon leaving, flashlights should register as fully charged again and will display as such correctly.
  - Semi-what fixed NutcrackerAI.cs issue, and somewhat introduced a server rpc bug but it shouldn't be spamming the object reference missing thing every tick. If anyone has more experience with enemy ai, please feel free to get in contact with me, so we can see what we can do to fix the issues better!
</details>

<details>
  <summary>0.24.6</summary>

This update adds new events and modifies some various things in the code. I may do a bit of tweaking with the values for some of these new events down the road.

# Additions

  - Added Time Chaos. Requires Special Events to be enabled.
  - Added the Leaf Boys from BioDiversity. https://thunderstore.io/c/lethal-company/p/super_fucking_cool_and_badass_team/Biodiversity/
  - Added Welcome To Ooblterra event. https://thunderstore.io/c/lethal-company/p/Skeleton_Studios/Welcome_To_Ooblterra/

# Changes

  - Updated readme to reflect mod compatibility.
  - ReadSettingsEarly function should run better.
  - Worm event no longer spawns snare fleas outside, and instead buffs snare fleas spawns inside.

</details>
<details>
  <summary>0.24.5</summary>

# Additions

  - Added [VeryBad] Soul Devourer event from Soul Devourer Enemy Mod. https://thunderstore.io/c/lethal-company/p/bcs4313/Soul_Devourer_Enemy/
  - Added [Bad] Light Eater Enemy event. https://thunderstore.io/c/lethal-company/p/Lega/LightEater/

# Fixes

  - Shrimp now depends on LCShrimpMod, fixing an incorrect mod dependency check.
  - Forced Events will no longer complain about an empty white space

# Changes

  - MoaiEnemy event has been nerfed.
  - Soul Devourer is now its own event.

</details>
<details>
  <summary>0.24.4</summary>

# Fixes

  - Hopefully the issues people have had to that ShipLightsFailure being removed, or something about Terminal nodes with it? ShipCoreFailure may still attempt to roll "ShipLightsFailure" if your events config for shipcorefailure still references shiplightsfailure, but it will not cause a softlock. This should only affect users who have the same config files before 0.24.2.

</details>

<details>
  <summary>0.24.3</summary>

Apparently the changelog never saved despite me double checking it, it somehow reverted... This version is just to fix the changelog and adds nothing on top of 0.24.2.

</details>

<details>
  <summary>0.24.2</summary>

# Additions

  - Ability to let other mods handle power counts and spawn curves. A few settings will still affect these as I thought users may be interested in those features still working alongside this.

# Removed

  - Temporarily removed Ship Lights Failure for the time being due to crashes. Hopefully it can be back in a timely manner.

</details>

<details>
  <summary>0.24.1</summary>

# Additions

  - Factory Size Multiplier: Can be globally adjusted with respect to difficulty scaling. Recommended to leave as is if you don't wish to change dungeon size. Setting this number too big or too small may cause generation issues or delays. Tested values were from 0.5x to 25x, but may depend on interior and system specs.
  - Starting time: Can adjust the starting time globally. This can also be adjusted with respect to difficulty scaling.
  - Time Scaling: Can be adjusted to change how fast a day goes. This can be adjusted with respect to difficulty scaling.

</details>
<details>
  <summary>0.24.0</summary>

# Additions

  - Ability to suppress get method, prefab, Enum warnings from the game. On second run of 0.24.0, will apply it automatically to true.

# Changes

  - Enable extra loggings was moved to [Debugging] instead of [General].

# Fixes

  - A softlock involving null items should no longer occur
  - Fixed a way items and enemies were instanced

</details>

<details>
  <summary>0.23.10</summary>

# Additions

  - Scrap amount and value caps

</details>
<details>
  <summary>0.23.9</summary>

# Fixes

  - Fixed code to work with ShipInventory 1.2.6 and ShipInventoryUpdated.

</details>
<details>
  <summary>0.23.8</summary>
  
## Added

  - Added HotBarHassle and HotBarMania events (Thanks Zehs for the help here!)
  - Several code optimizations (Thanks Zehs for the help here!)

## Changes

  - = Asset Bundles are now outside of the dll. Makes easier to use.

</details>
<details>
  <summary>0.23.7</summary>
  
## Changes

  - VeryLateShip has been configured to use the same time speed as "Hell" event. As well as not being dependant on scaling like some of the other time functions. **For this to be turned off, you can go into the CoreProperites and disable the Time Adjustment in Event Features Category** (I may move this into the specific events themselves in the future is possible).

</details>
<details>
  <summary>0.23.6</summary>

Also, forgot to link previous update but if you want the Goku event: https://github.com/TheSoftDiamond/BCMER_CustomEvents/blob/main/Goku.json

## Changes

  - Apparently in my state of low sleep, I had forgot to unregister HotBarHassle and HotBarMania.

</details>
<details>
  <summary>0.23.5</summary>

I've been working on a website for the mod, which you can find at https://bcmer.softdiamond.net/ so this update has been a bit delayed.

## Additions

  - Sanitization of Event Names (Spaces, etc would cause softlocks/crashes)

## Changes

  - Skull Enemy had wrong dependency for it.
  - Adjusted weight of GiantsOutside event

## Removed

  - **Removed GokuBracken event**. You can find its custom event json here:
  - Removed a couple of events that are no longer part of the mod.
  - *Removed a few other events that had no obvious mod dependency or source as to what mod they were from. Some of which were deprecated events or removed from the event registry from BCME(R) itself. If anyone knows the source of these mods, feel free to contact me.*

Note: HotBarMania and HotBarHassle were going to be a part of this version, but due to issues and finetuning required, I will be pushing it back to a future build.

</details>

<details>
  <summary>0.23.4</summary>

## Additions

  - Added [Neutral] Needy Cats Event

## Changes

  - Added more Gold Scrap to the LCGoldScrap Event.
  - Optimized code so that duplicate events should not occur
  - Fixed Custom Events AGAIN (Hopefully the last time)

</details>
<details>
  <summary>0.23.3</summary>

## Additions

  - Extra Logging. Useful for debugging purposes and more. Defaults to off in config.

## Changes

  - Due to broken Barber behavior from events, the Barber event has been removed again.

</details>
<details>
  <summary>0.23.2</summary>

## Changes

  - Bug fixes regarding Custom Events. (For more context, the changes in 0.23.1 were not reliably working as expected, so this hopefully should be the last time I need to deal with this issue!). If you encounter issues, try deleting the CustomEvents.cfg file and see if that fixes the issue(s).

</details>
<details>
  <summary>0.23.1</summary>

## Changes

  - The CustomEvent.cfg error should be fixed
 
</details>
<details>
  <summary>0.23.0</summary>

## Additions

  - Custom Event Support. See [here](https://github.com/TheSoftDiamond/BrutalCompanyMinusExtraReborn/blob/main/CustomEventDocumentation.md) for info on how custom events supports work.

## Changes

  - Fixed some events referring to wrong dependency by accident.
 
</details>
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
