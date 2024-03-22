﻿using System;
using System.Collections;
using System.Globalization;
using UnityCore.Scene;
using UnityEngine;

namespace UnityCore
{
    namespace Menus
    {
        public class PageController : MonoBehaviourSingleton<PageController>
        {
            public PageType EntryPage;
            public Page[] PagesScene;

            private Hashtable m_Pages;

            [Header("Buttons main UI")]
            public ButtonBackpackSuper ButtonBackpackSuper;

            public ButtonEquipToggle ButtonEquipToggle;
            public ButtonBack ButtonBack;

            [Header("Buttons inside BackpackSuper")]
            public ButtonInstrumentSelect ButtonInstrumentSuper;

            public ButtonClosetSelect ButtonClosetSuper;
            public ButtonResourceSelect ButtonResourceSuper;

            [Header("gameObjects to hide when pausing")]
            [SerializeField]
            private GameObject _buttonsBottomLeft;
            [SerializeField]
            private GameObject _buttonsTopRight;


            //public GameObject BackpackImage0, BackpackImage1, ClosetImage0, ClosetImage1;
            [Header("Overlay Camera for UI")]
            public Camera CameraUI_Backpack_Closet;

            [Header("Loading screens")]
            [SerializeField]
            private GameObject _castleLoadingScreen;
            [SerializeField]
            private GameObject _caveLoadingScreen;
            [SerializeField]
            private GameObject _forestLoadingScreen;
            [SerializeField]
            private GameObject _misterWitchHouseLoadingScreen;
            [SerializeField]
            private GameObject _PrinceTowerScreen;
            [SerializeField]
            private GameObject _SwampScreen;

            #region Unity Functions

            protected override void Awake()
            {
                base.Awake();
                m_Pages = new Hashtable();
                RegisterAllPages();
                TurnAllPagesOffExcept(EntryPage);
            }

            #endregion


            #region Public Functions

            public void TurnPageOn(PageType typeToTurnOn)
            {
                if (typeToTurnOn == PageType.None) return;
                if (PageExists(typeToTurnOn) == false)
                {
                    Debug.Log("You're trying to turn a page on [" + typeToTurnOn + "] that has not been registered");
                    return;
                }

                // check if the page im turning is a loading page..
                // if is loading page.... turn off page at the end


                Page page = GetPage(typeToTurnOn);
                page.gameObject.SetActive(true);
                page.Animate(true);
            }

            public void TurnPageOff(PageType typeToTurnOff, PageType typeToTurnOn = PageType.None,
                bool waitForExit = false)
            {
                //if (typeToTurnOff == PageType.None) return;
                if (PageExists(typeToTurnOff) == false)
                {
                    Debug.Log("You're trying to turn a page off [" + typeToTurnOff + "] that has not been registered");
                    return;
                }

                Page offPage = GetPage(typeToTurnOff);
                if (offPage.gameObject.activeSelf == true)
                {
                    offPage.Animate(false);
                }

                if (typeToTurnOn != PageType.None)
                {
                    Page onPage = GetPage(typeToTurnOn);
                    if (waitForExit == true)
                    {
                        StopCoroutine(WaitForPageExit(onPage, offPage));
                        StartCoroutine(WaitForPageExit(onPage, offPage));
                    }
                    else
                    {
                        TurnPageOn(onPage.Type);
                    }
                }
            }

            public bool PageIsOn(PageType pageType)
            {
                if (PageExists(pageType) == false)
                {
                    //Debug.Log("You are trying to detect if a page is on [" + pageType + "], but it has not been registered");
                    return false;
                }

                var page = GetPage(pageType);
                return page.isOn /*|| page.gameObject.activeSelf*/;
            }

            // custom methods below
            public void TurnAllPagesOffExcept(PageType turnOn)
            {
                for (int i = 0; i < PagesScene.Length; i++)
                {
                    var page = PagesScene[i];
                    if (PageIsOn(page.Type) == true || GetPage(page.Type).gameObject.activeSelf)
                    {
                        TurnPageOff(PagesScene[i].Type);
                    }
                }

                TurnPageOn(turnOn);
            }

