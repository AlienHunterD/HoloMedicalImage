using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;

public class SliceManagerScript : MonoBehaviour {

    public SpriteRenderer forwardSlice;
    public SpriteRenderer backwardSlice;
    public SliceType sliceType;
    public float cutoff = 0.5f;

    DataManager _dataManager;
    int _sliceNumber;
    Vector3 _offsetBase;
    ControllerInput _controllerInput;

	// Use this for initialization
	void Start ()
    {
        _dataManager = DataManager.Instance;
        _sliceNumber = _dataManager.GetNumSlices(sliceType)/2;
        _offsetBase = new Vector3();
        _controllerInput = new ControllerInput(0, 0.1f);

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
        UpdateSlice();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float input = 0.0f;
        bool updated = false;
        _controllerInput.Update();

        if(_controllerInput.GetButton(ControllerButton.View))
        {
            _sliceNumber = _dataManager.GetNumSlices(sliceType) / 2;
            UpdateSlice();
            return;
        }

        switch (sliceType)
        {
            case SliceType.Axial:
                input = _controllerInput.GetAxisLeftThumbstickY();
                if (Mathf.Abs(input) > cutoff)
                {
                    updated = true; 
                    if (input > 0f)
                        _sliceNumber++;
                    else
                        _sliceNumber--;
                }
                break;
            case SliceType.Coronal:
                input = _controllerInput.GetAxisRightThumbstickY();
                if (Mathf.Abs(input) > cutoff)
                {
                    updated = true;
                    if (input > 0f)
                        _sliceNumber++;
                    else
                        _sliceNumber--;
                }
                break;
            case SliceType.Sagittal:
                input = _controllerInput.GetAxisLeftThumbstickX();
                if (Mathf.Abs(input) > cutoff)
                {
                    updated = true;
                    if (input > 0f)
                        _sliceNumber++;
                    else
                        _sliceNumber--;
                }
                break;
        }

        if (updated)
        {
            if (_sliceNumber < 0)
                _sliceNumber = 0;
            else if (_sliceNumber >= _dataManager.GetNumSlices(sliceType))
                _sliceNumber = _dataManager.GetNumSlices(sliceType) - 1;
            UpdateSlice();
        }
        
    }

    private void UpdateSlice()
    {
        Sprite temp1 = Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType), new Rect(0, 0, _dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f, 0.5f));
        forwardSlice.sprite = temp1;
        forwardSlice.transform.localPosition = _offsetBase * 0.01f * (float)(_sliceNumber - _dataManager.GetNumSlices(sliceType) / 2);
        Sprite temp2 = Sprite.Create(_dataManager.GetSlice(_sliceNumber, sliceType, true), new Rect(0, 0, _dataManager.GetSliceSize(sliceType).x, _dataManager.GetSliceSize(sliceType).y), new Vector2(0.5f, 0.5f));
        backwardSlice.sprite = temp2;
    } 
}
