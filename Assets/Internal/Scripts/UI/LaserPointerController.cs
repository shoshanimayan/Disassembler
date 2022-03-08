using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

namespace UI
{
	public class LaserPointerController: MonoBehaviour
	{

        ///////////////////////////////
        //  INSPECTOR VARIABLES      //
        ///////////////////////////////
        [SerializeField] GameObject _reticle;
        ///////////////////////////////
        //  PRIVATE VARIABLES         //
        ///////////////////////////////
        private XRRayInteractor _xrray;
        private LineRenderer _linerender;
        private XRInteractorLineVisual _xrLineVis;
        private GameObject _hand;
        ///////////////////////////////
        //  PRIVATE METHODS           //
        ///////////////////////////////
        private void Start()
        {
            _hand = gameObject;
            _xrray = _hand.GetComponent<XRRayInteractor>();
            _linerender = _hand.GetComponent<LineRenderer>();
            _xrLineVis = _hand.GetComponent<XRInteractorLineVisual>();
            _reticle.SetActive(false);
        }

        private void SetHandStatus(bool active)
        {
            if (active)
            {
                _linerender.enabled = true;
                _xrLineVis.enabled = true;
                _xrray.maxRaycastDistance = 5;
               _reticle.SetActive(true); 

            }
            else
            {
                _linerender.enabled = false;
                _xrLineVis.enabled = false;
                _xrray.maxRaycastDistance = 5;
                _reticle.SetActive(false); 

            }
        }

        private void CheckIfPointingAtMenu()
        {
            RaycastResult hit;
            if (_xrray.TryGetCurrentUIRaycastResult(out hit))
            {
                SetHandStatus(true);
                _reticle.transform.position = hit.worldPosition;

            }
            else 
            {
                SetHandStatus(false);

            }

        }

        private void Update()
        {
            CheckIfPointingAtMenu();
        }

        private void OnDisable()
        {
            if (_reticle)
            {
                _reticle.SetActive(false);
            }
        }



    }
}
