using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a food nutrition USDA standard reference 
    ///             object with properties derived from the SR 24 Food Nutrition database.
    ///Author:		www.devtreks.org
    ///Date:		2013, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Although this class inherits from Output, it doesn't initiate 
    ///             any Input properties or attributes. This keeps the calculator simple 
    ///             and clean. If Input properties can be changed by this calculator, 
    ///             change the input properties/attributes in the calculator 
    ///             (i.e. foodStock.SetInputProperties).
    public class FN1OutputCalculator : Output
    {
        
        //environmental impacts (benefits)
    }
}
