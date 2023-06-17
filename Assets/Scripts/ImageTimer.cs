using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public bool Tick;
    public float maxTime;
    public float currentTime;
    private Image img;
    void Start()
    {
        img = GetComponent<Image>();
        currentTime = maxTime;
    }
        
   void Update()
    {
        Tick = false;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0 )
        {
            Tick = true;
            currentTime = maxTime;
        }
       img.fillAmount = currentTime / maxTime;
    }   
}
