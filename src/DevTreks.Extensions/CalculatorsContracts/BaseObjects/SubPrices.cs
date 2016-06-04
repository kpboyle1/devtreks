using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add sub- revenue, operating costs, allocated overhead costs, 
    ///             capital costs, environmental impacts, or environmental factors 
    ///             to inputs and outputs. 
    ///             Typical examples include contingencies, energy, water, water, 
    ///             repair in capital inputs. Enviromental impact examples include 
    ///             carbon and SO2 which are both traded in markets and may have 'prices'.
    ///             Environmental factors include garbage and food contaminants for food 
    ///             nutrition.
    ///Date:		2013, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        By convention subprices 1 to 5 are for either costs or revenues 
    ///             and 6 to 10 are for environmental impacts.
    /// </summary>      
    public class SubPrices
    {
        //constructor
        public SubPrices()
        {
            InitSubPricesProperties();
        }
        //copy constructors
        public SubPrices(SubPrices calculator)
        {
            CopySubPricesProperties(calculator);
        }
        //sub unit costs or revenues can be added 
        //to these price types, ui uses string version
        public enum PRICE_TYPES
        {
            rev     = 1,
            oc      = 2,
            aoh     = 3,
            cap     = 4
        }
        //severity of environmental impact, relative preference for food substitute
        //exceed or meet a quantitative standard
        public enum DEGREE_RATING
        {
            none    = 0,
            low     = 1,
            medium  = 2,
            high    = 3
        }
        //description for p1 to p5
        public string SubPDescription { get; set; }
        //p6 to p10
        public string NRDescription { get; set; }
        //name
        public string SubPName1 { get; set; }
        //aggregation label
        public string SubPLabel1 { get; set; }
        //PRICE_TYPES enum
        public string SubPType1 { get; set; }
        //NIST 135 lump sum fempUPV index 
        public double SubPFactor1 { get; set; }
        //planning construction phase fempUPV index
        public double SubPYears1 { get; set; }
        //amount
        public double SubPAmount1 { get; set; }
        //unit
        public string SubPUnit1 { get; set; }
        //price
        public double SubPPrice1 { get; set; }
        //escalation rate
        public double SubPEscRate1 { get; set; }
        //uniform, geometric, linear ...
        public string SubPEscType1 { get; set; }
        //total subcost (calculated)
        public double SubPTotal1 { get; set; }
        //total per unit subcost (unit amount passed in by parent) 
        //(calculated)
        public double SubPTotalPerUnit1 { get; set; }
        //factor rating: severity of env factor, relative degree of substituteability, perception of total cost
        public string SubPDegreeRating1 { get; set; }
        
        public string SubPName2 { get; set; }
        public string SubPLabel2 { get; set; }
        public string SubPType2 { get; set; }
        public double SubPFactor2 { get; set; }
        public double SubPYears2 { get; set; }
        public double SubPAmount2 { get; set; }
        public string SubPUnit2 { get; set; }
        public double SubPPrice2 { get; set; }
        public double SubPEscRate2 { get; set; }
        public string SubPEscType2 { get; set; }
        public double SubPTotal2 { get; set; }
        public double SubPTotalPerUnit2 { get; set; }
        public string SubPDegreeRating2 { get; set; }
        
        public string SubPName3 { get; set; }
        public string SubPLabel3 { get; set; }
        public string SubPType3 { get; set; }
        public double SubPFactor3 { get; set; }
        public double SubPYears3 { get; set; }
        public double SubPAmount3 { get; set; }
        public string SubPUnit3 { get; set; }
        public double SubPPrice3 { get; set; }
        public double SubPEscRate3 { get; set; }
        public string SubPEscType3 { get; set; }
        public double SubPTotal3 { get; set; }
        public double SubPTotalPerUnit3 { get; set; }
        public string SubPDegreeRating3 { get; set; }

        public string SubPName4 { get; set; }
        public string SubPLabel4 { get; set; }
        public string SubPType4 { get; set; }
        public double SubPFactor4 { get; set; }
        public double SubPYears4 { get; set; }
        public double SubPAmount4 { get; set; }
        public string SubPUnit4 { get; set; }
        public double SubPPrice4 { get; set; }
        public double SubPEscRate4 { get; set; }
        public string SubPEscType4 { get; set; }
        public double SubPTotal4 { get; set; }
        public double SubPTotalPerUnit4 { get; set; }
        public string SubPDegreeRating4 { get; set; }

        public string SubPName5 { get; set; }
        public string SubPLabel5 { get; set; }
        public string SubPType5 { get; set; }
        public double SubPFactor5 { get; set; }
        public double SubPYears5 { get; set; }
        public double SubPAmount5 { get; set; }
        public string SubPUnit5 { get; set; }
        public double SubPPrice5 { get; set; }
        public double SubPEscRate5 { get; set; }
        public string SubPEscType5 { get; set; }
        public double SubPTotal5 { get; set; }
        public double SubPTotalPerUnit5 { get; set; }
        public string SubPDegreeRating5 { get; set; }

        public string SubPName6 { get; set; }
        public string SubPLabel6 { get; set; }
        public string SubPType6 { get; set; }
        public double SubPFactor6 { get; set; }
        public double SubPYears6 { get; set; }
        public double SubPAmount6 { get; set; }
        public string SubPUnit6 { get; set; }
        public double SubPPrice6 { get; set; }
        public double SubPEscRate6 { get; set; }
        public string SubPEscType6 { get; set; }
        public double SubPTotal6 { get; set; }
        public double SubPTotalPerUnit6 { get; set; }
        public string SubPDegreeRating6 { get; set; }

        public string SubPName7 { get; set; }
        public string SubPLabel7 { get; set; }
        public string SubPType7 { get; set; }
        public double SubPFactor7 { get; set; }
        public double SubPYears7 { get; set; }
        public double SubPAmount7 { get; set; }
        public string SubPUnit7 { get; set; }
        public double SubPPrice7 { get; set; }
        public double SubPEscRate7 { get; set; }
        public string SubPEscType7 { get; set; }
        public double SubPTotal7 { get; set; }
        public double SubPTotalPerUnit7 { get; set; }
        public string SubPDegreeRating7 { get; set; }

        public string SubPName8 { get; set; }
        public string SubPLabel8 { get; set; }
        public string SubPType8 { get; set; }
        public double SubPFactor8 { get; set; }
        public double SubPYears8 { get; set; }
        public double SubPAmount8 { get; set; }
        public string SubPUnit8 { get; set; }
        public double SubPPrice8 { get; set; }
        public double SubPEscRate8 { get; set; }
        public string SubPEscType8 { get; set; }
        public double SubPTotal8 { get; set; }
        public double SubPTotalPerUnit8 { get; set; }
        public string SubPDegreeRating8 { get; set; }

        public string SubPName9 { get; set; }
        public string SubPLabel9 { get; set; }
        public string SubPType9 { get; set; }
        public double SubPFactor9 { get; set; }
        public double SubPYears9 { get; set; }
        public double SubPAmount9 { get; set; }
        public string SubPUnit9 { get; set; }
        public double SubPPrice9 { get; set; }
        public double SubPEscRate9 { get; set; }
        public string SubPEscType9 { get; set; }
        public double SubPTotal9 { get; set; }
        public double SubPTotalPerUnit9 { get; set; }
        public string SubPDegreeRating9 { get; set; }

        public string SubPName10 { get; set; }
        public string SubPLabel10 { get; set; }
        public string SubPType10 { get; set; }
        public double SubPFactor10 { get; set; }
        public double SubPYears10 { get; set; }
        public double SubPAmount10 { get; set; }
        public string SubPUnit10 { get; set; }
        public double SubPPrice10 { get; set; }
        public double SubPEscRate10 { get; set; }
        public string SubPEscType10 { get; set; }
        public double SubPTotal10 { get; set; }
        public double SubPTotalPerUnit10 { get; set; }
        public string SubPDegreeRating10 { get; set; }

        private const string cSubPDescription = "SubPDescription";
        private const string cNRDescription = "NRDescription";
        private const string cSubPName1 = "SubPName1";
        private const string cSubPLabel1 = "SubPLabel1";
        private const string cSubPType1 = "SubPType1";
        private const string cSubPFactor1 = "SubPFactor1";
        private const string cSubPYears1 = "SubPYears1";
        private const string cSubPAmount1 = "SubPAmount1";
        private const string cSubPUnit1 = "SubPUnit1";
        private const string cSubPPrice1 = "SubPPrice1";
        private const string cSubPEscRate1 = "SubPEscRate1";
        private const string cSubPEscType1 = "SubPEscType1";
        private const string cSubPTotal1 = "SubPTotal1";
        private const string cSubPTotalPerUnit1 = "SubPTotalPerUnit1";  
        private const string cSubPDegreeRating1 = "SubPDegreeRating1"; 

        private const string cSubPName2 = "SubPName2";
        private const string cSubPLabel2 = "SubPLabel2";
        private const string cSubPType2 = "SubPType2";
        private const string cSubPAmount2 = "SubPAmount2";
        private const string cSubPUnit2 = "SubPUnit2";
        private const string cSubPPrice2 = "SubPPrice2";
        private const string cSubPFactor2 = "SubPFactor2";
        private const string cSubPYears2 = "SubPYears2";
        private const string cSubPEscRate2 = "SubPEscRate2";
        private const string cSubPEscType2 = "SubPEscType2";
        private const string cSubPTotal2 = "SubPTotal2";
        private const string cSubPTotalPerUnit2 = "SubPTotalPerUnit2";
        private const string cSubPDegreeRating2 = "SubPDegreeRating2"; 

        private const string cSubPName3 = "SubPName3";
        private const string cSubPLabel3 = "SubPLabel3";
        private const string cSubPType3 = "SubPType3";
        private const string cSubPFactor3 = "SubPFactor3";
        private const string cSubPYears3 = "SubPYears3";
        private const string cSubPAmount3 = "SubPAmount3";
        private const string cSubPUnit3 = "SubPUnit3";
        private const string cSubPPrice3 = "SubPPrice3";
        private const string cSubPEscRate3 = "SubPEscRate3";
        private const string cSubPEscType3 = "SubPEscType3";
        private const string cSubPTotal3 = "SubPTotal3";
        private const string cSubPTotalPerUnit3 = "SubPTotalPerUnit3";
        private const string cSubPDegreeRating3 = "SubPDegreeRating3"; 

        private const string cSubPName4 = "SubPName4";
        private const string cSubPLabel4 = "SubPLabel4";
        private const string cSubPType4 = "SubPType4";
        private const string cSubPFactor4 = "SubPFactor4";
        private const string cSubPYears4 = "SubPYears4";
        private const string cSubPAmount4 = "SubPAmount4";
        private const string cSubPUnit4 = "SubPUnit4";
        private const string cSubPPrice4 = "SubPPrice4";
        private const string cSubPEscRate4 = "SubPEscRate4";
        private const string cSubPEscType4 = "SubPEscType4";
        private const string cSubPTotal4 = "SubPTotal4";
        private const string cSubPTotalPerUnit4 = "SubPTotalPerUnit4";
        private const string cSubPDegreeRating4 = "SubPDegreeRating4"; 

        private const string cSubPName5 = "SubPName5";
        private const string cSubPLabel5 = "SubPLabel5";
        private const string cSubPType5 = "SubPType5";
        private const string cSubPFactor5 = "SubPFactor5";
        private const string cSubPYears5 = "SubPYears5";
        private const string cSubPAmount5 = "SubPAmount5";
        private const string cSubPUnit5 = "SubPUnit5";
        private const string cSubPPrice5 = "SubPPrice5";
        private const string cSubPEscRate5 = "SubPEscRate5";
        private const string cSubPEscType5 = "SubPEscType5";
        private const string cSubPTotal5 = "SubPTotal5";
        private const string cSubPTotalPerUnit5 = "SubPTotalPerUnit5";
        private const string cSubPDegreeRating5 = "SubPDegreeRating5"; 

        private const string cSubPName6 = "SubPName6";
        private const string cSubPLabel6 = "SubPLabel6";
        private const string cSubPType6 = "SubPType6";
        private const string cSubPFactor6 = "SubPFactor6";
        private const string cSubPYears6 = "SubPYears6";
        private const string cSubPAmount6 = "SubPAmount6";
        private const string cSubPUnit6 = "SubPUnit6";
        private const string cSubPPrice6 = "SubPPrice6";
        private const string cSubPEscRate6 = "SubPEscRate6";
        private const string cSubPEscType6 = "SubPEscType6";
        private const string cSubPTotal6 = "SubPTotal6";
        private const string cSubPTotalPerUnit6 = "SubPTotalPerUnit6";
        private const string cSubPDegreeRating6 = "SubPDegreeRating6"; 

        private const string cSubPName7 = "SubPName7";
        private const string cSubPLabel7 = "SubPLabel7";
        private const string cSubPType7 = "SubPType7";
        private const string cSubPFactor7 = "SubPFactor7";
        private const string cSubPYears7 = "SubPYears7";
        private const string cSubPAmount7 = "SubPAmount7";
        private const string cSubPUnit7 = "SubPUnit7";
        private const string cSubPPrice7 = "SubPPrice7";
        private const string cSubPEscRate7 = "SubPEscRate7";
        private const string cSubPEscType7 = "SubPEscType7";
        private const string cSubPTotal7 = "SubPTotal7";
        private const string cSubPTotalPerUnit7 = "SubPTotalPerUnit7";
        private const string cSubPDegreeRating7 = "SubPDegreeRating7"; 

        private const string cSubPName8 = "SubPName8";
        private const string cSubPLabel8 = "SubPLabel8";
        private const string cSubPType8 = "SubPType8";
        private const string cSubPFactor8 = "SubPFactor8";
        private const string cSubPYears8 = "SubPYears8";
        private const string cSubPAmount8 = "SubPAmount8";
        private const string cSubPUnit8 = "SubPUnit8";
        private const string cSubPPrice8 = "SubPPrice8";
        private const string cSubPEscRate8 = "SubPEscRate8";
        private const string cSubPEscType8 = "SubPEscType8";
        private const string cSubPTotal8 = "SubPTotal8";
        private const string cSubPTotalPerUnit8 = "SubPTotalPerUnit8";
        private const string cSubPDegreeRating8 = "SubPDegreeRating8"; 

        private const string cSubPName9 = "SubPName9";
        private const string cSubPLabel9 = "SubPLabel9";
        private const string cSubPType9 = "SubPType9";
        private const string cSubPFactor9 = "SubPFactor9";
        private const string cSubPYears9 = "SubPYears9";
        private const string cSubPAmount9 = "SubPAmount9";
        private const string cSubPUnit9 = "SubPUnit9";
        private const string cSubPPrice9 = "SubPPrice9";
        private const string cSubPEscRate9 = "SubPEscRate9";
        private const string cSubPEscType9 = "SubPEscType9";
        private const string cSubPTotal9 = "SubPTotal9";
        private const string cSubPTotalPerUnit9 = "SubPTotalPerUnit9";
        private const string cSubPDegreeRating9 = "SubPDegreeRating9"; 

        private const string cSubPName10 = "SubPName10";
        private const string cSubPLabel10 = "SubPLabel10";
        private const string cSubPType10 = "SubPType10";
        private const string cSubPFactor10 = "SubPFactor10";
        private const string cSubPYears10 = "SubPYears10";
        private const string cSubPAmount10 = "SubPAmount10";
        private const string cSubPUnit10 = "SubPUnit10";
        private const string cSubPPrice10 = "SubPPrice10";
        private const string cSubPEscRate10 = "SubPEscRate10";
        private const string cSubPEscType10 = "SubPEscType10";
        private const string cSubPTotal10 = "SubPTotal10";
        private const string cSubPTotalPerUnit10 = "SubPTotalPerUnit10";
        private const string cSubPDegreeRating10 = "SubPDegreeRating10"; 

        public virtual void InitSubPricesProperties()
        {
            this.SubPDescription = string.Empty;
            this.NRDescription = string.Empty;
            this.SubPName1 = string.Empty;
            this.SubPLabel1 = string.Empty;
            this.SubPType1 = string.Empty;
            this.SubPFactor1 = 0;
            this.SubPYears1 = 0;
            this.SubPAmount1 = 0;
            this.SubPUnit1 = string.Empty;
            this.SubPPrice1 = 0;
            this.SubPEscRate1 = 0;
            this.SubPEscType1 = string.Empty;
            this.SubPTotal1 = 0;
            this.SubPTotalPerUnit1 = 0;
            this.SubPDegreeRating1 = DEGREE_RATING.none.ToString();
            this.SubPFactor1 = 0;
            this.SubPName2 = string.Empty;
            this.SubPLabel2 = string.Empty;
            this.SubPType2 = string.Empty;
            this.SubPFactor2 = 0;
            this.SubPYears2 = 0;
            this.SubPAmount2 = 0;
            this.SubPUnit2 = string.Empty;
            this.SubPPrice2 = 0;
            this.SubPEscRate2 = 0;
            this.SubPEscType2 = string.Empty;
            this.SubPTotal2 = 0;
            this.SubPTotalPerUnit2 = 0;
            this.SubPDegreeRating2 = DEGREE_RATING.none.ToString();
            this.SubPName3 = string.Empty;
            this.SubPLabel3 = string.Empty;
            this.SubPType3 = string.Empty;
            this.SubPAmount3 = 0;
            this.SubPUnit3 = string.Empty;
            this.SubPPrice3 = 0;
            this.SubPFactor3 = 0;
            this.SubPYears3 = 0;
            this.SubPEscRate3 = 0;
            this.SubPEscType3 = string.Empty;
            this.SubPTotal3 = 0;
            this.SubPTotalPerUnit3 = 0;
            this.SubPDegreeRating3 = DEGREE_RATING.none.ToString();

            this.SubPName4 = string.Empty;
            this.SubPLabel4 = string.Empty;
            this.SubPType4 = string.Empty;
            this.SubPAmount4 = 0;
            this.SubPUnit4 = string.Empty;
            this.SubPPrice4 = 0;
            this.SubPFactor4 = 0;
            this.SubPYears4 = 0;
            this.SubPEscRate4 = 0;
            this.SubPEscType4 = string.Empty;
            this.SubPTotal4 = 0;
            this.SubPTotalPerUnit4 = 0;
            this.SubPDegreeRating4 = DEGREE_RATING.none.ToString();

            this.SubPName5 = string.Empty;
            this.SubPLabel5 = string.Empty;
            this.SubPType5 = string.Empty;
            this.SubPAmount5 = 0;
            this.SubPUnit5 = string.Empty;
            this.SubPPrice5 = 0;
            this.SubPFactor5 = 0;
            this.SubPYears5 = 0;
            this.SubPEscRate5 = 0;
            this.SubPEscType5 = string.Empty;
            this.SubPTotal5 = 0;
            this.SubPTotalPerUnit5 = 0;
            this.SubPDegreeRating5 = DEGREE_RATING.none.ToString();

            this.SubPName6 = string.Empty;
            this.SubPLabel6 = string.Empty;
            this.SubPType6 = string.Empty;
            this.SubPAmount6 = 0;
            this.SubPUnit6 = string.Empty;
            this.SubPPrice6 = 0;
            this.SubPFactor6 = 0;
            this.SubPYears6 = 0;
            this.SubPEscRate6 = 0;
            this.SubPEscType6 = string.Empty;
            this.SubPTotal6 = 0;
            this.SubPTotalPerUnit6 = 0;
            this.SubPDegreeRating6 = DEGREE_RATING.none.ToString();

            this.SubPName7 = string.Empty;
            this.SubPLabel7 = string.Empty;
            this.SubPType7 = string.Empty;
            this.SubPAmount7 = 0;
            this.SubPUnit7 = string.Empty;
            this.SubPPrice7 = 0;
            this.SubPFactor7 = 0;
            this.SubPYears7 = 0;
            this.SubPEscRate7 = 0;
            this.SubPEscType7 = string.Empty;
            this.SubPTotal7 = 0;
            this.SubPTotalPerUnit7 = 0;
            this.SubPDegreeRating7 = DEGREE_RATING.none.ToString();

            this.SubPName8 = string.Empty;
            this.SubPLabel8 = string.Empty;
            this.SubPType8 = string.Empty;
            this.SubPAmount8 = 0;
            this.SubPUnit8 = string.Empty;
            this.SubPPrice8 = 0;
            this.SubPFactor8 = 0;
            this.SubPYears8 = 0;
            this.SubPEscRate8 = 0;
            this.SubPEscType8 = string.Empty;
            this.SubPTotal8 = 0;
            this.SubPTotalPerUnit8 = 0;
            this.SubPDegreeRating8 = DEGREE_RATING.none.ToString();

            this.SubPName9 = string.Empty;
            this.SubPLabel9 = string.Empty;
            this.SubPType9 = string.Empty;
            this.SubPAmount9 = 0;
            this.SubPUnit9 = string.Empty;
            this.SubPPrice9 = 0;
            this.SubPFactor9 = 0;
            this.SubPYears9 = 0;
            this.SubPEscRate9 = 0;
            this.SubPEscType9 = string.Empty;
            this.SubPTotal9 = 0;
            this.SubPTotalPerUnit9 = 0;
            this.SubPDegreeRating9 = DEGREE_RATING.none.ToString();

            this.SubPName10 = string.Empty;
            this.SubPLabel10 = string.Empty;
            this.SubPType10 = string.Empty;
            this.SubPAmount10 = 0;
            this.SubPUnit10 = string.Empty;
            this.SubPPrice10 = 0;
            this.SubPFactor10 = 0;
            this.SubPYears10 = 0;
            this.SubPEscRate10 = 0;
            this.SubPEscType10 = string.Empty;
            this.SubPTotal10 = 0;
            this.SubPTotalPerUnit10 = 0;
            this.SubPDegreeRating10 = DEGREE_RATING.none.ToString();
        }
        public virtual void CopySubPricesProperties(
            SubPrices calculator)
        {
            this.SubPDescription = calculator.SubPDescription;
            this.NRDescription = calculator.NRDescription;
            this.SubPName1 = calculator.SubPName1;
            this.SubPLabel1 = calculator.SubPLabel1;
            this.SubPType1 = calculator.SubPType1;
            this.SubPFactor1 = calculator.SubPFactor1;
            this.SubPYears1 = calculator.SubPYears1;
            this.SubPAmount1 = calculator.SubPAmount1;
            this.SubPUnit1 = calculator.SubPUnit1;
            this.SubPPrice1 = calculator.SubPPrice1;
            this.SubPEscRate1 = calculator.SubPEscRate1;
            this.SubPEscType1 = calculator.SubPEscType1;
            this.SubPTotal1 = calculator.SubPTotal1;
            this.SubPTotalPerUnit1 = calculator.SubPTotalPerUnit1;
            this.SubPDegreeRating1 = calculator.SubPDegreeRating1;
            this.SubPName2 = calculator.SubPName2;
            this.SubPLabel2 = calculator.SubPLabel2;
            this.SubPType2 = calculator.SubPType2;
            this.SubPFactor2 = calculator.SubPFactor2;
            this.SubPYears2 = calculator.SubPYears2;
            this.SubPAmount2 = calculator.SubPAmount2;
            this.SubPUnit2 = calculator.SubPUnit2;
            this.SubPPrice2 = calculator.SubPPrice2;
            this.SubPEscRate2 = calculator.SubPEscRate2;
            this.SubPEscType2 = calculator.SubPEscType2;
            this.SubPTotal2 = calculator.SubPTotal2;
            this.SubPTotalPerUnit2 = calculator.SubPTotalPerUnit2;
            this.SubPDegreeRating2 = calculator.SubPDegreeRating2;
            this.SubPName3 = calculator.SubPName3;
            this.SubPLabel3 = calculator.SubPLabel3;
            this.SubPType3 = calculator.SubPType3;
            this.SubPFactor3 = calculator.SubPFactor3;
            this.SubPYears3 = calculator.SubPYears3;
            this.SubPAmount3 = calculator.SubPAmount3;
            this.SubPUnit3 = calculator.SubPUnit3;
            this.SubPPrice3 = calculator.SubPPrice3;
            this.SubPTotal3 = calculator.SubPTotal3;
            this.SubPEscRate3 = calculator.SubPEscRate3;
            this.SubPEscType3 = calculator.SubPEscType3;
            this.SubPTotal3 = calculator.SubPTotal3;
            this.SubPTotalPerUnit3 = calculator.SubPTotalPerUnit3;
            this.SubPDegreeRating3 = calculator.SubPDegreeRating3;

            this.SubPName4 = calculator.SubPName4;
            this.SubPLabel4 = calculator.SubPLabel4;
            this.SubPType4 = calculator.SubPType4;
            this.SubPFactor4 = calculator.SubPFactor4;
            this.SubPYears4 = calculator.SubPYears4;
            this.SubPAmount4 = calculator.SubPAmount4;
            this.SubPUnit4 = calculator.SubPUnit4;
            this.SubPPrice4 = calculator.SubPPrice4;
            this.SubPTotal4 = calculator.SubPTotal4;
            this.SubPEscRate4 = calculator.SubPEscRate4;
            this.SubPEscType4 = calculator.SubPEscType4;
            this.SubPTotal4 = calculator.SubPTotal4;
            this.SubPTotalPerUnit4 = calculator.SubPTotalPerUnit4;
            this.SubPDegreeRating4 = calculator.SubPDegreeRating4;

            this.SubPName5 = calculator.SubPName5;
            this.SubPLabel5 = calculator.SubPLabel5;
            this.SubPType5 = calculator.SubPType5;
            this.SubPFactor5 = calculator.SubPFactor5;
            this.SubPYears5 = calculator.SubPYears5;
            this.SubPAmount5 = calculator.SubPAmount5;
            this.SubPUnit5 = calculator.SubPUnit5;
            this.SubPPrice5 = calculator.SubPPrice5;
            this.SubPTotal5 = calculator.SubPTotal5;
            this.SubPEscRate5 = calculator.SubPEscRate5;
            this.SubPEscType5 = calculator.SubPEscType5;
            this.SubPTotal5 = calculator.SubPTotal5;
            this.SubPTotalPerUnit5 = calculator.SubPTotalPerUnit5;
            this.SubPDegreeRating5 = calculator.SubPDegreeRating5;

            this.SubPName6 = calculator.SubPName6;
            this.SubPLabel6 = calculator.SubPLabel6;
            this.SubPType6 = calculator.SubPType6;
            this.SubPFactor6 = calculator.SubPFactor6;
            this.SubPYears6 = calculator.SubPYears6;
            this.SubPAmount6 = calculator.SubPAmount6;
            this.SubPUnit6 = calculator.SubPUnit6;
            this.SubPPrice6 = calculator.SubPPrice6;
            this.SubPTotal6 = calculator.SubPTotal6;
            this.SubPEscRate6 = calculator.SubPEscRate6;
            this.SubPEscType6 = calculator.SubPEscType6;
            this.SubPTotal6 = calculator.SubPTotal6;
            this.SubPTotalPerUnit6 = calculator.SubPTotalPerUnit6;
            this.SubPDegreeRating6 = calculator.SubPDegreeRating6;

            this.SubPName7 = calculator.SubPName7;
            this.SubPLabel7 = calculator.SubPLabel7;
            this.SubPType7 = calculator.SubPType7;
            this.SubPFactor7 = calculator.SubPFactor7;
            this.SubPYears7 = calculator.SubPYears7;
            this.SubPAmount7 = calculator.SubPAmount7;
            this.SubPUnit7 = calculator.SubPUnit7;
            this.SubPPrice7 = calculator.SubPPrice7;
            this.SubPTotal7 = calculator.SubPTotal7;
            this.SubPEscRate7 = calculator.SubPEscRate7;
            this.SubPEscType7 = calculator.SubPEscType7;
            this.SubPTotal7 = calculator.SubPTotal7;
            this.SubPTotalPerUnit7 = calculator.SubPTotalPerUnit7;
            this.SubPDegreeRating7 = calculator.SubPDegreeRating7;

            this.SubPName8 = calculator.SubPName8;
            this.SubPLabel8 = calculator.SubPLabel8;
            this.SubPType8 = calculator.SubPType8;
            this.SubPFactor8 = calculator.SubPFactor8;
            this.SubPYears8 = calculator.SubPYears8;
            this.SubPAmount8 = calculator.SubPAmount8;
            this.SubPUnit8 = calculator.SubPUnit8;
            this.SubPPrice8 = calculator.SubPPrice8;
            this.SubPTotal8 = calculator.SubPTotal8;
            this.SubPEscRate8 = calculator.SubPEscRate8;
            this.SubPEscType8 = calculator.SubPEscType8;
            this.SubPTotal8 = calculator.SubPTotal8;
            this.SubPTotalPerUnit8 = calculator.SubPTotalPerUnit8;
            this.SubPDegreeRating8 = calculator.SubPDegreeRating8;

            this.SubPName9 = calculator.SubPName9;
            this.SubPLabel9 = calculator.SubPLabel9;
            this.SubPType9 = calculator.SubPType9;
            this.SubPFactor9 = calculator.SubPFactor9;
            this.SubPYears9 = calculator.SubPYears9;
            this.SubPAmount9 = calculator.SubPAmount9;
            this.SubPUnit9 = calculator.SubPUnit9;
            this.SubPPrice9 = calculator.SubPPrice9;
            this.SubPTotal9 = calculator.SubPTotal9;
            this.SubPEscRate9 = calculator.SubPEscRate9;
            this.SubPEscType9 = calculator.SubPEscType9;
            this.SubPTotal9 = calculator.SubPTotal9;
            this.SubPTotalPerUnit9 = calculator.SubPTotalPerUnit9;
            this.SubPDegreeRating9 = calculator.SubPDegreeRating9;

            this.SubPName10 = calculator.SubPName10;
            this.SubPLabel10 = calculator.SubPLabel10;
            this.SubPType10 = calculator.SubPType10;
            this.SubPFactor10 = calculator.SubPFactor10;
            this.SubPYears10 = calculator.SubPYears10;
            this.SubPAmount10 = calculator.SubPAmount10;
            this.SubPUnit10 = calculator.SubPUnit10;
            this.SubPPrice10 = calculator.SubPPrice10;
            this.SubPTotal10 = calculator.SubPTotal10;
            this.SubPEscRate10 = calculator.SubPEscRate10;
            this.SubPEscType10 = calculator.SubPEscType10;
            this.SubPTotal10 = calculator.SubPTotal10;
            this.SubPTotalPerUnit10 = calculator.SubPTotalPerUnit10;
            this.SubPDegreeRating10 = calculator.SubPDegreeRating10;
        }
        public virtual void SetSubPricesProperties(XElement calculator)
        {
            //set this object's properties
            this.SubPDescription = CalculatorHelpers.GetAttribute(calculator,
               cSubPDescription);
            this.NRDescription = CalculatorHelpers.GetAttribute(calculator,
               cNRDescription);
            this.SubPName1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName1);
            this.SubPLabel1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel1);
            this.SubPType1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType1);
            this.SubPAmount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount1);
            this.SubPUnit1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit1);
            this.SubPPrice1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice1);
            this.SubPFactor1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor1);
            this.SubPYears1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears1);
            this.SubPEscRate1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate1);
            this.SubPEscType1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType1);
            this.SubPTotal1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal1);
            this.SubPTotalPerUnit1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit1);
            this.SubPDegreeRating1 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating1);
            this.SubPName2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName2);
            this.SubPLabel2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel2);
            this.SubPType2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType2);
            this.SubPFactor2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor2);
            this.SubPYears2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears2);
            this.SubPAmount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount2);
            this.SubPUnit2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit2);
            this.SubPPrice2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice2);
            this.SubPEscRate2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate2);
            this.SubPEscType2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType2);
            this.SubPTotal2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal2);
            this.SubPTotalPerUnit2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit2);
            this.SubPDegreeRating2 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating2);
            this.SubPName3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName3);
            this.SubPLabel3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel3);
            this.SubPType3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType3);
            this.SubPFactor3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor3);
            this.SubPYears3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears3);
            this.SubPAmount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount3);
            this.SubPUnit3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit3);
            this.SubPPrice3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice3);
            this.SubPEscRate3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate3);
            this.SubPEscType3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType3);
            this.SubPTotal3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal3);
            this.SubPTotalPerUnit3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit3);
            this.SubPDegreeRating3 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating3);

            this.SubPName4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName4);
            this.SubPLabel4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel4);
            this.SubPType4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType4);
            this.SubPFactor4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor4);
            this.SubPYears4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears4);
            this.SubPAmount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount4);
            this.SubPUnit4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit4);
            this.SubPPrice4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice4);
            this.SubPEscRate4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate4);
            this.SubPEscType4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType4);
            this.SubPTotal4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal4);
            this.SubPTotalPerUnit4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit4);
            this.SubPDegreeRating4 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating4);

            this.SubPName5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName5);
            this.SubPLabel5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel5);
            this.SubPType5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType5);
            this.SubPFactor5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor5);
            this.SubPYears5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears5);
            this.SubPAmount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount5);
            this.SubPUnit5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit5);
            this.SubPPrice5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice5);
            this.SubPEscRate5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate5);
            this.SubPEscType5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType5);
            this.SubPTotal5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal5);
            this.SubPTotalPerUnit5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit5);
            this.SubPDegreeRating5 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating5);

            this.SubPName6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName6);
            this.SubPLabel6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel6);
            this.SubPType6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType6);
            this.SubPFactor6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor6);
            this.SubPYears6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears6);
            this.SubPAmount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount6);
            this.SubPUnit6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit6);
            this.SubPPrice6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice6);
            this.SubPEscRate6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate6);
            this.SubPEscType6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType6);
            this.SubPTotal6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal6);
            this.SubPTotalPerUnit6 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit6);
            this.SubPDegreeRating6 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating6);

            this.SubPName7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName7);
            this.SubPLabel7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel7);
            this.SubPType7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType7);
            this.SubPFactor7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor7);
            this.SubPYears7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears7);
            this.SubPAmount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount7);
            this.SubPUnit7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit7);
            this.SubPPrice7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice7);
            this.SubPEscRate7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate7);
            this.SubPEscType7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType7);
            this.SubPTotal7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal7);
            this.SubPTotalPerUnit7 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit7);
            this.SubPDegreeRating7 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating7);

            this.SubPName8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName8);
            this.SubPLabel8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel8);
            this.SubPType8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType8);
            this.SubPFactor8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor8);
            this.SubPYears8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears8);
            this.SubPAmount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount8);
            this.SubPUnit8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit8);
            this.SubPPrice8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice8);
            this.SubPEscRate8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate8);
            this.SubPEscType8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType8);
            this.SubPTotal8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal8);
            this.SubPTotalPerUnit8 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit8);
            this.SubPDegreeRating8 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating8);

            this.SubPName9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName9);
            this.SubPLabel9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel9);
            this.SubPType9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType9);
            this.SubPFactor9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor9);
            this.SubPYears9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears9);
            this.SubPAmount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount9);
            this.SubPUnit9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit9);
            this.SubPPrice9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice9);
            this.SubPEscRate9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate9);
            this.SubPEscType9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType9);
            this.SubPTotal9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal9);
            this.SubPTotalPerUnit9 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit9);
            this.SubPDegreeRating9 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating9);

            this.SubPName10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPName10);
            this.SubPLabel10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPLabel10);
            this.SubPType10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPType10);
            this.SubPFactor10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPFactor10);
            this.SubPYears10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPYears10);
            this.SubPAmount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPAmount10);
            this.SubPUnit10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPUnit10);
            this.SubPPrice10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPPrice10);
            this.SubPEscRate10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPEscRate10);
            this.SubPEscType10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPEscType10);
            this.SubPTotal10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotal10);
            this.SubPTotalPerUnit10 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSubPTotalPerUnit10);
            this.SubPDegreeRating10 = CalculatorHelpers.GetAttribute(calculator,
               cSubPDegreeRating10);

        }
        public virtual void SetSubPricesProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cSubPDescription:
                    this.SubPDescription = attValue;
                    break;
                case cNRDescription:
                    this.NRDescription = attValue;
                    break;
                case cSubPName1:
                    this.SubPName1 = attValue;
                    break;
                case cSubPLabel1:
                    this.SubPLabel1 = attValue;
                    break;
                case cSubPType1:
                    this.SubPType1 = attValue;
                    break;
                case cSubPFactor1:
                    this.SubPFactor1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears1:
                    this.SubPYears1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount1:
                    this.SubPAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit1:
                    this.SubPUnit1 = attValue;
                    break;
                case cSubPPrice1:
                    this.SubPPrice1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate1:
                    this.SubPEscRate1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType1:
                    this.SubPEscType1 = attValue;
                    break;
                case cSubPTotal1:
                    this.SubPTotal1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit1:
                    this.SubPTotalPerUnit1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating1:
                    this.SubPDegreeRating1 = attValue;
                    break;
                case cSubPName2:
                    this.SubPName2 = attValue;
                    break;
                case cSubPLabel2:
                    this.SubPLabel2 = attValue;
                    break;
                case cSubPType2:
                    this.SubPType2 = attValue;
                    break;
                case cSubPFactor2:
                    this.SubPFactor2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears2:
                    this.SubPYears2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount2:
                    this.SubPAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit2:
                    this.SubPUnit2 = attValue;
                    break;
                case cSubPPrice2:
                    this.SubPPrice2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate2:
                    this.SubPEscRate2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType2:
                    this.SubPEscType2 = attValue;
                    break;
                case cSubPTotal2:
                    this.SubPTotal2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit2:
                    this.SubPTotalPerUnit2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating2:
                    this.SubPDegreeRating2 = attValue;
                    break;
                case cSubPName3:
                    this.SubPName3 = attValue;
                    break;
                case cSubPLabel3:
                    this.SubPLabel3 = attValue;
                    break;
                case cSubPType3:
                    this.SubPType3 = attValue;
                    break;
                case cSubPFactor3:
                    this.SubPFactor3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears3:
                    this.SubPYears3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount3:
                    this.SubPAmount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit3:
                    this.SubPUnit3 = attValue;
                    break;
                case cSubPPrice3:
                    this.SubPPrice3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate3:
                    this.SubPEscRate3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType3:
                    this.SubPEscType3 = attValue;
                    break;
                case cSubPTotal3:
                    this.SubPTotal3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit3:
                    this.SubPTotalPerUnit3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating3:
                    this.SubPDegreeRating3 = attValue;
                    break;
                case cSubPName4:
                    this.SubPName4 = attValue;
                    break;
                case cSubPLabel4:
                    this.SubPLabel4 = attValue;
                    break;
                case cSubPType4:
                    this.SubPType4 = attValue;
                    break;
                case cSubPFactor4:
                    this.SubPFactor4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears4:
                    this.SubPYears4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount4:
                    this.SubPAmount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit4:
                    this.SubPUnit4 = attValue;
                    break;
                case cSubPPrice4:
                    this.SubPPrice4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate4:
                    this.SubPEscRate4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType4:
                    this.SubPEscType4 = attValue;
                    break;
                case cSubPTotal4:
                    this.SubPTotal4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit4:
                    this.SubPTotalPerUnit4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating4:
                    this.SubPDegreeRating4 = attValue;
                    break;
                case cSubPName5:
                    this.SubPName5 = attValue;
                    break;
                case cSubPLabel5:
                    this.SubPLabel5 = attValue;
                    break;
                case cSubPType5:
                    this.SubPType5 = attValue;
                    break;
                case cSubPFactor5:
                    this.SubPFactor5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears5:
                    this.SubPYears5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount5:
                    this.SubPAmount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit5:
                    this.SubPUnit5 = attValue;
                    break;
                case cSubPPrice5:
                    this.SubPPrice5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate5:
                    this.SubPEscRate5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType5:
                    this.SubPEscType5 = attValue;
                    break;
                case cSubPTotal5:
                    this.SubPTotal5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit5:
                    this.SubPTotalPerUnit5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating5:
                    this.SubPDegreeRating5 = attValue;
                    break;
                case cSubPName6:
                    this.SubPName6 = attValue;
                    break;
                case cSubPLabel6:
                    this.SubPLabel6 = attValue;
                    break;
                case cSubPType6:
                    this.SubPType6 = attValue;
                    break;
                case cSubPFactor6:
                    this.SubPFactor6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears6:
                    this.SubPYears6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount6:
                    this.SubPAmount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit6:
                    this.SubPUnit6 = attValue;
                    break;
                case cSubPPrice6:
                    this.SubPPrice6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate6:
                    this.SubPEscRate6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType6:
                    this.SubPEscType6 = attValue;
                    break;
                case cSubPTotal6:
                    this.SubPTotal6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit6:
                    this.SubPTotalPerUnit6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating6:
                    this.SubPDegreeRating6 = attValue;
                    break;
                case cSubPName7:
                    this.SubPName7 = attValue;
                    break;
                case cSubPLabel7:
                    this.SubPLabel7 = attValue;
                    break;
                case cSubPType7:
                    this.SubPType7 = attValue;
                    break;
                case cSubPFactor7:
                    this.SubPFactor7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears7:
                    this.SubPYears7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount7:
                    this.SubPAmount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit7:
                    this.SubPUnit7 = attValue;
                    break;
                case cSubPPrice7:
                    this.SubPPrice7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate7:
                    this.SubPEscRate7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType7:
                    this.SubPEscType7 = attValue;
                    break;
                case cSubPTotal7:
                    this.SubPTotal7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit7:
                    this.SubPTotalPerUnit7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating7:
                    this.SubPDegreeRating7 = attValue;
                    break;
                case cSubPName8:
                    this.SubPName8 = attValue;
                    break;
                case cSubPLabel8:
                    this.SubPLabel8 = attValue;
                    break;
                case cSubPType8:
                    this.SubPType8 = attValue;
                    break;
                case cSubPFactor8:
                    this.SubPFactor8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears8:
                    this.SubPYears8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount8:
                    this.SubPAmount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit8:
                    this.SubPUnit8 = attValue;
                    break;
                case cSubPPrice8:
                    this.SubPPrice8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate8:
                    this.SubPEscRate8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType8:
                    this.SubPEscType8 = attValue;
                    break;
                case cSubPTotal8:
                    this.SubPTotal8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit8:
                    this.SubPTotalPerUnit8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating8:
                    this.SubPDegreeRating8 = attValue;
                    break;
                case cSubPName9:
                    this.SubPName9 = attValue;
                    break;
                case cSubPLabel9:
                    this.SubPLabel9 = attValue;
                    break;
                case cSubPType9:
                    this.SubPType9 = attValue;
                    break;
                case cSubPFactor9:
                    this.SubPFactor9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears9:
                    this.SubPYears9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount9:
                    this.SubPAmount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit9:
                    this.SubPUnit9 = attValue;
                    break;
                case cSubPPrice9:
                    this.SubPPrice9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate9:
                    this.SubPEscRate9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType9:
                    this.SubPEscType9 = attValue;
                    break;
                case cSubPTotal9:
                    this.SubPTotal9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit9:
                    this.SubPTotalPerUnit9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating9:
                    this.SubPDegreeRating9 = attValue;
                    break;
                case cSubPName10:
                    this.SubPName10 = attValue;
                    break;
                case cSubPLabel10:
                    this.SubPLabel10 = attValue;
                    break;
                case cSubPType10:
                    this.SubPType10 = attValue;
                    break;
                case cSubPFactor10:
                    this.SubPFactor10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears10:
                    this.SubPYears10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount10:
                    this.SubPAmount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit10:
                    this.SubPUnit10 = attValue;
                    break;
                case cSubPPrice10:
                    this.SubPPrice10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate10:
                    this.SubPEscRate10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType10:
                    this.SubPEscType10 = attValue;
                    break;
                case cSubPTotal10:
                    this.SubPTotal10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit10:
                    this.SubPTotalPerUnit10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDegreeRating10:
                    this.SubPDegreeRating10 = attValue;
                    break;
                default:
                    break;
            }
        }
        public virtual string GetSubPricesProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cSubPDescription:
                    sPropertyValue = this.SubPDescription;
                    break;
                case cNRDescription:
                    sPropertyValue = this.NRDescription;
                    break;
                case cSubPName1:
                    sPropertyValue = this.SubPName1;
                    break;
                case cSubPLabel1:
                    sPropertyValue = this.SubPLabel1;
                    break;
                case cSubPType1:
                    sPropertyValue = this.SubPType1;
                    break;
                case cSubPFactor1:
                    sPropertyValue = this.SubPFactor1.ToString();
                    break;
                case cSubPYears1:
                    sPropertyValue = this.SubPYears1.ToString();
                    break;
                case cSubPAmount1:
                    sPropertyValue = this.SubPAmount1.ToString();
                    break;
                case cSubPUnit1:
                    sPropertyValue = this.SubPUnit1.ToString();
                    break;
                case cSubPPrice1:
                    sPropertyValue = this.SubPPrice1.ToString();
                    break;
                case cSubPEscRate1:
                    sPropertyValue = this.SubPEscRate1.ToString();
                    break;
                case cSubPEscType1:
                    sPropertyValue = this.SubPEscType1;
                    break;
                case cSubPTotal1:
                    sPropertyValue = this.SubPTotal1.ToString();
                    break;
                case cSubPTotalPerUnit1:
                    sPropertyValue = this.SubPTotalPerUnit1.ToString();
                    break;
                case cSubPDegreeRating1:
                    sPropertyValue = this.SubPDegreeRating1.ToString();
                    break;
                case cSubPName2:
                    sPropertyValue = this.SubPName2;
                    break;
                case cSubPLabel2:
                    sPropertyValue = this.SubPLabel2;
                    break;
                case cSubPType2:
                    sPropertyValue = this.SubPType2;
                    break;
                case cSubPFactor2:
                    sPropertyValue = this.SubPFactor2.ToString();
                    break;
                case cSubPYears2:
                    sPropertyValue = this.SubPYears2.ToString();
                    break;
                case cSubPAmount2:
                    sPropertyValue = this.SubPAmount2.ToString();
                    break;
                case cSubPUnit2:
                    sPropertyValue = this.SubPUnit2.ToString();
                    break;
                case cSubPPrice2:
                    sPropertyValue = this.SubPPrice2.ToString();
                    break;
                case cSubPEscRate2:
                    sPropertyValue = this.SubPEscRate2.ToString();
                    break;
                case cSubPEscType2:
                    sPropertyValue = this.SubPEscType2;
                    break;
                case cSubPTotal2:
                    sPropertyValue = this.SubPTotal2.ToString();
                    break;
                case cSubPTotalPerUnit2:
                    sPropertyValue = this.SubPTotalPerUnit2.ToString();
                    break;
                case cSubPDegreeRating2:
                    sPropertyValue = this.SubPDegreeRating2.ToString();
                    break;
                case cSubPName3:
                    sPropertyValue = this.SubPName3.ToString();
                    break;
                case cSubPLabel3:
                    sPropertyValue = this.SubPLabel3;
                    break;
                case cSubPFactor3:
                    sPropertyValue = this.SubPFactor3.ToString();
                    break;
                case cSubPYears3:
                    sPropertyValue = this.SubPYears3.ToString();
                    break;
                case cSubPType3:
                    sPropertyValue = this.SubPType3;
                    break;
                case cSubPAmount3:
                    sPropertyValue = this.SubPAmount3.ToString();
                    break;
                case cSubPUnit3:
                    sPropertyValue = this.SubPUnit3.ToString();
                    break;
                case cSubPPrice3:
                    sPropertyValue = this.SubPPrice3.ToString();
                    break;
                case cSubPEscRate3:
                    sPropertyValue = this.SubPEscRate3.ToString();
                    break;
                case cSubPEscType3:
                    sPropertyValue = this.SubPEscType3;
                    break;
                case cSubPTotal3:
                    sPropertyValue = this.SubPTotal3.ToString();
                    break;
                case cSubPTotalPerUnit3:
                    sPropertyValue = this.SubPTotalPerUnit3.ToString();
                    break;
                case cSubPDegreeRating3:
                    sPropertyValue = this.SubPDegreeRating3.ToString();
                    break;
                case cSubPName4:
                    sPropertyValue = this.SubPName4.ToString();
                    break;
                case cSubPLabel4:
                    sPropertyValue = this.SubPLabel4;
                    break;
                case cSubPFactor4:
                    sPropertyValue = this.SubPFactor4.ToString();
                    break;
                case cSubPYears4:
                    sPropertyValue = this.SubPYears4.ToString();
                    break;
                case cSubPType4:
                    sPropertyValue = this.SubPType4;
                    break;
                case cSubPAmount4:
                    sPropertyValue = this.SubPAmount4.ToString();
                    break;
                case cSubPUnit4:
                    sPropertyValue = this.SubPUnit4.ToString();
                    break;
                case cSubPPrice4:
                    sPropertyValue = this.SubPPrice4.ToString();
                    break;
                case cSubPEscRate4:
                    sPropertyValue = this.SubPEscRate4.ToString();
                    break;
                case cSubPEscType4:
                    sPropertyValue = this.SubPEscType4;
                    break;
                case cSubPTotal4:
                    sPropertyValue = this.SubPTotal4.ToString();
                    break;
                case cSubPTotalPerUnit4:
                    sPropertyValue = this.SubPTotalPerUnit4.ToString();
                    break;
                case cSubPDegreeRating4:
                    sPropertyValue = this.SubPDegreeRating4.ToString();
                    break;
                case cSubPName5:
                    sPropertyValue = this.SubPName5.ToString();
                    break;
                case cSubPLabel5:
                    sPropertyValue = this.SubPLabel5;
                    break;
                case cSubPFactor5:
                    sPropertyValue = this.SubPFactor5.ToString();
                    break;
                case cSubPYears5:
                    sPropertyValue = this.SubPYears5.ToString();
                    break;
                case cSubPType5:
                    sPropertyValue = this.SubPType5;
                    break;
                case cSubPAmount5:
                    sPropertyValue = this.SubPAmount5.ToString();
                    break;
                case cSubPUnit5:
                    sPropertyValue = this.SubPUnit5.ToString();
                    break;
                case cSubPPrice5:
                    sPropertyValue = this.SubPPrice5.ToString();
                    break;
                case cSubPEscRate5:
                    sPropertyValue = this.SubPEscRate5.ToString();
                    break;
                case cSubPEscType5:
                    sPropertyValue = this.SubPEscType5;
                    break;
                case cSubPTotal5:
                    sPropertyValue = this.SubPTotal5.ToString();
                    break;
                case cSubPTotalPerUnit5:
                    sPropertyValue = this.SubPTotalPerUnit5.ToString();
                    break;
                case cSubPDegreeRating5:
                    sPropertyValue = this.SubPDegreeRating5.ToString();
                    break;
                case cSubPName6:
                    sPropertyValue = this.SubPName6.ToString();
                    break;
                case cSubPLabel6:
                    sPropertyValue = this.SubPLabel6;
                    break;
                case cSubPFactor6:
                    sPropertyValue = this.SubPFactor6.ToString();
                    break;
                case cSubPYears6:
                    sPropertyValue = this.SubPYears6.ToString();
                    break;
                case cSubPType6:
                    sPropertyValue = this.SubPType6;
                    break;
                case cSubPAmount6:
                    sPropertyValue = this.SubPAmount6.ToString();
                    break;
                case cSubPUnit6:
                    sPropertyValue = this.SubPUnit6.ToString();
                    break;
                case cSubPPrice6:
                    sPropertyValue = this.SubPPrice6.ToString();
                    break;
                case cSubPEscRate6:
                    sPropertyValue = this.SubPEscRate6.ToString();
                    break;
                case cSubPEscType6:
                    sPropertyValue = this.SubPEscType6;
                    break;
                case cSubPTotal6:
                    sPropertyValue = this.SubPTotal6.ToString();
                    break;
                case cSubPTotalPerUnit6:
                    sPropertyValue = this.SubPTotalPerUnit6.ToString();
                    break;
                case cSubPDegreeRating6:
                    sPropertyValue = this.SubPDegreeRating6.ToString();
                    break;
                case cSubPName7:
                    sPropertyValue = this.SubPName7.ToString();
                    break;
                case cSubPLabel7:
                    sPropertyValue = this.SubPLabel7;
                    break;
                case cSubPFactor7:
                    sPropertyValue = this.SubPFactor7.ToString();
                    break;
                case cSubPYears7:
                    sPropertyValue = this.SubPYears7.ToString();
                    break;
                case cSubPType7:
                    sPropertyValue = this.SubPType7;
                    break;
                case cSubPAmount7:
                    sPropertyValue = this.SubPAmount7.ToString();
                    break;
                case cSubPUnit7:
                    sPropertyValue = this.SubPUnit7.ToString();
                    break;
                case cSubPPrice7:
                    sPropertyValue = this.SubPPrice7.ToString();
                    break;
                case cSubPEscRate7:
                    sPropertyValue = this.SubPEscRate7.ToString();
                    break;
                case cSubPEscType7:
                    sPropertyValue = this.SubPEscType7;
                    break;
                case cSubPTotal7:
                    sPropertyValue = this.SubPTotal7.ToString();
                    break;
                case cSubPTotalPerUnit7:
                    sPropertyValue = this.SubPTotalPerUnit7.ToString();
                    break;
                case cSubPDegreeRating7:
                    sPropertyValue = this.SubPDegreeRating7.ToString();
                    break;
                case cSubPName8:
                    sPropertyValue = this.SubPName8.ToString();
                    break;
                case cSubPLabel8:
                    sPropertyValue = this.SubPLabel8;
                    break;
                case cSubPFactor8:
                    sPropertyValue = this.SubPFactor8.ToString();
                    break;
                case cSubPYears8:
                    sPropertyValue = this.SubPYears8.ToString();
                    break;
                case cSubPType8:
                    sPropertyValue = this.SubPType8;
                    break;
                case cSubPAmount8:
                    sPropertyValue = this.SubPAmount8.ToString();
                    break;
                case cSubPUnit8:
                    sPropertyValue = this.SubPUnit8.ToString();
                    break;
                case cSubPPrice8:
                    sPropertyValue = this.SubPPrice8.ToString();
                    break;
                case cSubPEscRate8:
                    sPropertyValue = this.SubPEscRate8.ToString();
                    break;
                case cSubPEscType8:
                    sPropertyValue = this.SubPEscType8;
                    break;
                case cSubPTotal8:
                    sPropertyValue = this.SubPTotal8.ToString();
                    break;
                case cSubPTotalPerUnit8:
                    sPropertyValue = this.SubPTotalPerUnit8.ToString();
                    break;
                case cSubPDegreeRating8:
                    sPropertyValue = this.SubPDegreeRating8.ToString();
                    break;
                case cSubPName9:
                    sPropertyValue = this.SubPName9.ToString();
                    break;
                case cSubPLabel9:
                    sPropertyValue = this.SubPLabel9;
                    break;
                case cSubPFactor9:
                    sPropertyValue = this.SubPFactor9.ToString();
                    break;
                case cSubPYears9:
                    sPropertyValue = this.SubPYears9.ToString();
                    break;
                case cSubPType9:
                    sPropertyValue = this.SubPType9;
                    break;
                case cSubPAmount9:
                    sPropertyValue = this.SubPAmount9.ToString();
                    break;
                case cSubPUnit9:
                    sPropertyValue = this.SubPUnit9.ToString();
                    break;
                case cSubPPrice9:
                    sPropertyValue = this.SubPPrice9.ToString();
                    break;
                case cSubPEscRate9:
                    sPropertyValue = this.SubPEscRate9.ToString();
                    break;
                case cSubPEscType9:
                    sPropertyValue = this.SubPEscType9;
                    break;
                case cSubPTotal9:
                    sPropertyValue = this.SubPTotal9.ToString();
                    break;
                case cSubPTotalPerUnit9:
                    sPropertyValue = this.SubPTotalPerUnit9.ToString();
                    break;
                case cSubPDegreeRating9:
                    sPropertyValue = this.SubPDegreeRating9.ToString();
                    break;
                case cSubPName10:
                    sPropertyValue = this.SubPName10.ToString();
                    break;
                case cSubPLabel10:
                    sPropertyValue = this.SubPLabel10;
                    break;
                case cSubPFactor10:
                    sPropertyValue = this.SubPFactor10.ToString();
                    break;
                case cSubPYears10:
                    sPropertyValue = this.SubPYears10.ToString();
                    break;
                case cSubPType10:
                    sPropertyValue = this.SubPType10;
                    break;
                case cSubPAmount10:
                    sPropertyValue = this.SubPAmount10.ToString();
                    break;
                case cSubPUnit10:
                    sPropertyValue = this.SubPUnit10.ToString();
                    break;
                case cSubPPrice10:
                    sPropertyValue = this.SubPPrice10.ToString();
                    break;
                case cSubPEscRate10:
                    sPropertyValue = this.SubPEscRate10.ToString();
                    break;
                case cSubPEscType10:
                    sPropertyValue = this.SubPEscType10;
                    break;
                case cSubPTotal10:
                    sPropertyValue = this.SubPTotal10.ToString();
                    break;
                case cSubPTotalPerUnit10:
                    sPropertyValue = this.SubPTotalPerUnit10.ToString();
                    break;
                case cSubPDegreeRating10:
                    sPropertyValue = this.SubPDegreeRating10.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetSubPricesAttributes(string attNameExtension,
            ref XElement calculator)
        {
            
            //don't needlessly add these to linkedviews if they are not being used
            if (this.SubPName1 != string.Empty && this.SubPName1 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDescription, attNameExtension), this.SubPDescription);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cNRDescription, attNameExtension), this.NRDescription);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName1, attNameExtension), this.SubPName1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel1, attNameExtension), this.SubPLabel1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType1, attNameExtension), this.SubPType1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor1, attNameExtension), this.SubPFactor1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears1, attNameExtension), this.SubPYears1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount1, attNameExtension), this.SubPAmount1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit1, attNameExtension), this.SubPUnit1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice1, attNameExtension), this.SubPPrice1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate1, attNameExtension), this.SubPEscRate1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType1, attNameExtension), this.SubPEscType1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal1, attNameExtension), this.SubPTotal1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit1, attNameExtension), this.SubPTotalPerUnit1);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating1, attNameExtension), this.SubPDegreeRating1);
            }
            if (this.SubPName2 != string.Empty && this.SubPName2 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName2, attNameExtension), this.SubPName2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel2, attNameExtension), this.SubPLabel2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType2, attNameExtension), this.SubPType2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor2, attNameExtension), this.SubPFactor2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears2, attNameExtension), this.SubPYears2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount2, attNameExtension), this.SubPAmount2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit2, attNameExtension), this.SubPUnit2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice2, attNameExtension), this.SubPPrice2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate2, attNameExtension), this.SubPEscRate2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType2, attNameExtension), this.SubPEscType2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal2, attNameExtension), this.SubPTotal2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit2, attNameExtension), this.SubPTotalPerUnit2);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating2, attNameExtension), this.SubPDegreeRating2);
            }
            if (this.SubPName3 != string.Empty && this.SubPName3 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName3, attNameExtension), this.SubPName3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel3, attNameExtension), this.SubPLabel3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType3, attNameExtension), this.SubPType3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor3, attNameExtension), this.SubPFactor3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears3, attNameExtension), this.SubPYears3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount3, attNameExtension), this.SubPAmount3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit3, attNameExtension), this.SubPUnit3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice3, attNameExtension), this.SubPPrice3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate3, attNameExtension), this.SubPEscRate3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType3, attNameExtension), this.SubPEscType3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal3, attNameExtension), this.SubPTotal3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit3, attNameExtension), this.SubPTotalPerUnit3);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating3, attNameExtension), this.SubPDegreeRating3);
            }
            if (this.SubPName4 != string.Empty && this.SubPName4 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName4, attNameExtension), this.SubPName4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel4, attNameExtension), this.SubPLabel4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType4, attNameExtension), this.SubPType4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor4, attNameExtension), this.SubPFactor4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears4, attNameExtension), this.SubPYears4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount4, attNameExtension), this.SubPAmount4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit4, attNameExtension), this.SubPUnit4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice4, attNameExtension), this.SubPPrice4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate4, attNameExtension), this.SubPEscRate4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType4, attNameExtension), this.SubPEscType4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal4, attNameExtension), this.SubPTotal4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit4, attNameExtension), this.SubPTotalPerUnit4);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating4, attNameExtension), this.SubPDegreeRating4);
            }
            if (this.SubPName5 != string.Empty && this.SubPName5 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName5, attNameExtension), this.SubPName5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel5, attNameExtension), this.SubPLabel5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType5, attNameExtension), this.SubPType5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor5, attNameExtension), this.SubPFactor5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears5, attNameExtension), this.SubPYears5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount5, attNameExtension), this.SubPAmount5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit5, attNameExtension), this.SubPUnit5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice5, attNameExtension), this.SubPPrice5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate5, attNameExtension), this.SubPEscRate5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType5, attNameExtension), this.SubPEscType5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal5, attNameExtension), this.SubPTotal5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit5, attNameExtension), this.SubPTotalPerUnit5);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating5, attNameExtension), this.SubPDegreeRating5);
            }
            if (this.SubPName6 != string.Empty && this.SubPName6 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName6, attNameExtension), this.SubPName6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel6, attNameExtension), this.SubPLabel6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType6, attNameExtension), this.SubPType6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor6, attNameExtension), this.SubPFactor6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears6, attNameExtension), this.SubPYears6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount6, attNameExtension), this.SubPAmount6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit6, attNameExtension), this.SubPUnit6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice6, attNameExtension), this.SubPPrice6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate6, attNameExtension), this.SubPEscRate6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType6, attNameExtension), this.SubPEscType6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal6, attNameExtension), this.SubPTotal6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit6, attNameExtension), this.SubPTotalPerUnit6);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating6, attNameExtension), this.SubPDegreeRating6);
            }
            if (this.SubPName7 != string.Empty && this.SubPName7 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName7, attNameExtension), this.SubPName7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel7, attNameExtension), this.SubPLabel7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType7, attNameExtension), this.SubPType7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor7, attNameExtension), this.SubPFactor7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears7, attNameExtension), this.SubPYears7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount7, attNameExtension), this.SubPAmount7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit7, attNameExtension), this.SubPUnit7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice7, attNameExtension), this.SubPPrice7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate7, attNameExtension), this.SubPEscRate7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType7, attNameExtension), this.SubPEscType7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal7, attNameExtension), this.SubPTotal7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit7, attNameExtension), this.SubPTotalPerUnit7);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating7, attNameExtension), this.SubPDegreeRating7);
            }
            if (this.SubPName8 != string.Empty && this.SubPName8 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName8, attNameExtension), this.SubPName8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel8, attNameExtension), this.SubPLabel8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType8, attNameExtension), this.SubPType8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor8, attNameExtension), this.SubPFactor8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears8, attNameExtension), this.SubPYears8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount8, attNameExtension), this.SubPAmount8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit8, attNameExtension), this.SubPUnit8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice8, attNameExtension), this.SubPPrice8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate8, attNameExtension), this.SubPEscRate8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType8, attNameExtension), this.SubPEscType8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal8, attNameExtension), this.SubPTotal8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit8, attNameExtension), this.SubPTotalPerUnit8);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating8, attNameExtension), this.SubPDegreeRating8);
            }
            if (this.SubPName9 != string.Empty && this.SubPName9 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName9, attNameExtension), this.SubPName9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel9, attNameExtension), this.SubPLabel9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType9, attNameExtension), this.SubPType9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor9, attNameExtension), this.SubPFactor9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears9, attNameExtension), this.SubPYears9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount9, attNameExtension), this.SubPAmount9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit9, attNameExtension), this.SubPUnit9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice9, attNameExtension), this.SubPPrice9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate9, attNameExtension), this.SubPEscRate9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType9, attNameExtension), this.SubPEscType9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal9, attNameExtension), this.SubPTotal9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit9, attNameExtension), this.SubPTotalPerUnit9);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating9, attNameExtension), this.SubPDegreeRating9);
            }
            if (this.SubPName10 != string.Empty && this.SubPName10 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPName10, attNameExtension), this.SubPName10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel10, attNameExtension), this.SubPLabel10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType10, attNameExtension), this.SubPType10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPFactor10, attNameExtension), this.SubPFactor10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears10, attNameExtension), this.SubPYears10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPAmount10, attNameExtension), this.SubPAmount10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit10, attNameExtension), this.SubPUnit10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPPrice10, attNameExtension), this.SubPPrice10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPEscRate10, attNameExtension), this.SubPEscRate10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType10, attNameExtension), this.SubPEscType10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal10, attNameExtension), this.SubPTotal10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit10, attNameExtension), this.SubPTotalPerUnit10);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDegreeRating10, attNameExtension), this.SubPDegreeRating10);
            }
        }
        public virtual void SetSubPricesAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.SubPName1 != string.Empty && this.SubPName1 != Constants.NONE)
            {
                writer.WriteAttributeString(
                    string.Concat(cSubPDescription, attNameExtension), this.SubPDescription);
                writer.WriteAttributeString(
                    string.Concat(cNRDescription, attNameExtension), this.NRDescription);
                writer.WriteAttributeString(
                     string.Concat(cSubPName1, attNameExtension), this.SubPName1);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel1, attNameExtension), this.SubPLabel1);
                writer.WriteAttributeString(
                     string.Concat(cSubPType1, attNameExtension), this.SubPType1);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor1, attNameExtension), this.SubPFactor1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears1, attNameExtension), this.SubPYears1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount1, attNameExtension), this.SubPAmount1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit1, attNameExtension), this.SubPUnit1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice1, attNameExtension), this.SubPPrice1.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate1, attNameExtension), this.SubPEscRate1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType1, attNameExtension), this.SubPEscType1);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal1, attNameExtension), this.SubPTotal1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit1, attNameExtension), this.SubPTotalPerUnit1.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating1, attNameExtension), this.SubPDegreeRating1.ToString());
            }
            if (this.SubPName2 != string.Empty && this.SubPName2 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName2, attNameExtension), this.SubPName2);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel2, attNameExtension), this.SubPLabel2);
                writer.WriteAttributeString(
                     string.Concat(cSubPType2, attNameExtension), this.SubPType2);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor2, attNameExtension), this.SubPFactor2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears2, attNameExtension), this.SubPYears2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount2, attNameExtension), this.SubPAmount2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit2, attNameExtension), this.SubPUnit2.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice2, attNameExtension), this.SubPPrice2.ToString());
                writer.WriteAttributeString(
                      string.Concat(cSubPEscRate2, attNameExtension), this.SubPEscRate2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType2, attNameExtension), this.SubPEscType2);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal2, attNameExtension), this.SubPTotal2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit2, attNameExtension), this.SubPTotalPerUnit2.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating2, attNameExtension), this.SubPDegreeRating2.ToString());
            }
            if (this.SubPName3 != string.Empty && this.SubPName3 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName3, attNameExtension), this.SubPName3);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel3, attNameExtension), this.SubPLabel3);
                writer.WriteAttributeString(
                     string.Concat(cSubPType3, attNameExtension), this.SubPType3);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor3, attNameExtension), this.SubPFactor3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears3, attNameExtension), this.SubPYears3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount3, attNameExtension), this.SubPAmount3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit3, attNameExtension), this.SubPUnit3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice3, attNameExtension), this.SubPPrice3.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate3, attNameExtension), this.SubPEscRate3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType3, attNameExtension), this.SubPEscType3);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal3, attNameExtension), this.SubPTotal3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit3, attNameExtension), this.SubPTotalPerUnit3.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating3, attNameExtension), this.SubPDegreeRating3.ToString());
            }
            if (this.SubPName4 != string.Empty && this.SubPName4 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName4, attNameExtension), this.SubPName4);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel4, attNameExtension), this.SubPLabel4);
                writer.WriteAttributeString(
                     string.Concat(cSubPType4, attNameExtension), this.SubPType4);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor4, attNameExtension), this.SubPFactor4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears4, attNameExtension), this.SubPYears4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount4, attNameExtension), this.SubPAmount4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit4, attNameExtension), this.SubPUnit4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice4, attNameExtension), this.SubPPrice4.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate4, attNameExtension), this.SubPEscRate4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType4, attNameExtension), this.SubPEscType4);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal4, attNameExtension), this.SubPTotal4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit4, attNameExtension), this.SubPTotalPerUnit4.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating4, attNameExtension), this.SubPDegreeRating4.ToString());
            }
            if (this.SubPName5 != string.Empty && this.SubPName5 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName5, attNameExtension), this.SubPName5);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel5, attNameExtension), this.SubPLabel5);
                writer.WriteAttributeString(
                     string.Concat(cSubPType5, attNameExtension), this.SubPType5);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor5, attNameExtension), this.SubPFactor5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears5, attNameExtension), this.SubPYears5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount5, attNameExtension), this.SubPAmount5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit5, attNameExtension), this.SubPUnit5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice5, attNameExtension), this.SubPPrice5.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate5, attNameExtension), this.SubPEscRate5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType5, attNameExtension), this.SubPEscType5);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal5, attNameExtension), this.SubPTotal5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit5, attNameExtension), this.SubPTotalPerUnit5.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating5, attNameExtension), this.SubPDegreeRating5.ToString());
            }
            if (this.SubPName6 != string.Empty && this.SubPName6 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName6, attNameExtension), this.SubPName6);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel6, attNameExtension), this.SubPLabel6);
                writer.WriteAttributeString(
                     string.Concat(cSubPType6, attNameExtension), this.SubPType6);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor6, attNameExtension), this.SubPFactor6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears6, attNameExtension), this.SubPYears6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount6, attNameExtension), this.SubPAmount6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit6, attNameExtension), this.SubPUnit6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice6, attNameExtension), this.SubPPrice6.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate6, attNameExtension), this.SubPEscRate6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType6, attNameExtension), this.SubPEscType6);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal6, attNameExtension), this.SubPTotal6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit6, attNameExtension), this.SubPTotalPerUnit6.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating6, attNameExtension), this.SubPDegreeRating6.ToString());
            }
            if (this.SubPName7 != string.Empty && this.SubPName7 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName7, attNameExtension), this.SubPName7);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel7, attNameExtension), this.SubPLabel7);
                writer.WriteAttributeString(
                     string.Concat(cSubPType7, attNameExtension), this.SubPType7);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor7, attNameExtension), this.SubPFactor7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears7, attNameExtension), this.SubPYears7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount7, attNameExtension), this.SubPAmount7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit7, attNameExtension), this.SubPUnit7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice7, attNameExtension), this.SubPPrice7.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate7, attNameExtension), this.SubPEscRate7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType7, attNameExtension), this.SubPEscType7);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal7, attNameExtension), this.SubPTotal7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit7, attNameExtension), this.SubPTotalPerUnit7.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating7, attNameExtension), this.SubPDegreeRating7.ToString());
            }
            if (this.SubPName8 != string.Empty && this.SubPName8 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName8, attNameExtension), this.SubPName8);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel8, attNameExtension), this.SubPLabel8);
                writer.WriteAttributeString(
                     string.Concat(cSubPType8, attNameExtension), this.SubPType8);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor8, attNameExtension), this.SubPFactor8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears8, attNameExtension), this.SubPYears8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount8, attNameExtension), this.SubPAmount8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit8, attNameExtension), this.SubPUnit8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice8, attNameExtension), this.SubPPrice8.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate8, attNameExtension), this.SubPEscRate8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType8, attNameExtension), this.SubPEscType8);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal8, attNameExtension), this.SubPTotal8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit8, attNameExtension), this.SubPTotalPerUnit8.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating8, attNameExtension), this.SubPDegreeRating8.ToString());
            }
            if (this.SubPName9 != string.Empty && this.SubPName9 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName9, attNameExtension), this.SubPName9);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel9, attNameExtension), this.SubPLabel9);
                writer.WriteAttributeString(
                     string.Concat(cSubPType9, attNameExtension), this.SubPType9);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor9, attNameExtension), this.SubPFactor9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears9, attNameExtension), this.SubPYears9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount9, attNameExtension), this.SubPAmount9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit9, attNameExtension), this.SubPUnit9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice9, attNameExtension), this.SubPPrice9.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate9, attNameExtension), this.SubPEscRate9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType9, attNameExtension), this.SubPEscType9);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal9, attNameExtension), this.SubPTotal9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit9, attNameExtension), this.SubPTotalPerUnit9.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating9, attNameExtension), this.SubPDegreeRating9.ToString());
            }
            if (this.SubPName10 != string.Empty && this.SubPName10 != Constants.NONE)
            {
                writer.WriteAttributeString(
                     string.Concat(cSubPName10, attNameExtension), this.SubPName10);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel10, attNameExtension), this.SubPLabel10);
                writer.WriteAttributeString(
                     string.Concat(cSubPType10, attNameExtension), this.SubPType10);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor10, attNameExtension), this.SubPFactor10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears10, attNameExtension), this.SubPYears10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount10, attNameExtension), this.SubPAmount10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit10, attNameExtension), this.SubPUnit10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice10, attNameExtension), this.SubPPrice10.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate10, attNameExtension), this.SubPEscRate10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPEscType10, attNameExtension), this.SubPEscType10);
                writer.WriteAttributeString(
                    string.Concat(cSubPTotal10, attNameExtension), this.SubPTotal10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cSubPTotalPerUnit10, attNameExtension), this.SubPTotalPerUnit10.ToString());
                writer.WriteAttributeString(
                   string.Concat(cSubPDegreeRating10, attNameExtension), this.SubPDegreeRating10.ToString());
            }
        }
    }
}
