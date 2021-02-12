using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFunctions : MonoBehaviour
{
    [SerializeField] Animator fadeAnim = null;
    [SerializeField] Animator shopUI = null;
    [SerializeField] Animator shopModel = null;
    [SerializeField] GameObject shopText = null;

    private void Start()
    {
        fadeAnim.GetComponent<AnimEvent>()?.Add_Callback("FadeOver", Play);
        shopUI.GetComponent<AnimEvent>()?.Add_Callback("ShopExitOver", () => shopUI.gameObject.SetActive(false));
    }

    public void FadeOut()
    {
        fadeAnim.Play("FadeOut");
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Shop()
    {
        shopUI.gameObject.SetActive(true);
        shopUI.Play("ShopUIEnter");
    }

    public void Back()
    {
        shopUI.Play("ShopUIExit");
        shopModel.Play("ShopClosed");
        shopText.SetActive(true);
    }
}
