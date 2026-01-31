using System;

[Serializable] // lui dire de sérialiser cette class pour qu'elle puisse envoyer les donnée dans les fichier Json
public class SaveData // pas besoin de mettre MonoBehaviour, car notre SaveData, ne va jamais etre un Obj instancier dans la scene
{
    public string PlayerName;
    public int RunCount;
}

