
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

            [Header("Tracks things")]

            public List<AudioTrack> TracksOST;
            public List<AudioTrack> TracksUI;
            public List<AudioTrack> TracksSirMouse;
            public List<AudioTrack> TracksWorld;

            private List<float> _targetVolumesQuiet = new List<float>();
            private List<float> _targetVolumesNormal = new List<float>();

            private AudioJob _lastViableJob;

            private Coroutine _runningAudioInfluencerRoutine;


            #region Extra Classes

            [System.Serializable]
            public class AudioTrack
            {
                public AudioType Type;
                public AudioSource Source;
            }
            private class AudioJob
            {
                public AudioElement AudioEM = new AudioElement(); // initializing it fixes a nullcheck
                public AudioAction Action;               
                public bool Fade;
                public float Delay;
                
                public AudioJob(AudioElement audioEM, AudioAction action, bool fade, float delay)
                {
                    AudioEM.Clip = audioEM.Clip;
                    AudioEM.Type = audioEM.Type;
                    AudioEM.Volume = audioEM.Volume;
                    AudioEM.Pitch = audioEM.Pitch;
                    AudioEM.RandomizePitchSlightly = audioEM.RandomizePitchSlightly;
                    AudioEM.PitchLowerLimitAddition = audioEM.PitchLowerLimitAddition;
                    AudioEM.PitchUpperLimitAddition = audioEM.PitchUpperLimitAddition;

                    Action = action;
                    Fade = fade;
                    Delay = delay;
                } // callef for play()
                public AudioJob(AudioType type, AudioAction action, bool fade, float delay)
                {
                    AudioEM.Type = type;
                    Action = action;
                    Fade = fade;
                    Delay = delay;
                } // called for stop() 
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
                    Instance = this;
                }
            }

            #endregion



            #region Public Functions

            public void PlayAudio(AudioElement audioEm, bool fade = false, float delay = 0f)
            {
                AddJobClip(new AudioJob(audioEm, AudioAction.START, fade, delay));
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

            public void TurnDownVolumeFor(AudioType typeAudio = AudioType.OST , bool iwantToBeQuieter = true)
            {
                // make sure to only do this when the coroutine is null
                if (_runningAudioInfluencerRoutine == null)
                {
                    List<AudioSource> sourcesToInfluence = new List<AudioSource>();
                    switch (typeAudio)
                    {
                        case AudioType.OST:
                            for (int i = 0; i < TracksOST.Count; i++)
                            {
                                sourcesToInfluence.Add(TracksOST[i].Source);

                                // add the currently set volumes to a float list (so we can refer to them later)
                                _targetVolumesNormal.Add(TracksOST[i].Source.volume);
                                _targetVolumesQuiet.Add(TracksOST[i].Source.volume);
                            }
                            break;
                        case AudioType.SFX_UI:
                            for (int i = 0; i < TracksUI.Count; i++)
                            {
                                sourcesToInfluence.Add(TracksUI[i].Source);
                                _targetVolumesNormal.Add(TracksUI[i].Source.volume);
                                _targetVolumesQuiet.Add(TracksUI[i].Source.volume);
                            }
                            break;
                        case AudioType.SFX_SirMouse:
                            for (int i = 0; i < TracksSirMouse.Count; i++)
                            {
                                sourcesToInfluence.Add(TracksSirMouse[i].Source);
                                _targetVolumesNormal.Add(TracksSirMouse[i].Source.volume);
                                _targetVolumesQuiet.Add(TracksSirMouse[i].Source.volume);
                            }
                            break;
                        case AudioType.SFX_World:
                            for (int i = 0; i < TracksWorld.Count; i++)
                            {
                                sourcesToInfluence.Add(TracksWorld[i].Source);
                                _targetVolumesNormal.Add(TracksWorld[i].Source.volume);
                                _targetVolumesQuiet.Add(TracksWorld[i].Source.volume);
                            }
                            break;
                        default:
                            break;

                    }
                    _runningAudioInfluencerRoutine = StartCoroutine(InfluenceVolumeOnSources(sourcesToInfluence, iwantToBeQuieter));
                }
                else
                {
                    Debug.Log("Will not influence Audio levels, as the previous coroutine to change audio levels was still running. " +
                              "SOLUTION : create cgreater wait time between wanting to change audio levels, or change the system");
                }

            }
            public void TurnDownVolumeForOSTAndWorld(bool iwantToBeQuieter = true)
            {
                // make sure to only do this when the coroutine is null
                if (_runningAudioInfluencerRoutine == null)
                {
                    List<AudioSource> sourcesToInfluence = new List<AudioSource>();

                    for (int i = 0; i < TracksOST.Count; i++)
                    {
                        sourcesToInfluence.Add(TracksOST[i].Source);

                        // add the currently set volumes to a float list (so we can refer to them later)
                        _targetVolumesNormal.Add(TracksOST[i].Source.volume);
                        _targetVolumesQuiet.Add(TracksOST[i].Source.volume);
                    }
                    for (int i = 0; i < TracksWorld.Count; i++)
                    {
                        sourcesToInfluence.Add(TracksWorld[i].Source);
                        _targetVolumesNormal.Add(TracksWorld[i].Source.volume);
                        _targetVolumesQuiet.Add(TracksWorld[i].Source.volume);
                    }

                    _runningAudioInfluencerRoutine = StartCoroutine(InfluenceVolumeOnSources(sourcesToInfluence, iwantToBeQuieter));
                }
                else
                {
                    Debug.Log("Will not influence Audio levels, as the previous coroutine to change audio levels was still running. " +
                              "SOLUTION : create cgreater wait time between wanting to change audio levels, or change the system");
                }

            }
            public void MuteOst(bool muteMe = true)
            {
                List<AudioSource> sourcesToInfluence = new List<AudioSource>();

                for (int i = 0; i < TracksOST.Count; i++)
                {
                    sourcesToInfluence.Add(TracksOST[i].Source);

                    // add the currently set volumes to a float list (so we can refer to them later)
                    _targetVolumesNormal.Add(TracksOST[i].Source.volume);
                    _targetVolumesQuiet.Add(TracksOST[i].Source.volume);
                }

                StartCoroutine(MuteVolumeOnSources(sourcesToInfluence, muteMe)); ;
            }
            public void VerifyAudioTracks()
            {
                List<AudioTrack> tracksToRemove = new List<AudioTrack>();
                RemoveNullAudioTrack(TracksOST, tracksToRemove);
                RemoveNullAudioTrack(TracksUI, tracksToRemove);
                RemoveNullAudioTrack(TracksSirMouse, tracksToRemove);
                RemoveNullAudioTrack(TracksWorld, tracksToRemove);
            }

            #endregion



            #region Private Functions


            private void AddJobClip(AudioJob job)
            {
                IEnumerator jobRunner = null;

                if (job.AudioEM.Clip == null)
                {
                    job.AudioEM.Clip = _lastViableJob.AudioEM.Clip;
                    job.AudioEM.Clip = _lastViableJob.AudioEM.Clip;                   
                }

                // start job
                jobRunner = RunAudioJobClip(job);
                _lastViableJob = job;
                
                StartCoroutine(jobRunner);
            }
            private IEnumerator RunAudioJobClip(AudioJob job)
            {
                yield return new WaitForSeconds(job.Delay);

                // figuring out what track to use
                AudioTrack trackToUse = null;
                trackToUse = FindTrackThatsCurrentlyNotPlaying(job.AudioEM.Type);

                // getting the volume/pitch and leaving them on default value if got values are 0
                float volumeToUse = 1;
                float pitchToUse = 1;
                if (job.AudioEM.Volume != 0)
                {
                    volumeToUse = job.AudioEM.Volume;
                }
                if (job.AudioEM.Pitch != 0)
                {
                    pitchToUse = job.AudioEM.Pitch;
                }

                // adjust pitch slightly if bool was checked
                if (job.AudioEM.RandomizePitchSlightly == true)
                {
                    float bottomLimit = pitchToUse - job.AudioEM.PitchLowerLimitAddition;
                    float upperLimit = pitchToUse + job.AudioEM.PitchUpperLimitAddition;

                    float randomPitch = Random.Range(bottomLimit, upperLimit);

                    pitchToUse = randomPitch;
                }


                if (trackToUse != null)
                {
                    switch (job.Action)
                    {
                        case AudioAction.START:
                            trackToUse.Source.clip = job.AudioEM.Clip; 
                            trackToUse.Source.volume = volumeToUse;
                            trackToUse.Source.pitch = pitchToUse;
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
                }

                yield return null;
            }
            private AudioTrack FindTrackThatsCurrentlyNotPlaying(AudioType audioType)
            {
                AudioTrack trackToUse = null;

                switch (audioType)
                {
                    case AudioType.OST:
                        for (int i = 0; i < TracksOST.Count; i++)
                        {
                            if (TracksOST[i].Source.isPlaying == false)
                            {
                                trackToUse = TracksOST[i];
                                //Debug.Log("using OST track..." + trackToUse.Source.gameObject);
                                return trackToUse;
                            }
                        }
                        trackToUse = TracksOST[0];
                        //Debug.Log("using OST 0 track..." + trackToUse.Source.gameObject);
                        return trackToUse;

                    case AudioType.SFX_UI:
                        for (int i = 0; i < TracksUI.Count; i++)
                        {
                            if (TracksUI[i].Source.isPlaying == false)
                            {
                                trackToUse = TracksUI[i];
                                //Debug.Log("using UI track..." + trackToUse.Source.gameObject);
                                return trackToUse;
                            }
                        }
                        trackToUse = TracksUI[0];
                        //Debug.Log("using UI 0 track..." + trackToUse.Source.gameObject);
                        return trackToUse;
                    case AudioType.SFX_SirMouse:
                        for (int i = 0; i < TracksSirMouse.Count; i++)
                        {
                            if (TracksSirMouse[i].Source.isPlaying == false)
                            {
                                trackToUse = TracksSirMouse[i];
                                //Debug.Log("using Mouse track..." + trackToUse.Source.gameObject);
                                return trackToUse;
                            }
                        }
                        trackToUse = TracksSirMouse[0];
                        //Debug.Log("using Mouse 0 track..." + trackToUse.Source.gameObject);
                        return trackToUse;
                    case AudioType.SFX_World:
                        for (int i = 0; i < TracksWorld.Count; i++)
                        {
                            if (TracksWorld[i].Source.isPlaying == false)
                            {
                                trackToUse = TracksWorld[i];
                                //Debug.Log("using world track..." + trackToUse.Source.gameObject);
                                return trackToUse;
                            }
                        }
                        trackToUse = TracksWorld[0];
                        //Debug.Log("using world track 0..." + trackToUse.Source.gameObject);
                        return trackToUse;
                    default:
                        Debug.Log("you forgot to add an AudioType to an AudioElement");
                        return null;
                }
            }
            private IEnumerator InfluenceVolumeOnSources(List<AudioSource> sources, bool iWantToBeQuieter) // this IEnum is problematic if I am calling it too fast in a row...
            {
                if (iWantToBeQuieter == true)
                {
                    // continually decrease volume on all sources until certain point
                    for (int i = 0; i < sources.Count; i++)
                    {
                        // set the goal of our targetVolumeQUIET
                        _targetVolumesQuiet[i] = (float)(_targetVolumesNormal[i] / 5f);

                        // loop until we reach that goal
                        while (sources[i].volume > _targetVolumesQuiet[i]) 
                        {
                            sources[i].volume -= 0.1f;
                            yield return new WaitForEndOfFrame();
                        }
                        sources[i].volume = _targetVolumesQuiet[i];                     
                    }

                    // clear the target volumes
                    _targetVolumesNormal.Clear();
                    _targetVolumesQuiet.Clear();

                    //Debug.Log("turned down volume for some sources");
                }
                else
                {
                    // continually INCREASE volume on all sources until certain point
                    for (int i = 0; i < sources.Count; i++)
                    {
                        // set the goal of our targetVolumeQUIET
                        _targetVolumesNormal[i] = (float)(_targetVolumesQuiet[i] * 5f);

                        // loop until we reach that goal
                        while (sources[i].volume < _targetVolumesNormal[i]) // index out of range ....??? // targte volumes double count (something is being done twice here)
                        {
                            sources[i].volume += 0.1f;
                            yield return new WaitForEndOfFrame();
                        }
                        sources[i].volume = _targetVolumesNormal[i];
                    }

                    // clear the target volumes
                    _targetVolumesNormal.Clear();
                    _targetVolumesQuiet.Clear();

                    //Debug.Log("turned UP volume for some sources");
                }

                _runningAudioInfluencerRoutine = null;
            }
            private IEnumerator MuteVolumeOnSources(List<AudioSource> sources, bool muteMe)
            {
                if (muteMe == true)
                {
                    // continually decrease volume on all sources until certain point
                    for (int i = 0; i < sources.Count; i++)
                    {
                        // set our target values
                        _targetVolumesNormal[i] = (sources[i].volume);
                        _targetVolumesQuiet[i] = 0;

                        // loop until we reach that goal
                        while (sources[i].volume > 0)
                        {
                            sources[i].volume -= 0.1f;
                            yield return new WaitForEndOfFrame();
                        }
                        sources[i].volume = 0;
                    }

                    Debug.Log("muted tracks");
                }
                else
                {
                    // continually INCREASE volume on all sources until certain point
                    for (int i = 0; i < sources.Count; i++)
                    {
                        // loop until we reach that goal
                        while (sources[i].volume < _targetVolumesNormal[i])
                        {
                            sources[i].volume += 0.1f;
                            yield return new WaitForEndOfFrame();
                        }
                        sources[i].volume = _targetVolumesNormal[i];
                    }

                    // clear the target volumes
                    _targetVolumesNormal.Clear();
                    _targetVolumesQuiet.Clear();

                    Debug.Log("un-muted tracks");
                }
            }

            private void RemoveNullAudioTrack(List<AudioTrack> trackListToCheck, List<AudioTrack> tracksToRemove)
            {
                for (int i = 0; i < trackListToCheck.Count; i++)
                {
                    if (trackListToCheck[i].Source == null)
                    {
                        tracksToRemove.Add(trackListToCheck[i]);
                    }
                }
                for (int i = 0; i < tracksToRemove.Count; i++)
                {
                    trackListToCheck.Remove(tracksToRemove[i]);
                }
                tracksToRemove.Clear();
            }

            #endregion
        }
    }
}
