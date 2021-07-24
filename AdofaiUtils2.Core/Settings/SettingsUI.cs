using System.Collections.Generic;
using UnityEngine;

namespace AdofaiUtils2.Core.Settings
{
    internal class SettingsUI : MonoBehaviour
    {
        public static bool Open;
        public static bool Escape;
        private Vector2 tabScrollPosition;
        private int _tabIndex;

        private string[] Keys
        {
            get
            {
                var keys = new List<string>();
                foreach (var v in SettingsManager.SettingsMap)
                {
                    var settings = v.Value;
                    keys.Add(settings.Id);
                }

                return keys.ToArray();
            }
        }

        private string[] Items
        {
            get
            {
                var items = new List<string>();
                foreach (var v in SettingsManager.SettingsMap)
                {
                    var settings = v.Value;
                    items.Add(settings.TabName);
                }

                return items.ToArray();
            }
        }


        private SettingsBase current
        {
            get
            {
                SettingsManager.SettingsMap.TryGetValue(Keys[_tabIndex] ?? "AdofaiUtils2.Core.CoreSettings",
                    out var guiSettings);
                return guiSettings;
            }
        }

        private void Update()
        {
            if (SettingsModule.Instance.Settings.settingsKey.Down() && !Open)
            {
                Open = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && Open)
            {
                Escape = true;
                Open = false;
                var guiSettings = current;
                if (guiSettings != null)
                {
                    guiSettings.Save();
                }
            }
        }
        
        private readonly float DesignWidth = 960.0f;
        private readonly float DesignHeight = 540.0f;

        private void OnGUI()
        {
            if (!Open) return;
            
            float resX = Screen.width / DesignWidth;
        
            float resY = Screen.height / DesignHeight;
            
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(resX, resY, 1));

            GUI.skin = Assets.GUISkin;


            var guiSettings = current;

            GUILayout.BeginArea(new Rect(50, 50, DesignWidth - 100, DesignHeight - 100),
                guiSettings == null ? "AdofaiUtils2 설정" : guiSettings.TabName,
                GUI.skin.window);
            _tabIndex = GUILayout.Toolbar(_tabIndex, Items);

            if (guiSettings == null)
            {
                GUILayout.Label("설정을 선택해주세요.");
            }
            else
            {
                guiSettings.OnGUI();
            }

            GUILayout.EndArea();
        }
    }
}