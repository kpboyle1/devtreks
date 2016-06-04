<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, May -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!--array holding references to constants, locals, lists ...-->
	<xsl:param name="linkedListsArray" />
	<!-- calcs -->
	<xsl:param name="saveMethod" />
	<!-- default linked view -->
	<xsl:param name="defaultLinkedViewId" />
	<!-- the last step is used to display different parts of the calculator-->
	<xsl:param name="lastStepNumber" />
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
	<!-- the node being calculated (custom docs' nodename can be a devpack node, while this might be budgetgroup) -->
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
	<!-- what is the club's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<div class="box">
			<xsl:if test="($serverActionType = 'linkedviews')">
				<div id="stepsmenu">
					<xsl:value-of select="DisplayDevPacks:WriteMenuSteps('4')"/>
				</div>
			</xsl:if>
			<xsl:apply-templates select="root" />
			<div>
				<br />
				<strong>Current view of document</strong>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="root">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
		<div id="divstepzero">
      <xsl:choose>
        <xsl:when test="($docToCalcNodeName = 'inputgroup' 
							or $docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries'
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
					sure this is the right calculator?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b">Irrigation Cost Calculation View</h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool calculates costs for irrigation input uris. The discounted totals include operating,
					allocated overhead, and capital costs. Operating costs include fuel, repair and maintenance, water, and labor costs. Allocated overhead costs include capital recovery, and taxes, housing and insurance costs. 
          All costs are calculated on a per volume of applied water basis (acin or m3). The amount is the total water applied per season. 
          For example, 25 acin applied per acre per season * $5 total cost per acin applied = $125 total irrigation cost per acre per season.
			</p>
			<p>
				<strong>Calculation View Description</strong>
				<br />
				<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorDescription" />
			</p>
			<p>
				<strong>Version: </strong>
				<xsl:value-of select="linkedview[@Id=$linkedviewid]/@Version"/>
			</p>
			<p>
				<a id="aFeedback" name="Feedback">
					<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
					Feedback About <xsl:value-of select="$selectedFileURIPattern" />
				</a>
			</p>
		</div>
		<div id="stepmessage" />
		<xsl:apply-templates select="linkedview" />
	</xsl:template>
	<xsl:template match="linkedview">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
		<xsl:if test="(@Id = $linkedviewid) or (@Id = 1)">
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@CalculatorName,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<div id="divstepone">
        <h4 class="ui-bar-b"><strong>Step 1 of 4. Make Selections</strong></h4>
		    <xsl:variable name="calcParams1">'&amp;step=steptwo<xsl:value-of select="$calcParams" />'</xsl:variable>
			  <xsl:if test="($viewEditType = 'full')">
				  <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams1)"/>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'steptwo')">
          <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			  </xsl:if>
        <br />
			  <div>
          <label for="CalculatorName" class="ui-hidden-accessible"></label>
					<input id="CalculatorName" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
            </xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					</input>
				</div>
					<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
        <div >
				  <label for="lblDescription">Description</label>
				  <textarea class="Text75H100PCW" id="lblDescription" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorDescription;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@CalculatorDescription" />
				  </textarea>
			  </div>
        <div >
				  <label for="lblMediaURL">Media URL</label>
				  <textarea class="Text75H100PCW" id="lblMediaURL" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MediaURL;string;500</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@MediaURL" />
				  </textarea>
			  </div>
		  </div>
      <div id="divsteptwo">
        <h4 class="ui-bar-b"><strong>Step 2 of 4. Make Selections</strong></h4>
		    <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			  <xsl:if test="($viewEditType = 'full')">
			    <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'stepthree')">
          <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			  </xsl:if>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
        <h4>Relations</h4>
	      <xsl:if test="($docToCalcNodeName != 'inputseries' or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				  <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
            @UseSameCalculator, @Overwrite)"/>
		    </xsl:if>
        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
            @WhatIfTagName, @RelatedCalculatorsType)"/>
        </div>
			  <xsl:value-of select="DisplayDevPacks:WriteSelectListsForPrices(
				  @UnitGroupId, $searchurl, $serverSubActionType, 'full', 
				  @PriceGas, @PriceDiesel, @PriceLP, @PriceNG,
				  @PriceElectric, @PriceOil, @PriceRegularLabor, 
				  @PriceMachineryLabor, @PriceSupervisorLabor, @TaxPercent, 
				  @InsurePercent, @HousingPercent)"/>
      </div>
			<div id="divstepthree">
        <h4 class="ui-bar-b"><strong>Step 3 of 4. Calculate</strong></h4>
        <xsl:variable name="calcParams3">'&amp;step=stepfour<xsl:value-of select="$calcParams" />'</xsl:variable>
			  <xsl:if test="($viewEditType = 'full')">
				  <xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'stepfour')">
            <h4 class="ui-bar-b"><strong>Success. Please review the calculations below.</strong></h4>
			  </xsl:if>
        <xsl:choose>
				<xsl:when test="($docToCalcNodeName = 'inputgroup' 
						or $docToCalcNodeName = 'input' 
						or $docToCalcNodeName = 'inputseries'
						or contains($docToCalcNodeName, 'devpack') 
						or contains($docToCalcNodeName, 'linkedview'))">
					<xsl:if test="(@RepairCost >= 0)
						or (@CapitalRecoveryCost >= 0)">
            <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" data-mini="true">
              <h4 class="ui-bar-b">
                <strong>Operating Costs</strong>
              </h4>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Total Engine Flywheel Power (hp) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Total Engine Flywheel Power (kW) :
                  </xsl:if>
                  <xsl:value-of select="@EngineFlywheelPower" />
                </div>
                <div class="ui-block-b">
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Water Horsepower (hp) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Water Horsepower (kW) :
                  </xsl:if>
                  <xsl:value-of select="@WaterHP" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Brake Horsepower (hp) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Brake Horsepower (kW) :
                  </xsl:if>
                  <xsl:value-of select="@BrakeHP" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Actual Fuel Amount (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Actual Fuel Amount  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@FuelAmount" />
                </div>
                <div class="ui-block-b">
                  Fuel Unit:
                  <xsl:value-of select="@FuelUnit" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Fuel Cost (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Fuel Cost  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@FuelCost" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Water pumped (acre inches/hour) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Water pumped (cubic meters/hour) :
                  </xsl:if>
                  <xsl:value-of select="@PumpCapacity" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Required Fuel Amount (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Required Fuel Amount  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@FuelAmountRequired" />
                </div>
                <div class="ui-block-b">
                  Pumping Plant Performance:
                  <xsl:value-of select="@PumpingPlantPerformance" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Season Applied Amount (acre inches) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Season Applied Amount (cubic meters) :
                  </xsl:if>
                  <xsl:value-of select="@SeasonWaterApplied" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Pump Hours Needed per Season (per acre) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Pump Hours Needed per Season (per hectare) :
                  </xsl:if>
                  <xsl:value-of select="@PumpHoursPerUnitArea" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Water Cost (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Water Cost  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@WaterCost" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Water Price (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Water Price  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@WaterPrice" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  Irrigation Labor Price (per hour):
                  <xsl:value-of select="@LaborPrice" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Irrigation Labor Amount (per acre):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Irrigation Labor Amount  (per hectare) :
                  </xsl:if>
                  <xsl:value-of select="@LaborAmount" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Irrigation Labor Cost (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Irrigation Labor Cost  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@LaborCost" />
                </div>
                <div class="ui-block-b">
                  Equipment Labor Price (per hour):
                  <xsl:value-of select="@PriceMachineryLabor" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Equipment Labor Amount (per acre) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Equipment Labor Amount (per hectare) :
                  </xsl:if>
                  <xsl:value-of select="@EquipmentLaborAmount" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Equipment Labor Cost (per acre inch):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Equipment Labor Cost  (per m3) :
                  </xsl:if>
                  <xsl:value-of select="@EquipmentLaborCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Lube Amount (gallons) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Lube Amount (liters) :
                  </xsl:if>
                  <xsl:value-of select="@LubeOilAmount" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Lube Oil Cost (per acre inch) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Lube Oil Cost (per m3):
                  </xsl:if>
                  <xsl:value-of select="@LubeOilCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Repair Cost (per acre inch) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Repair Cost (per m3): :
                  </xsl:if>
                  <xsl:value-of select="@RepairCost" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Extra Energy (standby) Cost (per acre inch) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Extra Energy (standby) Cost (per m3): :
                  </xsl:if>
                  <xsl:value-of select="@EnergyExtraCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    <strong>Total Operating Cost (per acre inch)</strong>  :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    <strong>Total Operating Cost (per m3)</strong>  :
                  </xsl:if>
                </div>
                <div class="ui-block-b">
                  <strong>
                    <xsl:value-of select="@InputPrice1" />
                  </strong>
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Capital Recovery Cost (per acre inch) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Capital Recovery Cost (per m3): :
                  </xsl:if>
                  <xsl:value-of select="@CapitalRecoveryCost" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Taxes, Housing, Insurance Cost (per acre inch) :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Taxes, Housing, Insurance Cost (per m3): :
                  </xsl:if>
                  <xsl:value-of select="@TaxesHousingInsuranceCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    <strong>Total Allocated Overhead Cost (per acre inch)</strong>  :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    <strong>Total Allocated Overhead Cost (per m3)</strong>  :
                  </xsl:if>
                </div>
                <div class="ui-block-b">
                  <strong>
                    <xsl:value-of select="@InputPrice2" />
                  </strong>
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  Capital Cost:
                  <xsl:value-of select="@InputPrice3" />
                </div>
                <div class="ui-block-b">
                  Capital Unit:
                  <xsl:value-of select="@InputUnit3" />
                </div>
              </div>
            </div>
						</xsl:if>
            <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
            <h4 class="ui-bar-b"><strong>A. Select options</strong></h4>
						<div class="ui-field-contain">
            <fieldset data-role="controlgroup" data-type="horizontal">
              <legend>Inflation Options</legend>
							  <input type="radio" id="InflationOptions1" data-mini="true">
								  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								  </xsl:if>
                  <xsl:attribute name="value">1</xsl:attribute>
								  <xsl:if test="(@OptionForInflation = '1')">
									  <xsl:attribute name="checked">true</xsl:attribute>
								  </xsl:if>
							  </input>
							  <label for="InflationOptions1" >First Year</label>
							  <input type="radio" id="InflationOptions2" data-mini="true">
								  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								  </xsl:if>
								  <xsl:attribute name="value">2</xsl:attribute>
								  <xsl:if test="(@OptionForInflation = '2')">
									  <xsl:attribute name="checked">true</xsl:attribute>
								  </xsl:if>
							  </input>
							  <label for="InflationOptions2" >All Years</label>
							  <input type="radio" id="InflationOptions3" data-mini="true">
								  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								  </xsl:if>
								  <xsl:attribute name="value">3</xsl:attribute>
								  <xsl:if test="(@OptionForInflation = '3')">
									  <xsl:attribute name="checked">true</xsl:attribute>
								  </xsl:if>
							  </input>
							  <label for="InflationOptions3" >Do Not Use</label>
					  </fieldset>
          </div>
          </div>
				 <h4 class="ui-bar-b"><strong>B. Fill in machinery variables</strong></h4>
          <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
            <h4 class="ui-bar-b">
              <strong>Allocated Overhead</strong>
            </h4>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblIrrigationNetArea" >
                  Net Irrigation Area
                </label>
                <input type="text" id="lblIrrigationNetArea" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name">
                      <xsl:value-of select="$searchurl" />;IrrigationNetArea;double;8
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationNetArea" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblIrrigationNetAreaUnit" >Net Irrigation Area Unit</label>
                <input type="text" id="lblIrrigationNetAreaUnit" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name">
                      <xsl:value-of select="$searchurl" />;IrrigationNetAreaUnit;string;25
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationNetAreaUnit" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblMarketValue" >Market Value (input.CAPPrice)</label>
                <xsl:choose>
                  <xsl:when test="($docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries')">
                    <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                      <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('MarketValue', @MarketValue, 'decimal', '8')"/>
                    </xsl:if>
                    <xsl:if test="($viewEditType = 'full')">
                      <xsl:choose>
                        <xsl:when test="(contains($docToCalcNodeName, 'devpack')
									or contains($docToCalcNodeName, 'linkedview')
									or contains($selectedFileURIPattern, 'temp'))">
                          <input id="lblMarketValue" type="text" data-mini="true">
                            <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MarketValue;double;8</xsl:attribute>
                            <xsl:attribute name="value"><xsl:value-of select="@MarketValue" /></xsl:attribute>
                          </input>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('MarketValue', @MarketValue, 'decimal', '8')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <input id="lblMarketValue" type="text" data-mini="true">
                      <xsl:if test="($viewEditType = 'full')">
                        <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MarketValue;double;8</xsl:attribute>
                      </xsl:if>
                      <xsl:attribute name="value">
                        <xsl:value-of select="@MarketValue" />
                      </xsl:attribute>
                    </input>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
              <div class="ui-block-b">
                <label for="lblPlannedUseHours" >Planned Use Hours</label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input id="lblPlannedUseHours" type="text" data-mini="true">
                    <xsl:attribute name="value">
                      <xsl:value-of select="@PlannedUseHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input id="lblPlannedUseHours" type="text" data-mini="true">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PlannedUseHrs;integer;4</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@PlannedUseHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblSalvageValue" >Salvage Value</label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="lblSalvageValue" data-mini="true">
                    <xsl:attribute name="value">
                      <xsl:value-of select="@SalvageValue" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="lblSalvageValue" data-mini="true">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SalvageValue;double;8</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@SalvageValue" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
              </div>
              <div class="ui-block-b">
                <label for="lblStartingHours" >Starting Hours</label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="lblStartingHours" data-mini="true">
                    <xsl:attribute name="value">
                      <xsl:value-of select="@StartingHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="lblStartingHours" data-mini="true">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;StartingHrs;integer;4</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@StartingHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblListPriceAdj" >List Price Adj (+)</label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="lblListPriceAdj" data-mini="true">
                    <xsl:attribute name="value">
                      <xsl:value-of select="@ListPriceAdj" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="lblListPriceAdj" data-mini="true">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ListPriceAdj;double;8</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@ListPriceAdj" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
              </div>
              <div class="ui-block-b">
                <label for="lblUsefulLifeHours" >Useful Life Hours</label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="lblUsefulLifeHours" data-mini="true">
                    <xsl:attribute name="value">
                      <xsl:value-of select="@UsefulLifeHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="lblUsefulLifeHours" data-mini="true">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;UsefulLifeHrs;integer;4</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@UsefulLifeHrs" />
                    </xsl:attribute>
                  </input>
                </xsl:if>
              </div>
            </div>
          </div>
          <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
            <h4 class="ui-bar-b">
              <strong>Power Costs</strong>
            </h4>
            <div class="ui-field-contain">
              <fieldset data-role="controlgroup" data-type="horizontal">
                <legend>Fuel Consumption Option</legend>
                <input type="radio" id="FuelOptions1" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForFuelConsumption;integer;4</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">1</xsl:attribute>
                  <xsl:if test="(@OptionForFuelConsumption = '1')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="FuelOptions1" >Energy Use (flywheel)</label>
                <input type="radio" id="FuelOptions2" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForFuelConsumption;integer;4</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">2</xsl:attribute>
                  <xsl:if test="(@OptionForFuelConsumption = '2')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="FuelOptions2" >Nebraska PP Criteria (whp)</label>
              </fieldset>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblEngineEfficiency" >Engine Efficiency (for 'Base on Energy Use')</label>
                <input type="text" id="lblEngineEfficiency" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;EngineEfficiency;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@EngineEfficiency" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblEngineEfficiency" >Fuel Type</label>
                <select class="Select225" id="lblEngineEfficiency" data-mini="true">
                  <option>
                    see Martin Reference
                  </option>
                  <option>
                    Diesel - 31%
                  </option>
                  <option>
                    Gas - 23%
                  </option>
                  <option>
                    LPG - 25%
                  </option>
                  <option>
                    1000 cubic foot Natural Gas - 21%
                  </option>
                  <option>
                    1 kilowatt hour of Electricity - 88%
                  </option>
                </select>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblFuelType" >Fuel Type</label>
                <select class="Select225" id="lblFuelType" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FuelType;string;50</xsl:attribute>
                  </xsl:if>
                  <option>
                    <xsl:attribute name="value">none</xsl:attribute>
                    <xsl:if test="(@FuelType = 'none')">
                      <xsl:attribute name="selected" />
                    </xsl:if>none
                  </option>
                  <option>
                    <xsl:attribute name="value">diesel</xsl:attribute>
                    <xsl:if test="(@FuelType = 'diesel')">
                      <xsl:attribute name="selected" />
                    </xsl:if>diesel
                  </option>
                  <option>
                    <xsl:attribute name="value">gas</xsl:attribute>
                    <xsl:if test="(@FuelType = 'gas')">
                      <xsl:attribute name="selected" />
                    </xsl:if>gas
                  </option>
                  <option>
                    <xsl:attribute name="value">lpg</xsl:attribute>
                    <xsl:if test="(@FuelType = 'lpg')">
                      <xsl:attribute name="selected" />
                    </xsl:if>lpg
                  </option>
                  <option>
                    <xsl:attribute name="value">electric</xsl:attribute>
                    <xsl:if test="(@FuelType = 'electric')">
                      <xsl:attribute name="selected" />
                    </xsl:if>electric
                  </option>
                  <option>
                    <xsl:attribute name="value">naturalgas</xsl:attribute>
                    <xsl:if test="(@FuelType = 'naturalgas')">
                      <xsl:attribute name="selected" />
                    </xsl:if>naturalgas
                  </option>
                </select>
              </div>
              <div class="ui-block-b">
                <label for="lblFuelConsumptionPerHour" >Actual Fuel Consumption Per Hour</label>
                <input type="text" id="lblFuelConsumptionPerHour" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FuelConsumptionPerHour;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@FuelConsumptionPerHour" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblFlowRate" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Flow Rate (gpm)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Flow Rate (l/s)
                  </xsl:if>
                </label>
                <input type="text" id="lblFlowRate" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FlowRate;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@FlowRate" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblDate" >Date</label>
                <input type="text" id="lblDate" name="lblDate" data-mini="true" disabled="true">
                  <xsl:attribute name="value">
                    <xsl:value-of select="@InputDate"/>
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblStaticHead" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Static Head (feet)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Static Head (meters)
                  </xsl:if>
                </label>
                <input type="text" id="lblStaticHead" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;StaticHead;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@StaticHead" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblPressureHead" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Pressure Head (psi)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Pressure Head (kPa)
                  </xsl:if>
                </label>
                <input type="text" id="lblPressureHead" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PressureHead;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@PressureHead" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblFrictionHead" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Friction Head (feet)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Friction Head (meters)
                  </xsl:if>
                </label>
                <input type="text" id="lblFrictionHead" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FrictionHead;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@FrictionHead" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblOtherHead" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Other Head (feet)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Other Head (meters)
                  </xsl:if>
                </label>
                <input type="text" id="lblOtherHead" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OtherHead;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@OtherHead" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblPumpEfficiency" >Pump Efficiency</label>
                <input type="text" id="lblPumpEfficiency" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PumpEfficiency;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@PumpEfficiency" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblGearDriveEfficiency" >Gear Drive Efficiency</label>
                <input type="text" id="lblGearDriveEfficiency" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;GearDriveEfficiency;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@GearDriveEfficiency" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <xsl:if test="(@UnitGroupId = '1001')">
                  Extra Power 1 (hp)
                </xsl:if>
                <xsl:if test="(@UnitGroupId != '1001')">
                  Extra Power 1 (kW)
                </xsl:if>
                <input type="text" id="lblExtraPower1" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ExtraPower1;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@ExtraPower1" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <xsl:if test="(@UnitGroupId = '1001')">
                  Extra Power 2 (hp)
                </xsl:if>
                <xsl:if test="(@UnitGroupId != '1001')">
                  Extra Power 2 (kW)
                </xsl:if>
                <input type="text" id="lblExtraPower1" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ExtraPower2;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@ExtraPower2" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
          </div>
          <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
            <h4 class="ui-bar-b">
              <strong>Water Costs</strong>
            </h4>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblSeasonWaterNeed" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Season Water Need (ac-in)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Season Water Need  (m3)
                  </xsl:if>
                </label>
                <input type="text" id="lblSeasonWaterNeed" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SeasonWaterNeed;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@SeasonWaterNeed" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblDistributionUniformity" >Distribution Uniformity</label>
                <input type="text" id="lblDistributionUniformity" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;DistributionUniformity;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@DistributionUniformity" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblSeasonWaterExtraCredit" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Season Water Extra Credit (ac-in)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Season Water Extra Credit  (m3)
                  </xsl:if>
                </label>
                <input type="text" id="lblSeasonWaterExtraCredit" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SeasonWaterExtraCredit;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@SeasonWaterExtraCredit" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblSeasonWaterExtraDebit" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Season Water Extra Debit (ac-in)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Season Water Extra Debit  (m3)
                  </xsl:if>
                </label>
                <input type="text" id="lblSeasonWaterExtraDebit" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SeasonWaterExtraDebit;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@SeasonWaterExtraDebit" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblWaterPrice" >
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Water Price (per ac-in)
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Water Price  (per m3)
                  </xsl:if>
                </label>
                <input type="text" id="lblWaterPrice" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"> <xsl:value-of select="$searchurl" />;WaterPrice;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@WaterPrice" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblWaterPriceUnit" >Water Price Unit</label>
                <input type="text" id="lblWaterPriceUnit" name="lblWaterPriceUnit" data-mini="true"  disabled="true">
                  <xsl:attribute name="value">
                    <xsl:value-of select="@WaterPriceUnit"/>
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <h4 class="ui-bar-b">
              <strong>Labor Costs</strong>
            </h4>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblTimes" >
                  Number of Irrigations Per Season
                </label>
                <input type="text" id="lblIrrigationTimes" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IrrigationTimes;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationTimes" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblIrrigationDurationPerSet" >Irrigation Duration Per Set</label>
                <input type="text" id="lblIrrigationDurationPerSet" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IrrigationDurationPerSet;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationDurationPerSet" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblIrrigationDurationUnit" >
                  Irrigation Duration Unit
                </label>
                <input type="text" id="lblIrrigationDurationUnit" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IrrigationDurationUnit;string;25</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationDurationUnit" />
                  </xsl:attribute>
                </input>
              </div>
              <div class="ui-block-b">
                <label for="lblIrrigationDurationLaborHoursPerSet" >Irrigation Duration Labor Hours Per Set</label>
                <input type="text" id="lblIrrigationDurationLaborHoursPerSet" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IrrigationDurationLaborHoursPerSet;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@IrrigationDurationLaborHoursPerSet" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <label for="lblLaborType" >Labor Type For Irrigating</label>
                <select class="Select225" id="lblLaborType" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborType;string;50</xsl:attribute>
                  </xsl:if>
                  <option>
                    <xsl:attribute name="value">none</xsl:attribute>
                    <xsl:if test="(@LaborType = 'none')">
                      <xsl:attribute name="selected" />
                    </xsl:if>none
                  </option>
                  <option>
                    <xsl:attribute name="value">regular</xsl:attribute>
                    <xsl:if test="(@LaborType = 'regular')">
                      <xsl:attribute name="selected" />
                    </xsl:if>regular
                  </option>
                  <option>
                    <xsl:attribute name="value">machinery</xsl:attribute>
                    <xsl:if test="(@LaborType = 'machinery')">
                      <xsl:attribute name="selected" />
                    </xsl:if>machinery
                  </option>
                  <option>
                    <xsl:attribute name="value">supervisory</xsl:attribute>
                    <xsl:if test="(@LaborType = 'supervisory')">
                      <xsl:attribute name="selected" />
                    </xsl:if>supervisory
                  </option>
                </select>
              </div>
              <div class="ui-block-b">
                <label for="lblLaborAmountAdj" >Percent Equipment Labor Per Hour of Irrigation Labor</label>
                <input type="text" id="lblLaborAmountAdj" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborAmountAdj;double;8</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value">
                    <xsl:value-of select="@LaborAmountAdj" />
                  </xsl:attribute>
                </input>
              </div>
            </div>
          </div>
          <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
              <h4 class="ui-bar-b"><strong>Repair Costs</strong></h4>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
											R, M and L Cost per Net Acre Irrigated per Season
										</xsl:if>
										<xsl:if test="(@UnitGroupId != '1001')">
											R, M and L Cost per Net Hectare Irrigated per Season
										</xsl:if>
										<input type="text" id="lblRepairCostsPerNetAcOrHa" data-mini="true">
                      <xsl:if test="($viewEditType = 'full')">
											  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;RepairCostsPerNetAcOrHa;double;8</xsl:attribute>
											</xsl:if>
                      <xsl:attribute name="value"><xsl:value-of select="@RepairCostsPerNetAcOrHa" /></xsl:attribute>
										</input>
								</div>
								<div class="ui-block-b">
									<label for="lblRandMPercent" >Percent R, M and L Cost</label>
									<input type="text" id="lblRandMPercent" data-mini="true">
                    <xsl:if test="($viewEditType = 'full')">
											<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;RandMPercent;double;8</xsl:attribute>
										</xsl:if>
                    <xsl:attribute name="value"><xsl:value-of select="@RandMPercent" /></xsl:attribute>
									</input>
								</div>
							</div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <label for="lblEnergyExtraCostPerNetAcOrHa" >
                  <xsl:if test="(@UnitGroupId = '1001')">
											Extra Energy (Standby) Cost per Net Acre Irrigated per Season
										</xsl:if>
										<xsl:if test="(@UnitGroupId != '1001')">
											Extra Energy (Standby) Cost per Net Hectare Irrigated per Season
										</xsl:if>
                  </label>
									<input type="text" id="lblEnergyExtraCostPerNetAcOrHa" data-mini="true">
                    <xsl:if test="($viewEditType = 'full')">
											<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;EnergyExtraCostPerNetAcOrHa;double;8</xsl:attribute>
										</xsl:if>
                    <xsl:attribute name="value"><xsl:value-of select="@EnergyExtraCostPerNetAcOrHa" /></xsl:attribute>
									</input>
								</div>
							<div class="ui-block-b">
							</div>
						</div>
            </div>
					</xsl:when>
					<xsl:otherwise>
						<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
						sure this is the right calculator?</h3>
					</xsl:otherwise>
					</xsl:choose>
			</div>
      <div id="divstepfour">
			  <xsl:variable name="filexttype"><xsl:value-of select="DisplayDevPacks:GetSubString($selectedFileURIPattern,'/','5')" /></xsl:variable>
			  <xsl:if test="($lastStepNumber != 'stepfive')">
          <h4 class="ui-bar-b">Step 4 of 4. Save</h4>
				  <xsl:if test="$filexttype = 'temp' or contains($docToCalcNodeName, 'linkedview')">
            <p>
							  <strong>Temporary Calculations.</strong> Calculations are temporarily saved when temporary calculations are run.
					  </p>
				  </xsl:if>
				  <xsl:if test="($filexttype != 'temp') and (contains($docToCalcNodeName, 'linkedview') = false)">
					  <xsl:variable name="calcParams4a">'&amp;step=stepfive&amp;savemethod=calcs<xsl:value-of select="$calcParams" />'</xsl:variable>
            <p>
							  <strong>Method 1.</strong> Do you wish to save step 2's calculations? These calculations are viewed by opening this particular calculator addin.
					  </p>
					  <xsl:if test="($viewEditType = 'full')">
						  <xsl:value-of select="DisplayDevPacks:MakeDevTreksButton('savecalculation', 'SubmitButton1Enabled150', 'Save Calcs', $contenturipattern, $selectedFileURIPattern, 'prepaddin', 'linkedviews', 'runaddin', 'none', $calcParams4a)" />
					  </xsl:if>
				  </xsl:if>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'stepfive')
				  or ($lastStepNumber = 'stepfour' and $filexttype = 'temp')
				  or ($lastStepNumber = 'stepfour' and contains($docToCalcNodeName, 'linkedview'))">
						  <h3>
							  <xsl:if test="($saveMethod = 'calcs'
									  or $filexttype = 'temp'
									  or contains($docToCalcNodeName, 'linkedview'))">
								  Your calculations have been saved. The calculations can be viewed whenever
								  this calculator addin is opened.
							  </xsl:if>
						  </h3>
			  </xsl:if>
		  </div>
			<div id="divsteplast">
			<h4 class="ui-bar-b">Instructions (beta)</h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 1</h4>
			<ul data-role="listview">
				  <li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. In the USA, DevTreks recommends 
					using Office of Management and Budget rates for the same year as the date of the input.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
        <li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
				<li><strong>Step 3. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 2. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
        <li><strong>Step 2. Fuel Prices:</strong> The prices should be as of the date of the input. Prices that won't be used, such as natural gas prices, do not need to be filled out.</li>
				<li><strong>Step 2. Labor Prices:</strong> These prices should reflect actual gross hourly wages for each category of labor.</li>
				<li><strong>Step 2. Tax Percent:</strong> A percent that is multiplied by the market value to derive a tax cost. If uncertain, choose 1 percent.</li>
				<li><strong>Step 2. Housing Percent:</strong> A percent that is multiplied by the market value to derive a housing cost. If uncertain, choose 0.5 percent.</li>
				<li><strong>Step 2. Insure Percent:</strong> A percent that is multiplied by the market value to derive an insurance cost. If uncertain, choose 0.5 percent.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3. Power</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Inflation Options:</strong> These options change the capital recovery cost.</li>
				<li><strong>Step 3. Fuel Consumption Option:</strong> The 'Energy Use' option calculates fuel use based on the energy expended at the flywheel. The 'Nebraska Power Plant Criteria' option is based on water horsepower and an efficient pumping plant. The references below explain the use of each of these options.</li>
        <li><strong>Step 3. Engine Efficiency:</strong> A multiplier used with the 'Energy Use' fuel consumption option that adjusts energy consumed by the efficiency of the engine. The drop down list on the right shows typical effiencies. This number is divided by 100 in the calculations.</li>
        <li><strong>Step 3. Fuel Consumption Per Hour:</strong> The amount of measured fuel consumed per hour of use (i.e. 5 gallons per hour). Used with all fuel consumption options to determine an actual vs. required fuel consumption and the Pumping Plant Performance (listed below).</li>
        <li><strong>Step 3. Flow Rate:</strong> Gallons per minute, or liters per second, pumped.</li>
				<li><strong>Step 3. Static Head:</strong> 'Vertical distance pump must list water from pickup point to discharge point.' (Caterpillar)</li>
				<li><strong>Step 3. Pressure Head:</strong> System pressure in pounds per square inch, or kPA.</li>
				<li><strong>Step 3. Friction Head:</strong> . 'Estimate 1 foot, or meter, per 100 feet, or meters, from pickup point to start of irrigation system or open discharge' (Caterpillar).</li>
				<li><strong>Step 3. Other Head:</strong> Such as drawdown, column loss, or elevation gain, in feet or meters.</li>
				<li><strong>Step 3. Pump Efficiency:</strong> Available with pump specifications (or use 75). This number is divided by 100 in the calculations.</li>
				<li><strong>Step 3. Gear Drive Efficiency:</strong>  'For direct driven centrifugal pump, use 100, for right angle drive, use 95. (Caterpillar) This number is divided by 100 in the calculations.</li>
        <li><strong>Step 3. Extra Power 1:</strong> 'To drive pivot or or travelling system. For pivot system, estimate 1.1 engine horsepower or kilowatt (1.5 hp) per tower. Irrigation system supplier will quote power for electric, hydraulic, or other, drive'.(Caterpillar)</li>
				<li><strong>Step 3. Extra Power 2:</strong> 'For engine accessories and cooling. For preliminary use, estimate 5% of engine horsepower or kilowatt.' The engine supplier can provide this information.(Caterpillar)</li>
        <li><strong>Step 3. Energy Extra Cost Per Net Ac Or Ha:</strong> When energy costs include annual standby, hookup, or other nonfuel, costs, divide the additional cost by the net acres irrigated and enter the result here. For example, $1000 spent on a field's standby electricity charge divided by 155 net acres (ha) equals $6.45 per acre (ha).</li>
		  </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3. Water 1</h4>
      <ul data-role="listview">
        <li><strong>Step 3. Season Water Need:</strong> Amount of water needed by the plant over the course of a season (ETcrop). Unit is acre inches or cubic meters.</li>
				<li><strong>Step 3. Season Water Extra Credit:</strong> Amount of water supplied by rain, or other moisture, during the season. Unit is acre inches or cubic meters.</li>
        <li><strong>Step 3. Season Water Extra Debit:</strong> Additional water needing to be applied during the season to deal with frost, leaching, evaporation, or some other water loss. Unit is acre inches or cubic meters.</li>
				<li><strong>Step 3. Water Price:</strong> Price of water purchased from irrigation district or some other supplier. The price must be entered as per acre inch or per cubic meter. When only a portion of the total seasonal irrigation water applied has a price, use separate calculators for each portion (i.e. because the pumping costs are likely to differ). </li>
        <li><strong>Step 3. Water Price Unit:</strong> Fixed as acre inch or liters</li>
        <li><strong>Step 3. Distribution Uniformity:</strong> Uniformity of water applied to field. This number is divided by 100 in the calculations.</li>
      </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3. Water 2</h4>
      <ul data-role="listview">
        <li><strong>Step 3. Irrigation Net Area:</strong> For example, a 160 gross acre field may have 155 net irrigated acres due to roads, canals, and other noncropped space. Note that when this input is added to an operation or component, the 'amount' of the operation or component will replace this number in the calculations. When this input is added to an operating or capital budget, the 'amount' entered for the time period will replace this number in the calculations.</li>
        <li><strong>Step 3. Irrigation Area Unit:</strong> Typically acre or hectare.</li>
        <li><strong>Step 3. Number of Irrigations Per Season:</strong> Self-explanatory. Note that when this input is added to an operation or component, the 'times' entered for the input will replace this number in the calculations. For an example of these irrigation properties, please see the Sanden et al reference.</li>
        <li><strong>Step 3. Irrigation Duration Per Set:</strong> Length of time that each irrigation cycle uses. For example, if each irrigation cycle lasts ten days, enter 10.</li>
        <li><strong>Step 3. Irrigation Duration Unit:</strong> Unit of measurement for irrigation durations (i.e. typically days, but could be hours).</li>
        <li><strong>Step 3. Irrigation Duration Labor Hours Per Set:</strong> Number of irrigation labor hours used during each 'Irrigation Duration Set'. For example, if 4 hours of irrigation labor are used for each day of a 10 irrigation cycle, enter 4.</li>
        <li><strong>Step 3. Labor Type For Irrigating:</strong> Choose the type of labor doing the irrigating. Uses the matching labor cost per hour from step 2.</li>
        <li><strong>Step 3. Percent Equipment Labor Per Hour of Irrigation Labor:</strong> Percent of an hour of irrigation labor to assign to equipment labor (i.e. setup, transportation). This number will be divided by 100 in the calculations.</li>
			</ul>
      <ul data-role="listview">
        <li><strong>Step 3. Repair and Maintenance Per Net Ac Or Ha:</strong> When seasonal repair and maintenance bills are available, divide those total costs by the net acres irrigated and enter the result here. For example, $1000 spent on irrigation repairs and maintence divided by 155 net acres (ha) equals $6.45 per acre (ha).</li>
        <li><strong>Step 3. Repair and Maintenance Percent:</strong> When repair bills are not available, enter this percent (it will be multiplied by the market value of this input and then divided by the net acres irrigated). The percent can be found in several irrigation costing publications (i.e. Hallam et al). This number will be divided by 100 in the calculations</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3. Energy</h4>
      <ul data-role="listview">
        <li><strong>Step 3. Total Engine Flywheel Power:</strong> Total quantity of energy (in hp or Kw) expended at the flywheel. Please see the Caterpillar reference.</li>
        <li><strong>Step 3. Water Horsepower:</strong> Total quantity of energy required to pump the water. Please see the references for more precise definitions.</li>
        <li><strong>Step 3. Brake Horsepower:</strong> Water horsepower divided by pumping and gearhead efficiency.</li>
        <li><strong>Step 3. Season Water Applied:</strong> Total quantity of water applied to the net acres (ha) irrigated, accounting for additional credits and debits. Unit is acre inches or cubic meters.</li>
        <li><strong>Step 3. Fuel, water, labor, and lube/oil amounts:</strong> Amounts of these resources used per acre inch, or cubic meter, of water applied. For example, 5 gallons of diesel might be needed to apply 1 acre inch, or cubic meter, of water.</li>
        <li><strong>Step 3. Fuel, water, labor, repair, and lube/oil costs:</strong> Costs of these resources used per acre inch, or cubic meter, of water applied. For example, $15 worth of fuel might be needed to apply 1 acre inch, or cubic meter, of water.</li>
        <li><strong>Step 3. Required Fuel Amount:</strong> The required energy needed to pump the volume of water being applied. Please see the Martin et al, and Roger et al, references.</li>
        <li><strong>Step 3. Actual Fuel Amount:</strong> The acutal fuel consumed when pumping the volume of water being applied. </li>
        <li><strong>Step 3. Pumping Plant Performance:</strong> The required energy needed to pump the volume of water being applied divided by the actual energy expended to pump that water. A pumping plant performance of 79 (79% of Nebraska Pumping Plant Performance Criteria) uses 27% more fuel than required.
        Please see the Martin et al, and Roger et al, references.</li>
        <li><strong>Step 3. Energy Extra Cost:</strong> Annual per net acre standby, hookup, or other nonfuel, costs, divided by the total volume of water applied per ac (ha). For example, a $10 per net acre standby electric charge dividied by 15 ac inches applied water equals $.67 per acre inch cost.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Caterpillar Inc.</strong> Irrigation Engine Ratings Guide. Peoria, Illinois, 2009</li>
        <li><strong>Guerrero, Amosson, Marek, and Johnson</strong> Economic Evaluation of Wind Energy as an Alternative to Natural Gas Powered Irrigation. Journal of Agriculture and Applied Economics, 42,2(May, 2010)</li>
        <li><strong>Hallam, Eidman, Morehart and Klonsky (editors).</strong> Commodity Cost and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
        <li><strong>Martin, Dorn, Melvin, Corr and Kranz.</strong> Evaluating Energy Use for Pumping Irrigation Water. (authors are University of Nebraska) Proceedings of the 23rd Annual Central Plains Irrigation Conference. Burlington, CO. 2011</li>
        <li><strong>Sanden, Klonsky, Putnam, Schwankl, and Bali</strong> Comparing Costs and Efficiencies of Different Alfalfa Irrigation Systems. UC Davis, Davis, California, USA, 2011</li>
        <li><strong>Rogers and Alam</strong> Comparing Irrigation Energy Costs. Kansas State University, Manhattan, KS, USA, 2006</li>
			</ul>
      </div>
			</div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

  