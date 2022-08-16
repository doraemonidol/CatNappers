using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDeathTimer : MonoBehaviour
{
    public float timer;
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (particle)
                Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
