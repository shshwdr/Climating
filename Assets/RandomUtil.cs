using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomUtil
{
    public static int RandomBasedOnProbability(List<float> probability)
    {
        float sum = probability.Sum();
        float rand = Random.Range(0, sum);
        for (int i = 0; i < probability.Count; i++)
        {
            rand -= probability[i];
            if (rand <= 0)
            {
                return i;
            }
        }
        Debug.LogError(("RandomUtil.RandomBasedOnProbability: Something went wrong"));
        return 0;
    }
    
    
    public static int RandomBasedOnProbabilityMaxWith100(List<float> probability)
    {
        float sum = probability.Sum();
        float rand = Random.Range(0, 100);
        for (int i = 0; i < probability.Count; i++)
        {
            rand -= probability[i];
            if (rand <= 0)
            {
                return i;
            }
        }
        Debug.LogError(("RandomUtil.RandomBasedOnProbability: Something went wrong"));
        return -1;
    }
}
