# Custom Events Documentation

The custom event system built into place is built to replace the Custom Monster Events. The custom event system supports Enemies, Items and Hazards (Though there may be actual hazards, BCMER treats nonhazard objects such as rocks even as a "hazard" at least code-wise). It is optional for what you want to spawn in.

<details>
   <summary>Object Behavior</summary>

Because we are technically manipulating the game in ways that the game may not support properly... enemies, hazards, and/or potentially items may not work as expected when put outside of the natural spawning mechanics. 
</details>

<details>
   <summary>Frequently Asked Questions</summary>

##### I added an event that spawns something, but the item/hazard/enemy didn't show up

- If you encountered an event where your item/hazard/enemy didn't spawn this could be for a few reasons:
   - The name of the thing you are attempting to spawn may be incorrectly spelled or the mod it uses is not enabled.
   - The thing you are attempting to spawn may not be able to spawn inside/outside.
   - If it is neither of the above, it may just be due to bad ingame luck... or _SoftDiamond made a bug_.
 
##### My event(s) does not show up in terminal or never occurs

- This could be due to a couple reasons:
   - The event may be disabled or the mod it requires in the config is disabled.
   - Malformed JSON text
   - Custom events, as a whole, are disabled in the CoreProperties.cfg
   - Other events are taking priority in the list.
 
##### Could I make an event that spawns every other event?

- Yes, there is nothing stopping you from making an event that spawns every other event too, though I'm not sure if the game will like that..

##### Events still show up in the old CustomEvents Config? Do I modify the events there?

- No. While custom event data shows up in the CustomEvents Config, this is only being done for networking purposes in the game. Anything modified in here will be cleared upon every launch of the game. A method is being looked into to see if it can be done without the need for this file.. but as of now, doing it this way prevents a softlock. Your custom event json files are the files you want to modify and won't be deleted or cleared by BCMER.
   
</details>

## Table of Contents

