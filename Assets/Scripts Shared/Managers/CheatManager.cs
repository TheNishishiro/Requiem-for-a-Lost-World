using System;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
using Objects.Drops.ExpDrop;
using Objects.Players.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Managers
{
    public class CheatManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField input;
        [SerializeField] private TextMeshProUGUI console;
        [SerializeField] private List<GameObject> hideableUiElements;
        [SerializeField] private GameObject cheatConsole;
        private List<string> _logList = new ();
        
        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }
        
        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (_logList.Count > 50)
                _logList.Remove(_logList.Last());

            var color = type switch
            {
                LogType.Error => "red",
                LogType.Exception => "red",
                LogType.Warning => "orange",
                _ => ""
            };
            _logList.Insert(0,
                string.IsNullOrWhiteSpace(color)
                    ? $"{DateTime.Now:T} [{type}] {logString}"
                    : $"{DateTime.Now:T} [{type}] <color={color}>{logString}</color>");

            console.text = string.Join("<br>", _logList);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                cheatConsole.SetActive(!cheatConsole.activeSelf);
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteCommand(input.text);
                input.text = string.Empty;
            }
        }



        private Dictionary<string, Action> commands = new()
        {
            {
                "immortal", () =>
                {
                    GameManager.instance.playerStatsComponent.IsInvincible = true;
                    Debug.Log("Can die: false");
                }
            },
            {
                "mortal", () =>
                {
                    GameManager.instance.playerStatsComponent.IsInvincible = false;
                    Debug.Log("Can die: true");
                }
            },
            {
                "foliage_render_enable", () =>
                {
                    FindFirstObjectByType<Terrain>(FindObjectsInactive.Include).drawTreesAndFoliage = true;
                    Debug.Log("Foliage: on");
                }
            },
            {
                "foliage_render_disable", () =>
                {
                    FindFirstObjectByType<Terrain>(FindObjectsInactive.Include).drawTreesAndFoliage = false;
                    Debug.Log("Foliage: off");
                }
            },
            {
                "terrain_draw_disable", () =>
                {
                    FindFirstObjectByType<Terrain>(FindObjectsInactive.Include).enabled = false;
                    Debug.Log("Terrain draw: off");
                }
            },
            {
                "terrain_draw_enable", () =>
                {
                    FindFirstObjectByType<Terrain>(FindObjectsInactive.Include).enabled = true;
                    Debug.Log("Terrain draw: on");
                }
            },
            {
                "enemy_spawn_disable", () =>
                {
                    EnemyManager.instance.IsDisableEnemySpawn = true;
                    Debug.Log("Spawn enemies: off");
                }
            },
            {
                "enemy_spawn_enable", () =>
                {
                    EnemyManager.instance.IsDisableEnemySpawn = false;
                    Debug.Log("Spawn enemies: on");
                }
            },
            {
                "enemy_kill_all", () =>
                {
                    EnemyManager.instance.GlobalDamage(9999999, new ElementalWeapon(Element.Cosmic));
                    Debug.Log("Damage dealt to all enemies.");
                }
            },
            {
                "volume_disable", () =>
                {
                    FindAnyObjectByType<Volume>(FindObjectsInactive.Include).gameObject.SetActive(false);
                    Debug.Log("Shaders: off");
                }
            },
            {
                "volume_enable", () =>
                {
                    FindAnyObjectByType<Volume>(FindObjectsInactive.Include).gameObject.SetActive(true);
                    Debug.Log("Shaders: on");
                }
            },
            {
                "pickup_summon", () =>
                {
                    PickupManager.instance.SummonToPlayer();
                    Debug.Log("Summoned all pickups.");
                }
            },
            {
                "pickup_destroy_all", () =>
                {
                    PickupManager.instance.DestroyAll();
                    Debug.Log("Destroyed all pickups.");
                }
            },
            {
                "destructables_spawn_disable", () =>
                {
                    DestructablesManager.instance.IsSpawnDisabled = true;
                    Debug.Log("Spawn crates: off");
                }
            },
            {
                "destructables_spawn_enable", () =>
                {
                    DestructablesManager.instance.IsSpawnDisabled = false;
                    Debug.Log("Spawn crates: on");
                }
            },
        };
        
        private void ExecuteCommand(string command)
        {
            Debug.Log(command);
            if (commands.ContainsKey(command))
            {
                commands[command]();
                return;
            }
            
            switch (command)
            {
                case "help":
                    Debug.Log(string.Join(", ", commands.Keys.Select(x => $"<color=green>{x}</color>")));
                    return;
                case "ui_hide":
                    hideableUiElements.ForEach(x => x.gameObject.SetActive(false));
                    Debug.Log("UI: off");
                    return;
                case "ui_show":
                    hideableUiElements.ForEach(x => x.gameObject.SetActive(true));
                    Debug.Log("UI: on");
                    return;
            }

            if (command.StartsWith("player_add_exp"))
            {
                var amount = command.Split(' ');
                if (amount.Length < 2) return;
                
                GameManager.instance.playerComponent.AddExperience(int.Parse(amount.Last()));
                Debug.Log($"Added {amount.Last()} EXP to player");
                return;
            }
        }
        
    }
}