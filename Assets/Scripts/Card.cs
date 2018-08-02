using UnityEngine;

public class Card : MonoBehaviour
{
    //[Header("Sprite Attribute")]
    //public Sprite[] m_SpriteAttribute; // 4 chat 
    //[Header("Sprite Value")]
    //public Sprite[] m_SpriteValue;  //13 value
    [Header("BackGround")]
    public Sprite[] m_Background;
    [Header("Sprite Cards")]
    public Sprite[] m_SpriteCard;

    public BoxCollider2D m_Box;

    public SpriteRenderer m_Attribute;
    public SpriteRenderer m_ValueSprite;
    public SpriteRenderer m_SmallAttribute;
    [Header("Hint sprite")]
    public GameObject m_Border;
    public bool isDrag = true;
    private Transform m_Transform;
    private int Offset = 10;
    private Vector3 Position;
    private SpriteRenderer Background;
    [Header("CardBack")]
    public Sprite[] CardBacks;
    [Header("CardFace")]
    public Sprite[] CardFaces;
    public int IndexCardFace = 0;
    public int m_IndexCardFace
    {
        get
        {
            return IndexCardFace;
        }

        set
        {
            IndexCardFace = value;
            if (IsFlip)
                ChangeSprite();
        }
    }

    public bool IsShow = false;

    private int IndexCardBack = 0;
    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        Background = GetComponent<SpriteRenderer>();
        m_Box = GetComponent<BoxCollider2D>();
        //m_Box.enabled = false;
    }

    void Start()
    {
        Position = Compute.GetInstance().GetPositionOrigin();
        // Debug.Log(Position);
    }
    [SerializeField]
    private int m_Value;
    public int Value
    {
        get { return m_Value; }
        set
        {
            // Debug.Log("xxxx");
            m_Value = value;
            ChangeSprite();
        }
    }
    [SerializeField]
    private int m_Index;
    [SerializeField]
    private int m_Collum;
    public int Index
    {
        get
        {
            return m_Index;
        }
        set
        {
            m_Index = value;
            ChangePosition();
        }
    }

    public int Collum
    {
        get
        {
            return m_Collum;
        }
        set
        {
            m_Collum = value;
            //SetPositionCollum();
        }
    }

    public int IndexArray;

    [SerializeField]
    private bool IsFlip = false;

    public bool isFlip
    {
        get
        {
            return IsFlip;
        }
        set
        {
            IsFlip = value;
            ChangeBackGround();
        }
    }

    public bool IsInteractable = true;

    void SetPositionCollum()
    {
        if (m_Collum < GameData.TOTAL_COLLUM)
            Position = Compute.GetInstance().GetPositionCollum(m_Collum);
        else
            Position = Compute.GetInstance().GetPositionOrigin();
    }

    private void ChangeSprite()
    {
        if (m_Value <= 39)
          Background.sprite = m_SpriteCard[m_Value];
        else
        {
          Background.sprite = SceneManager.instance.CardFaceController.GetCurrentCardFace()[12 - (52 - m_Value)];
        }
        IsShow = true;
    }

    void OnMouseDown()
    {
        //Debug.Log(IndexArray);
        if (IsFlip)
        {
            //if (Collum >= GameData.TOTAL_COLLUM)
            //    Compute.Dept = transform.position.z;
            Compute.GetInstance().HandleOnTouch(Collum, Index, Value, IndexArray, transform.position);
        }
    }

    void OnMouseDrag()
    {
        if (IsFlip && isDrag)
        {
            Compute.GetInstance().HandleOnDrag(Camera.main.ScreenToWorldPoint(Input.mousePosition), Collum, Index);
        }
    }

    void OnMouseUp()
    {
        if (IsFlip)
        {
            Compute.GetInstance().HandleEndDrag(Collum, Index, Value, IndexArray, transform.position);
        }
    }

    public void OnThisCardClick()
    {
        OnMouseDown();
        OnMouseDrag();
        if (IsFlip)
        {
            Compute.GetInstance().HandleEndDrag(Collum, Index, Value, IndexArray, transform.position, false);
        }
    }

    void ChangePosition()
    {

        if (m_Collum < GameData.NUMBER_COLLUM)
        {
            Vector3 temp = transform.position;
            temp.z = -m_Index;
            transform.position = temp;
        }
        //else
        //{
        //    //Debug.Log(m_Collum);
        //    //Vector3 temp = transform.position;
        //    //temp.z = -m_Index;
        //    //m_Transform.position = temp;
        //}





    }


    public void ChangeBackGround()
    {
        if (!IsFlip)
        {
            Background.sprite = SceneManager.instance.CardPackController.CardBack[IndexCardBack];

        }
        else
        {
            ChangeSprite();

        }
    }

    public void TurnInteractable(bool value)
    {
        // Debug.Log("TurnInteractable " + value);
        m_Box.enabled = value;
    }

    public void ResetCard()
    {
        IsFlip = false;
        Background.sprite = SceneManager.instance.CardPackController.CardBack[IndexCardBack];
        Collum = GameData.NUMBER_COLLUM + 1;
        Index = -1;
        m_Border.SetActive(false);
        m_Box.enabled = true;
        transform.localScale = new Vector3(GameData.CARD_SCALE, GameData.CARD_SCALE / 9 * 10, 1);
        ShowEnableClick();
    }

    public void ShowHint()
    {
        m_Border.SetActive(true);
    }

    public void HideHint()
    {
        m_Border.SetActive(false);
    }

    public void Default()
    {
        IsFlip = false;
        Background.sprite = CardBacks[IndexCardBack];
        Background.color = new Color32(255, 255, 255, 255);
        IsShow = false;
    }

    public void Open()
    {
        IsFlip = true;
        Background.sprite = m_SpriteCard[m_Value];
        Background.color = new Color32(255, 255, 255, 255);
        IsShow = true;

    }

    public void SetCardBack(int index)
    {
        IndexCardBack = index;
    }

    public void ShowEnableClick(bool isShow = true)
    {
        IsShow = isShow;
        if (isShow)
        {
            Background.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            Background.color = new Color32(175, 175, 175, 255);
        }
    }

    public void Setposition()
    {
        if (m_Collum < GameData.TOTAL_COLLUM)
        {
            Vector3 temp = transform.position;
            temp.z = -m_Index;
            transform.position = temp;
        }
    }

}