            public IEnumerator TurnPageOffDelay(PageType typeToTurnOff, PageType typeToTurnOn = PageType.None,
                bool waitForExit = false, float delayTime = 0.25f)
            {
                yield return new WaitForSeconds(delayTime);

                if (typeToTurnOff == PageType.None)
                {
                    Debug.Log("You're trying to turn Nothing off");
                }

                if (PageExists(typeToTurnOff) == false)
                {
                    Debug.Log("You're trying to turn a page off [" + typeToTurnOff + "] that has not been registered");
                }

                Page offPage = GetPage(typeToTurnOff);
                if (offPage.gameObject.activeSelf == true)
                {
                    offPage.Animate(false);
                }

                if (typeToTurnOn != PageType.None)
                {
                    Page onPage = GetPage(typeToTurnOn);
                    if (waitForExit == true)
                    {
                        StopCoroutine(WaitForPageExit(onPage, offPage));
                        StartCoroutine(WaitForPageExit(onPage, offPage));
                    }
                    else
                    {
                        TurnPageOn(onPage.Type);
                    }
                }

                BackpackController.Instance.enabled = false;
            }


            public void NotifyBackpackSuper()
            {
                if (ButtonBackpackSuper.IhaveNotificationsLeftCloset == false &&
                    ButtonBackpackSuper.IhaveNotificationsLeftInstruments == false)
                {
                    ButtonBackpackSuper.NotificationObject.SetActive(false);
                }
                else
                {
                    ButtonBackpackSuper.NotificationObject.SetActive(true);
                }
            }


            public void OpenBagImage(bool state)
            {
                //BackpackImage0.SetActive(!state);
                //BackpackImage1.SetActive(state);
            }

            public void OpenClosetImage(bool state)
            {
                //ClosetImage0.SetActive(!state);
                //ClosetImage1.SetActive(state);
            }


            public void ShowGameplayHUD(bool state)
            {
                ButtonBackpackSuper.gameObject.SetActive(state);

                if (InstrumentController.Instance.CheckIfFoundInstrument() == true)
                {
                    ButtonEquipToggle.gameObject.SetActive(state);
                }

                _buttonsTopRight.SetActive(state);

                ButtonBack.gameObject.SetActive(!state);
            }
            public void ShowBottomLeftButtons(bool state)
            {
                _buttonsBottomLeft.SetActive(state);
            }
            public void FullyHideUIButtons(bool hideMe)
            {
                if (hideMe == true)
                {
                    _buttonsBottomLeft.SetActive(false);
                    _buttonsTopRight.SetActive(false);
                }
                else
                {
                    _buttonsBottomLeft.SetActive(true);
                    _buttonsTopRight.SetActive(true);
                }
            }

            public void SetLoadingScreen(LoadingScreenImage loadingScreen)
            {
                GameObject loadingScreenPrefab = null;

                switch (loadingScreen)
                {
                    case LoadingScreenImage.Castle:
                        loadingScreenPrefab = Instantiate(_castleLoadingScreen);
                        break;
                    case LoadingScreenImage.Cave:
                        loadingScreenPrefab = Instantiate(_caveLoadingScreen);
                        break;
                    case LoadingScreenImage.Forest:
                        loadingScreenPrefab = Instantiate(_forestLoadingScreen);
                        break;
                    case LoadingScreenImage.MisterWitchHouse:
                        loadingScreenPrefab = Instantiate(_misterWitchHouseLoadingScreen);
                        break;
                    case LoadingScreenImage.PrinceTower:
                        loadingScreenPrefab = Instantiate(_PrinceTowerScreen);
                        break;
                    case LoadingScreenImage.Swamp:
                        loadingScreenPrefab = Instantiate(_SwampScreen);
                        break;
                }

                if (loadingScreenPrefab != null)
                {
                    loadingScreenPrefab.transform.parent = PagesScene[0].transform;
                }
            }

            #endregion


            #region Private Functions

            private IEnumerator WaitForPageExit(Page on, Page off)
            {
                while (off.TargetState != PageState.None)
                {
                    yield return null;
                }

                TurnPageOn(on.Type);
            }

            private void RegisterAllPages()
            {
                for (int i = 0; i < PagesScene.Length; i++)
                {
                    RegisterPage(PagesScene[i]);
                }
            }

            private void RegisterPage(Page page)
            {
                if (PageExists(page.Type))
                {
                    Debug.Log("You are trying to register a page [" + page.Type +
                              "] that has already been registered : " + page.gameObject.name);
                    return;
                }

                m_Pages.Add(page.Type, page);
            }

            private Page GetPage(PageType type)
            {
                if (PageExists(type) == false)
                {
                    Debug.Log("You are trying to get a page [" + type + "] that has not been registered");
                    return null;
                }

                return (Page)m_Pages[type];
            }

            private bool PageExists(PageType type)
            {
                return m_Pages.ContainsKey(type);
            }

            #endregion
        }
    }
}