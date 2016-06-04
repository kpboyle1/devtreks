<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="budgetgroup" />
			<div>
				<a id="aFeedback" name="Feedback">
					<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
					Feedback About <xsl:value-of select="$selectedFileURIPattern" />
				</a>
      </div>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<h4 class="ui-bar-b">
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
		<h4>
      <strong>Budget Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<h4>
      <strong>Budget</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@EnterpriseName" />
    </h4>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="budgetoutcomes" />
    <div>
      <strong>Operations</strong>
    </div>
		<xsl:apply-templates select="budgetoperations" />
    <h4>
      Time Period Totals
    </h4>
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
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
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
    <h4>
      <strong>Operation</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Benefits</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Physical Health Rating : <xsl:value-of select="@TPhysicalHealthRating" />
          </div>
          <div class="ui-block-b">
            Emotional Health Rating : <xsl:value-of select="@TEmotionalHealthRating" />
          </div>
          <div class="ui-block-a">
            Social Health Rating : <xsl:value-of select="@TSocialHealthRating" />
          </div>
          <div class="ui-block-b">
            Economic Health Rating : <xsl:value-of select="@TEconomicHealthRating" />
          </div>
          <div class="ui-block-a">
            Health Care Delivery Rating : <xsl:value-of select="@THealthCareDeliveryRating" />
          </div>
          <div class="ui-block-b">
            Average Benefit Rating : <xsl:value-of select="@TAverageBenefitRating" />
          </div>
          <div class="ui-block-a">
            Before Treatment QOL Rating : <xsl:value-of select="@TBeforeQOLRating" />
          </div>
          <div class="ui-block-b">
            After Treatment QOL Rating : <xsl:value-of select="@TAfterQOLRating" />
          </div>
          <div class="ui-block-a">
            Before Treatment Years : <xsl:value-of select="@TBeforeYears" />
          </div>
          <div class="ui-block-b">
            After Treatment Years : <xsl:value-of select="@TAfterYears" />
          </div>
          <div class="ui-block-a">
            Probability of After Treatment Years : <xsl:value-of select="@TAfterYearsProb" />
          </div>
          <div class="ui-block-b">
            Equity Multiplier : <xsl:value-of select="@TEquityMultiplier" />
          </div>
          <div class="ui-block-a">
            Quality Adjusted Life Years (QALY) : <xsl:value-of select="@TQALY" />
          </div>
          <div class="ui-block-b">
            Incremental QALY :  <xsl:value-of select="@TICERQALY" />
          </div>
          <div class="ui-block-a">
            Time Tradeoff Years : <xsl:value-of select="@TTimeTradeoffYears" />
          </div>
          <div class="ui-block-b">
            TTO QALY : <xsl:value-of select="@TTTOQALY" />
          </div>
          <div class="ui-block-a">
            Output Cost : <xsl:value-of select="@TOutputCost" />
          </div>
          <div class="ui-block-b">
            Benefit Adjustment : <xsl:value-of select="@TBenefitAdjustment" />
          </div>
          <div class="ui-block-a">
            Adjusted Benefit : <xsl:value-of select="@TAdjustedBenefit" />
          </div>
          <div class="ui-block-b">
            Discount (real) Rate : <xsl:value-of select="@TRealRate" />
          </div>
        </div>
      </div>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Costs</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            
          </div>
          <div class="ui-block-b">
            
          </div>
          <div class="ui-block-a">
            
          </div>
          <div class="ui-block-b">
            Diagnosis Quality Rating : <xsl:value-of select="@TDiagnosisQualityRating"/>
          </div>
          <div class="ui-block-a">
           Treatment Quality Rating : <xsl:value-of select="@TTreatmentQualityRating"/>
          </div>
          <div class="ui-block-b">
           Treatment Benefit Rating : <xsl:value-of select="@TTreatmentBenefitRating"/>
          </div>
          <div class="ui-block-a">
            Treatment Cost Rating : <xsl:value-of select="@TTreatmentCostRating"/>
          </div>
          <div class="ui-block-b">
           Knowledge Transfer Rating : <xsl:value-of select="@TKnowledgeTransferRating"/>
          </div>
          <div class="ui-block-a">
            Constrained Choice Rating : <xsl:value-of select="@TConstrainedChoiceRating"/>
          </div>
          <div class="ui-block-b">
            Insurance Coverage Rating : <xsl:value-of select="@TInsuranceCoverageRating"/>
          </div>
          <div class="ui-block-a">
            Cost Rating :  <xsl:value-of select="@TCostRating"/>
          </div>
          <div class="ui-block-b">
           
          </div>
          <div class="ui-block-a">
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Receiver Cost : <xsl:value-of select="@TReceiverCost"/>
          </div>
          <div class="ui-block-b">
           Incentives Cost : <xsl:value-of select="@TIncentivesCost"/>
          </div>
          <div class="ui-block-a">
            Insurance Provider Cost : <xsl:value-of select="@TInsuranceProviderCost"/>
          </div>
          <div class="ui-block-b">
            Health Care Provider Cost : <xsl:value-of select="@THealthCareProviderCost"/>
          </div>
          <div class="ui-block-a">
             Additional Cost : <xsl:value-of select="@TAdditionalCost"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Base Price : <xsl:value-of select="@TBasePrice"/>
          </div>
          <div class="ui-block-b">
           Base Price Adjustment : <xsl:value-of select="@TBasePriceAdjustment"/>
          </div>
          <div class="ui-block-a">
           Adjusted Price : <xsl:value-of select="@TAdjustedPrice"/>
          </div>
          <div class="ui-block-b">
           Contracted Price : <xsl:value-of select="@TContractedPrice"/>
          </div>
          <div class="ui-block-a">
            List Price : <xsl:value-of select="@TListPrice"/>
          </div>
          <div class="ui-block-b">
            Market Price : <xsl:value-of select="@TMarketPrice"/>
          </div>
          <div class="ui-block-a">
            Production Cost Price : <xsl:value-of select="@TProductionCostPrice"/>
          </div>
          <div class="ui-block-b">
           Annual Premium Self : <xsl:value-of select="@TAnnualPremium1"/>
          </div>
          <div class="ui-block-a">
            Annual Premium Other : <xsl:value-of select="@TAnnualPremium2"/>
          </div>
          <div class="ui-block-b">
           Assigned Premium Cost : <xsl:value-of select="@TAssignedPremiumCost"/>
          </div>
          <div class="ui-block-a">
            
          </div>
          <div class="ui-block-b">
           Additional Price 1 : <xsl:value-of select="@TAdditionalPrice1"/>
          </div>
          <div class="ui-block-a">
            Additional Amount 1 : <xsl:value-of select="@TAdditionalAmount1"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Additional Cost 1 : <xsl:value-of select="@TAdditionalCost1"/>
          </div>
          <div class="ui-block-a">
          </div>
          <div class="ui-block-b">
           Additional Price 2 : <xsl:value-of select="@TAdditionalPrice2"/>
          </div>
          <div class="ui-block-a">
            Additional Amount 2 : <xsl:value-of select="@TAdditionalAmount2"/>
          </div>
          <div class="ui-block-b">
           
          </div>
          <div class="ui-block-a">
            Additional Cost 2 : <xsl:value-of select="@TAdditionalCost2"/>
          </div>
          <div class="ui-block-a">
            CoPay 1 Amount : <xsl:value-of select="@TCoPay1Amount"/>
          </div>
          <div class="ui-block-b">
           CoPay 2 Amount : <xsl:value-of select="@TCoPay2Amount"/>
          </div>
          <div class="ui-block-a">
            CoPay 1 Rate : <xsl:value-of select="@TCoPay1Rate"/>
          </div>
          <div class="ui-block-b">
           CoPay 2 Rate : <xsl:value-of select="@TCoPay2Rate"/>
          </div>
          <div class="ui-block-a">
            Incentive 1 Amount : <xsl:value-of select="@TIncentive1Amount"/>
          </div>
          <div class="ui-block-b">
           Incentive 2 Amount : <xsl:value-of select="@TIncentive2Amount"/>
          </div>
          <div class="ui-block-a">
            Incentive 1 Rate : <xsl:value-of select="@TIncentive1Rate"/>
          </div>
          <div class="ui-block-b">
           Incentive 2 Rate : <xsl:value-of select="@TIncentive2Rate"/>
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Health Care Provider : <xsl:value-of select="@HealthCareProvider" />
          </div>
          <div class="ui-block-b">
            Insurance Provider : <xsl:value-of select="@InsuranceProvider"/>
          </div>
          <div class="ui-block-a">
            Package Type : <xsl:value-of select="@PackageType"/>
          </div>
          <div class="ui-block-b">
            Diagnosis Quality Rating : <xsl:value-of select="@DiagnosisQualityRating"/>
          </div>
          <div class="ui-block-a">
           Treatment Quality Rating : <xsl:value-of select="@TreatmentQualityRating"/>
          </div>
          <div class="ui-block-b">
           Treatment Benefit Rating : <xsl:value-of select="@TreatmentBenefitRating"/>
          </div>
          <div class="ui-block-a">
            Treatment Cost Rating : <xsl:value-of select="@TreatmentCostRating"/>
          </div>
          <div class="ui-block-b">
           Knowledge Transfer Rating : <xsl:value-of select="@KnowledgeTransferRating"/>
          </div>
          <div class="ui-block-a">
            Constrained Choice Rating : <xsl:value-of select="@ConstrainedChoiceRating"/>
          </div>
          <div class="ui-block-b">
            Insurance Coverage Rating : <xsl:value-of select="@InsuranceCoverageRating"/>
          </div>
          <div class="ui-block-a">
            Cost Rating :  <xsl:value-of select="@CostRating"/>
          </div>
          <div class="ui-block-b">
           Severity of Condition : <xsl:value-of select="@ConditionSeverity"/>
          </div>
          <div class="ui-block-a">
            Is In Network : <xsl:value-of select="@IsInNetwork"/>
          </div>
          <div class="ui-block-b">
           Price Type : <xsl:value-of select="@HC1PriceType"/>
          </div>
          <div class="ui-block-a">
            Receiver Cost : <xsl:value-of select="@ReceiverCost"/>
          </div>
          <div class="ui-block-b">
           Incentives Cost : <xsl:value-of select="@IncentivesCost"/>
          </div>
          <div class="ui-block-a">
            Insurance Provider Cost : <xsl:value-of select="@InsuranceProviderCost"/>
          </div>
          <div class="ui-block-b">
            Health Care Provider Cost : <xsl:value-of select="@HealthCareProviderCost"/>
          </div>
          <div class="ui-block-a">
             Additional Cost : <xsl:value-of select="@AdditionalCost"/>
          </div>
          <div class="ui-block-b">
           Use Added Costs In Input : <xsl:value-of select="@UseAddedCostsInInput"/>
          </div>
          <div class="ui-block-a">
            Base Price : <xsl:value-of select="@BasePrice"/>
          </div>
          <div class="ui-block-b">
           Base Price Adjustment : <xsl:value-of select="@BasePriceAdjustment"/>
          </div>
          <div class="ui-block-a">
           Adjusted Price : <xsl:value-of select="@AdjustedPrice"/>
          </div>
          <div class="ui-block-b">
           Contracted Price : <xsl:value-of select="@ContractedPrice"/>
          </div>
          <div class="ui-block-a">
            List Price : <xsl:value-of select="@ListPrice"/>
          </div>
          <div class="ui-block-b">
            Market Price : <xsl:value-of select="@MarketPrice"/>
          </div>
          <div class="ui-block-a">
            Production Cost Price : <xsl:value-of select="@ProductionCostPrice"/>
          </div>
          <div class="ui-block-b">
           Annual Premium Self : <xsl:value-of select="@AnnualPremium1"/>
          </div>
          <div class="ui-block-a">
            Annual Premium Other : <xsl:value-of select="@AnnualPremium2"/>
          </div>
          <div class="ui-block-b">
           Assigned Premium Cost : <xsl:value-of select="@AssignedPremiumCost"/>
          </div>
          <div class="ui-block-a">
            Additional Cost Name 1 : <xsl:value-of select="@AdditionalName1"/>
          </div>
          <div class="ui-block-b">
           Additional Price 1 : <xsl:value-of select="@AdditionalPrice1"/>
          </div>
          <div class="ui-block-a">
            Additional Amount 1 : <xsl:value-of select="@AdditionalAmount1"/>
          </div>
          <div class="ui-block-b">
           Additional Unit 1 : <xsl:value-of select="@AdditionalUnit1"/>
          </div>
          <div class="ui-block-a">
            Additional Cost 1 : <xsl:value-of select="@AdditionalCost1"/>
          </div>
          <div class="ui-block-a">
            Additional Cost Name 2 : <xsl:value-of select="@AdditionalName2"/>
          </div>
          <div class="ui-block-b">
           Additional Price 2 : <xsl:value-of select="@AdditionalPrice2"/>
          </div>
          <div class="ui-block-a">
            Additional Amount 2 : <xsl:value-of select="@AdditionalAmount2"/>
          </div>
          <div class="ui-block-b">
           Additional Unit 2 : <xsl:value-of select="@AdditionalUnit2"/>
          </div>
          <div class="ui-block-a">
            Additional Cost 2 : <xsl:value-of select="@AdditionalCost2"/>
          </div>
          <div class="ui-block-a">
            CoPay 1 Amount : <xsl:value-of select="@CoPay1Amount"/>
          </div>
          <div class="ui-block-b">
           CoPay 2 Amount : <xsl:value-of select="@CoPay2Amount"/>
          </div>
          <div class="ui-block-a">
            CoPay 1 Rate : <xsl:value-of select="@CoPay1Rate"/>
          </div>
          <div class="ui-block-b">
           CoPay 2 Rate : <xsl:value-of select="@CoPay2Rate"/>
          </div>
          <div class="ui-block-a">
            Incentive 1 Amount : <xsl:value-of select="@Incentive1Amount"/>
          </div>
          <div class="ui-block-b">
           Incentive 2 Amount : <xsl:value-of select="@Incentive2Amount"/>
          </div>
          <div class="ui-block-a">
            Incentive 1 Rate : <xsl:value-of select="@Incentive1Rate"/>
          </div>
          <div class="ui-block-b">
           Incentive 2 Rate : <xsl:value-of select="@Incentive2Rate"/>
          </div>
        </div>
         <div >
			    <strong>Input Quality Assessment : </strong><xsl:value-of select="@InputQualityAssessment" />
	      </div>
         <div >
			    <strong>Additional Costs Description : </strong><xsl:value-of select="@AdditionalCostsDescription" />
	      </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Physical Health Rating : <xsl:value-of select="@PhysicalHealthRating" />
          </div>
          <div class="ui-block-b">
            Emotional Health Rating : <xsl:value-of select="@EmotionalHealthRating" />
          </div>
          <div class="ui-block-a">
            Social Health Rating : <xsl:value-of select="@SocialHealthRating" />
          </div>
          <div class="ui-block-b">
            Economic Health Rating : <xsl:value-of select="@EconomicHealthRating" />
          </div>
          <div class="ui-block-a">
            Health Care Delivery Rating : <xsl:value-of select="@HealthCareDeliveryRating" />
          </div>
          <div class="ui-block-b">
            Average Benefit Rating : <xsl:value-of select="@AverageBenefitRating" />
          </div>
          <div class="ui-block-a">
            Before Treatment QOL Rating : <xsl:value-of select="@BeforeQOLRating" />
          </div>
          <div class="ui-block-b">
            After Treatment QOL Rating : <xsl:value-of select="@AfterQOLRating" />
          </div>
          <div class="ui-block-a">
            Before Treatment Years : <xsl:value-of select="@BeforeYears" />
          </div>
          <div class="ui-block-b">
            After Treatment Years : <xsl:value-of select="@AfterYears" />
          </div>
          <div class="ui-block-a">
            Probability of After Treatment Years : <xsl:value-of select="@AfterYearsProb" />
          </div>
          <div class="ui-block-b">
            Equity Multiplier : <xsl:value-of select="@EquityMultiplier" />
          </div>
          <div class="ui-block-a">
            Quality Adjusted Life Years (QALY) : <xsl:value-of select="@QALY" />
          </div>
          <div class="ui-block-b">
            Incremental QALY :  <xsl:value-of select="@ICERQALY" />
          </div>
          <div class="ui-block-a">
            Time Tradeoff Years : <xsl:value-of select="@TimeTradeoffYears" />
          </div>
          <div class="ui-block-b">
            TTO QALY : <xsl:value-of select="@TTOQALY" />
          </div>
          <div class="ui-block-a">
            Output Cost : <xsl:value-of select="@OutputCost" />
          </div>
          <div class="ui-block-b">
            Benefit Adjustment : <xsl:value-of select="@BenefitAdjustment" />
          </div>
          <div class="ui-block-a">
            Adjusted Benefit : <xsl:value-of select="@AdjustedBenefit" />
          </div>
          <div class="ui-block-b">
            Discount (real) Rate : <xsl:value-of select="@RealRate" />
          </div>
          <div class="ui-block-a">
            Output Effect1 Name : <xsl:value-of select="@OutputEffect1Name" />
          </div>
          <div class="ui-block-b">
            Output Effect1 Unit : <xsl:value-of select="@OutputEffect1Unit" />
          </div>
          <div class="ui-block-a">
            Output Effect1 Amount : <xsl:value-of select="@OutputEffect1Amount" />
          </div>
          <div class="ui-block-b">
            Output Effect1 Price : <xsl:value-of select="@OutputEffect1Price" />
          </div>
          <div class="ui-block-a">
            Output Effect1 Cost : <xsl:value-of select="@OutputEffect1Cost" />
          </div>
          <div class="ui-block-b">
            Age : <xsl:value-of select="@Age" />
          </div>
          <div class="ui-block-a">
            Gender : <xsl:value-of select="@Gender" />
          </div>
          <div class="ui-block-b">
            Education Years : <xsl:value-of select="@EducationYears" />
          </div>
          <div class="ui-block-a">
            Race : <xsl:value-of select="@Race" />
          </div>
          <div class="ui-block-b">
            Work Status : <xsl:value-of select="@WorkStatus" />
          </div>
        </div>
        <div >
			    <strong>Benefit Assessment :</strong> <xsl:value-of select="@BenefitAssessment" />
	      </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
