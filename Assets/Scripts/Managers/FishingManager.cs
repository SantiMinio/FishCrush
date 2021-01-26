using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance { get; private set; }

    [Header("GeneralThings")]
    [SerializeField] float captureSpaceMin = 0;
    public float CaptureSpaceMin { get => captureSpaceMin; }
    [SerializeField] float captureSpaceMax = 100;
    public float CaptureSpaceMax { get => captureSpaceMax; }
    [SerializeField] float winBarMin = 0;
    [SerializeField] float winBarMax = 100;
    [SerializeField] float winBarIncreaseSpeed = 1;
    [SerializeField] float winBarDecreaseSpeed = 2;
    [SerializeField] float winBarDefaultValue = 30;
    float winBarCurrentPos;

    [Header("FishingBar")]

    [SerializeField] float barScale = 10;
    [SerializeField] float aceleration = 1.5f;
    [SerializeField] float desaceleration = -1f;
    [SerializeField] float maxPositiveSpeed = 10;
    [SerializeField] float initialPositiveSpeed = 5;
    [SerializeField] float maxNegativeSpeed = -10;
    [SerializeField] float initialNegativeSpeed = 5;
    public float currentAceleration = 0;
    float barCurrentPos;
    float minPos;
    float maxPos;

    public bool moveBar;

    Fish currentFish;

    private void Awake()
    {
        instance = this;
        if (barScale > captureSpaceMax) barScale = captureSpaceMax;
    }

    private void Start()
    {
        UIManager.instance.InitializeHUD(barScale, winBarMin, winBarMax);
    }

    public void StartFishing(Fish _currentFish)
    {
        currentFish = _currentFish;
        winBarCurrentPos = winBarDefaultValue;
        float halfScale = barScale / 2;
        minPos = captureSpaceMin + halfScale;
        maxPos = captureSpaceMax - halfScale;
        barCurrentPos = minPos;
        currentAceleration = 0;
        currentFish.StartFishing(captureSpaceMin, captureSpaceMax, barCurrentPos);
        UIManager.instance.UpdateBarPos(barCurrentPos);
        UIManager.instance.UpdateWinBarPos(winBarCurrentPos);
        UIManager.instance.UpdateFishPos(currentFish.currentPos);

        StartCoroutine(FishingUpdate());
    }

    IEnumerator FishingUpdate()
    {
        float halfScale = barScale / 2;
        bool fishSuccessful = false;
        while (true)
        {
            currentFish.MoveInBattle();
            BarMove();

            if (FishInBar(halfScale))
                winBarCurrentPos += Time.deltaTime * winBarIncreaseSpeed;
            else
                winBarCurrentPos -= Time.deltaTime * winBarDecreaseSpeed;

            UIManager.instance.UpdateBarPos(barCurrentPos);
            UIManager.instance.UpdateWinBarPos(winBarCurrentPos);
            UIManager.instance.UpdateFishPos(currentFish.currentPos);

            if (winBarCurrentPos>= winBarMax)
            {
                fishSuccessful = true;
                break;
            }
            else if(winBarCurrentPos <= winBarMin)
            {
                fishSuccessful = false;
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }

        moveBar = false;
        GameManager.instance.FishingOver(fishSuccessful, currentFish);
        currentFish.ReturnToPool();
    }

    bool FishInBar(float _halfScale)
    {
        float barMax = barCurrentPos + _halfScale;
        float barMin = barCurrentPos - _halfScale;

        if (currentFish.currentPos <= barMax && currentFish.currentPos >= barMin) return true;
        else return false;
    }

    void BarMove()
    {
        if (moveBar)
        {
            if (currentAceleration < maxPositiveSpeed)
            {
                currentAceleration += Time.deltaTime * aceleration;
                if (currentAceleration > maxPositiveSpeed) currentAceleration = maxPositiveSpeed;
            }
        }
        else
        {
            if (currentAceleration > maxNegativeSpeed)
            {
                currentAceleration += Time.deltaTime * desaceleration;
                if (currentAceleration < maxNegativeSpeed) currentAceleration = maxNegativeSpeed;
            }
        }

        barCurrentPos += Time.deltaTime * currentAceleration;

        if (barCurrentPos > maxPos) { barCurrentPos = maxPos; currentAceleration = 0; }
        else if (barCurrentPos < minPos) {barCurrentPos = minPos; currentAceleration = 0; }
    }
}
