using System.Runtime.InteropServices;
using UnityEngine;

namespace MuckHaxx2
{
    public class Loader
    {
        [DllImport("Dx11DrawingTools.dll", EntryPoint = "InitDx11")]
        public static extern void InitDx11();

        public static GameObject _loadObject;


        public static void Load()
        {
            _loadObject = new GameObject();

            _loadObject.AddComponent<Main>();




            Object.DontDestroyOnLoad(_loadObject);
        }

        public static void Unload()
        {
            _Unload();
        }

        public static void _Unload()
        {
            GameObject.Destroy(_loadObject);
        }

       
    }
}
