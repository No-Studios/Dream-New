using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTile : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public PlayerController pRef; 
    bool already_clicked = false; 
    // Start is called before the first frame update
    void Start()
    {
        pRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
}
