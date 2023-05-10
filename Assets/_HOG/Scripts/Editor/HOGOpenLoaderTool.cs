using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

public class HOGOpenLoaderTool
{
    public class OpenLoaderTool
    {
        [MenuItem("HOG/PlayGame")]
        //[System.Obsolete]
        public static void OpenLoader()
        {
            string currentSceneName = "GameScene";
            File.WriteAllText(".lastScene", currentSceneName);
            EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Hog/Scenes/Loader.unity");
            EditorApplication.isPlaying = true;
        }

        [MenuItem("HOG/LoadEditedScene")]
        public static void ReturnToLastScene()
        {
            string lastScene = File.ReadAllText(".lastScene");
            EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Hog/Scenes/{lastScene}.unity");
        }

    }
}
