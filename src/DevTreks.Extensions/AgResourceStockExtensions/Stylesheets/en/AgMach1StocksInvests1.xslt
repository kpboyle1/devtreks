<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
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
			<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <h4>
      <strong>Investment Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@PracticeName" />
    </h4>
    <!--<div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="investmentoutcomes" />-->
    <div>
      <strong>Components</strong>
    </div>
		<xsl:apply-templates select="investmentcomponents" />
    <h4>
      Time Period Totals
    </h4>
    <xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<h4>
      <strong>Component</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
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
		<xsl:if test="($localName != 'investmentinput' and $localName != 'investmentoutput')">
      <xsl:if test="($localName != 'investmentcomponent')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
          <h4 class="ui-bar-b">
            <strong>Benefit Details</strong>
          </h4>
        </div>
      </xsl:if>
      <xsl:if test="($localName != 'investmentoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Market Value : <xsl:value-of select="@TMarketValue" />
          </div>
          <div class="ui-block-b">
            Salvage Value : <xsl:value-of select="@TSalvageValue"/>
          </div>
          <div class="ui-block-a">
            Cap Recov Cost : <xsl:value-of select="@TCapitalRecoveryCost"/>
          </div>
          <div class="ui-block-b">
            THI Cost : <xsl:value-of select="@TTaxesHousingInsuranceCost"/>
          </div>
          <div class="ui-block-a">
            Starting Hrs : <xsl:value-of select="@TStartingHrs"/>
          </div>
          <div class="ui-block-b">
           Planned Use Hrs : <xsl:value-of select="@TPlannedUseHrs"/>
          </div>
          <div class="ui-block-a">
            Useful Life Hrs : <xsl:value-of select="@TUsefulLifeHrs"/>
          </div>
          <div class="ui-block-b">
           Horsepower : <xsl:value-of select="@THP"/>
          </div>
          <div class="ui-block-a">
            Speed : <xsl:value-of select="@TSpeed"/>
          </div>
          <div class="ui-block-b">
            Width : <xsl:value-of select="@TWidth"/>
          </div>
          <div class="ui-block-a">
            Fuel Amount :  <xsl:value-of select="@TFuelAmount"/>
          </div>
          <div class="ui-block-b">
           Fuel Price : <xsl:value-of select="@TFuelPrice"/>
          </div>
          <div class="ui-block-a">
            Fuel Cost : <xsl:value-of select="@TFuelCost"/>
          </div>
          <div class="ui-block-b">
           Labor Amount : <xsl:value-of select="@TLaborAmount"/>
          </div>
          <div class="ui-block-a">
            Labor Price : <xsl:value-of select="@TLaborPrice"/>
          </div>
          <div class="ui-block-b">
           Labor Cost : <xsl:value-of select="@TLaborCost"/>
          </div>
          <div class="ui-block-a">
            Lube Oil Amounts : <xsl:value-of select="@TLubeOilAmount"/>
          </div>
          <div class="ui-block-b">
           Lube Oil Price : <xsl:value-of select="@TLubeOilPrice"/>
          </div>
          <div class="ui-block-a">
           Lube Oil Cost : <xsl:value-of select="@TLubeOilCost"/>
          </div>
          <div class="ui-block-b">
           Repair Cost : <xsl:value-of select="@TRepairCost"/>
          </div>
          <div class="ui-block-a">
           Equiv PTO HP : <xsl:value-of select="@THPPTOEquiv"/>
          </div>
          <div class="ui-block-b">
           Field Efficiency : <xsl:value-of select="@TFieldEffTypical"/>
          </div>
          <div class="ui-block-a">
            Operating Cost : <xsl:value-of select="@TOC"/>
          </div>
          <div class="ui-block-b">
           Alloc OH Cost : <xsl:value-of select="@TAOH"/>
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'investmentinput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Market Value : <xsl:value-of select="@MarketValue" />
          </div>
          <div class="ui-block-b">
            Salvage Value : <xsl:value-of select="@SalvageValue"/>
          </div>
          <div class="ui-block-a">
            Cap Recov Cost : <xsl:value-of select="@CapitalRecoveryCost"/>
          </div>
          <div class="ui-block-b">
            THI Cost : <xsl:value-of select="@TaxesHousingInsuranceCost"/>
          </div>
          <div class="ui-block-a">
            Starting Hrs : <xsl:value-of select="@StartingHrs"/>
          </div>
          <div class="ui-block-b">
           Planned Use Hrs : <xsl:value-of select="@PlannedUseHrs"/>
          </div>
          <div class="ui-block-a">
            Useful Life Hrs : <xsl:value-of select="@UsefulLifeHrs"/>
          </div>
          <div class="ui-block-b">
           Horsepower : <xsl:value-of select="@HP"/>
          </div>
          <div class="ui-block-a">
            Speed : <xsl:value-of select="@FieldSpeedTypical"/>
          </div>
          <div class="ui-block-b">
            Width : <xsl:value-of select="@Width"/>
          </div>
          <div class="ui-block-a">
            Fuel Amount :  <xsl:value-of select="@FuelAmount"/>
          </div>
          <div class="ui-block-b">
           Fuel Price : <xsl:value-of select="@FuelPrice"/>
          </div>
          <div class="ui-block-a">
            Fuel Cost : <xsl:value-of select="@FuelCost"/>
          </div>
          <div class="ui-block-b">
           Labor Amount : <xsl:value-of select="@LaborAmount"/>
          </div>
          <div class="ui-block-a">
            Labor Price : <xsl:value-of select="@LaborPrice"/>
          </div>
          <div class="ui-block-b">
           Labor Cost : <xsl:value-of select="@LaborCost"/>
          </div>
          <div class="ui-block-a">
            Lube Oil Amounts : <xsl:value-of select="@LubeOilAmount"/>
          </div>
          <div class="ui-block-b">
           Lube Oil Price : <xsl:value-of select="@LubeOilPrice"/>
          </div>
          <div class="ui-block-a">
           Lube Oil Cost : <xsl:value-of select="@LubeOilCost"/>
          </div>
          <div class="ui-block-b">
           Repair Cost : <xsl:value-of select="@RepairCost"/>
          </div>
          <div class="ui-block-a">
           Equiv PTO HP : <xsl:value-of select="@HPPTOEquiv"/>
          </div>
          <div class="ui-block-b">
           Field Efficiency : <xsl:value-of select="@FieldEffTypical"/>
          </div>
          <div class="ui-block-a">
            Amount : <xsl:value-of select="@ServiceCapacity"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Operating Cost : <xsl:value-of select="@TOC"/>
          </div>
          <div class="ui-block-b">
           Alloc OH Cost : <xsl:value-of select="@TAOH"/>
          </div>
        </div>
         <div >
			    <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
	      </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'investmentoutput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>