* [Top](#Custom-Events-Documentation)
* [Capabilities](#Capabilities)
* [Limitations](#Limitations)
* [Retrieving Asset Names](#Retrieving-Asset-Names)
* [Setting Up Events](#Setting-Up-Events)
    * [Getting Started](#Getting-Started)
    * [Creating a File](#Creating-a-File)


## Capabilities

The custom event system handles the following functionalities
- **Item Support**: Handles various item properties such as Rarity, Amount, Value, Transmutation Amount.
- **Enemy Support**: Manages enemy properties including Inside/Outside Minimum and Maximum Amount, Rarity.
- **Hazard Support**: Controls inside and outisde hazards with properties such as density, amount.
- **Event Chaining**: Supports spawning events in with other events at the same time, both official and custom.
- **Customization**: Customize the weight, description, mod requirements, color, event types, and name.

## Limitations

There are limitations however that the custom event cannot support due to how its created. If you are looking for an more advanced system, consider making a mod based on the [BCME-ExternalModule](https://thunderstore.io/c/lethal-company/p/UnloadedHangar/BCME_ExternalModule/)
- **Complex Logic**: The event system is not capable of events that require conditional triggers or complex logic.
- **Time-based events**: Cannot handle time-based events.
- **Weather support**: Does not currently support weather integration.

## Retrieving Asset Names
- **Enemies and Items**: You can use the `menemies` and `mitems` commands respectively in the terminal to retrieve their names.
- **Hazards**: Due to the way they are created for the game, you will have to look for the source code of the mod you are wanting to add events from to find what you are looking for. It varies from mod to mod.
- The list below a list of string names for official Lethal Company items, hazards, and enemies.
<details>
<summary>Enemies</summary>
  
- Centipede
- SandSpider
- HoarderBug
- Flowerman
- Crawler
- Blob
- DressGirl
- Puffer
- Nutcracker
- MouthDog
- ForestGiant
- SandWorm
- RedLocustBees
- BaboonHawk
- SpringMan
- Jester
- LassoMan
- MaskedPlayerEnemy
- Doublewing
- DocileLocustBees
- Butler
- RadMech
- FlowerSnake
- CaveDweller
</details>

<details>
<summary>Items</summary>

- Cog1
- EnginePart1
- FishTestProp
- MetalSheet
- FlashLaserPointer
- BigBolt
- BottleBin
- Ring
- SteeringWheel
- MoldPan
- EggBeater
- PickleJar
- DustPan
- Airhorn
- ClownHorn
- CashRegister
- Candy
- GoldBar
- YieldSign
- DiyFlashbang
- GiftBox
- Flask
- ToyCube
- Remote
- RobotToy
- MagnifyingGlass
- StopSign
- TeaKettle
- Mug
- SodaCanRed
- Phone
- Hairdryer
- Brush
- Bell
- WhoopieCushion
- ComedyMask
- TragedyMask
- RubberDuck
- ChemicalJug
- FancyLamp
- FancyPainting
- FancyCup
- Toothpaste
- PillBottle
- PerfumeBottle
- Dentures
- 7Ball
- EasterEgg
- ToyTrain
- ToiletPaperRolls
- SoccerBall
- PlasticCup
- GarbageLid
- ControlPad
- Clock
- ZedDog
  
</details>
<details>
<summary>Hazards</summary>
  
- LargeRock1
- LargeRock2
- LargeRock3
- LargeRock4
- GreyRockGrouping2
- GreyRockGrouping4
- GiantPumpkin
- tree
- treeLeaflessBrown.001 Variant
- treeLeafless.002_LOD0
- treeLeafless.003_LOD0
- Landmine
- TurretContainer
- SpikeRoofTrapHazard
  
</details>

## Setting Up Events

### Getting Started

Before getting started, navigate to `CoreProperties.cfg` and go to the _Enable Custom Events_ option, and make sure it is set to **True**. This will be required if one wants to use Custom Events.

### Creating a File

You will have to navigate to your `BrutalCompanyMinusExtraReborn\CustomEvents` directory inside your config folder and create a file that ends in the _.json_ filetype. For some variable

There are a few important points to note regarding the ranges, and how things work in the code.

<details>
<summary>How scale works</summary>

A lot of options in the config will contain a scale, a scale will contain a **Base**, **Increment**, **MinCap** and **MaxCap**.

The formula used to compute the scale is `Base + (DaysPassed * Increment)`.

MinCap is the value that the computed value wont go below.

MaxCap is the value that the computed value wont go above.

</details>

<details>
<summary>Ranges</summary>

Due to how the mod may represent percentages, or integers in different ways from scale to scale, and other variables, this section was created to help aleviate any confusions regarding how the code works.

### Item Related:

- **Rarity**: Must be a single integer from 0 to 100. If multiple items are in your mod, split this up amongst the other items. But should all stay under or equal to 100 altogether.
- **TransmuteAmount**: Normalized scale containing float numbers from 0 to 1, representing percentage of items to be transmutated.
- **ScrapAmount**: Scale that Represents a multiplier on the amount of scrap for the event.
- **ScrapValue**: Scale Represents a multiplier on the value of scrap for the event.

### Enemy Related

- **InsideRarity/OutsideRarity**: Scales using floats that represents the rarity of the enemy from 0 to 100.
- **MinInside/MinOutside**: Scales using floats represents the minimum amount of enemy to spawn in that location. Recommended to keep the Base, MInCap, MaxCap as integers.
- **MaxInside/Maxoutside**: Scales using floats represents the maximum amount of enemy to spawn in that location. Recommended to keep the Base, MInCap, MaxCap as integers.

### Hazard Related

- **MinDensity/MaxDensity**: Percentage of outside nodes to attempt to spawn a hazard prefab on.

</details>

#### Example

```json
[
    {
        "Name": "TestEvent",
        "Type": "VeryGood",
        "Weight": 50000,
        "Color": "#FF0000",
        "Descriptions": [
            "TEST EVENT",
            "EVENT TIME"
        ],
        "Enabled": true,
        "AddEventIfOnly": ["Event"],
        "Items": {
            "TransmuteAmount": [
                1,
                0,
                1,
                1
            ],
            "ScrapAmount": [
                2.4,
                0,
                2.4,
                2.4
            ],
            "ScrapValue": [
                100,
                0,
                100,
                100
            ],
            "Items": [
                {
                    "Name": "BigBolt",
                    "Rarity": 50
                },
                {
                    "Name": "BottleBin",
                    "Rarity": 50
                }
            ]
        },
        "Enemies": [
            {
                "Name": "Flowerman",
                "InsideRarity": [
                    100,
                    0.5,
                    100,
                    100
                ],
                "OutsideRarity": [
                    100,
                    0.5,
                    100,
                    100
                ],
                "MinInside": [
                    1,
                    0.015,
                    10,
                    20
                ],
                "MaxInside": [
                    31,
                    0.067,
                    32,
                    32
                ],
                "MinOutside": [
                    12,
                    0.015,
                    12,
                    22
                ],
                "MaxOutside": [
                    32,
                    0.067,
                    32,
                    43
                ]
            }
        ],
        "Hazards": [
            {
                "PrefabName": "Landmine",
                "Type": "Inside",
                "MinSpawn": [22, 1, 22, 23],
                "MaxSpawn": [33, 1, 33, 34],
                "SpawnOptions": {
                    "FacingAwayFromWall": false,
                    "FacingWall": true,
                    "BackToWall": false,
                    "BackFlushWithWall": false,
                    "RequireDistanceBetween": true,
                    "DisallowNearEntrances": false
                }
            },
            {
                "PrefabName": "SpikeRoofTrapHazard",
                "Type": "Outside",
                "MinDensity": [
                    0.005,
                    0.001,
                    0.005,
                    0.018
                ],
                "MaxDensity": [
                    0.021,
                    0.001,
                    0.021,
                    0.027
                ]
            }
        ],
        "EventsToRemove": [
            "BlackFriday"
        ],
        "EventsToSpawnWith": [
            "EarlyShip"
        ]
    }
]
```

#### Data Types

```json
[
  {
    "Name": "String",
    "Type": "String",
    "Weight": "Integer",
    "Color": "String",
    "Descriptions": [
      "String",
      "String"
    ],
    "Enabled": "Boolean",
    "AddEventIfOnly": [
      "String"
    ],
    "Items": {
      "TransmuteAmount": [
        "Float",
        "Float",
        "Float",
        "Float"
      ],
      "ScrapAmount": [
        "Float",
        "Float",
        "Float",
        "Float"
      ],
      "ScrapValue": [
        "Integer",
        "Integer",
        "Integer",
        "Integer"
      ],
      "Items": [
        {
          "Name": "String",
          "Rarity": "Integer"
        },
        {
          "Name": "String",
          "Rarity": "Integer"
        }
      ]
    },
    "Enemies": [
      {
        "Name": "String",
        "InsideRarity": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ],
        "OutsideRarity": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ],
        "MinInside": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ],
        "MaxInside": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ],
        "MinOutside": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ],
        "MaxOutside": [
          "Integer",
          "Float",
          "Integer",
          "Integer"
        ]
      }
    ],
    "Hazards": [
      {
        "PrefabName": "String",
        "Type": "String",
        "MinSpawn": [
          "Integer",
          "Integer",
          "Integer",
          "Integer"
        ],
        "MaxSpawn": [
          "Integer",
          "Integer",
          "Integer",
          "Integer"
        ],
        "SpawnOptions": {
          "FacingAwayFromWall": "Boolean",
          "FacingWall": "Boolean",
          "BackToWall": "Boolean",
          "BackFlushWithWall": "Boolean",
          "RequireDistanceBetween": "Boolean",
          "DisallowNearEntrances": "Boolean"
        }
      },
      {
        "PrefabName": "String",
        "Type": "String",
        "MinDensity": [
          "Float",
          "Float",
          "Float",
          "Float"
        ],
        "MaxDensity": [
          "Float",
          "Float",
          "Float",
          "Float"
        ]
      }
    ],
    "EventsToRemove": [
      "String"
    ],
    "EventsToSpawnWith": [
      "String"
    ]
  }
]
```


