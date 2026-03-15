using Game.Enums;
using UnityEngine;

namespace Game.Actors
{
    public class MonsterActor : Actor
    {
        [SerializeField] private EFertilizerType _conditionType;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                if (character.TryGetFertilizer(out FertilizerActor fertilizer))
                {
                    if (_conditionType == fertilizer.FertilizerType)
                    {
                        fertilizer.SelfTransform.SetParent(null);
                        Debug.Log("AAAAAAA!!!");
                    }
                    else
                    {
                        Debug.Log("GG WP");
                    }
                }
            }
        }
    }
}