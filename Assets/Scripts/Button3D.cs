using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [SerializeField] UnityEvent ButtonAction = null;

    [SerializeField] Animator anim = null;
    [SerializeField] string animName = "Play";
    [SerializeField] GameObject[] objectsToDissappear = new GameObject[0];
    bool noInteractable;

    private void OnMouseDown()
    {
        if (noInteractable) return;

        for (int i = 0; i < objectsToDissappear.Length; i++)
        {
            objectsToDissappear[i].SetActive(false);
        }

        ButtonAction.Invoke();
        anim.Play(animName);

        noInteractable = true;
    }
}
