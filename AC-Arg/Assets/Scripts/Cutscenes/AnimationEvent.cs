using UnityEngine;

[CreateAssetMenu(fileName = "AnimationEvent", menuName = "CutsceneEvents/AnimationEvent", order = 1)]
public class AnimationEvent : CutsceneEvent
{
    [SerializeField] private CutsceneAnimationTarget targetModel; //tal vez sea mejor pedir un Animator?
    [SerializeField] private string animationName;

    public CutsceneAnimationTarget TargetModel => targetModel;
    public string AnimationName => animationName;

    public override void Execute()
    {
        //targetModel.GetComponent<Animator>().Play(animationName);
        CutsceneAnimationManager.Instance.PlayCutsceneAnimation(targetModel, animationName);
        Debug.Log("Animation Event: playing animation " + animationName + " on " + targetModel);
    }
}
