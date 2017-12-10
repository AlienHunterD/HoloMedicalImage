using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceManagerScript : MonoBehaviour {

    public SpriteRenderer forwardSlice;
    public SpriteRenderer backwardSlice;
    public SliceType sliceType;

    DataManager _dataManager;
    int _sliceNumber;
    Vector3 _offsetBase;

	// Use this for initialization
	void Start ()
    {
        _dataManager = DataManager.Instance;
        _sliceNumber = 0;
        _offsetBase = new Vector3();
        switch(sliceType)
        {
            case SliceType.Axial:
                _offsetBase = new Vector3(0, 1, 0);
                break;
            case SliceType.Coronal:
                _offsetBase = new Vector3(0, 0, 1);
                break;
            case SliceType.Sagittal:
                _offsetBase = new Vector3(1, 0, 0);
                break;
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        _sliceNumber = (_sliceNumber + 1) % _dataManager.GetNumSlices(sliceType);

        Sprite temp1 = Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType), new Rect(0,0,_dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f,0.5f));
        forwardSlice.sprite = temp1;
        forwardSlice.transform.localPosition = _offsetBase * 0.01f * (float)(_sliceNumber - _dataManager.GetNumSlices(sliceType)/2);
        Sprite temp2 = Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType, true), new Rect(0, 0, _dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f, 0.5f));
        backwardSlice.sprite = temp2;
    }
}
