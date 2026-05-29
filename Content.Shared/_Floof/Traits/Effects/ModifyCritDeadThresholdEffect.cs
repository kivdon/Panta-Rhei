using Content.Shared._DV.Traits.Effects;
using Content.Shared.Mobs.Components;

namespace Content.Shared._Floof.Traits.Effects;

/// <summary>
/// Effect that modifies the Critical and/or Dead threshold of an entity.
/// </summary>
public sealed partial class ModifyCritDeadThresholdEffect : BaseTraitEffect
{
    /// <summary>
    /// How much to multiply the Critical threshold by.
    /// </summary>
    [DataField]
    public float CritModifier = 0f;

    /// <summary>
    /// How much to multiply the Dead threshold by.
    /// </summary>
    [DataField]
    public float DeadModifier = 0f;

    public override void Apply(TraitEffectContext ctx)
    {
        if (!ctx.EntMan.TryGetComponent<MobThresholdsComponent>(ctx.Player, out var threshDict))
            return;

        //The new Dictionary we'll be playing with.
        SortedDictionary<FixedPoint2, MobState> newDict = new SortedDictionary<FixedPoint2, MobState>();
        newDict.Add(0, "Alive");

        // Make some temporary values to capture the existing Crit and Dead values. 
        //var newCrit = threshDict.FirstOrDefault(x => x.Value == "Critical").Key;
        //var newDead = threshDict.FirstOrDefault(x => x.Value == "Dead").Key;

        // Make some temporary values to capture the existing Crit and Dead values. 
        var newCrit = 0f;
        var newDead = 0f;

        //Grab the existing Dead value
        if (threshDict.TryGetValue("Dead", out newDead)) {
            //Make changes only if something is passed in via the trait.
            if (DeadModifier != 0} {
                newDead = newDead * DeadModifier;
            }
            //Grab the existing Crit value. There shouldn't be a case in which we send in a Dict that doesn't have Crit, but let's account for that anyway.
            if (threshDict.TryGetValue("Critical", out newCrit)) {
                //Make changes only if something is passed in via the trait.
                if (CritModifier != 0} {
                  newCrit = newCrit * CritModifier;

                  //Safeguard to make sure this value will not be too low, though this shouldn't happen.
                  if (newCrit <= 5) {
                    newCrit = 5;
                  }
                }
                //Safeguard to make sure Dead is not less than or equal to Crit.
                if (newDead <= newCrit) {
                    newDead = newCrit + 0.1;
                }
                
                newDict.Add(newCrit, "Critical");
                newDict.Add(newDead, "Dead");
            }
            else { //In the unlikely event that there is no Critical at all.
                //There is no reason the Dead value should ever be 0 or lower, but just in case...
                if (newDead <= 0) {
                    newDead = 5;
                }
                newDict.Add(newDead, "Dead");
            }
        }
        else {
            //This should never happen, but if there's no Dead value, there's a problem. Throw a value into it.
            newDict.Add(1, "Dead");
        }

/*
        //Make changes only if something is passed in via the trait.
        if (CritModifier != 0} {
          newCrit = newCrit * CritModifier;

          //Safeguard to make sure this value will not be too low, though this shouldn't happen.
          if (newCrit <= 5) {
            newCrit = 5;
          }
        }

        //Make changes only if something is passed in via the trait.
        if (DeadModifier != 0} {
          newDead = newDead * DeadModifier;

          //Safeguard to make sure Dead is not less than or equal to Crit.
          if (newDead <= newCrit) {
            newDead = newCrit + 0.1;
          }
        }
        
        SortedDictionary<FixedPoint2, MobState> newDict = new SortedDictionary<FixedPoint2, MobState>();
        newDict.Add(0, "Alive");
        newDict.Add(newCrit, "Critical");
        newDict.Add(newDead, "Dead");
*/

        threshDict = newDict;
    }
}
