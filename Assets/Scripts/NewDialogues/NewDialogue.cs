using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace NewDialogues
{
	[System.Serializable]
	public class NewDialogue : MonoBehaviour
	{

		public string name;
		public LocalizedString[] sentences;
		public UnityEvent OnDialogueFinish;

		private void Awake()
		{
			OnDialogueFinish ??= new UnityEvent();
		}

		public ref UnityEvent GetOnFinishEvent()
		{
			return ref OnDialogueFinish;
		}
	}
}
