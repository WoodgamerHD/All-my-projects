﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GodOfWeapons
{




    class ESPUtils : MonoBehaviour
    {
        static ESPUtils()
        {
            whiteTexture = Texture2D.whiteTexture;
            drawingTex = new Texture2D(1, 1);

            drawMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = (HideFlags)61
            };

            drawMaterial.SetInt("_SrcBlend", 5);
            drawMaterial.SetInt("_DstBlend", 10);
            drawMaterial.SetInt("_Cull", 0);
            drawMaterial.SetInt("_ZWrite", 0);
        }

        public static Color GetHealthColour(float health, float maxHealth)
        {
            Color result = Color.green;

            float percentage = health / maxHealth;

            if (percentage >= 0.75f)
            {
                result = Color.green;
            }
            else
            {
                result = Color.yellow;
            }

            if (percentage <= 0.25f)
            {
                result = Color.red;
            }

            return result;
        }

       
        public static Vector3 WorldToScreenPoint(Vector3 wp)
        {
            Vector4 vector = Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix * new Vector4(wp.x, wp.y, wp.z, 1f);
            if (vector.w < 0.1f)
            {
                return Vector3.zero;
            }
            float num = 1f / vector.w;
            vector.x *= num;
            vector.y *= num;
            Vector2 vector2 = new Vector2(0.5f * (float)Camera.main.pixelWidth, 0.5f * (float)Camera.main.pixelHeight);
            vector2.x += 0.5f * vector.x * (float)Camera.main.pixelWidth + 0.5f;
            vector2.y -= 0.5f * vector.y * (float)Camera.main.pixelHeight + 0.5f;
            return new Vector3(vector2.x, vector2.y, wp.z);
        }
      
      
        public static void CornerBox(Vector2 pos, Vector2 size, Color colour)
        {
            Color oldColour = GUI.color;
            GUI.color = colour;

            GUI.DrawTexture(new Rect(pos.x, pos.y, 1, size.y), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x + size.x, pos.y, 1, size.y), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, 1), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x, pos.y + size.y, size.x, 1), whiteTexture);

            GUI.color = oldColour;
        }

      

     
        public static bool IsOnScreen(Vector3 position)
        {
            return position.y > 0.01f && position.y < Screen.height - 5f && position.z > 0.01f;
        }
        public static bool IsVisible(Vector3 position)
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
        public static void CornerBox(Vector2 Head, float Width, float Height, float thickness, Color color, bool outline)
        {
            int num = (int)(Width / 4f);
            int num2 = num;

            if (outline)
            {
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, num + 2, 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x + Width / 2f - num - 1f, Head.y - 1f, num + 2, 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y - 1f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - 4f, num + 2, 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - num2 - 4f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x + Width / 2f - num - 1f, Head.y + Height - 4f, num + 2, 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y + Height - num2 - 4f, 3f, num2 + 3, Color.black);
            }

            RectFilled(Head.x - Width / 2f, Head.y, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - num2 - 3f, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y + Height - num2 - 3f, 1f, num2 + 1, color);
        }

        public static void RectFilled(float x, float y, float width, float height, Color color)
        {
            if (color != lastTexColour)
            {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();

                lastTexColour = color;
            }

            GUI.DrawTexture(new Rect(x, y, width, height), drawingTex);
        }

        public static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, FontStyle fontStyle = FontStyle.Bold, int depth = 1)
        {
            __style.fontSize = size;
           // __style.richText = true;
            __style.normal.textColor = color;
            __style.fontStyle = fontStyle;

            __outlineStyle.fontSize = size;
            //__outlineStyle.richText = true;
            __outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            __outlineStyle.fontStyle = fontStyle;

            GUIContent content = new GUIContent(text);
            GUIContent content2 = new GUIContent(text);
            if (center)
            {
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                pos.x -= __style.CalcSize(content).x / 2f;
            }
            switch (depth)
            {
                case 0:
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 1:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 2:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 3:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
            }
        }
        public static void Draw3DBox(Bounds b, Color color)
        {
            Vector3[] pts = new Vector3[8];
            pts[0] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[1] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[2] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[3] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
            pts[4] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[5] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[6] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[7] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

            for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

            GL.PushMatrix();
            GL.Begin(1);
            drawMaterial.SetPass(0);
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Begin(1);
            drawMaterial.SetPass(0);
            GL.Color(color);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);

            GL.End();
            GL.PopMatrix();

        }
        private static Texture2D drawingTex;
        private static Texture2D whiteTexture;

        private static Color lastTexColour;

        public static Material drawMaterial;
      
        private static GUIStyle __style = new GUIStyle();
        private static GUIStyle __outlineStyle = new GUIStyle();
    }
}
