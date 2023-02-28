using System.Collections;
using System.Collections.Generic;
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
        // block input
        GameManager.Instance.BlockInput = true;

        // turn on the correct page
        PageController.Instance.TurnAllPagesOffExcept(PageType.RewardScreen);

        // unlock the piece(s) in the closet + store the gameobjects in a list
        List<GameObject> instantiatedObjects = new List<GameObject>();
        for (int i = 0; i < skinPiecesToGive.Count; i++)
        {
            // unlock the skinPiece
            GameObject objectToInstantiate = SkinsMouseController.Instance.UnlockSkinPiece(skinPiecesToGive[i]);    
            // instantiate it
            GameObject objectToAdd = Instantiate(objectToInstantiate, _visualOfAnimatorSkinPiece.transform);
            // set it to false
            objectToAdd.SetActive(false);
            // add to list
            instantiatedObjects.Add(objectToAdd);
        }

        // tone down audio of OST & World
        AudioController.Instance.TurnDownVolumeForOSTAndWorld();
        
        // play animations in proper sequence
        StartCoroutine(PlayAnimations(instantiatedObjects));
    }


    private IEnumerator PlayAnimations(List<GameObject> instantiatedObjects)
    {
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
}
