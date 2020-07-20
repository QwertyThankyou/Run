using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LaunchScene : MonoBehaviour
{
    public VideoPlayer video;
    private AsyncOperation _async;

    IEnumerator Start()
    {
        _async = SceneManager.LoadSceneAsync(1);
        _async.allowSceneActivation = false;
        yield return new WaitForSeconds((float)video.clip.length);
        _async.allowSceneActivation = true;
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
