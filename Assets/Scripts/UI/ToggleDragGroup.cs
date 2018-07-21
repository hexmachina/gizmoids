using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

[DisallowMultipleComponent]
public class ToggleDragGroup : UIBehaviour
{

	[SerializeField] private bool m_AllowSwitchOff = false;
	public bool allowSwitchOff { get { return m_AllowSwitchOff; } set { m_AllowSwitchOff = value; } }

	private List<ToggleDrag> m_Toggles = new List<ToggleDrag>();

	protected ToggleDragGroup()
	{ }

	private void ValidateToggleIsInGroup(ToggleDrag toggle)
	{
		if (toggle == null || !m_Toggles.Contains(toggle))
			throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] { toggle, this }));
	}

	public void NotifyToggleOn(ToggleDrag toggle)
	{
		ValidateToggleIsInGroup(toggle);

		// disable all toggles in the group
		for (var i = 0; i < m_Toggles.Count; i++)
		{
			if (m_Toggles[i] == toggle)
				continue;

			m_Toggles[i].isOn = false;
		}
	}

	public void UnregisterToggle(ToggleDrag toggle)
	{
		if (m_Toggles.Contains(toggle))
			m_Toggles.Remove(toggle);
	}

	public void RegisterToggle(ToggleDrag toggle)
	{
		if (!m_Toggles.Contains(toggle))
			m_Toggles.Add(toggle);
	}

	public bool AnyTogglesOn()
	{
		return m_Toggles.Find(x => x.isOn) != null;
	}

	public IEnumerable<ToggleDrag> ActiveToggles()
	{
		return m_Toggles.Where(x => x.isOn);
	}

	public void SetAllTogglesOff()
	{
		bool oldAllowSwitchOff = m_AllowSwitchOff;
		m_AllowSwitchOff = true;

		for (var i = 0; i < m_Toggles.Count; i++)
			m_Toggles[i].isOn = false;

		m_AllowSwitchOff = oldAllowSwitchOff;
	}
}
