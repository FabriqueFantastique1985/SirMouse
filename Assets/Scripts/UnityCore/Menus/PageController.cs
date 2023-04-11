﻿
using System;
using System.Collections;
using UnityEngine;


namespace UnityCore
{
    namespace Menus
    {
        public class PageController : MonoBehaviour
        {
            public static PageController Instance;

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


            //public GameObject BackpackImage0, BackpackImage1, ClosetImage0, ClosetImage1;
            [Header("Camera extra for UI ??")]
            public Camera CameraUI_Backpack_Closet;

            #region Unity Functions

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    TurnAllPagesOffExcept(PageType.None);
                    if (EntryPage != PageType.None)
                    {
                        TurnPageOn(EntryPage);
                    }

                    if (gameObject.transform.parent)
                        DontDestroyOnLoad(gameObject.transform.parent);
                    else
                        DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
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
            public void TurnPageOff(PageType typeToTurnOff, PageType typeToTurnOn = PageType.None, bool waitForExit = false)
            {
                if (typeToTurnOff == PageType.None) return;
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

                return GetPage(pageType).isOn;
            }
            // custom methods below
            public void TurnAllPagesOffExcept(PageType turnOn)
            {
                for (int i = 0; i < PagesScene.Length; i++)
                {
                    if (PageIsOn(PagesScene[i].Type) == true)
                    {
                        TurnPageOff(PagesScene[i].Type);
                    }
                }
                TurnPageOn(turnOn);
            }
            public IEnumerator TurnPageOffDelay(PageType typeToTurnOff, PageType typeToTurnOn = PageType.None, bool waitForExit = false, float delayTime = 0.25f)
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

                BackpackController.BackpackInstance.enabled = false;
            }



            public void NotifyBackpackSuper()
            {
                if (ButtonBackpackSuper.IhaveNotificationsLeftCloset == false && ButtonBackpackSuper.IhaveNotificationsLeftInstruments == false)
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
                ButtonEquipToggle.gameObject.SetActive(state);

                ButtonBack.gameObject.SetActive(!state);
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
                for (int i = 0; i <  PagesScene.Length; i++)
                {
                    RegisterPage(PagesScene[i]);
                }
            }
            private void RegisterPage(Page page)
            {
                if (PageExists(page.Type))
                {
                    Debug.Log("You are trying to register a page [" + page.Type + "] that has already been registered : " + page.gameObject.name);
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

