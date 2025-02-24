# Brutal Company Minus Extra Reborn
![Screenshot](https://softdiamond.net/images/smalllogoBCMER.png)

<details>
  <summary><b>Special Thanks to</b></summary>
  
  - UnloadedHangar, for allowing me to continue the project and help develop on it.
  - bmnr2819, for their contributions and help with testing.
  - KanoliAvali, for their contribution and help with testing

</details>

- This mod is required on all clients
- This mod will make the game harder. Download if you want to suffer.
- Configs are fully generated when loading moons.
- Highly customizable!
- Modded event support when said mods installed.
- Supports external event injection

# Features
<details>
  <summary><b>Event Mechanics</b></summary>
    
  - Whenever you land on a moon, a couple of events will be chosen (Between 2 to 5 by default), these events will appear in the UI in the top right hand corner of your screen, this can be open and closed (by pressing 'k' by default) and scrolled by pressing arrow buttons on the keyboard or a custom set value in the config.

  - Events will come in 6 main types and are categorized by color, with each type having their own weights. This can be changed in the config to use custom weights instead.

| Event Type | Base Weight | Increment | MinCap | MaxCap |
|-|-|-|-|-|
| Very Good | 3 | 0.14 | 3 | 17 |
| Good | 23 | -0.1 | 13 | 23 |
| Neutral | 10 | -0.05 | 5 | 10 |
| Bad | 40 | -0.15 | 25 | 40 |
| Very Bad | 5 | 0.25 | 5 | 30 |
| Remove Enemy | 15 | -0.05 | 10 | 15 |

</details>
<details>
  <summary><b>Weather Multipliers</b></summary>

  - Weathers come with scrapValue and scrapAmount multipliers.
  - These will also be displayed ingame on the terminal in this format <mark>(xScrapValue, xScrapAmount)</mark> as such
![Screenshot](https://softdiamond.net/BCMERAssets/WeatherMultipliers.png)

  - You can also enable randomize weather multipliers in the config, which adds a bit of randomization to the weather after every day.
  - Default multiplier values goes as follows, these can be changed in the config.

**WARNING: Setting a factory size value below 1.00 may crash your game**

| Weather    | Scrap value | Scrap amount | 
|------------|-------------|--------------|
| None       | 1.00        | 1.00         |
| DustClouds | 1.05        | 1.00         |
| Rainy      | 1.05        | 1.00         |
| Stormy     | 1.35        | 1.25         |
| Foggy      | 1.15        | 1.10         |
| Flooded    | 1.25        | 1.15         |
| Eclipsed   | 1.35        | 1.25         |
</details>

<details>
  <summary><b>Difficulty Scaling</b></summary>

  - This mod will scale from certain factors, it can scale from **Days Passed**, **Scrap In Ship**, **Moon Risk**, **Weather** and **Quota**, by default **Days Passed**, **Scrap In Ship** and **Moon Risk** are used, these can all be configured in the config.
  - Everything scales off of a number called difficulty, by default difficulty caps at 100.

| Source | Multiplier/Additional | Cap | Enabled by default? |
|-|-|-|-|
| Days Passed | x1 | 60 | True |
| Scrap In Ship | x0.0025 | 30 | True |
| Moon Grade | D:-8, C:-8, B:-4, A:+5, S:+10, S+:+15, S++:+20, S+++:+30, Other:+10 | None | True |
| Quota | x0.005 | 100 | False |
| Weather | None:+0, Rainy:+2, Flooded:+4, Foggy:+4, Stormy:+7, Eclipsed:+7 | None | True |
| Player Scaling | 1x | None | False |

  - All events scale off of difficulty, event type chances scale off of difficulty and alot more.

### Player Scaling

The player scaling function is turned off by default. It is a powerful tool that can make Brutal Company more easier or harder depending on the amount of players. You can control:
- **Base Player Amount**
  - Formulas work off this number. Keep this number to the expected ideal amount of players you'd want as the base amount of players, where more or less players will cause a change in difficulty. 
- **Player Scaling Multiplier/Factor**
  - Recommended to keep around 1, but depends on how your configs are set.
  - Below 1: Lower player counts become harder, while higher player counts become easier
  - Above 1: Lower player counts become easier, while higher player counts become harder
- **Linear** or **Exponential** Growth on the formula
  - Linear grows at a constant smooth rate, useful for smooth changes in difficulty.
  - Exponential grows at a steeper rate, useful for steep changes in difficulty.

Calculating Player Delta:
![Screenshot](https://softdiamond.net/BCMERAssets/playerDelta.png)

Linear Function:
![Screenshot](https://softdiamond.net/BCMERAssets/theLinear.png)

Exponential Function:
![Screenshot](https://softdiamond.net/BCMERAssets/theequation.png)


  - This is what can be shown in the UI
![Screenshot](https://softdiamond.net/BCMERAssets/Difficulty.png)
</details>

<details>
  <summary><b>Terminal Commands</b></summary>
    
  - The mod has commands that only host can use, to display all commands type 'mhelp' into the terminal.

![Screenshot](https://softdiamond.net/BCMERAssets/mhelp.png)

  - Example of 'mevents' command.

![Screenshot](https://softdiamond.net/BCMERAssets/mevents.png)

  - Example of 'mevents nutcracker' command.

![Screenshot](https://softdiamond.net/BCMERAssets/nutcracker.png)

  - Example of 'mevent hell hell nutcracker' command.

![Screenshot](https://softdiamond.net/BCMERAssets/mventsSet.png)

  - Example of 'menemies' command. (List will vary depending on mods installed)

![Screenshot](https://softdiamond.net/BCMERAssets/menemies.png)

</details>

<details>
  <summary><b>Event List</b></summary>
  
  - List of all events in corresponding types
  
  <details>
    <summary><b>Very Good</b></summary>
    
| Name | Description |
|-|-|
| Big Bonus | Large sum of credis |
| Scrap Galore | Increased scrap value and amount |
| Golden Bars | Only golden bars will spawn on the map |
| Big Delivery | Spawns a shipment with a bunch of items |
| Plenty Outside Scrap | Spawns scrap outside |
| Black Friday | Everything will go on sale |
| Safe Outside | Will prevents enemies from spawning outside and certain events |
| VeryEarlyShip | Ship is as early as it can be |
  </details>

  <details>
    <summary><b>Good</b></summary>

| Name | Description |
|-|-|
| Bounty | Killing enemies will now reward credits |
| Bonus | Sum of credits |
| Smaller Map | Reduces factory size |
| More Scrap | Increased scrap amount |
| Higher Scrap Value | Increased scrap value |
| Golden Facility | Only spawns Goldencup, Ring, Goldbar, Fancylamp, Perfumebottle, Painting and Cashregister on the map |
| Dentures | Only spawns teeth on the map |
| Pickles | Only spawns pickles on the map |
| Honk | Only spawns horns on the map |
| Transmute Scrap Small | Takes any one-handed scrap in map scrap pool and only spawns that |
| Small Delivery | Spawns a shipment with some items |
| Scarce Outside Scrap | Spawns scrap outside |
| Fragile Enemies | Decreases enemy hp |
| Full Access | All Doors are unlocked and open and big doors are all unlocked, prevents facility ghost. |
| Early Ship | Time will start earlier |
| More exits | Spawns entrances and exits |
| DoorOverdrive[Does NOT occur if CrowdControl is present] | Makes the ship door power to be constantly 100% |
  </details>

  <details>
    <summary><b>Remove Enemy</b></summary>
    - These will also prevent any event with that 'enemy' to be picked.
    

| Name | Description |
|-|-|
| No Baboons | Removes Baboon Hawk |
| No Bracken | Removes Bracken |
| No Coilhead | Removes Coil Head |
| No Dogs | Removes Eyeless Dog |
| No Giants | Removes Forest Keeper |
| No Hoarding Bugs | Removes Hoarding Bug |
| No Jester | Removes Jester |
| No Ghosts | Removes Ghost Girl and Facility Ghost |
| No Lizards | Removes Spore Lizard |
| No Nutcrackers | Removes Nutcracker |
| No Spiders | Removes Bunker Spider |
| No Thumpers | Removes Thumper |
| No Snare Fleas | Removes Snare Flea |
| No Worm | Removes Earth Leviathan |
| No Slimes | Removes Hygrodere |
| No Maksks | Removes Masked and removes related scraps |
| No Turrets | Removes Turret |
| No Landmines | Removes Landmine |
| No OldBirds | Removes Oldbird |
| No Butlers | Removes butlers |
| No Spiketraps | Removes spiketraps |
  </details>

  <details>
    <summary><b>Neutral</b></summary>

| Name | Description |
|-|-|
| Nothing | Nothing |
| Locusts | Spawns Roaming Locusts |
| Birds | Spawns Manticoils |
| Trees | Spawns Trees |
| Leafless Brown Trees | Spawns trees without leaves |
| Leafless Trees | Spawns spooky trees |
| Gloomy | Makes the atmosphere foggy |
| Rainy | Makes it rain (No mud) | 
| Heavy Rain | Makes the atmosphere rainy, flooded and stormy. Triple rain. |
| RGBShipLights | Allows you to change the ship lights color when the event is active | 
  </details>

  <details>
    <summary><b>Bad</b></summary>

| Name | Description |
|-|-|
| Hoarding Bugs | Spawns Hoarding bugs outside and inside, comes with Scarce Outside Scrap |
| Bees | Spawns Bees outside |
| Landmines | Increased rates of landmines inside |
| Lizard | Spawns Spore Lizard inside |
| Slimes | Spawns Hygrodere outside and inside |
| Thumpers | Spawns Thumpers inside |
| Turrets | Increased rates of turrets inside |
| Spiders | Spawns Bunker Spiders outside and inside, comes with Leafless Brown Trees |
| Snare Fleas | Spawns Snare Fleas inside |
| Facility Ghost | The ghost can open/close bigdoors and doors, mess with lights, mess with the breaker and can lock/unlock doors(Rare) |
| Outside Turrets | Spawns turrets outside, comes with Trees |
| Outside Landmines | Spawns Landmines outside |
| Dustpans | Makes the game spawn lot of dustpans | 
| Grabbable Turrets | Turns some of the turrets on the map into scrap |
| Grabbable Mines | Turns some of the mines on the map into scrap |
| Shipment Fees | Any shipment's on given moon will deduct credits as a fee |
| Strong Enemies | Increases enemy hp |
| Reality Warp | Attempting to grab scrap will make it transform into something else, sometimes a landmine or turret |
| Kamikazie Bugs | Hoarding bugs will now blow up when angered | 
| Masked | Spawned masked enemies |
| Butlers | Will Spawn butlers |
| Spike Traps | Will spawn spike traps inside |
| Flower Snake | Will spawn flower snakes inside and outside |
| Late Ship | Time will start a little later |
| Holiday Season | Turns scrap into mystery boxes and eggs and spawns nutcrackers and hoarding bugs inside |
| Welcome To The Factory | Makes the game spawn metallic objects only |
| Dweller | Spawns maneater outside  |
| TargetingFailure | Makes the teleporter choose random player when teleport button is pressed |
| TeleporterFailure | Breaks teleporters |
| TerminalFailure | Breaks the terminal |
| WalkieFailure | Makes walkies unoperational |
| LeverFailure | Makes you unable to leave by pulling the ship lever(Ship leaves at midnight) |
| ManualCameraFailure | Breaks the vannila ship screen |
| ShipLightsFailure | Breaks the light switch and kills the ship lights |
| DoorFailure | Makes the ship door broken so they cant be closed |
| FlashLightsFailure | Makes flashlights unoperational |
| ItemChargerFailure | Makes you unable to charge anything electric |
| JetpackFailure | Makes jetpacks unoperational |
| LassoEv | Spawns lot of LassoMan enemies[Requires NonShippingAuthorisation to be present in order to work] |
  </details>

  <details>
    <summary><b>Very Bad</b></summary>
    
| Name | Description |
|-|-|
| Nutcracker | Spawns Nutcrackers outside and inside, comes with Turrets |
| Arachnophobia | Spawns alot of Bunker Spiders outside and inside, comes with Leafless Trees |
| Bracken | Spawns Brackens Inside |
| Coilhead | Spawns Coilheads Inside |
| BaboonHorde | Spawns alot of Baboon Hawks outside |
| Dogs | Spawns Eyeless Dogs outside and inside |
| Jester | Spawns Jesters inside |
| Little Girl | Spawns Ghost Girls inside |
| Anti Coilhead | Changes Coilhead AI and spawns them inside, comes with Leafless Trees and Gloomy |
| Bad Produce | Decreased Scrap Value but Increased Scrap Amount |
| Transmute Scrap Big | Takes any two-handed scrap in map scrap pool and only spawns that |
| War Zone | Acts as Quad event including Turrets, Landmines, Outside Turrets and Outside Landmines and will also come with artillery fire!! |
| Bug Horde | Spawns a load of Hoarding Bugs outside and inside, comes with Scarce Outside Scrap |
| Forest Giant | Spawns a Forest Keeper inside |
| Inside Bees | Spawns Bees outside and inside |
| Nutslayer | Spawns the Nutslayer inside the facility, kills everything... comes with gloomy, thumpers, spiders and masked. |
| Hell | Great reward, but at what cost... |
| AllWeather | Acts as Eclipsed, Stormy, Flooded and Raining all in one day |
| Worms | Will spawn worms inside and outside and snare fleas inside and outside |
| OldBirds | Will spawn old birds and comes with Landmines and Outside Landmines |
| DoorCircuitFailure[Does NOT occur if CrowdControl is present] | Shuts the ship door at 3PM and opens it at 10PM |
| ShipCoreFailure | Breaks almost everything in the ship | 
| MaskedHorde[Removed in 0.18.3] | Spawns lot of Mimics and i mean a lot |
| NutSlayersMore | Spawns lot of NutSlayers only outside[Warning! This is Extreme and you probably wont survive] |
| VeryLateShip | The ship arrives at a very late hour! |
| GiantsOutside | Spawns a whole bunch of Giants Outside. Alternative to GiantShowdown |
  </details>
  
</details>

<details>
  <summary><b>Modded Event List</b></summary>
  
  - These events will only appear with said mods installed
  - Currently supported mods include Lethalthings, Diversity, Scopophobia, HerobrineMod, SirenHead, RollingGiant, TheFiend, Lockers, TheGiantSpecimens, Football, Mimics, LCGoldScrapMod, Toilhead, Moonswept, ShockwaveDrones, FacelessStalker, EmergencyDice, MoaiEnemy, BaldiEnemy, TestAccountVariety, Surfaced, Peepers, NightmareFoxy, Man Stalker, Skull Enemy, ...
  
  <details>
    <summary><b>Very Good</b></summary>
    
| Name | Description |
|-|-|
| CityOfGold | Will only spawn golden scrap |
  </details>

  <details>
    <summary><b>Good</b></summary>

| Name | Description |
|-|-|
| Dice | Only spawns various dice |
| Nemo | Spawns that clownfish |
| HotBarMania | Spawns more inventory slots |
  </details>

  <details>
    <summary><b>Remove Enemy</b></summary>
    - These will also prevent any event with that 'enemy' to be picked.
    

| Name | Description |
|-|-|
| No Lockers | Prevents lockers from spawning |
| No ImmortalSnail | Prevents the immortal snail from spawning |
| No Fiend | Prevents the fiend from appearing |
| No ShyGuy | Prevents the shy guy from appearing |
| No Peepers | Prevents the peepers from appearing | 
| No Mimics | Prevents mimics from spawning |
| No MantiToils | Prevents mantitoils from spawning |
| No MantiToilSlayer | Prevents MantiToilSlayers from spawning |
| No Slayers | Prevents all slayers from spawning(NutSlayer excluded) |
| No ToilSlayers | Prevents ToilSlayers from spawning |
  </details>

  <details>
    <summary><b>Neutral</b></summary>

| Name | Description |
|-|-|
| NeedyCats | Spawns a whole bunch of cats |
  </details>

  <details>
    <summary><b>Bad</b></summary>

| Name | Description |
|-|-|
| Roomba | Spawns boombas inside and sometimes outside |
| TeleporterTraps | Spawns teleporters traps inside |
| Mimics | Increased mimics amount |
| Peepers | Spawns peepers inside and outside |
| Shrimp | Spawns the shrimp inside |
| RollingGiants | Spawns the rolling giants inside |
| ImmortalSnail | Spawns the immortal snail inside |
| Lockers | Spawns the lockers inside |
| Football | Spawns football inside |
| Cleaners | Spawns cleaners inside |
| Mobile Turrets | Spawns Walking turrets Inside |
| Shockwave Drones | Spawns ShockwaveDrones inside |
| TakeyGokuBracken[Requires: BCME - ExternalModule] | Spawns quite a lot TakeyGokuBracken enemies |
| Baldi | Baldi will chase you throughout the facility |
| YeetBomb | Prpulsion Mines Everywhere |
| Seamine | Seamines... EVERYWHERE outside |
| SkullEnemy | The floating skull chases you.. |
| Foxy | Nightmare Foxy chases... |
| Bellcrab | Spawns a whole bunch of bellcrabs.. Good luck. |
| HotBarHassle | You lose inventory slots |
  </details>

  <details>
    <summary><b>Very Bad</b></summary>
    
| Name | Description |
|-|-|
| The Fiend | Will spawn the fiend |
| Herobrine | Will spawn herobrine |
| Sirenhead | Will spawn the sirenhead |
| Walkers | Will spawn the walker |
| ShyGuy | Will spawn the shyguy | 
| GiantShowdown | Will spawn the redwood giant and giants outside |
| Bad Dice | Only spawns bad dice |
| SlenderMan | Spawns slender man |
| MantiToils | Spawns lot of MantiToils outside |
| AllSlayers | Spawns all slayers from ToilHead mod |
| MantiToilSlayer | Spawns lot of MantiToilSlayers |
| ToilSlayer | Spawns ToilSlayers |
| ZombieApocalypse[Removed in 0.19.1] | Spawns lot Mimics with Zombies reskin |
| MoaiEnemy | Spawns a lot of Moai enemies. Good luck will be needed. |
| Bertha | Big Bertha Mines... EVERYWHERE outside |
| ManStalker | Spawns Man Stalker to chase you.. Avoid at all costs |
| Critters | Spawns a whole bunch of critters |
| PlaytimeBig | Spawns playtime enemies |
| ItsPlaytime | Ready for yourself for Critters and some Playtime Enemies |


  </details>
  
</details>

<details>
  <summary><b>External Events Support</b></summary>

  - As of 0.19.1 this mod supports external events injection from other mods meaning
 other modders can add their own events with their own extension mod.
  - If you want to make your own extension mod then you can decompile official extension [BCME - ExternalModule](https://thunderstore.io/c/lethal-company/p/UnloadedHangar/BCME_ExternalModule/) and see for your self how it works.

</details>

# Configuration
Alot of options in the config will contain a **scale**, a scale will contain a **Base**, **Increment**, **MinCap** and **MaxCap**.

This is used to scale the game harder the more days pass.

The formula used to compute the **scale** is `Base + (DaysPassed * Increment)`.

**MinCap** is the value that the computed value wont go below.

**MaxCap** is the value that the computed value wont go above.

# Config overview

## Core Properties Settings
Location: BrutalCompanyMinusExtraReborn\CoreProperties.cfg`

#### Custom Events

`Enable Custom Events?` : Enable Custom Event Support. See the Custom Event Documentation for more info.

#### Events Features

`Enable Hell Time Adjustment` : Adjust whether or not Hell Event uses regular Lethal Company time scale, or an adjusted one for the event. Defaults to True (adjusted scale for event).

`Enable VeryLateShip Time Adjustment` : Adjust whether or not VeryLateShip Event uses regular Lethal Company time scale, or an adjusted one for the event. Defaults to True (adjusted scale for event).

#### General

`Enable Extra Logging?` : Mostly used for debugging purposes, but also can hide some details in the Log Output that tend to cause a lot of console spam, for example: NutSlayer event.

## Difficulty Config 
Location: `BrutalCompanyMinusExtraReborn\Difficulty_Settings.cfg`

#### Event Settings

`Use custom weights`: By default mod will use event type weights.

`Event scale amount`: A **scale** that describes the base amount of events.

`Weights for bonus events`: Extra chances for bonus events added on top of the base events.

`Display events in chat`: Display events in chat?

#### Event Type Scrap Multipliers

`VeryBad Scrap Amount Scale` : The amount multiplier **scale** for **VeryBad** events that are active during the moon.

`VeryBad Scrap Value Scale` : The value multiplier **scale** for **VeryBad** events that are active during the moon.

**...**

`Remove Scrap Value Scale` : The value multiplier **scale** for **Remove** events that are active during the moon.

#### Event Type Weights

`Very Good Weight scale`: The weight **scale** for **VeryGood** event to be chosen

**...**

`Very Bad Weight scale`: The weight **scale** for **VeryBad** event to be chosen

#### Difficulty

`Spawn Chance Multiplier`: A **scale** that multiplys the spawn rate.

`Inside Spawn Chance Additive`: This will add to all keyframes for the insideSpawn animation curve.

`Outside Spawn Chance Additive`: This will add to all keyframes for the outsideSpawn animation curve.

`Spawn Cap Multiplier`: A **scale** that multiplys the spawn caps, this allows for a higher maximum amount of enemies to spawn outside and inside.

`Additional Inside Max Enemy Power`: A **scale** that adds to the inside max enemy power count.

`Additional Outside Max Enemy Power`: A **scale** that adds to the outside max enemy power count.

`Addional hp`: A **scale** that adds bonus hp to enemies.

`Global scrap value multiplier scale`: A **scale** that multiplies all scrap value.

`Global scrap amount multiplier scale`: A **scale** that multiplies all scrap amount.

`Good event increment multiplier`: A global multiplier that will multiply all **Good** and **Very Good** increments.

`Bad event increment multiplier`: A global multiplier that will multiply all **Bad** and **Very Bad** increments.

#### Difficulty Scaling

`Difficulty Transitions`: A visual indicator in the UI that can be configured.

`Ignore max cap`: Ignore max cap from **scales**?

`Difficulty max cap`: Difficulty wont go beyond this number

`Scale by days passed`: Scale by days passed?

`Days passed difficulty multiplier`: Multiplier on days passed.

`Days passed difficulty cap`: Days passed wont add difficulty beyond this number.

`Scale by scrap in ship`: Scale by scrap in ship?

`Difficulty per scrap in ship`: Multiplier on scrap in ship.

`Scrap in ship difficulty cap`: Scrap in ship wont add difficulty beyond this number.

`Scale by quota`: Scale by quota to hit?

`Difficulty per quota value?`: Multiplier on quota.

`Quota difficulty cap`: Quota wont add difficulty beyond this number.

`Scale by moon grade?`: Scale by moon Grade?

`Grade difficulty scaling`: Additive difficulties depending on moon risk/grade.

`Scale by weather type?`: Scale by weather type?

`None weather difficulty?`: Difficulty added if none weather.

**...**

`Eclipsed weather difficulty?`: Difficulty added if eclipsed weather.

#### Player Scaling

`Enable Player Scaling`: Should the amount of players affect the difficulty.

`Plaayer scaling type`: Linear or Exponential functions _(see Difficulty Scaling under Features for how formulas work)_

`Player Scaling Factor/Multiplier`: Multiplier for player scaling

`Base player amount`: Formulas work off of this as it is considered the _default_ value for how player scaling works.

#### Quota Settings

`Enable Quota Changes`: Enable quota changes? Once set to true open up a save to generate the rest of the config.

`Deadline Days Amount`: Deadline

`Starting credits`: Default is 60

`Starting quota`: Starting quota?

`Base Increase`: Quota scaling

`Increase Steepness`: Quota scaling

#### Moons Settings

`Moons to Not Spawn Events On` : Seperated by comma, the list of moons you do not want events to spawn on. Uses *newLevel.PlanetName* flag but other name flags are being looked into being added. Example: `41 Experimentation, 56 Vow` would prevent Experimentation and Vow from recieving events.

## Weather config
Location: `BrutalCompanyMinusExtraReborn\Weather_Settings.cfg`

`Enable Weather Multipliers`: Enable weather multipliers?

`Enable Terminal Text`: Enable terminal text?

`Randomize Weather Multipliers`: This will randomize **ScrapValue** and **ScrapAmount** multipliers for every weather after every day.

`Random Weather Multiplier Min Inclusive`: Lower bound of random value for **ScrapValue** and **ScrapAmount**.

`Random Weather Multiplier Max Inclusive`: Upper bound of random value for **ScrapValue** and **ScrapAmount**.

#### None

`Value Multiplier`:  Multiply scrap value by.

`Amount Multiplier`: Multiply scrap amount by.

**...**

#### Eclipsed

`Value Multiplier`:  Multiply scrap value by.

`Amount Multiplier`: Multiply scrap amount by.

## UI config
Location: `BrutalCompanyMinusExtraReborn\UI_Settings.cfg`

`UI Key`: They key used to toggle the UI.

`Normalize Scrap Value Display`: In game default scrap value multiplier is 0.4, having this enabled will multiply the display value by 2.5 to make it look normal.

`Enable UI`: Enable UI?

`Show UI Letter Box`: Show UI Letter box that contains the key?

`Show Extra Properties`: Show additional info in the UI that is **DaysPassed**, **ScrapValue**, **ScrapAmount**, **FactorySize**, **SpawnChance**, **SpawnCap** and **BonusEnemyHp**

`Pop Up UI`: Will the UI popup when landing on a new moon?

`UI Time`: The time the UI will appear for when popped up.

`Scroll speed`: Speed on scrolling on UI when scrolling using arrows on keyboard.

`Display UI after ship leaves?`: Display events only after ship leaves?

`Display extra properties on UI after ship leaves?`: Display extra properties such as eventType chances and difficulty after ship leaves?

`Display events`: Display events? or keep them hidden...
