using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum MoveType
{
	Path = 0,
	Homing = 1,
	Waypoint = 2,
	Detour = 3
}

public class Mover : MonoBehaviour
{
	[System.Serializable]
	public class DetourEvent : UnityEvent<bool> { }

	public MoveType type;

	public bool moving;
	public bool randomized;
	public bool useRadius;
	//public bool useForce;
	//public bool spreadOut;

	public float speed;

	//public int direction;
	public float startRadius;
	public float endRadius;
	public Vector3 startPoint;
	public Vector3 endPoint;
	public int degree;
	public float distance;
	private float radian;

	public Transform currentTarget;

	public UnityEvent onTargetReached = new UnityEvent();

	public DetourEvent onDetour = new DetourEvent();

	public void Mobilize(Vector3 start, Vector3 end, float sp)
	{
		startPoint = start;
		endPoint = end;
		speed = sp;
		moving = true;
		StartCoroutine(FollowPath(endPoint));
	}

	public void Mobilize(float sp)
	{
		speed = sp;
		if (randomized)
		{
			SetRandomAngle();
		}

		if (useRadius)
		{
			SetStartPoint(startRadius);

			SetEndPoint(endRadius);
		}

		StartCoroutine(FollowPath(endPoint));
	}

	public void StartMovement(MoveType moveType, float sp, Vector3 point)
	{
		moving = true;
		type = moveType;
		StopAllCoroutines();
		switch (type)
		{
			case MoveType.Path:
				speed = sp;
				endPoint = point;
				StartCoroutine(FollowPath(endPoint));
				break;
			case MoveType.Homing:
				StartCoroutine(FollowPath(point, sp));
				Debug.Log("Homing requires a Transform target");
				break;
			case MoveType.Detour:
				onDetour.Invoke(true);
				StartCoroutine(FollowPath(point, sp));
				break;
			default:
				moving = false;
				break;
		}
	}

	public void StartMovement(MoveType moveType, float sp, Transform transform)
	{
		if (!gameObject.activeSelf)
		{
			return;
		}
		moving = true;
		type = moveType;
		StopAllCoroutines();
		switch (type)
		{
			case MoveType.Path:
				speed = sp;
				endPoint = transform.position;
				StartCoroutine(FollowPath(endPoint));
				break;
			case MoveType.Homing:
				currentTarget = transform;
				StartCoroutine(FollowTarget(currentTarget, sp));
				break;
			case MoveType.Detour:
				onDetour.Invoke(true);
				StartCoroutine(FollowPath(transform.position, sp));
				break;
			default:
				moving = false;
				break;
		}
	}

	IEnumerator FollowPath(Vector3 point)
	{
		distance = Vector3.Distance(transform.position, point);
		while (distance > 0)
		{
			if (moving)
			{
				distance = Vector3.Distance(transform.position, point);
				transform.position = Vector3.MoveTowards(transform.position, point, (Time.deltaTime * speed));
			}
			yield return null;
		}
		moving = false;
		ReachedDestination(point);

		yield return null;
	}

	IEnumerator FollowPath(Vector3 point, float sp)
	{
		distance = Vector3.Distance(transform.position, point);
		//print(speed * Time.deltaTime);
		while (distance > 0)
		{
			if (moving)
			{
				distance = Vector3.Distance(transform.position, point);
				transform.position = Vector3.MoveTowards(transform.position, point, (Time.deltaTime * sp));
			}
			yield return null;
		}
		moving = false;
		//print("Target Reached");
		ReachedDestination(point);

		yield return null;
	}

	public IEnumerator FollowTarget(Transform tf, float sp)
	{
		distance = Vector3.Distance(transform.position, tf.position);
		//print(speed * Time.deltaTime);
		while (distance > 0)
		{
			if (moving)
			{
				if (this != null && tf != null)
				{
					distance = Vector3.Distance(transform.position, tf.position);
					transform.position = Vector3.MoveTowards(transform.position, tf.position, (Time.deltaTime * sp));
				}
				else
				{
					distance = 0;
				}

			}
			yield return null;
		}
		moving = false;

		Vector3 point = new Vector3();
		if (tf)
		{
			point = tf.position;
		}
		else
		{
			point = transform.position;
		}

		ReachedDestination(point);
	}

	public void RedirectPath(Vector3 point, float sp)
	{
		//detour = true;
		StopAllCoroutines();
		StartCoroutine(FollowPath(point, sp));
	}

	public void RestoreOriginalPath()
	{
		StopAllCoroutines();
		moving = true;
		if (gameObject.activeSelf)
		{
			StartCoroutine(FollowPath(endPoint, speed));
		}
	}

	public void SetMovement(bool m)
	{
		moving = m;
	}

	public void ReachedDestination(Vector3 point)
	{
		transform.position = point;
		switch (type)
		{
			case MoveType.Detour:
				onDetour.Invoke(false);
				StartMovement(MoveType.Path, speed, endPoint);
				break;
			case MoveType.Path:
			case MoveType.Homing:
				onTargetReached.Invoke();
				break;
			default:
				break;
		}
	}

	public void GoToEndPoint()
	{
		transform.position = endPoint;
	}

	public void GoToStartPoint()
	{
		transform.position = startPoint;
	}

	public void SetAngle(int aisle)
	{
		radian = ((Mathf.PI * 0.25f) * aisle) + (90 * (Mathf.PI * 0.25f));
		degree = (int)(radian * (180 / Mathf.PI));
	}

	public void SetAngle(float degrees)
	{
		radian = degrees * (Mathf.PI / 180);
		degree = (int)(radian * (180 / Mathf.PI));
	}

	public void SetAngleWithinRange(int aisle, float range)
	{
		float spread = (Random.Range(0, range) - (range / 2f)) * (Mathf.PI / 180f);
		radian = ((Mathf.PI * 0.25f) * aisle) + (90 * (Mathf.PI * 0.25f)) + spread;
		degree = (int)(radian * (180 / Mathf.PI));
	}

	public void SetRandomAngle()
	{
		radian = (Random.value * 2f) * Mathf.PI;

		degree = (int)(radian * (180 / Mathf.PI));
	}

	public void SetDirection(int d)
	{
		//direction = d;
		SetAngle(d);
	}

	public void SetStartPoint(float sRadius)
	{
		float radius = sRadius;
		float newX = radius * Mathf.Cos(radian);
		float newY = radius * Mathf.Sin(radian);
		startPoint = new Vector3(newX, newY);
	}

	public void AddStartRaduisJitter(float jitter)
	{
		startRadius += (Random.value * jitter);
	}

	public void SetEndPoint(float x, float y)
	{
		endPoint = new Vector3(x, y);
	}

	public void SetEndPoint(float eRadius)
	{
		float radius = eRadius;
		//print("Set End Point angle " + angle);
		float newX = radius * Mathf.Cos(radian);
		float newY = radius * Mathf.Sin(radian);
		SetEndPoint(newX, newY);
	}

	public void SetEndPoint(float eRadius, Vector3 offset)
	{
		float radius = eRadius;
		float newX = (radius * Mathf.Cos(radian)) + offset.x;
		float newY = (radius * Mathf.Sin(radian)) + offset.y;
		SetEndPoint(newX, newY);
	}
}
