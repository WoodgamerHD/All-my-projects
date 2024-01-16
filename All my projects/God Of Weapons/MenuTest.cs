using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GodOfWeapons
{
    class Main : MonoBehaviour
    {
     
        private bool[] tabOpen = new bool[3]; // One bool for each tab
        private bool enableESP = false; // Checkbox for "Enable ESP" in the "Visuals" tab
    

        private Rect mainRect = new Rect(20, 20, 200, 200);
        private Rect[] subRects = { new Rect(240, 20, 200, 200), new Rect(240, 240, 200, 200), new Rect(240, 460, 200, 200) };
        private string[] tabNames = { "Visuals", "Misc", "Player" };


        private void OnGUI()
        {
            GUI.backgroundColor = Color.black;



            // Draw main window
            mainRect = GUILayout.Window(0, mainRect, DrawMainWindow, "Main Window", GUILayout.Width(200));

          
            // Draw sub-windows for each tab
            for (int i = 0; i < tabOpen.Length; i++)
            {
                if (tabOpen[i])
                {
                    subRects[i] = GUILayout.Window(i + 1, subRects[i], DrawSubWindow, tabNames[i],GUILayout.Width(200));
                }
            }
        }

        private void DrawMainWindow(int id)
        {
            // Set the background to black
            GUI.backgroundColor = Color.black;

            // Draw Tabs vertically
            GUILayout.BeginVertical();

            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Button(tabNames[i], GUILayout.ExpandWidth(true)))
                {
                    // Toggle the sub-window for the clicked tab
                    tabOpen[i] = !tabOpen[i];
                }
            }

            // Set the text color to gray
       //     GUI.contentColor = Color.gray;

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private void DrawSubWindow(int id)
        {
            // Set the background to black
            GUI.backgroundColor = Color.black;

            // Set the text color to gray
        //    GUI.contentColor = Color.gray;

            GUILayout.Label("Content for " + tabNames[id - 1]);

            switch (tabNames[id - 1])
            {
                case "Visuals":
                    enableESP = GUILayout.Toggle(enableESP, "Enable ESP");
                    break;
                case "Misc":
                    enableESP = GUILayout.Toggle(enableESP, "Enable Misc");
                    break;
                case "Player":
                    enableESP = GUILayout.Toggle(enableESP, "Enable Player");
                    break;
            }

            GUI.DragWindow();
        }
    }
    }