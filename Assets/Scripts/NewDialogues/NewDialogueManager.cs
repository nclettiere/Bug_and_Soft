using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace NewDialogues
{
	public class NewDialogueManager : MonoBehaviour
	{

		public TextMeshProUGUI nameText, dialogueText;

		public Animator animator;

		private int sentenceIndex;
		private Queue<string> sentences;
		private Queue<Tuple<UnityEvent, int>> DialogueEvents;

		// Use this for initialization
		void Start()
		{
			sentences = new Queue<string>();
			DialogueEvents = new Queue<Tuple<UnityEvent, int>>();
		}

		public void StartDialogue(NewDialogueGroup group)
		{
			GameManager.Instance.EnterDialogueMode();
			
			animator.SetBool("IsOpen", true);

			sentences.Clear();
			
			foreach (var dialogue in group.GetDialogues())
			{
				DialogueEvents.Enqueue(new Tuple<UnityEvent, int>(dialogue.GetOnFinishEvent(), dialogue.sentences.Length));
				nameText.text = dialogue.name;

				foreach (LocalizedString sentence in dialogue.sentences)
				{
					sentences.Enqueue(sentence.GetLocalizedString());
				}
			}

			DisplayNextSentence();
		}

		public void DisplayNextSentence()
		{
			if (sentences.Count == 0)
			{
				EndDialogue();
				return;
			}
			
			if (sentenceIndex == DialogueEvents.Peek().Item2 - 1)
			{
				Debug.Log("Executing Event");
				DialogueEvents.Dequeue().Item1.Invoke();
			}

			string sentence = sentences.Dequeue();
			sentenceIndex++;
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		IEnumerator TypeSentence(string sentence)
		{
			dialogueText.text = "";
			foreach (char letter in sentence.ToCharArray())
			{
				dialogueText.text += letter;
				yield return null;
			}
		}

		void EndDialogue()
		{
			sentenceIndex = 0;
			
			while (DialogueEvents.Count > 0)
			{
				Debug.Log("Executing Event");
				DialogueEvents.Dequeue().Item1.Invoke();
			}
			
			DialogueEvents.Clear();
			animator.SetBool("IsOpen", false);
			
			GameManager.Instance.ExitDialogueMode();
		}
		
		public void SkipAll()
		{
			
			while (DialogueEvents.Count > 0)
			{
				DialogueEvents.Dequeue().Item1.Invoke();
			}

			EndDialogue();
		}

	}
}
