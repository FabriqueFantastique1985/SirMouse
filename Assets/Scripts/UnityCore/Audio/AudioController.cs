
using System.Collections;
using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public class AudioController : MonoBehaviour
        {
            public static AudioController Instance;

            public AudioTrack[] Tracks;

            private Hashtable m_AudioTable; // relationship between audio types (key) and audio tracks (value)
            private Hashtable m_JobTable;   // relationship between audio types (key) and jobs (value) (Coroutine, IEnumerator)



            #region Extra Classes

            [System.Serializable]
            public class AudioObject
            {
                public AudioType Type;
                public AudioClip Clip;
            }

            [System.Serializable]
            public class AudioTrack
            {
                public AudioSource Source;
                public AudioObject[] AudioObj;
            }

            private class AudioJob
            {
                public AudioAction Action;
                public AudioType Type;
                public bool Fade;
                public float Delay;

                public AudioJob(AudioAction action, AudioType type, bool fade, float delay)
                {
                    Action = action;
                    Type = type;
                    Fade = fade;
                    Delay = delay;
                }
            }
            private enum AudioAction
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

            public void PlayAudio(AudioType type, bool fade = false, float delay = 0f)
            {
                AddJob(new AudioJob(AudioAction.START, type, fade, delay));
            }
            public void StopAudio(AudioType type, bool fade = false, float delay = 0f)
            {
                AddJob(new AudioJob(AudioAction.STOP, type, fade, delay));
            }
            public void RestartAudio(AudioType type, bool fade = false, float delay = 0f)
            {
                AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay));
            }

            #endregion



            #region Private Functions

            private void Configure()
            {
                Instance = this;
                m_AudioTable = new Hashtable();
                m_JobTable = new Hashtable();
                GenerateAudioTable();
            }

            private void GenerateAudioTable()
            {
                foreach (AudioTrack track in Tracks)
                {
                    foreach (AudioObject obj in track.AudioObj)
                    {
                        // do not duplicate keys
                        if (m_AudioTable.ContainsKey(obj.Type))
                        {
                            Debug.Log("alrdy contains it");
                        }
                        else
                        {
                            m_AudioTable.Add(obj.Type, track);
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

            private void AddJob(AudioJob job)
            {
                // remove conflicting jobs
                RemoveConflictingJobs(job.Type);

                // start job
                IEnumerator jobRunner = RunAudioJob(job);
                m_JobTable.Add(job.Type, jobRunner);
                StartCoroutine(jobRunner);
            }

            private IEnumerator RunAudioJob(AudioJob job)
            {
                yield return new WaitForSeconds(job.Delay);

                AudioTrack track = (AudioTrack)m_AudioTable[job.Type];
                track.Source.clip = GetAudioClipFromAudioTrack(job.Type, track);

                switch (job.Action)
                {
                    case AudioAction.START:
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


            private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
            {
                foreach (AudioObject obj in track.AudioObj)
                {
                    if (obj.Type == type)
                    {
                        return obj.Clip;
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
                    AudioTrack audioTrackInUse = (AudioTrack)m_AudioTable[audioType];
                    AudioTrack audioTrackNeeded = (AudioTrack)m_AudioTable[type];

                    if (audioTrackNeeded.Source == audioTrackInUse.Source)
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

