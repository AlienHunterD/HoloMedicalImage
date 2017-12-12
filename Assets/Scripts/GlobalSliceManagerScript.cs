using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;

public class GlobalSliceManagerScript : MonoBehaviour {

    public SliceManagerScript[] slices;

    ControllerInput _controllerInput;
    DataManager _dataManager;

	// Use this for initialization
	void Start ()
    {
        _controllerInput = new ControllerInput(0, 0.1f);
        _dataManager = DataManager.Instance;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool updateNeeded = false;
        float cutoff = 0.5f;
        float input;
        _controllerInput.Update();

        if (_controllerInput.GetButton(ControllerButton.DPadUp))
        {
            updateNeeded = _dataManager.SizeWindow(4);
        }
        else if (_controllerInput.GetButton(ControllerButton.DPadDown))
        {
            updateNeeded = _dataManager.SizeWindow(-4);
        }
        else if (_controllerInput.GetButton(ControllerButton.DPadRight))
        {
            updateNeeded = _dataManager.MoveWindow(4);
        }
        else if (_controllerInput.GetButton(ControllerButton.DPadLeft))
        {
            updateNeeded = _dataManager.MoveWindow(-4);
        }

        if (updateNeeded)
        {
            foreach (SliceManagerScript slice in slices)
            {
                slice.RefreshSlices();
            }
        }

        if (_controllerInput.GetButton(ControllerButton.View))
        {
            foreach (SliceManagerScript slice in slices)
                slice.CenterSlice();
        }

        input = _controllerInput.GetAxisLeftThumbstickY();
        if (Mathf.Abs(input) > cutoff)
        {
            if (input > 0f)
                slices[0].MoveSlice(1);
            else
                slices[0].MoveSlice(-1);
        }

        input = _controllerInput.GetAxisRightThumbstickY();
        if (Mathf.Abs(input) > cutoff)
        {
            if (input > 0f)
                slices[1].MoveSlice(1);
            else
                slices[1].MoveSlice(-1);
        }

        input = _controllerInput.GetAxisLeftThumbstickX();
        if (Mathf.Abs(input) > cutoff)
        {
            if (input > 0f)
                slices[2].MoveSlice(1);
            else
                slices[2].MoveSlice(-1);
        }

    }
}
