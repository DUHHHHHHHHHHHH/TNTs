using System;

[Serializable]
public class Profile
{
    public string userName;
    public Playstyle playstyle;
}

[Serializable]
public class Playstyle
{
    public Keybinds keybinds;
}

[Serializable]
public class Keybinds
{
    public string k1;
    public string k2;
    public string k3;
    public string k4;
}
