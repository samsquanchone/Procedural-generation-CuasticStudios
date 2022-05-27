using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill 
{
    public int level;
    public Sprite skillIcon;
    private int currentExp;
    private int expToNextLevel;

    public Skill()
    {
        level = 1;
        Mathf.Clamp(level, 1, 99);
        expToNextLevel = 5;
        currentExp = 0;
    }

    public void AddExp (int xpToAdd)
    {
        currentExp += xpToAdd;
        if (CheckIfCanLevelUp())
            LevelUp();      
    }

    private bool CheckIfCanLevelUp ()
    {
        if (currentExp >= expToNextLevel)
            return true;
        else
            return false;
    }

    private void LevelUp ()
    {
        level += 1;
        expToNextLevel = currentExp + level;
        currentExp = 0;
        Debug.Log("Skill level up!");
    }

    public int GetCurrentExp()
    {
        return currentExp;
    }
    public int GetExpToNextLevel()
    {
        return expToNextLevel;
    }
}
