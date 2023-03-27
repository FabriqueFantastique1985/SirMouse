using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityCore.Audio;
using UnityCore.Menus;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance { get; private set; }

    [SerializeField]
    private Animator _animatorExplosion;
    [SerializeField]
    private Animator _animatorSkinPiece;
    [SerializeField]
    private GameObject _visualOfAnimatorSkinPiece;

    [Header("Confettis")]
    [SerializeField]
    private List<ParticleSystem> _confettis = new List<ParticleSystem>();

    [Header("Sounds")]
    [SerializeField]
    private AudioElement _soundEffectExplosion;
    [SerializeField]
    private AudioElement _soundEffectPopIn;
    [SerializeField]
    private AudioElement _soundEffectPopOut;
    [SerializeField]
    private AudioElement _soundEffectIdle;



    private const string _animPopIn = "UI_Reward_Pop_In";
    private const string _triggerPopOut = "PopOut";


    private void Awake()
    {
        // Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }



    public void GiveReward(List<SkinPieceElement> skinPiecesToGive)
    {
        // unlock the piece(s) in the closet + store the gameobjects in a list
        List<GameObject> instantiatedObjects = new List<GameObject>();
        bool foundSomethingToGive = false;
        for (int i = 0; i < skinPiecesToGive.Count; i++)
        {
            // ! UNLOCK the skinPiece !
            ButtonSkinPiece buttonOfinterest = SkinsMouseController.Instance.UnlockSkinPiece(skinPiecesToGive[i]);


            if (buttonOfinterest != null)
            {
                foundSomethingToGive = true;

                // create a visual for to pop up in the explosion
                GameObject objectToInstantiate = buttonOfinterest.MySpriteToActivateWhenFound;

                // add to notification list           
                ClosetController.Instance.AddNotificationToList(buttonOfinterest);

                // instantiate it (only 1 of feet,arms,legs)
                if (skinPiecesToGive[i].Data.MyBodyType == Type_Body.ArmRight ||
                    skinPiecesToGive[i].Data.MyBodyType == Type_Body.LegRight ||
                    skinPiecesToGive[i].Data.MyBodyType == Type_Body.FootRight)
                {
                    // do nothing
                }
                else
                {
                    GameObject objectToAdd = Instantiate(objectToInstantiate, _visualOfAnimatorSkinPiece.transform);
                    // set it to false
                    objectToAdd.SetActive(false);
                    // add to list
                    instantiatedObjects.Add(objectToAdd);
                }
            }
            else
            {
                Debug.Log("You received a nullref somewhere when unlocking a SkinPiece. YOU GET NOTHING MUAHAHAHA");
            }
        }

        if (foundSomethingToGive == true)
        {
            // block input
            GameManager.Instance.BlockInput = true;

            // turn on the correct page
            PageController.Instance.TurnAllPagesOffExcept(PageType.RewardScreen);

            // activate notifs
            ClosetController.Instance.NotificationActivater();

            // tone down audio of OST & World
            AudioController.Instance.TurnDownVolumeForOSTAndWorld();

            // play animations in proper sequence
            StartCoroutine(PlayAnimations(instantiatedObjects));
        }
    }
    public void GiveCheatReward()
    {
        // unlock the piece(s) in the closet + store the gameobjects in a list
        List<GameObject> instantiatedObjects = new List<GameObject>();
        bool foundSomethingToGive = false;
        ButtonSkinPiece buttonOfinterest = SkinsMouseController.Instance.UnlockAllSkins();

        // creating a visual popup of 1 item //
        GameObject objectToInstantiate = buttonOfinterest.MySpriteToActivateWhenFound; // assign this a single button, doesn't matter what one
        GameObject objectToAdd = Instantiate(objectToInstantiate, _visualOfAnimatorSkinPiece.transform);
        // set it to false
        objectToAdd.SetActive(false);
        // add to list
        instantiatedObjects.Add(objectToAdd);
        //                             //

        foundSomethingToGive = true;
        if (foundSomethingToGive == true)
        {
            // block input
            GameManager.Instance.BlockInput = true;

            // turn on the correct page
            PageController.Instance.TurnAllPagesOffExcept(PageType.RewardScreen);

            // tone down audio of OST & World
            AudioController.Instance.TurnDownVolumeForOSTAndWorld();

            // play animations in proper sequence
            StartCoroutine(PlayAnimations(instantiatedObjects));
        }
    }


    private IEnumerator PlayAnimations(List<GameObject> instantiatedObjects)
    {
        // CONFETTI
        PlayConfetti();

        // pop in explosion
        _animatorExplosion.Play(_animPopIn);
        // SOUND
        AudioController.Instance.PlayAudio(_soundEffectExplosion);

        yield return new WaitForSeconds(0.5f);

        // SOUND
        if (_soundEffectIdle.Clip != null)
        {
            AudioController.Instance.PlayAudio(_soundEffectIdle);
        }       

        // pop in skinpieces 1 by 1
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            // pop in
            instantiatedObjects[i].SetActive(true);
            _animatorSkinPiece.Play(_animPopIn);
            // SOUND
            AudioController.Instance.PlayAudio(_soundEffectPopIn);

            yield return new WaitForSeconds(2.2f);

            // pop out
            _animatorSkinPiece.SetTrigger(_triggerPopOut);
            // SOUND
            AudioController.Instance.PlayAudio(_soundEffectPopOut);

            yield return new WaitForSeconds(0.3f);
            instantiatedObjects[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);
        _animatorExplosion.SetTrigger(_triggerPopOut);



        // destroy all the objects in the visuals at the end 
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
        instantiatedObjects.Clear();


        yield return new WaitForSeconds(0.1f);


        // un-block input
        GameManager.Instance.BlockInput = false;
        // turn off the page
        PageController.Instance.TurnPageOff(PageType.RewardScreen);
        // tone down audio of OST
        AudioController.Instance.TurnDownVolumeForOSTAndWorld(false);
    }

    public void PlayConfetti(Type_Confetti confettiType = Type_Confetti.Normal)
    {
        var highestValue = Enum.GetValues(typeof(Type_Confetti)).Cast<Type_Confetti>().Max();

        for (int i = 0; i < (int)highestValue; i++)
        {
            if (i == (int)confettiType)
            {
                _confettis[i].Play();
                break;
            }
        }
    }
}



public enum Type_Confetti
{
    Normal = 0,
    Leaves = 1,
    Flowers = 2,
    Popcorn = 3
}
