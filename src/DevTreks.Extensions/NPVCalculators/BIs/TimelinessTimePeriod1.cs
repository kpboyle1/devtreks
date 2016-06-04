using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		This class extends the base TimePeriodClass with 
    ///             labor scheduling and timeliness penalty parameters. These 
    ///             parameters are used in calculators and resource stock analyzers 
    ///             to schedule operations and to penalize late operations and 
    ///             optimize machinery selections.
    ///Author:		www.devtreks.org
    ///Date:		2012, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    public class TimelinessTimePeriod1 : TimePeriod
    {
        public TimelinessTimePeriod1() { }
        //copy constructor
        public TimelinessTimePeriod1(TimelinessTimePeriod1 tp)
            : base(tp)
        {
        }
        //scheduling and selection optimization collection
        //each member is a feasible combination of power and nopower input for that opcomp
        public List<TimelinessOpComp1> TimelinessOpComps { get; set; }
    }
}
