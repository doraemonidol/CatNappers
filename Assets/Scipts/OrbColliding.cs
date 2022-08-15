using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbColliding : MonoBehaviour
{
    [SerializeField] AxieFigure _birdFigure;
    string genes = PlayerPrefs.GetString("selectingGenes", "0x2000000000000300008100e08308000000010010088081040001000010a043020000009008004106000100100860c40200010000084081060001001410a04406");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Orb")
        {
            string axieId = PlayerPrefs.GetString("selectingId", "2727");//GameObject.FindGameObjectWithTag.SetGenes(axieId, genes);

            _birdFigure.SetGenes(axieId, genes);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
