using System;
using System.Collections;
using CameraManagement;
using UnityEngine;

namespace Cutscenes.Cutscene0
{
    public class Interaction0 : MonoBehaviour
    {
        public GameObject sword;
        
        private Animator _animator;
        private Animator _swordAnimator;
        
        [SerializeField] private DynamicCamera _dynamicCamera;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _swordAnimator = sword.GetComponent<Animator>();
        }

        public void OnWalkEnded()
        {
            _animator.SetBool("EnterPose1", true);
            sword.SetActive(true);
            StartCoroutine(StartSwordAnim());
        }

        private IEnumerator StartSwordAnim()
        {
            yield return new WaitForSeconds(3);
            _animator.SetBool("EnterPose1", false);
            _animator.SetBool("EnterPose2", true);
            yield return new WaitForSeconds(1);
            _dynamicCamera.ChangeTarget(sword.transform);
            _swordAnimator.SetBool("GoForAltar", true);
        }

    }
}