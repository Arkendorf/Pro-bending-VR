using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class InputManager : MonoBehaviour
{
    public NetworkedPlayer myPlayer;
    public InputField nameInput;
    public GameObject canvas;
    public GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void UpdateNickname()
    {
        myPlayer.nickName.text = nameInput.text;
        canvas.SetActive(false);
        pointer.SetActive(false);
    }
}
