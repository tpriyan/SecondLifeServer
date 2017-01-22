using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;


public partial class TextureChangerAPI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if action exists in the request
        List<KeyValuePair<string, string>> listOfValues = this.GetPostDataKeyValuePair(this.getPostData(Request));

        if (this.existsInList("operation", listOfValues))
        {
            switch(this.getValue("operation", listOfValues))
            {
                case "getunitslist":
                    this.GetUnitsList(listOfValues);
                    break;

                case "addchildunit":
                    this.addChildUnit(listOfValues);
                    break;

                case "removeallchildunits":
                    this.RemoveAllChildUnits(listOfValues);
                    break;

                case "getLinkedRentalUnit":
                    this.GetLinkedRentalUnit(listOfValues);
                    break;

                case "createorupdateunits":
                    this.CreateOrUpdateUnits(listOfValues);
                    break;

                case "listlinkedunits":
                    this.ListAllLinkedUnits(listOfValues);
                    break;
            }
        }
        else
        {
            Response.Write("NOOPSPECIFIED");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private void ListAllLinkedUnits(List<KeyValuePair<string, string>> _values)
    {
        if (this.existsInList("objectguid", _values))
        {
            try
            {
                Response.Write(TextureChanger.Logic.CRUDOperations.ListLinkedUnits(this.getValue("objectguid", _values)));
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }
        }
        else
        {
            Response.Write("MISSINGOBJECTGUID");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private void CreateOrUpdateUnits(List<KeyValuePair<string, string>> _values)
    {
        if (this.existsInList("slurl", _values) &&
            this.existsInList("objectguid", _values) &&
            this.existsInList("type", _values) &&
            this.existsInList("owner", _values) &&
            this.existsInList("isinitialcall", _values) &&
            this.existsInList("linkedrentalunitid", _values) &&
            this.existsInList("version", _values))
        {
            try
            {
                TextureChanger.Logic.CRUDOperations.createOrUpdateURL(this.getValue("objectguid", _values),
                                                                      this.getValue("slurl", _values),
                                                                      this.getValue("owner", _values),
                                                                      this.getValue("type", _values),
                                                                      this.getValue("name", _values),
                                                                      this.getValue("linkedrentalunitid", _values),
                                                                      this.getValue("isinitialcall", _values),
                                                                      this.getValue("version", _values));
                                        
                Response.Write("OK");
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }
        }
        else
        {
            Response.Write("INPUTDATAERROR");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private void GetLinkedRentalUnit(List<KeyValuePair<string, string>> _values)
    {
        if (this.existsInList("objectguid", _values))
        {
            try
            {
                Response.Write(TextureChanger.Logic.CRUDOperations.getLinkedRentalUnitGUID(this.getValue("objectguid", _values)));
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }
        }
        else
        {
            Response.Write("MISSINGOBJECTGUID");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private void RemoveAllChildUnits(List<KeyValuePair<string, string>> _values)
    {
        if (this.existsInList("objectguid", _values))
        {
            try
            {
                TextureChanger.Logic.CRUDOperations.addRemoveLinkedUnit(this.getValue("objectguid", _values),
                                                                        "",
                                                                        "",
                                                                        "",
                                                                        "",
                                                                        "true");
                Response.Write("ALLCHILDUNITSDELETED");
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }
        }
        else
        {
            Response.Write("MISSINGOBJECTGUID");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private void addChildUnit(List<KeyValuePair<string, string>> _values)
    {
        if(this.existsInList("objectguid", _values) &&
           this.existsInList("childobject", _values) &&
           this.existsInList("defaulttheme", _values) &&
           this.existsInList("name", _values) &&
           this.existsInList("type", _values))

        {
            try
            {
                TextureChanger.Logic.CRUDOperations.addRemoveLinkedUnit(this.getValue("objectguid", _values),
                                                                        this.getValue("childobject", _values),
                                                                        this.getValue("defaulttheme", _values),
                                                                        this.getValue("name", _values),
                                                                        this.getValue("type", _values),
                                                                        "false");
                Response.Write("CHILDUNITADDED");
                Response.Flush();
                Response.SuppressContent = true;

            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }
        }
        else
        {
            Response.Write("INPUTDATAERROR");
            Response.Flush();
            Response.SuppressContent = true;
        }
       
    }

    private void GetUnitsList(List<KeyValuePair<string, string>> _values)
    {
        string unitsList = string.Empty;
        if(this.existsInList("ownerguid", _values))
        {
            try
            {
                unitsList = TextureChanger.Logic.CRUDOperations.getUnitsList(this.getValue("ownerguid", _values));
                Response.Write(unitsList);
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("UNKNOWNEXCEPTION");
                Response.Flush();
                Response.SuppressContent = true;
            }

        }
        else
        {
            Response.Write("MISSINGOWNERGUID");
            Response.Flush();
            Response.SuppressContent = true;
        }
    }

    private List<KeyValuePair<string, string>> GetPostDataKeyValuePair(string _value)
    {
        string[] keyValueString = _value.Split('&');
        var listOfRequestData = new List<KeyValuePair<string, string>>();

        foreach (string s in keyValueString)
        {
            string[] tmpData = s.Split('=');
            listOfRequestData.Add(new KeyValuePair<string, string>(tmpData[0], tmpData[1]));
        }

        return listOfRequestData;
    }

    private Boolean existsInList(string _key, List<KeyValuePair<string, string>> _listOfRequestData)
    {
        Boolean retValue = false;

        foreach (KeyValuePair<string, string> element in _listOfRequestData)
        {
            if (element.Key == _key)
            {
                retValue = true;
                break;
            }
        }

        return retValue;
    }

    private string getValue(string _key, List<KeyValuePair<string, string>> _listOfRequestData)
    {
        string retValue = "";

        foreach (KeyValuePair<string, string> element in _listOfRequestData)
        {
            if (element.Key == _key)
            {
                retValue = element.Value;
                break;
            }
        }

        return retValue;
    }

    private string getPostData(HttpRequest _pageRequest)
    {
        var sr = new System.IO.StreamReader(Request.InputStream);
        string content = sr.ReadToEnd();

        return content;
    }
}