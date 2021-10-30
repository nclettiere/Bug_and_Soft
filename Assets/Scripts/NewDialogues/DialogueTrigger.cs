using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewDialogues
{
	public class DialogueTrigger : MonoBehaviour
	{

		[FormerlySerializedAs("dialogue")] public NewDialogue newDialogue;

		public void TriggerDialogue()
		{
			//FindObjectOfType<DialogueManager>().StartDialogue(newDialogue);
		}
	}
}
