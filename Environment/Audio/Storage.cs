using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Class for storing audio settings
 */
public class Storage : MonoBehaviour
{
    public Slider musicSlider, combatSlider;

    void Start()
    {
        int level = SceneManager.GetActiveScene().buildIndex;

        onLevelLoaded(level);

    }

    void onLevelLoaded(int level)
    {
        if (level == 0)
        {
            GameObject source = GameObject.Find("AudioManager");

            if (musicSlider == null || combatSlider == null) return;
            musicSlider.onValueChanged.AddListener(delegate { source.GetComponent<AudioManager>().updateMusicVolume(musicSlider.value); });
            combatSlider.onValueChanged.AddListener(delegate { source.GetComponent<AudioManager>().updateCombatVolume(combatSlider.value); });
        }
    }
}
