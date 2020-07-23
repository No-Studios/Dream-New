using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public GameObject tileGroup; 
    // Start is called before the first frame update
    void Start()
    {
        CreateTileGroups(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTileGroups(int numOfTileGroups)
    {
        float tgOff = 5; 
        for (int i = 0; i < numOfTileGroups; i++)
        {
            Instantiate(tileGroup);

            tileGroup.transform.position = new Vector3(transform.position.x + tgOff, transform.position.y, transform.position.z);
            tgOff += 5; 
        }
    }



}
