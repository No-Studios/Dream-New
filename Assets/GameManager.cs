using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    public GameObject tileGroup;
    public List<GameObject> all_tile_groups;
    public Transform[] spawns;
    public Transform[] spots_to_move_to;
    public GameObject canvas_name_template;  
    private Canvas[] group_names; 

    private bool time_to_spawn = false;
    private bool done_spawning = false;
    public int current_spot_to_move_to = 0; 
    private int current_tile_to_spawn = 0;

    public float max_spawn_time = 4f; 
    public float spawn_timer = 4f;
    public DreamRequest dreamRef;
    public int numOfTileGroups = 5;
    // Start is called before the first frame update
    void Start()
    {
        spawn_timer = max_spawn_time;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Make it so that the tile groups infinitely spawn
        if(time_to_spawn && current_tile_to_spawn < all_tile_groups.Count)
        {
            if(spawn_timer <= 0)
            {
                all_tile_groups[current_tile_to_spawn].SetActive(true);
                all_tile_groups[current_tile_to_spawn].transform.position = spawns[0].transform.position;
                all_tile_groups[current_tile_to_spawn].GetComponent<TileGroup>().MoveToPosition(spots_to_move_to[current_spot_to_move_to].position);
                all_tile_groups[current_tile_to_spawn].GetComponent<TileGroup>().current_spawn_index = 0;
                spawn_timer = max_spawn_time;
                current_tile_to_spawn++;
            }
            else
            {
                spawn_timer -= Time.deltaTime; 
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateTileGroups(numOfTileGroups);
            time_to_spawn = true;

        }
    }

    public void CreateTileGroups(int numOfTileGroups)
    {
        float tgOff = 5; 
        for (int i = 0; i < numOfTileGroups; i++)
        {
            Debug.Log(canvas_name_template);
            GameObject c = Instantiate(canvas_name_template);
            GameObject g = Instantiate(tileGroup);
            c.transform.parent = g.transform; 
            g.GetComponent<TileGroup>().CreateTileGroup(5);
            g.transform.localScale = new Vector3(.3f, .3f, .3f);
            all_tile_groups.Add(g);
            tileGroup.transform.position = new Vector3(transform.position.x + tgOff, transform.position.y, transform.position.z);
            c.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + .9f, g.transform.position.z);
            c.transform.localScale = new Vector3(.005f, .005f, .005f);
            c.transform.GetChild(0).GetComponent<Text>().text = dreamRef.GetRandomDreamItem();
            g.SetActive(false);

            tgOff += 5; 
        }
    }



}
