using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cup : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Pick Up")]

    [SerializeField]
    private Transform pickUpTarget;

    [SerializeField]
    private float pickUpTime;

    [Header("Put Down")]

    [SerializeField]
    private Transform putDownTarget;

    [SerializeField]
    private float putDownTime;

    [Header("Shake")]

    [SerializeField]
    private Vector3 shakeOffset;

    [SerializeField]
    private float shakeTime;

    [Header("Tip")]

    [SerializeField]
    private Vector3 tipRotation;

    [SerializeField]
    private float tipTime;

    [Header("Upright")]

    [SerializeField]
    private float uprightTime;

    private Coroutine actionCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(2f);
        PutDown();
        yield return new WaitForSeconds(2f);
        PickUp();
        yield return new WaitForSeconds(2f);
        StartShaking();
        yield return new WaitForSeconds(2f);
        StopShaking();
        yield return new WaitForSeconds(0.1f);
        Tip();
        yield return new WaitForSeconds(2f);
        Upright();
    }

    /// <summary>
    /// Puts the cup down.
    /// </summary>
    public void PutDown()
    {
        Debug.Log("[Cup] PutDown");
        StartActionCoroutine(MoveBetween(transform.position, putDownTarget.position, putDownTime, Ease.QuadInOut));
    }

    /// <summary>
    /// Picks the cup up.
    /// </summary>
    public void PickUp()
    {
        Debug.Log("[Cup] PickUp");
        StartActionCoroutine(MoveBetween(transform.position, pickUpTarget.position, pickUpTime, Ease.QuadInOut));
    }

    /// <summary>
    /// Starts shaking the cup back and forth.
    /// </summary>
    public void StartShaking()
    {
        Debug.Log("[Cup] StartShaking");
        StartActionCoroutine(MoveBackAndForth(shakeOffset, shakeTime, Ease.QuadInOut));
    }

    /// <summary>
    /// Stops shaking the cup.
    /// </summary>
    public void StopShaking()
    {
        Debug.Log("[Cup] StopShaking");
        var time = shakeTime * Vector3.Distance(transform.position, pickUpTarget.position) / shakeOffset.magnitude;
        StartActionCoroutine(MoveBetween(transform.position, pickUpTarget.position, time, Ease.Linear));
    }

    /// <summary>
    /// Tips the cup.
    /// </summary>
    public void Tip()
    {
        Debug.Log("[Cup] Tip");
        StartActionCoroutine(RotateBetween(transform.rotation, Quaternion.Euler(tipRotation), tipTime, Ease.QuadInOut));
    }

    /// <summary>
    /// Turns the cup upright.
    /// </summary>
    public void Upright()
    {
        Debug.Log("[Cup] TipBack");
        StartActionCoroutine(RotateBetween(transform.rotation, Quaternion.identity, uprightTime, Ease.QuadInOut));
    }

    private void StartActionCoroutine(IEnumerator coroutine)
    {
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
        actionCoroutine = StartCoroutine(coroutine);
    }

    private IEnumerator MoveBetween(Vector3 start, Vector3 end, float time, Func<float, float> ease)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            rb.MovePosition(Vector3.Lerp(start, end, ease(elapsedTime / time)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveBackAndForth(Vector3 offset, float time, Func<float, float> ease)
    {
        var positive = transform.position + offset;
        var negative = transform.position - offset;
        yield return StartCoroutine(MoveBetween(transform.position, positive, time / 2f, ease));
        while (true)
        {
            yield return StartCoroutine(MoveBetween(positive, negative, time, ease));
            yield return StartCoroutine(MoveBetween(negative, positive, time, ease));
        }
    }

    private IEnumerator RotateBetween(Quaternion start, Quaternion end, float time, Func<float, float> ease)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            rb.MoveRotation(Quaternion.Slerp(start, end, ease(elapsedTime / time)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
