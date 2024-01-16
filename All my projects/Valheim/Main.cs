
using BepInEx;
using HarmonyLib;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MuckHaxx2
{
    [BepInPlugin("Test.ValheimMod", "Valheim Mod", "1.0.0")]
    [BepInProcess("valheim.exe")]


    class Main : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony("Test.ValheimMod");

        void Awake()
        {
            harmony.PatchAll();
        }



        // Token: 0x04000006 RID: 6
        public static bool aimbot = false;

        // Token: 0x04000007 RID: 7
        public static bool AutoAim = false;

        // Token: 0x04000008 RID: 8
        public static float AimDistance = 100f;

        // Token: 0x04000009 RID: 9
        public static int SmoothAimSpeed = 1;

        // Token: 0x0400000D RID: 13
        public static bool NoSpread = false;



        bool esp = true;
        bool ItemESP = true;
        bool BaseAI_esp = true;
        bool prefab_esp = true;
        bool Containeresp = true;
      
        //  bool SpeedHack = false;
        bool Godmode = false;
        bool CharVac = false;
        bool Walkonwater = false;
        bool Nocostbuild = false;
        bool ghostkill = false;
        bool debugFly = false;
     
        bool MenuYoggle;
        
        bool unlStamina = false;

        bool SetSleepingtest = false;
      
        bool AttackTest = false;
     

        private Color blackCol;
        private Color entityBoxCol;

        // Token: 0x0400001B RID: 27
        public Camera GameCam = Utils.GetMainCamera();
        public Camera _mCamera;


        // Token: 0x0400001F RID: 31
        private List<BaseAI> StoreAI = new List<BaseAI>();

        private List<Container> Container = new List<Container>();

        private List<Attack> AttackWeapon = new List<Attack>();

        private List<Pickable> Prefab = new List<Pickable>();


        private List<VisEquipment> VisEquip = new List<VisEquipment>();

        // Token: 0x04000029 RID: 41
        private static Rect MenuWindow = new Rect(60f, 250f, 300f, 230f);

        // Token: 0x04000020 RID: 32
        public List<Character> charEyePos = new List<Character>();

        private List<ZNetView> ZNetView_ = new List<ZNetView>();

        // Token: 0x0400001D RID: 29
        private List<Player> _Players = Player.GetAllPlayers();

        // Token: 0x04000758 RID: 1880
        public GameObject m_thorPrefab;

        // Token: 0x04000759 RID: 1881
        public float m_thorSpawnDistance = 300f;

        // Token: 0x0400075A RID: 1882
        public float m_thorSpawnAltitudeMax = 100f;

        // Token: 0x0400075B RID: 1883
        public float m_thorSpawnAltitudeMin = 100f;

        // Token: 0x0400075C RID: 1884
        public float m_thorInterval = 10f;

        // Token: 0x0400075D RID: 1885
        public float m_thorChance = 1f;
        // Token: 0x04000F12 RID: 3858
        private ZDO m_zdo;

        // Token: 0x040014BF RID: 5311
        public DropTable m_defaultItems = new DropTable();

        // Token: 0x040004D6 RID: 1238
        private  ZNetView m_nview;

        // Token: 0x04000021 RID: 33
        public static float c_jumpForce = 10f;

        // Token: 0x04000022 RID: 34
        public static float c_RunAndSwimSpeed = 10f;

        // Token: 0x04000023 RID: 35
        public static float c_interactDistance = 5f;

        // Token: 0x04000024 RID: 36
        public static float cam_FOV = 65f;

        // Token: 0x0400002B RID: 43
        private string prefabTextBox;

        private int counter;

        // Token: 0x0400001A RID: 26
        private Dictionary<int, GameObject> NamedPrefabs;

        IEnumerator SleepFor(float Time)
        {
            yield return new WaitForSeconds(Time);
        }
        private static Rect Teleporterr = new Rect(MenuWindow.x + 1550f, MenuWindow.y / 2f + 230f, 500f, 500f);


        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
        // Token: 0x04000013 RID: 19
        private GUIStyle LabelStyle = new GUIStyle();
        // Token: 0x04000016 RID: 22
        private bool BTeleporter = false;


        public float width = 1f;

        private Rect windowRect = new Rect(0, 0, 400, 400); // Window position and size


        private Inventory m_inventory = new Inventory("Inventory", null, 8, 4);

        private float DistanceFromCamera(Vector3 worldPos)
        {
            return Vector3.Distance(_mCamera.transform.position, worldPos);
        }


        private int tab = 0; // Current tab index
        private Color backgroundColor = Color.grey; // Background color
                                                    // Token: 0x04000456 RID: 1110
        public Color boxColor = Color.white;
        public float lineWidth = 0.02f;
        public bool drawDebugInfo = false;
        public bool drawBoundingBox = true;

        private Bounds bounds;
        private bool hasBounds = false;

        private void OnDrawGizmos()
        {
            if (drawBoundingBox && hasBounds)
            {
                Gizmos.color = boxColor;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(bounds.center, Vector3.one * lineWidth);

                if (drawDebugInfo)
                {
                    Debug.Log("Bounds center: " + bounds.center);
                    Debug.Log("Bounds size: " + bounds.size);
                }
            }
        }

        private void CalculateBounds()
        {
            bounds = new Bounds(charEyePos[0].transform.position, Vector3.zero);

            foreach (Character player in this.charEyePos)
            {
                bounds.Encapsulate(player.transform.position);
            }

            hasBounds = true;
        }



        public void OnGUI()
        {
          

            GUI.Label(new Rect(10, 200, 100, 20), Godmode ? " Godmode on" : " Godmode off");



            GUI.Label(new Rect(100, 200, 100, 20), GetLocalPlayer().GetPlayerModel() + "");

            if (MenuYoggle) // Only draw the menu when showMenu is true
            {
                // Set the background color
              

                windowRect = GUI.Window(0, windowRect, MenuWindow2, "Menu"); // Create the window with title "Menu"
            }
            if (BTeleporter)
            {
                Teleporterr = GUI.Window(1, Teleporterr, Teleporter, "Teleporter");
            }



            foreach (Player player in this._Players)
            {
                //    Debug.Log($"player");

                Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);

                Vector3 enemyBottom = player.transform.position;
                Vector3 enemyTop;
                enemyTop.x = enemyBottom.x;
                enemyTop.z = enemyBottom.z;
                enemyTop.y = enemyBottom.y + 2f;
                Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);

                if (player.IsDead())
                    return;

                if (ESPUtils.IsOnScreen(w2s))
                {
                    if (player /*!= this.GetLocalPlayer()*/)
                    {


                        float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
                        float x = w2s.x - height * 0.3f;
                        float y = Screen.height - worldToScreenTop.y;

                        if (esp)
                        {
                            ESPUtils.OutlineBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
                            ESPUtils.OutlineBox(new Vector2(x, y), new Vector2(height / 2f, height), entityBoxCol);
                            ESPUtils.OutlineBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);

                          //  ESPUtils.DrawAllBones(GetAllBones(player.transform), Color.green, 0);

                        }

                        if (esp)
                        {
                            ESPUtils.DrawString(new Vector2(w2s.x, Screen.height - w2s.y + 8f),
                            player.GetPlayerName() + "\n" + "HP " + player.GetHealth() + "\n" + "ID " + player.GetPlayerID(), TestColor, true, 12, FontStyle.Bold);
                        }
                      
                    
                            if (esp)
                        {

                            float currentHealth = player.GetHealth();

                            float maxHealth = player.GetMaxHealth();
                            float percentage = (float)(currentHealth / maxHealth);
                            float barHeight = height * percentage;

                            Color barColour = ESPUtils.GetHealthColour((float)currentHealth, maxHealth);

                            ESPUtils.RectFilled(x - 5f, y, 4f, height, blackCol);
                            ESPUtils.RectFilled(x - 4f, y + height - barHeight - 1f, 2f, barHeight, barColour);
                        }
                    }
              
                }
            }
            if (Containeresp)
            {


                foreach (Container player in this.Container)
                {

               
                    Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);

                    Vector3 enemyBottom = player.transform.position;
                    Vector3 enemyTop;
                    enemyTop.x = enemyBottom.x;
                    enemyTop.z = enemyBottom.z;
                    enemyTop.y = enemyBottom.y + 2f;
                    Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                    Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);



                    if (ESPUtils.IsOnScreen(w2s))
                    {

                        float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
                        float x = w2s.x - height * 0.3f;
                        float y = Screen.height - worldToScreenTop.y;


                        if (Containeresp)
                        {
                            ESPUtils.DrawString(new Vector2(w2s.x, Screen.height - w2s.y + 8f),
                            player.m_name + "\n" + player.GetInventory(), Color.green, true, 12, FontStyle.Bold);
                        }

                    }
                }

            }
            if (ItemESP)
            {
                foreach (Collider collider in Physics.OverlapSphere(this.GetLocalPlayer().transform.position + Vector3.up, 100f, LayerMask.GetMask(new string[]
                {
                    "item"
                })))
                {
                    if (collider.attachedRigidbody)
                    {
                        ItemDrop component = collider.attachedRigidbody.GetComponent<ItemDrop>();
                        if (!(component == null) && component.GetComponent<ZNetView>().IsValid())
                        {
                            Vector3 w2s2 = Camera.main.WorldToScreenPoint(component.gameObject.transform.position);

                            Vector3 enemyBottom2 = component.gameObject.transform.position;
                            Vector3 enemyTop2;
                            enemyTop2.x = enemyBottom2.x;
                            enemyTop2.z = enemyBottom2.z;
                            enemyTop2.y = enemyBottom2.y + 2f;
                            Vector3 worldToScreenBottom2 = Camera.main.WorldToScreenPoint(enemyBottom2);
                            Vector3 worldToScreenTop2 = Camera.main.WorldToScreenPoint(enemyTop2);



                            if (ESPUtils.IsOnScreen(w2s2))
                            {

                                float height2 = Mathf.Abs(worldToScreenTop2.y - worldToScreenBottom2.y);
                                float x2 = w2s2.x - height2 * 0.3f;
                                float y2 = Screen.height - worldToScreenTop2.y;

                                if (ItemESP)
                                {
                                    ESPUtils.DrawString(new Vector2(w2s2.x, Screen.height - w2s2.y + 8f),
                                   component.name.Replace("(Clone)", "") + "\n" + $"M:{DistanceFromCamera(component.transform.position).ToString("F1")}", Color.blue, true, 12, FontStyle.Bold);
                                }

                            }

                        }
                    }
                }
            }


          


                    if (BaseAI_esp)
            {

                foreach (Character player in this.charEyePos)
                {




                    //  Debug.Log($"BaseAI");

                  
                    if (player.IsPlayer())
                        return;



                    Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);

                    Vector3 enemyBottom = player.transform.position;
                    Vector3 enemyTop;
                    enemyTop.x = enemyBottom.x;
                    enemyTop.z = enemyBottom.z;
                    enemyTop.y = enemyBottom.y + 2f;
                    Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                    Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);

              

                    if (ESPUtils.IsOnScreen(w2s))
                    {

                  


                        if (BaseAI_esp)
                        {
                            

                            ESPUtils.DrawString(new Vector2(w2s.x, Screen.height - w2s.y + 8f),
                            player.name.Replace("(Clone)", "") + "\n" + "HP: " + player.GetHealth() + "\n" + $"M: {DistanceFromCamera(player.transform.position).ToString("F1")}", Color.red, true, 12, FontStyle.Bold);

                        }
                    }
                }
            }
            

          }

        // Token: 0x0600000A RID: 10 RVA: 0x000021AC File Offset: 0x000003AC
        public Dictionary<int, GameObject> GetPrefabList()
        {
            return GetPrivateFieldValue<Dictionary<int, GameObject>, ZNetScene>(ZNetScene.instance, "m_namedPrefabs");
        }
        public void MenuWindow2(int windowID)
        {
            GUILayout.BeginHorizontal();

            // Create toggle buttons for each tab
            GUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Toggle(tab == 0, "Main", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 0;
            }
            if (GUILayout.Toggle(tab == 1, "Esp", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 1;
            }
            if (GUILayout.Toggle(tab == 2, "Misc", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 2;
            }
            if (GUILayout.Toggle(tab == 3, "Random Shit", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 3;
            }
            if (GUILayout.Toggle(tab == 4, "Commands", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 4;
            }
            if (GUILayout.Toggle(tab == 5, "DevDebug", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 5;
            }
            GUILayout.EndVertical();

            // Display content for the selected tab

            GUILayout.BeginVertical();

            switch (tab)
            {
                case 0:
                   

                    if (GUILayout.Button(aimbot ? " aimbot on" : " aimbot off"))
                    {
                        aimbot = !aimbot;
                    }
                    if (GUILayout.Button(Nocostbuild ? " Nocostbuild on" : " Nocostbuild off"))
                    {
                        Nocostbuild = !Nocostbuild;
                    }
                    if (GUILayout.Button(unlStamina ? " unlStamina on" : " unlStamina off"))
                    {
                        unlStamina = !unlStamina;
                    }

                    if (GUILayout.Button(ghostkill ? " ghostkill on" : " ghostkill off"))
                    {
                        ghostkill = !ghostkill;
                    }
                    if (GUILayout.Button(Walkonwater ? " Walkonwater on" : " Walkonwater off"))
                    {
                        Walkonwater = !Walkonwater;
                    }
                    if (GUILayout.Button(debugFly ? " debugFly on" : " debugFly off"))
                    {
                        debugFly = !debugFly;
                    }



                    if (GUILayout.Button(Godmode ? " Godmode on" : " Godmode off"))
                    {
                        Godmode = !Godmode;
                    }
                    if (GUILayout.Button(SetSleepingtest ? " SetSleepingtest on" : " SetSleepingtest off"))
                    {
                        SetSleepingtest = !SetSleepingtest;
                    }
                    if (GUILayout.Button("Load Dev World"))
                    {
                        World.GetDevWorld();
                    }



                        break;
                case 1:
                   
                    // Draw a button for each action in Tab 2
                    if (GUILayout.Button(esp ? " Esp on" : " Esp off"))
                    {
                        esp = !esp;
                    }

                    if (GUILayout.Button(BaseAI_esp ? " BaseAI on" : " BaseAI off"))
                    {
                        BaseAI_esp = !BaseAI_esp;
                    }
                    if (GUILayout.Button(prefab_esp ? " prefab on" : " prefab off"))
                    {
                        prefab_esp = !prefab_esp;
                    }
                    if (GUILayout.Button(Containeresp ? " Container on" : " Container off"))
                    {
                        Containeresp = !Containeresp;
                    }
                    if (GUILayout.Button(ItemESP ? " ItemESP on" : " ItemESP off"))
                    {
                        ItemESP = !ItemESP;
                    }
                    if (GUILayout.Button("player model 1"))
                    {
                        foreach (Player player in this._Players)
                        {
                            player.SetPlayerModel(1);


                        }
                    }
                    if (GUILayout.Button("player model 2"))
                    {
                        foreach (Player player in this._Players)
                        {

                            player.SetPlayerModel(2);
                        }
                    }
                    if (GUILayout.Button("player model 3"))
                    {
                        foreach (Player player in this._Players)
                        {

                            player.SetPlayerModel(3);
                        }
                    }
                    if (GUILayout.Button("Shout"))
                    {
                        Chat.instance.SendText(Talker.Type.Shout, "Cunt");
                    }
                    if (GUILayout.Button("SpawnLoot"))
                    {
                        foreach (Ragdoll Player in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
                        {
                            Vector3 averageBodyPosition = Player.GetAverageBodyPosition();
                            SpawnLoot(averageBodyPosition);

                        }
                    }
                    break;
                case 2:
                    // Content for tab 2
                    if (GUILayout.Button("PlayerList"))
                    {
                        BTeleporter = !BTeleporter;
                    }
                    if (GUILayout.Button("PING"))
                    {
                        ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", new object[]
            {
                new Vector3(0,0,0),
                3,
                "N-Word",
                "",
                PrivilegeManager.GetNetworkUserId()
            });

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
                    if (GUILayout.Button("materials dumper"))
                    {

                        DumpLoadedMaterials();
                    }


                    if (GUILayout.Button("Dump Prefabs"))
                    {
                        DumpPrefabs();
                    }
                  
                    if (GUILayout.Button("ClearAll"))
                    {

                        ClutterSystem.instance.ClearAll();
                    }


                        break;
                case 3:


                    GUI.Label(new Rect(153f, 30f, 130f, 20f), new GUIContent("Run/Swim Speed"));
                    c_RunAndSwimSpeed = GUI.HorizontalSlider(new Rect(153f, 55f, 128f, 20f), c_RunAndSwimSpeed, 5f, 60f);



                    GUI.Label(new Rect(153f, 60f, 130f, 20f), new GUIContent("Change Jump Height", "Turn on Godmode"));
                    c_jumpForce = GUI.HorizontalSlider(new Rect(153f, 85f, 128f, 20f), c_jumpForce, 5f, 60f);



                    GUI.Label(new Rect(153f, 100f, 130f, 20f), new GUIContent("Interact distance", "Interact from faaar away"));
                    c_interactDistance = GUI.HorizontalSlider(new Rect(153f, 120f, 128f, 20f), c_interactDistance, 5f, 40f);


                    GUI.Label(new Rect(153f, 130f, 130f, 20f), new GUIContent("Change FOV (20-120)", "Change your field of view"));
                    cam_FOV = GUI.HorizontalSlider(new Rect(153f, 150f, 128f, 20f), cam_FOV, 20f, 120f);

                    GUI.Label(new Rect(155f, 264f, 50f, 23f), "Prefab:");
                    prefabTextBox = GUI.TextField(new Rect(155f, 290f, 127f, 23f), prefabTextBox);
                    if (GUI.Button(new Rect(155f, 319f, 70f, 23f), new GUIContent("Spawn 1x", "Spawn the item just once")) && prefabTextBox != null)
                    {
                        try
                        {
                            this.GiveItem(prefabTextBox);
                        }
                        catch
                        {
                            this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Prefab not found. Better use Copy&Paste", 0, null);
                        }
                    }

                    break;
                case 4:



                    if (GUILayout.Button("Skip to Morning"))
                    {
                        skipToMorning();

                    }
                    if (GUILayout.Button("all wave"))
                    {
                        foreach (Player player in this._Players)
                        {
                            player.StartEmote("wave", true);
                        }

                    }
                    if (GUILayout.Button("CreateTombStone"))
                    {
                        foreach (Player player in this._Players)
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(player.m_tombstone, player.GetCenterPoint(), base.transform.rotation);
                            gameObject.GetComponent<Container>().GetInventory().MoveInventoryToGrave(m_inventory);
                            TombStone component = gameObject.GetComponent<TombStone>();
                            PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
                            component.Setup(playerProfile.GetName(), playerProfile.GetPlayerID());
                        }
                    }

                        if (GUILayout.Button("optterrain"))
                    {
                        TerrainComp.UpgradeTerrain();

                    }
                    if (GUILayout.Button("genloc"))
                    {
                        ZoneSystem.instance.GenerateLocations();

                    }
                    if (GUILayout.Button("Remove birds"))
                    {
                        int num = 0;
                        RandomFlyingBird[] array = UnityEngine.Object.FindObjectsOfType<RandomFlyingBird>();
                        for (int i = 0; i < array.Length; i++)
                        {
                            ZNetView component = array[i].GetComponent<ZNetView>();
                            if (component && component.IsValid() && component.IsOwner())
                            {
                                component.Destroy();
                                num++;
                            }
                        }
                        Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Removed birds: " + num.ToString(), 0, null);

                    }



                    if (GUILayout.Button("projectile_beam"))
                    {
                        this.GiveItem("projectile_beam", new Vector3(GetLocalPlayer().transform.position.x, GetLocalPlayer().transform.position.y + 5f, GetLocalPlayer().transform.position.z));
                    }
                    if (GUILayout.Button("ExploreAll"))
                    {
                        Minimap.instance.ExploreAll();
                    }


                    if (GUILayout.Button("Message"))
                    {
                        foreach (Player player in this._Players)
                        {
                          
                            // Loop through the objects and print their names to the console

                            //  BaseAI.DoProjectileHitNoise(base.transform.position, 9999, GetLocalPlayer());

                            player.Message(MessageHud.MessageType.Center, "KIllerBee Likes FemBoy", 10, null);


                            //    player.GetSEMan().AddStatusEffect(this.m_lootStatusEffect.name, true, 0, 0f);

                            //    player.SetPlayerModel(1);
                            //        player.AddNoise(88);
                        

                            player.m_maxCarryWeight = float.MaxValue;


                            player.SetSkinColor(new Vector3(10, 0, 10));
                            player.SetHairColor(new Vector3(10, 0, 10));


                        }


                    }
                    if (GUILayout.Button("Change Env"))
                    {
                        switch (this.counter)
                        {
                            case 0:
                                this.setEnv("Clear");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Clear", 0, null);
                                this.counter++;
                                break;
                            case 1:
                                this.setEnv("Twilight_Clear");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Twilight_Clear", 0, null);
                                this.counter++;
                                break;
                            case 2:
                                this.setEnv("Misty");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Misty", 0, null);
                                this.counter++;
                                break;
                            case 3:
                                this.setEnv("Darklands_dark");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Darklands_dark", 0, null);
                                this.counter++;
                                break;
                            case 4:
                                this.setEnv("Heath clear");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Heath clear", 0, null);
                                this.counter++;
                                break;
                            case 5:
                                this.setEnv("DeepForest Mist");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: DeepForest Mist", 0, null);
                                this.counter++;
                                break;
                            case 6:
                                this.setEnv("GDKing");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: GDKing", 0, null);
                                this.counter++;
                                break;
                            case 7:
                                this.setEnv("Rain");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Rain", 0, null);
                                this.counter++;
                                break;
                            case 8:
                                this.setEnv("LightRain");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: LightRain", 0, null);
                                this.counter++;
                                break;
                            case 9:
                                this.setEnv("ThunderStorm");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: ThunderStorm", 0, null);
                                this.counter++;
                                break;
                            case 10:
                                this.setEnv("Eikthyr");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Eikthyr", 0, null);
                                this.counter++;
                                break;
                            case 11:
                                this.setEnv("GoblinKing");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: GoblinKing", 0, null);
                                this.counter++;
                                break;
                            case 12:
                                this.setEnv("nofogts");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: nofogts", 0, null);
                                this.counter++;
                                break;
                            case 13:
                                this.setEnv("SwampRain");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: SwampRain", 0, null);
                                this.counter++;
                                break;
                            case 14:
                                this.setEnv("Bonemass");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Bonemass", 0, null);
                                this.counter++;
                                break;
                            case 15:
                                this.setEnv("Snow");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Snow", 0, null);
                                this.counter++;
                                break;
                            case 16:
                                this.setEnv("Twilight_Snow");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Twilight_Snow", 0, null);
                                this.counter++;
                                break;
                            case 17:
                                this.setEnv("Twilight_SnowStorm");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Twilight_SnowStorm", 0, null);
                                this.counter++;
                                break;
                            case 18:
                                this.setEnv("SnowStorm");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: SnowStorm", 0, null);
                                this.counter++;
                                break;
                            case 19:
                                this.setEnv("Moder");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Moder", 0, null);
                                this.counter++;
                                break;
                            case 20:
                                this.setEnv("Ashrain");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Ashrain", 0, null);
                                this.counter++;
                                break;
                            case 21:
                                this.setEnv("Crypt");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: Crypt", 0, null);
                                this.counter++;
                                break;
                            case 22:
                                this.setEnv("SunkenCrypt");
                                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Changed env to: SunkenCrypt", 0, null);
                                this.counter = 0;
                                break;
                        }
                    }

                    break;
                case 5:
                    if (GUILayout.Button("Munin"))
                    {
                        //Munin
                        this.GiveItem("Munin", new Vector3(GetLocalPlayer().transform.position.x, GetLocalPlayer().transform.position.y + 1f, GetLocalPlayer().transform.position.z));

                    }

                    break;
             
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow(); // Allow the user to drag the window around
        }


        public string filePath = "Materials.txt";

        public void DumpLoadedMaterials()
        {
            Dictionary<GameObject, List<Material>> objectMaterials = new Dictionary<GameObject, List<Material>>();

         
            GameObject[] objects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in objects)
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    if (!objectMaterials.ContainsKey(obj))
                    {
                        objectMaterials[obj] = new List<Material>();
                    }

                    Material[] materials = renderer.sharedMaterials;
                    foreach (Material material in materials)
                    {
                        if (!objectMaterials[obj].Contains(material))
                        {
                            objectMaterials[obj].Add(material);
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Loaded materials:");

                foreach (KeyValuePair<GameObject, List<Material>> pair in objectMaterials)
                {
                    if (pair.Value.Count > 0)
                    {
                        //   writer.WriteLine($"{pair.Key.name} : ");
                        foreach (Material material in pair.Value)
                        {
                            writer.WriteLine($"GameObject<{pair.Key.name}> is Using material <{material.name}>");
                        }
                    }
                }
            }

            Debug.Log($"Materials list dumped to file: {filePath}");
        }


        public void SpawnLoot(Vector3 center)
        {
            ZDO zdo = this.m_nview.GetZDO();
            int @int = zdo.GetInt("drops", 0);
            if (@int <= 0)
            {
                return;
            }
            List<KeyValuePair<GameObject, int>> list = new List<KeyValuePair<GameObject, int>>();
            for (int i = 0; i < @int; i++)
            {
                int int2 = zdo.GetInt("drop_hash" + i.ToString(), 0);
                int int3 = zdo.GetInt("drop_amount" + i.ToString(), 0);
                GameObject prefab = ZNetScene.instance.GetPrefab(int2);
                if (prefab == null)
                {
                    ZLog.LogWarning("Ragdoll: Missing prefab:" + int2.ToString() + " when dropping loot");
                }
                else
                {
                    list.Add(new KeyValuePair<GameObject, int>(prefab, int3));
                }
            }
            CharacterDrop.DropItems(list, center + Vector3.up * 0.75f, 0.5f);
        }



        // Token: 0x0600000B RID: 11 RVA: 0x000021C0 File Offset: 0x000003C0
        public void DumpPrefabs()
        {
            this.NamedPrefabs = this.GetPrefabList();
            if (this.NamedPrefabs != null)
            {
                DateTime now = DateTime.Now;
                FileStream fileStream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Prefabs Dump.txt", FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(string.Format("Dump", "0", DateTime.Now));
                int num = 0;
                foreach (KeyValuePair<int, GameObject> keyValuePair in this.NamedPrefabs)
                {
                    streamWriter.WriteLine(keyValuePair.Key.ToString() + "\t" + keyValuePair.Value.name);
                    num++;
                }
                string value = string.Format("Dumped {0} prefab in {1} ms", num, (DateTime.Now - now).TotalMilliseconds);
                streamWriter.WriteLine(value);
                this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "Prefabs dumped and saved to Desktop", 0, null);
                streamWriter.Close();
                fileStream.Close();
                return;
            }
            this.GetLocalPlayer().Message(MessageHud.MessageType.Center, "ERROR dumping prefabs", 0, null);
        }
        public void GiveItem(string g_prefab)
        {
            if (this.GetLocalPlayer() != null)
            {
                GameObject prefab = ZNetScene.instance.GetPrefab(g_prefab);
                if (prefab != null)
                {
                    try
                    {
                        UnityEngine.Object.Instantiate<GameObject>(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity).GetComponent<Character>().SetLevel(1);
                        Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Spawned " + g_prefab + "x", 0, null);
                        return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Object " + g_prefab + " not found.", 0, null);
                return;
            }
            Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Player not found. Set to local player.", 0, null);
            this.GiveItem(g_prefab);
        }
        public GameCamera Cam()
        {
            return GameCamera.instance;
        }
        public Player GetLocalPlayer()
        {
            return Player.m_localPlayer;
        }
        public void setEnv(string Env)
        {
            EnvMan.instance.SetForceEnvironment(Env);
        }

        public void skipToMorning()
        {
            SetPrivateFieldValue<EnvMan, double>(EnvMan.instance, "m_timeSkipSpeed", 6.0);
            EnvMan.instance.SkipToMorning();
        }

        private Vector3 m_vel = Vector3.zero;
    
      /*  private void DoAOE(Vector3 hitPoint, ref bool hitCharacter, ref bool didDamage)
        {
            Collider[] array = Physics.OverlapSphere(hitPoint, 40, 2, QueryTriggerInteraction.UseGlobal);

            foreach (Player player in this._Players)
            {

                HashSet<GameObject> hashSet = new HashSet<GameObject>();
                foreach (Collider collider in array)
                {
                    GameObject gameObject = Projectile.FindHitObject(collider);
                    IDestructible component = gameObject.GetComponent<IDestructible>();
                    if (component != null && !hashSet.Contains(gameObject))
                    {
                        hashSet.Add(gameObject);
                        if (this.IsValidTarget(component))
                        {
                            if (component is Character)
                            {
                                hitCharacter = true;
                            }
                            Vector3 vector = collider.ClosestPointOnBounds(hitPoint);
                            Vector3 vector2 = (Vector3.Distance(vector, hitPoint) > 0.1f) ? (vector - hitPoint) : this.m_vel;
                            vector2.y = 0f;
                            vector2.Normalize();
                            HitData hitData = new HitData();
                            hitData.m_hitCollider = collider;
                            hitData.m_damage = ;
                            hitData.m_pushForce = 3f;
                            hitData.m_backstabBonus = 1f;
                            hitData.m_ranged = true;
                            hitData.m_point = vector;
                            hitData.m_dir = vector2.normalized;
                            hitData.m_statusEffect = "";
                            hitData.m_skillLevel = GetLocalPlayer().GetSkillLevel(this.m_skill);
                            hitData.m_dodgeable = false;
                            hitData.m_blockable = false;
                            hitData.m_skill = this.m_skill;
                            hitData.m_skillRaiseAmount = 1f;
                            hitData.SetAttacker(GetLocalPlayer());
                            component.Damage(hitData);
                            didDamage = true;
                        }
                    }
                }
            }
        }*/

        // Token: 0x0400042D RID: 1069
      

        public void Start()
        {
            base.InvokeRepeating("UpdateEntities", 0.5f, 0.5f);

            if (ZNetView.m_initZDO != null)
            {
                m_zdo = ZNetView.m_initZDO;
            }

            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;


            //AttackWeapon = GetComponent<Attack>();
            m_nview = GetComponent<ZNetView>();
            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);
        }
        // Token: 0x06000020 RID: 32
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public void GiveItem(string g_prefab, Vector3 position)
        {
            if (this.GetLocalPlayer() != null)
            {
                GameObject prefab = ZNetScene.instance.GetPrefab(g_prefab);
                if (prefab != null)
                {
                    try
                    {
                        UnityEngine.Object.Instantiate<GameObject>(prefab, position, Quaternion.identity).GetComponent<Character>().SetLevel(0);
                        this.GetLocalPlayer().Message(MessageHud.MessageType.TopLeft, "Spawned " + g_prefab + "x", 0, null);
                        return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Object " + g_prefab + " not found.", 0, null);
                return;
            }
            Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Player not found. Set to local player.", 0, null);
            this.GiveItem(g_prefab, position);
        }

        public void Teleporter(int windowID)
        {
           

            GUI.Label(new Rect(88f, 35f, 100f, 50f), "Teleporter / Troll", this.LabelStyle);
            List<ZNet.PlayerInfo> playerList = ZNet.instance.GetPlayerList();
            if (GUI.Button(new Rect(170f, 90f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[0].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 90f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[0].m_position.x, playerList[0].m_position.y + 2, playerList[0].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 90f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[0].m_position.x, playerList[0].m_position.y + 2, playerList[0].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 130f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[1].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 130f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[1].m_position.x, playerList[1].m_position.y + 2, playerList[1].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 130f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[1].m_position.x, playerList[1].m_position.y + 2, playerList[1].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 170f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[2].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 170f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[2].m_position.x, playerList[2].m_position.y + 2, playerList[2].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 170f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[2].m_position.x, playerList[2].m_position.y + 2, playerList[2].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 210f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[3].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 210f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[3].m_position.x, playerList[3].m_position.y + 2, playerList[3].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 210f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[3].m_position.x, playerList[3].m_position.y + 2, playerList[3].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 250f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[4].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 250f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[4].m_position.x, playerList[4].m_position.y + 2, playerList[4].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 250f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[4].m_position.x, playerList[4].m_position.y + 2, playerList[4].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 290f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[5].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 290f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[5].m_position.x, playerList[5].m_position.y + 2, playerList[5].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 290f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[5].m_position.x, playerList[5].m_position.y + 2, playerList[5].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 330f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[6].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 330f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[6].m_position.x, playerList[6].m_position.y + 2, playerList[6].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 330f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[6].m_position.x, playerList[6].m_position.y + 2, playerList[6].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 370f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[7].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 370f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[7].m_position.x, playerList[7].m_position.y + 2, playerList[7].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 370f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[7].m_position.x, playerList[7].m_position.y + 2, playerList[7].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 410f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[8].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 410f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[8].m_position.x, playerList[8].m_position.y + 2, playerList[8].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 410f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[8].m_position.x, playerList[8].m_position.y + 2, playerList[8].m_position.z));
            }
            if (GUI.Button(new Rect(170f, 450f, 60f, 20f), "To Him"))
            {
                this.GetLocalPlayer().TeleportTo(playerList[9].m_position, Quaternion.identity, true);
            }
            if (GUI.Button(new Rect(235f, 450f, 45f, 20f), "Troll"))
            {
                this.GiveItem("Greydwarf", new Vector3(playerList[9].m_position.x, playerList[9].m_position.y + 2, playerList[9].m_position.z));
            }
            if (GUI.Button(new Rect(285f, 450f, 85f, 20f), "bilebomb_explosion"))
            {
                this.GiveItem("bilebomb_explosion", new Vector3(playerList[9].m_position.x, playerList[9].m_position.y + 2, playerList[9].m_position.z));
            }
            GUI.Label(new Rect(15f, 80f, 140f, 50f), string.Concat(new object[]
            {
                "P1: ",
                playerList[0].m_name,
                "\n",
                playerList[0].m_position,
                "\n ID: ",
                playerList[0].m_characterID

            }));
            GUI.Label(new Rect(15f, 120f, 140f, 50f), string.Concat(new object[]
            {
                "P2: ",
                playerList[1].m_name,
                "\n",
                playerList[1].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 160f, 140f, 50f), string.Concat(new object[]
            {
                "P3: ",
                playerList[2].m_name,
                "\n",
                playerList[2].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 200f, 140f, 50f), string.Concat(new object[]
            {
                "P4: ",
                playerList[3].m_name,
                "\n",
                playerList[3].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 240f, 140f, 50f), string.Concat(new object[]
            {
                "P5: ",
                playerList[4].m_name,
                "\n",
                playerList[4].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 280f, 140f, 50f), string.Concat(new object[]
            {
                "P6: ",
                playerList[5].m_name,
                "\n",
                playerList[5].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 320f, 140f, 50f), string.Concat(new object[]
            {
                "P7: ",
                playerList[6].m_name,
                "\n",
                playerList[6].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 360f, 140f, 50f), string.Concat(new object[]
            {
                "P8: ",
                playerList[7].m_name,
                "\n",
                playerList[7].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 400f, 140f, 50f), string.Concat(new object[]
            {
                "P9: ",
                playerList[8].m_name,
                "\n",
                playerList[8].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));
            GUI.Label(new Rect(15f, 440f, 140f, 50f), string.Concat(new object[]
            {
                "P10: ",
                playerList[9].m_name,
                "\n",
                playerList[9].m_position,
                "\n ",
                "ID: ",
                playerList[0].m_characterID
            }));

            GUI.DragWindow(); // Allow the user to drag the window around
        }

        public void simbot()
        {
            float minDist = 99999; // Reset Distance to some high number because we know that the Enemies will be closer to us than that number.
            Vector2 AimTarget = Vector2.zero; // Declare a new Vector2 for the Screen Position where the mouse will aim at.
            int smooth = 5; // Smooth that gets used to move your mouse. If you make it smaller it will be more snapy.

            try
            {


                foreach (BaseAI Player in this.StoreAI)
                {


                    if (Player.GetTargetCreature().GetVisual())
                        return;
                   

                    // Get the World Position of the Enemys Body Part..
                    Vector3 Enemy_Bodypart_Position = new  Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
                        // ..and convert it to Screen Positions.
                        var shit = Camera.main.WorldToScreenPoint(Enemy_Bodypart_Position);

                        if (shit.z > 0f) // If the enemy isn't behind the camera (us), do...
                        {
                            // Get Distance.
                            float dist = System.Math.Abs(Vector2.Distance(new Vector2(shit.x, Screen.height - shit.y), new Vector2((Screen.width / 2), (Screen.height / 2))));
                            if (dist < 300) // FOV
                            {
                                if (dist < minDist) // If we find a closer target, set him as the aim target.
                                {
                                    minDist = dist;
                                    AimTarget = new Vector2(shit.x, Screen.height - shit.y);
                                }
                            }
                        }
                    }
                    if (AimTarget != Vector2.zero) // If the Vector isn't empty (there is no enemy position converted to screen position), don't aim.
                    {
                        // Center of the Screen
                        double DistX = AimTarget.x - Screen.width / 2.0f;
                        double DistY = AimTarget.y - Screen.height / 2.0f;

                        // Aimbot Smooth.
                        DistX /= smooth;
                        DistY /= smooth;

                        if (Input.GetKey(KeyCode.Z)) // Aimbot Key.
                        {
                            mouse_event(0x0001, (int)DistX, (int)DistY, 0, 0); // Move Mouse to that point.
                        }
                    }
                
            }
            catch
            {
                // Handle Errors here.
            }
        }
        // Token: 0x06000010 RID: 16 RVA: 0x00002384 File Offset: 0x00000584
        public BaseAI getAI()
        {
            foreach (BaseAI item in BaseAI.GetAllInstances())
            {
                this.StoreAI.Add(item);
            }
            return null;
        }
       
        // Token: 0x06000011 RID: 17 RVA: 0x000023DC File Offset: 0x000005DC
        public Character getHeadPos()
        {
            foreach (Character item in Character.GetAllCharacters())
            {
                this.charEyePos.Add(item);
            }
            return null;
        }
        public void UpdateEntities()
        {
            this.AttackWeapon.Clear();
            this.VisEquip.Clear();


            if (this.esp)
            {
                this.StoreAI.Clear();
                this.charEyePos.Clear();
                this.Container.Clear();
                this.Prefab.Clear();
              
                this.getAI();
                this.getHeadPos();
                this.getHeadPos();
            }
        }
        private static readonly BindingFlags FieldProperty = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

        public static void SetPrivateFieldValue<Class, Value>(Class pThis, string field, Value value)
        {
            pThis.GetType().GetField(field, FieldProperty).SetValue(pThis, value);
        }
        public static Return GetPrivateFieldValue<Return, Class>(Class pThis, string field)
        {
            return (Return)((object)pThis.GetType().GetField(field, FieldProperty).GetValue(pThis));
        }
        public void g_UnlockDLCs(bool isUnlocked)
        {
            foreach (DLCMan.DLCInfo dlcinfo in DLCMan.instance.m_dlcs)
            {
                if (!dlcinfo.m_installed)
                {
                    dlcinfo.m_installed = isUnlocked;
                }
            }
        }
      

        public void InvokeRPC(string method, params object[] parameters)
        {
            ZRoutedRpc.instance.InvokeRoutedRPC(this.m_zdo.m_owner, this.m_zdo.m_uid, method, parameters);
        }



        public void DoProjectileHitNoise(Vector3 center, float range, Character attacker)
	{
            foreach (BaseAI baseAI in this.StoreAI)
            {
                if ((!attacker || baseAI.IsEnemy(attacker)) && Vector3.Distance(baseAI.transform.position, center) < range && m_nview && m_nview.IsValid())
			{
                    InvokeRPC("OnNearProjectileHit", new object[]
				{
					center,
					range,
					attacker ? attacker.GetZDOID() : ZDOID.None
				});
			}
		}
	}

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                MenuYoggle = !MenuYoggle;
             
            }


            this.Cam().m_fov = cam_FOV;
            this.GetLocalPlayer().m_jumpForce = c_jumpForce;
            this.GetLocalPlayer().m_runSpeed = c_RunAndSwimSpeed;
            this.GetLocalPlayer().m_swimSpeed = c_RunAndSwimSpeed / 1.5f;
            this.GetLocalPlayer().m_maxInteractDistance = c_interactDistance;


            foreach (Attack Player in this.AttackWeapon)
            {
                if (AttackTest)
                {
                    Player.m_projectileAccuracy = 0f;
                    Player.m_projectileAccuracyMin = 0f;
                    Player.m_projectileVel = 100;
                    Player.m_projectileVelMin = 0f;
                    Player.m_projectiles = 10;
                }
                else
                { }
            }
            if (Godmode)
            {
                GetLocalPlayer().SetGodMode(true);
            }
            else
            {
                GetLocalPlayer().SetGodMode(false);
            }
            if (CharVac)
            {
                foreach (Character player in this.charEyePos)
                {
                    //    player.TeleportTo(new Vector3(player.transform.position.x + GetLocalPlayer().transform.position.x, player.transform.position.y + GetLocalPlayer().transform.position.y), new Quaternion(), false);

                    player.transform.position = new Vector3(GetLocalPlayer().transform.position.x, GetLocalPlayer().transform.position.y + 1f, GetLocalPlayer().transform.position.z + 1f);

                }

            }

            foreach (Player player in this._Players)
            {
                if (SetSleepingtest)
                {
                    player.SetSleeping(true);
                }
                else
                {
                    player.SetSleeping(false);

                }

            }
            if (Nocostbuild)
            {
                Player.m_localPlayer.SetNoPlacementCost(true);
            }
            else
            {
                Player.m_localPlayer.SetNoPlacementCost(false);
            }

            if (Walkonwater)
            {

                GetLocalPlayer().m_swimDepth = -0.5f;
            }
            else
            {
                GetLocalPlayer().m_swimDepth = 1f;
            }


            if (unlStamina)
            {
                float maxStamina = this.GetLocalPlayer().GetMaxStamina();
                GetLocalPlayer().AddStamina(maxStamina);
            }
            if (debugFly)
            {
                SetPrivateFieldValue<Player, bool>(this.GetLocalPlayer(), "m_debugFly", true);
            }
            else
            {
                SetPrivateFieldValue<Player, bool>(this.GetLocalPlayer(), "m_debugFly", false);
            }

            if (aimbot)
            {
                simbot();

            }
            

            if (ghostkill)
            {
                foreach (Character character in Character.GetAllCharacters())
                {
                    if ((float)((int)Vector2.Distance(Player.m_localPlayer.transform.position, character.transform.position)) < 10f && !character.IsPlayer() && !character.IsTamed())
                    {
                        HitData hitData = new HitData();
                        hitData.m_damage.m_damage = 99999f;
                        character.Damage(hitData);
                    }
                }
            }
            _mCamera = Camera.main;

        }
  
    }
}

