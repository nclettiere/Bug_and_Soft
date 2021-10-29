using System;
using System.Collections;
using CameraManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cutscenes.Cutscene0
{
    public class Interaction1 : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private AudioSource _swordClip;
        [SerializeField] private AudioSource _finalHorn;
        [SerializeField] private CanvasGroup _blackScreen;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnAnimEnded()
        {
            _animator.SetBool("GoForAltar", false);
            _animator.SetBool("OnAltar", true);
        }
        
        public void PlaySwordSFX()
        {
            _swordClip.Play();

            StartCoroutine(HornPlayDelayed());
        }

        private IEnumerator HornPlayDelayed()
        {
            yield return new WaitForSeconds(1);
            _finalHorn.Play();
            yield return new WaitForSeconds(1);
            
            float counter = 0f;

            while (counter < 4f)
            {
                counter += Time.deltaTime;
                _blackScreen.alpha = Mathf.Lerp(0, 1, counter / 2f);
                yield return null;
            }

            yield return new WaitUntil(() => _finalHorn.isPlaying);

            SceneManager.MoveGameObjectToScene(Camera.main.gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene("Creditos", LoadSceneMode.Single);
            
            yield return 0;
        }
    }
}