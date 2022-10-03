
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



            #region Unity Functions

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    if (EntryPage != PageType.None)
                    {
                        TurnPageOn(EntryPage);
                    }
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

                Page page = GetPage(typeToTurnOn);
                page.gameObject.SetActive(true);
                page.Animate(true);
            }

            public void TurnPageOff(PageType typeToTurnOff, PageType typeToTurnOn, bool waitForExit = false)
            {

            }

            #endregion



            #region Private Functions

            private IEnumerator WaitForPageExit(Page on, Page off)
            {
                yield return null;
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
                    Debug.Log();
                    return;
                }
            }

            private Page GetPage(PageType type)
            {
                return null;
            }

            private bool PageExists(PageType type)
            {
                return false;
            }

            #endregion
        }
    }
}

