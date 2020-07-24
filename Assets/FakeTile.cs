using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTile : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public PlayerController pRef; 
    bool already_clicked = false;
    TileGroup parentGroup; 
    // Start is called before the first frame update
    void Start()
    {
        pRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        parentGroup = transform.parent.GetComponent<TileGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!already_clicked)
        {
            pRef.selected_tile_group = this.transform.parent.gameObject;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hooo");
        if(collision.tag == "EP")
        {
            if(this.gameObject.transform.parent.GetComponent<TileGroup>().current_spawn_index == 1)
            {
                Debug.LogError("WORK THOT");
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            parentGroup.moving_to_position = false;
            parentGroup.goto_next_spawn = true;
        }
    }
}
