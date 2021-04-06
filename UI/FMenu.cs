using FMenu.Utils;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using FMenu.Modules;
using FMenu.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Logger = FMenu.Utils.Logger;

namespace FMenu.UI
{
    public class MenuSettings {

        private RegistryKey settingsRegistry;

        public string LanguageSelected { get; set; }
        public string MenuVersion { get; set; }
        public string GameVersion { get; set; }
        public string[] Developers { get; set; }
        public string[] BetaTesters { get; set; }
        public string[] AvailableLanguages { get; set; }

        public MenuSettings()
        {
            this.settingsRegistry = Registry.CurrentUser.OpenSubKey("Software\\FMenu\\Settings", true);

            this.MenuVersion = "0.1.7";
            this.Developers = new string[3] { "Daanbreur", "Subzay", "StuX" };
            this.BetaTesters = new string[3] { "RapierXbox", "Lautnix", "Keltusar" };
            this.AvailableLanguages = new string[3] { "en-US", "de-DE", "es-ES" };
        }

        public void Update()
        {
        }

        public void Load()
        {
        }

    }

    class MenuItem
    {
        public string Title { get; set; }
        public bool Toggle { get; set; }

        public MenuItem()
        {
        }

        public MenuItem(string title)
        {
            this.Title = title;
            this.Toggle = false;
        }
    }

    public class Menu : MonoBehaviour
    {
        public static Logger logger = new Logger();

        Vector3 worldPosition;

        private Rect MainWindow;
        private Rect SubmenuWindow;

        private bool GuiEnabled = true;

        MenuSettings settings;

        MenuItem MovementMenu = new MenuItem($"> {strings.movement}");
        MenuItem ModsMenu = new MenuItem($"> {strings.mods}");
        MenuItem FireworkspawnMenu = new MenuItem($"> {strings.fireworksspawn}");
        MenuItem BuildmenuMenu = new MenuItem($"> {strings.buildmenu}");
        MenuItem SettingsMenu = new MenuItem($"> {strings.settings}");
        MenuItem AboutMenu = new MenuItem($"> {strings.about}");

        Dropdown languageDropdown;

        private VersionLabelMod versionLabelMod;
        private AutoclickerMod autoclickerMod;


        public string BuildObject;
        public Transform objectToMove;

        private FireworkRespawner _Spawnfireworks;
        private MethodInfo spawnFireworksRaw = null;

        //private VersionLabel _Version;
        //private TMPro.TextMeshProUGUI _versionRaw = null;

        private string DeveloperString = "";
        private string BetaTestersString = "";

        private int GetY(int n)
        { return 90 + n * 30; }

        private void Start()
        {
            MainWindow = new Rect(20f, 50f, 200f, 300f);
            SubmenuWindow = new Rect(230f, 50f, 200f, 300f);

            settings = new MenuSettings();
            settings.GameVersion = Application.version;

            foreach (string developer in settings.Developers) DeveloperString += $"• {developer}\n";
            foreach (string betatester in settings.BetaTesters) BetaTestersString += $"• {betatester}\n";

            languageDropdown = new Dropdown(new Rect(235f, (float)GetY(0), 190f, 60f), settings.AvailableLanguages, "Current Language");
            versionLabelMod = new VersionLabelMod();
            autoclickerMod = new AutoclickerMod();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                GuiEnabled = !GuiEnabled;
                Cursor.lockState = !GuiEnabled ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = GuiEnabled;
            }
                

            if (Input.GetKeyDown(KeyCode.Delete))
                Loader.Unload();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);


