using UnityEngine;

namespace MuckHaxx2
{
    

	/* ---------------------------------------------------- */

	public class ExtRender : MonoBehaviour
	{
		public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);
		// Token: 0x04000002 RID: 2
		private static Texture2D _staticRectTexture;

		// Token: 0x04000003 RID: 3
		private static GUIStyle _staticRectStyle;


		public static Color Color
		{
			get { return GUI.color; }
			set { GUI.color = value; }
		}
		public static void GUIDrawRect(Rect position)
		{
			if (_staticRectTexture == null)
			{
				_staticRectTexture = new Texture2D(1, 1);
			}
			if (_staticRectStyle == null)
			{
				_staticRectStyle = new GUIStyle();
			}
			_staticRectTexture.SetPixel(0, 0, Color.red);
			_staticRectTexture.Apply();
			_staticRectStyle.normal.background = _staticRectTexture;
			GUI.Box(position, GUIContent.none, _staticRectStyle);
		}
		public static void DrawBoxEsp(Vector3 objPos, float flW, float flH)
		{
			GUIDrawRect(new Rect(objPos.x - flW / 2f, objPos.y - flH, 1f, flH));
			GUIDrawRect(new Rect(objPos.x + flW / 2f, objPos.y - flH, 1f, flH));
			GUIDrawRect(new Rect(objPos.x - flW / 2f, objPos.y - flH, flW, 1f));
			GUIDrawRect(new Rect(objPos.x - flW / 2f, objPos.y, flW, 1f));
		}
		public static void DrawBox(Vector2 position, Vector2 size, Color color, bool centered = true)
		{
			Color = color;
			DrawBox(position, size, centered);
		}
		public static void DrawBox(Vector2 position, Vector2 size, bool centered = true)
		{
			var upperLeft = centered ? position - size / 2f : position;
            GUI.DrawTexture(new Rect(position.y, size.y ,0,0), null, ScaleMode.StretchToFill);
		}

		public static void DrawString(Vector2 position, string label, Color color, bool centered = true)
		{
			Color = color;
			DrawString(position, label, centered);
		}
		public static void DrawString(Vector2 position, string label, bool centered = true)
		{
			var content = new GUIContent(label);
			var size = StringStyle.CalcSize(content);
			var upperLeft = centered ? position - size / 2f : position;
			GUI.Label(new Rect(upperLeft.y, size.y,0,0), content);
		}

		public static Texture2D lineTex;
		public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
		{
			Matrix4x4 matrix = GUI.matrix;
			if (!lineTex)
				lineTex = new Texture2D(1, 1);

			Color color2 = GUI.color;
			GUI.color = color;
			float num = Vector3.Angle(pointB - pointA, Vector2.right);

			if (pointA.y > pointB.y)
				num = -num;

			GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
			GUIUtility.RotateAroundPivot(num, pointA);
			GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineTex);
			GUI.matrix = matrix;
			GUI.color = color2;
		}

		public static void DrawBox(float x, float y, float w, float h, Color color, float thickness)
		{
			DrawLine(new Vector2(x, y), new Vector2(x + w, y), color, thickness);
			DrawLine(new Vector2(x, y), new Vector2(x, y + h), color, thickness);
			DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color, thickness);
			DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color, thickness);
		}

        public static void DrawText(string text, float X, float Y, Color color, float size)
        {
            LineMaterial.SetPass(0);
            GUIStyle guistyle = new GUIStyle();
            guistyle.normal.textColor = color;
            guistyle.alignment = TextAnchor.UpperCenter;
            guistyle.fontSize = (int)size;
            GUI.Label(new Rect(new Vector2(X, Y), guistyle.CalcSize(new GUIContent())), text, guistyle);
        }
        public static void DrawBoxOutline(Vector2 Point, float width, float height, Color color, float thickness)
		{
			DrawLine(Point, new Vector2(Point.x + width, Point.y), color, thickness);
			DrawLine(Point, new Vector2(Point.x, Point.y + height), color, thickness);
			DrawLine(new Vector2(Point.x + width, Point.y + height), new Vector2(Point.x + width, Point.y), color, thickness);
			DrawLine(new Vector2(Point.x + width, Point.y + height), new Vector2(Point.x, Point.y + height), color, thickness);
		}
        private static readonly Material LineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
    }
}
