
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public class AudioController : MonoBehaviour
        {
            public static AudioController Instance;

            public AudioTrack[] Tracks;

            public Hashtable AudioTable; // relationship between audio clips (key) and audio tracks (value)
            private Hashtable m_JobTable;   // relationship between audio clips (key) or types and jobs (value) (Coroutine, IEnumerator)


            #region Extra Classes

            [System.Serializable]
            public class AudioTrack
            {
                public AudioType Type;
                public AudioSource Source;
                public List<AudioElement> AudioElements = new List<AudioElement>();
            }
            private class AudioJob
            {
                public AudioClip Clip;
                public AudioType Type;
                public AudioAction Action;               
                public bool Fade;
                public float Delay;
                
                public AudioJob(AudioClip clip, AudioType type, AudioAction action, bool fade, float delay)
                {
                    Clip = clip;                  
                    Type = type;
                    Action = action;
                    Fade = fade;
                    Delay = delay;
                }
                public AudioJob(AudioType type, AudioAction action, bool fade, float delay)
                {
                    Type = type;
                    Action = action;
                    Fade = fade;
                    Delay = delay;
                }
            }
            public enum AudioAction
            {
                START,
                STOP,
                RESTART
            }

            #endregion



            #region Unity Functions

            private void Awake()
            {
                if (Instance == null)
                {
                    Configure();
                }
            }
            private void OnDisable()
            {
                Dispose();
            }

            #endregion



            #region Public Functions

            public void PlayAudio(AudioClip clip, AudioType type, bool fade = false, float delay = 0f)
            {
                AddJobClip(new AudioJob(clip, type, AudioAction.START, fade, delay));
            }
            // restart and stop should not require a specific clip, but should realize what the currently playing clip is
            public void StopAudio(AudioType type, bool fade = false, float delay = 0f)
            {
                AddJobClip(new AudioJob(type, AudioAction.STOP, fade, delay));
            }
            public void RestartAudio(AudioType type, bool fade = false, float delay = 0f)
            {
                AddJobClip(new AudioJob(type, AudioAction.RESTART, fade, delay));
            }
            // adding of world sounds call from interactable/encounterable scripts
            public void AddAudioElement(AudioElement audioEM, int trackValue = 2) // default is world sounds track
            {
                if (AudioTable.ContainsKey(audioEM.Clip))
                {
                    Debug.Log("I've already been registered in the audio table");
                }
                else
                {
                    Tracks[trackValue].AudioElements.Add(audioEM);
                    AudioTable.Add(audioEM.Clip, Tracks[trackValue]);
                }            
            } 

            #endregion



            #region Private Functions

            private void Configure()
            {
                Instance = this;
                AudioTable = new Hashtable();
                m_JobTable = new Hashtable();
                GenerateAudioTableStart();
            }
            private void GenerateAudioTableStart()  
            {
                foreach (AudioTrack track in Tracks)
                {
                    foreach (AudioElement obj in track.AudioElements)
                    {
                        // do not duplicate keys
                        if (AudioTable.ContainsKey(obj.Clip))
                        {
                            Debug.Log("alrdy contains it");
                        }
                        else
                        {
                            AudioTable.Add(obj.Clip, track);
                        }
                    }
                }
            }
            private void Dispose()
            {
                foreach (DictionaryEntry entry in m_JobTable) // error here when transitioning scenes
                {
                    IEnumerator job = (IEnumerator)entry.Value;
                    StopCoroutine(job);
                }
            }
            private void AddJobClip(AudioJob job)
            {
                // remove conflicting jobs
                RemoveConflictingJobs(job.Type); // i think this ones fine

                // start job
                IEnumerator jobRunner = RunAudioJobClip(job);
                m_JobTable.Add(job.Type, jobRunner);
                StartCoroutine(jobRunner);
            }
            private IEnumerator RunAudioJobClip(AudioJob job)
            {
                yield return new WaitForSeconds(job.Delay);

                AudioTrack track = (AudioTrack)AudioTable[job.Type]; 

                switch (job.Type)
                {
                    case AudioType.OST:
                        track = Tracks[0];
                        break;
                    case AudioType.SFX_UI:
                        track = Tracks[1];
                        break;
                    case AudioType.SFX_World:
                        track = Tracks[2];
                        break;
                }

                switch (job.Action)
                {
                    case AudioAction.START:
                        track.Source.clip = GetAudioClipFromAudioTrack(job, track); // this is where clips get assigned
                        track.Source.Play();
                        break;
                    case AudioAction.STOP:
                        if (job.Fade == false)
                        {
                            track.Source.Stop();
                        }
                        break;
                    case AudioAction.RESTART:
                        track.Source.Stop();
                        track.Source.Play();
                        break;
                }

                if (job.Fade == true)
                {
                    float initial = job.Action == AudioAction.START || job.Action == AudioAction.RESTART ? 0.0f : 1.0f;
                    float target = initial == 0 ? 1 : 0;
                    float duration = 1.0f;
                    float timer = 0.0f;

                    while (timer <= duration)
                    {
                        track.Source.volume = Mathf.Lerp(initial, target, timer / duration);
                        timer += Time.deltaTime;
                        yield return null;
                    }

                    if (job.Action == AudioAction.STOP)
                    {
                        track.Source.Stop();
                    }
                }

                m_JobTable.Remove(job.Type);

                Debug.Log("Job count + " + m_JobTable.Count);

                yield return null;
            }
            private AudioClip GetAudioClipFromAudioTrack(AudioJob job, AudioTrack track)
            {
                foreach (AudioElement trackElement in track.AudioElements)
                {
                    if (trackElement.Clip == job.Clip) 
                    {
                        return trackElement.Clip;
                    }
                }
                return null;
            }
            private void RemoveConflictingJobs(AudioType type)
            {
                if (m_JobTable.ContainsKey(type))
                {
                    RemoveJob(type);
                }

                AudioType conflictAudio = AudioType.None;
                foreach (DictionaryEntry entry in m_JobTable)
                {
                    AudioType audioType = (AudioType)entry.Key;                 
                    AudioTrack audioTrackInUse = (AudioTrack)AudioTable[audioType];  // write this differently

                    //AudioTrack track = (AudioTrack)AudioTable[job.Type];
                    //switch (job.Type)
                    //{
                    //    case AudioType.OST:
                    //        track = Tracks[0];
                    //        break;
                    //    case AudioType.SFX_UI:
                    //        track = Tracks[1];
                    //        break;
                    //    case AudioType.SFX_World:
                    //        track = Tracks[2];
                    //        break;
                    //}


                    Debug.Log((AudioTrack)AudioTable[audioType] + "using this track");
                    AudioTrack audioTrackNeeded = (AudioTrack)AudioTable[type];

                    if (audioTrackNeeded.Source == audioTrackInUse.Source)  // bug here when calling play of same type close to each other
                    {
                        // conflict
                        conflictAudio = audioType;
                    }
                }
                if (conflictAudio != AudioType.None)
                {
                    RemoveJob(conflictAudio);
                }
            }
            private void RemoveJob(AudioType type)
            {
                if (m_JobTable.ContainsKey(type) == false)
                {
                    Debug.Log("trying to stop a job that is not running");
                }

                IEnumerator runningJob = (IEnumerator)m_JobTable[type];
                StopCoroutine(runningJob);
                m_JobTable.Remove(type);
            }

            #endregion
        }
    }
}
