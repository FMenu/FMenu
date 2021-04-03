using UnityEngine;

namespace FMenu
{
    public class Loader
    {
        public static GameObject Load;

        public static void Init()
        {
            Debug.Log("Loading FMenu");
            Application.runInBackground = true;
            Loader.Load = new GameObject("FMenu");
            Loader.Load.transform.parent = null;
            Load.AddComponent<UI.Menu>();
            Object.DontDestroyOnLoad(Load);
        }

        public static void Unload()
        {
            Object.Destroy(Load);
        }
    }
}
