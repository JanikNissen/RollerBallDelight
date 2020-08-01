using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Rigidbody))]
public class PlayerPaint : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] private bool INFINITPAINT;
    [Header("BallRadius")]
    [SerializeField] private float currentRadius;
    [SerializeField] private float maxPaintRadius;
    [SerializeField] private float fallOffRadius;
    [SerializeField] private float decreaseRadiusMultiplier;

    [Header("Colors/UI")]
    [SerializeField] private int colorIndex;
    [SerializeField] private Color[] Colors;

    [SerializeField] private Image paintMeter;
    [SerializeField] private Text fillPercent;

    [Header("Progress")]
    [SerializeField] private float pixelsDrawn;
    
    

    private void Awake()
    {
        paintMeter.color = Colors[colorIndex];      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            currentRadius = maxPaintRadius;

        paintMeter.fillAmount = currentRadius / maxPaintRadius;
       


    }

    private float accumulateFillAmount(List<DrawPaint> mySurfaces)
    {
        float returnValue = 0;

        foreach (DrawPaint surface in mySurfaces)
            returnValue += surface.getFillAmount();
        //Debug.Log(returnValue);
        return returnValue;
    }


    private void OnCollisionStay(Collision other)
    {
        float subtraction = 0f;
        if (other.gameObject.CompareTag("Paintable") && currentRadius > 0f)
            subtraction = other.gameObject.GetComponent<DrawPaint>().Paint(other.GetContact(0).point, currentRadius, Colors[colorIndex]) * decreaseRadiusMultiplier;

        if (!INFINITPAINT)
            currentRadius -= subtraction;
        if (currentRadius <= fallOffRadius)
            currentRadius = 0f;

        pixelsDrawn = accumulateFillAmount(Constants.Objectives) / Mathf.Max(1f, Constants.Objectives.Count) - accumulateFillAmount(Constants.AntiObjectives) / Mathf.Max(1f, Constants.AntiObjectives.Count);
        fillPercent.text = (pixelsDrawn * 100f).ToString("n1")+"%";

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Paintsource"))
            currentRadius = maxPaintRadius;

    }


}
