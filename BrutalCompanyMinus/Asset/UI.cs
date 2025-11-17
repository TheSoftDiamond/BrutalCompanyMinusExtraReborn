using BrutalCompanyMinus.Minus;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using HarmonyLib;
using GameNetcodeStuff;
using UnityEngine.InputSystem.Controls;
using Unity.Netcode;
using System.Collections;
using System;

namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    public class UI : MonoBehaviour
    {
        public static UI Instance { get; private set; }
        public static GameObject eventUIObject { get; set; }

        public GameObject panelBackground, upArrowPanel, downArrowPanel;
        public TextMeshProUGUI panelText, letter, upArrow, downArrow;
        public Scrollbar panelScrollBar;

        public string key = "K";

        public KeyControl keyControl, upKeyControl, downKeyControl;

        public bool showCaseEvents = false;

        public float showCaseEventTime = 45.0f;
        public float curretShowCaseEventTime = 0.0f;

        public bool keyPressEnabledTyping = true, keyPressEnabledTerminal = true, keyPressEnabledSettings = true;

        public Keyboard keyboard;

        public static bool canClearText = true;

        // Color settings for the UI letter
        public static string colorHex = "00A000";

        public static float uiColorReduction = 0.6275f;

        public static Color letterColor = new Color(0.0f, 0.6275f, 0.0f);

        public static int hexColor = 0x00A000;

        // Arrow color settings

        public static string colorArrowHex = "00A000";

        public static Color arrowColor = new Color(0.0f, 0.6275f, 0.0f);

        public static float arrowColorActive = 255f/160f; // Inverse of 0.6275f

        public static int hexArrowColor = 0x00A000;

        // Text color

        public static string colorTextHex = "00FF00";

        // Menu Color

        //public static string menuColorHex = "000000";

        //public static float menuColorAlpha = 0.498f;

        //public static Color MenuColor = new Color(0.0f, 0.0f, 0.0f, 0.498f);

        //public static int hexMenuColor = 0x000000;

        //public static Image menuAsset;

        public void Start()
        {
            Instance = this;

            showCaseEventTime = Configuration.UITime.Value;
            Net.Instance.textUI.OnValueChanged += (previous, current) => panelText.text = current.ToString(); // For Text update

            // Try get hex color text from config
            try
            {
                colorTextHex = Configuration.colorText.Value.Replace("#", "");
            }
            catch
            {
                Log.LogWarning("Failed to get text color configuration, using default color.");
            }

            Component[] components = UI.eventUIObject.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                try
                {
                    switch (comp.name)
                    {
                        case "EventPanel":
                            if (panelBackground == null) panelBackground = comp.gameObject;
                            break;
                        case "EventText":
                            if (panelText == null) panelText = comp.GetComponent<TextMeshProUGUI>();
                            break;
                        case "Letter":
                            if (letter == null)
                            {
                                letter = comp.GetComponent<TextMeshProUGUI>();
                                key = Configuration.UIKey.Value.ToUpper();
                                try 
                                { 
                                    colorHex = Configuration.color.Value;
                                    uiColorReduction = Mathf.Max(Configuration.uiColorReduction.Value, 0);
                                    //Turn the string into hex color
                                    try
                                    {
                                        hexColor = Convert.ToInt32(colorHex.Replace("#", ""), 16);
                                        // Convert hex to Color

                                        letterColor = new Color(
                                            ((hexColor >> 16) & 0xFF) / 255f * uiColorReduction,
                                            ((hexColor >> 8) & 0xFF) / 255f * uiColorReduction,
                                            (hexColor & 0xFF) / 255f * uiColorReduction
                                        );

                                        letter.color = letterColor;

                                        Log.LogDebug("Set UI letter color to: " + colorHex);
                                    }
                                    catch 
                                    {
                                        letter.color = new Color(0.0f, 0.6275f, 0.0f);
                                        Log.LogWarning("Failed to parse color hex string, using default color.");
                                    }
                                }
                                catch 
                                {
                                    letter.color = new Color(0.0f, 0.6275f, 0.0f);
                                    Log.LogWarning("Failed to get color configurations correctly, using default color.");
                                }
                                letter.text = key;
                            }
                            break;
                        case "LetterPanel":
                            if (!Configuration.ShowUILetterBox.Value || !Configuration.EnableUI.Value) comp.gameObject.SetActive(false);
                            break;
                        case "Scrollbar":
                            if (panelScrollBar == null) panelScrollBar = comp.GetComponent<Scrollbar>();
                            break;
                        case "UpArrowPannel":
                            if (upArrowPanel == null) upArrowPanel = comp.gameObject;
                            break;
                        case "DownArrowPanel":
                            if (downArrowPanel == null) downArrowPanel = comp.gameObject;
                            break;
                        case "UpArrow":
                            if (upArrow == null) upArrow = comp.GetComponent<TextMeshProUGUI>();
                            try
                            {
                                colorArrowHex = Configuration.colorArrows.Value;
                                arrowColorActive = Configuration.colorArrowsIncrease.Value;
                                try
                                {
                                    hexArrowColor = Convert.ToInt32(colorArrowHex.Replace("#", ""), 16);
                                    // Convert hex to Color

                                    arrowColor = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f,
                                        (hexArrowColor & 0xFF) / 255f
                                    );

                                    upArrow.color = arrowColor;

                                    Log.LogDebug("Set UI Up Arrow color to: " + colorArrowHex);
                                }
                                catch
                                {
                                    upArrow.color = new Color(0.0f, 0.6275f, 0.0f);
                                    Log.LogWarning("Failed to parse color hex string, using default color.");
                                }
                            }
                            catch
                            {
                                Log.LogWarning("Failed to get arrow color configuration, using default color.");
                            }
                            break;
                        case "DownArrow":
                            if (downArrow == null) downArrow = comp.GetComponent<TextMeshProUGUI>();
                            try
                            {
                                colorArrowHex = Configuration.colorArrows.Value;
                                arrowColorActive = Mathf.Max(Configuration.colorArrowsIncrease.Value, 0);
                                try
                                {
                                    hexArrowColor = Convert.ToInt32(colorArrowHex.Replace("#", ""), 16);
                                    // Convert hex to Color

                                    arrowColor = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f,
                                        (hexArrowColor & 0xFF) / 255f
                                    );

                                    downArrow.color = arrowColor;

                                    Log.LogDebug("Set UI Down Arrow color to: " + colorArrowHex);
                                }
                                catch
                                {
                                    downArrow.color = new Color(0.0f, 0.6275f, 0.0f);
                                    Log.LogWarning("Failed to parse color hex string, using default color.");
                                }
                            }
                            catch
                            {
                                Log.LogWarning("Failed to get arrow color configuration, using default color.");
                            }
                            break;
                    }
                }
                catch
                {
                    Log.LogError("Failed to capture EventUI component/s.");
                }
            }

            keyboard = Keyboard.current;
            if (keyboard != null && Configuration.EnableUI.Value)
            {
                keyControl = keyboard.FindKeyOnCurrentKeyboardLayout(key);

                downKeyControl = keyboard.downArrowKey;
                upKeyControl = keyboard.upArrowKey;

                keyboard.onTextInput += OnKeyboardInput;
            }

            panelText.text = Manager.textUI.ToString();
        }

        void Update()
        {
            if (showCaseEvents)
            {
                curretShowCaseEventTime -= Time.deltaTime; // Decrement timer
                if (curretShowCaseEventTime <= showCaseEventTime * 0.6f) panelScrollBar.value -= (1 / (showCaseEventTime * 0.8f)) * Time.deltaTime * 2.0f;
                // End showcase events
                if (curretShowCaseEventTime < 0.0f)
                {
                    panelScrollBar.value = 1.0f; // Reset to top
                    showCaseEvents = false;
                    TogglePanel(false);
                }
            }

            if (panelBackground.activeSelf && downKeyControl != null && upKeyControl != null)
            {
                if (downKeyControl.isPressed)
                {
                    showCaseEvents = false;
                    try
                    {
                        downArrow.color = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f * arrowColorActive,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f * arrowColorActive,
                                        (hexArrowColor & 0xFF) / 255f * arrowColorActive
                                    );
                    }
                    catch
                    {
                        downArrow.color = new Color(0.0f, 1.0f, 0.0f);
                    }
                    panelScrollBar.value -= Time.deltaTime * Configuration.scrollSpeed.Value;
                }
                else
                {
                    try
                    {
                        downArrow.color = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f,
                                        (hexArrowColor & 0xFF) / 255f
                                    );
                    }
                    catch
                    {
                        downArrow.color = new Color(0.0f, 0.6f, 0.0f);
                    }
                }

                if (upKeyControl.isPressed)
                {
                    showCaseEvents = false;
                    try
                    {
                        upArrow.color = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f * arrowColorActive,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f * arrowColorActive,
                                        (hexArrowColor & 0xFF) / 255f * arrowColorActive
                                    );
                    }
                    catch
                    {
                        upArrow.color = new Color(0.0f, 1.0f, 0.0f);
                    }
                    panelScrollBar.value += Time.deltaTime * Configuration.scrollSpeed.Value;
                }
                else
                {
                    try
                    {
                        upArrow.color = new Color(
                                        ((hexArrowColor >> 16) & 0xFF) / 255f,
                                        ((hexArrowColor >> 8) & 0xFF) / 255f,
                                        (hexArrowColor & 0xFF) / 255f
                                    );
                    }
                    catch
                    {
                        upArrow.color = new Color(0.0f, 0.6f, 0.0f);
                    }
                }
            }
        }

        public static void SpawnObject()
        {
            if (eventUIObject != null) return;

            eventUIObject = (GameObject)Assets.bundle.LoadAsset("EventGUI");
            eventUIObject.AddComponent<UI>();

            eventUIObject = Instantiate(eventUIObject, Vector3.zero, Quaternion.identity);
        }

        public static void GenerateText(List<MEvent> events)
        {
            // Generate Text
            string text = $"<br><color=#{colorTextHex}>Events:</color><br>";
            foreach (string eventDescription in EventManager.currentEventDescriptions)
            {
                text += $"<color=#{colorTextHex}>-</color>{eventDescription}<br>";
            }

            // Extra properties
            if (Configuration.ShowExtraProperties.Value)
            {
                float ScrapValueMultiplier = RoundManager.Instance.scrapValueMultiplier * Manager.scrapValueMultiplier;
                if (Configuration.NormaliseScrapValueDisplay.Value) ScrapValueMultiplier *= 2.5f;

                text += GetDifficultyText();

                text += $"<br><br><color=#{colorTextHex}>Other:</color>";

                text +=
                    $"<br><color=#{colorTextHex}> -Scrap Value: x{ScrapValueMultiplier:F2}</color>" +
                    $"<br><color=#{colorTextHex}> -Scrap Amount: x{(RoundManager.Instance.scrapAmountMultiplier * Manager.scrapAmountMultiplier):F2}</color>" +
                    $"<br><color=#{colorTextHex}> -Factory Size: x{RoundManager.Instance.currentLevel.factorySizeMultiplier:F2}</color>" +
                    $"<br><color=#{colorTextHex}> -Spawn Chance: x{Manager.spawnChanceMultiplier:F2}</color>" +
                    $"<br><color=#{colorTextHex}> -Spawn Cap: x{Manager.spawncapMultipler:F2}</color>" +
                    $"<br><color=#{colorTextHex}> -Bonus enemy hp: {plusMinus(Manager.bonusEnemyHp)}</color>"
                    ;
            }

            Net.Instance.textUI.Value = new FixedString4096Bytes(text);
            if (Configuration.PopUpUI.Value && Configuration.EnableUI.Value) Net.Instance.ShowCaseEventsClientRpc();
        }

        private static string plusMinus(float value)
        {
            string s = value.ToString();
            if (value >= 0) s = "+" + s;
            return s;
        }

        private static string plusMinusExclusive(float value) => (value < 0) ? "" : "+";

        [ServerRpc(RequireOwnership = false)]
        private static void ClearTextServerRpc()
        {
            try
            {
                ClearText();
            }
            catch (Exception e)
            {
                Log.LogError("Failed to clear text: " + e.Message);
            }
        }

        public static void ClearText()
        {
            if (Configuration.DisplayExtraPropertiesAfterShipLeaves.Value)
            {
                string text = "";

                Manager.ComputeDifficultyValues();
                if (!Configuration.useCustomWeights.Value)
                {
                    EventManager.UpdateAllEventWeights();
                    text +=
                        $"<br><color=#{colorTextHex}>EventType Chances:</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#800000>VeryBad</color><color=#{colorTextHex}>:  {Helper.GetPercentage(EventManager.eventTypeRarities[0])}</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#FF0000>Bad</color><color=#{colorTextHex}>:      {Helper.GetPercentage(EventManager.eventTypeRarities[1])}</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#FFFFFF>Neutral</color><color=#{colorTextHex}>:  {Helper.GetPercentage(EventManager.eventTypeRarities[2])}</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#008000>Good</color><color=#{colorTextHex}>:     {Helper.GetPercentage(EventManager.eventTypeRarities[3])}</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#00FF00>VeryGood</color><color=#{colorTextHex}>: {Helper.GetPercentage(EventManager.eventTypeRarities[4])}</color>" +
                        $"<br> <color=#{colorTextHex}>-</color><color=#008000>Remove</color><color=#{colorTextHex}>:   {Helper.GetPercentage(EventManager.eventTypeRarities[5])}</color><br>";
                }

                text += GetDifficultyText();

                Net.Instance.textUI.Value = new FixedString4096Bytes(text);
            }
            else
            {
                Net.Instance.textUI.Value = new FixedString4096Bytes(" ");
            }
        }

        private static string GetDifficultyText()
        {
            string text =
                $"<br><color=#{colorTextHex}>Difficulty:</color> {Helper.GetDifficultyText()}" +
                $"<br><color=#{colorTextHex}> -Difficulty:</color>  <color=#{Helper.GetDifficultyColorHex(Manager.difficulty, Configuration.difficultyMaxCap.Value)}>{Manager.difficulty:F1}</color>";

            if (Configuration.scaleByDaysPassed.Value) text += $"<br><color=#{colorTextHex}> -Day:        </color><color=#{Helper.GetDifficultyColorHex(Manager.daysDifficulty, Configuration.daysPassedDifficultyCap.Value)}>{plusMinusExclusive(Manager.daysDifficulty)}{Manager.daysDifficulty:F1}</color>";
            if (Configuration.scaleByQuota.Value) text += $"<br><color=#{colorTextHex}> -Quota:      </color><color=#{Helper.GetDifficultyColorHex(Manager.quotaDifficulty, Configuration.quotaDifficultyCap.Value)}>{plusMinusExclusive(Manager.quotaDifficulty)}{Manager.quotaDifficulty:F1}</color>";
            if (Configuration.scaleByScrapInShip.Value) text += $"<br><color=#{colorTextHex}> -Ship Scrap: </color><color=#{Helper.GetDifficultyColorHex(Manager.scrapInShipDifficulty, Configuration.scrapInShipDifficultyCap.Value)}>{plusMinusExclusive(Manager.scrapInShipDifficulty)}{Manager.scrapInShipDifficulty:F1}</color>";
            if (Configuration.scaleByMoonGrade.Value) text += $"<br><color=#{colorTextHex}> -Moon risk:  </color><color=#{Helper.GetDifficultyColorHex(Manager.moonGradeDifficulty, Configuration.gradeAdditives["S+++"])}>{plusMinusExclusive(Manager.moonGradeDifficulty)}{Manager.moonGradeDifficulty:F1}</color>";
            if (Configuration.scaleByWeather.Value) text += $"<br><color=#{colorTextHex}> -Weather:    </color><color=#{Helper.GetDifficultyColorHex(Manager.weatherDifficulty, 7.0f)}>{plusMinusExclusive(Manager.weatherDifficulty)}{Manager.weatherDifficulty:F1}</color>";
            return text;
        }

        public void OnKeyboardInput(char input)
        {
            bool pressed = false;
            if (keyControl != null)
            {
                pressed = keyControl.isPressed;
            }
            else
            {
                pressed = (input.ToString().ToUpper() == key.ToUpper());
            }
            if (pressed && keyPressEnabledTyping && keyPressEnabledTerminal && keyPressEnabledSettings)
            {
                bool newState = !panelBackground.activeSelf;

                if (!newState && showCaseEvents)
                {
                    showCaseEvents = false;
                    panelScrollBar.value = 1.0f; // Reset to top
                }

                TogglePanel(newState);
            }
        }

        public void UnsubscribeFromKeyboardEvent()
        {
            if (Configuration.EnableUI.Value) keyboard.onTextInput -= OnKeyboardInput;
        }

        public void TogglePanel(bool state)
        {
            if (Configuration.DisplayExtraPropertiesAfterShipLeaves.Value && Net.Instance.textUI.Value.IsEmpty) ClearTextServerRpc();

            panelBackground.SetActive(state);
            upArrowPanel.SetActive(state);
            downArrowPanel.SetActive(state);
            try
            {
                letter.color = new Color(
                    state ? ((hexColor >> 16) & 0xFF) / 255f : ((hexColor >> 16) & 0xFF) / 255f * uiColorReduction,
                    state ? ((hexColor >> 8) & 0xFF) / 255f : ((hexColor >> 8) & 0xFF) / 255f * uiColorReduction,
                    state ? (hexColor & 0xFF) / 255f : (hexColor & 0xFF) / 255f * uiColorReduction
                );
            }
            catch
            {
                //Log.LogWarning("Failed to set letter color using hexColor. Falling back to default green.");
                letter.color = new Color(0.0f, state ? 1.0f : 0.6275f, 0.0f);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SetDiscordStatusDetails))]
        private static void OnChangeLevel(ref StartOfRound __instance)
        {
            if (!NetworkManager.Singleton.IsServer || !canClearText) return;
            try
            {
                ClearText();
            }
            catch (Exception e)
            {
                __instance.StartCoroutine(ClearAfterDelay());
            }
        }

        private static IEnumerator ClearAfterDelay()
        {
            yield return new WaitForSeconds(0.2f);
            try
            {
                ClearText();
            }
            catch (Exception e)
            {
                Log.LogError("Encountered an error in the ClearAfterDelay coroutine: " + e.Message);
            }
        }

        [HarmonyPrefix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(StartOfRound), "ShipLeave")]
        private static void OnShipLeave()
        {
            canClearText = true;
            if (!RoundManager.Instance.IsHost) return;

            if (!Configuration.DisplayUIAfterShipLeaves.Value)
            {
                ClearText();
                return;
            }

            if (Configuration.EnableUI.Value) GenerateText(EventManager.currentEvents);

            if (Configuration.showEventsInChat.Value)
            {
                HUDManager.Instance.AddTextToChatOnServer("<color=#FFFFFF>Events:</color>");
                foreach (string eventDescription in EventManager.currentEventDescriptions)
                {
                    HUDManager.Instance.AddTextToChatOnServer(eventDescription);
                }
            }
        }

        // Disable inputs when in terminal, in settings or is typing.

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Terminal), "Update")]
        private static void OnTerminalUpdate(ref bool ___terminalInUse)
        {
            try
            {
                Instance.keyPressEnabledTerminal = !___terminalInUse;
            }
            catch (Exception e)
            {
                Log.LogError("Failed to update keyPressEnabledTerminal: " + e.Message);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        public static void OnPlayerControllerBUpdate(ref QuickMenuManager ___quickMenuManager)
        {
            try
            {
                Instance.keyPressEnabledSettings = !___quickMenuManager.isMenuOpen;
            }
            catch (Exception e)
            {
                Log.LogError("Failed to update keyPressEnabledSettings: " + e.Message);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), "Update")]
        public static void OnUpdate(ref PlayerControllerB ___localPlayer)
        {
            try
            {
                Instance.keyPressEnabledTyping = !___localPlayer.isTypingChat;
            }
            catch (Exception e)
            {
                //Log.LogError("Failed to update keyPressEnabledTyping: " + e.Message);
            }
        }
    }
}