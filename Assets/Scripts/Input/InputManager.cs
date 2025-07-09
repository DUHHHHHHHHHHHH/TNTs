using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Profile currentProfile;

    private KeyCode key1;
    private KeyCode key2;
    private KeyCode key3;
    private KeyCode key4;

    void Start()
    {
        LoadKeyCodesFromProfile();
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

    public bool IsDonPressed() { return Input.GetKeyDown(key3) || Input.GetKeyDown(key4); }
    public bool IsKanPressed() { return Input.GetKeyDown(key1) || Input.GetKeyDown(key2); }
}
