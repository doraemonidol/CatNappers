using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
   private void OncolilisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name=="End")
        {
            SceneManager.LoadScene("v2");
        }
    }
}
