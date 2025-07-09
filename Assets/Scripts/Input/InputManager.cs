using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InputManager : MonoBehaviour
{
    public Profile currentProfile;

    private KeyCode key1;
    private KeyCode key2;
    private KeyCode key3;
    private KeyCode key4;

    public SoundManager soundManager;

    void Start()
    {
        LoadKeyCodesFromProfile();

        if (soundManager == null)
            soundManager = FindObjectOfType<SoundManager>();
    }

    void LoadKeyCodesFromProfile()
    {
        key1 = ParseKeyCode(currentProfile.playstyle.keybinds.k1);
        key2 = ParseKeyCode(currentProfile.playstyle.keybinds.k2);
        key3 = ParseKeyCode(currentProfile.playstyle.keybinds.k3);
        key4 = ParseKeyCode(currentProfile.playstyle.keybinds.k4);
    }

    KeyCode ParseKeyCode(string keyString)
    {
        try
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
        }
        catch
        {
            Debug.LogWarning($"KeyCode parsing failed for '{keyString}', defaulting to None");
            return KeyCode.None;
        }
    }

    public bool IsDonPressed()
    {
        bool pressed = Input.GetKeyDown(key3) || Input.GetKeyDown(key4);
        if (pressed && soundManager != null)
            soundManager.PlayDon();
        return pressed;
    }

    public bool IsKanPressed()
    {
        bool pressed = Input.GetKeyDown(key1) || Input.GetKeyDown(key2);
        if (pressed && soundManager != null)
            soundManager.PlayKan();
        return pressed;
    }
}
