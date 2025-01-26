using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public struct Battery
{
    /* Life & Times
     * Are in Seconds
     */
    [SerializeField] private float mLife;
    [SerializeField] private float mMaxTime;
    [SerializeField] private float mCurrTime;

    [SerializeField] private Image mImageUI;

    [SerializeField] Color mFillColour;
    [SerializeField] Color mEmptyColour;

    [SerializeField] private List<AudioClip> mAudioList;
    [SerializeField] private List<AudioMixerGroup> mAudioGroupList;
    [SerializeField] private AudioSource mAudioSource;


    public void Def_Init()
    {
        Reset(10.0f, 10.0f);
        FillColour = Color.white;
        EmptyColour = new Color(0.2f, 0.2f, 0.2f, 1.0f);
    }

    /// <summary>
    /// If life is not set, it assumed you want it to equal max time
    /// </summary>
    /// <param name="curr_time"></param>
    /// <param name="max_time"></param>
    /// <param name="life"></param>
    public void Reset(float curr_time,  float max_time, float life = 0.0f)
    {
        mCurrTime = curr_time;
        MaxTime = max_time;
        Life = (life == 0) ? max_time : life;
    }

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
            if (value > mLife)
            {
                mMaxTime = mLife;
                return;
            }
            mMaxTime = value;
        }
    }

    public float GetCurTime { get { return mCurrTime; } }

    public Image ImageUI
    {
        get { return mImageUI; }
        set { mImageUI = value; }   
    }

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
    public void Update()
    {
        mCurrTime -= Time.deltaTime;
        UpdateTimeLeftVisuals();
        
    }


    private void UpdateTimeLeftVisuals()
    {
        float ratio = mCurrTime / mLife;
        mImageUI.fillAmount = ratio;

        mImageUI.color = Color.Lerp(mEmptyColour, mFillColour, ratio);

        PlayAudio(ratio);
    }


    private void PlayAudio(float lvl)
    {
        if(mAudioList.Count < 0)
            return;
        int mRound = (int)MathF.Floor(lvl * 100);
        int mIndex;
        switch(mRound)
        {
            case 80: mIndex = 0;
                    break;
            case 50: mIndex = 1;
                break;
            case 30: mIndex = 2;
                break;
            case 0: mIndex = 3;
                break;
            default: return;
        }

        mAudioSource.clip = mAudioList[mIndex];
        mAudioSource.outputAudioMixerGroup = mAudioGroupList[mIndex];
        mAudioSource.Play();
    }
};


public class MobileTopBar : MonoBehaviour
{
    [SerializeField] private Text mTimeText = null;

    [SerializeField] private Battery mBattery;


    public Battery Battery
    {
        get { return mBattery; }
        set { mBattery = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!mTimeText)
            mTimeText = GetComponentInChildren<Text>();

        //mBattery.Def_Init();    
        //if(!mBattery.ImageUI)
        //{
        //    if(GameObject.FindGameObjectWithTag("BatteryFill").
        //                                  TryGetComponent<Image>(out Image image_com))
        //    {
        //        mBattery.ImageUI = image_com;
        //    }

        //}

       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeVisuals();
        mBattery.Update();
    }


    private void UpdateTimeVisuals()
    {
        if (!mTimeText)
            return;

        DateTime now = System.DateTime.Now;
        mTimeText.text = now.Hour.ToString("00") + ":" + now.Minute.ToString("00");


    }

}




