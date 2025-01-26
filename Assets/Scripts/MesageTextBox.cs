using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MesageTextBox : MonoBehaviour
{

    [SerializeField] private float mMaxWidth = 200;
    [SerializeField] private Text mText = null;

    [Space]
    [Header("Debug")]
    [SerializeField] private bool bDebugging = false;
    [SerializeField] private float mPerferredWidth = 0;
    [SerializeField] private float mPerferredHeight = 0;

    [SerializeField] private float mDebugWidth = 0;
    [SerializeField] private float mDebugHeight = 0;

    [SerializeField] private Vector2 mDebugAnchorPos;
    [SerializeField] private Vector2 mPos;


    private void Awake()
    {
        if(!mText)
            mText = GetComponentInChildren<Text>();
    }


    // Update is called once per frame
    public void Update()
    {
        //For debugging
        if(bDebugging)
            Init();
    }

    public void Init()
    {
        if(!mText) GetComponentInChildren<Text>();
        mPerferredWidth = mText.preferredWidth;
        mPerferredHeight = mText.preferredHeight;

        //adjust width
        if(gameObject.TryGetComponent<RectTransform>(out RectTransform rect_trans))
        {
            mDebugAnchorPos = rect_trans.anchorMax;
            //adjust width
            if(rect_trans.rect.width < mPerferredWidth && rect_trans.rect.width < mMaxWidth)
            {
                float width = Mathf.Min(mPerferredWidth, mMaxWidth);
                int invert_anchor = ((int)rect_trans.anchorMax.x - 1) * -1;  //[0 >> 1] [1 >> 0] >> Determine anchor side Helper 
                rect_trans.offsetMax = new Vector2(invert_anchor * width, rect_trans.offsetMax.y);
                rect_trans.offsetMin = new Vector2((int)rect_trans.anchorMax.x * -width, rect_trans.offsetMin.y);
                mPos = rect_trans.offsetMin;

            }  //adjust height
            else if (rect_trans.rect.height < mPerferredHeight)
            {
                //rect_trans
                mDebugHeight = rect_trans.offsetMin.y;
                rect_trans.offsetMin = new Vector2(rect_trans.offsetMin.x, -mPerferredHeight);
            }
        }
    }
}
