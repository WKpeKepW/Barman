// Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class dialogs
{

    private dialogsDialog[] dialogField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("dialog")]
    public dialogsDialog[] dialog
    {
        get
        {
            return this.dialogField;
        }
        set
        {
            this.dialogField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class dialogsDialog
{

    private string textNPCField;

    private dialogsDialogChoise[] choiseField;

    private byte idField;

    private string nameField;

    /// <remarks/>
    public string textNPC
    {
        get
        {
            return this.textNPCField;
        }
        set
        {
            this.textNPCField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("choise")]
    public dialogsDialogChoise[] choise
    {
        get
        {
            return this.choiseField;
        }
        set
        {
            this.choiseField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class dialogsDialogChoise
{

    private byte gotoIDField;

    private bool gotoIDFieldSpecified;

    private bool theEndField;

    private bool theEndFieldSpecified;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte gotoID
    {
        get
        {
            return this.gotoIDField;
        }
        set
        {
            this.gotoIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool gotoIDSpecified
    {
        get
        {
            return this.gotoIDFieldSpecified;
        }
        set
        {
            this.gotoIDFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool theEnd
    {
        get
        {
            return this.theEndField;
        }
        set
        {
            this.theEndField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool theEndSpecified
    {
        get
        {
            return this.theEndFieldSpecified;
        }
        set
        {
            this.theEndFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}


