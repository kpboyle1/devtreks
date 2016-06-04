using DevTreks.Data.AppHelpers;
using DevTreks.Models;
using System;

namespace DevTreks.Data.RuleHelpers
{
    // <summary>
    ///Purpose:		Agreements rule-enforcing class. All club payments and revenues are 
    ///             contractually defined in service agreements and calculated using these 
    ///             rules.
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        This uses CLR objects to set totals
    ///             These rules are not enforced. If subscriptions evolved to 
    ///             money-based, revisit.
    /// </summary>
    public class AgreementRulesDTO
    {
        
        public void RunTotals(ContentURI agreementURI)
        {
            if (agreementURI.URINodeName == Agreement.AGREEMENT_TYPES.serviceaccount.ToString()
                && agreementURI.URIModels.Account != null)
            {
                if (agreementURI.URIModels.Account.AccountToService != null)
                {
                    //keep service subscriptions updated with current member count
                    //subscriptions are paid on a per member fee
                    int iCurrentMemberCount = GetCurrentMemberCount(agreementURI);
                    decimal dTotalCostIn = 0;
                    decimal dNetCostOut = 0;
                    MakeTotals(agreementURI, iCurrentMemberCount,
                        dTotalCostIn, out dNetCostOut);
                    agreementURI.URIModels.Account.NetCost = dNetCostOut;
                }
            }
        }
        private void MakeTotals(ContentURI agreementURI, int currentClubMemberCount,
            decimal dTotalCostIn, out decimal dNetCostOut)
        {
            dNetCostOut = 0;
            //initialize variables used to calculate costs
            decimal dSumCost1 = 0;
            string sParentNodeName = agreementURI.URINodeName;
            foreach (AccountToService accountservice in agreementURI.URIModels.Account.AccountToService)
            {
                //keep service subscriptions up to date
                if (currentClubMemberCount != accountservice.Amount1)
                {
                    //verify against db records (and internally handle updating db if the number has changed)
                    bool bHasUpdated = UpdateSubscribedServiceMemberCount(agreementURI,
                        accountservice, accountservice.IsOwner, currentClubMemberCount, accountservice.Amount1);
                    accountservice.Amount1 = currentClubMemberCount;
                }
                //get and set the total cost per month
                accountservice.TotalCost = GetMonthlyEquivalentTotals(accountservice);
                //set the web hosting fees (prior to any incentives being calculated)
                double dbTotalServiceCost = (double)accountservice.TotalCost;
                SetHostFee(agreementURI, accountservice, dbTotalServiceCost);
                double dbHostFee = (double)accountservice.HostServiceFee;
                decimal dNetServiceCost = (decimal)dbTotalServiceCost;
                if (accountservice.IsOwner)
                {
                    //owners view their net revenues (net of hosting fees)
                    dNetServiceCost = (decimal)(dbTotalServiceCost - dbHostFee);
                }
                else
                {
                    //subscribers view the total amount (including host fees) which they must pay
                }
                if (accountservice.AccountToIncentive != null)
                {
                    if (accountservice.AccountToIncentive.Count > 0)
                    {
                        foreach (AccountToIncentive accountincentive in accountservice.AccountToIncentive)
                        {
                            //recurse to incentives
                            dNetServiceCost = MakeIncentiveTotals(agreementURI, accountincentive, currentClubMemberCount,
                                dNetServiceCost);
                        }
                    }
                }
                accountservice.NetCost = Math.Round(dNetServiceCost, 2);
                //sum for each incentive returned as the output parameter
                dSumCost1 += dNetServiceCost;
            }
            //pass the net cost back up to be summed
            dNetCostOut = dSumCost1;
            agreementURI.URIModels.Account.NetCost = Math.Round(dSumCost1, 2);
        }
        private decimal MakeIncentiveTotals(ContentURI agreementURI, AccountToIncentive accountIncentive,
            int currentClubMemberCount, decimal dTotalCostIn)
        {
            decimal dNetServiceCost = dTotalCostIn;
            double dbRate1 = accountIncentive.Incentive.IncentiveRate1;
            double dbTotalCostIn = (double)dTotalCostIn;
            decimal dRate1Cost = 0;
            //do the rate calculation, based on original service total cost, dRate1 will be negative for taxes
            dRate1Cost = (decimal)(dbTotalCostIn * accountIncentive.Incentive.IncentiveRate1);
            decimal dTotalIncentiveAdjCost = accountIncentive.Incentive.IncentiveAmount1 + dRate1Cost;
            decimal dSumCost1 = (dTotalIncentiveAdjCost + dTotalCostIn);
            accountIncentive.TotalCost = dTotalIncentiveAdjCost;
            dNetServiceCost = dNetServiceCost - accountIncentive.TotalCost;
            return dNetServiceCost;
        }
        private decimal GetMonthlyEquivalentTotals(AccountToService accountService)
        {
            decimal dTotalCost = accountService.Service.ServicePrice1 * accountService.Amount1;
            //convert costs to monthy basis for uniformity
            if (accountService.Service.ServiceUnit1 == Agreement.SERVICE_UNIT_TYPES.day.ToString())
            {
                dTotalCost = dTotalCost * 30;
            }
            else if (accountService.Service.ServiceUnit1 == Agreement.SERVICE_UNIT_TYPES.month.ToString())
            {
                //good total on hand
            }
            else if (accountService.Service.ServiceUnit1 == Agreement.SERVICE_UNIT_TYPES.year.ToString())
            {
                dTotalCost = dTotalCost / 12;
            }
            return Math.Round(dTotalCost, 2);
        }
        private void SetHostFee(ContentURI uri, 
            AccountToService accountservice, double totalServiceCost)
        {
            double dbHostFee = (double)accountservice.HostServiceFee;
            string sHostRate = uri.URIDataManager.HostFeeRate;
            if (string.IsNullOrEmpty(sHostRate))
                sHostRate = "0";
            double dbHostRate = Helpers.GeneralHelpers.ConvertStringToDouble(sHostRate);
            accountservice.HostServiceRate = dbHostRate;
            dbHostFee = (totalServiceCost * accountservice.HostServiceRate);
            accountservice.HostServiceFee = Math.Round((decimal)dbHostFee, 2);
        }
        private int GetCurrentMemberCount(ContentURI agreementURI)
        {
            //admin will check service agreements for 0 members
            int iCurrentMemberCount = 0;
            if (agreementURI.URIMember.ClubInUse != null)
            {
                //IMemberRepositoryEF memberReposit = new SqlRepositories.MemberRepository();
                //IList<AccountToMember> members = memberReposit.GetMembersByClub(
                //    agreementURI.URIMember.ClubInUse.PKId).ToList();
                //if (members != null)
                //{
                //    iCurrentMemberCount = members.Count;
                //}
            }
            return iCurrentMemberCount;
        }
        
        private bool UpdateSubscribedServiceMemberCount(ContentURI agreementURI, 
            AccountToService accountService, bool isOwner, int currentClubMemberCount, 
            int memberCount)
        {
            bool bHasUpdated = false;
            if (!isOwner)
            {
                AppHelpers.Agreement aggreement = new Agreement();
                //bHasUpdated = aggreement.UpdateSubscribedMemberCount(agreementURI, currentClubMemberCount,
                //    accountService.AccountId, accountService.PKId, memberCount);
            }
            return bHasUpdated;
        }
    }
}

