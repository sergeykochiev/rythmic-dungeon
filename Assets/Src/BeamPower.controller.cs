using UnityEngine;

public class BeamPowerController : PowerController
{
    public static int DefaultUseInterval = 5;
    public BeamPowerEntity prefab;

    public override void Init(Transform powerWielder)
    {
        this.powerWielder = powerWielder;
    }

    public override void OnUse()
    {
        foreach (var direction in Constants.Directions)
        {
            BeamPowerEntity entity = Instantiate(prefab, powerWielder.transform);
            entity.Blast(direction);
        }
    }

    public override int GetUseInterval()
    {
        return DefaultUseInterval;
    }
}