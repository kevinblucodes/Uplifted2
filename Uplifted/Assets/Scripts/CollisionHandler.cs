
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float reloadDelay = 1f;
    [SerializeField] float loadNextDelay = 1f;
    [SerializeField] float rotationThreshold = 2f;
    [SerializeField] AudioClip rocketExplosion;
    [SerializeField] AudioClip winBell;

    Rigidbody rb;
    AudioSource audioSource;

    bool isTransitioning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (!isTransitioning)
        {
            ProcessCollision(other);
        }
    }


    void OnCollisionStay(Collision other)
    {
        if (!isTransitioning)
        {
            float zRotation = transform.eulerAngles.z % 360;
            if (Mathf.Abs(zRotation) < rotationThreshold)
            {
                ProcessCollision(other);
            }
        }
    }

    void ProcessCollision(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                float zRotation = transform.eulerAngles.z % 360;
                if (Mathf.Abs(zRotation) < rotationThreshold)
                {
                    StartSuccessSequence();
                }
                else
                {
                    Debug.Log("Z rotation is out of range");
                }
                break;
            case "Fuel":
                Debug.Log("Fuel Acquired");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        //todo add particle effect upon landing
        rb.freezeRotation = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(winBell);
        }
        
        Invoke("LoadNextLevel", loadNextDelay);
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        //todo add particle effect upon crash
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(rocketExplosion);
        }
        Invoke("ReloadLevel", reloadDelay);
    }

    

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
