
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;


namespace Remnant
{

    class Main : MonoBehaviour
    {

        bool PlaceHolder = false;
      
        bool Entityesp = false;
        bool Playeresp = false;
        bool Crosshair = false;
    
       
        public static List<Monster> EnemyBase = new List<Monster>();
        public static List<MonoBehaviourPun> MonoBehaviourPun = new List<MonoBehaviourPun>();
        public static List<PlayerData> PlayerData = new List<PlayerData>();

   
  
   
        float natNextUpdateTime;
      

        private Color blackCol;
        private Color entityBoxCol;
        public static Camera cam;
   
        private Rect windowRect = new Rect(0, 0, 400, 400); // Window position and size
        private int tab = 0; // Current tab index
        private Color backgroundColor = Color.black; // Background color
        private bool showMenu = true; // Whether to show the menu or not

     
        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
      
        void MenuWindow(int windowID)
        {
            //   GUILayout.BeginHorizontal();

            // Create toggle buttons for each tab
            GUILayout.BeginHorizontal();
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
        
            GUILayout.EndVertical();
          

            GUILayout.BeginVertical();


            // Display content for the selected tab
            switch (tab)
            {
                case 0:
                    // Content for tab 1



                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    PlaceHolder = GUILayout.Toggle(PlaceHolder, "PlaceHolder");
                    PlaceHolder = GUILayout.Toggle(PlaceHolder, "PlaceHolder");

                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();

                    PlaceHolder = GUILayout.Toggle(PlaceHolder, "PlaceHolder");
                    PlaceHolder = GUILayout.Toggle(PlaceHolder, "PlaceHolder");

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();


             
                    break;
                case 1:
                    // Content for tab 2

                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    Playeresp = GUILayout.Toggle(Playeresp, "Player esp");
                    Entityesp = GUILayout.Toggle(Entityesp, "Entity esp");
          
                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    Crosshair = GUILayout.Toggle(Crosshair, "Crosshair");
                    PlaceHolder = GUILayout.Toggle(PlaceHolder, "PlaceHolder");
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                  
                    break;
                case 2:
                    // Content for tab 2

                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("OpenAllDoors"))
                    {
                        foreach (Monster player in EnemyBase)
                        {
                            player.OpenAllDoors();

                        }
                    }

                    if (GUILayout.Button("CloseAllDoors"))
                    {
                        foreach (Monster player in EnemyBase)
                        {
                            player.CloseAllDoors();

                        }
                    }

                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    if (GUILayout.Button("ReenableAllLights"))
                    {
                        foreach (Monster player in EnemyBase)
                        {
                            player.ReenableAllLights();
                            player.TurnOnLightsInRange(99999999,false);

                        }
                    }

                    if (GUILayout.Button("DisableAllLights"))
                    {
                        foreach (Monster player in EnemyBase)
                        {
                            player.DisableAllLights();

                        }
                    }

                    if (GUILayout.Button("RPCFoundEverything"))
                    {
                        foreach (MonoBehaviourPun player in MonoBehaviourPun)
                        {
                            player.photonView.RPC("RPCFoundEverything", RpcTarget.All, Array.Empty<object>());


                        }
                    }

                 
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    //      GUILayout.Label("fov: " + fov.ToString("F2"));
                    //    fov = GUILayout.HorizontalSlider(fov, 50f, 200f, GUILayout.ExpandWidth(true)); // Create vertical slider with a height of 200 pixels

                    break;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
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
        public void OnGUI()
        {
            GUI.contentColor = Color.yellow;
            GUI.Label(new Rect(10f, 985f, 110f, 30f), "Remnant Records");

            GUI.contentColor = Color.green;
            GUI.Label(new Rect(10f, 1000f, 150f, 30f), "Coded by : JakeRuCodes");
            GUI.contentColor = Color.white;


            if (Crosshair)
            {
                float centerX = Screen.width / 2f;
                float centerY = Screen.height / 2f;
                DrawCrosshair(centerX, centerY, 20f, 2f, Color.red); // Adjust the size and thickness here
            }
               if (showMenu) // Only draw the menu when showMenu is true
            {
                // Set the background color
                GUI.backgroundColor = backgroundColor;

                windowRect = GUI.Window(0, windowRect, MenuWindow, "Remnant Records"); // Create the window with title "Menu"
            }


            // ESP 


            if (Playeresp)
            {
                foreach (PlayerData player in PlayerData)
                {




                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


                    if (ESPUtils.IsOnScreen(w2s) && Vector3.Distance(Camera.main.transform.position, player.transform.position) < 500f)
                    {
                        DrawPlayerInfo(player);
                    }
                }
            }
                if(Entityesp)
                { 
                foreach (Monster player in EnemyBase)
                {
                  



                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


                    if (ESPUtils.IsOnScreen(w2s) && Vector3.Distance(Camera.main.transform.position, player.transform.position) < 500f)
                    {              
                        DrawEnemyInfo(player);
                    }
                }
            }

         
        }

        private string GetMonsterClassName(Monster monster)
        {
            // Implement your logic here to determine the monster's class name
            // You can use a switch or if-else statements to handle different monster classes
            // For example: 
            if (monster is Runner)
            {
                return "Runner";
            }
            else if (monster is Shusher)
            {
                return "Shusher";
            }
            else if (monster is Orphan)
            {
                return "Orphan";
            }
            else if (monster is Reflection)
            {
                return "Reflection";
            }
            else if (monster is Mannequin)
            {
                return "Mannequin";
            }
            // ... (handle other monster classes)

            return "Unknown"; // Default value if class name is not recognized
        }

        private string GetPlayerClassName(PlayerData player)
        {
            // Implement your logic here to determine the player's class name
            // You can use a switch or if-else statements to handle different player classes
            // For example: 
            if (player is Cartomancer)
            {
                return "Cartomancer";
            }
            else if (player is Medium)
            {
                return "Medium";
            }
            else if (player is Electrician)
            {
                return "Electrician";
            }
            else if (player is Musclehead)
            {
                return "Bodyguard";
            }
            // ... (handle other player classes)

            return "Unknown"; // Default value if class name is not recognized
        }

        public void DrawPlayerInfo(PlayerData player)
        {
            if (player.photonView.IsMine)
                return;

            // Get the player's class name
            string playerClassName = GetPlayerClassName(player);

            // Get the player's Steam name
            string playerSteamName = player.photonView.Owner.NickName;

            Vector3 enemyBottom = player.transform.position;
            Vector3 enemyTop;
            enemyTop.x = enemyBottom.x;
            enemyTop.z = enemyBottom.z;
            enemyTop.y = enemyBottom.y + 2f;
            Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);
            Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);


