using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TheForest.Buildings.Creation;
using TheForest.Items;
using TheForest.Items.Craft;
using TheForest.Utils;
using UnityEngine;


namespace MuckHaxx2
{
    class Main : MonoBehaviour
    {
        public static bool esp = true;
        public static bool SpeedHack = false;
        public static bool Godmode = false;
        public static bool OneHitToggle = false;
        public static bool flyToggle = false;
        public static bool MenuToggle = true;
        public static bool invisible = false;

        private static float DistanceToPlayer(Transform position) => DistanceToPlayer(position.position);
        private static float DistanceToPlayer(Vector3 position) => ESPUtils.Get3dDistance(LocalPlayer.Transform.position, position);



        public static List<mutantScriptSetup> EnemyBase = new List<mutantScriptSetup>();
        float natNextUpdateTime;



        // Speed multiplier for the cheat
        private Color blackCol;
        private Color entityBoxCol;

        public void DrawBoxESP(Vector3 footPosition, Vector3 headPosition, Color color)
        {
            System.Single height = headPosition.y - footPosition.y;
            System.Single widthOffset = 2f; System.Single width = height / widthOffset;
            ExtRender.DrawBox(footPosition.x - width / 2, Screen.height - footPosition.y - height, width, height, color, 2f);

        }

        // Array of tab names
        string[] tabs = new string[] { "Main", "Esp", "Misc", "Test" };
        // Index of the currently selected tab
        int selectedTabIndex = 0;

