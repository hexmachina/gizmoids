using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GizLib
{
	[System.Serializable]
	public class SectorData
	{
		public string name;

		public int place;
		public int aisle;

		public float angle;

		public List<Path> paths = new List<Path>();

		public float GetOffsetAngle(Vector2 origin, Vector2 point)
		{
			var radius = Vector2.Distance(origin, point);

			origin = new Vector2(origin.x, origin.y + radius);

			var deltaX = origin.x - point.x;
			var deltaY = origin.y - point.y;

			return Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
		}

		public Vector2 PointOnCircle(float rad, float angleInDegrees, Vector2 circleOrigin)
		{
			//if (clockwise)
			//{
			//	angleInDegrees -= offsetIntertnal;
			//}
			//else
			//{
			//	angleInDegrees += offsetIntertnal;
			//	angleInDegrees = 360 - angleInDegrees;
			//}

			float x = (rad * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + circleOrigin.x;
			float y = (rad * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + circleOrigin.y;

			return new Vector2(x, y);
		}

		public Vector2 GetOffsetPoint(Vector2 point)
		{
			var origin = Vector2.zero;

			var radius = Vector2.Distance(origin, point);

			return PointOnCircle(radius, angle + GetOffsetAngle(origin, point), origin);
		}

		public Vector2[] GetOffsetPoints()
		{
			Vector2[] allpoints = new Vector2[0];

			for (int i = 0; i < paths.Count; i++)
			{
				allpoints = allpoints.Concat(paths[i].points).ToArray();
			}

			for (int i = 0; i < allpoints.Length; i++)
			{
				allpoints[i] = GetOffsetPoint(allpoints[i]);
			}

			return allpoints;
		}
	}
}
