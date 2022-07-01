using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private const string mouseXstring = "Mouse X";
    private const string mouseYstring = "Mouse Y";
    private const string mouseScrollstring = "Mouse ScrollWheel";

    private const string XString = "Horizontal";
    private const string YString = "Vertical";
    private const string Space = "Jump";
    private const string Mouse0 = "Fire1";

    private const KeyCode roll = KeyCode.LeftShift;
    private const KeyCode Inven = KeyCode.I;
    private const KeyCode Status = KeyCode.P;
    private const KeyCode Skillpopup = KeyCode.K;
    private const KeyCode Community = KeyCode.F;
    private const KeyCode Skill1Key = KeyCode.Alpha1;
    private const KeyCode Skill2Key = KeyCode.Alpha2;
    private const KeyCode Skill3Key = KeyCode.Alpha3;
    private const KeyCode Skill4Key = KeyCode.Alpha4;
    private const KeyCode Skill5Key = KeyCode.Alpha5;
    private const KeyCode Item1Key = KeyCode.Alpha6;
    private const KeyCode Item2Key = KeyCode.Alpha7;
    private const KeyCode Item3Key = KeyCode.Alpha8;
    private const KeyCode Item4Key = KeyCode.Alpha9;
    private const KeyCode Item5Key = KeyCode.Alpha0;
    private const KeyCode MenuKey = KeyCode.Escape;
    private const KeyCode Quest = KeyCode.O;
    //private bool OpenInventory = UnityEngine.Input.GetKeyDown(Inven);


    public static float MouseX { get { return UnityEngine.Input.GetAxis(mouseXstring); } }
    public static float MouseY { get { return UnityEngine.Input.GetAxis(mouseYstring); } }
    public static float MouseScroll { get { return UnityEngine.Input.GetAxis(mouseScrollstring); } }

    public static float MoveX { get { return UnityEngine.Input.GetAxis(XString); } }
    public static float MoveY { get { return UnityEngine.Input.GetAxis(YString); } }
    public static bool Jump { get { return UnityEngine.Input.GetButtonDown(Space); } }
    public static bool Attack { get { return UnityEngine.Input.GetButtonDown(Mouse0); } }

    public static bool Menu { get { return UnityEngine.Input.GetKeyDown(MenuKey); } }
    public static bool Inventory { get { return UnityEngine.Input.GetKeyDown(Inven); } }
    public static bool Roll { get { return UnityEngine.Input.GetKeyDown(roll); } }
    public static bool PlayerState { get { return UnityEngine.Input.GetKeyDown(Status); } }
    public static bool SkillPopup { get { return UnityEngine.Input.GetKeyDown(Skillpopup); } }
    public static bool CommunityActive { get { return UnityEngine.Input.GetKeyDown(Community); } }
    public static bool QuestPopup { get { return UnityEngine.Input.GetKeyDown(Quest); } }

    public static bool SKill1 { get { return UnityEngine.Input.GetKeyDown(Skill1Key); } }
    public static bool SKill2 { get { return UnityEngine.Input.GetKeyDown(Skill2Key); } }
    public static bool SKill3 { get { return UnityEngine.Input.GetKeyDown(Skill3Key); } }
    public static bool SKill4 { get { return UnityEngine.Input.GetKeyDown(Skill4Key); } }
    public static bool SKill5 { get { return UnityEngine.Input.GetKeyDown(Skill5Key); } }

    public static bool Item1 { get { return UnityEngine.Input.GetKeyDown(Item1Key); } }
    public static bool Item2 { get { return UnityEngine.Input.GetKeyDown(Item2Key); } }
    public static bool Item3 { get { return UnityEngine.Input.GetKeyDown(Item3Key); } }
    public static bool Item4 { get { return UnityEngine.Input.GetKeyDown(Item4Key); } }
    public static bool Item5 { get { return UnityEngine.Input.GetKeyDown(Item5Key); } }
}
