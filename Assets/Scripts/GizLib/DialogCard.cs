using System;

namespace GizLib
{
	public enum CharacterRoster
	{
		Joe = 0,
		Mopey = 1
	}

	public enum Emotions
	{
		Happy,
		Angry,
		Scared,
		Confused,
		Explaining,
		Ooh,
		Worried,
		Laugh,
		Think,
		Bonk
	}

	[Serializable]
	public class DialogCard
	{
		public string text;
		public string header;
		public bool onRight;
		public CastMemberData castMember;
		public Emotions emotion;
		public CharacterRoster cast;
	}
}
