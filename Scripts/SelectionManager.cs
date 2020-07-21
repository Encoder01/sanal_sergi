using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [SerializeField]
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private string selectableTag = "SelectableResim";
        [SerializeField] private string selectableTag2 = "SelectableVideo";
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private GameObject panelVideo;
        [SerializeField] private GameObject panelResim;
        [SerializeField] private GameObject panelMenu;
        [SerializeField] private Image nokta;
        [SerializeField] private Image tablo;
        [SerializeField] private Text bilgiResim;
        [SerializeField] private Text bilgiVideo;
        [SerializeField] private Text HazirlayanResim;
         [SerializeField] private Text HazirlayanVideo;
        public VideoPlayer videoPlayer;
        public AudioSource ses;
        public bool isPaused = false;


        protected Transform _selection;


        private void Start()
        {
            panelMenu.SetActive(false);
            panelResim.SetActive(false);
            panelVideo.SetActive(false);
            nokta.enabled = true;
        }
        // Update is called once per frame
        void Update()
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                PauseGame();
            }
            
            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
                _selection = null;
                panelResim.SetActive(false);
                panelVideo.SetActive(false);
                nokta.enabled = true;
            }
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(ray, out hit, 2.0f))
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag) && isPaused == false)
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (Input.GetMouseButton(0))
                    {
                        Sprite sprite = Resources.Load<Sprite>("Sprites/" + selection.gameObject.name);
                        tablo.sprite = sprite;
                        string path = "Assets/Resources/" + selection.gameObject.name + ".txt";
                        StreamReader reader = new StreamReader(path);
                        string itemStrings = reader.ReadLine();
                         
                            while (itemStrings != null) 
                            {
                                string[] fields = itemStrings.Split('@');

                               HazirlayanResim.text=fields[0];
                               bilgiResim.text = fields[1];

                                itemStrings = reader.ReadLine();
                                 
                            }
                            
                        if (selectionRenderer != null)
                        {
                            panelResim.SetActive(true);
                            nokta.enabled = false;
                        }
                    }
                   

                    selectionRenderer.material = highlightMaterial;
                    _selection = selection;
                }
                else if (selection.CompareTag(selectableTag2) && isPaused == false)
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (Input.GetMouseButton(0))
                    {
                        string path = "Assets/Resources/" + selection.gameObject.name + ".txt";

                        StreamReader reader = new StreamReader(path);
                        string itemStrings = reader.ReadLine();
                        while (itemStrings != null)
                        {
                            string[] fields = itemStrings.Split('@');

                            HazirlayanVideo.text = fields[0];
                            bilgiVideo.text = fields[1];


                            itemStrings = reader.ReadLine();

                        }

                        VideoClip clip = Resources.Load<VideoClip>("Videos/" + selection.gameObject.name);
                        videoPlayer.clip = clip;
                        videoPlayer.Play();
                        ses.volume = 0.1f;
                        if (selectionRenderer != null)
                        {
                            panelVideo.SetActive(true);
                            nokta.enabled = false;
                        }
                    }
                    else
                    {
                        if (videoPlayer != null)
                            videoPlayer.Stop();
                        ses.volume = 0.7f;
                    }

                    selectionRenderer.material = highlightMaterial;
                    _selection = selection;
                }

            }
            else
            {
                if (videoPlayer != null)
                    videoPlayer.Stop();
            }
        }
     
       
        public void ResumeGame()
        {
            panelMenu.SetActive(false);
            panelResim.SetActive(false);
            panelVideo.SetActive(false);
            nokta.enabled = true;
            Time.timeScale = 1;
            isPaused = false;
            AudioListener.pause = false;
           
        }
        public void PauseGame()
        {
            isPaused = true;
            AudioListener.pause = true;
            panelMenu.SetActive(true);
            panelResim.SetActive(false);
            panelVideo.SetActive(false);
            nokta.enabled = false;
            if (videoPlayer != null)
                videoPlayer.Stop();
            Time.timeScale = 0;
        }
        

        public void QuitGame()
        {

            Application.Quit();
        }
    }
}