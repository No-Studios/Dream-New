using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroup : MonoBehaviour
{
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

    public List<GameObject> child_tiles; 
    // Start is called before the first frame update
    void Start()
    {
        child_tiles = new List<GameObject>(); 
        all_tiles = new List<SerializedTile>();
        base_tile = new SerializedTile();
        all_tiles.Add(base_tile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            CreateTileGroup(group_length);
            this.transform.localScale = new Vector3(.5f, .5f, .5f);


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
