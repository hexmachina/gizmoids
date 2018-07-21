using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProfileSelectPanel : MonoBehaviour
{
	[System.Serializable]
	public class UnityEventProfile : UnityEvent<GizLib.Profile> { }

	public Transform profileListContainer;

	public ProfileButton profileButtonPrefab;

	public AlertMessage alertMessage;

	public GameObject createProfilePrompt;

	public UnityEventProfile onProfileChanged = new UnityEventProfile();

	private void OnEnable()
	{
		if (LocalDatabase.Instance.userProfiles.Count == 0)
		{
			createProfilePrompt.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
		else if (profileListContainer.transform.childCount != LocalDatabase.Instance.userProfiles.Count)
		{
			RefreshPanel();
		}
	}

	public void RefreshPanel()
	{
		for (int i = 0; i < profileListContainer.transform.childCount; i++)
		{
			Destroy(profileListContainer.transform.GetChild(i).gameObject);
		}

		foreach (var item in LocalDatabase.Instance.userProfiles)
		{
			BuildProfileButton(item);
		}
	}

	public void BuildProfileButton(GizLib.Profile profile)
	{
		var pro = Instantiate(profileButtonPrefab);
		pro.transform.SetParent(profileListContainer);
		pro.transform.localScale = Vector3.one;
		pro.gameObject.name = profile.name;
		pro.profile = profile;
		if (pro.text)
		{
			pro.text.text = profile.name;
		}

		pro.buttonSelect.onClick.AddListener(() => SelectProfile(profile));

		pro.buttonRemove.onClick.AddListener(() => RemoveProfile(profile, pro.gameObject));
	}

	public void SelectProfile(GizLib.Profile profile)
	{
		LocalDatabase.Instance.activeProfile = profile;
		LocalDatabase.Instance.SetLastProfileIndex(profile);

		onProfileChanged.Invoke(profile);
		gameObject.SetActive(false);
	}

	public void RemoveProfile(GizLib.Profile profile, GameObject gameObject)
	{
		LocalDatabase.Instance.DeleteProfile(profile);
		onProfileChanged.Invoke(profile);
		Destroy(gameObject);
	}

	public void OnNewProfileSubmit(InputField inputField)
	{
		if (string.IsNullOrEmpty(inputField.text))
		{
			alertMessage.ShowMessage("Invalid Profile Name", "No Profile Name was submitted. Enter a new Profile Name.");
		}
		else if (LocalDatabase.Instance.userProfiles.Exists(x => x.name == inputField.text))
		{
			alertMessage.ShowMessage("Invalid Profile Name", inputField.text + " already exists. Enter a new Profile Name.");
			inputField.text = "";
		}
		else
		{
			var pro = LocalDatabase.Instance.CreateProfile(inputField.text);
			inputField.text = "";
			if (createProfilePrompt && createProfilePrompt.activeSelf)
			{
				createProfilePrompt.SetActive(false);
			}
			onProfileChanged.Invoke(pro);
			RefreshPanel();
		}
	}
}
