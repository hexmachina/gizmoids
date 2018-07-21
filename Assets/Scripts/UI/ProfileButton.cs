using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GizLib;

public class ProfileButton : MonoBehaviour
{

	//public UILabel uiLabel;
	public Text text;

	public Button buttonSelect;

	public Button buttonRemove;

	public Profile profile;


	public void SelectProfile()
	{
		LocalDatabase.Instance.activeProfile = profile;
		LocalDatabase.Instance.SetLastProfileIndex(profile);
		//TitleManager.Instance.SetTitleState(TitleState.Start);
		//AudioPlayer.Instance.PlaySoundEnum(ButtonSound.NextSound, transform);
	}

	public void RemoveProfile()
	{
		LocalDatabase.Instance.DeleteProfile(profile);
		//TitleManager.Instance.RefreshProfilePanels();
	}

	public void PromptRemoveProfile()
	{
		//var alert = TitleManager.Instance.alert;
		//alert.ShowPrompt("Delete Profile", "Are you sure you want to delete " + profile.name + "?");
		//alert.SetButton1(RemoveProfile, alert.HidePrompt);
		//alert.SetButton2(alert.HidePrompt);
	}
}
