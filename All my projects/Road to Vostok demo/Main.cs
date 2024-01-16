using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace MuckHaxx2
{

    class Main : MonoBehaviour
    {



      //  private bool esp = false;
     //   private bool chams = false;
        private bool temp = false;
        private bool WeaponTest = false;
        private bool Aimbottest = false;
        private bool LoadConfigTest = false;

        private bool EntitysEsp = false;
        private bool radartest = false;
        private bool Chamsesp = false;
        private bool ItemsEsp = false;
        private bool giftboxesp = false;
        private bool NumericLockesp = false;
        private bool NumericPad3esp = false;
        private bool InteractablePropESP = false;
        private bool SafeESP = false;
        private bool DoorEsp = false;
        private bool RespawnDoorEsp = false;
        private bool PlayersEsp = false;
        private bool GhostPlayersEsp = false;
        private bool ElevatorEsp = false;
        private bool ExitZoneEsp = false;
        private bool infStamina = false;
        private bool Godmode = false;
        private bool Computeresp = false;
        private bool LetterLockesp = false;
        private bool LeverDoorLockesp = false;
        private bool Clockesp = false;
        private bool Boxtoggle = false;
        private bool Developertest = false;
        private bool Radar = false;

        public static float customNameR = 1f; // Initial red value
        public static float customNameG = 0f; // Initial green value
        public static float customNameB = 0f; // Initial blue value
                                //    public static List<PlayerController> Playerst = new List<PlayerController>();
        public static List<AI> EnemyBase = new List<AI>();
        public static List<Container> Container = new List<Container>();
        public static List<WeaponManager> Weapon = new List<WeaponManager>();
        public static List<EnemyHit> EnemyHit = new List<EnemyHit>();
        public static List<Barricade> Barricade = new List<Barricade>();    
        public static List<Developer> Developer = new List<Developer>();
        public static List<Character> Character = new List<Character>();


      
        float natNextUpdateTime;
        private static Material chamsMaterial;

        private string saveFilePath = "config.txt"; // File path to save the configuration


        private Color blackCol;
        private Color entityBoxCol;
        public static Camera cam;
        private static Material xray;
        private Rect windowRect = new Rect(0, 0, 400, 400); // Window position and size
   
        private Color backgroundColor = Color.black; // Background color
        private Rect RadarWindow;

        //   private bool showMenu = false;




        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
        public static void DoChams()
        {
            foreach (AI player in EnemyBase)
            {
                if (player == null)
                {
                    continue;
                }

                foreach (Renderer renderer in player?.gameObject?.GetComponentsInChildren<Renderer>())
                {
                    //renderer.material = chamsMaterial;
                    renderer.material = xray;
                }

                /*Highlighter h = player.GetOrAddComponent<Highlighter>();
                
                if (h) {
                    h.FlashingOff();
                    h.ConstantOnImmediate(Color.red);
                }*/
            }
        }

        public static List<Transform> GetAllBones(Animator a)
        {
            List<Transform> Bones = new List<Transform>
    {
        a.GetBoneTransform(HumanBodyBones.Head), // 0
        a.GetBoneTransform(HumanBodyBones.Neck), // 1
        a.GetBoneTransform(HumanBodyBones.Spine), // 2
        a.GetBoneTransform(HumanBodyBones.Chest), // 4
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

            // Add the missing bones
            for (int i = 56; i <= (int)HumanBodyBones.LastBone; i++)
            {
                HumanBodyBones bone = (HumanBodyBones)i;
                Bones.Add(a.GetBoneTransform(bone));
            }

            return Bones;
        }



        private Color selectedColor = Color.white;

        Color customNameColor = Color.cyan;


        private bool IsVisible(Vector3 position)
        {
            RaycastHit hit;
            Vector3 direction = position - Camera.main.transform.position;
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit))
            {
                if (hit.transform.position == position)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool is_visible(Vector3 position, Vector3 target)
        {
            var local = position;
            var direction = target - local;
            float dist = direction.magnitude * 0.9f;
            var ray = new Ray(local, direction);
            return Physics.Raycast(ray, dist, 999, QueryTriggerInteraction.UseGlobal);
        }

        private bool showMenu = true;
    


    
        private Rect menuRect = new Rect(10, 10, 460, 300); // Initial position and size of the menu
        private int selectedTab = 0;
        private string[] tabNames = { "Main", "Esp","Debug" };
        private Vector2 scrollPosition = Vector2.zero;


        public Color ColorPicker(Rect rect, Color color)
        {
            GUI.Box(rect, "");
            color.r = GUI.HorizontalSlider(new Rect(rect.x + 5f, rect.y + 5f, 100f, 25f), color.r, 0f, 1f);
            color.g = GUI.HorizontalSlider(new Rect(rect.x + 5f, rect.y + 30f, 100f, 25f), color.g, 0f, 1f);
            color.b = GUI.HorizontalSlider(new Rect(rect.x + 5f, rect.y + 55f, 100f, 25f), color.b, 0f, 1f);

            Color color1 = new Color(color.r * 255, color.g * 255, color.b * 255);
            Color color2 = Color.white;

            Color.RGBToHSV(color1, out color2.r, out color2.g, out color2.b);

            GUI.Label(new Rect(rect.x + 5f, rect.y + 70f, 250f, 100f), $"RGB: ({color1.r}, {color1.g}, {color1.b})");
        //    GUI.Label(new Rect(rect.x + 5f, rect.y + 85f, 250f, 100f), $"HSV: ({color2.r}, {color2.g}, {color2.b})");

            GUI.color = color;
            Texture2D texture2D = new Texture2D(20, 20);
            GUI.Label(new Rect(rect.x + 5f, rect.y + 100f, 250f, 100f), texture2D);
            GUI.color = Color.white;
            return color;
        }
        public Animator animator;
        public Color boneColor = Color.red;

        private bool showBones = false;
        private List<Transform> boneList = new List<Transform>();




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
                    // Content for tab 1
                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    Aimbottest = GUILayout.Toggle(Aimbottest, "Aimbottest");
                    WeaponTest = GUILayout.Toggle(WeaponTest, "WeaponTest");
                    Radar = GUILayout.Toggle(Radar, "Radar");


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    temp = GUILayout.Toggle(temp, "temp");
                    temp = GUILayout.Toggle(temp, "temp");
                    temp = GUILayout.Toggle(temp, "temp");

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    if (GUILayout.Button("test"))
                    {


                    }

                    if (GUILayout.Button("Gameobjects dumper"))
                    {

                        StreamWriter SW = new StreamWriter("Gameobjects.txt");
                        // Find all objects in the scene
                        GameObject[] objects = FindObjectsOfType<GameObject>();

                        // Loop through the objects and print their names to the console
                        foreach (GameObject obj in objects)
                        {
                            SW.WriteLine(obj.name);
                        }

                    }

                    break;

                case 1:
                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    EntitysEsp = GUILayout.Toggle(EntitysEsp, "Esp");
                    Chamsesp = GUILayout.Toggle(Chamsesp, "chams");
                    radartest = GUILayout.Toggle(radartest, "radartest");


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    temp = GUILayout.Toggle(temp, "temp");
                  
                    customNameColor = ColorPicker(new Rect(220f, 155f, 120f, 140f), customNameColor);
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    break;
                case 2:
                    if (animator == null)
                    {
                        GUILayout.Label("No Animator attached.");
                    }
                    else
                    {
                        GUILayout.Label("Bone Debug Viewer");

                        showBones = GUILayout.Toggle(showBones, "Show Bones");

                        if (showBones)
                        {
                            GUILayout.Label("Bone List:");
                            boneList = GetAllBones(animator); // You need to implement GetAllBones

                            foreach (Transform boneTransform in boneList)
                            {
                                GUILayout.Label(boneTransform.name);
                            }
                        }
                    }



                    break;

            }

            // Save and load config buttons
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
        private void UI(int pID)
        {
            foreach (Character Local in Character)
            {



                switch (pID)
                {
                    case 3:
                        var pos = new Vector2(4, 18);
                        var LocalCamera = cam;
                        var LocalPlayer = Local.transform;
                        var mapPosition = Vector2.one * 4f;
                        var mapSize = Vector2.one * 162f;
                        var mapCenter = pos + mapPosition + mapSize * 0.5f;
                        var maxDistance = 100;

                          ESPUtils.DrawCrosshair(pos + mapPosition + mapSize * 0.5f, 6, 1, Color.white);
                          ESPUtils.DrawDot(pos + mapPosition + mapSize * 0.5f, Color.green);

                        var World2DRadar = (mapSize * 0.5f).magnitude / maxDistance;

                        foreach (AI Enemy in EnemyBase)
                        {
                            


                            var delta = Enemy.transform.position - LocalPlayer.transform.position;
                            if (delta.magnitude > maxDistance)
                                delta = delta.normalized * maxDistance;
                            delta *= World2DRadar;
                            if (delta.magnitude > mapSize.x * 0.5f)
                                delta = delta.normalized * mapSize.x * 0.5f;
                            var length = delta.magnitude;
                            var angle = Mathf.Atan2(delta.z, delta.x) * Mathf.Rad2Deg + 90f;
                            var newAngle = (angle + LocalCamera.transform.rotation.eulerAngles.y) * -1f;
                            var newVector = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * newAngle), Mathf.Cos(Mathf.Deg2Rad * newAngle)) * length;

                            ESPUtils.DrawCrosshair(mapCenter + newVector,10,1, Color.red);

                            ESPUtils.DrawHealth(mapCenter + newVector, Enemy.health,0.5f, true);
                        }



                        break;
                    default:
                        break;
                }
            }
            GUI.DragWindow();
        }

        public void OnGUI()
        {
            if (Radar) {
                GUI.backgroundColor = Color.black;
                RadarWindow = GUI.Window(3, RadarWindow, new GUI.WindowFunction(UI), "2D Radar");
            }

            if (showMenu) // Only draw the menu when showMenu is true
            {


          

                // Create a draggable window with a black background
                    GUI.backgroundColor = Color.black;
                    menuRect = GUI.Window(0, menuRect, DrawMenu, "Movable Menu");

            }
           

            

       



            if (EntitysEsp)
            {
                foreach (Container player in Container)
                {
                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


                    if (ESPUtils.IsOnScreen(w2s) && !player.AIContainer)
                    {
                        
                        ESPUtils.DrawString(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f),
                       player.containerName + "\n" , Color.magenta, true, 12, FontStyle.Bold);
                    }
                }
           
                    foreach (AI player in EnemyBase)
                {

                  
                        Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);
                        if (ESPUtils.IsOnScreen(w2s) )
                        {
                         


                                DrawEnemyInfo(player);
                        }
                    
                
                    


                }
            }
        }

        public float boxSize = 100f; // Adjust the size of the GUI box as needed.








        public void DrawEnemyInfo(AI player)
        {
            Vector3 enemyBottom = player.transform.position;
            Vector3 enemyTop;
            enemyTop.x = player.transform.position.x;
            enemyTop.z = player.transform.position.z;
            enemyTop.y = player.transform.position.y + 2f;
            Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
            Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);
            Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);

            float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
            float x = w2s.x - height * 0.3f;
            float y = UnityEngine.Screen.height - worldToScreenTop.y;

            Color blackCol = Color.black;

                 if (is_visible(player.transform.position,player.hips.transform.position))
                 {

                     // Draw the box
                     ESPUtils.CornerBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
                     ESPUtils.CornerBox(new Vector2(x, y), new Vector2(height / 2f, height), Color.green);
                  //   ESPUtils.Draw2DBox(x, y, height / 2f, height, Color.green);
                     ESPUtils.CornerBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);
                 }
                 else
                 {
                     // Draw the box
                     ESPUtils.CornerBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
                     ESPUtils.CornerBox(new Vector2(x, y), new Vector2(height / 2f, height), Color.red);
             //   ESPUtils.Draw2DBox(x, y, height / 2f, height, Color.red);
                     ESPUtils.CornerBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);
                 }

      //      boneList = GetAllBones(animator); // You need to implement GetAllBones
            ESPUtils.DrawAllBones(GetAllBones(player.GetComponent<Animator>()), TestColor);
          


            //    Vector3 p = player.transform.position;
            //    Vector3 s = player.transform.localScale;
            //   if (p != null & s != null)
            //  ESPUtils.Draw3DBox(new Bounds(p + new Vector3(0, 1.1f, 0), s + new Vector3(0, .95f, 0)), Color.red);


            // Calculate the initial positions of the text
            Vector2 namePosition = new Vector2(x + (height / 4f), y - 14f);
            Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);

            // Offset the text positions based on the player's movement
            namePosition -= new Vector2(player.transform.position.x - player.transform.position.x, 0f);
            hpPosition -= new Vector2(player.transform.position.x - player.transform.position.x, 0f);

            float distance = Vector3.Distance(Camera.main.transform.position, player.transform.position);
            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);

            // Draw the text at the adjusted positions with the adjusted font size
            ESPUtils.DrawString(namePosition, player.name.Replace("(Clone)", ""), Color.red, true, fontSize, FontStyle.Bold);
            ESPUtils.DrawString(hpPosition, "HP: " + player.health, ESPUtils.GetHealthColour((float)player.health, 100), false, fontSize, FontStyle.Bold);

            // Draw the health bar
            float currentHealth = player.health;
            float maxHealth = 100;
            float percentage = (float)(currentHealth / maxHealth);
            float barHeight = height * percentage;
            Color barColour = ESPUtils.GetHealthColour((float)currentHealth, maxHealth);
            ESPUtils.RectFilled(x - 5f, y, 4f, height, blackCol);
            ESPUtils.RectFilled(x - 4f, y + height - barHeight - 1f, 2f, barHeight, barColour);
        }

      

        public void Start()
        {
            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;
            RadarWindow = new Rect(20f, 60f, 180f, 192f);

            chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy
            };

            chamsMaterial.SetInt("_SrcBlend", 5);
            chamsMaterial.SetInt("_DstBlend", 10);
            chamsMaterial.SetInt("_Cull", 0);
            chamsMaterial.SetInt("_ZTest", 8); // 8 = see through walls.
            chamsMaterial.SetInt("_ZWrite", 0);
            chamsMaterial.SetColor("_Color", Color.red);


            xray = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset
            };
            Color customNameColor = new Color(customNameR, customNameG, customNameB);
            xray.SetInt("_ZTest", 8);
          //  xray.SetColor("_Color", customNameColor);
            xray.SetInt("_SrcBlend", 5);
            xray.SetInt("_DstBlend", 10);


            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);
        }
        void SaveConfig()
        {
            string configText = EntitysEsp.ToString() + ","
                + Chamsesp.ToString() + ","
                + ItemsEsp.ToString() + ","
                + giftboxesp.ToString() + ","
                + NumericLockesp.ToString() + ","
                + NumericPad3esp.ToString() + ","
                + InteractablePropESP.ToString() + ","
                + SafeESP.ToString() + ","
                + DoorEsp.ToString() + ","
                + RespawnDoorEsp.ToString() + ","
                + PlayersEsp.ToString() + ","
                + GhostPlayersEsp.ToString() + ","
                + ElevatorEsp.ToString() + ","
                + ExitZoneEsp.ToString() + ","
                + infStamina.ToString() + ","
                + Godmode.ToString() + ","
                + Computeresp.ToString() + ","
                + LetterLockesp.ToString() + ","
                + LeverDoorLockesp.ToString() + ","
                + Clockesp.ToString() + ","
                + Boxtoggle.ToString() + ","
                + customNameR.ToString() + ","
                + customNameG.ToString() + ","
                + customNameB.ToString();

            File.WriteAllText(saveFilePath, configText);
            Debug.Log("Config saved.");
        }
        void LoadConfig()
        {
            if (File.Exists(saveFilePath))
            {
                string configText = File.ReadAllText(saveFilePath);
                string[] configValues = configText.Split(',');

                if (configValues.Length >= 24)
                {
                    EntitysEsp = bool.Parse(configValues[0]);
                    Chamsesp = bool.Parse(configValues[1]);
                    ItemsEsp = bool.Parse(configValues[2]);
                    giftboxesp = bool.Parse(configValues[3]);
                    NumericLockesp = bool.Parse(configValues[4]);
                    NumericPad3esp = bool.Parse(configValues[5]);
                    InteractablePropESP = bool.Parse(configValues[6]);
                    SafeESP = bool.Parse(configValues[7]);
                    DoorEsp = bool.Parse(configValues[8]);
                    RespawnDoorEsp = bool.Parse(configValues[9]);
                    PlayersEsp = bool.Parse(configValues[10]);
                    GhostPlayersEsp = bool.Parse(configValues[11]);
                    ElevatorEsp = bool.Parse(configValues[12]);
                    ExitZoneEsp = bool.Parse(configValues[13]);
                    infStamina = bool.Parse(configValues[14]);
                    Godmode = bool.Parse(configValues[15]);
                    Computeresp = bool.Parse(configValues[16]);
                    LetterLockesp = bool.Parse(configValues[17]);
                    LeverDoorLockesp = bool.Parse(configValues[18]);
                    Clockesp = bool.Parse(configValues[19]);
                    Boxtoggle = bool.Parse(configValues[20]);

                    customNameR = float.Parse(configValues[21]);
                    customNameG = float.Parse(configValues[22]);
                    customNameB = float.Parse(configValues[23]);    

                    Debug.Log("Config loaded.");
                }
            }
        }

   





        public void Update()
        {
          
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }

          

            if (WeaponTest)
            {
                foreach (WeaponManager player in Weapon)
                {
                 //    player.weaponData.impactForce = 999;
                    player.weaponData.magazineSize = float.MaxValue;
                    player.weaponData.horizontalRecoil = -100;
                    player.weaponData.verticalRecoil = -100;
                    player.weaponData.weaponKick = 0;
                    player.weaponData.rotationPower = 0;
                    player.weaponData.rotationRecovery = 0;
                    player.weaponData.kickRecovery = -100;
                    player.weaponData.kickPower = -100;
                }
            }
           
            if(Aimbottest)
            {
                foreach (EnemyHit player in EnemyHit)
                {
                    if (!IsVisible(player.transform.position) && Input.GetKeyDown(KeyCode.Mouse0))
                    {

                        player.Hit(100, 20, new Vector3(0, 0, 0), new Vector3(10,10));
                    }
                }


               


            }


                natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 1f)
            {


                EnemyBase = FindObjectsOfType<AI>().ToList();
                Weapon = FindObjectsOfType<WeaponManager>().ToList();
                EnemyHit = FindObjectsOfType<EnemyHit>().ToList();
                Barricade = FindObjectsOfType<Barricade>().ToList();
                Container = FindObjectsOfType<Container>().ToList();
                Developer = FindObjectsOfType<Developer>().ToList();
                Character = FindObjectsOfType<Character>().ToList();
          
                if(Chamsesp)
                {
                  //  customNameColor = new Color(customNameR, customNameG, customNameB);
                    xray.SetColor("_Color", customNameColor);
                    DoChams();
                }

                natNextUpdateTime = 0f;
            }

           
            
           
            cam = Camera.main;

        }
    }
}

