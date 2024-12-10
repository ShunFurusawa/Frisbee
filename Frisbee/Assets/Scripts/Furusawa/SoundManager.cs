﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Furusawa
{
    public class SoundManager : MonoBehaviour
    {
        [Serializable]
        public class BGMData
        {
            public string    name; 
            public AudioClip audioClip;
            [Range(0f, 1f)]
            public float     volume;
        }

        [Serializable]
        public class SEData
        {
            public string    name;
            public AudioClip audioClip;
            [HideInInspector]
            public float     playedTime;  // 前回再生した時間 ※publicじゃないと動かないかも
            [Range(0f, 1f)]
            public float     volume;
        }

        [Serializable]
        private class AudioMixerGroups
        {
            public AudioMixerGroup BGM;
            public AudioMixerGroup SE;
        }
        
        [SerializeField] private BGMData[] bgmDatas;
        [SerializeField] private SEData[]  SEDatas;
        [Header("一度再生してから、次再生出来るまでの間隔(秒)")]
        [SerializeField] private　float playableDistance = 0.2f;
        
        //別名(name)をキーとした管理用Dictionary
        private Dictionary<string, BGMData> BGMDictionary = new Dictionary<string, BGMData>();
        private Dictionary<string, SEData> SEDictionary = new Dictionary<string, SEData>();
        
        public static SoundManager instance = null;
        
        //AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
        private AudioSource[] audioSourceList = new AudioSource[20];
        [SerializeField] private AudioMixerGroups _audioMixerGroups;

        private void SetInstance()
        {
            if (instance == null)
            {
                instance = this;
              //  DontDestroyOnLoad(gameObject);
            }
            else
            {
              //  Destroy(gameObject);
            }
        }
        private void Awake()
        {
            for (int i = 0; i < audioSourceList.Length; i++)
            {
                audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            }
            
            //それぞれDictionaryにセット
            foreach (BGMData bgmData in bgmDatas)
            {
                BGMDictionary.Add(bgmData.name, bgmData);
            }

            foreach (SEData SEData in SEDatas)
            {
                SEDictionary.Add(SEData.name, SEData);
            }
            
            SetInstance();
        }

        /// <summary>
        /// 未使用のAudioSourceの取得 全て使用中ならnull
        /// </summary>
        private AudioSource GetUnusedAudioSource()
        {
            for (int i = 0; i < audioSourceList.Length; i++)
            {
                if (audioSourceList[i].isPlaying == false)
                    return audioSourceList[i];
            }
            return null; 
        }
        
        public void PlayBGM(AudioClip bgm, float volume)
        {
            // 同時に鳴らす場合を考慮して毎回変数生成する(必要ないかも)
            AudioSource audioSource = GetUnusedAudioSource();

            if (audioSource == null)
            {
                Debug.Log("BGM play failed");
                return;
            }

            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.clip = bgm;
            audioSource.outputAudioMixerGroup = _audioMixerGroups.BGM;
            audioSource.Play();
        }

        public void PlaySE(AudioClip clip, float volume)
        {
            // 同時に鳴らす場合を考慮して毎回変数生成する(必要ないかも)
            AudioSource audioSource = GetUnusedAudioSource();

            if (audioSource == null)
            {
                Debug.Log("BGM play failed");
                return;
            }

            audioSource.volume = volume;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = _audioMixerGroups.SE;
            audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// これを再生したいタイミングで呼び出す。名前を指定してBGM、SE一覧から検索、再生。
        /// </summary>
        /// <param name="name">AudioClipの名前でなく登録した別名</param>
        public void Play(string name)
        {
            // それぞれの管理用Dictionaryから別名で検索、一致したら再生
            if (BGMDictionary.TryGetValue(name, out BGMData bgmData))
            {
                PlayBGM(bgmData.audioClip, bgmData.volume);
                Debug.Log("BGM play: " + name);
            }
            else if (SEDictionary.TryGetValue(name, out SEData seData))
            {
                if(Time.realtimeSinceStartup - seData.playedTime < playableDistance)
                    return;
                
                seData.playedTime = Time.realtimeSinceStartup; 　//次回用に今回の再生時間の保持 
                PlaySE(seData.audioClip, seData.volume);
                Debug.Log("SE play: " + name);
            }
            else
            {
                Debug.LogWarning("その別名は登録されていません:{name}");
            }
        }

    }
}