<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
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
			<xsl:apply-templates select="outputgroup" />
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
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<h4>
      <strong>Output Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		<h4>
      <strong>Output Series </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'outputgroup')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Group Details</strong>
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
		<xsl:if test="($localName != 'outputgroup')">
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

