using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFunctions : MonoBehaviour
{
    [SerializeField] Animator fadeAnim = null;

    private void Start()
    {
        fadeAnim.GetComponent<AnimEvent>()?.Add_Callback("FadeOver", Play);
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

    }
}
