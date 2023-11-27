using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuPanel : MonoBehaviour
{
    public Animator animator;
    public GameObject panelBackground;

    private GameObject panelToOpen = null;

    public void OnEnable()
    {
        animator.Play("MenuDownAnim");
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        if (panelToOpen == null)
        {
            panelBackground.SetActive(false);
        }
        else
        {
            panelToOpen.SetActive(true);
        }
    }

    public void ExitPanel()
    {
        animator.Play("MenuUpAnim");
        this.panelToOpen = null;
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }

    public void ExitPanelToOpen(GameObject panelToOpen)
    {
        animator.Play("MenuUpAnim");
        this.panelToOpen = panelToOpen;
    }
}
