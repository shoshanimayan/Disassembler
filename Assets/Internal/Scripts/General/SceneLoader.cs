using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using Gameplay;

namespace General
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        /////////////////////////
        // INSPECTOR VARIABLES //
        /////////////////////////
        [SerializeField]
        private AssetReference firstScene;
        [SerializeField]

        /////////////////////////
        //  PRIVATE VARIABLES  //
        /////////////////////////
        private AsyncOperationHandle<SceneInstance> _handle;
        private bool _unloaded;
        private  GameStateController _gameState { get { return GameStateController.Instance; } }
        private TelaportController _telaportController { get { return TelaportController.Instance; } }

        ///////////////////////
        //  PRIVATE METHODS  //
        ///////////////////////
        private void Awake()
        {
            Application.targetFrameRate = 90;
            if (Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out var rate))
            {
                float newRate = 90f; // fallback to this value if the query fails.
                if (Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out var rates))
                {
                    newRate = rates.Max();
                }
                if (rate < newRate)
                {
                    if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(newRate))
                    {
                        Time.fixedDeltaTime = 1f / newRate;
                        Time.maximumDeltaTime = 1f / newRate;
                    }
                }
            }
        }

        private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _handle = obj;
                _unloaded = false;
                _telaportController.ResetRig();
                _gameState.GoToMenu();
            }
        }

        private void UnloadScene()
        {
            Debug.Log("unloading level");
       
            Addressables.UnloadSceneAsync(_handle, true).Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                    Debug.Log("Successfully unloaded scene.");
                else
                {
                    Debug.Log(op.Status.ToString());
                }
            };
        }

        //////////////////
        //  PUBLIC API  //
        /////////////////

        public void FirstLoad()
        {
            Addressables.LoadSceneAsync(firstScene, UnityEngine.SceneManagement.LoadSceneMode.Additive).Completed += SceneLoadCompleted;

        }

        public void Load(AssetReference scene)
        {
            Debug.Log("loading level");
            if (!_unloaded)
            {
                _unloaded = true;
                UnloadScene();
            }
            Addressables.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive).Completed += SceneLoadCompleted;
        }


    }
}
