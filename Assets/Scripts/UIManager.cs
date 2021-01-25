using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Header("FishBar")]
    [SerializeField] GameObject generalBar = null;
    [SerializeField] GameObject charBarScaller = null;
    [SerializeField] GameObject charBarMovement = null;
    [SerializeField] GameObject fish = null;
    [SerializeField] Image winBar = null;

    [SerializeField] float hudMinSpace = 0;
    [SerializeField] float hudMaxSpace = 10;

    [Header("Other")]
    [SerializeField] Text fishCount = null;
    [SerializeField] string fishCountText = "¿Cuantos peces tenés Roberto? ";
    [SerializeField] GameObject arrow = null;
    int fishAmmount = 0;

    float totalSpace;
    float winBarMax;
    float winBarMin;
    Vector3 maxBar;
    Vector3 minBar;
    Vector3 maxFish;
    Vector3 minFish;

    private void Awake()
    {
        instance = this;
    }

    public void InitializeHUD(float charBarScale, float _winBarMin, float _winBarMax)
    {
        totalSpace = FishingManager.instance.CaptureSpaceMax - FishingManager.instance.CaptureSpaceMin;
        float percentScale = charBarScale / totalSpace * 100;

        float totalSpaceInHUD = hudMaxSpace - hudMinSpace;
        charBarScaller.transform.localScale = new Vector3(charBarScaller.transform.localScale.x, totalSpaceInHUD * percentScale / 100, charBarScaller.transform.localScale.z);

        winBarMin = _winBarMin;
        winBarMax = _winBarMax;

        generalBar.SetActive(false);
        fishCount.text = fishCountText + fishAmmount.ToString();
        maxBar = new Vector3(charBarMovement.transform.localPosition.x, hudMaxSpace, charBarMovement.transform.localPosition.z);
        minBar = new Vector3(charBarMovement.transform.localPosition.x, hudMinSpace, charBarMovement.transform.localPosition.z);
        maxFish = new Vector3(fish.transform.localPosition.x, hudMaxSpace, fish.transform.localPosition.z);
        minFish = new Vector3(fish.transform.localPosition.x, hudMinSpace, fish.transform.localPosition.z);
    }

    public void ActiveFishBar(bool active) => generalBar.SetActive(active);

    public void UpdateBarPos(float pos)
    {
        float percent = pos / totalSpace;

        charBarMovement.transform.localPosition = Vector3.Lerp(minBar, maxBar, percent);
    }

    public void UpdateFishPos(float pos)
    {
        float percent = pos / totalSpace;

        fish.transform.localPosition = Vector3.Lerp(minFish, maxFish, percent);
    }

    public void UpdateWinBarPos(float pos) { float percent = (pos - winBarMin) / (winBarMax - winBarMin); winBar.fillAmount = percent; }

    public void AddFish()
    {
        fishAmmount += 1;
        fishCount.text = fishCountText + fishAmmount.ToString();
    }

    public void ActivateArrow(bool activate)
    {
        arrow.SetActive(activate);
    }

    public void UpdateArrowScale(float scale)
    {
        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, scale);
    }
}