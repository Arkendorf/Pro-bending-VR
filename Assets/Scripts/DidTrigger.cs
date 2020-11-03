using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidTrigger : MonoBehaviour
{
    public bool didTrigger;
    // Start is called before the first frame update
    void Start()
    {
        didTrigger = false; 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        didTrigger = true;
    }

    public bool didYouTrigger(){
        return didTrigger;
    }
    
}
