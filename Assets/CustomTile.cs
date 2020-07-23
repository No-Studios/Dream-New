using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class CustomTile : Tile
{


    [SerializeField]
    private TileType type = TileType.BASE;

    private int tile_parent_id { get; set; }

    public CustomTile()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public TileType GetTileType()
    {
        return type; 
    }

    public void SetTileType(TileType t)
    {
        type = t;
    }


    public enum TileType
    {
        BASE,
        TYPE1
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Custom_Tiles")]
    public static void CreateCutomTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Custom_Tiles", "New Custom_Tiles", "asset", "Custom Tiles");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
#endif
}
