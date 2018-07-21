using System;
using System.Collections.Generic;

namespace GizLib
{
	public enum DialogType
	{
		None = 0,
		Character = 1,
		Tooltip = 2,
		Info = 3
		//Prompt = 3
	}

	[Serializable]
	public class Dialog
	{
		public DialogType dialogType = DialogType.None;
		public List<DialogCard> dialogCards = new List<DialogCard>();
	}
}