            float height = Mathf.Abs(worldToScreenTop.y - w2s.y);
            float x = w2s.x - height * 0.3f;
            float y = Screen.height - worldToScreenTop.y;

            Color blackCol = Color.black;

            // Draw the box
            ESPUtils.CornerBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
            ESPUtils.CornerBox(new Vector2(x, y), new Vector2(height / 2f, height), Color.red);
            ESPUtils.CornerBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);


            // Calculate the initial positions of the text
            Vector2 namePosition = new Vector2(x + (height / 4f), y - 14f);
            Vector2 SidePosition = new Vector2(x + (height / 2f) + 3f, y + 1f);
            Vector2 bottomTextPosition = new Vector2(x + (height / 4f) + 3f, y + height + 3f);

            // Offset the text positions based on the player's movement
            namePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            SidePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            bottomTextPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);

            float distance = Vector3.Distance(Camera.main.transform.position, player.transform.position);
            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);

            // Draw the text at the adjusted positions with the adjusted font size
            ESPUtils.DrawString(namePosition, playerSteamName, Color.green, true, fontSize, FontStyle.Bold);
            ESPUtils.DrawString(SidePosition, "Class: " + playerClassName, Color.green, false, fontSize, FontStyle.Bold);
            ESPUtils.DrawString(bottomTextPosition, "Test Buttom", Color.green, true, fontSize, FontStyle.Bold);

        }

        public void DrawEnemyInfo(Monster player)
        {

            string monsterClassName = GetMonsterClassName(player);

            Vector3 enemyBottom = player.transform.position;
            Vector3 enemyTop;
            enemyTop.x = enemyBottom.x;
            enemyTop.z = enemyBottom.z;
            enemyTop.y = enemyBottom.y + 2f;
            Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);
            Vector3 w2s = Camera.main.WorldToScreenPoint(player.transform.position);


            float height = Mathf.Abs(worldToScreenTop.y - w2s.y);
            float x = w2s.x - height * 0.3f;
            float y = Screen.height - worldToScreenTop.y;

            Color blackCol = Color.black;

            // Draw the box
              ESPUtils.CornerBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
              ESPUtils.CornerBox(new Vector2(x, y), new Vector2(height / 2f, height), Color.red);
             ESPUtils.CornerBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);


            // Calculate the initial positions of the text
            Vector2 namePosition = new Vector2(x + (height / 4f), y - 14f);
            Vector2 SidePosition = new Vector2(x + (height / 2f) + 3f, y + 1f);
            Vector2 bottomTextPosition = new Vector2(x + (height / 4f) + 3f, y + height + 3f);

            // Offset the text positions based on the player's movement
            namePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            SidePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            bottomTextPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);

            float distance = Vector3.Distance(Camera.main.transform.position, player.transform.position);
            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);

            // Draw the text at the adjusted positions with the adjusted font size
            ESPUtils.DrawString(namePosition, monsterClassName, Color.red, true, fontSize, FontStyle.Bold);
             ESPUtils.DrawString(SidePosition, "Test Side", Color.red, false, fontSize, FontStyle.Bold);
             ESPUtils.DrawString(bottomTextPosition, "Test Buttom", Color.red, true, fontSize, FontStyle.Bold);

        }


        public void Start()
        {
            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;

        
            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);
        }
        private void DrawCrosshair(float x, float y, float size, float thickness, Color color) // This draws your crosshair (color cannot be edited here)
        {
            float halfSize = size / 2f;

            GUI.color = color;

            // Draw horizontal line
            GUI.DrawTexture(new Rect(x - halfSize, y, size, thickness), Texture2D.whiteTexture);

            // Draw vertical line
            GUI.DrawTexture(new Rect(x, y - halfSize, thickness, size), Texture2D.whiteTexture);

            GUI.color = Color.white; // This does not change the color
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }
          



















            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 0.1f)
            {
                EnemyBase = FindObjectsOfType<Monster>().ToList();
                PlayerData = FindObjectsOfType<PlayerData>().ToList();
                MonoBehaviourPun = FindObjectsOfType<MonoBehaviourPun>().ToList();
                natNextUpdateTime = 0f;
            }

            cam = Camera.main;





        }
    }
}

