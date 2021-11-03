using System;
using System.Collections;
using CameraManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Cutscenes.Cutscene1
{
    public class Cutscene1Interaction0 : MonoBehaviour
    {
        private Animator _animator;
        private Animator _swordAnimator;

        [SerializeField] private QuickChatDialogue qcDialogue;

        [SerializeField] private DynamicCamera _dynamicCamera;
        [SerializeField] private Sprite _dialogo;
        [SerializeField] private Transform _dungeonEntrance;
        [SerializeField] private GameObject _fadeOutEffect;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            
            //SceneManager.MoveGameObjectToScene(GameObject.Find("/Managers"), SceneManager.GetActiveScene());
        }

        public void OnWalkEnded()
        {
            //_animator.SetBool("EnterPose1", true);
            //sword.SetActive(true);
            StartCoroutine(StartCameraAnim());
        }

        private IEnumerator StartCameraAnim()
        {
            qcDialogue.ShowNewChat(new Tuple<Sprite, int>(_dialogo, 3));
            yield return new WaitForSeconds(6);
            _dynamicCamera.UpdateSize(7, 1);
            _dynamicCamera.UpdateOffsetY(0);
            yield return new WaitForSeconds(1);
            _dynamicCamera.ChangeTarget(_dungeonEntrance);
            yield return new WaitForSeconds(12);
            _dynamicCamera.UpdateSize(2, 1.5f);
            _fadeOutEffect.SetActive(true);
            yield return 0;
        }
    }
}