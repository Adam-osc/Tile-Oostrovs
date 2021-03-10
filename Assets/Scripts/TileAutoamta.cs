using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.UI;

public class TileAutoamta : MonoBehaviour
{
    [Range(-100, 100)]
    public int Yplace = 0;

    public float CordsX;
    public float CordsY;
    public float CordsZ;

    [Range(0, 100)]
    public int iniChance;

    [Range(1, 8)]
    public int birthLimit;

    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR;
    private int count = 0;

    private int[,] terrainMap;
    private int[,] layerMap;
    public Vector3Int tmapSize;

    public Tilemap layer;
    public Tilemap topMap;
    public Tilemap botMap;

    public RuleTile topTile;
    public RuleTile goodTile;
    public UnityEngine.Tilemaps.Tile botTile;
    public UnityEngine.Tilemaps.Tile paintTile;
    public UnityEngine.Tilemaps.Tile greenTile;
    public UnityEngine.Tilemaps.Tile blueTile;

    private int dice;

    public Toggle toggle1;
    public bool EditMode;

    int width;
    int height;


    public void Awake()
    {
        EditMode = false;
    }


    public void doSim(int numR)
    {
        clearMap(false);
        width = tmapSize.x;
        height = tmapSize.y;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();



        }

        if (layerMap == null)
        {
            layerMap = new int[width, height];
            layerMap = terrainMap;
            secPos();


        }


        for (int i = 0; i < numR; i++)
        {
            terrainMap = genTilePos(terrainMap);

        }

        for (int i = 0; i < numR + 6; i++)
        {
            layerMap = genTilePos(layerMap);

        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (layerMap[x, y] == 1)
                    layer.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), goodTile);

                if (terrainMap[x, y] == 1)
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);



            }
        }

    }

    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;

                foreach (var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y > +0 && y + b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighb++;
                    }
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < deathLimit) newMap[x, y] = 0;
                    else
                    {
                        newMap[x, y] = 1;
                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;
                    else
                    {
                        newMap[x, y] = 0;
                    }
                }

            }
        }



        return newMap;
    }




    public void initPos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                dice = Random.Range(1, 101);
                terrainMap[x, y] = dice < iniChance ? 1 : 0;

            }
        }
    }

    public void secPos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                layerMap[x, y] = Random.Range(1, 101) < iniChance + 2 ? 1 : 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle1.isOn)
        {
            EditMode = true;
        }
        else
        {
            EditMode = false;
        }

        if (Input.GetMouseButtonDown(1) && EditMode == true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            CordsX = mousePos.x;
            CordsY = mousePos.y;
            CordsZ = mousePos.z;


            if (CordsX < 0 && CordsY < 0)
            {
                topMap.SetTile(new Vector3Int((int)CordsX - 1, (int)CordsY - 1, 0), paintTile);
            }

            else if (CordsX < 0)
            {
                topMap.SetTile(new Vector3Int((int)CordsX - 1, (int)CordsY, 0), paintTile);

            }

            else if (CordsY < 0)
            {
                topMap.SetTile(new Vector3Int((int)CordsX, (int)CordsY - 1, 0), paintTile);
            }

            else
            {
                topMap.SetTile(new Vector3Int((int)CordsX, (int)CordsY, 0), paintTile);
            }



        }


        if (Input.GetKey("m"))
        {
            doSim(numR);
        }

        if (Input.GetKey("f"))
        {
            clearMap(true);
        }

        if (Input.GetKey("r"))
        {
            SaveAssetMap();
            count++;

        }


    }

    public void Toggle(bool tog)
    {
        print(tog);


    }

    public void GreenColor()
    {
        paintTile = greenTile;
    }

    public void BlueColor()
    {
        paintTile = blueTile;
    }

    public void SaveAssetMap()
    {
        string saveName = "tmapXY" + count;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/" + saveName + ".prefab";
            if (PrefabUtility.SaveAsPrefabAsset(mf, savePath))
            {
                EditorUtility.DisplayDialog("Tilemap " + saveName," was saved in: " + savePath, "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap " + saveName, " not saved in: " + savePath + " An ERROR has occured", "Exit");
            }
        }
    }


    public void clearMap(bool complete)
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();

        if (complete)
        {
            terrainMap = null;
        }
    }
}
