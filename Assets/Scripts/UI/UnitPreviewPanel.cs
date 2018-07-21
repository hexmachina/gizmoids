using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPreviewPanel : MonoBehaviour
{
	public float bottomOffsetWithShelf;

	public float bottomOffsetNoShelf;

	public Vector3 cameraPosition = new Vector3(0, 1.5f, -10);

	public Text textHeader;

	public Text textBody;

	public Camera cameraPrefab;

	public RectTransform contentPanel;

	public GameObject buttonShelf;

	public Button buttonContinue;

	public Button buttonBackground;

	public GameObject preview;

	private Camera previewCamera;

	public void DisplayShelf(bool active)
	{
		buttonShelf.SetActive(active);
		if (active)
		{
			contentPanel.offsetMin = new Vector2(contentPanel.offsetMin.x, bottomOffsetWithShelf);
		}
		else
		{
			contentPanel.offsetMin = new Vector2(contentPanel.offsetMin.x, bottomOffsetNoShelf);
		}
	}

	public void BuildCamera()
	{
		if (!previewCamera)
		{
			previewCamera = Instantiate(cameraPrefab, cameraPosition, Quaternion.identity, null);
		}
	}

	public void SetPanel(UnitGraphics go, string header, string body, System.Action callback)
	{
		//gameObject.SetActive(true);
		//transform.SetAsLastSibling();
		SetButtonEvents(callback);

		SetPanel(go, header, body);
	}

	public void SetPanel(UnitGraphics go, string header, string body)
	{
		textBody.text = body;
		textHeader.text = header;
		BuildCamera();
		BuildPreview(go);
	}

	public void SetButtonEvents(System.Action callback)
	{
		if (callback == null)
		{
			return;
		}

		if (buttonBackground)
		{
			buttonBackground.onClick.RemoveAllListeners();
			buttonBackground.onClick.AddListener(callback.Invoke);
		}
		if (buttonContinue)
		{
			buttonContinue.onClick.RemoveAllListeners();
			buttonContinue.onClick.AddListener(callback.Invoke);
		}
	}

	public void RemovePreview()
	{
		if (preview)
		{
			Destroy(preview);
		}

	}

	public void BuildPreview(UnitGraphics gameObject)
	{
		RemovePreview();

		var ug = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		ug.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		preview = ug.gameObject;
		preview.layer = 8;

		var children = preview.GetComponentsInChildren<Transform>(true);
		foreach (var item in children)
		{
			item.gameObject.layer = 8;
		}
	}
}
