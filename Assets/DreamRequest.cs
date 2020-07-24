using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
public class DreamRequest : MonoBehaviour
{

    public string[] dream_components;

    public string[] dream_items; 
    public Text[] ui_text_components; 
    Dictionary<string, int> component_to_amountWanted; 

    
    // Start is called before the first frame update
    void Start()
    {
        component_to_amountWanted = new Dictionary<string, int>(); 
        foreach(string s in dream_components)
        {
            component_to_amountWanted.Add(s, Random.Range(1, 15));
        }

        int compCounter = 0; 
        foreach(Text t in ui_text_components)
        {
            t.text = "" + dream_components[compCounter] + ": " + component_to_amountWanted[dream_components[compCounter]];
            compCounter++; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetRandomDreamItem()
    {
        return dream_items[Random.Range(0, dream_items.Length)];
    }


}
