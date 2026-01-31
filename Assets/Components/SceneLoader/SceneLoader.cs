using UnityEngine;
using UnityEngine.SceneManagement;

namespace Component.SceneLoader
{
    public static class SceneLoader // faire cette classe (ce script), une classe static, afin de pouvoir l'appeler de n'importe quelles scenes
    {
        public static void LoadLevel()
        {
            // loader plusieurs scenes en meme temps :
            SceneManager.LoadScene("Level"); // loader la premiere 
            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive); // et j'ajoute donc ma scene "LevelUI" et precisant "LoadSceneMode.Additive", me permet de l'afficher plusieur scene en meme temps   
        }

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


