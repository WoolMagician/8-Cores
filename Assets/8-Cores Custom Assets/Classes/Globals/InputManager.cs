using UnityEngine;

[System.Serializable]
public class InputManager : MonoBehaviour
{
    [Header("General Settings", order = 0)]
    public bool UseJoypad = true;
    public bool joypadTriggersAreButtons = false;

    [Header("Joypad Settings", order = 0)]
    [Header("Buttons", order = 1)]
    public string startButton = "";
    public string selectButton = "";

    [Tooltip("Roll right")]
    public string r1 = "";

    [Tooltip("Roll left")]
    public string l1 = "";

    public string r3 = "";
    public string l3 = "";

    [Tooltip("Jump Button")]
    public string buttonA = "";

    [Tooltip("Cancel Button")]
    public string buttonB = "";

    [Tooltip("Light Attack Button")]
    public string buttonX = "";

    [Tooltip("Heavy Attack Button")]
    public string buttonY = "";

    public string dPadButtonUp = "";
    public string dPadButtonDown = "";
    public string dPadButtonRight = "";
    public string dPadButtonLeft = "";

    [Header("Axis", order = 1)]
    [Tooltip("Camera Horizontal Movement")]
    public string rightPadX = "";

    [Tooltip("Camera Vertical Movement")]
    public string rightPadY = "";

    [Tooltip("Player Right/Left Movement")]
    public string leftPadX = "";

    [Tooltip("Player Forward/Backward Movement")]
    public string leftPadY = "";

    public string r2 = "";
    public string l2 = "";
    public string dPadAxisUp = "";
    public string dPadAxisDown = "";
    public string dPadAxisRight = "";
    public string dPadAxisLeft = "";

    [Header("Keyboard & Mouse Settings", order = 0)]
    [Header("Keyboard", order = 1)]
    public string jumpButton = "";
    public string movementX = "";
    public string movementY = "";

    [Header("Mouse", order = 1)]
    public string cameraX = "";
    public string cameraY = "";

    public bool GetAxisAsButton(string axisName)
    {
        float axisValue = 0f;

        axisValue = Input.GetAxisRaw(axisName);

        if (axisValue >= 1)
        {
            return false;
        }

        if (axisValue < 1)
        {
            return true;
        }

        return false;
    }
}
