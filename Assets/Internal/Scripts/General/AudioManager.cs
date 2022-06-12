using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;
namespace General
{
    public class AudioManager : Singleton<AudioManager>
    {
        /////////////////////////
        // INSPECTOR VARIABLES //
        /////////////////////////
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private AudioClip[] _themeClips;
        /////////////////////////
        //  PRIVATE VARIABLES  //
        /////////////////////////
        private AudioSource _as;
        private AudioSource _asCamera;

        private void Awake()
        {
            _as = GetComponent<AudioSource>();
            _asCamera = Camera.main.gameObject.GetComponent<AudioSource>();
        }

        //////////////////
        //  PUBLIC API  //
        /////////////////

        public void PlayClip(string name)
        {
            foreach (AudioClip clip in _audioClips)
            {
                if (clip.name == name)
                {
                    _asCamera.PlayOneShot(clip);
                    return;
                }
            }
            Debug.LogError("no clip "+name);

        }

        public void PlayClipWithAction(string name, System.Action DoAfter)
        {
            AudioClip PlayedClip=null;
            foreach (AudioClip clip in _audioClips)
            {
                if (clip.name == name)
                {
                    _asCamera.PlayOneShot(clip);
                    PlayedClip = clip;
                    break;
                }
            }
            if (PlayedClip!=null)
            {
                DOTween.Sequence().AppendInterval(PlayedClip.length).AppendCallback(() => DoAfter());
            }
        }

       

        public void StopClip()
        {
            _asCamera.Stop();
        }

        public void PlayMainTheme()
        {
            _as.clip = _themeClips[0];

            _as.Play();
        }
     

        public void PlayGameTheme()
        {
            _as.clip = _themeClips[1];
            _as.Play();
        }
        public void StopTheme()
        {
            _as.Stop();
        }


    }
}
