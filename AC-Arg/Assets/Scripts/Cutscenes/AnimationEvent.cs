using UnityEngine;

[CreateAssetMenu(fileName = "AnimationEvent", menuName = "CutsceneEvents/AnimationEvent", order = 1)]
public class AnimationEvent : CutsceneEvent
{
    [SerializeField] private GameObject targetModel; //tal vez sea mejor pedir un Animator?
    [SerializeField] private string animationName;

    public GameObject TargetModel => targetModel;
    public string AnimationName => animationName;

    public override void Execute()
    {
        //targetModel.GetComponent<Animator>().Play(animationName);
        Debug.Log("Animation Event: playing animation " + animationName + " on " + targetModel);
    }
}
