using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroup : MonoBehaviour
{

    Canvas ca = new Canvas(); 
    
    [SerializeField]
    public TileBase tile_to_place = null;
    public GameObject temp_tile_to_place;
    
    [SerializeField]
    private int group_length = 0;

    public float tile_length = 1;
    public float tile_height = 1;

    private List<SerializedTile> all_tiles;
    private SerializedTile base_tile;
    public float xOffset = .8f;

    public bool moving_to_position = false;
    private Vector3 current_position_move_to; 

    public List<GameObject> child_tiles;
    public bool goto_next_spawn = false;

    public GameManager gm;
    public int current_spawn_index = 0;
    public int move_index = 0;
    public float next_move_wait;
    public float next_move_wait_max = 2f;
    public bool next_move_ready = true;
    private bool start_next_timer = false; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        child_tiles = new List<GameObject>();
        all_tiles = new List<SerializedTile>();
        base_tile = new SerializedTile();
        all_tiles.Add(base_tile);
        next_move_wait = next_move_wait_max;
        next_move_ready = true;

}
// Update is called once per frame
void Update()
    {

        if (start_next_timer)
        {
            if(next_move_wait <= 0)
            {
                next_move_ready = true;
                start_next_timer = false;
                next_move_wait = next_move_wait_max;
            }
            else
            {
                next_move_wait -= Time.deltaTime;
            }
        }

        if (moving_to_position && next_move_ready)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, current_position_move_to, 2f * Time.deltaTime);
        }

        if (goto_next_spawn)
        {
            Debug.Log("yyyyyy");
            next_move_ready = false;
            current_spawn_index++;
            move_index++;

            if (move_index >= gm.spots_to_move_to.Length)
            {
                move_index = 0;
            }

            if (current_spawn_index >= gm.spawns.Length)
            {
                Destroy(this);
                current_spawn_index = 0;
            }
            Debug.Log("current: " + gm.spawns.Length);
            this.transform.position = gm.spawns[current_spawn_index].position;
            MoveToPosition(gm.spots_to_move_to[move_index].position);
            goto_next_spawn = false;
            next_move_wait = next_move_wait_max;
            start_next_timer = true; 
        }
    }

    public void CreateTileGroup(int l)
    {
        group_length = l;
        for (int i = 1; i < group_length; i++)
        {
            SerializedTile tempTile = ChooseRandomTile();
            SerializedTile newTile = tempTile.SetRandomAdjacent(all_tiles);

            if (newTile != null)
            {
                all_tiles.Add(newTile);
            }
            else
            {
                Debug.LogWarning("null tile @: " + i);
            }
        }

        foreach(SerializedTile s in all_tiles)
        {


            GameObject g = Instantiate(temp_tile_to_place);
            g.AddComponent(typeof(FakeTile));
            //g.AddComponent(typeof(BoxCollider2D));
            g.GetComponent<FakeTile>().x = s.x;
            g.GetComponent<FakeTile>().y = s.y;
            float xPos = 0;
            g.name = "x: " + s.x + " y: " + s.y;
            xPos = s.x * .98f;
            if (Mathf.Abs(s.y % 2) == 1)
            {
                Debug.Log("yoyo");
                xPos += xOffset;
            }

            
            g.transform.position = new Vector3(this.transform.position.x + xPos , this.transform.position.y + s.y * .85f, 0);
            g.transform.parent = this.gameObject.transform;
            //Debug.Log("x: " + s.x + " y: " + s.y);
            string h = "";
            foreach(SerializedTile m in s.adjacent_tiles) {
                if (m == null)
                {
                    h += "null, ";
                }
                else
                {
                    h += m.ToString() + ", ";
                }
            }
            child_tiles.Add(g);
            //Debug.Log(h);
        }
        
        
        
    }

    public void MoveToPosition(Vector3 pos_to_move_to)
    {
        current_position_move_to = pos_to_move_to;
        moving_to_position = true; 
    }

    public class SerializedTile
    {
        public int x = 0;
        public int y = 0;
        public List<int> indexValues;
        public SerializedTile()
        {
            x = 0;
            y = 0;
            indexValues = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                indexValues.Add(i); 
            }
        }


        public SerializedTile[] adjacent_tiles = { null, null, null, null, null, null };

        public SerializedTile SetRandomAdjacent(List<SerializedTile> allTiles)
        {
            SerializedTile rand = new SerializedTile();
            bool debugInloop = false;
            int removeCount = 0; 
            while(rand != null)
            {
                int randIndex = Random.Range(0, indexValues.Count);
                rand = adjacent_tiles[randIndex];
                if (rand == null)
                {
                    debugInloop = true;
                    adjacent_tiles[randIndex] = new SerializedTile();
                    int indexToPlacePrevious = 0;
                    int newX = 0;
                    int newY = 0;
                    switch (randIndex)
                    {
                        case 0:
                            indexToPlacePrevious = 3;

                            newY = y; 
                            newX = x - 1; 
                            break;
                        case 1:
                            indexToPlacePrevious = 4;
                            newX = x - 1;
                            if (Mathf.Abs(y % 2) == 1)
                            {
                                newX = x;

                            }
                            newY = y - 1;
                            break;
                        case 2:
                            indexToPlacePrevious = 5;
                            newX = x;
                            if (Mathf.Abs(y % 2) == 1)
                            {
                                newX = x + 1;

                            }
                            newY = y - 1; 
                            break;
                        case 3:
                            indexToPlacePrevious = 0;
                            newX = x + 1;

                            newY = y;
                            break;
                        case 4:
                            indexToPlacePrevious = 1;
                            newX = x;
                            if (Mathf.Abs(y % 2) == 1)
                            {
                                newX = x + 1;

                            }
                            newY = y + 1; 
                            break;
                        case 5:
                            indexToPlacePrevious = 2;
                            newX = x - 1;
                            if (Mathf.Abs(y % 2) == 1)
                            {
                                newX = x;

                            }
                            newY = y + 1;
                            break;
                    }

                    bool checkAnother = false;

                    foreach (SerializedTile j in allTiles)
                    {
                        if(j.x == newX && j.y == newY)
                        {
                            checkAnother = true; 
                        }
                    }

                    if (!checkAnother)
                    {
                        adjacent_tiles[randIndex].y = newY;
                        adjacent_tiles[randIndex].x = newX;
                        adjacent_tiles[randIndex].adjacent_tiles[indexToPlacePrevious] = this;
                        return adjacent_tiles[randIndex];
                    }
                    else
                    {
                        List<int> tmpList = new List<int>();
                        removeCount++;
                        for(int p = 0; p < indexValues.Count; p++)
                        {
                            if(p != randIndex)
                            {
                                tmpList.Add(p);
                            }
                        }


                        indexValues = tmpList;
                        rand = new SerializedTile();
                        Debug.Log(indexValues.Count);
                        
                        if(tmpList.Count == 0)
                        {
                            return null; 
                        }
                        
                    }

                }


            }
            Debug.LogWarning("NO SIRRR: " + rand + " debug in loop: " + debugInloop);

            return rand;
        }

        public void SetAdjacentTile()
        {

        }

    }

    private SerializedTile ChooseRandomTile()
    {
        int randIndex = Random.Range(0, all_tiles.Count);
        return all_tiles[randIndex];
    }



    
}
