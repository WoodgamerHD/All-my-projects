

using Caapora;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;


namespace IslandIdleRPG
{

    class Main : MonoBehaviour
    {
    

      
        bool esp = true;
        bool godmode = false;
        bool FreeBuild = false;
        bool Weapontest = false;
        bool showconsole = false;
        bool chamsesp = false;
        bool WeaponCreater = false;

       
       
  
   
        float natNextUpdateTime;
        private static Material chamsMaterial;

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


            GUILayout.EndVertical();

            // Display content for the selected tab

            GUILayout.BeginVertical();


            // Display content for the selected tab
            switch (tab)
            {
                case 0:
                    // Content for tab 1



                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    godmode = GUILayout.Toggle(godmode, "godmode");
                    WeaponCreater = GUILayout.Toggle(WeaponCreater, "WeaponCreater(T)");
                  
                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    if (GUILayout.Button("MAX"))
                    {
                   

                        AchievementManager.instance.UnlockAchievement(AchievementNames.ADVENTURERI);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.COLLECT40WOODS);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.DEFEAT_10_MONSTERS);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HIRE_FIRST_WORKER);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ENGINEERI);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HEROI);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HEROII);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ENGINEERII);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ENGINEERIII);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ENGINEER_MASTER);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HEROIII);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HEROIV);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.HERO_LEGENDARY);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ONE_K_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.TEN_K_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ONEHUNDRED_K_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.FIVEHUNDRED_K_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ONE_MILLION_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.TEN_MILLION_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.ONE_HUNDRED_MILLION_GOLD);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.NOBLE_MAN);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.CHEF_II);
                        AchievementManager.instance.UnlockAchievement(AchievementNames.CHEF_I);
                    }
                    if (GUILayout.Button("FreeResearch"))
                    {
                       
                    }
                    if (GUILayout.Button("FreeBuild"))
                    {
                       
                    }
                    if (GUILayout.Button("AddHealthPlayer"))
                    {
                      
                    }
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                   
                    if (GUILayout.Button("Gameobjects dumper"))
                    {

                        StreamWriter SW = new StreamWriter("Gameobjects.txt");
                        // Find all objects in the scene
                        ItemBhvr[] objects = FindObjectsOfType<ItemBhvr>();

                        // Loop through the objects and print their names to the console
                        foreach (ItemBhvr obj in objects)
                        {
                            SW.WriteLine(obj.ItemName +  " | " + obj.IsKeyItem);
                        }

                    }

                    break;
                case 1:
                    // Content for tab 2

                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    esp = GUILayout.Toggle(esp, "esp");
                    chamsesp = GUILayout.Toggle(chamsesp, "chams");
          
                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    Weapontest = GUILayout.Toggle(Weapontest, "text");
                    Weapontest = GUILayout.Toggle(Weapontest, "text");
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

     


        private float DistanceFromCamera(Vector3 worldPos)
        {
            return Vector3.Distance(cam.transform.position, worldPos);
        }



        public void OnGUI()
        {

            if (showMenu) // Only draw the menu when showMenu is true
            {
                // Set the background color
                GUI.backgroundColor = backgroundColor;

                windowRect = GUI.Window(0, windowRect, MenuWindow, "Menu"); // Create the window with title "Menu"
            }

          
        }
       
      


        public void Start()
        {
            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;

            chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset
            };

        
            ;
            chamsMaterial.SetInt("_ZTest", 8); // 8 = see through walls.
           
            chamsMaterial.SetColor("_Color", Color.red);

            blackCol = new Color(0f, 0f, 0f, 120f);
            entityBoxCol = new Color(0.42f, 0.36f, 0.90f, 1f);
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


        //        EnemyBase = FindObjectsOfType<Character>().ToList();
              


              
                natNextUpdateTime = 0f;
            }
          
              

                   
                
            



            cam = Camera.main;

        }
    }
}

