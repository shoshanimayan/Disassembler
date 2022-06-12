using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
namespace Gameplay.Height
{
    
	public class HeightHandler: Singleton<HeightHandler>
    {

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] GameObject[] _toBeAdjusted;
        [SerializeField] Transform _handle;

        ///////////////////////////////
        //  PRIVATE VARIABLES         //
        ///////////////////////////////
        private XRGrabInteractable _grabInteractor => GetComponent<XRGrabInteractable>();
        private bool _grabbed;
        private float _heightCurrent;
        private bool _interactable;
        private float _originalHeight;
        private bool _initalized;
        ///////////////////////////////
        //  PRIVATE METHODS           //
        ///////////////////////////////

        private void Awake()
        {
            _originalHeight = transform.position.y; 
        }

        private void OnEnable()
        {
            _heightCurrent = transform.position.y ;
            _grabInteractor.selectEntered.AddListener(GrabbedBy);
            _grabInteractor.selectExited.AddListener(GrabEnd);
        }
        private void OnDisable()
        {
            _grabInteractor.selectEntered.RemoveListener(GrabbedBy);
            _grabInteractor.selectExited.RemoveListener(GrabEnd);
        }

        private void GrabEnd(SelectExitEventArgs arg0)
        {
            _grabbed = false;
        }

        private void GrabbedBy(SelectEnterEventArgs arg0)
        {

            _grabbed = true;
        }

        private void IncreaseHeight(GameObject obj, float yAmount)
        {
            obj.transform.position += new Vector3(0,yAmount,0);

        }

        private void IncreaseHeightAllObjects(float yAmount)
        {
            foreach (var obj in _toBeAdjusted)
            {
                IncreaseHeight(obj, yAmount);
            }
        }

        private float GetHeightDifference()
        {
            float newHeight = transform.position.y;
            var difference = newHeight - _heightCurrent;

            _heightCurrent = newHeight;
            return difference;


        }

        private void Update()
        {
            if (_interactable && _grabbed)
            {

                IncreaseHeightAllObjects(GetHeightDifference());
               
            }
        }

        private void ShowHideHandle(bool show)
        {
            if (show)
            {
                _handle.DOMoveZ(_handle.position.z - .2f, .5f);

            }
            else 
            {
                _handle.DOMoveZ(_handle.position.z+.2f,.5f);
            }
        }

        ///////////////////////////////
        //  PUBLIC API               //
        ///////////////////////////////
        public  void SetInteractable(bool interact)
        {
            _interactable = interact;
            if (!_initalized)
            {
                _initalized = true;
                return;
            }
            ShowHideHandle(interact);
        }

        public float GetHeightChange()
        {
            return transform.position.y - _originalHeight;
        
        }

        public void SetInitalHeight(float change)
        {
            IncreaseHeightAllObjects(change);
            transform.position += new Vector3(0, change, 0);
        }


    }
}
