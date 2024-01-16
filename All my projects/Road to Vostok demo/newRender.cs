
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuckHaxx2
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
            Color result;

            float percentage = health / maxHealth;

            if (percentage >= 0.75f)
            {
                result = Color.Lerp(Color.cyan, Color.green, (percentage - 0.75f) * 4f);
            }
            else if (percentage >= 0.5f)
            {
                result = Color.Lerp(Color.yellow, Color.green, (percentage - 0.5f) * 4f);
            }
            else if (percentage >= 0.25f)
            {
                result = Color.Lerp(Color.red, Color.yellow, (percentage - 0.25f) * 4f);
            }
            else
            {
                result = Color.Lerp(Color.magenta, Color.red, percentage * 4f);
            }

            return result;
        }
        public static void DrawDot(Vector3 position, Color dotColor)
        {
            Debug.DrawRay(position, Vector3.up * 0.1f, dotColor); // Adjust the length as needed
            Debug.DrawRay(position, Vector3.right * 0.1f, dotColor); // Adjust the length as needed
        }
        public static void DrawCross(Vector3 position, Vector2 mapSize, float size, Color color)
        {
            // Calculate the half extents of the map
            Vector2 halfMapSize = mapSize / 2f;

            // Ensure the cross doesn't go outside the map bounds
            float xSize = Mathf.Min(size, halfMapSize.x);
            float ySize = Mathf.Min(size, halfMapSize.y);

            // Draw the horizontal line of the cross
            DrawLine(position + Vector3.left * xSize, position + Vector3.right * xSize, color);

            // Draw the vertical line of the cross
            DrawLine(position + Vector3.down * ySize, position + Vector3.up * ySize, color);
        }
        public static void DrawCircle(Color Col, Vector2 Center, float Radius)
        {
            GL.PushMatrix();

            if (!drawMaterial.SetPass(0))
            {
                GL.PopMatrix();
                return;
            }

            GL.Begin(1);
            GL.Color(Col);

            for (float num = 0f; num < 6.28318548f; num += 0.05f)
            {
                GL.Vertex(new Vector3(Mathf.Cos(num) * Radius + Center.x, Mathf.Sin(num) * Radius + Center.y));
                GL.Vertex(new Vector3(Mathf.Cos(num + 0.05f) * Radius + Center.x, Mathf.Sin(num + 0.05f) * Radius + Center.y));
            }

            GL.End();
            GL.PopMatrix();
        }
        public static Vector3 WorldToScreenPoint(Vector3 wp)
        {
            Vector4 vector = Camera.current.projectionMatrix * Camera.current.worldToCameraMatrix * new Vector4(wp.x, wp.y, wp.z, 1f);
            if (vector.w < 0.1f)
            {
                return Vector3.zero;
            }
            float num = 1f / vector.w;
            vector.x *= num;
            vector.y *= num;
            Vector2 vector2 = new Vector2(0.5f * (float)Camera.current.pixelWidth, 0.5f * (float)Camera.current.pixelHeight);
            vector2.x += 0.5f * vector.x * (float)Camera.current.pixelWidth + 0.5f;
            vector2.y -= 0.5f * vector.y * (float)Camera.current.pixelHeight + 0.5f;
            return new Vector3(vector2.x, vector2.y, wp.z);
        }
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            Color oldColour = GUI.color;

            var rad2deg = 360 / (Math.PI * 2);

            Vector2 d = end - start;

            float a = (float)rad2deg * Mathf.Atan(d.y / d.x);

            if (d.x < 0)
                a += 180;

            int width2 = (int)Mathf.Ceil(width / 2);

            GUIUtility.RotateAroundPivot(a, start);

            GUI.color = color;

            GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), Texture2D.whiteTexture, ScaleMode.StretchToFill);

            GUIUtility.RotateAroundPivot(-a, start);

            GUI.color = oldColour;
        }
        internal static void DrawCrosshair(Vector2 pos, float size, float thickness, Color color) // This draws your crosshair (color cannot be edited here)
        {
            float halfSize = size / 2f;

            GUI.color = color;

            // Draw horizontal line
            GUI.DrawTexture(new Rect(pos.x - halfSize, pos.y, size, thickness), Texture2D.whiteTexture);

            // Draw vertical line
            GUI.DrawTexture(new Rect(pos.x, pos.y - halfSize, thickness, size), Texture2D.whiteTexture);

            GUI.color = Color.white; // This does not change the color
        }

        public static void DrawPlane(Vector3 position, Vector3 normal, float size, Color color, float duration, bool depthTest = true)
        {
            Vector3 vector;
            if (normal.normalized != Vector3.forward)
            {
                vector = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            }
            else
            {
                vector = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;
            }
            Vector3 vector2 = position + vector * size;
            Vector3 vector3 = position - vector * size;
            vector = Quaternion.AngleAxis(90f, normal) * vector;
            Vector3 vector4 = position + vector * size;
            Vector3 vector5 = position - vector * size;
            Debug.DrawLine(vector2, vector3, color, duration, depthTest);
            Debug.DrawLine(vector4, vector5, color, duration, depthTest);
            Debug.DrawLine(vector2, vector4, color, duration, depthTest);
            Debug.DrawLine(vector4, vector3, color, duration, depthTest);
            Debug.DrawLine(vector3, vector5, color, duration, depthTest);
            Debug.DrawLine(vector5, vector2, color, duration, depthTest);
            Debug.DrawRay(position, normal * size, color, duration, depthTest);
        }
        public static Color RandomColor()
        {
            return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }
        public static void DrawMarker(Vector3 position, float size, Color color)
        {
            Vector3 start = position + Vector3.up * size * 0.5f;
            Vector3 end = position - Vector3.up * size * 0.5f;
            Vector3 start2 = position + Vector3.right * size * 0.5f;
            Vector3 end2 = position - Vector3.right * size * 0.5f;
            Vector3 start3 = position + Vector3.forward * size * 0.5f;
            Vector3 end3 = position - Vector3.forward * size * 0.5f;
            DrawLine(start, end, color, 5f);
            DrawLine(start2, end2, color, 5f);
            DrawLine(start3, end3, color, 5f);
        }
        internal static void DrawHealth(Vector2 pos, float health, float size = 1.0f, bool center = false)
        {
            if (center)
            {
                pos -= new Vector2(26f * size, 0f);
            }
            pos += new Vector2(0f, 18f * size);
            BoxRect(new Rect(pos.x, pos.y, 52f * size, 5f * size), Color.black);
            pos += new Vector2(1f * size, 1f * size);
            Color color = Color.green;
            if (health <= 50f)
            {
                color = Color.yellow;
            }
            if (health <= 25f)
            {
                color = Color.red;
            }
            BoxRect(new Rect(pos.x, pos.y, 0.5f * health * size, 3f * size), color);
        }





        private static Color __color;
        internal static void BoxRect(Rect rect, Color color)
        {
            if (color != __color)
            {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();
                __color = color;
            }

            GUI.DrawTexture(rect, drawingTex);
        }
        public static void PlayerCornerBox(Vector2 Head, float Width, float Height, float thickness, int distance, Color color)
        {
            int num = (int)(Width / 4f);
            int num2 = num;
            RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);

            RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);

            RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);
            RectFilled(Head.x + Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);
            RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);

            RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 2), Color.black);

            RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);
            RectFilled(Head.x + Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 3), Color.black);
            RectFilled(Head.x - Width / 2f, Head.y, (float)num, 1f, color);

            RectFilled(Head.x - Width / 2f, Head.y, 1f, (float)num2, color);

            RectFilled(Head.x + Width / 2f - (float)num, Head.y, (float)num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y, 1f, (float)num2, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, (float)num, 1f, color);

            RectFilled(Head.x - Width / 2f, Head.y + Height - (float)num2 - 3f, 1f, (float)num2, color);

            RectFilled(Head.x + Width / 2f - (float)num, Head.y + Height - 3f, (float)num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y + Height - (float)num2 - 3f, 1f, (float)(num2 + 1), color);
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

        public static void DrawBones(Transform bone1, Transform bone2, Color c, int mode)
        {
            if (!Camera.main) //fix the crash maybe
            {
                return;
            }

            if (!bone1 || !bone2)
            {
                return;
            }

            Vector3 w1 = Camera.main.WorldToScreenPoint(bone1.position);
            Vector3 w2 = Camera.main.WorldToScreenPoint(bone2.position);
            if (w1.z > 0.0f && w2.z > 0.0f)
            {
                DrawLine(new Vector2(w1.x, Screen.height - w1.y), new Vector2(w2.x, Screen.height - w2.y), c, 2f);
            }
        }

        public static void DrawAllBones(List<Transform> b, Color c)
        {
            var t = b[13];
            t.position = new Vector3(b[0].position.x, t.position.y, b[0].position.z);

            int mode = 0;

            DrawBones(b[0], b[1], c, mode);
            DrawBones(b[1], b[2], c, mode);

            DrawBones(b[1], b[3], c, mode);
            DrawBones(b[3], b[4], c, mode);
            DrawBones(b[4], b[5], c, mode);
            DrawBones(b[5], b[6], c, mode);

            DrawBones(b[1], b[7], c, mode);
            DrawBones(b[7], b[8], c, mode);
            DrawBones(b[8], b[9], c, mode);
            DrawBones(b[9], b[10], c, mode);

            DrawBones(b[2], b[11], c, mode);
            DrawBones(b[11], b[12], c, mode);
            DrawBones(b[12], b[13], c, mode);

            DrawBones(b[2], b[14], c, mode);
            DrawBones(b[14], b[15], c, mode);
            DrawBones(b[15], b[16], c, mode);
        }


        public static bool IsOnScreen(Vector3 position)
        {
            return position.y > 0.01f && position.y < Screen.height - 5f && position.z > 0.01f;
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
        public static Vector3[] GetBoundPosition(Bounds bounds)//Vector3 mins, Vector3 maxs)
        {
            /*
            Vector3[] corners = {
                mins, maxs,
                new Vector3(mins.x,mins.y,maxs.z),
                new Vector3(mins.x,maxs.y,mins.z),
                new Vector3(maxs.x,mins.y,mins.z),
                new Vector3(mins.x,maxs.y,maxs.z),
                new Vector3(maxs.x,mins.y,maxs.z),
                new Vector3(maxs.x,maxs.y,mins.z),
            };*/
            Vector3 center = bounds.center;
            Vector3 extents = bounds.size / 2f;
            Vector3[] corners = {
                center + new Vector3(- extents.x,  extents.y,  - extents.z), //FrontTopLeft 0
                center + new Vector3( extents.x,  extents.y,  - extents.z), //FrontTopRight 1
                center + new Vector3(- extents.x,  - extents.y,  - extents.z), //FrontBottomLeft 2
                center + new Vector3(extents.x,  - extents.y,  - extents.z), //FrontBottomRight 3

                center + new Vector3( - extents.x,  extents.y,  extents.z), //BackTopLeft 4
                center + new Vector3( extents.x, extents.y,  extents.z), //BackTopRight 5
                center + new Vector3( - extents.x,  - extents.y,  extents.z), //BackBottomLeft 6
                center + new Vector3( extents.x,  - extents.y, extents.z) //BackBottomRight 7
            };
            return corners;
        }
    
        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            DrawLine(start, end, color, 1);
        }
        public static void DrawLine(Vector2 start, Vector2 end, Texture2D texture, int thick)
        {
            var vector = end - start;
            float pivot = 57.29578f * Mathf.Atan(vector.y / vector.x);
            if (vector.x < 0f)
                pivot += 180f;
            if (thick < 1)
                thick = 1;

            int yOffset = (int)Mathf.Ceil((float)(thick / 2));
            var rect = new Rect(start.x, start.y - yOffset, Vector2.Distance(start, end), (float)thick);
            GUIUtility.RotateAroundPivot(pivot, start);
            GUI.BeginGroup(rect);
            int num3 = Mathf.RoundToInt(rect.width);
            int num4 = Mathf.RoundToInt(rect.height);

            for (int i = 0; i < num4; i += texture.height)
            {
                for (int j = 0; j < num3; j += texture.width)
                {
                    GUI.DrawTexture(new Rect((float)j, (float)i, (float)texture.width, (float)texture.height), texture);
                }
            }
        }
        public static void DrawLine(Vector2 start, Vector2 end, Texture2D texture)
        {
            DrawLine(start, end, texture, 1);
        }

        public static void MakeMeGlow(GameObject me, Color color, bool glow)
        {
            if (!me)
                return;
            Highlight Glower = me.GetComponent<Highlight>();
           
            //  Glower.c(color);
            if (glow)
                Glower.HighlightON();
            else
                Glower.HighlightOFF();
        }

        public static void Draw2DBox(float x, float y, float w, float h, Color color)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
        }

       

        public static void Draw3DBox(GameObject target, Color color)
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            MeshRenderer meshrnd = target.GetComponentInChildren<MeshRenderer>();
            if (meshrnd)
                bounds = meshrnd.bounds;
            else
                return;
            Vector3[] corners = GetBoundPosition(bounds);
            Vector3[] ScreenW2S = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                corners[i] = Quaternion.Euler(target.transform.rotation.eulerAngles) * corners[i];
                ScreenW2S[i] = Camera.main.WorldToScreenPoint(corners[i]);
                ScreenW2S[i].y = Screen.height - ScreenW2S[i].y;
                //DrawShadow(new Rect(ScreenW2S[i].x, ScreenW2S[i].y, 200, 200), new GUIContent($"{i}"), GUI.skin.GetStyle(""), Color.red, Color.black, Vector2.zero);
            }
            if (ScreenW2S[4].z > 0.01f && ScreenW2S[0].z > 0.01f)
                DrawLine(ScreenW2S[4], ScreenW2S[0], color); //FTL - FTR
            if (ScreenW2S[6].z > 0.01f && ScreenW2S[2].z > 0.01f)
                DrawLine(ScreenW2S[6], ScreenW2S[2], color); //FTR - FBR
            if (ScreenW2S[7].z > 0.01f && ScreenW2S[3].z > 0.01f)
                DrawLine(ScreenW2S[7], ScreenW2S[3], color); //FBR - FBL
            if (ScreenW2S[1].z > 0.01f && ScreenW2S[5].z > 0.01f)
                DrawLine(ScreenW2S[1], ScreenW2S[5], color); //FBL - FTL

            if (ScreenW2S[4].z > 0.01f && ScreenW2S[6].z > 0.01f)
                DrawLine(ScreenW2S[4], ScreenW2S[6], color); //BTL - BTR
            if (ScreenW2S[0].z > 0.01f && ScreenW2S[2].z > 0.01f)
                DrawLine(ScreenW2S[0], ScreenW2S[2], color); //BTR - BBR
            if (ScreenW2S[7].z > 0.01f && ScreenW2S[1].z > 0.01f)
                DrawLine(ScreenW2S[7], ScreenW2S[1], color); //BBR - BBL
            if (ScreenW2S[3].z > 0.01f && ScreenW2S[5].z > 0.01f)
                DrawLine(ScreenW2S[3], ScreenW2S[5], color); //BBL - BTL

            if (ScreenW2S[4].z > 0.01f && ScreenW2S[7].z > 0.01f)
                DrawLine(ScreenW2S[4], ScreenW2S[7], color); //FTL - BTL
            if (ScreenW2S[6].z > 0.01f && ScreenW2S[1].z > 0.01f)
                DrawLine(ScreenW2S[6], ScreenW2S[1], color); //FTR - BTR
            if (ScreenW2S[0].z > 0.01f && ScreenW2S[3].z > 0.01f)
                DrawLine(ScreenW2S[0], ScreenW2S[3], color); //FBR - BBR
            if (ScreenW2S[2].z > 0.01f && ScreenW2S[5].z > 0.01f)
                DrawLine(ScreenW2S[2], ScreenW2S[5], color); //FBL - BBL
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




        public static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, FontStyle fontStyle = FontStyle.Bold, int depth = 1)
        {
            __style.fontSize = size;
            __style.richText = true;
            __style.normal.textColor = color;
            __style.fontStyle = fontStyle;
            __style.font = tahoma;
            __outlineStyle.fontSize = size;
            __outlineStyle.richText = true;
            __outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            __outlineStyle.fontStyle = fontStyle;
            __outlineStyle.font = tahoma;
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

        private static Texture2D drawingTex;
        private static Texture2D whiteTexture;

        private static Color lastTexColour;

        public static Material drawMaterial;
      
        private static GUIStyle __style = new GUIStyle();
        private static GUIStyle __outlineStyle = new GUIStyle();

        public static Font tahoma = Font.CreateDynamicFontFromOSFont("Segoe UI", 12);

    }
}
