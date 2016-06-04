<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, April -->
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
      <h4 class="ui-bar-b"><strong>Capital Service Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool calculates totals for general capital input uris. 
							The discounted totals include operating, allocated overhead, and capital costs. 
							Operating costs include fuel, repair and maintenance, and labor costs.
							Allocated overhead costs include capital recovery, and taxes, housing and insurance costs.
							Farm machinery and irrigation power inputs should use one of the existing, specialized calculators.
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
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
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
      <br />
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
	    <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4>Relations</h4>
	    <xsl:if test="($docToCalcNodeName != 'inputseries' or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				<xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
          @UseSameCalculator, @Overwrite)"/>
		  </xsl:if>
      <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, @RelatedCalculatorsType)"/>
      </div>
      <xsl:choose>
				<xsl:when test="($docToCalcNodeName = 'inputgroup' 
						or $docToCalcNodeName = 'input' 
						or $docToCalcNodeName = 'inputseries'
						or contains($docToCalcNodeName, 'devpack') 
						or contains($docToCalcNodeName, 'linkedview'))">
					<xsl:if test="(@RepairCost >= 0)
            or (@CapitalRecoveryCost >= 0)">
						<div class="ui-grid-a">
							<div class="ui-block-a">
								<xsl:if test="(@UnitGroupId = '1001')">
									Fuel (gal/hr): 
								</xsl:if>
								<xsl:if test="(@UnitGroupId != '1001')">
									Fuel (liters/hr): 
								</xsl:if>
								<xsl:value-of select="@FuelAmount" />
							</div>
							<div class="ui-block-b">
								Fuel Cost: 
								<xsl:value-of select="@FuelCost" />
							</div>
						</div>
						<div class="ui-grid-a">
							<div class="ui-block-a">
								Fuel Unit: 
								<xsl:value-of select="@FuelUnit" />
							</div>
							<div class="ui-block-b">
							</div>
						</div>
						<div class="ui-grid-a">
							<div class="ui-block-a">
								Repair Cost: 
								<xsl:value-of select="@RepairCost" />
							</div>
							<div class="ui-block-b">
								Labor Cost: 
								<xsl:value-of select="@LaborCost" />
							</div>
						</div>
						<div class="ui-grid-a">
							<div class="ui-block-a">
                <strong>Total Operating Cost ($/hour): </strong>
							</div>
              <div class="ui-block-b">
								<strong><xsl:value-of select="@InputPrice1" /></strong>
							</div>
						</div>
						<div class="ui-grid-a">
							<div class="ui-block-a">
								Capital Recovery Cost: 
								<xsl:value-of select="@CapitalRecoveryCost" />
							</div>
							<div class="ui-block-b">
								Taxes, Housing, Insurance: 
								<xsl:value-of select="@TaxesHousingInsuranceCost" />
							</div>
						</div>
						<div class="ui-grid-a">
							<div class="ui-block-a">
                <strong>Total Allocated Overhead Cost ($/hour): </strong>
							</div>
              <div class="ui-block-b">
								<strong><xsl:value-of select="@InputPrice2" /></strong>
							</div>
						</div>
						<div class="ui-grid-a">
							<!--In order to set a db attribute in a calulator, without 
							having the db property overwrite it, change its name (i.e. MarketValue) 
							and then set the db name (i.e. CAPPrice = MarketValue) when the calculation is run-->
							<div class="ui-block-a">
								<strong>Capital Cost:</strong> <xsl:value-of select="@InputPrice3" />
							</div>
							<div class="ui-block-b">
								Capital Unit: <xsl:value-of select="@InputUnit3" />
							</div>
						</div>
					</xsl:if>
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
        <h4 class="ui-bar-b"><strong>B. Fill in machinery variables</strong></h4>
				<div>
          <label for="CalculatorName" class="ui-hidden-accessible"></label>
					<input id="CalculatorName" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
            </xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					</input>
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
									</xsl:if>natural gas
								</option>
							</select>
					</div>
          <div class="ui-block-b">
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
                <xsl:attribute name="value"><xsl:value-of select="@MarketValue" /></xsl:attribute>
							</input>
						</xsl:otherwise>
						</xsl:choose>
          </div>
          <div class="ui-block-b">
            <label for="lblPlannedUseHours" >Planned Use Hours</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input id="lblPlannedUseHours" type="text" data-mini="true">
									<xsl:attribute name="value"><xsl:value-of select="@PlannedUseHrs" /></xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input id="lblPlannedUseHours" type="text" data-mini="true">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PlannedUseHrs;integer;4</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@PlannedUseHrs" /></xsl:attribute>
								</input>
							</xsl:if>
          </div>
        </div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblSalvageValue" >Salvage Value</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblSalvageValue" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@SalvageValue" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblSalvageValue" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SalvageValue;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@SalvageValue" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblStartingHours" >Starting Hours</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblStartingHours" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@StartingHrs" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblStartingHours" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;StartingHrs;integer;4</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@StartingHrs" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <label for="lblUsefulLifeHours" >Useful Life Hours</label>
              <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                <input type="text" id="lblUsefulLifeHours" data-mini="true">
                  <xsl:attribute name="value"><xsl:value-of select="@UsefulLifeHrs" /></xsl:attribute>
                </input>
              </xsl:if>
              <xsl:if test="($viewEditType = 'full')">
                <input type="text" id="lblUsefulLifeHours" data-mini="true">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;UsefulLifeHrs;integer;4</xsl:attribute>
                  <xsl:attribute name="value"><xsl:value-of select="@UsefulLifeHrs" /></xsl:attribute>
                </input>
              </xsl:if>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblEnergyUseHr" >Rated Energy Use (per hour)</label>
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input type="text" id="lblEnergyUseHr">
									<xsl:attribute name="value"><xsl:value-of select="@EnergyUseHr" /></xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input type="text" id="lblEnergyUseHr">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;EnergyUseHr;double;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@EnergyUseHr" /></xsl:attribute>
								</input>
							</xsl:if>
						</div>
						<div class="ui-block-b">
							<label for="lblEnergyEffTypical" >Energy Efficiency Typical</label>
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input type="text" id="lblEnergyEffTypical">
									<xsl:attribute name="value"><xsl:value-of select="@EnergyEffTypical" /></xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input type="text" id="lblEnergyEffTypical">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;EnergyEffTypical;double;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@EnergyEffTypical" /></xsl:attribute>
								</input>
							</xsl:if>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblRandMPercent" >Repair and Maint. Percent</label>
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input type="text" id="lblRandMPercent">
									<xsl:attribute name="value"><xsl:value-of select="@RandMPercent" /></xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input type="text" id="lblRandMPercent">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;RandMPercent;double;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@RandMPercent" /></xsl:attribute>
								</input>
							</xsl:if>
						</div>
						<div class="ui-block-b">
							<label for="lblDate" >Date</label>
              <input type="text" id="lblDate" name="lblDate" data-mini="true" disabled="true">
								<xsl:attribute name="value"><xsl:value-of select="@InputDate"/></xsl:attribute>
							</input>
						</div>
					</div>
					<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblLaborType" >Labor Type</label>
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
          <label for="lblLaborAmountAdj" >Labor Amount Adj</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblLaborAmountAdj" data-mini="true">
							<xsl:attribute name="value"><xsl:value-of select="@LaborAmountAdj" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblLaborAmountAdj" data-mini="true">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborAmountAdj;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@LaborAmountAdj" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
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
        <h4 class="ui-bar-b"><strong>Step 4 of 4. Save</strong></h4>
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
			<h4 class="ui-bar-b"><strong>Instructions (beta)</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 1</h4>
			<ul data-role="listview">
				<li><strong>Step 1. Machinery Constants:</strong> The constants derive from an American Society of Agricultural and Biological Engineers publication.</li>
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. In the USA, DevTreks recommends 
					using Office of Management and Budget rates for the same year as the date of the input.</li>
        <li><strong>Step 1. Size Ranges:</strong> This information is optional. Machinery selection and scheduling analyzers use these ranges to determine feasible combinations 
        of machinery (i.e. least cost) in operations and components. The size column should either be width for implements or horsepower for power inputs. The minimum horsepower column 
        is optional and used to limit the power unit used for pulling the associated implement to a minimum feasible hp. Guidelines can be found in the references below to calculate the minimum hp.</li>
			</ul>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
					<li><strong>Step 2. Fuel Prices:</strong> The prices should be as of the date of the input. Prices that won't be used, such as natural gas prices, do not need to be filled out.</li>
					<li><strong>Step 2. Labor Prices:</strong> These prices should reflect actual gross hourly wages for each category of labor.</li>
					<li><strong>Step 2. Tax Percent:</strong> A percent that is multiplied by the market value to derive a tax cost.</li>
					<li><strong>Step 2. Housing Percent:</strong> A percent that is multiplied by the market value to derive a housing cost.</li>
					<li><strong>Step 2. Insure Percent:</strong> A percent that is multiplied by the market value to derive an insurance cost.</li>
				</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
				<li><strong>Step 3. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 3. Inflation Options:</strong> These options change the capital recovery cost.</li>
				<li><strong>Step 3. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 3. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 3. Market Value:</strong> This calculator uses the associated input's input.CAPPrice for the market value (so fill in the CAPPrice before running the calculations).</li>
        <li><strong>Step 3. OC and AOH Amount Per Hour:</strong> This calculator uses the associated input's input.OCAmount (so fill in this number before running the calculations). If running descendant calculations, make sure this number has been entered accurately for the descenants.</li>
				<li><strong>Step 3. Labor Amount Adjustment:</strong> A multiplier that is used to adjust the amount of labor used operation a machinery input. For example, if the time spent setting up the machinery or transporting the machinery adds ten percent to the total amount of time it takes to complete the specified area, enter 10. This number is divided by 100 in the calculations.</li>
				<li><strong>Step 3. Rated Energy Use (per hour):</strong> Enter the quantity of fuel used per hour. </li>
				<li><strong>Step 3. Energy Efficiency Typical:</strong> A percent that is multiplied by the energy use to derive an adjusted energy use. 
          For example, if the Rated Energy Use is 1 gallon per hour and the the Energy Efficiency is 80. The total energy use per hour is .8 gallon per hour = 1 gallon per hour * (80 / 100). Note that this 
          number is divided by 100 in the calculations. </li>
				<li><strong>Step 3. Repair and Maintenance Percent:</strong> A percent that is multiplied by the market value to derive a repair and maintenance cost per hour. For consistency, we recommend entering this using 
          numbers greater than 0 (numbers less than zero will be multiplied by 100).</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Hallam, Eidman, Morehart and Klonsky (editors) </strong> Commodity Cost and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
			</ul>
      </div>
		</div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
