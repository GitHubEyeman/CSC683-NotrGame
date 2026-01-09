using System;
using UnityEngine;
using UnityEngine.UI;

public class RebindKeys : MonoBehaviour
{
    public enum ActionKey { Jump, Left, Right, Shot }

    [Header("UI (optional)")]
    public Text jumpKeyText;
    public Text leftKeyText;
    public Text rightKeyText;
    public Text shotKeyText;

    // Defaults
    private readonly KeyCode defaultJump = KeyCode.Space;
    private readonly KeyCode defaultLeft = KeyCode.A;
    private readonly KeyCode defaultRight = KeyCode.D;
    private readonly KeyCode defaultShot = KeyCode.LeftControl;

    // Runtime bindings
    private static readonly string PrefPrefix = "Rebind_";
    private ActionKey? waitingFor = null;

    private void Start()
    {
        // Ensure UI shows current bindings
        RefreshAllUI();
    }

    private void Update()
    {
        if (waitingFor == null) return;

        // Listen for any key / mouse button down
        if (Input.anyKeyDown)
        {
            // Detect which KeyCode was pressed
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    // Escape cancels
                    if (kc == KeyCode.Escape)
                    {
                        CancelRebind();
                        return;
                    }

                    SetBinding(waitingFor.Value, kc);
                    waitingFor = null;
                    RefreshAllUI();
                    return;
                }
            }
        }
    }

    // Public methods to wire to UI Buttons
    public void StartRebindJump() => StartRebind(ActionKey.Jump);
    public void StartRebindLeft() => StartRebind(ActionKey.Left);
    public void StartRebindRight() => StartRebind(ActionKey.Right);
    public void StartRebindShot() => StartRebind(ActionKey.Shot);

    public void CancelRebind()
    {
        waitingFor = null;
        RefreshAllUI();
    }

    public void ResetToDefaults()
    {
        SaveBinding(ActionKey.Jump, defaultJump);
        SaveBinding(ActionKey.Left, defaultLeft);
        SaveBinding(ActionKey.Right, defaultRight);
        SaveBinding(ActionKey.Shot, defaultShot);
        RefreshAllUI();
    }

    // Query API for gameplay code
    public static KeyCode GetKey(ActionKey action)
    {
        string key = PrefPrefix + action.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            return (KeyCode)PlayerPrefs.GetInt(key);
        }

        // No PlayerPref => return hardcoded defaults
        switch (action)
        {
            case ActionKey.Jump: return KeyCode.Space;
            case ActionKey.Left: return KeyCode.A;
            case ActionKey.Right: return KeyCode.D;
            case ActionKey.Shot: return KeyCode.LeftControl;
            default: return KeyCode.None;
        }
    }

    // Internal helpers
    private void StartRebind(ActionKey action)
    {
        waitingFor = action;
        SetWaitingText(action, "Press any key... (Esc to cancel)");
    }

    private void SetBinding(ActionKey action, KeyCode key)
    {
        SaveBinding(action, key);
    }

    private void SaveBinding(ActionKey action, KeyCode key)
    {
        PlayerPrefs.SetInt(PrefPrefix + action.ToString(), (int)key);
        PlayerPrefs.Save();
    }

    private void RefreshAllUI()
    {
        UpdateUIText(ActionKey.Jump, jumpKeyText);
        UpdateUIText(ActionKey.Left, leftKeyText);
        UpdateUIText(ActionKey.Right, rightKeyText);
        UpdateUIText(ActionKey.Shot, shotKeyText);
    }

    private void UpdateUIText(ActionKey action, Text ui)
    {
        if (ui == null) return;

        if (waitingFor == action)
        {
            ui.text = "Press any key...";
            return;
        }

        KeyCode kc = GetKey(action);
        ui.text = kc.ToString();
    }

    private void SetWaitingText(ActionKey action, string text)
    {
        switch (action)
        {
            case ActionKey.Jump: if (jumpKeyText) jumpKeyText.text = text; break;
            case ActionKey.Left: if (leftKeyText) leftKeyText.text = text; break;
            case ActionKey.Right: if (rightKeyText) rightKeyText.text = text; break;
            case ActionKey.Shot: if (shotKeyText) shotKeyText.text = text; break;
        }
    }
}
