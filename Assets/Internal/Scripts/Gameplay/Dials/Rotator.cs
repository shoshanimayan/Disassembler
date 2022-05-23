using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
namespace Gameplay
{

    public class Rotator : MonoBehaviour
    {
        ///////////////////////////////
        //  INSPECTOR VARIABLES      //
        ///////////////////////////////
        [SerializeField] Transform _linkedDial;
        [SerializeField] private int _snapRotationAmount = 25;
        [SerializeField] private float _angleTolerance;
        [SerializeField] private float _goalRotationPercentage;
        [SerializeField] private UnityEvent _onReachedRotation;

        ///////////////////////////////
        //  PRIVATE VARIABLES         //
        ///////////////////////////////
        private XRBaseInteractor _interactor;
        private float _startAngle;
        private bool _requiresStartAngle = true;
        private bool _shouldGetHandRotation = false;
        private bool _activated;
        private XRGrabInteractable _grabInteractor => GetComponent<XRGrabInteractable>();
        ///////////////////////////////
        //  PRIVATE METHODS           //
        ///////////////////////////////
        private void OnEnable()
        {
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
            _shouldGetHandRotation = false;
            _requiresStartAngle = true;
        }

        private void GrabbedBy(SelectEnterEventArgs arg0)
        {
            _interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
            _interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            _shouldGetHandRotation = true;
            _startAngle = 0f;

        }


        void Update()
        {
            if (_shouldGetHandRotation)
            {
                var rotationAngle = GetInteractorRotation(); //gets the current controller angle
                GetRotationDistance(rotationAngle);
            }
        }

        public float GetInteractorRotation() => _interactor.GetComponent<Transform>().eulerAngles.z;

        #region TheMath!
        private void GetRotationDistance(float currentAngle)
        {
            if (!_requiresStartAngle)
            {
                var angleDifference = Mathf.Abs(_startAngle - currentAngle);

                if (angleDifference > _angleTolerance)
                {
                    if (angleDifference > 270f) //checking to see if the user has gone from 0-360 - a very tiny movement but will trigger the angletolerance
                    {
                        float angleCheck;

                        if (_startAngle < currentAngle)
                        {
                            angleCheck = CheckAngle(currentAngle, _startAngle);

                            if (angleCheck < _angleTolerance)
                                return;
                            else
                            {
                                RotateDialClockwise();
                                _startAngle = currentAngle;
                            }
                        }
                        else if (_startAngle > currentAngle)
                        {
                            angleCheck = CheckAngle(currentAngle, _startAngle);

                            if (angleCheck < _angleTolerance)
                                return;
                            else
                            {
                                RotateDialAntiClockwise();
                                _startAngle = currentAngle;
                            }
                        }
                    }
                    else
                    {
                        if (_startAngle < currentAngle)
                        {
                            RotateDialAntiClockwise();
                            _startAngle = currentAngle;
                        }
                        else if (_startAngle > currentAngle)
                        {
                            RotateDialClockwise();
                            _startAngle = currentAngle;
                        }
                    }
                }
            }
            else
            {
                _requiresStartAngle = false;
                _startAngle = currentAngle;
            }
        }
        #endregion

        private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

        private void RotateDialClockwise()
        {
            _linkedDial.localEulerAngles = new Vector3(_linkedDial.localEulerAngles.x,
                                                      _linkedDial.localEulerAngles.y,
                                                      _linkedDial.localEulerAngles.z + _snapRotationAmount);

            RotationChanged(_linkedDial.localEulerAngles.z);

        }

        private void RotateDialAntiClockwise()
        {
            _linkedDial.localEulerAngles = new Vector3(_linkedDial.localEulerAngles.x,
                                                      _linkedDial.localEulerAngles.y,
                                                      _linkedDial.localEulerAngles.z - _snapRotationAmount);

            RotationChanged(_linkedDial.localEulerAngles.z);
        }

        private void RotationChanged(float rotVal)
        {
            if (rotVal > _goalRotationPercentage)
            {
                if (!_activated) {
                    _activated = true;
                _onReachedRotation.Invoke();
                }
            }
        }
    }
}

