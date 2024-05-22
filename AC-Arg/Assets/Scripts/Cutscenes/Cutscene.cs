using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "CutsceneEvents/Cutscene", order = 4)]
public class Cutscene : ScriptableObject
{
    [SerializeField] private List<ICutsceneEvent> events;

    public List<ICutsceneEvent> Events => events;
}
