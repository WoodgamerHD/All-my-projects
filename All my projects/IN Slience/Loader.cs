
using System.Runtime.InteropServices;
using UnityEngine;


namespace InSilence
{
    public class Loader : MonoBehaviour
    {
      
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
         
            UnityEngine.Object.Destroy(Loader._loadObject);
            Loader._loadObject = null;

        }

       
    }
}
