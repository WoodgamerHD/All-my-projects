using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace DeadFrontier2
{
    class Main : MonoBehaviour
    {

      
     

        //  bool SpeedHack = false;
        bool TransformMovement = false;
        bool OHK = false;
        bool Enemyesp = true;
        bool Containeresp = false;
        bool Playeresp = false;
        bool Chamsesp = false;
        bool MobVac = false;
        bool ConVac = false;
        bool ShowHealthbar = false;
     
     
        public static List<CF_11b55deb3b2d1afbc234b23007368c906b28fcd8_Corpsefuscated> ContainerBase = new List<CF_11b55deb3b2d1afbc234b23007368c906b28fcd8_Corpsefuscated>();
        public static List<CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated> EnemyBase = new List<CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated>();
        public static List<CF_1ee0a07ed45c00d253231d588e311bcec786dff4_Corpsefuscated> Playerst = new List<CF_1ee0a07ed45c00d253231d588e311bcec786dff4_Corpsefuscated>();
        public static List<CF_48732944307dd19cf8dee138c110abfcf457c204_Corpsefuscated> PlayerHealthbar = new List<CF_48732944307dd19cf8dee138c110abfcf457c204_Corpsefuscated>();
     
        float natNextUpdateTime;
        private static Material chamsMaterial;
        public static Camera cam;

      
        private bool showMenu = true; // Whether to show the menu or not

     
        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
        public static void DoChams()
        {
            foreach (CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated player in EnemyBase)
            {
                if (player == null)
                {
                    continue;
                }

                foreach (Renderer renderer in player?.gameObject?.GetComponentsInChildren<Renderer>())
                {
                    renderer.material = chamsMaterial;
                }
            }
        }
        // config
        void SaveConfig()
        {
            PlayerPrefs.SetInt("TransformMovement", TransformMovement ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("OHK", OHK ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("MobVac", MobVac ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("ConVac", ConVac ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("Playeresp", Playeresp ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("Enemyesp", Enemyesp ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("Containeresp", Containeresp ? 1 : 0); // so on with the etc
            PlayerPrefs.SetInt("Chamsesp", Chamsesp ? 1 : 0); // so on with the etc


            PlayerPrefs.Save();
            Debug.Log("Config saved.");
        }

        void LoadConfig()
        {
            if (PlayerPrefs.HasKey("TransformMovement"))
            {
                TransformMovement = PlayerPrefs.GetInt("TransformMovement") == 1; // so on with the etc
                OHK = PlayerPrefs.GetInt("OHK") == 1; // so on with the etc
                MobVac = PlayerPrefs.GetInt("MobVac") == 1; // so on with the etc
                ConVac = PlayerPrefs.GetInt("ConVac") == 1; // so on with the etc
                Playeresp = PlayerPrefs.GetInt("Playeresp") == 1; // so on with the etc
                Enemyesp = PlayerPrefs.GetInt("Enemyesp") == 1; // so on with the etc
                Containeresp = PlayerPrefs.GetInt("Containeresp") == 1; // so on with the etc
                Chamsesp = PlayerPrefs.GetInt("Chamsesp") == 1; // so on with the etc
            

                Debug.Log("Config loaded.");
            }
        }
        private Rect menuRect = new Rect(10, 10, 259, 200); // Initial position and size of the menu
        private int selectedTab = 0;
        private string[] tabNames = { "Main", "Esp"};

        //menu part
        private void DrawMenu(int windowID)
        {
            // Create toggle buttons for each tab
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Toggle(selectedTab == i, tabNames[i], "Button", GUILayout.ExpandWidth(true)))
                {
                    selectedTab = i; // Set the selected tab index
                }
            }
            GUILayout.EndHorizontal();

            // Display content for the selected tab
            switch (selectedTab)
            {
                case 0:
                 
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    TransformMovement = GUILayout.Toggle(TransformMovement, "Trans-Move");
                    OHK = GUILayout.Toggle(OHK, "1 HP Zombies");
                    MobVac = GUILayout.Toggle(MobVac, "Zombie Vac");


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    ConVac = GUILayout.Toggle(ConVac, "Cont Vac<Risky>");
                    ShowHealthbar = GUILayout.Toggle(ShowHealthbar, "Show HP");
                   
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                 

                    break;

                case 1:
                   
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    Enemyesp = GUILayout.Toggle(Enemyesp, "Zombies");
                    Playeresp = GUILayout.Toggle(Playeresp, "Players");
                    

                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    Containeresp = GUILayout.Toggle(Containeresp, "Container");
                    Chamsesp = GUILayout.Toggle(Chamsesp, "Chams");
                  

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    break;

             
            }

          
            GUILayout.BeginArea(new Rect(10, menuRect.height - 30, menuRect.width - 20, 40));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Config"))
            {
                SaveConfig();
            }

            if (GUILayout.Button("Load Config"))
            {
                LoadConfig();

            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUI.DragWindow(); // Allow the user to drag the window around
        }

        public static List<Transform> GetAllBones(Animator a)
        {
            List<Transform> Bones = new List<Transform>
            {
                a.GetBoneTransform(HumanBodyBones.Head), // 0
                a.GetBoneTransform(HumanBodyBones.Neck), // 1
                a.GetBoneTransform(HumanBodyBones.Spine), // 2
                a.GetBoneTransform(HumanBodyBones.Hips), // 3

                a.GetBoneTransform(HumanBodyBones.LeftShoulder), // 4
                a.GetBoneTransform(HumanBodyBones.LeftUpperArm), // 5
                a.GetBoneTransform(HumanBodyBones.LeftLowerArm), // 6
                a.GetBoneTransform(HumanBodyBones.LeftHand), // 7

                a.GetBoneTransform(HumanBodyBones.RightShoulder), // 8
                a.GetBoneTransform(HumanBodyBones.RightUpperArm), // 9
                a.GetBoneTransform(HumanBodyBones.RightLowerArm), // 10
                a.GetBoneTransform(HumanBodyBones.RightHand), // 11

                a.GetBoneTransform(HumanBodyBones.LeftUpperLeg), // 12
                a.GetBoneTransform(HumanBodyBones.LeftLowerLeg), // 13
                a.GetBoneTransform(HumanBodyBones.LeftFoot), // 14

                a.GetBoneTransform(HumanBodyBones.RightUpperLeg), // 15
                a.GetBoneTransform(HumanBodyBones.RightLowerLeg), // 16
                a.GetBoneTransform(HumanBodyBones.RightFoot) // 17
            };

            return Bones;
        }
        public static string GetLootContainerNames(string pText)
        {
            pText = pText.Replace("BarrelA_Barrel1 (1)3_Optimized", "Barrel").Replace("BarrelA_Barrel2 (1)2_Optimized", "Barrel").Replace("PowerboxA_Powerbox20_Optimized", "Powerbox").Replace("PowerboxA_Powerbox31_Optimized", "Powerbox").Replace("PowerboxA_Powerbox12_Optimized", "Powerbox").Replace("BarrelA_Barrel1 (1)3_Optimized", "Barrel").Replace("BarrelA_Barrel21_Optimized", "Barrel").Replace("BarrelA_Barrel10_Optimized", "Barrel").Replace("CarTruckA_Car1A0_Optimized", "Truck").Replace("CarTruckA_Car1B1_Optimized", "Truck").Replace("Cupboard_kitch_down1 (2)", "Kitchen Cupboard").Replace("Cupboard_kitch_down1 (5)", "Kitchen Cupboard").Replace("Cupboard_kitch_down1 (1)", "Kitchen Cupboard").Replace("Cupboard_kitch_down3 (3)", "Kitchen Cupboard").Replace("Cupboard_kitch_down1 (4)", "Kitchen Cupboard").Replace("Cupboard_kitch_down3 (2)", "Kitchen Cupboard").Replace("Cupboard_kitch_down3 (1)", "Kitchen Cupboard").Replace("CarBusA_Bus20_Optimized", "Bus").Replace("CarBusA_Bus11_Optimized", "Bus").Replace("Cupboard_kitch_up3 (2)", "Kitchen Cupboard").Replace("Cupboard_kitch_up2 (1)", "Kitchen Cupboard").Replace("Desk_large_Simple_Grid", "Desk Large").Replace("ZombieWoman_B_v01 (1)", "Zombie Woman").Replace("CarB_Car3B0_Optimized", "Car").Replace("CarB_Car3A1_Optimized", "Car").Replace("CarA_Car4B1_Optimized", "Car").Replace("Bathroom_Furniture_02", "Bathroom Furniture").Replace("Bathroom_Furniture_01", "Bathroom Furniture").Replace("ZombieMan_D_V1 (1)", "Zombie Man").Replace("Cupboard_kitch_down2", "Kitchen Cupboard").Replace("Cupboard_kitch_down3", "Kitchen Cupboard").Replace("CarA_Car40_Optimized", "Car").Replace("FemaleCorpseDecayed", "Female Corpse Decayed").Replace("ZombieWoman_A01 (1)", "Zombie Woman").Replace("ZombieWomanC_V1 (1)", "Zombie Woman").Replace("ZombieWomanD_V1 (1)", "Zombie Woman").Replace("Bed_Cabinet_Medical", "Bed Cabinet Medical").Replace("ZombieMan_C_V1 (1)", "Zombie Man").Replace("Cupboard_kitch_up1", "Kitchen Cupboard").Replace("Cupboard_kitch_up3", "Kitchen Cupboard").Replace("ZombieMan_D_V1 (1)", "Zombie Man").Replace("ZombieWoman_B_v01", "Zombie Woman").Replace("ZombieMan_B01 (1)", "Zombie Man").Replace("ZombieWoman_A01", "Zombie Woman").Replace("ZombieWomanC_V1", "Zombie Woman").Replace("ZombieWomanD_V1", "Zombie Woman").Replace("ZombieManA (1)", "Zombie Man").Replace("ZombieMan_C_V1", "Zombie Man").Replace("ZombieMan_D_V1", "Zombie Man").Replace("FemaleCorpseA", "Female Corpse").Replace("FemaleCorpseB", "Female Corpse").Replace("FemaleCorpseC", "Female Corpse").Replace("FemaleCorpseD", "Female Corpse").Replace("FemaleCorpseE", "Female Corpse").Replace("Cupboard_old1", "Old Cupboard").Replace("Cupboard_old2", "Old Cupboard").Replace("Cupboard_old3", "Old Cupboard").Replace("Cupboard_old4", "Old Cupboard").Replace("Cupboard_old5", "Old Cupboard").Replace("ZombieMan_B01", "Zombie Man").Replace("Sideboard_1", "Sideboard").Replace("Sideboard_2", "Sideboard").Replace("Sideboard_3", "Sideboard").Replace("MaleCorpseA", "Male Corpse").Replace("MaleCorpseB", "Male Corpse").Replace("MaleCorpseC", "Male Corpse").Replace("MaleCorpseD", "Male Corpse").Replace("MaleCorpseE", "Male Corpse").Replace("MaleCorpseF", "Male Corpse").Replace("MaleCorpseG", "Male Corpse").Replace("Wardrobe3_B", "Wardrobe").Replace("Wardrobe3_A", "Wardrobe").Replace("ZombieManA", "Zombie Man").Replace("ZombieManB", "Zombie Man").Replace("Desk_Large", "Desk Large").Replace("PowerboxA", "Powerbox").Replace("PowerboxB", "Powerbox").Replace("CarTruckA", "Truck").Replace("DumpsterA", "Dumpster").Replace("Cupboard1", "Cupboard").Replace("Wardrobe1", "Wardrobe").Replace("Wardrobe2", "Wardrobe").Replace("Cupboard2", "Cupboard").Replace("Cabinet_2", "Cabinet").Replace("TractorA", "Tractor").Replace("Cupboard", "Cupboard").Replace("CarBusA", "Bus").Replace("CarVanA", "Van").Replace("BarrelA", "Barrel").Replace("BarrelB", "Barrel").Replace("Cabinet", "Cabinet").Replace("Desk_1", "Desk").Replace("Desk_2", "Desk").Replace("Desk_3", "Desk").Replace("BinA", "Trash Bin").Replace("BinB", "Trash Bin").Replace("CarA", "Car").Replace("CarB", "Car").Replace("ShelfBase01", "Shelf").Replace("Enemy1", "Boss Loot").Replace("cardboard_boxes", "Cardboard Boxes B").Replace("cardboard_boxes_small", "Cardboard Boxes Small").Replace("cardboard_boxes_small_2", "Cardboard Boxes Small").Replace("Cupboard_Old_Paint1", "Cardboard Boxes Small");
            return new Regex("\\([\\d-]\\)").Replace(pText, string.Empty);
        }

        public void OnGUI()
        {
            if (showMenu) // Only draw the menu when showMenu is true
            {
                GUI.backgroundColor = Color.black; // background color
                menuRect = GUI.Window(0, menuRect, DrawMenu, "WoodgamerHD Small Menu"); // name of the menu
            }
            if (Enemyesp)
            {
                foreach (CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated player in EnemyBase)
                {



                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


                    Animator component2 = player.GetComponent<Animator>();
                    if (ESPUtils.IsOnScreen(w2s) && player.health > 0f)
                    {



                        if (Enemyesp)
                        {
                            ESPUtils.DrawAllBones(GetAllBones(component2), ESPUtils.GetHealthColour(player.health, 99999f));

                            
                            ESPUtils.DrawString(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f),
                             "Zombie" + "\n" + "HP: " + player.health + "\n", Color.red, true, 12, FontStyle.Bold);




                        }


                    }
                }
            }

            if (Playeresp)
            {
                foreach (CF_1ee0a07ed45c00d253231d588e311bcec786dff4_Corpsefuscated player in Playerst)
                {


                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);

                    Animator component2 = player.GetComponent<Animator>();
                    if (ESPUtils.IsOnScreen(w2s))
                    {



                        if (Playeresp)
                        {
                            ESPUtils.DrawAllBones(GetAllBones(component2), Color.green);

                           

                            ESPUtils.DrawString(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f),
                             player.characterName + "\n" + "CharID: " + player.characterID.ToString(), Color.green, true, 12, FontStyle.Bold);




                        }



                    }
                }
            }
            if (Containeresp)
            {
                foreach (CF_11b55deb3b2d1afbc234b23007368c906b28fcd8_Corpsefuscated player in ContainerBase)
                {
                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


                    if (ESPUtils.IsOnScreen(w2s) && player.requireLootAllowed)
                    {



                        if (Containeresp)
                        {

                            ESPUtils.DrawString(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f),
                             GetLootContainerNames(player.gameObject.name) + "\n", Color.cyan, true, 12, FontStyle.Bold);



                        }


                    }
                }

            }
              

            
        }


     

        public void Start()
        {
         
            chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy
            };


            chamsMaterial.SetInt("_ZTest", 8);
            chamsMaterial.SetColor("_Color", Color.red);
            chamsMaterial.SetInt("_SrcBlend", 5);
            chamsMaterial.SetInt("_DstBlend", 10);



        }
   
      
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert)) // Toggle the menu when the Tab key is pressed
            {
                showMenu = !showMenu;
            }

           

            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 2f)
            {

                ContainerBase = FindObjectsOfType<CF_11b55deb3b2d1afbc234b23007368c906b28fcd8_Corpsefuscated>().ToList();
                EnemyBase = FindObjectsOfType<CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated>().ToList();
                Playerst = FindObjectsOfType<CF_1ee0a07ed45c00d253231d588e311bcec786dff4_Corpsefuscated>().ToList();
                PlayerHealthbar = FindObjectsOfType<CF_48732944307dd19cf8dee138c110abfcf457c204_Corpsefuscated>().ToList();
              
                if (Chamsesp)
                {
                    DoChams();
                }
                natNextUpdateTime = 0f;
            }

            if(ShowHealthbar)
            {
                foreach (CF_48732944307dd19cf8dee138c110abfcf457c204_Corpsefuscated player in PlayerHealthbar)
                {
               //     player.ShowHealthBarNow();
                    player.compassArrow.gameObject.SetActive(true);
                    player.statusText.gameObject.SetActive(true);
                    player.characterNameTextTemplate.gameObject.SetActive(true);
                    player.interactionText.gameObject.SetActive(true);
                }


                }

            if (TransformMovement)
            {
              
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position += 1.25f * Camera.main.transform.forward;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position += new Vector3(0f, 5f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position += new Vector3(0f, -5f, 0f);
                }
                

            }
            if (OHK)
            {
                foreach (CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated player in EnemyBase)
                {
                   
                    if (player.health > 0f)
                    {
                        player.health = 0.1f;
                    }
                       
                }

            }
          
            if (MobVac)
            {
                foreach (CF_7172bbac62f6d9361a32e509c7ab9dd042a734ef_Corpsefuscated player in EnemyBase)
                {
                    player.transform.position = new Vector3(CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.x, CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.y, CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.z + 3f);
                }
            }
            if (ConVac)
            {
                foreach (CF_11b55deb3b2d1afbc234b23007368c906b28fcd8_Corpsefuscated player in ContainerBase)
                {
                    if (player.requireLootAllowed)
                    {
                        player.transform.position = new Vector3(CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.x, CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.y, CF_dc7ef78c177b1cd919b3fd315d1ad244b60362ee_Corpsefuscated.player.transform.position.z + 3f);
                    }
                }
            }
            cam = Camera.main;

        }
    }
}

