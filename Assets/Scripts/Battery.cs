using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    /* Life & Times
     * Are in Seconds
     */
    [SerializeField] private float mLife = 2.0f;
    [SerializeField] private float mMaxTime = 2.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float mCurrTime = 2.0f;

    [SerializeField] private Image mImageUI;

    [SerializeField] Color mFillColour = Color.white;
    [SerializeField] Color mEmptyColour = new Color(0.2f, 0.2f, 0.2f, 1.0f);

    public float Life
    { 
        get { return mLife; } 
        set { mLife = value; }
    }

    public float MaxTime
    {
        get { return mMaxTime; }
        set 
        { 
            if(value > mLife)
            {
                mMaxTime = mLife;
                return;
            }
            mMaxTime = value; 
        }
    }

    public float GetCurTime { get { return mCurrTime; }}

    public Color FillColour
    {
        get { return mFillColour; }
        set { mFillColour = value; }
    }

    public Color EmptyColour
    {
        get { return mEmptyColour; } 
        set { mEmptyColour = value; }
    }

    // Update is called once per frame
    void Update()
    {
        mCurrTime -= Time.deltaTime;
        UpdateTimeLeftVisuals();
    }


    void UpdateTimeLeftVisuals()
    {
        float ratio = mCurrTime / mLife;
        mImageUI.fillAmount = ratio;

        mImageUI.color = Color.Lerp(mEmptyColour, mFillColour, ratio);  
    }
}
