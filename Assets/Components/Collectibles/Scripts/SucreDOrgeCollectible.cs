using UnityEngine;

public class SucreDOrgeCollectible : CollectibleCM // remplacer Monobehavior ( car de base chacun de nos scripts herite de Monobehavior, mettre a la place Food (le nom de ma class mere)
{
    public override void Collected() // ceci s'applle overrider la fonction, j'ajoute le mot "override"
    {
        base.Collected(); // me permet de garder ma fonction Eated de base
                      // et j'y ajoute mon code supplementaire par au dessus ou en dessous
    }
}
