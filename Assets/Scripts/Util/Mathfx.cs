using UnityEngine;
using System;

public class Mathfx
{
	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
	}

	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
	}

	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
	}

	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float v1 = (x - min) / (max - min);
		float v2 = (x - min) / (max - min);
		return -2 * v1 * v1 * v1 + 3 * v2 * v2;
	}

	public static float Lerp(float start, float end, float value)
	{
		return ((1.0f - value) * start) + (value * end);
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
		float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
		return lineStart + (closestPoint * lineDirection);
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 fullDirection = lineEnd - lineStart;
		Vector3 lineDirection = Vector3.Normalize(fullDirection);
		float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
		return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
	}
	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	// test for value that is near specified float (due to floating point inprecision)
	// all thanks to Opless for this!
	public static bool Approx(float val, float about, float range)
	{
		return ((Mathf.Abs(val - about) < range));
	}

	// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
	// compares the square of the distance to the square of the range as this 
	// avoids calculating a square root which is much slower than squaring the range
	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return ((val - about).sqrMagnitude < range * range);
	}

	/*
	  * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
	  * This is useful when interpolating eulerAngles and the object
	  * crosses the 0/360 boundary.  The standard Lerp function causes the object
	  * to rotate in the wrong direction and looks stupid. Clerp fixes that.
	  */
	public static float Clerp(float start, float end, float value)
	{
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
		float retval = 0.0f;
		float diff = 0.0f;

		if ((end - start) < -half)
		{
			diff = ((max - start) + end) * value;
			retval = start + diff;
		}
		else if ((end - start) > half)
		{
			diff = -((max - end) + start) * value;
			retval = start + diff;
		}
		else retval = start + (end - start) * value;

		// Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
		return retval;
	}

	public static Vector3 GetPointOnCircle(float radius, float degree, Vector3 originOffset)
	{
		float radian = degree * (Mathf.PI / 180f);
		float newX = radius * Mathf.Cos(radian) + originOffset.x;
		float newY = radius * Mathf.Sin(radian) + originOffset.y;
		return new Vector3(newX, newY);
	}

	public static float GetRadius(Vector3 point, Vector3 origin)
	{
		var diameter = Mathf.Sqrt(Mathf.Pow(point.x - origin.x, 2) + Mathf.Pow(point.y - origin.y, 2));
		return diameter / 2;
	}

	public static float GetAngle(Vector3 point, Vector3 origin)
	{
		return Mathf.Atan2(origin.y - point.y, origin.x - point.x) * (180 / Mathf.PI);
		//var angle = Mathf.atan2(y2 - y1, x2 - x1) * 180 / Mathf.PI;
	}

	//True modulo, as opposed to remainder
	public static float Modulo (float operand1, float operand2)
	{
		return operand1 - operand2 * Mathf.Floor(operand1 / operand2);
	}

	public static Color ColorMoveTowards(Color a, Color b, float t)
	{
		float red;
		if(a.r == b.r)
		{
			red = a.r;
		}
		else
		{
			red = Mathf.MoveTowards(a.r, b.r, t);
		}
		float green;
		if (a.g == b.g)
		{
			green = a.g;
		}
		else
		{
			green = Mathf.MoveTowards(a.g, b.g, t);
		}
		float blue;
		if (a.b == b.b)
		{
			blue = a.b;
		}
		else
		{
			blue = Mathf.MoveTowards(a.b, b.b, t);
		}
		float alpha;
		if (a.a == b.a)
		{
			alpha = a.a;
		}
		else
		{
			alpha = Mathf.MoveTowards(a.a, b.a, t);
		}
		return new Color(red, green, blue, alpha);

	}

	public static bool CompareColors(Color a, Color b, float varient = 0)
	{
		float distance = 0;
		bool difference = false;
		distance += Mathf.Abs(a.r - b.r);
		distance += Mathf.Abs(a.g - b.g);
		distance += Mathf.Abs(a.b - a.b);
		distance += Mathf.Abs(a.a - a.a);
		if (distance <= varient)
		{
			difference = true;
		}
		Debug.Log("color difference " + distance);

		//return distance;
		//bool compare = true;
		//if (a.r != b.r)
		//{
		//    compare = false;
		//}
		//if (a.g != b.g)
		//{
		//    compare = false;
		//}
		//if (a.b != b.b)
		//{
		//    compare = false;
		//}
		//if (a.a != b.a)
		//{
		//    compare = false;
		//}
		return difference;
	}

}
