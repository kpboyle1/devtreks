using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		Xml node insertion handling class
    ///Author:		www.devtreks.org
    ///Date:		2013, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class AddHelperLinq
    {
        public AddHelperLinq() { }
        public enum SELECTION_OPTIONS
        {
            none                = 0,
            //html element names
            rdobuildselects     = 1,
            rdoselects          = 2,
            //options
            allancestors        = 3,
            selectedancestor    = 4,
            //works with select_existing_params
            selectedparent      = 5,
            //subaction view
            buildnewdocview     = 6,
            //build new doc selections pending
            selectionsmade      = 7
        }
        //where do selections go?
        public enum SELECT_EXISTING_PARAMS
        {
            selectsnodeuripattern       = 0,
            selectsuripattern           = 1,
            selectionsnodeneededname    = 2,
            selectsattributename        = 3,
            selectscalcparams           = 4,
            parentnode                  = 5,
            defaultnode                 = 6,
            adddefault_                 = 7
        }
        //step 1. organize the arguments that will be used throughout this class
        public static EditHelper.ArgumentsEdits MakeArgumentAdds(ContentURI uri,
            SELECTION_OPTIONS selectionOption, string selectedAncestorURIPattern, 
            string numberToAdd)
        {
            EditHelper.ArgumentsEdits oArgumentsAdds = new EditHelper.ArgumentsEdits();
            oArgumentsAdds.SelectionOption = selectionOption;
            oArgumentsAdds.NumberToAdd = numberToAdd;
            oArgumentsAdds.NeedsBaseIds = false;
            //set up default props for uris being edited
            //convention: URIToAdd is each node being inserted or deleted
            //URIToEdit is base node (node getting insertions or node holding document)
            oArgumentsAdds.URIToEdit = new ContentURI(uri);
            if (selectionOption == SELECTION_OPTIONS.selectedparent
                || selectionOption == SELECTION_OPTIONS.selectedancestor)
            {
                //selections being added to parenturipattern in selectedAncestorURIPattern
                if (!string.IsNullOrEmpty(uri.URIDataManager.SelectionsNodeURIPattern))
                {
                    //switch oArgumentsAdds.URIToEdit to the specific node being edited
                    ContentURI.ChangeURIPattern(oArgumentsAdds.URIToEdit,
                        oArgumentsAdds.URIToEdit.URIDataManager.SelectionsNodeURIPattern);
                    //but keep the apptype of uri (an input being added to a component
                    //in an investment, needs an investment schema)
                    oArgumentsAdds.URIToEdit.URIDataManager.AppType
                        = uri.URIDataManager.AppType;
                    oArgumentsAdds.URIToEdit.URIDataManager.SubAppType
                        = uri.URIDataManager.SubAppType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(selectedAncestorURIPattern))
                    {
                        //selectedAncestorURIPattern = childuripattern;parenturipattern (cuts db hits)
                        string sChildURIPattern = string.Empty;
                        string sParentURIPattern = string.Empty;
                        GetChildParentURIPatterns(selectedAncestorURIPattern,
                            out sChildURIPattern, out sParentURIPattern);
                        //selections being made to a doc that uses only one ancestor
                        if (!string.IsNullOrEmpty(sParentURIPattern))
                        {
                            //this is the ancestor that will be shared by all selections
                            ContentURI.ChangeURIPattern(oArgumentsAdds.URIToEdit,
                                sParentURIPattern);
                        }
                        else
                        {
                            //use the original selectedAncestorURIPattern 
                            ContentURI.ChangeURIPattern(oArgumentsAdds.URIToEdit,
                                selectedAncestorURIPattern);
                        }
                    }
                }
                oArgumentsAdds.IsDbEdit = (selectionOption == SELECTION_OPTIONS.selectedparent)
                    ? Helpers.GeneralHelpers.IsDbEdit(uri) : false;
            }
            else if (selectionOption == SELECTION_OPTIONS.allancestors)
            {
                //each individual selection in uri.URIDataManager.SelectsList 
                //determines where selection will be appended
                //oArgumentsAdds.URIToEdit.URIPattern is set from first selection's parent
            }
            return oArgumentsAdds;
        }
        //step 2. prepare a list of the selections to be inserted
        public static void SetSelectionsToAdd(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsAdds)
        {
            //return a contenturi list of the selections to add
            //this list has to stay in this order, or 
            //ancestors won't be processed when needed
            argumentsAdds.SelectionsToAdd = new List<ContentURI>();
            if (!string.IsNullOrEmpty(
                argumentsAdds.URIToEdit.URIDataManager.SelectedList))
            {
                string[] arrSelections 
                    = argumentsAdds.URIToEdit.URIDataManager.SelectedList.Split(
                    Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
                int i = 0;
                if (arrSelections != null)
                {
                    int iLength = arrSelections.Length;
                    string sSelection = string.Empty;
                    string sChildURIPattern = string.Empty;
                    string sParentURIPattern = string.Empty;
                    for (i = 0; i < iLength; i++)
                    {
                        sSelection = arrSelections[i];
                        if (!string.IsNullOrEmpty(sSelection))
                        {
                            //selection = childuripattern;parenturipattern (cuts db hits)
                            GetChildParentURIPatterns(sSelection,
                                out sChildURIPattern, out sParentURIPattern);
                            //build a new contenturi
                            Network selectionNetwork
                                = Helpers.LinqHelpers.GetSameNetworkFromList(
                                sChildURIPattern, argumentsAdds.SelectionsToAdd);
                            ContentURI newSelection = new ContentURI(sChildURIPattern,
                                selectionNetwork);
                            if (argumentsAdds.SelectionOption 
                                == SELECTION_OPTIONS.selectedparent
                                || argumentsAdds.SelectionOption 
                                == SELECTION_OPTIONS.selectedancestor)
                            {
                                newSelection.URIDataManager.ParentURIPattern
                                    = argumentsAdds.URIToEdit.URIPattern;
                            }
                            else if (argumentsAdds.SelectionOption 
                                == SELECTION_OPTIONS.allancestors)
                            {
                                //each individual selection in uri.URIDataManager.SelectsList 
                                //determines where selection will be appended
                                newSelection.URIDataManager.ParentURIPattern
                                    = sParentURIPattern;
                            }
                            //add it to the list
                            argumentsAdds.SelectionsToAdd.Add(newSelection);
                            if (i == 0)
                            {
                                //adjust uritoedit if group nodes being inserted to service node
                                UpdateURIToEditAppTypes(
                                    argumentsAdds, newSelection);
                            }
                        }
                    }
                }
                else
                {
                    uri.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                            "ADDS_NOSELECTIONS");
                }
                //v 1.3.1: this duplicates XmlLinq.AddElementToParentUsingURI, when AddMoreDefaultElements is used
                //but the models use this rather than AddMoreDefaults
                int iNumberToAdd = Helpers.GeneralHelpers.ConvertStringToInt(argumentsAdds.NumberToAdd);
                if (argumentsAdds.SelectionsToAdd.Count == 1
                    && iNumberToAdd > 0)
                {
                    for (i = 1; i < iNumberToAdd; i++)
                    {
                        argumentsAdds.SelectionsToAdd.Add(argumentsAdds.SelectionsToAdd.FirstOrDefault());
                    }
                }
            }
        }
        private static void UpdateURIToEditAppTypes(
            EditHelper.ArgumentsEdits addsArguments,
            ContentURI newSelection)
        {
            if (addsArguments.URIToEdit.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                if (newSelection.URIDataManager.AppType
                    != Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
                {
                    //change uritoedit to actual app
                    addsArguments.URIToEdit.URIDataManager.AppType
                        = newSelection.URIDataManager.AppType;
                    addsArguments.URIToEdit.URIDataManager.SubAppType
                        = newSelection.URIDataManager.SubAppType;
                }
            }
        }
        //step 3. add the ancestors needed by each selection
        public async Task<bool> AddAncestorsToSelectionsAsync(ContentURI uri,
            EditHelper.ArgumentsEdits addsArguments,
            XElement root)
        {
            bool bHasAncestors = false;
            //fill in uri.URIDataManager.Ancestors for each member of argumentsAdds.SelectionsToAdd
            //remember that addsArguments.URIToEdit and addsArguments.URIToAdd can
            //use different db connections
            if (addsArguments.SelectionOption 
                == SELECTION_OPTIONS.selectedancestor)
            {
                //addsArguments.URIToEdit is the ancestor for all sections
                await SetAncestorsForSelectedAncestorAsync(addsArguments);
                foreach (ContentURI selectionURI in addsArguments.SelectionsToAdd)
                {
                    if (addsArguments.URIToEdit.URIPattern
                        != selectionURI.URIPattern)
                    {
                        selectionURI.URIDataManager.ParentURIPattern
                            = addsArguments.URIToEdit.URIPattern;
                    }
                }
            }
            else if (addsArguments.SelectionOption
                == SELECTION_OPTIONS.allancestors)
            {
                foreach (ContentURI selectionURI in addsArguments.SelectionsToAdd)
                {
                    await SetAncestorsForAllAncestorsAsync(addsArguments, 
                        selectionURI);
                }
            }
            else if (addsArguments.SelectionOption
                == SELECTION_OPTIONS.selectedparent)
            {
                //no ancestors to deal with 
                //(they already exist in the doc being edited)
            }
            return bHasAncestors;
        }
        private async Task SetAncestorsForSelectedAncestorAsync(
            EditHelper.ArgumentsEdits addsArguments)
        {
            if (!Helpers.GeneralHelpers.IsRootChild(
                addsArguments.URIToEdit.URINodeName))
            {
                //get ancestors of addsArguments.URIToEdit
                SqlRepositories.ContentRepository rep 
                    = new SqlRepositories.ContentRepository(addsArguments.URIToEdit);
                addsArguments.URIToEdit.URIDataManager.Ancestors
                    = await rep.GetAncestorsAsync(addsArguments.URIToEdit);
                //addsArguments.URIToEdit.Ancestors gets copied to each selection
                //add addsArguments.URIToEdit as the immediate ancestor (needs a parenturipattern)
                addsArguments.URIToEdit.URIDataManager.Ancestors.Add(addsArguments.URIToEdit);
                //set the uri.URIDataManager.ParentURIPattern for each ancestor
                Helpers.LinqHelpers.SetParentURIPatternsForAncestors(
                    addsArguments.URIToEdit.URIDataManager.Ancestors);
              
            }
            else
            {
                //the only ancestor needed is uritoedit
                //uritoedit will be added to root element
                //and selections will be added to uritoedit
                //setting uritoedit's parent to string.empty causes it to be added to 
                //root element
                addsArguments.URIToEdit.URIDataManager.ParentURIPattern = string.Empty;
                addsArguments.URIToEdit.URIDataManager.Ancestors = new List<ContentURI>();
                addsArguments.URIToEdit.URIDataManager.Ancestors.Add(addsArguments.URIToEdit);
            }
        }
        private async Task SetAncestorsForAllAncestorsAsync(
            EditHelper.ArgumentsEdits addsArguments,
            ContentURI selectionURI)
        {
            string sParentNodeName = ContentURI.GetURIPatternPart(
                selectionURI.URIDataManager.ParentURIPattern,
                ContentURI.URIPATTERNPART.node);
            //sParentNode can be string.empty
            if (sParentNodeName != null) 
            {
                //see if the ancestors have already been added to another list member
                //don't needlessly process the same ancestors multiple times
                //(to reduce xml to linq queries and memory use)
                bool bListContainsAncestors = Helpers.LinqHelpers.ListContainsAncestors(
                    selectionURI, addsArguments.SelectionsToAdd);
                if (!bListContainsAncestors)
                {
                    //see if only ancestor needed is selectionURI.URIDataManager.ParentURIPattern
                    if ((!Helpers.GeneralHelpers.IsRootChild(sParentNodeName))
                        || selectionURI.URIDataManager.ParentURIPattern
                            == selectionURI.URIPattern)
                    {
                        //retrieve ancestors from db
                        SqlRepositories.ContentRepository rep
                            = new SqlRepositories.ContentRepository(selectionURI);
                        selectionURI.URIDataManager.Ancestors
                            = await rep.GetAncestorsAsync(selectionURI);
                        //set the uri.URIDataManager.ParentURIPattern for each ancestor
                        Helpers.LinqHelpers.SetParentURIPatternsForAncestors(
                            selectionURI.URIDataManager.Ancestors);
                        if (sParentNodeName == string.Empty
                            || selectionURI.URIDataManager.ParentURIPattern
                                != selectionURI.URIPattern)
                        {
                            Helpers.LinqHelpers.SetParentURIPatternFromAncestors(
                                selectionURI);
                        }
                    }
                    else
                    {
                        //add the group node to the ancestors list (no db hit needed)
                        //copy the properties of selectionURI
                        ContentURI parentURI
                            = new ContentURI(selectionURI);
                        //change the uripattern to parent
                        parentURI.ChangeURIPattern(selectionURI.URIDataManager.ParentURIPattern,
                            selectionURI.URINetwork);
                        //setting its parent to string.empty causes it to be added to 
                        //root element
                        parentURI.URIDataManager.ParentURIPattern = string.Empty;
                        if (selectionURI.URIDataManager.Ancestors == null)
                            selectionURI.URIDataManager.Ancestors = new List<ContentURI>();
                        selectionURI.URIDataManager.Ancestors.Add(parentURI);
                    }
                }
            }
        }
        //step 4. add the ancestors
        public void AddAncestorsToLinqDoc(ContentURI uri,
            EditHelper.ArgumentsEdits addsArguments,
            XElement root)
        {
            if (addsArguments.SelectionOption 
                == SELECTION_OPTIONS.allancestors)
            {
                //add the ancestors to the linq doc before adding the selections
                List<ContentURI> lstSelectionsWithAncestors
                    = Helpers.LinqHelpers.GetURIsWithAncestors(addsArguments.SelectionsToAdd);
                if (lstSelectionsWithAncestors != null)
                {
                    foreach (ContentURI selectedURI in lstSelectionsWithAncestors)
                    {
                        AddAncestorsToLinqDoc(uri, root, addsArguments,
                            selectedURI);
                        if (uri.ErrorMessage != string.Empty)
                            break;
                    }
                }
            }
            else if (addsArguments.SelectionOption
                == SELECTION_OPTIONS.selectedancestor)
            {
                //uritoedit holds ancestors
                AddAncestorsToLinqDoc(uri, root, addsArguments,
                    addsArguments.URIToEdit);
            }
        }
        private void AddAncestorsToLinqDoc(ContentURI uri,
            XElement root, EditHelper.ArgumentsEdits addsArguments,
            ContentURI selectedURI)
        {
            bool bIsAncestor = true;
            if (selectedURI.URIDataManager.Ancestors != null)
            {
                foreach (ContentURI ancestorURI
                    in selectedURI.URIDataManager.Ancestors)
                {
                    //don't hit db and run linq queries for ancestors 
                    //that are already in root (ok for selections but not ancestors)
                    //ancestors are uris and unique across apps
                    bool bAncestorExists = false;
                    XNamespace y0 = XmlLinq.GetNamespaceForNode(
                        root, ancestorURI.URINodeName);
                    if (y0 == null)
                    {
                        bAncestorExists = XmlLinq.DescendantExists(root,
                            ancestorURI);
                    }
                    else
                    {
                        bAncestorExists = XmlLinq.DescendantExistsWithNS(y0, root,
                            ancestorURI);
                    }
                    if (!bAncestorExists)
                    {
                        AddSelectionToLinqDoc(bIsAncestor, uri,
                            root, addsArguments, ancestorURI);
                    }
                    if (uri.ErrorMessage != string.Empty)
                        break;
                }
            }
        }
        //step 5. add the selections to linq element
        public bool AddNewSelectionsToLinqDoc(ContentURI uri,
            EditHelper.ArgumentsEdits addsArguments,
            XElement root)
        {
            bool bHasAddedSelections = false;
            bool bIsAncestor = false;
            //refactor: the Parallel generated errors when resource packs were added
            foreach (ContentURI selectedURI in addsArguments.SelectionsToAdd)
            {
                if (uri.ErrorMessage == string.Empty)
                {
                    if (addsArguments.SelectionOption
                        == SELECTION_OPTIONS.allancestors)
                    {
                        //insertions get their parent params from addsArguments.URIToEdit
                        //each selection needs to reset addsArguments.URIToEdit to its parent
                        ContentURI.ChangeURIPattern(addsArguments.URIToEdit,
                            selectedURI.URIDataManager.ParentURIPattern);
                    }
                    //add the selection
                    AddSelectionToLinqDoc(bIsAncestor, uri,
                        root, addsArguments, selectedURI);
                }
                if (uri.ErrorMessage != string.Empty)
                    break;
            }
            if (uri.ErrorMessage == string.Empty)
                bHasAddedSelections = true;
            return bHasAddedSelections;
        }
        
        private void AddSelectionToLinqDoc(bool isAncestor,
            ContentURI uri, XElement root, 
            EditHelper.ArgumentsEdits addsArguments,
            ContentURI selectedURI)
        {
            //program using addsArguments alone
            SetURIToAddForInsertion(addsArguments, 
                isAncestor, selectedURI);
            if (!addsArguments.IsCustomNodeBeingAdded)
            {
                //set the sqlxml parameters needed to retrieve the selectedURI's xml
                //SetSqlXmlParameters(isAncestor, addsArguments);
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                {
                    addsArguments.ErrorMessage = string.Empty;
                }
            }
            if (addsArguments.ErrorMessage == string.Empty)
            {
                XElement selectedElement = null;
                if (!addsArguments.IsCustomNodeBeingAdded)
                {
                    if (isAncestor && addsArguments.URIToAdd.URINodeName
                        == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        //sqlxml can't be used, instead uses a custom node
                        selectedElement = GetSelectionForServiceBase(addsArguments);
                    }
                    else
                    {
                        selectedElement = GetSelection(addsArguments);
                    }
                }
                else
                {
                    //sqlxml can't be used, instead uses a custom node
                    selectedElement = GetSelectionForCustomNode(addsArguments, root);
                }
                if (selectedElement != null)
                {
                    bool bIsReadyToAdd = false;
                    if (!isAncestor)
                    {
                        if (!addsArguments.IsCustomNodeBeingAdded)
                        {
                            //all selection nodes are changed prior to being inserted 
                            //(both tempdocs and db docs adhere to same schemas)
                            //addsArguments.URIToEdit must be the parent of selectedURI
                            bIsReadyToAdd =
                                ChangeSelectionForInsertion(uri, addsArguments,
                                    ref selectedElement);
                        }
                        else
                        {
                            bIsReadyToAdd = true;
                        }
                    }
                    else
                    {
                        bIsReadyToAdd = true;
                    }
                    if (bIsReadyToAdd)
                    {
                        //add it to its parent in root
                        AddElementToParent(addsArguments,
                            root, selectedElement);
                    }
                }
                else
                {
                    uri.ErrorMessage 
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                            "ADDS_NOSELECTION");
                }
            }
            //this is not an else clause
            if (selectedURI.ErrorMessage != string.Empty)
            {
                uri.ErrorMessage = selectedURI.ErrorMessage;
            }
            else if (addsArguments.ErrorMessage != string.Empty)
            {
                uri.ErrorMessage = addsArguments.ErrorMessage;
            }
        }
        private static void SetURIToAddForInsertion(
            EditHelper.ArgumentsEdits addsArguments,
            bool isAncestor, ContentURI selectedURI)
        {
            addsArguments.URIToAdd = new ContentURI(selectedURI);
            addsArguments.URIToAddOriginalURIPattern = selectedURI.URIPattern;
            addsArguments.IsAncestorBeingAdded = isAncestor;
            Helpers.GeneralHelpers.APPLICATION_TYPES eAppType = 
                Helpers.GeneralHelpers.APPLICATION_TYPES.none;
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES eSubAppType 
                = Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.none;
            Helpers.GeneralHelpers.GetAppTypesFromNodeName(
                addsArguments.URIToAdd.URINodeName, out eAppType, out eSubAppType);
            if (eAppType == Helpers.GeneralHelpers.APPLICATION_TYPES.none
                || eSubAppType == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.none)
            {
                if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    if (addsArguments.URIToEdit.URIDataManager.UseSelectedLinkedView)
                    {
                        addsArguments.IsCustomNodeBeingAdded = true;
                    }
                }
            }
        }
        private XElement GetSelection(EditHelper.ArgumentsEdits addsArguments)
        {
            XElement selectedElement = null;
            string sId = ContentURI.GetURIPatternPart(addsArguments.URIToAdd.URIPattern,
                ContentURI.URIPATTERNPART.id);
            Stream xmlStream = null;
            //SqlXmlHelper oSqlXmlHelper = new SqlXmlHelper();
            //oSqlXmlHelper.GetDevTrek(sId, addsArguments.URIToAdd,
            //    addsArguments.DevTrekSchemaName,
            //    addsArguments.DevTrekNodeName,
            //    addsArguments.DevTrekNamespace,
            //    addsArguments.DevTrekNodeId,
            //    out xmlStream);
            if (xmlStream != null)
            {
                using (xmlStream)
                {
                    //make an xelement for further manipulation
                    using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                    {
                        //skip the root node so that edits can be made
                        //directly to selectedElement
                        xmlReader.MoveToContent();
                        if (xmlReader.LocalName
                            == Helpers.GeneralHelpers.ROOT_PATH)
                        {
                            xmlReader.Read();
                        }
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            //read subtree ensures last </root> el is ignored
                            selectedElement
                                = XElement.Load(xmlReader.ReadSubtree());
                        }
                        else
                        {
                            addsArguments.ErrorMessage
                                = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                                "ADDS_NODBRECORD");
                        }
                    }
                }
            }
            //oSqlXmlHelper = null;
            return selectedElement;
        }
        private XElement GetSelectionForServiceBase(
            EditHelper.ArgumentsEdits addsArguments)
        {
            XElement selectedElement = null;
            //the transition from serviceagreement namespaces to service namespaces
            //cause incompatibilities best dealt with by avoiding namespaces
            selectedElement = new XElement(
                addsArguments.URIToAdd.URINodeName,
                new XAttribute(AppHelpers.Calculator.cId, addsArguments.URIToAdd.URIId.ToString()),
                new XAttribute(AppHelpers.Calculator.cName, addsArguments.URIToAdd.URIName.ToString())
            );
            //if more is needed, use
            ////sqlxml returns all children groups, so use sp instead
            //_repository.SetServiceAndChangeApplication(
            //    addsArguments.URIToAdd, addsArguments.URIToAdd.URIId);
            return selectedElement;
        }
        private XElement GetSelectionForCustomNode(
            EditHelper.ArgumentsEdits addsArguments, XElement root)
        {
            XElement selectedElement = null;
            //use the nodename and id=1 to retrieve a default node from the 
            //document being edited (the base story must follow the story1.xml pattern)
            XElement defaultElement = XmlLinq.GetElement(root,
                addsArguments.URIToAdd.URINodeName, "1");
            if (defaultElement != null)
            {
                selectedElement = new XElement(defaultElement);
                //now no byref, so can remove child nodes
                //don't want child nodes
                selectedElement.RemoveNodes();
                //sequential ids (i.e. add defaults) must be randomized with the same 
                //random object
                string sNewId = XmlLinq.AddRandomIdToElement(addsArguments, selectedElement);
                string sNewURIPattern = ContentURI.ChangeURIPatternPart(
                    addsArguments.URIToAdd.URIPattern, ContentURI.URIPATTERNPART.id,
                    sNewId);
                addsArguments.URIToAdd.URIPattern = sNewURIPattern;
                addsArguments.URIToAdd.URIId 
                    = Helpers.GeneralHelpers.ConvertStringToInt(sNewId);
            }
            else
            {
                addsArguments.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                                "ADDS_NODEFAULTNODETOCOPY");
            }
            return selectedElement;
        }
        private XElement GetSelectionForServiceBaseNS(
            EditHelper.ArgumentsEdits addsArguments)
        {
            XElement selectedElement = null;
            //results in a y0:nodename syntax
            //the transition from serviceagreement namespaces to services
            //cause incompatibilities best dealt with by avoiding namespaces
            //if used, refer to help file for changing the namespace of an entire xml tree
            XNamespace y0 = addsArguments.DevTrekNamespace.Replace("'", string.Empty);
            selectedElement = new XElement(
                y0 + addsArguments.URIToAdd.URINodeName,
                new XAttribute(XNamespace.Xmlns + "y0", addsArguments.DevTrekNamespace.Replace("'", string.Empty)),
                new XAttribute(AppHelpers.Calculator.cId, addsArguments.URIToAdd.URIId.ToString()),
                new XAttribute(AppHelpers.Calculator.cName, addsArguments.URIToAdd.URIName.ToString())
            );
            return selectedElement;
        }
        //private static void SetSqlXmlParameters(bool isAncestor,
        //    EditHelper.ArgumentsEdits addsArguments)
        //{
        //    SetSchemaPropertiesForSelections(isAncestor, addsArguments);
        //    //use uritoadd params to get the selection
        //    SqlXmlHelper.SetSqlXmlParams(addsArguments, addsArguments.URIToAdd);
        //    if (string.IsNullOrEmpty(addsArguments.DevTrekSchemaName)
        //        || string.IsNullOrEmpty(addsArguments.DevTrekNodeId)
        //        || string.IsNullOrEmpty(addsArguments.DevTrekNodeName))
        //    {
        //        addsArguments.ErrorMessage
        //            = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
        //            "ADDS_NOSQLPARAMS");
        //    }
        //}
        private static void SetSchemaPropertiesForSelections(bool isAncestor,
            EditHelper.ArgumentsEdits addsArguments)
        {
            //selections don't need updategrams
            addsArguments.IsUpdateGram = false;
            //selections generally don't need full schemas
            addsArguments.NeedsFullSchema = false;
            if (!isAncestor)
            {
                //exceptions to single node schemas
                if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements
                    && addsArguments.URIToAdd.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
                {
                    //retrieve multiple nodes without doing any updates
                    addsArguments.NeedsFullSchema = true;
                    addsArguments.IsUpdateGram = false;
                }
                else if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                        != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //devpack nodes need children added too (to get their media resources too)
                        if (addsArguments.URIToEdit.URINodeName
                            == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackgroup.ToString()
                            || addsArguments.URIToEdit.URINodeName
                            == AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                        {
                            //retrieve multiple nodes without doing any updates
                            addsArguments.NeedsFullSchema = true;
                            addsArguments.IsUpdateGram = false;
                        }
                    }
                }
                else if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.resources)
                {
                    if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                        != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //resourcepack nodes need children added too (to get their media resources too)
                        if (addsArguments.URIToEdit.URINodeName
                            == AppHelpers.Resources.RESOURCES_TYPES.resourcegroup.ToString())
                        {
                            //retrieve multiple nodes without doing any updates
                            addsArguments.NeedsFullSchema = true;
                            addsArguments.IsUpdateGram = false;
                        }
                    }
                }
                else if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.prices)
                {
                    if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                        != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        if (addsArguments.URIToAdd.URINodeName
                            == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                            || addsArguments.URIToAdd.URINodeName
                            == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()
                            || addsArguments.URIToAdd.URINodeName
                            == AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                        {
                            //retrieve multiple nodes without doing any updates
                            addsArguments.NeedsFullSchema = true;
                            addsArguments.IsUpdateGram = false;
                        }
                    }
                }
                else if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.economics1)
                {
                    if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                        != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        if (addsArguments.URIToAdd.URINodeName
                            == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                            || addsArguments.URIToAdd.URINodeName
                            == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                        {
                            //retrieve multiple nodes without doing any updates
                            addsArguments.NeedsFullSchema = true;
                            addsArguments.IsUpdateGram = false;
                        }
                        else
                        {
                            //timeperiod operation/component selections require children inputs too
                            if (addsArguments.URIToEdit.URINodeName
                                == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                                || addsArguments.URIToEdit.URINodeName
                                == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                if (addsArguments.URIToAdd.URINodeName
                                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString()
                                    || addsArguments.URIToAdd.URINodeName
                                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString()
                                    || addsArguments.URIToAdd.URINodeName
                                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString()
                                    || addsArguments.URIToAdd.URINodeName
                                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                                {
                                    //retrieve multiple nodes without doing any updates
                                    addsArguments.NeedsFullSchema = true;
                                    addsArguments.IsUpdateGram = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void AddElementToParent(
            EditHelper.ArgumentsEdits addsArguments,
            XElement root, XElement selectedElement)
        {
            if (!root.HasElements)
            {
                XmlLinq.AddElementToParent(addsArguments,
                    root, selectedElement);
            }
            else if (addsArguments.URIToAdd.URIDataManager.ParentURIPattern
                == string.Empty)
            {
                //no parenturipattern means that it gets added to root
                //but only if it has not been added already (don't need 
                //multiple ancestors of the same type)
                XmlLinq.AddElementToRootIfUnique(addsArguments, root,
                    selectedElement);
            }
            else
            {
                string sGroupingNodeName = GetGroupingNodeName(addsArguments);
                bool bHasInserted = XmlLinq.AddElementToParentUsingURI(
                    addsArguments, root, selectedElement, 
                    sGroupingNodeName);
            }
        }

        private static bool ChangeSelectionForInsertion(ContentURI uri,
            EditHelper.ArgumentsEdits addsArguments, 
            ref XElement selectedElement)
        {
            //all xml must be consistent with schemas, whether db or tempdocs
            bool bIsReadyToAdd = false;
            //change admin app props
            ChangeAdminAttributes(addsArguments, selectedElement);
            //change apps attributes
            ChangeAttributes(addsArguments, ref selectedElement);
            //change id if tempdoc
            ChangeTempDocId(uri, addsArguments, ref selectedElement);
            //update addsArguments.URIToAdd.URIPattern to selectedEl atts
            UpdateURIToAdd(addsArguments, selectedElement);
            if (addsArguments.ErrorMessage == string.Empty)
            {
                bIsReadyToAdd = true;
            }
            return bIsReadyToAdd;
        }
        private static void ChangeAdminAttributes(
            EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement)
        {
            //admin attributes to change
            if (addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                AppHelpers.Agreement.ChangeAttributesForInsertion(addsArguments,
                    addsArguments.URIToAdd, selectedElement);
            }
            else if (addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.networks
                || addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.addins
                || addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.locals
                || addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                //accountid is always currently logged-in user (for all db-inserted contracts)
                selectedElement.SetAttributeValue(AppHelpers.General.ACCOUNTID,
                    addsArguments.URIToEdit.URIMember.ClubInUse.PKId.ToString());
                bool bIsBaseKey = false;
                string sChildForeignName = EditHelper.GetForeignKeyName(
                    addsArguments.URIToEdit.URIDataManager.AppType,
                    addsArguments.URIToEdit.URIDataManager.SubAppType,
                    addsArguments.URIToEdit.URINodeName, bIsBaseKey);
                //change the foreign key 
                if (sChildForeignName != AppHelpers.General.ACCOUNTID)
                {
                    selectedElement.SetAttributeValue(sChildForeignName,
                        addsArguments.URIToEdit.URIId.ToString());
                }
                //is default network, club, or addin is always false 
                selectedElement.SetAttributeValue(AppHelpers.Members.ISDEFAULTCLUB, "0");
                if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.members)
                {
                    //members attributes
                    AppHelpers.Members.ChangeAttributesForInsertion(selectedElement);
                }
                else if (addsArguments.URIToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.locals)
                {
                    //locals attributes
                    AppHelpers.Locals.ChangeAttributesForInsertion(selectedElement);
                }
            }
            else if (addsArguments.URIToEdit.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.prices
                || addsArguments.URIToEdit.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.economics1)
            {
                if (addsArguments.URIToEdit.URIDataManager.ServerSubActionType
                    == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                {
                    //add default locals (helps the initial ui with unit selections ...)
                    Data.AppHelpers.Locals.AddLocals(addsArguments.URIToEdit,
                        addsArguments.URIToAdd.URIId, selectedElement);
                }
            }
        }
        private static void ChangeParentId(
            EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement)
        {
            bool bIsBaseKey = false;
            string sChildForeignName = EditHelper.GetForeignKeyName(
                addsArguments.URIToEdit.URIDataManager.AppType,
                addsArguments.URIToEdit.URIDataManager.SubAppType, 
                addsArguments.URIToEdit.URINodeName, bIsBaseKey);
            if (sChildForeignName != string.Empty 
                && addsArguments.URIToEdit.URIId.ToString() != string.Empty
                && addsArguments.URIToEdit.URIId.ToString() != "0" 
                && addsArguments.URIToEdit.URINodeName != Helpers.GeneralHelpers.ROOT_PATH)
            {
                if (addsArguments.URIToEdit.URINodeName
                    != AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    //set the new parent id (setattval also positions the nav back on the element)
                    selectedElement.SetAttributeValue(sChildForeignName,
                        addsArguments.URIToEdit.URIId.ToString());
                    if (addsArguments.URIToAdd.URINodeName.ToString()
                        == AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                    {
                        //recursive node insertions require setting the recursive key (i.e. ParentId) to null
                        //so it won't reference old parent
                        string sRecursiveChildForeignName 
                            = EditHelper.GetRecursiveParentKeyName();
                        //need a null recursive parent id 
                        selectedElement.SetAttributeValue(sRecursiveChildForeignName, 
                            null);
                    }
                }
                else
                {
                    if (addsArguments.URIToEdit.URINodeName
                        == addsArguments.URIToAdd.URINodeName.ToString())
                    {
                        //a recursive (self refd) node is being added
                        string sRecursiveChildForeignName = string.Empty;
                        sRecursiveChildForeignName 
                            = EditHelper.GetRecursiveParentKeyName();
                        //set the recursive parent id 
                        selectedElement.SetAttributeValue(sRecursiveChildForeignName,
                            addsArguments.URIToEdit.URIId.ToString());
                        //set the recursive node's parent ids
                        string sRecursiveNodeParentKeyName = string.Empty;
                        string sRecursiveNodeParentId = string.Empty;
                        EditHelper.GetParentsOfRecursiveNodesIds(addsArguments,
                            addsArguments.URIToEdit.URIDataManager.AppType, 
                            out sRecursiveNodeParentKeyName,
                            out sRecursiveNodeParentId);
                        if (!string.IsNullOrEmpty(sRecursiveNodeParentKeyName)
                            && !string.IsNullOrEmpty(sRecursiveNodeParentId))
                        {
                            selectedElement.SetAttributeValue(sRecursiveNodeParentKeyName,
                                sRecursiveNodeParentId);
                        }
                    }
                    else
                    {
                        //set the regular parent id 
                        selectedElement.SetAttributeValue(sChildForeignName,
                            addsArguments.URIToEdit.URIId.ToString());
                    }
                }
            }
            else
            {
                addsArguments.ErrorMessage
                     = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                        "ADDS_CANTADDCHILDTOPARENT");
            }
        }
        private static void ChangeAttributes(
            EditHelper.ArgumentsEdits addsArguments,
            ref XElement selectedElement)
        {
            //new atts only needed when base tables added to join tables 
            //(i.e. inputs to budgets)
            bool bNeedsNewAttributes = NeedsNewAttributes(
                addsArguments.URIToEdit.URIDataManager.AppType,
                addsArguments.URIToEdit.URINodeName,
                addsArguments.URIToAdd.URINodeName);
            //check for element name switches (i.e. inputseries into operation)
            bool bDeleteNonStdAtts = true;
            string sSelectedURINodeName = addsArguments.URIToAdd.URINodeName;
            GetNodeNameAndDeleteNonStdAtts(addsArguments.URIToEdit.URINodeName,
                ref sSelectedURINodeName, bDeleteNonStdAtts,
                ref selectedElement);
            if (bNeedsNewAttributes == false)
            {
                //change the parentid
                ChangeParentId(addsArguments, selectedElement);
                //check for, and add, missing children grouping nodes (i.e. tp/outs and tp/ops)
                AddGroupingElements(addsArguments, selectedElement);
            }
            else
            {
                //uses a reader to change attributes
                ReplaceSelectedElement(addsArguments,
                    ref selectedElement, sSelectedURINodeName);
            }
        }
        private static bool NeedsNewAttributes(
            Helpers.GeneralHelpers.APPLICATION_TYPES appType,
            string parentNodeName, string currentNodeName)
        {
            //new atts are generally needed when base tables are added to join tables 
            //(which happens the majority of inserts)
            bool bNeedsNewAtts = false;
            if (currentNodeName 
                == AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName
                == AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
            {
                if (parentNodeName
                    != AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Resources.RESOURCES_TYPES.resourcegroup.ToString())
                {
                    //devpacks (devpackresourcepack used for nav) and linkedviews (linkedviewresourcepack used for nav)
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                if (parentNodeName
                    != AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                {
                    //linkedview being linked to other apps
                    bNeedsNewAtts = true;
                }
            }
            else if (currentNodeName 
                == AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                //refactor?: devpackpart is not recursive
                //devpackpart can be added to devpackpart because that table is a self refd table
                if (parentNodeName 
                    != AppHelpers.DevPacks.DEVPACKS_TYPES.devpackgroup.ToString()
                    && parentNodeName 
                    != AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    bNeedsNewAtts = true;
                }
            }
            else if (appType == Helpers.GeneralHelpers.APPLICATION_TYPES.networks
                || appType == Helpers.GeneralHelpers.APPLICATION_TYPES.locals
                || appType == Helpers.GeneralHelpers.APPLICATION_TYPES.addins
                || appType == Helpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                bNeedsNewAtts = true;
            }
            return bNeedsNewAtts;
        }
        private static void ChangeTempDocId(ContentURI uri, 
            EditHelper.ArgumentsEdits addsArguments, ref XElement selectedElement)
        {
            //tempdocs need randomized ids because their nodes don't have the uniqueness of db docs
            if (uri.URIFileExtensionType
                == Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                if (addsArguments.RndGenerator == null)
                    addsArguments.RndGenerator = new Random();
                string sNewId
                    = Helpers.GeneralHelpers.GetRandomInteger(addsArguments.RndGenerator).ToString();
                //still need the oldid to be able to retrieve linkedviews
                string sOldId = XmlLinq.GetAttributeValue(selectedElement, 
                    AppHelpers.Calculator.cId);
                selectedElement.SetAttributeValue(AppHelpers.Calculator.cId, sNewId);
                selectedElement.SetAttributeValue(AppHelpers.General.OLDID, sOldId);
            }
        }
        private static void GetNodeNameAndDeleteNonStdAtts(string parentNodeName, 
            ref string currentNodeName, bool deleteNonStdAtts, 
            ref XElement selectedElement)
        {
            if (currentNodeName 
                == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    //inserting an input series into a parent expecting an input
                    currentNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString();
                    if (deleteNonStdAtts == true)
                    {
                        //get rid of non-standard attributes before using moving them using ChangeAttributes
                        selectedElement.SetAttributeValue("InputId", null);
                    }
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
            {
                if (parentNodeName == AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                {
                    //inserting an input into an input series
                    currentNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString();
                    if (selectedElement.HasElements)
                    {
                        selectedElement.RemoveNodes();
                    }
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                if (parentNodeName 
                    != AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    //inserting an output series into a parent expecting an output
                    currentNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString();
                    if (deleteNonStdAtts == true)
                    {
                        //get rid of non-standard attributes before using moving them using ChangeAttributes
                        selectedElement.SetAttributeValue("OutputId", null);
                    }
                }
            }
            else if (currentNodeName 
                == AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
            {
                if (parentNodeName 
                    == AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                {
                    //inserting an output into an output series
                    currentNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString();
                    if (selectedElement.HasElements)
                    {
                        selectedElement.RemoveNodes();
                    }
                }
            }
            else if (currentNodeName 
                == AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString())
            {
                if (parentNodeName 
                    == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    //attaching an existing resource pack to a linkedview
                    currentNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString();
                }
                else if (parentNodeName 
                    == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                {
                    //attaching an existing resource pack to a devpackpart
                    currentNodeName = AppHelpers.DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString();
                }
            }
            else if (currentNodeName 
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                if (parentNodeName
                    == AppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
                {
                    //attaching a base node to a join table node
                    currentNodeName = AppHelpers.AddIns.ADDIN_TYPES.addin.ToString();
                }
                else if (parentNodeName
                    == AppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    //attaching a base node to a join table node
                    currentNodeName = AppHelpers.Locals.LOCAL_TYPES.local.ToString();
                }
            }
            else if (currentNodeName 
                == AppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString())
            {
                if (parentNodeName 
                    == AppHelpers.Networks.NETWORK_TYPES.networkaccountgroup.ToString())
                {
                    //attaching a base node to a join table node
                    currentNodeName = AppHelpers.Networks.NETWORK_TYPES.network.ToString();
                }
            }
            else if (currentNodeName 
                == AppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString())
            {
                if (parentNodeName 
                    == AppHelpers.Members.MEMBER_TYPES.memberaccountgroup.ToString())
                {
                    //attaching a base node to a join table node
                    currentNodeName = AppHelpers.Members.MEMBER_TYPES.member.ToString();
                }
            }
        }
        private static void AddGroupingElements(
            EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement)
        {
            if (addsArguments.URIToEdit.URIDataManager.AppType 
                == Helpers.GeneralHelpers.APPLICATION_TYPES.economics1)
            {
                if (addsArguments.URIToAdd.URINodeName 
                    == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                    || addsArguments.URIToAdd.URINodeName 
                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //before adding a single time period, adds it children grouping els too
                    string sLocalName = selectedElement.Name.ToString();
                    if ((sLocalName 
                        == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                        || sLocalName 
                        == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                        && selectedElement.HasElements == false)
                    {
                        AddGroupingNodes(addsArguments, selectedElement);
                    }
                }
            }
        }
        public static void AddGroupingNodes(
            EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement)
        {
            if (addsArguments.URIToEdit.URIDataManager.SubAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
            {
                //append outcomes grouping node
                selectedElement.Add(new XElement(AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString()));
                //append operations grouping node
                selectedElement.Add(new XElement(AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString()));
            }
            else if (addsArguments.URIToEdit.URIDataManager.SubAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments)
            {
                //append outcomes grouping node
                selectedElement.Add(new XElement(AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString()));
                //append components grouping node
                selectedElement.Add(new XElement(AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString()));
            }
        }
        public static string GetGroupingNodeName(
            EditHelper.ArgumentsEdits addsArguments)
        {
            string sGroupingNodeName = string.Empty;
            if (addsArguments.URIToEdit.URINodeName
                == AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                || addsArguments.URIToEdit.URINodeName
                == AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                {
                    sGroupingNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString();
                }
                else if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                {
                    sGroupingNodeName = AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString();
                }
                else if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                {
                    sGroupingNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString();
                }
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString())
                {
                    sGroupingNodeName = AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString();
                }
            }
            return sGroupingNodeName;
        }
        private static void ReplaceSelectedElement(
            EditHelper.ArgumentsEdits addsArguments, 
            ref XElement selectedElement, string currentNodeName)
        {
            bool bIsRecursiveNode = false;
            using (StringWriter oStringWriter = new StringWriter())
            {
                XmlWriterSettings oXmlWriterSettings = new XmlWriterSettings();
                oXmlWriterSettings.Indent = true;
                oXmlWriterSettings.OmitXmlDeclaration = true;
                using (XmlWriter oXmlWriter
                    = XmlWriter.Create(oStringWriter, oXmlWriterSettings))
                {
                    bool bIsUpdateGram = false;
                    //although baseid is generated, writenode handles it
                    string sBaseId = string.Empty;
                    WriteNode(addsArguments, selectedElement,
                        addsArguments.URIToEdit.URIId.ToString(),
                        ref sBaseId, currentNodeName,
                        bIsRecursiveNode, bIsUpdateGram, oXmlWriter);
                    oXmlWriter.Flush();
                    selectedElement = new XElement(XElement.Parse(oStringWriter.ToString()));
                }
            }
        }
        public static void WriteNode(EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement, string parentId,
            ref string baseTableId, string currentNodeName,
            bool isRecursiveNode, bool isUpdateGram, XmlWriter xmlWriter)
        {
            //write using a reader
            if (selectedElement.NodeType
                == XmlNodeType.Element)
            {
                using (XmlReader oInsertedNodesReader =
                    selectedElement.CreateReader())
                {
                    //write all of the nodes (including children) 
                    while (oInsertedNodesReader.Read())
                    {
                        if (!oInsertedNodesReader.LocalName.Equals(
                            Helpers.GeneralHelpers.ROOT_PATH))
                        {
                            MakeNode(addsArguments, oInsertedNodesReader,
                                parentId, ref baseTableId, currentNodeName,
                                isRecursiveNode, isUpdateGram, xmlWriter);
                        }
                        else
                        {
                            SkipLinkedView(oInsertedNodesReader); 
                            //make the next node without a read action
                            MakeNode(addsArguments, oInsertedNodesReader,
                                parentId, ref baseTableId, currentNodeName,
                                isRecursiveNode, isUpdateGram, xmlWriter);
                        }
                    }
                }
            }
        }
        public static void SkipLinkedView(XmlReader insertedNodesReader)
        {
            if (insertedNodesReader.LocalName.Equals(
                 Helpers.GeneralHelpers.ROOT_PATH))
            {
                insertedNodesReader.Skip();
                //recurse to handle siblings
                if (insertedNodesReader.LocalName.Equals(
                 Helpers.GeneralHelpers.ROOT_PATH))
                {
                    SkipLinkedView(insertedNodesReader);
                }
            }
        }
        private static void MakeNode(EditHelper.ArgumentsEdits addsArguments,
            XmlReader insertedNodesReader,
            string parentId, ref string baseTableId,
            string currentNodeName, bool isRecursiveNode, bool isUpdateGram,
            XmlWriter xmlWriter)
        {
            string sCurrentNodeName = currentNodeName;
            if (insertedNodesReader.IsStartElement())
            {
                if (insertedNodesReader.Depth == 0 && isUpdateGram == false)
                {
                    //starting element name may have changed (i.e. output to outputseries)
                    xmlWriter.WriteStartElement(currentNodeName);
                }
                else
                {
                    xmlWriter.WriteStartElement(insertedNodesReader.LocalName);
                    sCurrentNodeName = insertedNodesReader.LocalName;
                }
                if (insertedNodesReader.HasAttributes
                    && insertedNodesReader.LocalName.Equals(
                    Helpers.GeneralHelpers.ROOT_PATH) == false)
                {
                    if (insertedNodesReader.IsEmptyElement == false)
                    {
                        ////write the attributes
                        //if (isUpdateGram)
                        //{
                        //    SqlXmlHelper.InsertGramWriteAttributes(xmlWriter, insertedNodesReader);
                        //}
                        //else
                        //{
                            ChangeAttributes(addsArguments,
                                insertedNodesReader, parentId, ref baseTableId,
                                sCurrentNodeName, isRecursiveNode, xmlWriter);
                        //}
                        insertedNodesReader.MoveToElement();
                    }
                    else
                    {
                        //write the attributes
                        //if (isUpdateGram)
                        //{
                        //    SqlXmlHelper.InsertGramWriteAttributes(xmlWriter, insertedNodesReader);
                        //}
                        //else
                        //{
                            ChangeAttributes(addsArguments,
                                insertedNodesReader, parentId, ref baseTableId,
                                sCurrentNodeName, isRecursiveNode, xmlWriter);
                        //}
                        insertedNodesReader.MoveToElement();
                        //<input id="" />
                        //close it out
                        xmlWriter.WriteEndElement();
                    }
                }
                else if (insertedNodesReader.IsEmptyElement == true)
                {
                    //i.e. outputs without any outputs yet
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                if (insertedNodesReader.NodeType == XmlNodeType.EndElement)
                {
                    //</operation>
                    //must be moving back up the hierarchy
                    xmlWriter.WriteEndElement();
                }
            }
        }
        
        private static void ChangeAttributes(
            EditHelper.ArgumentsEdits addsArguments, 
            XmlReader selectedNodeReader, string parentId,
            ref string baseTableId, string currentNodeName,
            bool isRecursiveNode, XmlWriter writer)
        {
            //write the attributes
            if (currentNodeName 
                == AppHelpers.Economics1.BUDGET_TYPES.budgetoutput.ToString()
                || currentNodeName 
                == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutput.ToString()
                || currentNodeName 
                == AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                //change the standard attributes
                ChangeStandardAttributes(selectedNodeReader, 
                    ref baseTableId, writer);
                //change the subapplication specific attributes
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    isRecursiveNode = true;
                    parentId = addsArguments.URIToAdd.URIId.ToString();
                }
                else if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                {
                    parentId = addsArguments.URIToEdit.URIId.ToString();
                }
                //change the subapplication specific attributes
                AppHelpers.Prices.ChangeOutputAttributes(addsArguments, 
                    selectedNodeReader, parentId, baseTableId, 
                    currentNodeName, isRecursiveNode, writer);
            }
            else if (currentNodeName 
                == AppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString()
                || currentNodeName
                == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString())
            {
                //change the subapplication specific attributes
                AppHelpers.Prices.ChangeOperationandComponentAttributes(
                    addsArguments, selectedNodeReader, parentId, 
                    baseTableId, currentNodeName, isRecursiveNode, writer);
            }
            else if (currentNodeName 
                == AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString()
                || currentNodeName
                == AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
            {
                //change the subapplication specific attributes
                AppHelpers.Prices.ChangeOperationandComponentAttributes(
                    addsArguments, selectedNodeReader,
                    parentId, baseTableId, currentNodeName, 
                    isRecursiveNode, writer);
            }
            else if (currentNodeName 
                == AppHelpers.Economics1.BUDGET_TYPES.budgetinput.ToString()
                || currentNodeName 
                == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                //input base table to budget/cost system-to-input table
                ChangeStandardAttributes(selectedNodeReader, 
                    ref baseTableId, writer);
                //change the subapplication specific attributes
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    || addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    isRecursiveNode = true;
                    parentId = addsArguments.URIToAdd.URIId.ToString();
                }
                else if (addsArguments.URIToEdit.URINodeName
                    == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    || addsArguments.URIToEdit.URINodeName
                    == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    parentId = addsArguments.URIToEdit.URIId.ToString();
                }
                AppHelpers.Prices.ChangeInputAttributes(addsArguments, 
                    selectedNodeReader, parentId, baseTableId, 
                    currentNodeName, isRecursiveNode, writer);
            }
            else if (currentNodeName
                == AppHelpers.Economics1.BUDGET_TYPES.budgetinput.ToString()
                || currentNodeName
                == AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            {
                //input base table to budget/cost system-to-input table
                ChangeStandardAttributes(selectedNodeReader,
                    ref baseTableId, writer);
                //change the subapplication specific attributes
                if (addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    || addsArguments.URIToAdd.URINodeName
                    == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    isRecursiveNode = true;
                    parentId = addsArguments.URIToAdd.URIId.ToString();
                }
                else if (addsArguments.URIToEdit.URINodeName
                    == AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    || addsArguments.URIToEdit.URINodeName
                    == AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                {
                    parentId = addsArguments.URIToEdit.URIId.ToString();
                }
                AppHelpers.Prices.ChangeInputAttributes(addsArguments,
                    selectedNodeReader, parentId, baseTableId,
                    currentNodeName, isRecursiveNode, writer);
            }
            else if (currentNodeName 
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString()
                || currentNodeName 
                == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                //linkedview or devpackdoc to resource pack (many to many table)
                ChangeStandardAttributes(selectedNodeReader, 
                    ref baseTableId, writer);
                ChangeStoryAttributes(addsArguments,
                    currentNodeName, baseTableId, writer);
            }
            else
            {
                //members, networks, locals and addins
                ChangeStandardAttributes(selectedNodeReader, 
                    ref baseTableId, writer);
                ChangeAdminBaseId(addsArguments,
                    currentNodeName, baseTableId, 
                    writer);
            }
        }

        private static void ChangeStandardAttributes(XmlReader selectedNodeReader,
            ref string baseTableId, XmlWriter writer)
        {
            string sAttName = string.Empty;
            string sAttValue = string.Empty;
            while (selectedNodeReader.MoveToNextAttribute())
            {
                sAttName = selectedNodeReader.Name;
                sAttValue = selectedNodeReader.Value;
                if (sAttName.Equals(AppHelpers.Calculator.cId))
                {
                    baseTableId = sAttValue;
                }
                writer.WriteAttributeString(sAttName, sAttValue);
            }
        }
        private static void ChangeStoryAttributes(
            EditHelper.ArgumentsEdits addsArguments,
            string currentNodeName, string baseTableId, 
            XmlWriter writer)
        {
            string sParentId = addsArguments.URIToEdit.URIId.ToString();
            string sChildForeignName = string.Empty;
            string sBaseForeignName = string.Empty;
            AppHelpers.Resources.GetChildForeignKeyName(
                AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString(),
                out sChildForeignName);
            string sParentNodeName = addsArguments.URIToEdit.URINodeName;
            if (currentNodeName 
                == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                //resource pack (basetableid) being attached to linkedview
                writer.WriteAttributeString(sChildForeignName, baseTableId);
                bool bIsBaseKey = false;
                sChildForeignName = EditHelper.GetForeignKeyName(addsArguments.URIToEdit.URIDataManager.AppType,
                    addsArguments.URIToEdit.URIDataManager.SubAppType,
                    sParentNodeName, bIsBaseKey);
                writer.WriteAttributeString(sChildForeignName, sParentId);
            }
            else if (currentNodeName 
                == AppHelpers.DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                //resource pack (basetableid) being attached to devpackpart
                writer.WriteAttributeString(sChildForeignName, baseTableId);
                bool bIsBaseKey = false;
                sChildForeignName = EditHelper.GetForeignKeyName(addsArguments.URIToEdit.URIDataManager.AppType,
                    addsArguments.URIToEdit.URIDataManager.SubAppType,
                    sParentNodeName, bIsBaseKey);
                writer.WriteAttributeString(sChildForeignName, sParentId);
            }
        }
        private static void ChangeAdminBaseId(
            EditHelper.ArgumentsEdits addsArguments,
            string currentNodeName, string baseTableId,
            XmlWriter writer)
        {
            bool bIsBaseKey = true;
            string sBaseKeyAttName = EditHelper.GetForeignKeyName(
                addsArguments.URIToEdit.URIDataManager.AppType,
                addsArguments.URIToEdit.URIDataManager.SubAppType,
                addsArguments.URIToEdit.URINodeName, bIsBaseKey);
            if (!string.IsNullOrEmpty(sBaseKeyAttName))
            {
                writer.WriteAttributeString(sBaseKeyAttName, baseTableId);
            }
        }
        public static string MakeChildParentURIPattern(string childURIPattern,
            string parentURIPattern)
        {
            string sChildParentDelimitedString = string.Concat(childURIPattern,
                Helpers.GeneralHelpers.STRING_DELIMITER, parentURIPattern);
            string sURIPattern = string.Empty;
            if (!childURIPattern.EndsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER)
                && parentURIPattern.StartsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
            {
                sChildParentDelimitedString = string.Concat(childURIPattern, parentURIPattern);
            }
            if (childURIPattern.EndsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER)
                && !parentURIPattern.StartsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
            {
                sChildParentDelimitedString = string.Concat(childURIPattern, parentURIPattern);
            }
            else if (childURIPattern.EndsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER)
                && parentURIPattern.StartsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
            {
                sURIPattern = parentURIPattern.Substring(1, parentURIPattern.Length -1);
                sChildParentDelimitedString = string.Concat(childURIPattern, sURIPattern);
            }
            else if (!childURIPattern.EndsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER)
                && !parentURIPattern.StartsWith(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
            {
                sURIPattern = parentURIPattern.Substring(1, parentURIPattern.Length - 1);
                sChildParentDelimitedString = string.Concat(childURIPattern, Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER, 
                    sURIPattern);
            }
            return sChildParentDelimitedString;
        }
        public static void GetChildParentURIPatterns(string childParentURIPatterns,
            out string childURIPattern, out string parentURIPattern)
        {
            childURIPattern = Helpers.GeneralHelpers.GetSubstringFromFront(
                childParentURIPatterns, Helpers.GeneralHelpers.STRING_DELIMITERS, 1);
            parentURIPattern = string.Empty;
            if (string.IsNullOrEmpty(childURIPattern))
            {
                childURIPattern = childParentURIPatterns;
            }
            else
            {
                parentURIPattern = Helpers.GeneralHelpers.GetSubstringFromFront(
                    childParentURIPatterns, Helpers.GeneralHelpers.STRING_DELIMITERS, 2);
            }
        }

        public static void SetTempURIForDisplay(ContentURI uri, 
            EditHelper.ArgumentsEdits addsArguments, XElement root)
        {
            //tempdocs need adjustment before they can be displayed
            if (uri.URIPattern == string.Empty
                || uri.URIId == 0
                || uri.URINodeName == Helpers.GeneralHelpers.NONE
                || uri.URIDataManager.SubAppType 
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.none)
            {
                string sTempURIPattern = uri.URIPattern;
                if (addsArguments.URIToEdit.URINodeName
                    != AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                {
                    Helpers.AppSettings.GetTempDocURIPattern(
                        addsArguments.URIToEdit, out sTempURIPattern);
                }
                else
                {
                    Helpers.AppSettings.GetTempDocURIPattern(
                        addsArguments.URIToAdd, out sTempURIPattern);
                }
                ContentURI.ChangeURIPattern(uri, sTempURIPattern);
            }
            uri.URIDataManager.EditViewEditType
                = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
        }
        private static void UpdateURIToAdd(
            EditHelper.ArgumentsEdits addsArguments,
            XElement selectedElement)
        {
            if (addsArguments.URIToAdd.URINodeName
                != selectedElement.Name.LocalName)
            {
                string sId
                    = XmlLinq.GetAttributeValue(selectedElement,
                    AppHelpers.Calculator.cId);
                string sNewURIPattern = Helpers.GeneralHelpers.MakeURIPattern(
                    addsArguments.URIToAdd.URIName, sId, addsArguments.URIToAdd.URINetworkPartName,
                    selectedElement.Name.LocalName, addsArguments.URIToAdd.URIFileExtensionType);
                addsArguments.URIToAdd.ChangeURIPattern(sNewURIPattern);
            }
        }
    }
}