        IEnumerator SleepFor(float Time)
        {
            yield return new WaitForSeconds(Time);
        }
        // Token: 0x0600002F RID: 47 RVA: 0x000034E4 File Offset: 0x000016E4
        public static string getEntName(string text)
        {
            text = text.Replace("Go", "").Replace("(Clone)", "").Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "").Replace("lb_", "").Replace("_small", " Small").Replace("_medium", " Medium").Replace("_CaveB", " (B)").Replace("_CaveA", " (A)").Replace("_dummy", "").Replace("_base", "").Replace("_net", "").Replace("_fat", " Fat").Replace("_baby", " Baby").Replace("_creepy", " (Strong)").Replace("blueBird", "Blue Bird").Replace("mutant_male", "Male Cannibal").Replace("mutant_female", "Female Cannibal").Replace("CaveHole", "Cave").Replace("()", "").Replace("caveNarrowEntrance", "Cave Gap").Replace("redBird", "Red Bird");
            return new Regex("<[^>]+>").Replace(text, string.Empty);
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000036CC File Offset: 0x000018CC
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]).ToString() + s.Substring(1);
        }
        // Token: 0x04000010 RID: 16
        public static Camera GameCamera = Camera.main;

        public static List<Transform> allCannibals = new List<Transform>();
        // Token: 0x0400000D RID: 13
        public static List<GameObject> allNetCannibals = new List<GameObject>();

        public void OnGUI()
        {

            GUI.Label(new Rect(15f, 15f, 200f, 200f), "The Forest Hack");


            GUI.Label(new Rect(10, 200, 100, 20), invisible ? "Player invisible" : "Player Seen");

            string text = string.Format("Time Scale: {0}", Time.timeScale);
            GUI.Box(new Rect(0f, (float)Screen.height - 25f, 150f, 25f), "");
            GUI.Label(new Rect(5f, (float)Screen.height - 25f, 150f, 25f), text);

            if (esp && TheForest.Utils.LocalPlayer.IsInWorld)
                {
                foreach (GameObject cannibals in Scene.MutantControler.activeWorldCannibals)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeBabies)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeCannibals)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeCaveCannibals)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeFamilies)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeInstantSpawnedCannibals)
                {
                    DrawEnemyInfo(cannibals);
                }
                foreach (GameObject cannibals in Scene.MutantControler.activeSkinnyCannibals)
                {
                    DrawEnemyInfo(cannibals);

                }




            }
         
            
       



            if (MenuToggle)
            {
                // Begin a horizontal group for the tabs
                GUILayout.BeginHorizontal();

                // Draw a button for each tab
                for (int i = 0; i < tabs.Length; i++)
                {
                    if (GUILayout.Button(tabs[i]))
                    {
                        selectedTabIndex = i;
                    }
                }

                // End the horizontal group for the tabs
                GUILayout.EndHorizontal();

                // Display the contents of the selected tab
                switch (selectedTabIndex)
                {
                    case 0:
                        // Begin a vertical group for the buttons in Tab 1
                        GUILayout.BeginVertical();

                        // Draw a button for each action in Tab 1
                        if (GUILayout.Button("Cut Trees 100"))
                        {
                            foreach (RaycastHit hit in Physics.SphereCastAll(TheForest.Utils.LocalPlayer.Transform.position, 100, new Vector3(1f, 0f, 0f)))
                            {
                                if (hit.collider.GetComponent<TreeHealth>() != null)
                                {
                                    hit.collider.gameObject.SendMessage("Explosion", 100f);
                                }
                            }
                        }
                        if (GUILayout.Button("Cut Trees 200"))
                        {
                            foreach (RaycastHit hit in Physics.SphereCastAll(TheForest.Utils.LocalPlayer.Transform.position, 200, new Vector3(1f, 0f, 0f)))
                            {
                                if (hit.collider.GetComponent<TreeHealth>() != null)
                                {
                                    hit.collider.gameObject.SendMessage("Explosion", 100f);
                                }
                            }
                        }
                        if (GUILayout.Button("Cut Trees LAG"))
                        {
                            foreach (RaycastHit hit in Physics.SphereCastAll(TheForest.Utils.LocalPlayer.Transform.position, 599, new Vector3(1f, 1f, 1f)))
                            {
                                if (hit.collider.GetComponent<TreeHealth>() != null)
                                {
                                    hit.collider.gameObject.SendMessage("Explosion", 100f);
                                }
                            }
                        }
                        if (GUILayout.Button("Kill all troll"))
                        {
                            KillAll();
                        }
                        if (GUILayout.Button("cutgrass 100"))
                        {
                            _cutgrass("100");
                        }
                        if (GUILayout.Button("Build All Ghosts"))
                        {
                            TheForest.Utils.Scene.ActiveMB.StartCoroutine(this.BuildAllGhostsRoutine());
                        }
                        // End the vertical group for the buttons in Tab 1
                        GUILayout.EndVertical();
                        break;
                    case 1:
                        // Begin a vertical group for the buttons in Tab 2
                        GUILayout.BeginVertical();

                        // Draw a button for each action in Tab 2
                        if (GUILayout.Button(esp ? " Esp on" : " Esp off"))
                        {
                            esp = !esp;
                        }
                      




                        // End the vertical group for the buttons in Tab 2
                        GUILayout.EndVertical();
                        break;
                    case 2:
                        // Begin a vertical group for the buttons in Tab 3
                        GUILayout.BeginVertical();

                        // Draw a button for each action in Tab 3


                        if (GUILayout.Button("Godmode"))
                        {
                            Godmode = !Godmode;

                        }
                        if (GUILayout.Button("climbable Wall"))
                        {
                            flyToggle = !flyToggle;

                        }
                        if (GUILayout.Button(OneHitToggle ? " OneHit on" : " OneHit off"))
                        {
                            OneHitToggle = !OneHitToggle;
                        }
                        if (GUILayout.Button(invisible ? " invisible on" : " invisible off"))
                        {
                            invisible = !invisible;
                        }
                        if (GUILayout.Button("Max Inventory"))
                        {
                            if (TheForest.Utils.LocalPlayer.IsInWorld)
                            {
                                foreach (TheForest.Items.Item item in TheForest.Items.ItemDatabase.Items)
                                {
                                    item._maxAmount = int.MaxValue;
                                }
                            }
                        }
                            if (GUILayout.Button(" Give ALL "))
                        {
                            foreach (Item item in ItemDatabase.Items)
                            {

                               
                                    if (item._maxAmount >= 0 && !item.MatchType(Item.Types.Story) && TheForest.Utils.LocalPlayer.Inventory.InventoryItemViewsCache.ContainsKey(item._id))
                                    {
                                    TheForest.Utils.LocalPlayer.Inventory.AddItem(item._id, 100000, true, false, null);
                                    }
                                
                               }
                        }
                        // End the vertical group for the buttons in Tab 3
                        GUILayout.EndVertical();
                        break;
                    case 3:
                        {
                            // Begin a vertical group for the buttons in Tab 3
                            GUILayout.BeginVertical();

                            if (GUILayout.Button("rabbit"))
                            {
                                spawnAnAnimal("rabbit", false);
                            }
                            if (GUILayout.Button("lizard"))
                            {
                                spawnAnAnimal("lizard", false);
                            }
                          
                            GUILayout.EndVertical();
                            break;
                        }
                }
            }
           
               

            /*    GUI.Box(new Rect(10f, 10f, 300f, 300f), "Menu");

                if (GUI.Button(new Rect(20f, 40f, 80f, 20f), "Box_esp"))
                {
                    Box_esp = !Box_esp;
                }
                if (GUI.Button(new Rect(20f, 70f, 80f, 20f), "Add HP"))
                {
                    foreach (GamePlayer player in UnityEngine.Object.FindObjectsOfType(typeof(GamePlayer)) as GamePlayer[])
                    {

                         player.AddHp(99999);
                      //  player.GetState(EUnitState.Invincible);


                    }
                }
                if (GUI.Button(new Rect(20f, 100f, 80f, 20f), "GodMode"))
                {
                    GodMode = !GodMode;
                }

                if (GUI.Button(new Rect(20f, 120f, 80f, 20f), "Test"))
                {
                    Create();
                }*/










        }
        public void DrawEnemyInfo(GameObject obj)
        {
            mutantScriptSetup character = obj.GetComponentInChildren<mutantScriptSetup>();

            Vector3 w2s = Camera.main.WorldToScreenPoint(character.transform.position);

            if (ESPUtils.IsOnScreen(w2s))
            {
                Vector3 enemyBottom = character.transform.position;
                Vector3 enemyTop;
                enemyTop.x = enemyBottom.x;
                enemyTop.z = enemyBottom.z;
                enemyTop.y = enemyBottom.y += character.controller == null ? 5 : character.controller.height;
                Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);



                float height = Mathf.Abs(worldToScreenBottom.y - w2s.y);
                float x = w2s.x - height * 0.3f;
                float y = UnityEngine.Screen.height - worldToScreenBottom.y;

                if (esp)
                {
                    ESPUtils.OutlineBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
                    ESPUtils.OutlineBox(new Vector2(x, y), new Vector2(height / 2f, height), entityBoxCol);
                    ESPUtils.OutlineBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);
                }


                if (esp)
                {
                    ESPUtils.DrawString(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f),
                    "Cannibals " + "\n" + "HP: " + character.health.Health + "\n" + character.weapon.name, Color.red, true, 12, FontStyle.Normal);
                }

                if (esp)
                {
                    double currentHealth = character.health.Health;

                    int maxHealth = character.health.maxHealth;
                    float percentage = (float)(currentHealth / maxHealth);
                    float barHeight = height * percentage;

                    Color barColour = ESPUtils.GetHealthColour((float)currentHealth, maxHealth);

                    ESPUtils.RectFilled(x - 5f, y, 4f, height, blackCol);
                    ESPUtils.RectFilled(x - 4f, y + height - barHeight - 1f, 2f, barHeight, barColour);
                }
            }
        }
            private IEnumerator BuildAllGhostsRoutine()
        {
            Craft_Structure[] css = UnityEngine.Object.FindObjectsOfType<Craft_Structure>();
            if (css != null && css.Length > 0)
            {
                Debug.Log("$> Begin build all " + css.Length + " ghosts");
                foreach (Craft_Structure cs in css)
                {
                    ReceipeIngredient[] presentAll = cs.GetPresentIngredients();
                    List<Craft_Structure.BuildIngredients> requiredAll = cs._requiredIngredients;
                    int i = 0;
                    while (i < requiredAll.Count && i < presentAll.Length)
                    {
                        if (requiredAll[i]._amount != presentAll[i]._amount)
                        {
                            Craft_Structure.BuildIngredients needed = requiredAll[i];
                            ReceipeIngredient present = presentAll[i];
                            for (int k = needed._amount - present._amount; k > 0; k--)
                            {
                                cs.SendMessage("AddIngredient", i);
                            }
                            if (BoltNetwork.isRunning)
                            {
                                yield return null;
                            }
                        }
                        i++;
                    }
                    yield return null;
                }
                Debug.Log("$> Done building all " + css.Length + " ghosts");
            }
            else
            {
                Debug.Log("$> found no ghost buildings to complete");
            }
            yield break;
        }

        void DrawESP(Transform transform)
        {
            Vector3 screenPos = TheForest.Utils.LocalPlayer.MainCam.WorldToScreenPoint(transform.position);
            Vector3 pos = TheForest.Utils.LocalPlayer.Transform.position;

            if (screenPos.z > 0 & screenPos.y < Screen.width - 2)
            {
                float dist = Vector3.Distance(pos, TheForest.Utils.LocalPlayer.Transform.position);
                if (dist < 1000)
                {
                    screenPos.y = Screen.height - (screenPos.y + 1f);
                    ExtRender.DrawText(" Cannibal", screenPos.x, screenPos.y, Color.cyan,1f);
                }
                SleepFor(0.300f);
            }
            SleepFor(0.300f);
        }
      
        public void Start()
        {

            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);

           
        }

        public static void KillAll()
        {
            animalHealth[] array = FindObjectsOfType<animalHealth>();
            animalHealth[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                animalHealth animalHealth = array2[i];
                if (animalHealth.gameObject.activeInHierarchy)
                {
                    animalHealth.SendMessage("Die");
                }
            }

            lb_Bird[] arrayB = FindObjectsOfType<lb_Bird>();
            lb_Bird[] arrayB2 = arrayB;
            for (int i = 0; i < arrayB2.Length; i++)
            {
                lb_Bird birdHealth = arrayB2[i];
                if (birdHealth.gameObject.activeInHierarchy)
                {
                    birdHealth.SendMessage("die");
                }
            }

        }
        public void _cutgrass(string radiusArg)
        {
            if (string.IsNullOrEmpty(radiusArg))
            {
                radiusArg = "1";
            }
            int num = int.Parse(radiusArg);
            NeoGrassCutter.Cut(TheForest.Utils.LocalPlayer.Transform.position, (float)num, true);
            GUI.Label(new Rect(10, 250, 100, 20), "$> Cut grass arround player with a radius of " + radiusArg);
        }
        private void spawnAnAnimal(string type, bool trapped)
        {
            List<string> list = new List<string>
            {
                "rabbit",
                "lizard",
                "deer",
                "turtle",
                "tortoise",
                "raccoon",
                "squirrel",
                "boar",
                "crocodile"
            };
            string prefabName = type + "Go";
            if (!list.Contains(type) || !PoolManager.Pools["creatures"].prefabs.ContainsKey(prefabName))
            {
                string text = "$> usage: spawnanimal <";
                foreach (string str in list)
                {
                    text = text + str + " | ";
                }
                text += ">";
                Debug.Log(text);
                return;
            }
            Transform transform = PoolManager.Pools["creatures"].Spawn(prefabName, TheForest.Utils.LocalPlayer.Transform.position + 2f * TheForest.Utils.LocalPlayer.Transform.forward, Quaternion.identity);
            if (transform)
            {
                if (BoltNetwork.isServer && transform.gameObject.GetComponent<CoopAnimalServer>())
                {
                    AnimalSpawnController.AttachAnimalToNetwork(null, transform.gameObject);
                }
                if (trapped)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.transform.position = TheForest.Utils.LocalPlayer.Transform.position + 2f * TheForest.Utils.LocalPlayer.Transform.forward;
                    animalHealth componentInChildren = transform.GetComponentInChildren<animalHealth>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.Trap = gameObject;
                    }
                    transform.SendMessage("startUpdateSpawn");
                    transform.gameObject.SendMessageUpwards("setTrapped", gameObject, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        public void Update()
        {


            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 0.2f)
            {


                EnemyBase = FindObjectsOfType<mutantScriptSetup>().ToList();
                




                natNextUpdateTime = 0f;
            }



            if (Godmode)
            {
                TheForest.Utils.LocalPlayer.Stats.Health = 1337;
                TheForest.Utils.LocalPlayer.Stats.Stamina = 1337;
                TheForest.Utils.LocalPlayer.Stats.Energy = 1337;
                TheForest.Utils.LocalPlayer.Stats.Sanity.CurrentSanity = 1337;
                TheForest.Utils.LocalPlayer.Stats.Armor = 100;
                TheForest.Utils.LocalPlayer.Stats.Fullness = 1337;
                TheForest.Utils.LocalPlayer.Stats.StarvationCurrentDuration = 0;
                TheForest.Utils.LocalPlayer.Stats.Thirst = 0;
                TheForest.Utils.LocalPlayer.Stats.AirBreathing.TakingDamage = false;
                TheForest.Utils.LocalPlayer.Stats.AirBreathing.CurrentLungAir = 9000f;

                Cheats.Creative = true;
            }
            else
            { Cheats.Creative = false; }
            if (flyToggle)
            {
                foreach (climbableWallSetup player in FindObjectsOfType(typeof(climbableWallSetup)) as climbableWallSetup[])
                {

                    player.occupied = true;
                    player.invalid = true;
                }
                }
            else
            { }
            if (SpeedHack)
            {
                Time.timeScale = 10f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Insert))
            {
                MenuToggle = !MenuToggle;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.X))
            {
                SpeedHack = !SpeedHack;
            }

            if (invisible)
            {
                TheForest.Utils.LocalPlayer.GameObject.layer = 31;
            
            }
            else
            {
                TheForest.Utils.LocalPlayer.GameObject.layer = 18;
              
            }
            foreach (GameObject obj in Scene.MutantControler.activeWorldCannibals)
            {
                mutantScriptSetup character = obj.GetComponentInChildren<mutantScriptSetup>();

                if (OneHitToggle)
                {
                    character.health.Hit(int.MaxValue);
                }
                else
                {
                    
                }
            }
        }
    }
}

