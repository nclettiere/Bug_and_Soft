using UnityEngine;

namespace Cutscenes.Cutscene1
{
    public class Cutscene1Interaction1 : MonoBehaviour
    {
        public void StartLevel1()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadLevel1();
            }
        }
    }
}