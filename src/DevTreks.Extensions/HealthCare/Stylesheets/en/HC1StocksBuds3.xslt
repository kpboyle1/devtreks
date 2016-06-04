<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, January -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with? -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated-->
	<xsl:param name="docToCalcNodeName" />
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams" />
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName" />
	<!-- which network is this doc from? -->
	<xsl:param name="networkId" />
	<!-- what is the start row? -->
	<xsl:param name="startRow" />
	<!-- what is the end row? -->
	<xsl:param name="endRow" />
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams" />
	<!-- what is the guide's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<xsl:apply-templates select="root" />
	</xsl:template>
	<xsl:template match="root">
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="budgetgroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
		<tr>
			<th scope="col" colspan="10">
				Budget Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10"><strong>Time Period</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@EnterpriseName" /></strong>
			</td>
		</tr>
			<tr>
				<th scope="col" colspan="10">Outcomes</th>
			</tr>
      <tr>
			<td>
				<strong>
					Physical Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Emotional Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Social Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Economic Health Rating
				</strong>
			</td>
      <td>
				<strong>
					Health Care Delivery Rating
				</strong>
			</td>
      <td>
				<strong>
					Average Benefit Rating
				</strong>
			</td>
      <td>
				<strong>
					Before Treatment QOL Rating
				</strong>
			</td>
      <td>
				<strong>
					After Treatment QOL Rating
				</strong>
			</td>
      <td>
				<strong>
					Before Treatment Years
				</strong>
			</td>
      <td>
				<strong>
					After Treatment Years
				</strong>
			</td>
    </tr>
    <tr>
			<td>
				<strong>
					Probability of After Treatment Years
				</strong>
			</td>
			<td>
				<strong>
					Equity Multiplier
				</strong>
			</td>
			<td>
				<strong>
					Quality Adjusted Life Years
				</strong>
			</td>
      <td>
				<strong>
					Incremental QALY
				</strong>
			</td>
			<td>
				<strong>
					Time Tradeoff Years
				</strong>
			</td>
      <td>
				<strong>
					TTO QALY
				</strong>
			</td>
      <td>
				<strong>
					Output Cost
				</strong>
			</td>
			<td>
				<strong>
					Benefit Adjustment
				</strong>
			</td>
			<td>
				<strong>
					Adjusted Benefit
				</strong>
			</td>
      <td>
				<strong>
					Discount (real) Rate
				</strong>
			</td>
    </tr>
    <tr>
			<td>
				<strong>
					Output Effect1 Name
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Unit
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Amount
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Price
				</strong>
			</td>
      <td>
				<strong>
					Output Effect1 Cost
				</strong>
			</td>
      <td>
				<strong>
					Age
				</strong>
			</td>
			<td>
				<strong>
					Gender
				</strong>
			</td>
			<td>
				<strong>
					Education Years
				</strong>
			</td>
      <td>
				<strong>
					Race
				</strong>
			</td>
      <td>
				<strong>
					Work Status
				</strong>
			</td>
    </tr>
		  <xsl:apply-templates select="budgetoutcomes" />
		<tr>
			<th scope="col" colspan="10">Operations</th>
		</tr>
		<tr>
      <td>
				<strong>
					Health Care Provider
				</strong>
			</td>
			<td>
				<strong>
					Insurance Provider
				</strong>
			</td>
			<td>
				<strong>
					Package Type
				</strong>
			</td>
			<td>
				<strong>
					Diagnosis Quality Rating
				</strong>
			</td>
			<td>
				<strong>
					Treatment Quality Rating
				</strong>
			</td>
			<td>
				<strong>
					Treatment Benefit Rating
				</strong>
			</td>
			<td>
				<strong>
					Treatment Cost Rating
				</strong>
			</td>
			<td>
				<strong>
					Knowledge Transfer Rating
				</strong>
			</td>
			<td>
				<strong>
					Constrained Choice Rating
				</strong>
			</td>
      <td>
				<strong>
					Insurance Coverage Rating
				</strong>
			</td>
		</tr>
		<tr>
      <td>
				<strong>
					Cost Rating
				</strong>
			</td>
			<td>
				<strong>
					Severity of Condition
				</strong>
			</td>
      <td>
				<strong>
					Is In Network
				</strong>
			</td>
      <td>
				<strong>
					Price Type
				</strong>
			</td>
			<td>
				<strong>
					Receiver Cost
				</strong>
			</td>
			<td>
				<strong>
					Incentives Cost
				</strong>
			</td>
			<td>
				<strong>
					Insurance Provider Cost
				</strong>
			</td>
			<td>
				<strong>
					Health Care Provider Cost
				</strong>
			</td>
			<td>
				<strong>
					Additional Cost
				</strong>
			</td>
			<td>
				<strong>
					Use Added Costs In Input
				</strong>
			</td>
		</tr>
    <tr>
      <td>
				<strong>
					Base Price
				</strong>
			</td>
      <td>
				<strong>
					Base Price Adjustment
				</strong>
			</td>
      <td>
				<strong>
					Adjusted Price
				</strong>
			</td>
      <td>
				<strong>
					Contracted Price
				</strong>
			</td>
			<td>
				<strong>
					List Price
				</strong>
			</td>
      <td>
				<strong>
					Market Price
				</strong>
			</td>
			<td>
				<strong>
					Production Cost Price
				</strong>
			</td>
      <td>
				<strong>
					Annual Premium Self
				</strong>
			</td>
      <td>
				<strong>
					Annual Premium Other
				</strong>
			</td>
      <td>
				<strong>
					Assigned Premium Cost
				</strong>
			</td>
		</tr>
    <tr>
      <td>
				<strong>
					Additional Cost Name 1
				</strong>
			</td>
			<td>
				<strong>
					Additional Price 1
				</strong>
			</td>
      <td>
				<strong>
					Additional Amount 1
				</strong>
			</td>
      <td>
				<strong>
					Additional Unit 1
				</strong>
			</td>
			<td>
				<strong>
					Additional Cost 1
				</strong>
			</td>
			<td>
				<strong>
					Additional Cost Name 2
				</strong>
			</td>
			<td>
				<strong>
					Additional Price 2
				</strong>
			</td>
      <td>
				<strong>
					Additional Amount 2
				</strong>
			</td>
      <td>
				<strong>
					Additional Unit 2
				</strong>
			</td>
			<td>
				<strong>
					Additional Cost 2
				</strong>
			</td>
		</tr>
    <tr>
      <td>
				<strong>
					CoPay 1 Amount
				</strong>
			</td>
			<td>
				<strong>
					CoPay 2 Amount
				</strong>
			</td>
      <td>
				<strong>
					CoPay 1 Rate
				</strong>
			</td>
      <td>
				<strong>
					CoPay 2 Rate
				</strong>
			</td>
			<td>
				<strong>
					Incentive 1 Amount
				</strong>
			</td>
			<td>
				<strong>
					Incentive 2 Amount
				</strong>
			</td>
			<td>
				<strong>
					Incentive 1 Rate
				</strong>
			</td>
			<td>
				<strong>
					Incentive 2 Rate
				</strong>
			</td>
			<td colspan="2">
			</td>
		</tr>
		<xsl:apply-templates select="budgetoperations" />
		<tr>
			<th scope="col" colspan="10"><strong>Time Period Totals</strong></th>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<tr>
			<th scope="col" colspan="10"><strong>Outcome</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <tr>
		        <td scope="col" colspan="10"><strong>Benefits</strong></td>
	      </tr>
        <tr>
			  <td>
					  <xsl:value-of select="@TPhysicalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEmotionalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TSocialHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEconomicHealthRating" />
			  </td>
        <td>
					  <xsl:value-of select="@THealthCareDeliveryRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TAverageBenefitRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TBeforeQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TAfterQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TBeforeYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TAfterYears" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TAfterYearsProb" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEquityMultiplier" />
			  </td>
			  <td>
					  <xsl:value-of select="@TQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@TICERQALY" />
			  </td>
			  <td>
					  <xsl:value-of select="@TTimeTradeoffYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TTTOQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@TOutputCost" />
			  </td>
			  <td>
					  <xsl:value-of select="@TBenefitAdjustment" />
			  </td>
			  <td>
					  <xsl:value-of select="@TAdjustedBenefit" />
			  </td>
        <td>
					  <xsl:value-of select="@TRealRate" />
			  </td>
      </tr>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <tr>
		        <td scope="col" colspan="10"><strong>Costs</strong></td>
	      </tr>
			  <tr>
          <td>
			    </td>
			    <td>
			    </td>
			    <td>
			    </td>
			    <td>
				    <xsl:value-of select="@TDiagnosisQualityRating"/>
			    </td>
			    <td>
				    <xsl:value-of select="@TTreatmentQualityRating"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TTreatmentBenefitRating"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TTreatmentCostRating"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TKnowledgeTransferRating"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TConstrainedChoiceRating"/>
			    </td>
          <td>
					    <xsl:value-of select="@TInsuranceCoverageRating"/>
			    </td>
		    </tr>
		    <tr>
          <td>
					    <xsl:value-of select="@TCostRating"/>
			    </td>
			    <td>
			    </td>
          <td>
			    </td>
          <td>
			    </td>
			    <td>
					    <xsl:value-of select="@TReceiverCost"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TIncentivesCost"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TInsuranceProviderCost"/>
			    </td>
			    <td>
					    <xsl:value-of select="@THealthCareProviderCost"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TAdditionalCost"/>
			    </td>
			    <td>
			    </td>
		    </tr>
        <tr>
          <td>
					    <xsl:value-of select="@TBasePrice"/>
			    </td>
          <td>
					    <xsl:value-of select="@TBasePriceAdjustment"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAdjustedPrice"/>
			    </td>
          <td>
					    <xsl:value-of select="@TContractedPrice"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TListPrice"/>
			    </td>
          <td>
					    <xsl:value-of select="@TMarketPrice"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TProductionCostPrice"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAnnualPremium1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAnnualPremium2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TAssignedPremiumCost"/>
			    </td>
		    </tr>
        <tr>
          <td>
			    </td>
			    <td>
					    <xsl:value-of select="@TAdditionalPrice1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAdditionalAmount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAdditionalUnit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TAdditionalCost1"/>
			    </td>
			    <td>
			    </td>
			    <td>
					    <xsl:value-of select="@TAdditionalPrice2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAdditionalAmount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TAdditionalUnit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TAdditionalCost2"/>
			    </td>
		    </tr>
        <tr>
          <td>
					    <xsl:value-of select="@TCoPay1Amount"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TCoPay2Amount"/>
			    </td>
          <td>
					    <xsl:value-of select="@TCoPay1Rate"/>
			    </td>
          <td>
					    <xsl:value-of select="@TCoPay2Rate"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TIncentive1Amount"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TIncentive2Amount"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TIncentive1Rate"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TIncentive2Rate"/>
			    </td>
			    <td colspan="2">
			    </td>
		    </tr>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <tr>
		    <td scope="col" colspan="10"><strong>Costs</strong></td>
	    </tr>
			<tr>
        <td>
				  <xsl:value-of select="@HealthCareProvider" />
			  </td>
			  <td>
				  <xsl:value-of select="@InsuranceProvider"/>
			  </td>
			  <td>
				  <xsl:value-of select="@PackageType"/>
			  </td>
			  <td>
				  <xsl:value-of select="@DiagnosisQualityRating"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TreatmentQualityRating"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TreatmentBenefitRating"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TreatmentCostRating"/>
			  </td>
			  <td>
					  <xsl:value-of select="@KnowledgeTransferRating"/>
			  </td>
			  <td>
					  <xsl:value-of select="@ConstrainedChoiceRating"/>
			  </td>
        <td>
					  <xsl:value-of select="@InsuranceCoverageRating"/>
			  </td>
		  </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@CostRating"/>
			  </td>
			  <td>
					  <xsl:value-of select="@ConditionSeverity"/>
			  </td>
        <td>
					  <xsl:value-of select="@IsInNetwork"/>
			  </td>
        <td>
					  <xsl:value-of select="@HC1PriceType"/>
			  </td>
			  <td>
					  <xsl:value-of select="@ReceiverCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IncentivesCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@InsuranceProviderCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@HealthCareProviderCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@UseAddedCostsInInput"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@BasePrice"/>
			  </td>
        <td>
					  <xsl:value-of select="@BasePriceAdjustment"/>
			  </td>
        <td>
					  <xsl:value-of select="@AdjustedPrice"/>
			  </td>
        <td>
					  <xsl:value-of select="@ContractedPrice"/>
			  </td>
			  <td>
					  <xsl:value-of select="@ListPrice"/>
			  </td>
        <td>
					  <xsl:value-of select="@MarketPrice"/>
			  </td>
			  <td>
					  <xsl:value-of select="@ProductionCostPrice"/>
			  </td>
        <td>
					  <xsl:value-of select="@AnnualPremium1"/>
			  </td>
        <td>
					  <xsl:value-of select="@AnnualPremium2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AssignedPremiumCost"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@AdditionalName1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalPrice1"/>
			  </td>
        <td>
					  <xsl:value-of select="@AdditionalAmount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@AdditionalUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalCost1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalName2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalPrice2"/>
			  </td>
        <td>
					  <xsl:value-of select="@AdditionalAmount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@AdditionalUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@AdditionalCost2"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@CoPay1Amount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@CoPay2Amount"/>
			  </td>
        <td>
					  <xsl:value-of select="@CoPay1Rate"/>
			  </td>
        <td>
					  <xsl:value-of select="@CoPay2Rate"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Incentive1Amount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Incentive2Amount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Incentive1Rate"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Incentive2Rate"/>
			  </td>
			  <td colspan="2">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Input Quality Assessment : </strong><xsl:value-of select="@InputQualityAssessment" />
			  </td>
      </tr>
      <tr>
        <td colspan="10">
          <strong>Additional Costs Description : </strong><xsl:value-of select="@AdditionalCostsDescription" />
			  </td>
      </tr>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
			<tr>
        <td>
					  <xsl:value-of select="@PhysicalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@EmotionalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@SocialHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@EconomicHealthRating" />
			  </td>
        <td>
					  <xsl:value-of select="@HealthCareDeliveryRating" />
			  </td>
        <td>
					  <xsl:value-of select="@AverageBenefitRating" />
			  </td>
        <td>
					  <xsl:value-of select="@BeforeQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@AfterQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@BeforeYears" />
			  </td>
        <td>
					  <xsl:value-of select="@AfterYears" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@AfterYearsProb" />
			  </td>
			  <td>
					  <xsl:value-of select="@EquityMultiplier" />
			  </td>
			  <td>
					  <xsl:value-of select="@QALY" />
			  </td>
        <td>
					  <xsl:value-of select="@ICERQALY" />
			  </td>
			  <td>
					  <xsl:value-of select="@TimeTradeoffYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TTOQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@OutputCost" />
			  </td>
			  <td>
					  <xsl:value-of select="@BenefitAdjustment" />
			  </td>
			  <td>
					  <xsl:value-of select="@AdjustedBenefit" />
			  </td>
        <td>
					  <xsl:value-of select="@RealRate" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@OutputEffect1Name" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Unit" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Amount" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Price" />
			  </td>
        <td>
					  <xsl:value-of select="@OutputEffect1Cost" />
			  </td>
        <td>
					  <xsl:value-of select="@Age" />
			  </td>
			  <td>
					  <xsl:value-of select="@Gender" />
			  </td>
			  <td>
					  <xsl:value-of select="@EducationYears" />
			  </td>
        <td>
					  <xsl:value-of select="@Race" />
			  </td>
        <td>
					  <xsl:value-of select="@WorkStatus" />
			  </td>
      </tr>
      <tr>
        <td colspan="10">
          <strong>Benefit Assessment : </strong><xsl:value-of select="@BenefitAssessment" />
			  </td>
      </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
