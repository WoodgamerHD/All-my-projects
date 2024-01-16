
//using AMG;
//using AMG.Collector;
//using MoreMountains.Feedbacks;
//using Steamworks;
//using System.Collections.Generic;
//using System.Linq;

//using UnityEngine;


//namespace GodOfWeapons
//{

//    class Main : MonoBehaviour
//    {

//        bool godmode = false;
       
//        public static List<CharacterManager> CharacterManager = new List<CharacterManager>();
//        public static List<CollectorManager> CollectorManager = new List<CollectorManager>();
//        public static List<MMF_Player> MMF_Player = new List<MMF_Player>();
//        public static List<LevelManager> LevelManager = new List<LevelManager>();
//        public static List<AutoSpawnManager> AutoSpawnManager = new List<AutoSpawnManager>();
       
//        private List<DataManager> DataManager = new List<DataManager>();


//        float natNextUpdateTime;
  
//        public static Camera cam;
   
//        private bool showMenu = true; // Whether to show the menu or not

  
//        public static Color TestColor
//        {
//            get
//            {
//                return new Color(1f, 0f, 1f, 1f);
//            }
//        }
//        private int selectedIndex = 0; // The currently selected option
//        private string[] menuOptions = {
//        "Godmode: Off",
//        "Spawn Chest",
//        "Max Gold",
//        "Max Experience",
//        "Max Meta and Keys",
//        "Unlock All",
//        "Achievements",
//    };

//        private float menuX = 50; // X-coordinate of the menu
//        private float menuY = 50; // Y-coordinate of the menu
//        private float optionSpacing = 30; // Spacing between menu options

//        public void OnGUI()
//        {
//            if (showMenu)
//            {
//                float boxWidth = 200;
//                float boxHeight = menuOptions.Length * optionSpacing + 10; // Add padding for the background

//                float boxX = menuX;
//                float boxY = menuY;

//                GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");

//                for (int i = 0; i < menuOptions.Length; i++)
//                {
//                    if (i == selectedIndex)
//                    {
//                        GUI.color = Color.yellow;
//                    }
//                    else
//                    {
//                        GUI.color = Color.white;
//                    }

//                    float optionX = boxX + 10; // Add padding for the text
//                    float optionY = boxY + 5 + i * optionSpacing; // Add padding for the text

//                    GUI.Label(new Rect(optionX, optionY, 200, 30), menuOptions[i]);
//                }
//            }
        





//            /* if (showMenu) // Only draw the menu when showMenu is true
//             {
//                 // Get the screen width and height
//                 int screenWidth = Screen.width;
//                 int screenHeight = Screen.height;

//                 // Calculate the position and font size based on the screen resolution
//                 Vector2 position = new Vector2(screenWidth * 0.1f, screenHeight * 0.4f);
//                 int fontSize = Mathf.FloorToInt(screenHeight * 0.02f);

//                 // Your text to display
//                 string text = $"F1: Godmode: {godmode}" + "\n" +
//                               "F2: SpawnChest" + "\n" +
//                               "F3: AddGold" + "\n" +
//                               "F4: AddExperience" + "\n" +
//                               "F5: AddMetaCurrency" + "\n" +
//                               "F6: Unlockall" + "\n" +
//                               "F7: Achievements" + "\n" +
//                               "F8: ItemBomb";

//                 // Call your DrawString function with the updated parameters
//                 ESPUtils.DrawString(position, text, TestColor, true, fontSize, FontStyle.Bold);
//             }
//            */
//        }



//        public void Start()
//        {

           

        
//        }
//        private void ExecuteSelectedOption()
//        {
//            switch (selectedIndex)
//            {
//                case 0:
//                    ToggleGodMode();
//                    break;
//                case 1:
//                    foreach (LevelManager item in LevelManager)
//                    {
//                        foreach (CharacterManager local in CharacterManager)
//                        {
//                            item.SpawnChest(local.transform.position);
//                        }
//                    }
//                    break;
//                case 2:
//                    foreach (CharacterManager item in CharacterManager)
//                    {
//                        item.AddGold(999999999, false);
                        
//                    }
//                    break;
//                case 3:
//                    foreach (CharacterManager item in CharacterManager)
//                    {
//                        item.AddExperience(999999999);
//                    }
//                    break;
//                case 4:
//                    foreach (DataManager item in DataManager)
//                    {
//                        item.GlobalGameData.material2 = 999999999;
//                        item.GlobalGameData.material1 = 999999999;
//                    }
//                    break;
//                case 5:
//                   foreach (CollectorManager item in CollectorManager)
//                    {
//                        item.DevUnlockAll();
//                        item.UnlockAll();
//                    }
                
                  

//                    break;
//                case 6:
//                    int totalAchievements = 93;
//                    for (int achievementID = 0; achievementID < totalAchievements; achievementID++)
//                    {
//                        string achievementName = SteamUserStats.GetAchievementName((uint)achievementID);
//                        SteamUserStats.SetAchievement(achievementName);
//                    }
//                    break;
            
//            }
//        }
//        private void ToggleGodMode()
//        {
          
//            godmode = !godmode;
//            menuOptions[0] = "Godmode: " + (godmode ? "On" : "Off");
//        }
//        public void Update()
//        {
           
//                if (godmode)
//            {
//                foreach (CharacterManager item in CharacterManager)
//                {
//                    item.Health = float.MaxValue;
                   
//                }

//            }
            

//            if (Input.GetKeyDown(KeyCode.Insert))
//            {
//                showMenu = !showMenu;
//            }

//            if (showMenu)
//            {
//                if (Input.GetKeyDown(KeyCode.Keypad2)) // Numpad 2
//                {
//                    selectedIndex = (selectedIndex + 1) % menuOptions.Length;
//                }
//                else if (Input.GetKeyDown(KeyCode.Keypad8)) // Numpad 8
//                {
//                    selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
//                }
//                else if (Input.GetKeyDown(KeyCode.Keypad5))
//                {
//                    ExecuteSelectedOption();
//                }
//            }

//            natNextUpdateTime += Time.deltaTime;

//            if (natNextUpdateTime >= 1f)
//            {


               
//                CharacterManager = FindObjectsOfType<CharacterManager>().ToList();
//                CollectorManager = FindObjectsOfType<CollectorManager>().ToList();
//                MMF_Player = FindObjectsOfType<MMF_Player>().ToList();
//                LevelManager = FindObjectsOfType<LevelManager>().ToList();
//                AutoSpawnManager = FindObjectsOfType<AutoSpawnManager>().ToList();
//                DataManager = FindObjectsOfType<DataManager>().ToList();
                
              
          
//                natNextUpdateTime = 0f;
//            }
          
              
//            cam = Camera.main;

//        }
//    }
//}

