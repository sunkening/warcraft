using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : BaseSprite {
    public List<Sprite>[] directonFrames = new List<Sprite>[(int)UnitDirection.Total];
}
