using System;
using System.Text;

using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Helpers
{
    // <summary>
    ///Purpose:		Help xslt stylesheet display comparisons
    ///Author:		www.devtreks.org
    ///Original script author is: Gerardo A., ARS, SW Watershed Research Center
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
	public class DisplayComparisons
	{
		public DisplayComparisons()
		{
			//individual object 
		}
		private string[] arrValues = null;
		private string[] arrNOPs = null;
		private static char[] DELIMITER_NAME = new char[] {';'};

        public string writeStringFullColumnTD(string name, string fullColCount)
        {
            //no title cell in col1
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount) + 1;
            if (iFullColCount < 10)
            {
                //these are minimal 10 col styles
                iFullColCount = 10;
            }
            string sHtml = string.Empty;
            StringBuilder oHtml = new StringBuilder();
            oHtml.Append("<td colspan='");
            oHtml.Append(iFullColCount);
            oHtml.Append("'>");
            oHtml.Append("<strong>");
            oHtml.Append(name);
            oHtml.Append("</strong>");
            oHtml.Append("</td>");
            sHtml = oHtml.ToString();
            oHtml = null;
            return sHtml;
        }
        public string writeStringFullColumnTH(string name, string fullColCount)
        {
            //no title cell in col1
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount) + 1;
            if (iFullColCount < 10)
            {
                //these are minimal 10 col styles
                iFullColCount = 10;
            }
            string sHtml = string.Empty;
            StringBuilder oHtml = new StringBuilder();
            oHtml.Append("<th colspan='");
            oHtml.Append(iFullColCount);
            oHtml.Append("'>");
            oHtml.Append(name);
            oHtml.Append("</th>");
            sHtml = oHtml.ToString();
            oHtml = null;
            return sHtml;
        }
		public string writeTitles(string fullColCount)
		{
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
			int i = 0;
			string sHtml = string.Empty;
			StringBuilder oHtml = new StringBuilder();
			while(i != iFullColCount)
			{
				if(i == 0)
				{
					oHtml.Append("<th scope='col'>All</th>");
				}
				else
				{
					oHtml.Append("<th scope='col'>Alt. ");
					oHtml.Append((i - 1)); 
					oHtml.Append("</th>");
				}
				i++;
			}
			sHtml = oHtml.ToString();
			oHtml = null;
			return sHtml;
		}
        public string WriteMobileTitles(string fullColCount)
        {
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
            int i = 0;
            string sHtml = string.Empty;
            bool bIsEvenNumber = true;
            StringBuilder oHtml = new StringBuilder();
            while (i != iFullColCount)
            {
                if (bIsEvenNumber)
                {
                    oHtml.Append("<div class='ui-block-a'>Alt. ");
                    oHtml.Append((i - 1));
                    oHtml.Append("</div>");
                }
                else
                {
                    oHtml.Append("<div class='ui-block-b'>Alt. ");
                    oHtml.Append((i - 1));
                    oHtml.Append("</div>");
                }
                i++;
                bIsEvenNumber = bIsEvenNumber ? false : true;
            }
            sHtml = oHtml.ToString();
            oHtml = null;
            return sHtml;
        }

		public string writeTitles2(string fullColCount, string title, string count1)
		{
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
            int iCount1 = DisplayHelpers.GetIntValue(count1);
			int i = 0;
			int j = 0;
			int iNewStart = 0;
			string sHtml = string.Empty;
			StringBuilder oHtml = new StringBuilder();
			while(i != iFullColCount)
			{
				if (j != 0) iNewStart = i / j;
				if (iNewStart == iCount1 || i == 0) 
				{
					//restart index
					if (title.StartsWith("Alt")) 
					{
						oHtml.Append("<th scope='col' colspan=");
						oHtml.Append(count1);
						oHtml.Append(">");
						if (i == 0) 
						{
							oHtml.Append("All Docs");
						}
						else 
						{
							oHtml.Append(title);
							oHtml.Append(" ");
							oHtml.Append((j - 1)); 
						}
						oHtml.Append("</th>");
						j += 1;
					}
					else 
					{
						oHtml.Append("<th>" + "All");
						oHtml.Append(" ");
						oHtml.Append(i); 
						oHtml.Append("</th>");
						j += 1;
					}
					
				}
				else 
				{
					if (title.StartsWith("Alt") == false) 
					{
						oHtml.Append("<th>" + title);
						oHtml.Append(" ");
						oHtml.Append(i); 
						oHtml.Append("</th>");
					}
				}
				i++;
			}
			sHtml = oHtml.ToString();
			oHtml = null;
			return sHtml;
		}
        public string WriteMobileTitles2(string fullColCount, string title, string count1)
        {
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
            int iCount1 = DisplayHelpers.GetIntValue(count1);
            int i = 0;
            int j = 0;
            int iNewStart = 0;
            string sHtml = string.Empty;
            StringBuilder oHtml = new StringBuilder();
            while (i != iFullColCount)
            {
                if (j != 0) iNewStart = i / j;
                if (iNewStart == iCount1 || i == 0)
                {
                    //restart index
                    if (title.StartsWith("Alt"))
                    {
                        oHtml.Append("<th scope='col' colspan=");
                        oHtml.Append(count1);
                        oHtml.Append(">");
                        if (i == 0)
                        {
                            oHtml.Append("All Docs");
                        }
                        else
                        {
                            oHtml.Append(title);
                            oHtml.Append(" ");
                            oHtml.Append((j - 1));
                        }
                        oHtml.Append("</th>");
                        j += 1;
                    }
                    else
                    {
                        oHtml.Append("<th>" + "All");
                        oHtml.Append(" ");
                        oHtml.Append(i);
                        oHtml.Append("</th>");
                        j += 1;
                    }

                }
                else
                {
                    if (title.StartsWith("Alt") == false)
                    {
                        oHtml.Append("<th>" + title);
                        oHtml.Append(" ");
                        oHtml.Append(i);
                        oHtml.Append("</th>");
                    }
                }
                i++;
            }
            sHtml = oHtml.ToString();
            oHtml = null;
            return sHtml;
        }
		public string writeNames(string description, string fullColCount)
		{
			string sHtml = string.Empty;
			if (description != string.Empty && description != null)
			{
				string[] arrNames = description.Split(DELIMITER_NAME);
				if (arrNames != null) 
				{
					StringBuilder oHtml = new StringBuilder();
					string sDisplayName = string.Empty;
					string sNewFileCountExtension = string.Empty;
					string sOldFileCountExtension = string.Empty;
					int iCount = arrNames.Length;
					int i = 0;

					int iCurrentColCount = 0;
					int j = 0;
					while(i != iCount)
					{
						//limited to 30 chars for displaying
						string sName = arrNames[i];
						GetValues(sName, out sDisplayName, out sNewFileCountExtension);
                        int iNewFileCountExtension = DisplayHelpers.GetIntValue(sNewFileCountExtension);
						//keep indentically numbered files (mult tp names) out of colcount and don't display
						if (sNewFileCountExtension.Equals(sOldFileCountExtension) == false)
						{
							//insert placeholder cols, so names line up with correct column
							for(j = iCurrentColCount; j < iNewFileCountExtension; j++)
							{
								oHtml.Append("<td>");
								oHtml.Append("</td>");
								iCurrentColCount += 1;
							}
							AppendName(sName, ref oHtml);
							iCurrentColCount += 1;
						}
						sOldFileCountExtension = sNewFileCountExtension;
						i++;
					}
					sHtml = oHtml.ToString();
					oHtml = null;
				}
				arrNames = null;
			}
			return sHtml;
		}
		
		private void AppendName(string name, ref StringBuilder html)
		{
			int iLength = name.Length;
			if (iLength > 30) iLength = 30;
			html.Append("<td>");
			html.Append(name.Substring(0, iLength));
			html.Append("</td>");
		}
        public string WriteMobileNames(string description, string fullColCount)
        {
            string sHtml = string.Empty;
            if (description != string.Empty && description != null)
            {
                string[] arrNames = description.Split(DELIMITER_NAME);
                if (arrNames != null)
                {
                    StringBuilder oHtml = new StringBuilder();
                    string sDisplayName = string.Empty;
                    string sNewFileCountExtension = string.Empty;
                    string sOldFileCountExtension = string.Empty;
                    int iCount = arrNames.Length;
                    int i = 0;

                    int iCurrentColCount = 0;
                    int j = 0;
                    bool bIsEvenNumber = true;
                    while (i != iCount)
                    {
                        //limited to 30 chars for displaying
                        string sName = arrNames[i];
                        GetValues(sName, out sDisplayName, out sNewFileCountExtension);
                        int iNewFileCountExtension = DisplayHelpers.GetIntValue(sNewFileCountExtension);
                        bool bIsEvenJNumber = true;
                        //keep identically numbered files (mult tp names) out of colcount and don't display
                        if (sNewFileCountExtension.Equals(sOldFileCountExtension) == false)
                        {
                            //insert placeholder cols, so names line up with correct column
                            for (j = iCurrentColCount; j < iNewFileCountExtension; j++)
                            {
                                if (bIsEvenJNumber)
                                {
                                    oHtml.Append("<div class='ui-block-a'>");
                                }
                                else
                                {
                                    oHtml.Append("<div class='ui-block-b'>");
                                }
                                oHtml.Append("</div>");
                                iCurrentColCount += 1;
                                bIsEvenJNumber = bIsEvenJNumber ? false : true;
                            }
                            AppendMobileName(bIsEvenNumber, i, sName, ref oHtml);
                            iCurrentColCount += 1;
                        }
                        sOldFileCountExtension = sNewFileCountExtension;
                        i++;
                        bIsEvenNumber = bIsEvenNumber ? false : true;
                    }
                    sHtml = oHtml.ToString();
                    oHtml = null;
                }
                arrNames = null;
            }
            return sHtml;
        }

        private void AppendMobileName(bool isEvenCol, int i, string name, ref StringBuilder html)
        {
            int iLength = name.Length;
            if (iLength > 30) iLength = 30;
            if (isEvenCol)
            {
                html.Append("<div class='ui-block-a'>Alt.");
            }
            else
            {
                html.Append("<div class='ui-block-b'>Alt.");
            }
            html.Append(i - 0);
            html.Append(": ");
            html.Append(name.Substring(0, iLength));
            html.Append("</div>");
        }
		//temporary array to hold all values
		public void initValues(string fullColCount)
		{
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
			int i = 0;
            //always init the arrays
            arrValues = new string[iFullColCount];
			if (iFullColCount > 0) 
			{
				while(i != (iFullColCount - 1))
				{
                    SetArraysValue(i, "empty");
					i++;
				}
			}
		}
		//temporary array to hold all values for NOP
		public void initValuesNOP(string fullColCount)
		{
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
			int j = 0;
			arrNOPs = new string[iFullColCount];
			while(j != (iFullColCount - 1))
			{
                SetArraysValue(j, "empty");
				j++;
			}
		}
		//print the net operating profits values
		public void printNOP(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0, attName.LastIndexOf("_") + 1) == "TAMR_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				//keep MEAN and STDDEV suffixes out
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    SetArraysValue(iPlot, attValue);
				}
			}
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMOC_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
					//stateful array used with remaining print nets
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print the net operating profits values for cost effectiveness analysis
		public void printNOP2(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0, attName.LastIndexOf("_") + 1) == "TAMR_MEAN_OUT_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				//keep MEAN and STDDEV suffixes out
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    SetArraysValue(iPlot, attValue);
				}
			}
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMOC_MEAN_REVENUE_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
					//stateful array used with remaining print nets
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print the net profits values
		public void printNProfits(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMAOH_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
					//stateful array used with remaining print nets
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print the net profits values
		public void printNProfits2(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMAOH_MEAN_REVENUE_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
					//stateful array used with remaining print nets
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print the nets including cash capital costs
		public void printNCAP(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMCAP_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print the nets including cash capital costs
		public void printNCAP2(string attName, string attValue)
		{

			string sPlot = string.Empty;
			int iPlot = 0;
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMCAP_MEAN_REVENUE_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
                    if (iLength > (iPlotStart + 3))
                    {
                        //_100
                        sPlot = attName.Substring(iPlotStart + 1, 3);
                    }
                    else
                    {
                        //_10
                        sPlot = attName.Substring(iPlotStart + 1, 2);
                    }
				} 
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
                if (DisplayHelpers.isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
                    double dbAttValue = DisplayHelpers.GetDoubleValue(attValue);
                    double dbPlotNOP = DisplayHelpers.GetDoubleValue(arrNOPs[iPlot]);
					double dbNOP = dbPlotNOP - dbAttValue;
                    SetArraysValue(iPlot, dbNOP.ToString("f2"));
				}
			}
		}
		//print values
		public void printValue(string attTest, string attName, string attValue)
		{
            if (attName.StartsWith(attTest))
            {
                string sPlot = string.Empty;
                int iPlot = 0;
                int iStart = 0;
                int iLength = 0;
                sPlot = GetExtensionNumber(attName, out iLength, out iStart);
                if (attName.Substring(0, iStart + 1) == attTest)
                {
                    if (DisplayHelpers.isNumber(sPlot))
                    {
                        iPlot = Convert.ToInt32(sPlot);
                        SetArraysValue(iPlot, attValue);
                    }
                }
            }
		}
		//add the column value to array
		public void printValue2(string attTest, string attName, string attValue, 
			string fileCount, string outputCount)
		{

			if (attName.StartsWith(attTest))
			{
				string sRootName = string.Empty;
				string sLastExtension = string.Empty;
				string sFirstExtension = string.Empty;
				int iIndex = 0;
				GetExtensions(attName, out sRootName, out sLastExtension, out iIndex);
				if(sRootName == attTest)
				{
					//attname = TAMR_MEAN_REVENUE_0
                    iIndex = Convert.ToInt32(sLastExtension);
                    SetArraysValue(iIndex, attValue);
				}
				else if(sRootName != string.Empty && sRootName != null)
				{
					string sRoot2Name = string.Empty;
					//remove the last "-";
					sRootName = sRootName.Remove(sRootName.Length - 1, 1);
					GetExtensions(sRootName, out sRoot2Name, out sFirstExtension, out iIndex);
					if(sRoot2Name == attTest)
					{
						//attname = TAMR_MEAN_REVENUE_0_1 (first extension is outputs, last is files)
						iIndex = GetIndex(fileCount, outputCount, sLastExtension, sFirstExtension);
                        SetArraysValue(iIndex, attValue);
					}
				}
			}
		}
		private void GetExtensions(string attName, out string rootName, out string extension, out int extensionNum)
		{
			rootName = string.Empty;
			extension = string.Empty;
			extensionNum = 0;
			int iStart = 0;
			int iLength = 0;
			extension = GetExtensionNumber(attName, out iLength, out iStart);
            if (DisplayHelpers.isNumber(extension))
			{
				extensionNum = Convert.ToInt32(extension);
				//returns Name_
				rootName = attName.Remove(iStart + 1, extension.Length);
			}
		}
		public int GetIndex(string fileCount, string outputCount, string fileExtension, string outputExtension)
		{
			int iFileCount = Convert.ToInt32(fileCount);
			int iOutputCount = Convert.ToInt32(outputCount);
			//account for the all columns
			int iFileExtension = Convert.ToInt32(fileExtension) + 1;
			int iOutputExtension = Convert.ToInt32(outputExtension);
			int iIndex = (iOutputCount * iFileExtension) + iOutputExtension;
			return iIndex;
		}
		public string GetExtensionNumber(string attName, out int length, out int start)
		{
			string sPlot = string.Empty;
			start = attName.LastIndexOf("_");
			length = attName.Length;
			if (length > (start + 2))
			{
                //allow up to 999 comparisons
                if (length > (start + 3))
                {
                    //_100; 999 comparison limitation
                    sPlot = attName.Substring(start + 1, 3);
                }
                else
                {
                    //_10; 99 comparison limitation
                    sPlot = attName.Substring(start + 1, 2);
                }
			}
			else 
			{
				//_1
				sPlot = attName.Substring(start + 1, 1);
			}
			return sPlot;
		}
		//print all column values
		public string doPrintValues(string fullColCount)
		{
            string sHtml = string.Empty;
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
            if (iFullColCount != 0)
            {
                //write a new table column with it's value
                int i = 0;
                StringBuilder oHtml = new StringBuilder();
                while (i != (iFullColCount - 1))
                {
                    oHtml.Append("<td>");
                    if (arrValues != null)
                    {
                        if (arrValues[i] == "empty")
                        {
                            oHtml.Append("0.00");
                        }
                        else
                        {
                            oHtml.Append(arrValues[i]);
                        }
                    }
                    oHtml.Append("</td>");
                    i++;
                }
                sHtml = oHtml.ToString();
                oHtml = null;
                //reset the arrays
                arrValues = null;
                arrNOPs = null;
            }
			return sHtml;
		}
        public string DoPrintMobileValues(string fullColCount)
        {
            string sHtml = string.Empty;
            int iFullColCount = DisplayHelpers.GetIntValue(fullColCount);
            bool bIsEvenNumber = true;
            if (iFullColCount != 0)
            {
                //write a new table column with it's value
                int i = 0;
                StringBuilder oHtml = new StringBuilder();
                while (i != (iFullColCount - 1))
                {
                    if (bIsEvenNumber)
                    {
                        oHtml.Append("<div class='ui-block-a'>");
                    }
                    else
                    {
                        oHtml.Append("<div class='ui-block-b'>");
                    }
                    oHtml.Append("Alt");
                    oHtml.Append(i);
                    oHtml.Append(": ");
                    if (arrValues != null)
                    {
                        if (arrValues[i] == "empty")
                        {
                            oHtml.Append("0.00");
                        }
                        else
                        {
                            oHtml.Append(arrValues[i]);
                        }
                    }
                    oHtml.Append("</div>");
                    i++;
                    bIsEvenNumber = bIsEvenNumber ? false : true;
                }
                sHtml = oHtml.ToString();
                oHtml = null;
                //reset the arrays
                arrValues = null;
                arrNOPs = null;
            }
            return sHtml;
        }
        public static void GetValues(string obsMember, out string member, out string fileCountExtension)
        {
            string sValue = obsMember;
            string sAttributeDelimiter = DataHelpers.FILENAME_DELIMITER;
            fileCountExtension = string.Empty;
            int iFileExtensionStart = obsMember.LastIndexOf(sAttributeDelimiter);
            if (iFileExtensionStart != -1)
            {
                sValue = obsMember.Substring(0, iFileExtensionStart);
                //return the integer expressions (do not include the underscore delimiter)
                fileCountExtension = obsMember.Substring(iFileExtensionStart + 1, obsMember.Length - iFileExtensionStart - 1);
            }
            member = sValue;
        }
        private void SetArraysValue(
            int arrayIndex, string arrayValue)
        {
            if (arrValues != null)
            {
                if (arrValues.Length > arrayIndex)
                {
                    arrValues[arrayIndex] = arrayValue;
                }
            }
        }
        private void SetNOPsValue(
            int arrayIndex, string arrayValue)
        {
            if (arrNOPs != null)
            {
                if (arrNOPs.Length > arrayIndex)
                {
                    arrNOPs[arrayIndex] = arrayValue;
                }
            }
        }
        
	}
}
