using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleSet", menuName = "SubtitleSet")]
public class SubtitleSetSO : ScriptableObject
{
    public SubtitleLine[] subtitles;
}
