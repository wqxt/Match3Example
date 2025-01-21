using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
    public class Game : MonoBehaviour
    {
 
        void Start()
        {
        
        }


        void Update()
        {
            if(Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("Gameplay");
            }
        }
    }
}