            /// Tim spawn function
            if (_Spawnfireworks == null)
            {
                _Spawnfireworks = FindObjectOfType<FireworkRespawner>();
                if (_Spawnfireworks != null && spawnFireworksRaw == null)
                {
                    spawnFireworksRaw = _Spawnfireworks.GetType().GetMethod("RespawnFirework", BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                }
            }

            /// Custom version
            versionLabelMod.Update(settings);
            autoclickerMod.Update();


            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;

            /// Default Unity object spawner 
            if (Input.GetKeyDown(KeyCode.X))
            {
                switch (BuildObject)
                {
                    case "Cube":

                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.position = hit.point;
                        }

                        break;
                    case "Plane":

                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {
                            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                            plane.transform.position = hit.point;
                        }
                        break;
                    case "Sphere":

                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {
                            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            sphere.transform.position = hit.point;
                        }
                        break;
                    case "Capsule":

                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {
                            GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                            capsule.transform.position = hit.point;
                        }
                        break;
                    case "Cylinder":
                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {
                            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                            cylinder.transform.position = hit.point;
                        }
                        break;
                }
            }
        }

        private void OnGUI()
        {
            if (!GuiEnabled)
                return;

            GUI.Box(MainWindow, "FMenu");

            if (GUI.Button(new Rect(25f, (float)GetY(0), 190f, 30f), MovementMenu.Title))
            {
                MovementMenu.Toggle = !MovementMenu.Toggle;
                ModsMenu.Toggle = false;
                FireworkspawnMenu.Toggle = false;
                BuildmenuMenu.Toggle = false;
                SettingsMenu.Toggle = false;
                AboutMenu.Toggle = false;
            }

            if (GUI.Button(new Rect(25f, (float)GetY(1), 190f, 30f), ModsMenu.Title))
            {
                MovementMenu.Toggle = false;
                ModsMenu.Toggle = !ModsMenu.Toggle;
                FireworkspawnMenu.Toggle = false;
                BuildmenuMenu.Toggle = false;
                SettingsMenu.Toggle = false;
                AboutMenu.Toggle = false;
            }

            if (GUI.Button(new Rect(25f, (float)GetY(2), 190f, 30f), FireworkspawnMenu.Title))
            {
                MovementMenu.Toggle = false;
                ModsMenu.Toggle = false;
                FireworkspawnMenu.Toggle = !FireworkspawnMenu.Toggle;
                BuildmenuMenu.Toggle = false;
                SettingsMenu.Toggle = false;
                AboutMenu.Toggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(3), 190, 30), BuildmenuMenu.Title))
            {
                MovementMenu.Toggle = false;
                ModsMenu.Toggle = false;
                FireworkspawnMenu.Toggle = false;
                BuildmenuMenu.Toggle = !BuildmenuMenu.Toggle;
                SettingsMenu.Toggle = false;
                AboutMenu.Toggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(5), 190, 30), SettingsMenu.Title))
            {
                MovementMenu.Toggle = false;
                ModsMenu.Toggle = false;
                FireworkspawnMenu.Toggle = false;
                BuildmenuMenu.Toggle = false;
                SettingsMenu.Toggle = !SettingsMenu.Toggle;
                AboutMenu.Toggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(6), 190, 30), AboutMenu.Title))
            {
                MovementMenu.Toggle = false;
                ModsMenu.Toggle = false;
                FireworkspawnMenu.Toggle = false;
                BuildmenuMenu.Toggle = false;
                SettingsMenu.Toggle = false;
                AboutMenu.Toggle = !AboutMenu.Toggle;
            }

            if (MovementMenu.Toggle)
            {
                GUI.Box(SubmenuWindow, "Movement");
                GUI.Label(new Rect(235f, (float)GetY(0), 190f, 30f), "In development");
                //if (GUI.Button(new Rect(235f, (float)GetY(0), 190f, 30f), "Flyhacks"))
            }

            if (ModsMenu.Toggle)
            {
                GUI.Box(SubmenuWindow, $"{strings.mods_header}");
                if (GUI.Button(new Rect(235f, (float) GetY(0), 190f, 30f), "Autoclicker" + (autoclickerMod.Active ? "Enabled" : "Disabled")))
                {
                    autoclickerMod.Active = !autoclickerMod.Active;
                }

                if (GUI.Button(new Rect(235f, (float)GetY(1), 190f / 2, 30f), autoclickerMod.LeftActive ? "<color=green>Left</color>" : "Left"))
                {
                    autoclickerMod.LeftActive = !autoclickerMod.LeftActive;
                    autoclickerMod.RightActive = false;
                }

                if (GUI.Button(new Rect(235f + (190f / 2), (float) GetY(1), 190f / 2, 30f), autoclickerMod.RightActive ? "<color=green>Right</color>" : "Right"))
                {
                    autoclickerMod.RightActive = !autoclickerMod.RightActive;
                    autoclickerMod.LeftActive = false;
                }

            }

            if (FireworkspawnMenu.Toggle)
            {
                GUI.Box(SubmenuWindow, $"{strings.fireworksspawn_header}");
                if (GUI.Button(new Rect(235f, (float)GetY(0), 190f, 30f), "Tim"))
                    spawnFireworksRaw.Invoke(_Spawnfireworks, null);
            }

            if (BuildmenuMenu.Toggle)
            {

                GUI.Box(SubmenuWindow, $"{strings.buildmenu_header}");
                GUI.Label(new Rect(235, 70, 200, 300), $"{strings.buildmenu_text1}");
                GUI.Label(new Rect(235, 85, 200, 300), $"{strings.buildmenu_text2} {BuildObject}");

                if (GUI.Button(new Rect(235f, (float)GetY(1), 190f, 30f), $"{strings.buildmenu_cube}")) BuildObject = "Cube";
                if (GUI.Button(new Rect(235f, (float)GetY(2), 190f, 30f), $"{strings.buildmenu_plane}")) BuildObject = "Plane";
                if (GUI.Button(new Rect(235f, (float)GetY(3), 190f, 30f), $"{strings.buildmenu_sphere}")) BuildObject = "Sphere";
                if (GUI.Button(new Rect(235f, (float)GetY(4), 190f, 30f), $"{strings.buildmenu_capsule}")) BuildObject = "Capsule";
                if (GUI.Button(new Rect(235f, (float)GetY(5), 190f, 30f), $"{strings.buildmenu_cylinder}")) BuildObject = "Cylinder";
            }

            if (SettingsMenu.Toggle)
            {
                GUI.Box(SubmenuWindow, "Settings");

                languageDropdown.Draw();
                //logger.log($"Selected Index: {languageDropdown.currentlySelectedIndex} | Option in array: {settings.AvailableLanguages[languageDropdown.currentlySelectedIndex]}");
                settings.LanguageSelected = settings.AvailableLanguages[languageDropdown.currentlySelectedIndex];
                //settings.Update();

                versionLabelMod.RainbowEnabled = GUI.Toggle(new Rect(235f, (float)GetY(3), 190f, 30f), versionLabelMod.RainbowEnabled, "Rainbow Version Label");
            }

            if (AboutMenu.Toggle)
            {
                GUI.Box(SubmenuWindow, "About");
                GUI.Label(new Rect(235, 70, 200, 300), $"{strings.about_developers}:\n{DeveloperString}\n{strings.about_testers}:\n{BetaTestersString}\n{strings.about_version}: v{settings.MenuVersion}");
            }

        }
    }
}