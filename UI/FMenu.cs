using FMenu.Utils;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace FMenu.UI
{
    class MenuSettings {
        public string LanguageSelected { get; set; }
        public string MenuVersion { get; set; }
        public string[] Developers { get; set; }
        public string[] BetaTesters { get; set; }

        public MenuSettings()
        {
            this.LanguageSelected = "en-US";
            this.MenuVersion = "0.1.7";
            this.Developers = new string[3] { "Daanbreur", "Subzay", "StuX" };
            this.BetaTesters = new string[3] { "RapierXbox", "Lautnix", "Keltusar" };
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
        Vector3 worldPosition;

        private Rect MainWindow;
        private Rect SubmenuWindow;

        private bool GuiEnabled = true;


        MenuItem MovementMenu = new MenuItem("> Movement");
        MenuItem FireworkspawnMenu = new MenuItem("> Spawn Special Fireworks");
        MenuItem BuildmenuMenu = new MenuItem("> Build Menu");
        MenuItem SettingsMenu = new MenuItem("> Settings");
        MenuItem AboutMenu = new MenuItem("> About FMenu");

        /// <summary>
        /// Selected build item
        /// </summary>
        public string BuildObject;
        public Transform objectToMove;


        private FireworkRespawner _Spawnfireworks;
        private MethodInfo spawnFireworksRaw = null;

        private int GetY(int n)
        { return 90 + n * 30; }
        private void Start()
        {
            MainWindow = new Rect(20f, 50f, 200f, 300f);
            SubmenuWindow = new Rect(230f, 50f, 200f, 230f);
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


            /// Tim spawn function
            if (_Spawnfireworks == null)
            {
                _Spawnfireworks = FindObjectOfType<FireworkRespawner>();
                if (_Spawnfireworks != null && spawnFireworksRaw == null)
                {
                    spawnFireworksRaw = _Spawnfireworks.GetType().GetMethod("RespawnFirework", BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                }
            }
            ///////////////////////

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
            ////////////////////////////
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
                BuildmenuToggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(2), 190, 30), BuildmenuTitle))
            {
                BuildmenuToggle = !BuildmenuToggle;
                AboutToggle = false;
                SpawnFireworkToggle = false;
            }

            if (GUI.Button(new Rect(25, GetY(4), 190, 30), AboutTitle))
            {
                AboutToggle = !AboutToggle;
                SpawnFireworkToggle = false;
                BuildmenuToggle = false;
            }

            if (SpawnFireworkToggle)
            {
                GUI.Box(SubmenuWindow, "Spawn Special Fireworks");
                if (GUI.Button(new Rect(235f, (float)GetY(0), 190f, 30f), SpawnTimTitle))
                    spawnFireworksRaw.Invoke(_Spawnfireworks, null);
            }

            if (BuildmenuToggle)
            {

                GUI.Box(SubmenuWindow, "Build Menu");
                GUI.Label(new Rect(235, 70, 200, 300), "Select an object and press X");
                GUI.Label(new Rect(235, 85, 200, 300), "Selected object: " + BuildObject);

                if (GUI.Button(new Rect(235f, (float)GetY(1), 190f, 30f), "Cube")) BuildObject = "Cube";
                if (GUI.Button(new Rect(235f, (float)GetY(2), 190f, 30f), "Plane")) BuildObject = "Plane";
                if (GUI.Button(new Rect(235f, (float)GetY(3), 190f, 30f), "Sphere")) BuildObject = "Sphere";
                if (GUI.Button(new Rect(235f, (float)GetY(4), 190f, 30f), "Capsule")) BuildObject = "Capsule";
                if (GUI.Button(new Rect(235f, (float)GetY(5), 190f, 30f), "Cylinder")) BuildObject = "Cylinder";
            }

            if (AboutToggle)
            {
                GUI.Box(SubmenuWindow, "About");
                GUI.Label(new Rect(235, 70, 200, 300), "Made By:\n• Daanbreur\n• Subzay\n• StuX\n\nTesters:\n• RapierXbox\n• Lautnix\n\nVersion: v0.1.0");
            }

        }
    }
}