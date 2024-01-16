

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Networking.UnityWebRequest;
using static WorldAchievements;



namespace InSilence
{

    class Main : MonoBehaviour
    {
    

      
        bool esp = true;
        bool itemesp = true;
        
        public static List<CreatureAIManager> EnemyBase = new List<CreatureAIManager>();
        public static List<Item> ItemBase = new List<Item>();
        public static List<CreatureHealthShower> CreatureHealthShower = new List<CreatureHealthShower>();
        public static List<SkinChanger> SkinChanger = new List<SkinChanger>();
        public static List<ArmoryManager> ArmoryManager = new List<ArmoryManager>();
  
    

        float natNextUpdateTime;
        private static Material chamsMaterial;

        private Color blackCol;
        private Color entityBoxCol;
        public static Camera cam;
   
     
        private bool showMenu = true; // Whether to show the menu or not

        private int selectedIndex = 0; // The currently selected option
        private string[] menuOptions = {
        "ESP: On",
        "items: On",
        "Creature-Death",
        "Activate-Camuflage",
        "Deactivate-Camuflage",
        "Activate-DeadBody",
        "Armory Correct",
        "Achievements",
    };

        private float menuX = 50; // X-coordinate of the menu
        private float menuY = 50; // Y-coordinate of the menu
        private float optionSpacing = 30; // Spacing between menu options


