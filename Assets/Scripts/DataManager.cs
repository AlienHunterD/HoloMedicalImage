using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.IO;

public enum SliceType { Axial, Coronal, Sagittal };

public class DataManager
{
    static DataManager _Instance;
    int _dimX = 0, _dimY = 0, _dimZ;
    float _voxelSize = 1.0f; // in mm
    Texture2D _emptyTexture;
    List<Texture2D> _axialTextures;
    List<Texture2D> _axialTexturesReversed;
    List<Texture2D> _coronalTextures;
    List<Texture2D> _coronalTexturesReversed;
    List<Texture2D> _sagittalTextures;
    List<Texture2D> _sagittalTexturesReversed;
    byte[,,] _data;
    int _sizeX = 181, _sizeY = 217, _sizeZ = 181;


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

    public int GetNumSlices(SliceType sliceType)
    {
        switch (sliceType)
        {
            case SliceType.Axial:
                return _sizeZ;
            case SliceType.Coronal:
                return _sizeY;
            case SliceType.Sagittal:
                return _sizeX;
        }
        return 0;
    }

    public Vector2Int GetSliceSize(SliceType sliceType)
    {
        Vector2Int result = new Vector2Int();
        switch (sliceType)
        {
            case SliceType.Axial:
                result.x = _sizeX;
                result.y = _sizeY;
                break;
            case SliceType.Coronal:
                result.x = _sizeX;
                result.y = _sizeZ;
                break;
            case SliceType.Sagittal:
                result.x = _sizeY;
                result.y = _sizeZ;
                break;
        }
        return result;
    }

    public Texture2D GetSlice(int sliceNum, SliceType sliceType, bool reversed = false)
    {
        switch(sliceType)
        {
            case SliceType.Axial:
                if (reversed)
                    return _axialTexturesReversed[sliceNum];
                else
                    return _axialTextures[sliceNum];
            case SliceType.Coronal:
                if (reversed)
                    return _coronalTexturesReversed[sliceNum];
                else
                    return _coronalTextures[sliceNum];
            case SliceType.Sagittal:
                if (reversed)
                    return _sagittalTexturesReversed[sliceNum];
                else
                    return _sagittalTextures[sliceNum];
        }

        return _axialTextures[sliceNum];
    }


    private DataManager()
    {
        _emptyTexture = new Texture2D(1, 1);
        _emptyTexture.SetPixel(0, 0, Color.clear);
        _emptyTexture.Apply();

        Reset();
    }

    private void Reset()
    {
        _axialTextures = new List<Texture2D>();
        _axialTexturesReversed = new List<Texture2D>();
        _coronalTextures = new List<Texture2D>();
        _coronalTexturesReversed = new List<Texture2D>();
        _sagittalTextures = new List<Texture2D>();
        _sagittalTexturesReversed = new List<Texture2D>();

        _data = new byte[181, 217, 181];

        var dataAsset = (Resources.Load("t1_ai_msles2_1mm_pn0_rf0") as TextAsset).bytes;
        int index = 0;
        for (int z = 0; z < 181; z++)
            for (int y = 0; y < 217; y++)
                for (int x = 0; x < 181; x++)
                {
                    _data[z, y, x] = dataAsset[index];
                    index++;
                }

        Debug.Log(_data[100, 101, 102]);

        for (int z = 0; z < _sizeZ; z++)
        {
            _axialTextures.Insert(z, ConstructSlice(z, SliceType.Axial));
            _axialTexturesReversed.Insert(z, ConstructSlice(z, SliceType.Axial, true));
        }

        for (int y = 0; y < _sizeY; y++)
        {
            _coronalTextures.Insert(y, ConstructSlice(y, SliceType.Coronal));
            _coronalTexturesReversed.Insert(y, ConstructSlice(y, SliceType.Coronal, true));
        }

        for (int x = 0; x < _sizeX; x++)
        {
            _sagittalTextures.Insert(x, ConstructSlice(x, SliceType.Sagittal));
            _sagittalTexturesReversed.Insert(x, ConstructSlice(x, SliceType.Sagittal, true));
        }
    }

    private Texture2D ConstructSlice(int sliceNum, SliceType sliceType, bool reversed = false)
    {
        Texture2D tempTexture = null;
        byte[] imageData;
        int index, x, y, z;
        byte temp;

        switch (sliceType)
        {
            case SliceType.Axial:
                imageData = new byte[_sizeX * _sizeY * 4];
                tempTexture = new Texture2D(_sizeX, _sizeY,TextureFormat.ARGB32,false);
                index = 0;
                for (y = 0; y < _sizeY; y++) // Reverse the y direction to change the facing
                {
                    for (int looper = 0; looper < _sizeX; looper++)
                    {
                        if (reversed)
                            x = _sizeX - looper - 1;
                        else
                            x = looper;

                        temp = _data[sliceNum, y, x];
                        if (temp == 0)
                            imageData[index] = 0;
                        else
                            imageData[index] = 255;
                        index++;

                        for (int i = 0; i < 3; i++)
                        {
                            imageData[index] = temp;
                            index++;
                        }
                    }
                }

                tempTexture.LoadRawTextureData(imageData);
                tempTexture.Apply();
                break;
            case SliceType.Coronal:
                imageData = new byte[_sizeX * _sizeZ * 4];
                tempTexture = new Texture2D(_sizeX, _sizeZ, TextureFormat.ARGB32, false);
                index = 0;
                for (z = 0; z < _sizeZ; z++) // Reverse the z direction to change the facing
                {
                    for (int looper = 0; looper < _sizeX; looper++)
                    {
                        if (reversed)
                            x = _sizeX - looper - 1;
                        else
                            x = looper;

                        temp = _data[z, sliceNum, x];
                        if (temp == 0)
                            imageData[index] = 0;
                        else
                            imageData[index] = 255;
                        index++;

                        for (int i = 0; i < 3; i++)
                        {
                            imageData[index] = temp;
                            index++;
                        }
                    }
                }

                tempTexture.LoadRawTextureData(imageData);
                tempTexture.Apply();
                break;
            case SliceType.Sagittal:
                imageData = new byte[_sizeY * _sizeZ * 4];
                tempTexture = new Texture2D(_sizeY, _sizeZ, TextureFormat.ARGB32, false);
                index = 0;
                for (z = 0; z < _sizeZ; z++) // Reverse the z direction to change the facing
                    for (int looper = 0; looper < _sizeY; looper++)
                    {
                        if (reversed)
                            y = looper;
                        else
                            y = _sizeY - looper - 1;

                        temp = _data[z, y, sliceNum];
                        if (temp == 0)
                            imageData[index] = 0;
                        else
                            imageData[index] = 255;
                        index++;

                        for (int i = 0; i < 3; i++)
                        {
                            imageData[index] = temp;
                            index++;
                        }
                    }

                tempTexture.LoadRawTextureData(imageData);
                tempTexture.Apply();
                break;
        }

        return tempTexture;
    }

}
