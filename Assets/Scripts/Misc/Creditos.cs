using UnityEngine;

namespace Misc
{
    public class Creditos : MonoBehaviour
    {
        public void OnCreditosEnd()
        {
            GameManager.Instance.LoadLevel1Full();
        }
    }
}