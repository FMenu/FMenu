using FMenu.Utils;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FMenu.UI
{
    public class Menu : MonoBehaviour
    {
        Vector3 worldPosition;

        private Rect MainWindow;
        private Rect SubmenuWindow;

        private bool GuiEnabled = true;

        private string SpeedTitle = "Speed";
        public bool SpeedToggle;

        private string SpawnFireworkTitle = "> Spawn Special Fireworks";
        public bool SpawnFireworkToggle;

        private string SpawnTimTitle = "Tim";

        private string AboutTitle = "> About FMenu";
        public bool AboutToggle;

        private FireworkRespawner _Spawnfireworks;
        private MethodInfo spawnFireworksRaw = null;

        private int GetY(int n)
        { return 90 + n * 30; }
        private void Start()
        {
            MainWindow = new Rect(20f, 50f, 200f, 300f);
            SubmenuWindow = new Rect(230f, 50f, 200f, 200f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                GuiEnabled = !GuiEnabled;

            if (Input.GetKeyDown(KeyCode.Delete))
                Loader.Unload();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            if (_Spawnfireworks == null)
            {
                _Spawnfireworks = FindObjectOfType<FireworkRespawner>();
                if (_Spawnfireworks != null && spawnFireworksRaw == null)
                {
                    spawnFireworksRaw = _Spawnfireworks.GetType().GetMethod("RespawnFirework", BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                }
            }
        }

        private void OnGUI()
        {
            if (!GuiEnabled)
                return;

            GUI.Box(MainWindow, "FMenu");

            //if (GUI.Button(new Rect(25, GetY(0), 190, 30), SpeedTitle + (SpeedToggle ? " <color=green>ON</color>" : " <color=red>OFF</color>")))
            //    SpeedToggle = !SpeedToggle;

            if (GUI.Button(new Rect(25f, (float)GetY(1), 190f, 30f), SpawnFireworkTitle))
            {
                SpawnFireworkToggle = !SpawnFireworkToggle;
                AboutToggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(4), 190, 30), AboutTitle))
            {
                AboutToggle = !AboutToggle;
                SpawnFireworkToggle = false;
            }

            if (SpawnFireworkToggle)
            {
                GUI.Box(SubmenuWindow, "Spawn Special Fireworks");
                if (GUI.Button(new Rect(235f, (float)GetY(0), 190f, 30f), SpawnTimTitle))
                    spawnFireworksRaw.Invoke(_Spawnfireworks, null);
            }

            if (AboutToggle)
            {
                GUI.Box(SubmenuWindow, "About");
                GUI.Label(new Rect(235, 70, 200, 300), "Made By:\n• Daanbreur\n• Subzay\n• StuX\n\nTesters:\n• RapierXbox\n• Lautnix\n\nVersion: v0.1.0");
            }

        }
    }
}