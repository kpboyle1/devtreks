using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Streams through locals documents 
    ///             and publishes events that subscribers 
    ///             can use to run calculations and analyses on 
    ///             each node in each document. Developers can override the 
    ///             protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. DevTreks 'app store' api will be continually improved.
    ///            
    ///             
    /// </summary>
    public class LocalCalculator : GeneralCalculator 
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected LocalCalculator() { }
        protected LocalCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }
        
        //allows derived classes to override the default streaming 
        //and save method
        protected virtual bool StreamAndSaveCalculation()
        {
            bool bHasCalculations = false;
            //both calculators and analyzers use observationpath for initial file
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath
                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            if (!CalculatorHelpers.URIAbsoluteExists(
                this.GCCalculatorParams.ExtensionDocToCalcURI,
                this.GCCalculatorParams.AnalyzerParms.ObservationsPath))
                return bHasCalculations;
            //new temporary path to store calculator results
            //when writing to file, close it with the using
            StringWriter output = new StringWriter();
            this.GCCalculatorParams.DocToCalcReader 
                = DevTreks.Data.Helpers.FileStorageIO.GetXmlReader(
                    this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.InitialDocToCalcURI,
                    this.GCCalculatorParams.AnalyzerParms.ObservationsPath);
            if (this.GCCalculatorParams.DocToCalcReader != null)
            {
                using (this.GCCalculatorParams.DocToCalcReader)
                {
                    //descendent nodes are also processed using streaming techniques
                    //note that XStreamingElements were tested but had faults
                    IEnumerable<XElement> root =
                        from startingElement in StreamRoot()
                        select startingElement;
                    XmlWriterSettings writerSettings = new XmlWriterSettings();
                    writerSettings.OmitXmlDeclaration = true;
                    writerSettings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(
                        output, writerSettings))
                    {
                        foreach (XElement el in root)
                        {
                            //stop processing if any error message is found
                            if (this.GCCalculatorParams.ErrorMessage == string.Empty)
                            {
                                el.WriteTo(writer);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            using (output)
            {
                if (this.GCCalculatorParams.ErrorMessage == string.Empty
                        && this.GCArguments.HasCalculations)
                {
                    //move the new calculations to tempDocToCalcPath
                    //this returns an error msg
                    this.GCCalculatorParams.ErrorMessage = CalculatorHelpers.MoveURIs(
                        this.GCCalculatorParams.ExtensionDocToCalcURI, output,
                        this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                    bHasCalculations = true;
                }
            }
            return bHasCalculations;
        }
        //private string GetFileToCalculate()
        //{
        //    string sNewDocToCalcTempDocPath = string.Empty;
        //    //parallel processing needs unique temp file names
        //    if (this.GCCalculatorParams.RndGenerator == null)
        //        this.GCCalculatorParams.RndGenerator = new Random();
        //    int iRandomNumber = CalculatorHelpers.GetRandomInteger(
        //        this.GCCalculatorParams.RndGenerator);
        //    string sTempExt = string.Concat(iRandomNumber.ToString(), ".xml");
        //    sNewDocToCalcTempDocPath
        //        = this.GCCalculatorParams.AnalyzerParms.ObservationsPath.Replace(".xml", sTempExt);
        //    return sNewDocToCalcTempDocPath;
        //}
        protected virtual IEnumerable<XElement> StreamRoot()
        {
            //loop through the descendents in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamLocalGroups()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            yield return currentElement;
                        }
                    }
                }
            }
        }

        protected virtual IEnumerable<XElement> StreamLocalGroups()
        {
            //loop through the descendents in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        //attach calculators to parents
                        XElement xmlDocElement = XElement.Load(
                            this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                        yield return xmlDocElement;
                    }
                    else if (this.GCCalculatorParams.DocToCalcReader.Name
                        == DevTreks.Data.AppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == DevTreks.Data.AppHelpers.Locals.LOCALGROUP_NS.ToString())
                    {
                        this.GCCalculatorParams.ChangeStartingParams(
                            this.GCCalculatorParams.DocToCalcReader.Name);
                        //use streaming techniques to process descendents
                        XElement currentElement
                            = CalculatorHelpers.GetCurrentElementWithAttributes(
                            this.GCCalculatorParams.DocToCalcReader);
                        IEnumerable<XElement> childElements =
                            from childElement in StreamLocals()
                            select childElement;
                        if (currentElement != null)
                        {
                            if (childElements != null)
                            {
                                //add the child elements to the currentElement
                                currentElement.Add(childElements);
                            }
                            //0.8.4: locals only run for currentcalcdocid (child local node)
                            //this.RunCalculationsAndSetUpdates(ref currentElement);
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == Constants.ROOT_PATH)
                    {
                        break;
                    }
                }
            }
        }
        private IEnumerable<XElement> StreamLocals()
        {
            //loop through the descendents in document order
            while (this.GCCalculatorParams.DocToCalcReader.Read())
            {
                if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.Element)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == DevTreks.Data.AppHelpers.Locals.LOCAL_TYPES.local.ToString())
                    {
                        XElement currentElement
                            = XElement.Load(this.GCCalculatorParams.DocToCalcReader.ReadSubtree());
                        if (currentElement != null)
                        {
                            //0.8.4: locals only run for currentcalcdocid (child local node)
                            //this.RunCalculationsAndSetUpdates(ref currentElement);
                            string sCurrentId = CalculatorHelpers.GetAttribute(currentElement, Calculator1.cId);
                            if (sCurrentId == this.GCCalculatorParams.CalcDocId)
                            {
                                this.RunCalculationsAndSetUpdates(ref currentElement);
                            }
                            yield return currentElement;
                        }
                    }
                }
                else if (this.GCCalculatorParams.DocToCalcReader.NodeType
                    == XmlNodeType.EndElement)
                {
                    if (this.GCCalculatorParams.DocToCalcReader.Name
                        == DevTreks.Data.AppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString()
                        || this.GCCalculatorParams.DocToCalcReader.Name
                        == DevTreks.Data.AppHelpers.Locals.LOCALGROUP_NS.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }
}
