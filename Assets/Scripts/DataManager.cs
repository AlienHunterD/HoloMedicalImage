using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class DataManager
{
    private static DataManager _Instance;

    int _dimX = 0, _dimY = 0, _dimZ;
    float _voxelSize = 1.0f; // in mm

    private Texture2D _emptyTexture;

    public static DataManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new DataManager();
            }
            return _Instance;
        }
    }

    private DataManager()
    {
        _emptyTexture = new Texture2D(1, 1);
        _emptyTexture.SetPixel(0, 0, Color.clear);
        _emptyTexture.Apply();
    }


}
