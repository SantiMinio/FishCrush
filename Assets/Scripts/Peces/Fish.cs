using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour, IUpdate
{
    protected float minPos;
    protected float maxPos;
    public float currentPos;
    float timerToDespawn;
    float currentMaxTimer;
    bool inBait;
    bool captured;
    float timerInBait;

    [SerializeField] float speedOutOfCombat = 2;
    [SerializeField] float viewAngle = 90;
    [SerializeField] float rotationSpeed = 5;

    [SerializeField] float minTimeInWater = 8;
    [SerializeField] float maxTimeInWater = 17;

    [SerializeField] MovementModule moveModule = null;
    [SerializeField] float fadeSpeed = 5;

    public Node myNodeSpawn;

    public virtual void StartFishing(float _minPos, float _maxPos, float initialPos)
    {
        minPos = _minPos;
        maxPos = _maxPos;
        currentPos = initialPos;
    }

    public virtual void Initialize()
    {
        GameManager.instance.updateManager.SuscribeToUpdate(this);
        moveModule.SetWaypoints(speedOutOfCombat, rotationSpeed);
        currentMaxTimer = Random.Range(minTimeInWater, maxTimeInWater);
        StartCoroutine(Appear());
    }

    public abstract void MoveInBattle();

    public void OnUpdate()
    {
        if (timerToDespawn >= maxTimeInWater) return;
        if (inBait)
        {
            timerInBait += Time.deltaTime;
            if (timerInBait>=0.5f)
            {
                inBait = false;
                timerInBait = 0;
            }
        }
        else
        {
            if (!captured) moveModule.Move();

            timerToDespawn += Time.deltaTime;

            if (timerToDespawn >= maxTimeInWater) StartCoroutine(Dissappear());
        }
    }

    public void Move(Vector3 baitPos)
    {
        transform.forward = Vector3.Slerp(transform.forward, (baitPos - transform.position).normalized, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speedOutOfCombat * Time.deltaTime;
        inBait = true;
        timerInBait = 0;
    }


    public void FishCaptured(bool b)
    {
        captured = b;

        inBait = false;
        timerInBait = 0;
    }

    public bool InFishBait(Vector3 baitPos)
    {
        Vector3 directionToTarget = baitPos - transform.position;
        directionToTarget.Normalize();

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget <= viewAngle) return true;
        else return false;
    }

    public virtual void ReturnToPool()
    {
        GameManager.instance.updateManager.DesuscribeToUpdate(this);
        timerToDespawn = 0;
        inBait = false;
        timerInBait = 0;
        captured = false;
    }

    IEnumerator Dissappear()
    {
        float currentFade = 1;
        Material mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        Color myColor = mat.color;
        GameManager.instance.updateManager.DesuscribeToUpdate(this);
        timerToDespawn = 0;
        inBait = false;
        captured = false;
        while (currentFade>0)
        {
            currentFade -= fadeSpeed * Time.deltaTime;
            if (currentFade < 0) currentFade = 0;
            mat.color = new Color(myColor.r, myColor.g, myColor.b, currentFade);
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.ReturnFishToPool(this);
    }

    IEnumerator Appear()
    {
        float currentFade = 0;
        Material mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        Color myColor = mat.color;
        mat.color = new Color(myColor.r, myColor.g, myColor.b, 0);
        while (currentFade < 1)
        {
            currentFade += fadeSpeed * Time.deltaTime;
            if (currentFade > 1) currentFade = 1;
            mat.color = new Color(myColor.r, myColor.g, myColor.b, currentFade);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
