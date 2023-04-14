using RoomGen;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject levelPrefab;
        public GameObject level;
        private const string Main = "PlayDemo";
        public GameObject currCharacter;
        public GameObject player = null;
        public int lvl = 1;
        public int scale = 1;
        private int currLvl = 1;

        private bool spawned = true;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName (Main) && SceneManager.GetActiveScene().isLoaded && !spawned)
            {
                level = Instantiate(levelPrefab);
                spawned = true;
                var lg = GameObject.FindWithTag("Level").GetComponent<LevelGenerator>();
                lg.setupLevelGenerator(scale, currLvl==lvl);
                lg.grid.InitializeGrid();
                player = GetComponent<PlayerSpawner>().Spawn(currCharacter.GameObject());
            }
        }

        public void SetCharacter(GameObject character)
        {
            currCharacter = character;
        }
        
        public void SetLevels(Slider slider)
        {
            lvl = (int)slider.value;
        }
        
        public void SetRooms(Slider slider)
        {
            scale = (int)slider.value;
        }
        
        public void PlayGame()
        {
            SceneManager.LoadScene("GameSetup");
        }
        public void HowTo()
        {
            ///load overlay canvas with simple instructions
        }
        
        public void Solo()
        {
            SceneManager.LoadScene("ChooseLevel");
        }
        /*
        public void Multi()
        {
            SceneManager.LoadScene("Starting");
        }
        */
        
        public void ChooseLevel()
        {
            // Currently does not work for playing again
            SceneManager.LoadSceneAsync("ChooseCharacter");
            Button button = GameObject.Find("Start").GetComponent<Button>();
            button.onClick.AddListener(Play);
            Debug.Log("added listener");
            //spawned = false;
            //Update();
        }

        
        
        public void Play()
        {
            Debug.Log("Called Play");
            spawned = false;
            Update();
            //DontDestroyOnLoad(gameObject);
            SceneManager.LoadSceneAsync("PlayDemo");
            player = GameObject.FindWithTag("Player");
        }

        public void Next()
        {
            currLvl++;
            Clear();
            Play();
            //SceneManager.LoadScene("LoadingScreen");
        }

        public void Win()
        {
            Clear();
            SceneManager.LoadScene("WinScreen");
        }
        
        public void Lose()
        {
            Clear();
            SceneManager.LoadScene("LossScreen");
        }

        public void Clear()
        {
            spawned = false;
            Destroy(player);
            Destroy(level);
            Destroy(GameObject.Find("Characters"));
            Destroy(GameObject.Find("Enemies"));
            Destroy(GameObject.Find("Items"));
        }
    }
}