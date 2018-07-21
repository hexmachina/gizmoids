using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : ColliderAbility, IAbility, IMovable
{

	public int power;
	public int span;
	public float ricochetUnit = 0.555f;

	public Mover mover;

	public CapsuleCollider2D capsuleCollider;

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Power"))
		{
			power = (int)valuePairs["Power"];
		}
		if (valuePairs.ContainsKey("Span"))
		{
			span = (int)valuePairs["Span"];
		}
		if (valuePairs.ContainsKey("Unit"))
		{
			ricochetUnit = valuePairs["Unit"];
		}
		var offsetX = capsuleCollider.offset.x;
		if (valuePairs.ContainsKey("OffsetX"))
		{
			offsetX = valuePairs["OffsetX"];
		}
		var offsetY = capsuleCollider.offset.y;
		if (valuePairs.ContainsKey("OffsetY"))
		{
			offsetY = valuePairs["OffsetY"];
		}
		capsuleCollider.offset = new Vector2(offsetX, offsetY);

		var sizeX = capsuleCollider.size.x;
		if (valuePairs.ContainsKey("SizeX"))
		{
			sizeX = valuePairs["SizeX"];
		}
		var sizeY = capsuleCollider.size.y;
		if (valuePairs.ContainsKey("SizeY"))
		{
			sizeY = valuePairs["SizeY"];
		}
		capsuleCollider.size = new Vector2(sizeX, sizeY);

		if (valuePairs.ContainsKey("Direction"))
		{
			if ((int)valuePairs["Direction"] == 0)
			{
				capsuleCollider.direction = CapsuleDirection2D.Vertical;
			}
			else
			{
				capsuleCollider.direction = CapsuleDirection2D.Horizontal;
			}

		}
	}

	protected override void TargetEntered(Collider2D collision)
	{
		var health = collision.GetComponent<Health>();
		if (!health && collision.transform.parent != null)
		{
			health = GetComponentInParent<Health>();
		}
		if (health)
		{
			health.TakeDamage(power);
		}
		var impactable = collision.GetComponent<IImpactable>();
		if (impactable != null)
		{
			impactable.Impact(gameObject);
		}
		Rebound();
	}

	public void Rebound()
	{

		var radius = Vector3.Distance(mover.transform.position, Vector3.zero) + (0.555f * span);
		var target = Mathfx.GetPointOnCircle(radius + (Random.value * 0.1f), mover.transform.eulerAngles.z + 90, Vector3.zero);

		mover.StartMovement(MoveType.Detour, mover.speed * 20, target);
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void SetMover(Mover theMover)
	{
		mover = theMover;
		mover.onDetour.AddListener(HideColliderOnRebound);
	}

	public void HideColliderOnRebound(bool rebound)
	{
		capsuleCollider.enabled = !rebound;
	}
}
