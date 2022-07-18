using General;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
namespace Gameplay
{

    public class Rotator : Interactable
    {
        ///////////////////////////////
        //  INSPECTOR VARIABLES      //
        ///////////////////////////////
        [SerializeField] Transform _linkedDial;
        [SerializeField] private int _snapRotationAmount = 25;
        [SerializeField] private float _angleTolerance;
        [SerializeField] private float _goalRotationPercentage;
        [SerializeField] private UnityEvent _onReachedRotation;
        [SerializeField] private bool _TurnOffOnEvent = true;
        ///////////////////////////////
        //  PRIVATE VARIABLES         //
        ///////////////////////////////
        private XRBaseInteractor _interactor;
        private float _startAngle;
        private bool _requiresStartAngle = true;
        private bool _shouldGetHandRotation = false;
        private bool _activated;
        private float _totalRotation;
        private Transform parent;

        private AudioManager _audioManager { get { return AudioManager.Instance; } }
        private GameStateController _gameState { get { return GameStateController.Instance; } }

        private XRGrabInteractable _grabInteractor => GetComponent<XRGrabInteractable>();
        ///////////////////////////////
        //  PRIVATE METHODS           //
        ///////////////////////////////
        private void OnEnable()
        {
            _totalRotation = 0;
            _activated = false;
            _grabInteractor.selectEntered.AddListener(GrabbedBy);
            _grabInteractor.selectExited.AddListener(GrabEnd);
            transform.parent = parent;

        }
        private void OnDisable()
        {
            _grabInteractor.selectEntered.RemoveListener(GrabbedBy);
            _grabInteractor.selectExited.RemoveListener(GrabEnd);
        }
        private void Awake()
        {
            parent = transform.parent;
        }

        private void GrabEnd(SelectExitEventArgs arg0)
        {
            _shouldGetHandRotation = false;
            _requiresStartAngle = true;
            transform.parent = parent;

        }

        private void GrabbedBy(SelectEnterEventArgs arg0)
        {
            _interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
            _interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            _shouldGetHandRotation = true;
            _startAngle = 0f;
            transform.parent = parent;

        }


        private void Update()
        {
            if (_shouldGetHandRotation && _interactable)
            {
                var rotationAngle = GetInteractorRotation(); //gets the current controller angle
                GetRotationDistance(rotationAngle);
            }

        }


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
                               // RotateDialClockwise();
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
                         //   RotateDialClockwise();
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

        private void RotateDialAntiClockwise()
        {
            _linkedDial.eulerAngles = new Vector3(_linkedDial.eulerAngles.x,
                                                      _linkedDial.eulerAngles.y,
                                                      _linkedDial.eulerAngles.z + _snapRotationAmount);
            RotationChanged(_snapRotationAmount);
        }

        private void RotationChanged(float rotVal)
        {
            _totalRotation += rotVal;
            if (_totalRotation>= _goalRotationPercentage)
            {
                if (!_activated) {
                    _interactable = false;
                    _activated = true;
                    _audioManager.PlayClip("blip");

                    _onReachedRotation.Invoke();
                    transform.parent = parent;

                    if (_TurnOffOnEvent)
                    {
                        gameObject.SetActive(false);

                    }

                }
            }
        }

        ///////////////////////////////
        //  PUBLIC API               //
        ///////////////////////////////

        public float GetInteractorRotation() => _interactor.GetComponent<Transform>().eulerAngles.z;

       

      
    }
}

