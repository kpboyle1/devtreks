
namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		General rules for ag budgeting calculators.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class AgBudgetingRules
    {
        /// <summary>
        /// Calculate amoritized capital service costs
        /// </summary>
        /// <param name="calculatorType"></param>
        /// <param name="i"></param>
        /// <param name="timePeriods"></param>
        /// <param name="plannedUsePeriods"></param>
        /// <param name="usefulLifePeriods"></param>
        /// <param name="realRate"></param>
        /// <param name="timeType"></param>
        /// <param name="sumCosts"></param>
        /// <param name="sumOutputs"></param>
        /// <param name="amortizedAvgCostperPeriod"></param>
        public static void CalcAmortizdServiceCosts(AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            int i, int timePeriods, int plannedUsePeriods, int usefulLifePeriods,
            double realRate, GeneralRules.TIME_TYPES timeType, double sumCosts, double sumOutputs,
            out double amortizedAvgCostperPeriod)
        {
            amortizedAvgCostperPeriod = 0;
            double dbAnnuityFactor = 0;
            //new machinery
            if (timePeriods > 1)
            {
                //convert to annuity
                dbAnnuityFactor = GeneralRules.CapRecovFactor(timePeriods, realRate, 0);
                sumCosts = sumCosts * dbAnnuityFactor;
                if (timeType == GeneralRules.TIME_TYPES.costsvary)
                {
                    amortizedAvgCostperPeriod = sumCosts / plannedUsePeriods;
                }
                else if (timeType == GeneralRules.TIME_TYPES.costsandoutputsvary)
                {
                    if (sumOutputs == 0)
                    {
                        //shouldn't be using outputs
                        amortizedAvgCostperPeriod = sumCosts / plannedUsePeriods;
                    }
                    else
                    {
                        amortizedAvgCostperPeriod = sumCosts / (sumOutputs / timePeriods);
                    }
                }
            }
            else
            {
                if ((i == 3) && (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery))
                {
                    amortizedAvgCostperPeriod = sumCosts / usefulLifePeriods;
                }
                else
                {
                    amortizedAvgCostperPeriod = sumCosts / plannedUsePeriods;
                }
            }
        }
        /// <summary>
        /// Calculate used machinery repair costs
        /// </summary>
        /// <param name="calculatorType"></param>
        /// <param name="timeUsed"></param>
        /// <param name="timePeriods"></param>
        /// <param name="plannedUsePeriods"></param>
        /// <param name="startingPeriods"></param>
        /// <param name="remainingPeriods"></param>
        /// <param name="realRate"></param>
        /// <param name="inflationRate"></param>
        /// <param name="timeType"></param>
        /// <param name="RF1"></param>
        /// <param name="RF2"></param>
        /// <param name="OC3Amount"></param>
        /// <param name="sumCosts"></param>
        /// <param name="sumOutputs"></param>
        /// <param name="sumUsedCost"></param>
        /// <param name="amortizedAvgCostperPeriod"></param>
        public static void CalcUsedMachineryRepairCosts(AgBudgetingHelpers.CALCULATOR_TYPES calculatorType, int timeUsed, int timePeriods, int plannedUsePeriods,
            int startingPeriods, int remainingPeriods, double realRate, double inflationRate, GeneralRules.TIME_TYPES timeType,
            double RF1, double RF2, double OC3Amount, double sumCosts, double sumOutputs, double sumUsedCost,
            out double amortizedAvgCostperPeriod)
        {
            amortizedAvgCostperPeriod = 0;
            double dbAnnuityFactor = 0;
            //repair costs for used machinery
            if (timePeriods > 1)
            {
                //only difference is the period over which the amortization takes place -note: this is ad hoc
                //sumCosts = sumCosts;
                //amortize this remainder over the remaining service life of the equipment
                dbAnnuityFactor = GeneralRules.CapRecovFactor((timePeriods - timeUsed), realRate, 0);
                sumCosts = sumCosts * dbAnnuityFactor;
                if (timeType == GeneralRules.TIME_TYPES.costsvary)
                {
                    amortizedAvgCostperPeriod = sumCosts / plannedUsePeriods;
                }
                else if (timeType == GeneralRules.TIME_TYPES.costsandoutputsvary)
                {
                    if (sumOutputs == 0)
                    {
                        //shouldn't be using outputs
                        amortizedAvgCostperPeriod = sumCosts / plannedUsePeriods;
                    }
                    else
                    {
                        //simple avg unit cost - or use a particular year in ui
                        amortizedAvgCostperPeriod = sumCosts / (sumOutputs / timePeriods);
                    }
                }
                else
                {
                    //the used costs will not have been set in for...next loop, set it here
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        sumUsedCost = (((RF1 * OC3Amount) * System.Math.Pow((startingPeriods / 1000.00), RF2))) * (1 + inflationRate);
                        amortizedAvgCostperPeriod = (sumCosts - sumUsedCost) / remainingPeriods;
                    }
                    else
                    {
                        amortizedAvgCostperPeriod = (sumCosts) / plannedUsePeriods;
                    }

                }
            }
            else
            {
                //12/9 : the costsdonotvary sets timeperiod to 1
                //the used costs will not have been set in for...next loop, set it here
                if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                {
                    sumUsedCost = (((RF1 * OC3Amount) * System.Math.Pow((startingPeriods / 1000.00), RF2))) * (1 + inflationRate);
                    amortizedAvgCostperPeriod = (sumCosts - sumUsedCost) / remainingPeriods;
                }
                else
                {
                    amortizedAvgCostperPeriod = (sumCosts) / plannedUsePeriods;
                }
            }
        }
        public static double GetFuelPrice(Machinery1Input machinput)
        {
            double dbFuelPrice = 0;
            if (machinput.Constants.FuelType
                == GeneralRules.FUEL_PRICE_TYPES.diesel.ToString())
            {
                dbFuelPrice = machinput.Constants.PriceDiesel;
            }
            else if (machinput.Constants.FuelType
                == GeneralRules.FUEL_PRICE_TYPES.electric.ToString())
            {
                dbFuelPrice = machinput.Constants.PriceElectric;
            }
            else if (machinput.Constants.FuelType
                == GeneralRules.FUEL_PRICE_TYPES.gas.ToString())
            {
                dbFuelPrice = machinput.Constants.PriceGas;
            }
            else if (machinput.Constants.FuelType
                == GeneralRules.FUEL_PRICE_TYPES.lpg.ToString())
            {
                dbFuelPrice = machinput.Constants.PriceLP;
            }
            else if (machinput.Constants.FuelType
                == GeneralRules.FUEL_PRICE_TYPES.naturalgas.ToString())
            {
                dbFuelPrice = machinput.Constants.PriceNG;
            }
            return dbFuelPrice;
        }
        public static double GetLaborPrice(Machinery1Input machinput)
        {
            double dbLaborPrice = 0;
            if (machinput.Constants.LaborType
                == GeneralRules.LABOR_PRICE_TYPES.machinery.ToString())
            {
                dbLaborPrice = machinput.Constants.PriceMachineryLabor;
            }
            else if (machinput.Constants.LaborType
                == GeneralRules.LABOR_PRICE_TYPES.regular.ToString())
            {
                dbLaborPrice = machinput.Constants.PriceRegularLabor;
            }
            else if (machinput.Constants.LaborType
                == GeneralRules.LABOR_PRICE_TYPES.supervisory.ToString())
            {
                dbLaborPrice = machinput.Constants.PriceSupervisorLabor;
            }
            return dbLaborPrice;
        }
    }
}
