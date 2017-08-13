using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 相当于原引擎的Animations
/// </summary>
public class CharacterAnimation 
{
    //public List<Sprite>[]  anim = new List<Sprite>[(int)UnitDirection.Total] ;
    public AnimationFram Start;
    public AnimationFram Still;
    public AnimationFram Death;
    public AnimationFram Attack;
    public AnimationFram Move;
    public AnimationFram Repair;
    public AnimationFram Train;
    public AnimationFram Research;
    public AnimationFram Upgrade;
    public AnimationFram Build;
    public AnimationFram[] Harvest=new AnimationFram[(int)ResourceType.MaxCosts];
    
}
