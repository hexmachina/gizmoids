using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/Unit/Gizmoid/Data")]
public class GizmoidData : EntityData, GizLib.IAcquirable
{
	public int order;

	[FormerlySerializedAs("price")]
	[SerializeField]
	private int m_price;
	public int Price
	{
		get { return m_price; }
		set
		{
			if (value != m_price)
			{
				value = m_price;
			}
		}
	}

	[FormerlySerializedAs("cooldown")]
	[SerializeField]
	private float m_cooldown;
	public float Cooldown
	{
		get
		{
			return m_cooldown;
		}
		set
		{
			if (value != m_cooldown)
			{
				m_cooldown = value;
			}
		}
	}

	[SerializeField]
	private Sprite m_icon;
	public Sprite Icon
	{
		get
		{
			return m_icon;
		}
		set
		{
			if (value != m_icon)
			{
				m_icon = value;
			}
		}
	}
}
