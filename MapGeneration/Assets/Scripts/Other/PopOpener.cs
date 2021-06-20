using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopOpener : MonoBehaviour
{
    public Transform miniMapPop;
    public LevelGeneration levelGeneration;
    public GameObject popBg;
    public Image img;
    public Text titleText;
    Animator anim;
    string popName;
    bool checkPop = false;

    #region Mini Map Pop
    bool openButton = true;
    public void OpenMapButton()
    {
        if (openButton && !checkPop) OpenSliderPop();
        else if (!openButton) CloseSliderPop();
    }
    void OpenSliderPop()
    {
        checkPop = true;
        openButton = false;
        popName = "Map";
        popBg.SetActive(true);
        miniMapPop.gameObject.SetActive(true);
        anim = miniMapPop.GetComponent<Animator>();
        StartCoroutine(FadeIn());
        titleText.text = "Your path";
        anim.Play("OpenPop");
    }
    void CloseSliderPop()
    {
        checkPop = false;
        openButton = true;
        anim = miniMapPop.GetComponent<Animator>();
        StartCoroutine(FadeOut());
        anim.Play("ClosePop");
    }
    #endregion


    #region BackGround Fade
    // fade from transparent to opaque
    IEnumerator FadeIn()
    {
        img.color = new Color(0.32f, 0.39f, 0.35f, 0f);
        // loop over 1 second
        for (float i = 0; i <= 0.94f; i += Time.deltaTime * 7)
        {
            // set color with i as alpha
            img.color = new Color(0.32f, 0.39f, 0.35f, i);
            yield return null;
        }

    }

    // fade from opaque to transparent
    IEnumerator FadeOut()
    {
        titleText.text = "";
        // loop over 1 second backwards
        for (float i = 0.94f; i >= 0; i -= Time.deltaTime * 7)
        {
            // set color with i as alpha
            img.color = new Color(0.32f, 0.39f, 0.35f, i);
            yield return null;
        }
        img.color = new Color(0.32f, 0.39f, 0.35f, 0f);
        popBg.SetActive(false);
        switch (popName)
        {
            case "Map":
                miniMapPop.gameObject.SetActive(false);
                StartCoroutine(levelGeneration.FadeArrows());
                break;
            case "Life":
                
                break;
            case "Inventar":

                break;
            case "Crafting":

                break;
        }
        
    }
    #endregion
}
