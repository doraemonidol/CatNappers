using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class OrbColliding : MonoBehaviour
{
    [SerializeField] AxieFigure _birdFigure;
    [SerializeField] GameManager controller;
    [SerializeField] GameObject GunPivot;
    [SerializeField] GameObject FireBallPivot;
    //bool _isPlaying = false;
    bool _isFetchingGenes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Orb")
        {
            Debug.Log("Orb detected");
            this.GetComponent<Player>().resetSkills();
            this.GetComponent<Player>().type = collision.gameObject.GetComponent<OrbManager>().type;
            string axieId = controller.figureList[collision.gameObject.GetComponent<OrbManager>().type];
            Debug.Log(axieId);
            /*string genes = PlayerPrefs.GetString("selectingGenes", "0x2000000000000300008100e08308000000010010088081040001000010a043020000009008004106000100100860c40200010000084081060001001410a04406");
            _birdFigure.SetGenes(axieId, genes);*/
            if (string.IsNullOrEmpty(axieId) || _isFetchingGenes) return;
            _isFetchingGenes = true;
            StartCoroutine(GetAxiesGenes(axieId));
            if (this.GetComponent<Player>().type == 2)
            {
                GunPivot.SetActive(true);
                this.GetComponent<SpringJoint2D>().enabled = true;
            }
            if (this.GetComponent<Player>().type == 3)
            {
                this.GetComponent<FireballShooter>().enabled = true;
            }
            Destroy(collision.gameObject);
        }
        int type = this.GetComponent<Player>().type;
        if (collision.gameObject.tag == "Water" && type != 0) {
            //Lose
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Thua");
        }
        if (collision.gameObject.tag == "Obstacles" && type != 1) {
            //Lose
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Thua");
        }
    }

    public IEnumerator GetAxiesGenes(string axieId)
    {
        string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
        JObject jPayload = new JObject();
        jPayload.Add(new JProperty("query", searchString));

        var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
        wr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        wr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");
        wr.timeout = 10;
        yield return wr.SendWebRequest();
        if (wr.error == null)
        {
            var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
            if (!string.IsNullOrEmpty(result))
            {
                JObject jResult = JObject.Parse(result);
                string genesStr = (string)jResult["data"]["axie"]["newGenes"];
                PlayerPrefs.SetString("selectingId", axieId);
                PlayerPrefs.SetString("selectingGenes", genesStr);
                //_idInput.text = axieId;
                _birdFigure.SetGenes(axieId, genesStr);
            }
        }
        _isFetchingGenes = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
