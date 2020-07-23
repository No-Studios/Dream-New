using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    public GameObject selected_tile_group = null;
    private TileGroup current_tile_group;
    public GridLayout grid_lay;
    public Tilemap tilemap;
    bool cached_tile_group = false;

    public float rotation = 60f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selected_tile_group != null)
        {
            if (!cached_tile_group)
            {
                current_tile_group = selected_tile_group.GetComponent<TileGroup>();
                cached_tile_group = true;
            }
            Vector3 mosPo = Input.mousePosition;
            mosPo.z = -2;
            selected_tile_group.transform.position = Camera.main.ScreenToWorldPoint(mosPo);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selected_tile_group.transform.Rotate(new Vector3(0, 0, rotation));
            }
            if (Input.GetMouseButtonDown(0))
            {
                List<TileBase> hovered_tbs = new List<TileBase>();
                bool allInside = true ;
                selected_tile_group.transform.localScale = new Vector3(1f,1f, 1f);
                foreach(GameObject g in current_tile_group.child_tiles)
                {
                    Vector3Int cellPos = grid_lay.WorldToCell(g.transform.position);
                    TileBase tb = tilemap.GetTile(cellPos);
                    hovered_tbs.Add(tb);
                    if(tb == null)
                    {
                        allInside = false;
                        break;
                    }

                }

                if (allInside)
                {
                    foreach(GameObject g in current_tile_group.child_tiles)
                    {
                        Vector3Int cellPos = grid_lay.WorldToCell(g.transform.position);
                        tilemap.SetTile(cellPos, current_tile_group.tile_to_place);
                    }
                    Destroy(selected_tile_group);
                    selected_tile_group = null;
                    current_tile_group = null;
                    cached_tile_group = false; 
                    
                }
            }
        }
    }
}
