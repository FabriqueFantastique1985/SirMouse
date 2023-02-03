
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
            public int SourcesAmountOST, SourcesAmountUI, SourcesAmountSirMouse, SourcesAmountWorld; // assign these proper amount in inspector

            public Hashtable AudioTable; // relationship between audio clips (key) and audio tracks (value)
            private Hashtable m_JobTable;   // relationship between audio clips (key) or types and jobs (value) (Coroutine, IEnumerator)

            private AudioJob _lastViableJob;


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

            public void PlayAudio(AudioElement audioEm, bool fade = false, float delay = 0f)
            {
                AddJobClip(new AudioJob(audioEm.Clip, audioEm.Type, AudioAction.START, fade, delay));
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
            public void AddAudioElement(AudioElement audioEM) // default is world sounds track
            {
                if (AudioTable.ContainsKey(audioEM.Clip))
                {
                    Debug.Log("I've already been registered in the audio table");
                }
                else
                {
                    int neededTrackIndex = (int)audioEM.Type - 1;
                    Tracks[neededTrackIndex].AudioElements.Add(audioEM);
                    AudioTable.Add(audioEM.Clip, Tracks[neededTrackIndex]);
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
                IEnumerator jobRunner = null;

                if (job.Clip == null)
                {
                    job.Clip = _lastViableJob.Clip;
                    job.Type = _lastViableJob.Type;
                }

                // remove conflicting jobs
               // RemoveConflictingJobs(job.Clip); 
                // start job
                jobRunner = RunAudioJobClip(job);
                //m_JobTable.Add(job.Clip, jobRunner); // argument exception ??? "item has alrdy been added (dictionary)
                _lastViableJob = job;
                
                StartCoroutine(jobRunner);
            }
            private IEnumerator RunAudioJobClip(AudioJob job)
            {
                yield return new WaitForSeconds(job.Delay);

                //AudioTrack trackToUse = (AudioTrack)AudioTable[job.Type];
                AudioTrack trackToUse = null;
                trackToUse = FindTrackThatsCurrentlyNotPlaying(job.Type);
                //switch (job.Type)
                //{
                //    case AudioType.OST:
                //        trackToUse = FindTrackThatsCurrentlyNotPlaying(AudioType.OST, trackToUse);
                //        break;
                //    case AudioType.SFX_UI:
                //        trackToUse = FindTrackThatsCurrentlyNotPlaying(AudioType.SFX_UI, trackToUse);
                //        break;
                //    case AudioType.SFX_SirMouse:
                //        trackToUse = FindTrackThatsCurrentlyNotPlaying(AudioType.SFX_SirMouse, trackToUse);
                //        break;
                //    case AudioType.SFX_World: // making 4 tracks for World_Sounds                      
                //        trackToUse = FindTrackThatsCurrentlyNotPlaying(AudioType.SFX_World, trackToUse);
                //        break;
                //}

                switch (job.Action)
                {
                    case AudioAction.START:
                        trackToUse.Source.clip = job.Clip; //GetAudioClipFromAudioTrack(job, track); // this is where clips get assigned // NULLREF???
                        trackToUse.Source.Play();
                        break;
                    case AudioAction.STOP:
                        if (job.Fade == false)
                        {
                            trackToUse.Source.Stop();
                        }
                        break;
                    case AudioAction.RESTART:
                        trackToUse.Source.Stop();
                        trackToUse.Source.Play();
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
                        trackToUse.Source.volume = Mathf.Lerp(initial, target, timer / duration);
                        timer += Time.deltaTime;
                        yield return null;
                    }

                    if (job.Action == AudioAction.STOP)
                    {
                        trackToUse.Source.Stop();
                    }
                }

                //m_JobTable.Remove(job.Clip);

                //Debug.Log("Job count + " + m_JobTable.Count);

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
            private void RemoveConflictingJobs(AudioClip clip)
            {
                if (m_JobTable.ContainsKey(clip)) 
                {
                    RemoveJob(clip);
                }

                AudioClip conflictAudio = null;
                foreach (DictionaryEntry entry in m_JobTable)
                {
                    AudioClip audioClip = (AudioClip)entry.Key;
                    AudioTrack audioTrackInUse = (AudioTrack)AudioTable[audioClip];  // write this differently

                    Debug.Log((AudioTrack)AudioTable[audioClip] + "using this track");

                    AudioTrack audioTrackNeeded = (AudioTrack)AudioTable[clip];

                    if (audioTrackNeeded.Source == audioTrackInUse.Source)  // bug here when calling play of same type close to each other
                    {
                        // conflict
                        conflictAudio = audioClip;
                    }
                }
                if (conflictAudio != null)
                {
                    RemoveJob(conflictAudio);
                }
            }
            private void RemoveJob(AudioClip clip)
            {
                if (m_JobTable.ContainsKey(clip) == false)
                {
                    Debug.Log("trying to stop a job that is not running");
                }

                IEnumerator runningJob = (IEnumerator)m_JobTable[clip];
                StopCoroutine(runningJob);
                m_JobTable.Remove(clip);
            }

            private AudioTrack FindTrackThatsCurrentlyNotPlaying(AudioType audioType)
            {
                AudioTrack trackToUse = null;

                switch (audioType)
                {
                    case AudioType.OST:
                        for (int i = 0; i < SourcesAmountOST; i++)
                        {
                            if (Tracks[i].Source.isPlaying == false)
                            {
                                trackToUse = Tracks[i];
                                Debug.Log("using OST track..." + trackToUse);
                                return trackToUse;
                            }
                        }
                        trackToUse = Tracks[0];
                        Debug.Log("using OST 0 track..." + trackToUse);
                        return trackToUse;

                    case AudioType.SFX_UI:
                        for (int i = SourcesAmountOST; i < (SourcesAmountOST + SourcesAmountUI); i++)
                        {
                            if (Tracks[i].Source.isPlaying == false)
                            {
                                trackToUse = Tracks[i];
                                Debug.Log("using UI track..." + trackToUse);
                                return trackToUse;
                            }
                        }
                        trackToUse = Tracks[SourcesAmountOST];
                        Debug.Log("using UI 0 track..." + trackToUse);
                        return trackToUse;
                    case AudioType.SFX_SirMouse:
                        for (int i = (SourcesAmountOST + SourcesAmountUI); i < (SourcesAmountOST + SourcesAmountUI + SourcesAmountSirMouse); i++)
                        {
                            if (Tracks[i].Source.isPlaying == false)
                            {
                                trackToUse = Tracks[i];
                                Debug.Log("using Mouse track..." + trackToUse);
                                return trackToUse;
                            }
                        }
                        trackToUse = Tracks[(SourcesAmountOST + SourcesAmountUI)];
                        Debug.Log("using Mouse 0 track..." + trackToUse);
                        return trackToUse;
                    case AudioType.SFX_World:
                        for (int i = (SourcesAmountOST + SourcesAmountUI + SourcesAmountSirMouse); i < (SourcesAmountOST + SourcesAmountUI + SourcesAmountSirMouse + SourcesAmountWorld); i++)
                        {
                            if (Tracks[i].Source.isPlaying == false)
                            {
                                trackToUse = Tracks[i];
                                Debug.Log("using world track..." + trackToUse);
                                return trackToUse;
                            }
                        }
                        trackToUse = Tracks[(SourcesAmountOST + SourcesAmountUI + SourcesAmountSirMouse)];
                        Debug.Log("using world track 0..." + trackToUse);
                        return trackToUse;
                    default:
                        Debug.Log("ADAM SANDLER HUHUHUHHUH " + trackToUse);
                        return null;
                }
            }

            #endregion
        }
    }
}
