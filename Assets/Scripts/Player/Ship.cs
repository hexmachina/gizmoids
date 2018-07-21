using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Ship : MonoBehaviour, IImpactable
{
	public Animator animator;

	public AudioClip clipThrust;

	public bool thrusting;

	public GizLib.UnityEventBool onThrustChanged = new GizLib.UnityEventBool();

	public UnityEvent onImpact = new UnityEvent();

	private AudioSource audioSource;

	public void StartThrusters()
	{
		if (animator)
		{
			animator.SetBool("Thrust", true);
		}
		BackgroundManager.instance.currentBackground.AdjustScrollSpeeds(0.05f);
	}

	public void StopThrusters()
	{
		if (animator)
		{
			animator.SetBool("Thrust", false);
		}
		BackgroundManager.instance.currentBackground.ResetScrollSpeeds();
	}

	public void EnableThrusters(bool enabled)
	{
		if (thrusting != enabled)
		{
			thrusting = enabled;
			onThrustChanged.Invoke(thrusting);
		}
		if (enabled)
		{
			if (!audioSource)
			{
				audioSource = AudioPlayer.Instance.PlaySoundClip(clipThrust);
				audioSource.loop = true;
			}
		}
		else
		{
			if (audioSource)
			{
				audioSource.Stop();
				audioSource.loop = false;
				audioSource = null;
			}
		}
	}

	public void Impact(GameObject gameObject)
	{
		//throw new System.NotImplementedException();
		onImpact.Invoke();
	}
}
