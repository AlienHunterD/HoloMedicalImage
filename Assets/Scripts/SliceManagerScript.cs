using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;

public class SliceManagerScript : MonoBehaviour {

    public SpriteRenderer forwardSlice;
    public SpriteRenderer backwardSlice;
    public SliceType sliceType;

    DataManager _dataManager;
    int _sliceNumber;
    Vector3 _offsetBase;
    ControllerInput _controllerInput;
    Dictionary<int, Sprite> _forwardCache;
    Dictionary<int, Sprite> _backwardCache;

    // Use this for initialization
    void Start ()
    {
        _dataManager = DataManager.Instance;
        _sliceNumber = _dataManager.GetNumSlices(sliceType)/2;
        _offsetBase = new Vector3();
        _controllerInput = new ControllerInput(0, 0.1f);
        _forwardCache = new Dictionary<int, Sprite>();
        _backwardCache = new Dictionary<int, Sprite>();

        switch (sliceType)
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
        UpdateSlice();
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void CenterSlice()
    {
        _sliceNumber = _dataManager.GetNumSlices(sliceType) / 2;
        UpdateSlice();
    }

    public void MoveSlice(int delta)
    {
        _sliceNumber += delta;
        if (_sliceNumber < 0)
            _sliceNumber = 0;
        else if (_sliceNumber >= _dataManager.GetNumSlices(sliceType))
            _sliceNumber = _dataManager.GetNumSlices(sliceType) - 1;
        UpdateSlice();
    }

    public void RefreshSlices()
    {
        _forwardCache.Clear();
        _backwardCache.Clear();
        UpdateSlice();
    }

    private void UpdateSlice()
    {
        forwardSlice.transform.localPosition = _offsetBase * 0.01f * (float)(_sliceNumber - _dataManager.GetNumSlices(sliceType) / 2);
        if (!_forwardCache.ContainsKey(_sliceNumber))
        {
            _forwardCache.Add(_sliceNumber, Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType), new Rect(0, 0, _dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f, 0.5f)));
            _backwardCache.Add(_sliceNumber, Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType, true), new Rect(0, 0, _dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f, 0.5f)));
        }

        forwardSlice.sprite = _forwardCache[_sliceNumber];
        backwardSlice.sprite = _backwardCache[_sliceNumber];
    } 
}
