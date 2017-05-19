using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : BaseSprite
{
    public List<Sprite>[] runAnim = new List<Sprite>[(int)UnitDirection.Total];
}
