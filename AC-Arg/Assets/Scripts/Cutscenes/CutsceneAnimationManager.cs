using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutsceneAnimationTarget
{
    Facundo,
    Varuzhan,
    Mother,
    WaterTank
}


public class CutsceneAnimationManager : Singleton<CutsceneAnimationManager>
{
    public Animator facundo, varuzhan, mother, waterTank;

    Dictionary<CutsceneAnimationTarget, Animator> animatorDict = new Dictionary<CutsceneAnimationTarget, Animator>();

    private void Start()
    {
        //fill the dict
        animatorDict.Add(CutsceneAnimationTarget.Facundo, facundo);
        animatorDict.Add(CutsceneAnimationTarget.Varuzhan, varuzhan);
        animatorDict.Add(CutsceneAnimationTarget.Mother, mother);
        animatorDict.Add(CutsceneAnimationTarget.WaterTank, waterTank);
    }

    public void PlayCutsceneAnimation(CutsceneAnimationTarget target, string animationClip)
    {
        //OJO QUE DEBEN LLAMARSE IGUAL EL CLIP Y EL ANIMATOR STATE
        animatorDict[target].Play(animationClip);
    }
}

