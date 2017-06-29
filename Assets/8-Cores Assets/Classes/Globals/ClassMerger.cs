using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public struct ClassMerger
{
    /// <summary>
    /// Function used to merge different class types with same properties.
    /// </summary>
    /// <param name="copyFrom"></param>
    /// <param name="copyTo"></param>
    public static void MergeClassProperties(object copyFrom, object copyTo)
    {
        BindingFlags flags;

        Dictionary<string, FieldInfo> targetDic;

        //Check if copyFrom or copyTo are null.
        if ((copyFrom == null) || (copyTo == null))
        {
            return;

        }

        //Assign binding flags for property search.
        flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        targetDic = copyTo.GetType().GetFields(flags).ToDictionary(f => f.Name);

        //Cicle trough each property of copyFrom, check if copyTo contains property then copy.
        foreach (FieldInfo field in copyFrom.GetType().GetFields(flags))
        {

            if (targetDic.ContainsKey(field.Name))
                targetDic[field.Name].SetValue(copyTo, field.GetValue(copyFrom));

            else
                Debug.LogWarning(string.Format("The field '{0}' has no corresponding field in the type '{1}'. Skipping field.", field.Name, copyTo.GetType().FullName));

        }
    }
}