        public void OnGUI()
        {
            if (showMenu) // Only draw the menu when showMenu is true
            {
                if (showMenu)
                {
                    float boxWidth = 200;
                    float boxHeight = menuOptions.Length * optionSpacing + 10; // Add padding for the background

                    float boxX = menuX;
                    float boxY = menuY;

                    GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");

                    for (int i = 0; i < menuOptions.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            GUI.color = Color.yellow;
                        }
                        else
                        {
                            GUI.color = Color.white;
                        }

                        float optionX = boxX + 10; // Add padding for the text
                        float optionY = boxY + 5 + i * optionSpacing; // Add padding for the text

                        GUI.Label(new Rect(optionX, optionY, 200, 30), menuOptions[i]);
                    }
                }
            }
            if (esp)
            {

                foreach (CreatureAIManager player in EnemyBase)
                {
                        Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);

                        if (ESPUtils.IsOnScreen(w2s))
                        {
                            DrawEnemyInfo(player);


                        }
                    
                }
            }
            if (itemesp)
            {
                foreach (Item player in ItemBase)
                {
                    Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);

                    if (ESPUtils.IsOnScreen(w2s))
                    {
                        DrawitemsInfo(player);


                    }

                }
            }
        
        }
     
        public void DrawEnemyInfo(CreatureAIManager player)
        {
            Vector3 enemyBottom = player.transform.position;
            Vector3 enemyTop;
            enemyTop.x = enemyBottom.x;
            enemyTop.z = enemyBottom.z;
            enemyTop.y = enemyBottom.y + 2f;
            Vector3 worldToScreenTop = cam.WorldToScreenPoint(enemyTop);
            Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


            float height = Mathf.Abs(worldToScreenTop.y - w2s.y);
            float x = w2s.x - height * 0.3f;
            float y = UnityEngine.Screen.height - worldToScreenTop.y;

            Color blackCol = Color.black;

            // Draw the box
            ESPUtils.CornerBox(new Vector2(x - 1f, y - 1f), new Vector2((height / 2f) + 2f, height + 2f), blackCol);
            ESPUtils.CornerBox(new Vector2(x, y), new Vector2(height / 2f, height), Color.red);
            ESPUtils.CornerBox(new Vector2(x + 1f, y + 1f), new Vector2((height / 2f) - 2f, height - 2f), blackCol);


            // Calculate the initial positions of the text
            Vector2 namePosition = new Vector2(x + (height / 4f), y - 14f);
            Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);
            Vector2 bottomTextPosition = new Vector2(x + (height / 4f) + 3f, y + height + 3f);

            // Offset the text positions based on the player's movement
            namePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            hpPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            bottomTextPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f); 

            float distance = Vector3.Distance(cam.transform.position, player.transform.position);
            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);

            // Draw the text at the adjusted positions with the adjusted font size
            ESPUtils.DrawString(namePosition, "Creature | HP:" + player.healthManager.currentHealth, Color.white, true, fontSize, FontStyle.Bold);

          
        }
        public void DrawitemsInfo(Item player)
        {
            Vector3 enemyBottom = player.transform.position;
            Vector3 enemyTop;
            enemyTop.x = enemyBottom.x;
            enemyTop.z = enemyBottom.z;
            enemyTop.y = enemyBottom.y + 2f;
            Vector3 worldToScreenTop = cam.WorldToScreenPoint(enemyTop);
            Vector3 w2s = cam.WorldToScreenPoint(player.transform.position);


            float height = Mathf.Abs(worldToScreenTop.y - w2s.y);
            float x = w2s.x - height * 0.3f;
            float y = UnityEngine.Screen.height - worldToScreenTop.y;

       
            Vector2 namePosition = new Vector2(x + (height / 4f), y - 14f);
            Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);
            Vector2 bottomTextPosition = new Vector2(x + (height / 4f) + 3f, y + height + 3f);

            // Offset the text positions based on the player's movement
            namePosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            hpPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);
            bottomTextPosition -= new Vector2(player.transform.position.x - enemyBottom.x, 0f);

            float distance = Vector3.Distance(cam.transform.position, player.transform.position);
            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);

            // Draw the text at the adjusted positions with the adjusted font size
            ESPUtils.DrawString(bottomTextPosition, player.item_name, Color.blue, true, fontSize, FontStyle.Bold);


        }

        public void Start()
        {
      
            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);
        }
        private void Toggleesp()
        {

            esp = !esp;
            menuOptions[0] = "ESP: " + (esp ? "On" : "Off");
        }
        private void Toggleespitems()
        {

            itemesp = !itemesp;
            menuOptions[1] = "items: " + (itemesp ? "On" : "Off");
        }

        private void ExecuteSelectedOption()
        {
            switch (selectedIndex)
            {
                case 0:
                    Toggleesp();
                    break;
                case 1:
                    Toggleespitems();
                    break;
                case 2:
                    foreach (CreatureHealthShower item in CreatureHealthShower)
                    {
                        item.CreatureDeath();
                    }
                    break;
                case 3:
                    foreach (SkinChanger item in SkinChanger)
                    {
                        item.ActivateCamuflage();
                    }

                    break;
                case 4:
                    foreach (SkinChanger item in SkinChanger)
                    {
                        item.DeactivateCamuflage();
                    }
                    break;
                case 5:

                    foreach (SkinChanger item in SkinChanger)
                    {
                        item.ActivateDeadBody();
                    }


                    break;
                case 6:

                    foreach (ArmoryManager item in ArmoryManager)
                    {
                        item.ActivateAction(true);
                    }


                    break;
                case 7:
                    int totalAchievements = 20; // how many Achievements the game has
                    for (int achievementID = 0; achievementID < totalAchievements; achievementID++)
                    {
                        string achievementName = SteamUserStats.GetAchievementName((uint)achievementID);
                        SteamUserStats.SetAchievement(achievementName);
                    }
                    break;

            }
        }

        public void Update()
        {

          


            if (UnityEngine.Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }

            if (showMenu)
            {
                if (Input.GetKeyDown(KeyCode.Keypad2)) // Numpad 2
                {
                    selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8)) // Numpad 8
                {
                    selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    ExecuteSelectedOption();
                }
            }

            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 0.2f)
            {


               EnemyBase = FindObjectsOfType<CreatureAIManager>().ToList();
                ItemBase = FindObjectsOfType<Item>().ToList();
                CreatureHealthShower = FindObjectsOfType<CreatureHealthShower>().ToList();
                SkinChanger = FindObjectsOfType<SkinChanger>().ToList();
                ArmoryManager = FindObjectsOfType<ArmoryManager>().ToList();
               
            
              
         
                natNextUpdateTime = 0f;
            }

        


       
            cam = Camera.main;

        }
    }
}

