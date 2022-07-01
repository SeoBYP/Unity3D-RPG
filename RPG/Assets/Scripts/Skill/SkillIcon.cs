using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIcon : BaseUI
{
    private Image m_image;
    private int m_skilltableId;
    private float m_SkillCoolTime;
    private string m_skillname;

    public bool IsHaveSkill;
    public static bool draging = false;
    public static bool dropImage = false;
    public static Sprite _EmptyIcon;
    
    public string Skillname { get { return m_skillname; } set { m_skillname = value; } }
    public int SkillId { get { return m_skilltableId; } set { m_skilltableId = value; } }
    public float SkillCoolTime { get { return m_SkillCoolTime; } set { m_SkillCoolTime = value; } }
    public Sprite Icon { get { return m_image.sprite; } set { m_image.sprite = value; } }

    public override void Init()
    {
        
    }

    public static void LoadEmptyIcon()
    {
        _EmptyIcon = Resources.Load<Sprite>("Icons/Mini_frame0");
    }

    public bool IsEmpty()
    {
        if (Icon == _EmptyIcon)
        {
            return true;
        }
        return false;
    }

    public void SetTableIdRoSprite(int skillid)
    {
        if(PlayerSkill.SkillIconDic.ContainsKey(skillid))
            m_image.sprite = PlayerSkill.SkillIconDic[skillid];

    }

    public void SetInfo(int skilltableid)
    {
        m_skilltableId = skilltableid;
        if (PlayerSkill.PlayerSkillStatDic.ContainsKey(skilltableid))
        {
            m_SkillCoolTime = PlayerSkill.PlayerSkillStatDic[skilltableid].CoolTime;
            m_skillname = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillName;
        }
        SetTableIdRoSprite(skilltableid);
    }

    public void SetImage(Image image)
    {
        m_image = image;
    }

    public void SetEmptyIcon()
    {
        m_skilltableId = 0;
        if(m_skilltableId == 0)
            m_image.sprite = _EmptyIcon;
    }
}
