using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using GizLib;

public class DialoguePanel : MonoBehaviour
{

	public Button buttonContinue;

	public Text textHeader;

	public Text textBody;

	public CanvasGroup dialogueBody;

	public RectTransform characterGroup;

	public DG.Tweening.Ease actorEnterance;

	public DG.Tweening.Ease actorExit;

	public UnityEvent onComplete = new UnityEvent();

	public List<CastMemberData> callSheet = new List<CastMemberData>();

	private Animator currentActor;

	private Dialog currentDialogue;

	private int cardIndex = 0;

	private Sequence sequence;

	public void StartDialogue(Dialog dialog)
	{
		currentDialogue = dialog;
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		transform.SetAsLastSibling();
		cardIndex = 0;
		ShowCard(cardIndex);
	}

	private void ShowCard(int index)
	{
		if (Time.timeScale == 0)
		{
			DOTween.defaultTimeScaleIndependent = true;
		}
		sequence = DOTween.Sequence();

		var card = currentDialogue.dialogCards[index];
		HandleCastMember(card.cast, card.emotion);
		textHeader.text = card.header;

		if (index == 0)
		{
			var rt = currentActor.transform as RectTransform;
			rt.anchoredPosition = new Vector2(2000, 0);
			dialogueBody.alpha = 0;
			sequence.SetUpdate(true);
			sequence.SetDelay(0.5f);
			sequence.Append(dialogueBody.DOFade(1, 0.75f));
			sequence.Join(rt.DOAnchorPos(new Vector2(400, 0), 0.75f).SetEase(actorEnterance))
				.AppendCallback(AddContinueButton);
		}
		textBody.text = string.Empty;
		sequence.Append(textBody.DOText(card.text, card.text.Length * 0.05f))
			.OnKill(OnKillSequence);
	}

	private void HandleCastMember(CharacterRoster cast, Emotions emotions)
	{
		var member = callSheet.Find(x => x.characterRoster == cast);
		if (!currentActor || currentActor.runtimeAnimatorController != member.animatorPrefab.runtimeAnimatorController)
		{
			if (currentActor)
			{
				Destroy(currentActor.gameObject);
			}
			currentActor = BuildCastMember(member.animatorPrefab);
		}
		SetCastEmotion(emotions);
	}

	public Animator BuildCastMember(Animator animator)
	{
		var actor = Instantiate(animator, characterGroup);
		actor.name = animator.name;
		actor.transform.localScale = Vector3.one;
		return actor;
	}

	public void SetCastEmotion(Emotions emotions)
	{
		if (currentActor)
		{
			try
			{
				currentActor.Play(emotions.ToString(), 0);
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
			}
		}
	}

	private void OnKillSequence()
	{
		textBody.text = currentDialogue.dialogCards[cardIndex].text;
		dialogueBody.alpha = 1;
		var rt = currentActor.transform as RectTransform;
		rt.anchoredPosition = new Vector2(400, 0);
	}

	public void AddContinueButton()
	{
		buttonContinue.onClick.AddListener(ContinueDialogue);
	}

	public void ContinueDialogue()
	{
		if (sequence != null && sequence.IsPlaying())
		{
			sequence.Kill();
			return;
		}

		cardIndex++;
		if (cardIndex < currentDialogue.dialogCards.Count)
		{
			ShowCard(cardIndex);
		}
		else
		{
			buttonContinue.onClick.RemoveListener(ContinueDialogue);
			var rt = currentActor.transform as RectTransform;
			sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(dialogueBody.DOFade(0, 0.5f));
			sequence.Join(rt.DOAnchorPos(new Vector2(2000, 0), 0.75f).SetEase(actorExit));
			sequence.OnComplete(DialogueCompleted);

		}
	}

	private void DialogueCompleted()
	{
		onComplete.Invoke();
		DOTween.defaultTimeScaleIndependent = false;
		gameObject.SetActive(false);
	}
}
