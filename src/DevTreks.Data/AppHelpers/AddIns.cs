namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for 'addins' 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        Clubs use this class to hold a collection of addins. 
    public class AddIns
    {
        public AddIns()
        {
        }
        //no being used anaywhere
        //public const string ADDIN_NAMESPACE_NODE_QRY = "urn:DevTreks-support-schemas:AddIn";
        //public const string ADDIN = "AddIn";

        public enum ADDIN_TYPES
        {
            //account to accounttoaddin view 
            //the addins belonging to a specific club
            addinaccountgroup   = 1,
            addin               = 2
        }

        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == ADDIN_TYPES.addinaccountgroup.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (uri.URIId == 0)
                {
                    //no link backwards (showing groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                    uri.URIDataManager.ChildrenNodeName = ADDIN_TYPES.addinaccountgroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = ADDIN_TYPES.addin.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == ADDIN_TYPES.addin.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards because it still will show group-item in toc
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (currentNodeName == ADDIN_TYPES.addin.ToString())
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = ADDIN_TYPES.addin.ToString();
                }

            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
             out string parentForeignKeyName, out string baseForeignKeyName)
        {
            parentForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName
                == ADDIN_TYPES.addinaccountgroup.ToString())
            {
                parentForeignKeyName = "AccountId";
                baseForeignKeyName = LinkedViews.LINKEDVIEWBASEID;
            }
            //else can insert members into base table during initial registration
        }
    }
}


