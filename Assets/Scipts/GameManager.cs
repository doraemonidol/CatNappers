using System.Collections;
using AxieMixer.Unity;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject _startMsgGO;
        [SerializeField] Button _mixBtn;
        [SerializeField] InputField _idInput;
        [SerializeField] AxieFigure _birdFigure;
        [SerializeField] AxieFigure _birdFigure2;
        [SerializeField] public string[] figureList = { "6679798" , "10814754" , "5815197" , "11356360" };

        bool _isPlaying = false;
        bool _isFetchingGenes = false;

        private void OnEnable()
        {
            _mixBtn.onClick.AddListener(OnMixButtonClicked);
        }

        private void OnDisable()
        {
            _mixBtn.onClick.RemoveListener(OnMixButtonClicked);
        }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;

        Mixer.Init();

        string[] axieId = { "641753", "978176" }; 
            string[] genes = { "0x800000000000010001c0402084080000000100002801400a0001000008608002000100000800820a000100001000800400010000080040020001000028004002", "0x800000000000010001c0402104100000000100002801400a0001000008004002000100001800c00400010000100080040001000008004002000100002801400a" };
        // 641753 0x800000000000010001c0402084080000000100002801400a0001000008608002000100000800820a000100001000800400010000080040020001000028004002
        // 978176 0x800000000000010001c0402104100000000100002801400a0001000008004002000100001800c00400010000100080040001000008004002000100002801400a
        //_idInput.text = axieId;
            _birdFigure.SetGenes(axieId[0], genes[0]);
            _birdFigure2.SetGenes(axieId[1], genes[1]);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isPlaying)
            {
                _startMsgGO.SetActive((Time.unscaledTime % .5 < .2));
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    _startMsgGO.SetActive(false);
                    _isPlaying = true;
                    Time.timeScale = 1f;
                }
            }
        }

        void OnMixButtonClicked()
        {
            if (string.IsNullOrEmpty(_idInput.text) || _isFetchingGenes) return;
            _isFetchingGenes = true;
            StartCoroutine(GetAxiesGenes(_idInput.text));
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
                    _idInput.text = axieId;
                    _birdFigure.SetGenes(axieId, genesStr);
                Debug.Log(genesStr);
                }
            }
            _isFetchingGenes = false;
        }
    }
