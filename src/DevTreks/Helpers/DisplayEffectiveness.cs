using System;
using System.Text;

namespace DevTreks.Helpers
{
    // <summary>
    ///Purpose:		Help xslt stylesheet display effectiveness analyses
    ///Author:		www.devtreks.org
    ///Original script author is: Gerardo A., ARS, SW Watershed Research Center
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
	public class DisplayEffectiveness
	{
		public DisplayEffectiveness()
		{
			//individual object 
		}
		private string[] arrValues = null;
		private string[] arrNOPs = null;
		private static char[] DELIMITER_NAME = new char[] {';'};

		public string writeTitles(string fullColCount)
		{
			int iFullColCount = (isNumber(fullColCount))? Convert.ToInt32(fullColCount) : 0;
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
						DisplayComparisons.GetValues(sName, out sDisplayName, out sNewFileCountExtension);
						int iNewFileCountExtension = (isNumber(sNewFileCountExtension))? Convert.ToInt32(sNewFileCountExtension) : 0;
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
		//temporary array to hold all values
		public void initValues(string fullColCount)
		{
			int iFullColCount = (isNumber(fullColCount))? Convert.ToInt32(fullColCount) : 0;
			int i = 0;
			//always init the arrays
			arrValues = new string[iFullColCount];
			while(i != (iFullColCount - 1))
			{
				arrValues[i] = "empty";
				i++;
			}
		}
		//temporary array to hold all values for NOP
		public void initValuesNOP(string fullColCount)
		{
			int iFullColCount = (isNumber(fullColCount))? Convert.ToInt32(fullColCount) : 0;
			int j = 0;
			arrNOPs = new string[iFullColCount];
			while(j != (iFullColCount - 1))
			{
				arrNOPs[j] = "empty";
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
					//_10
					sPlot = attName.Substring(iPlotStart + 1, 2);
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				//keep MEAN and STDDEV suffixes out
				if (isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
					arrNOPs[iPlot]  = attValue;
				}
			}
			if(attName.Substring(0,attName.LastIndexOf("_") + 1) == "TAMOC_")
			{
				int iPlotStart = attName.LastIndexOf("_");
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
					//_10
					sPlot = attName.Substring(iPlotStart + 1, 2);
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				if (isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
					double dbAttValue = (isNumber(attValue))? Convert.ToDouble(attValue) : 0;
					double dbPlotNOP = (isNumber(arrNOPs[iPlot]))? Convert.ToDouble(arrNOPs[iPlot]) : 0;
					double dbNOP = dbPlotNOP - dbAttValue;
					arrValues[iPlot] = dbNOP.ToString("f2");
					//stateful array used with remaining print nets
					arrNOPs[iPlot] = dbNOP.ToString("f2");
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
					//_10
					sPlot = attName.Substring(iPlotStart + 1, 2);
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				if (isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
					double dbAttValue = (isNumber(attValue))? Convert.ToDouble(attValue) : 0;
					double dbPlotNOP = (isNumber(arrNOPs[iPlot]) )? Convert.ToDouble(arrNOPs[iPlot]) : 0;
					double dbNOP = dbPlotNOP - dbAttValue;
					arrValues[iPlot] = dbNOP.ToString("f2");
					//stateful array used with remaining print nets
					arrNOPs[iPlot] = dbNOP.ToString("f2");
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
					//_10
					sPlot = attName.Substring(iPlotStart + 1, 2);
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				if (isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
					double dbAttValue = (isNumber(attValue))? Convert.ToDouble(attValue) : 0;
					double dbPlotNOP = (isNumber(arrNOPs[iPlot]) )? Convert.ToDouble(arrNOPs[iPlot]) : 0;
					double dbNOP = dbPlotNOP - dbAttValue;
					arrValues[iPlot] = dbNOP.ToString("f2");
				}
			}
		}
		//print the net profits values
		public void printValue(string attTest, string attName, string attValue)
		{
			string sPlot = string.Empty;
			int iPlot = 0;
			int iPlotStart = attName.LastIndexOf("_");
			if(attName.Substring(0, iPlotStart + 1) == attTest)
			{
				int iLength = attName.Length;
				if (iLength > (iPlotStart + 2))
				{
					//_10; 99 comparison limitation
					sPlot = attName.Substring(iPlotStart + 1, 2);
				}
				else 
				{
					//_1
					sPlot = attName.Substring(iPlotStart + 1, 1);
				}
				if (isNumber(sPlot))
				{
					iPlot = Convert.ToInt32(sPlot);
					arrValues[iPlot]  = attValue;
				}
			}
		}
		//print all values
		public string doPrintValues(string fullColCount)
		{
			int iFullColCount = (isNumber(fullColCount))? Convert.ToInt32(fullColCount) : 0;
			//write a new table column with it's value
			int i = 0;
			string sHtml = string.Empty;
			StringBuilder oHtml = new StringBuilder();
			while(i != (iFullColCount - 1))
			{
				oHtml.Append("<td>");
				if(arrValues[i] == "empty")
				{
					oHtml.Append("0.00");
				}
				else
				{
					oHtml.Append(arrValues[i]);
				}
				oHtml.Append("</td>");
				i++;
			}
			sHtml = oHtml.ToString();
			oHtml = null;
			//reset the arrays
			arrValues = null;
			arrNOPs = null;
			return sHtml;
		}
		private bool isNumber(string test)
		{
			bool bIsNumber = false;
			if (test != string.Empty && test != null)
			{ 
				if (test.StartsWith("0"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("1"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("2"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("3"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("4"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("5"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("6"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("7"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("8"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("9"))
				{
					bIsNumber = true;
					return bIsNumber;
				}
				else if (test.StartsWith("-"))
				{
					//negative number
					bIsNumber = true;
					return bIsNumber;
				}
			}
			return bIsNumber;
		}
	}
}

