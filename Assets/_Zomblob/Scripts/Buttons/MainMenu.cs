using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider musicSlider;
    public string firstLevelName = "Level1";
    public string OptionsMenu = "options here";
    public string customizeMenu = "customize here";

    public GameObject Titlepage;
    public GameObject customizePage;
    public GameObject OptionsPage;
    public GameObject[] characters;
    public GameObject Music;
    public GameObject FireSound;
    public GameObject MusicPanel;


    void Start()
    {
        Titlepage.SetActive(true);
        customizePage.SetActive(false);
        OptionsPage.SetActive(false);
        Music.SetActive(true);
        FireSound.SetActive(false);
        musicSlider.value = musicSource.volume;
        musicSlider.onValueChanged.AddListener(v=> musicSource.volume = v);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static class gameSettings{
        public static bool calm = false;
    }
    public void startGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(firstLevelName);
    }
    public void openOptions()
    {
        FindObjectOfType<CameraSweep>().SweepToC();
        Titlepage.SetActive(false);
        OptionsPage.SetActive(true);
    }
 
    public void Customize()
    {
        FindObjectOfType<CameraSweep>().SweepToB();
        OptionsPage.SetActive(false);
        Titlepage.SetActive(false);
        customizePage.SetActive(true);

    }
    public void calmMode()
    {
        MusicPanel.SetActive(false);
        Music.SetActive(false);
        FireSound.SetActive(true);
        gameSettings.calm = true;

    }
    public void exitCalmMode()
    {
        MusicPanel.SetActive(true);
        Music.SetActive(true);
        FireSound.SetActive(false);
        gameSettings.calm = false;

    }
    public void backToMenu()
    {
        FindObjectOfType<CameraSweep>().SweepToA();
        OptionsPage.SetActive(false);
        customizePage.SetActive(false);
        Titlepage.SetActive(true);
        if (gameSettings.calm == true){
        FireSound.SetActive(true);
        Music.SetActive(false);
         }else{
        FireSound.SetActive(false);
        Music.SetActive(true);
    }
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void characterSelectLeft(){
        for (int i=0; i<6; i++){
            if (characters[i].activeInHierarchy){
                characters[i].SetActive(false);
                if (i==0){
                    characters[5].SetActive(true);
                }
                else{
                    characters[i-1].SetActive(true);
                }
                break;
            }
        }
    }
    public void characterSelectRight(){
        for (int i=0; i<6; i++){
            if (characters[i].activeInHierarchy){
                characters[i].SetActive(false);
                if (i==5){
                    characters[0].SetActive(true);
                }
                else{
                    characters[i+1].SetActive(true);
                }
                break;
            }
        }
    }

}
