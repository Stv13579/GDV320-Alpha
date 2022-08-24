using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that manages all functions relating to stat modifiers
public static class StatModifier //Sebastian
{
    [Serializable]
    //Modifier struct, contains the modifier value and a string representing the source of the modifier
    public struct Modifier
    {
        public float modifierValue;
        public string modifierSource;

        public Modifier(float value = 1.0f, string source = "")
        {
            modifierValue = value;
            modifierSource = source;
        }
    }

    [Serializable]
    //Contains all data about a particular stat and its modifiers
    public struct FullStat
    {
        public float baseValue;
        public List<Modifier> additiveModifiers;
        public List<Modifier> additiveMultiplicativeModifiers;
        public List<Modifier> multiplicativeModifiers;

        public FullStat(float value = 0.0f)
        {
            baseValue = value;
            additiveModifiers = new List<Modifier>();
            additiveMultiplicativeModifiers = new List<Modifier>();
            multiplicativeModifiers = new List<Modifier>();
        }

    }

    //Checks if the given modifier is already in the list, if not adds it
    public static void AddModifier(List<Modifier> modifierList, Modifier modifierToAdd)
    {
        if (CheckModifier(modifierList, modifierToAdd) == -1)
        {
            modifierList.Add(modifierToAdd);
        }
    }

    //Searches the given list for the given modifier, if it finds the modifier it removes it from the list
    public static void RemoveModifier(List<Modifier> modifierList, Modifier modifierToRemove)
    {
        int check = CheckModifier(modifierList, modifierToRemove);
        if (check != -1)
        {
            modifierList.RemoveAt(check);
        }
    }

    //Checks if the given list contains the given modifier, returns its index if so
    public static int CheckModifier(List<Modifier> modifierList, Modifier modifierToCheck)
    {
        foreach (Modifier mod in modifierList)
        {
            if (mod.modifierSource == modifierToCheck.modifierSource)
            {
                return modifierList.IndexOf(mod);
            }
        }
        return -1;
    }

    //Checks if the given modifier is in the given list, if so updates the modifier with the new value, otherwise adds it
    public static void UpdateModifier(List<Modifier> modifierList, Modifier modifierToUpdate)
    {
        int check = CheckModifier(modifierList, modifierToUpdate);
        if (check != -1)
        {
            modifierList.RemoveAt(check);
            modifierList.Add(modifierToUpdate);
        }
        else
        {
            AddModifier(modifierList, modifierToUpdate);
        }
    }

    //Takes the base value and all modifiers for a given value, applies all the modifiers to the value, and returns it
    public static float UpdateValue(FullStat fullStat)
    {
        float value = fullStat.baseValue;
        float valueModifier = 0;
        foreach (Modifier additiveModifier in fullStat.additiveModifiers)
        {
            valueModifier += additiveModifier.modifierValue;
        }
        value += valueModifier;
        valueModifier = 1;
        foreach (Modifier additiveMultiplicativeModifier in fullStat.additiveMultiplicativeModifiers)
        {
            valueModifier += additiveMultiplicativeModifier.modifierValue;
        }
        value *= valueModifier;
        valueModifier = 1;
        foreach (Modifier multiplicativeMultiplier in fullStat.multiplicativeModifiers)
        {
            valueModifier *= multiplicativeMultiplier.modifierValue;
        }
        value *= valueModifier;
        return value;
    }

    //Struct for storing a coroutine and its modifier, so it can be stopped if needed
    struct ModifierRoutine
    {
        public Coroutine coroutine;
        public MonoBehaviour routineTarget;
        public Modifier modifier;

        public ModifierRoutine(Coroutine routine, MonoBehaviour target, Modifier mod)
        {
            coroutine = routine;
            routineTarget = target;
            modifier = mod;
        }
    }

    static List<ModifierRoutine> modifierRoutines = new List<ModifierRoutine>();
    //Checks the list of ModifireRoutines to see if a coroutine with the given modifier is already running on the given MonoBehaviour, returns the coroutine if so
    static void CheckModifierRoutine(MonoBehaviour target, Modifier modifier)
    {
        foreach(ModifierRoutine modifierRoutine in modifierRoutines)
        {
            if(modifierRoutine.modifier.modifierSource == modifier.modifierSource && modifierRoutine.routineTarget == target)
            {
                target.StopCoroutine(modifierRoutine.coroutine);
                modifierRoutines.Remove(modifierRoutine);
                return;
            }
        }
    }

    //Adds the given modifier to the given list, waits the given amount of time, then removes the modifier
    static IEnumerator AddModifierTemporary(List<Modifier> modifierList, Modifier temporaryModifier, float modifierTime)
    {
        AddModifier(modifierList, temporaryModifier);
        yield return new WaitForSeconds(modifierTime);
        RemoveModifier(modifierList, temporaryModifier);
    }

    //Checks if a coroutine with the given modifier is already running on the given MonoBehaviour, stops it if that is the case, the starts a new temporary modifier coroutine with the given target, list, and modifier
    public static void StartAddModifierTemporary(MonoBehaviour target, List<Modifier> modifierList, Modifier temporaryModifier, float modifierTime)
    {
        CheckModifierRoutine(target, temporaryModifier);
        modifierRoutines.Add(new ModifierRoutine(target.StartCoroutine(AddModifierTemporary(modifierList, temporaryModifier, modifierTime)), target, temporaryModifier));
    }
}
