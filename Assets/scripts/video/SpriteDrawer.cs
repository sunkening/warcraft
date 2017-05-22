using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDrawer : MonoBehaviour {
    public SpriteRenderer m_spriteRenderer;
    public Transform m_transform;
	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public Unit unit;
    /**
**  Draw unit-type on map.
**
**  @param type    Unit-type pointer.
**  @param sprite  Sprite to use for drawing
**  @param player  Player number for color substitution.
**  @param frame   Animation frame of unit-type.
**  @param x       Screen X pixel postion to draw unit-type.
**  @param y       Screen Y pixel postion to draw unit-type.
**
**  @todo  Do screen position caculation in high level.
**         Better way to handle in x mirrored sprites.
*/
    public void drawUnitType(List<Sprite>[] sprite, int player,int frameNum)
    {
        UnitType type = unit.unitType;
        int direction = (int)unit.direction;
        if (type.needFlip)
        {
            m_spriteRenderer.flipX = true;
            if(direction > (int)UnitDirection.LookingS)
            {
                direction = (int)UnitDirection.LookingS - (direction - (int)UnitDirection.LookingS);
            }
        }
        List<Sprite> frames = sprite[direction];

        if (frameNum >= frames.Count) {
            frameNum = 0;
        }
        m_spriteRenderer.sprite= frames[frameNum];
    }
}
