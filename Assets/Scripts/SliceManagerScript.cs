using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceManagerScript : MonoBehaviour {

    public SpriteRenderer forwardSlice;
    public SpriteRenderer backwardSlice;
    public SpriteRenderer coronalSlice;
    public SpriteRenderer sagittalSlice;

    DataManager _dataManager;
    int _axialSliceNum, _coronalSliceNum, _sagittalSliceNum;

	// Use this for initialization
	void Start ()
    {
        _dataManager = DataManager.Instance;
        _axialSliceNum = 0;
        _coronalSliceNum = 0;
        _sagittalSliceNum = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        _axialSliceNum = (_axialSliceNum + 1) % 181;
        _coronalSliceNum = (_coronalSliceNum + 1) % 217;
        _sagittalSliceNum = (_sagittalSliceNum + 1) % 181;

        Sprite temp1 = Sprite.Create(_dataManager.GetSlice(_axialSliceNum, SliceType.Axial), new Rect(0,0,181,217), new Vector2(0.5f,0.5f));
        forwardSlice.sprite = temp1;
        forwardSlice.transform.localPosition = new Vector3(0, 0.01f * (float)(_axialSliceNum - 90), 0);
        Sprite temp2 = Sprite.Create(_dataManager.GetSlice(_axialSliceNum, SliceType.Axial, true), new Rect(0, 0, 181, 217), new Vector2(0.5f, 0.5f));
        backwardSlice.sprite = temp2;

        Sprite temp3 = Sprite.Create(_dataManager.GetSlice(_coronalSliceNum, SliceType.Coronal), new Rect(0, 0, 181, 181), new Vector2(0.5f, 0.5f));
        coronalSlice.sprite = temp3;
        coronalSlice.transform.localPosition = new Vector3(0, 0, 0.01f * (float)(_coronalSliceNum - 108));

        Sprite temp4 = Sprite.Create(_dataManager.GetSlice(_sagittalSliceNum, SliceType.Sagittal), new Rect(0, 0, 217, 181), new Vector2(0.5f, 0.5f));
        sagittalSlice.sprite = temp4;
        sagittalSlice.transform.localPosition = new Vector3(0.01f * (float)(_sagittalSliceNum - 90), 0, 0);
    }
}
