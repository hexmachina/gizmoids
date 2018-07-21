using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
	public Text textHeader;

	[UnityEngine.Serialization.FormerlySerializedAs("textDody")]
	public Text textBody;

	public Button buttonContinue;

	public Text textButton;

	public void ShowMessage(string header, string body, System.Action callback = null)
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		transform.SetAsLastSibling();

		textHeader.text = header;

		textBody.text = body;

		buttonContinue.onClick.RemoveAllListeners();
		if (callback != null)
		{
			buttonContinue.onClick.AddListener(callback.Invoke);
		}
	}

}
