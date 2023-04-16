using Characters;
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
        public bool keepPlayer;
        public int lvl = 1;
        public int scale = 1;
        private int currLvl = 1;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            // Load overlay canvas with simple instructions.
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
            keepPlayer = false;
            SceneManager.LoadSceneAsync("ChooseCharacter");
            var button = GameObject.Find("Start").GetComponent<Button>();
            button.onClick.AddListener(Play);
            Debug.Log("Added listener");
        }
        
        public void Respawn()
        {
            keepPlayer = true;
            Play();
        }

        public void Play()
        {
            Debug.Log("Called Play");
            SceneManager.LoadSceneAsync("PlayDemo");
        }

        public void Next()
        {
            currLvl++;
            Clear();
            Play();
        }

        public void Win()
        {
            Destroy(level);
            SceneManager.LoadScene("WinScreen");
        }
        
        public void Lose()
        {
            Destroy(level);
            SceneManager.LoadScene("LossScreen");
        }

        public void Clear()
        {
            //spawned = false;
            // Destroy(player);
            Destroy(level);
            Destroy(GameObject.Find("Characters"));
            Destroy(GameObject.Find("Enemies"));
            Destroy(GameObject.Find("Items"));
        }

        private void OnEnable()
        {
            // Bind callback for every level change.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            // Cleanup callback for every level change.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // If this is the start of the main level, set it up.
            if (scene != SceneManager.GetSceneByName(Main) || levelPrefab == null)
            {
                return;
            }
                
            level = Instantiate(levelPrefab);
            LevelGenerator lg = level.GetComponent<LevelGenerator>();
            lg.setupLevelGenerator(scale, currLvl==lvl);
            lg.grid.InitializeGrid();
            if (keepPlayer)
            {
                var stats = player.GetComponent<Character>().CLS();
                player = currCharacter.GameObject();
                player.GetComponent<Character>().RestoreStats(stats);
                GetComponent<PlayerSpawner>().Spawn(player.GameObject());
            }
            else GetComponent<PlayerSpawner>().Spawn(currCharacter.GameObject());
        }
    }
}