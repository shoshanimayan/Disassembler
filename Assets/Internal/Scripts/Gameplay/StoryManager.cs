using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace General
{
    public class StoryManager : Singleton<StoryManager>
    {
        ///////////////////////////////
        //  PRIVATE VARIABLES         //
        ///////////////////////////////
        private Dictionary<string, Vector3[]> _storyHolder = new Dictionary<string, Vector3[]> { };

        ///////////////////////////////
        //  PUBLIC API               //
        ///////////////////////////////
        public void SetDictionary(Dictionary<string, Vector3[]> storys)
        {
            _storyHolder = storys;
        }

        public Dictionary<string, Vector3[]> GetStorys()
        {
            return _storyHolder;

        }

        public Vector3[] GetStory(string key)
        {
            return _storyHolder[key];
        }

        public void UpdateStorys(string key, Vector3[] story)
        {
           
            if (_storyHolder.ContainsKey(key))
            {
                if (_storyHolder[key] == story)
                {
                    Debug.Log("No Changes");
                }
                else
                {
                    _storyHolder[key] = story;
                }
            }
            else
            {
                _storyHolder[key] = story;

            }
        }

        public string CreateUser()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
