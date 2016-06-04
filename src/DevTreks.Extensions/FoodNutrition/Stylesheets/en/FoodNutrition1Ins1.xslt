<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8"/>
	<!-- pass in params -->
	<!--array holding references to constants, locals, lists ...-->
	<xsl:param name="linkedListsArray"/>
	<!-- calcs -->
	<xsl:param name="saveMethod"/>
	<!-- default linked view -->
	<xsl:param name="defaultLinkedViewId"/>
	<!-- the last step is used to display different parts of the calculator-->
	<xsl:param name="lastStepNumber"/>
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType"/>
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType"/>
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub"/>
	<!-- which node to start with? -->
	<xsl:param name="nodeName"/>
	<!-- which view to use? -->
	<xsl:param name="viewEditType"/>
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole"/>
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern"/>
	<!-- the addin being used -->
	<xsl:param name="calcDocURI"/>
	<!-- the node being calculated (custom docs' nodename can be a devpack node, while this might be budgetgroup) -->
	<xsl:param name="docToCalcNodeName"/>
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams"/>
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName"/>
	<!-- which network is this doc from? -->
	<xsl:param name="networkId"/>
	<!-- what is the start row? -->
	<xsl:param name="startRow"/>
	<!-- what is the end row? -->
	<xsl:param name="endRow"/>
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams"/>
	<!-- what is the club's email? -->
	<xsl:param name="clubEmail"/>
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath"/>
	<!-- init html -->
	<xsl:template match="@*|/|node()"/>
	<xsl:template match="/">
		<div class="box">
			<xsl:if test="($serverActionType = 'linkedviews')">
				<div id="stepsmenu">
					<xsl:value-of select="DisplayDevPacks:WriteMenuSteps('3')"/>
				</div>
			</xsl:if>
			<xsl:apply-templates select="root"/>
      <div>
				<br />
				<strong>Current view of document</strong>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="root">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
		<div id="divstepzero">
      <h4 class="ui-bar-b"><strong>Food Nutrition Input Calculation View</strong></h4>
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
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This calculator calculates the nutritional value of inputs per actual serving size 
							(i.e. not the default serving size on the back of food containers).
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
		<xsl:apply-templates select="linkedview"/>
	</xsl:template>
	<xsl:template match="linkedview">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
		<xsl:if test="(@Id = $linkedviewid) or (@Id = 1)">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@CalculatorName,@Id,$networkId,local-name(),'')"/></xsl:variable>
		<div id="divstepone">
      <h4 class="ui-bar-b"><strong>Step 1 of 3. Make Selections</strong></h4>
		  <xsl:variable name="calcParams1">'&amp;step=steptwo<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams1)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'steptwo')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <br />
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
		</div>
		<div id="divsteptwo">
			<h4 class="ui-bar-b"><strong>Step 2 of 3. Calculate</strong></h4>
			<xsl:variable name="calcParams3">'&amp;step=stepthree<xsl:value-of select="$calcParams"/>'</xsl:variable>
      <xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
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
					<xsl:when test="($docToCalcNodeName = 'inputgroup')">
						<div>
              <label for="CalculatorName" class="ui-hidden-accessible"></label>
					    <input id="CalculatorName" type="text" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
							    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
                </xsl:if>
						    <xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					    </input>
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
      <xsl:when test="($docToCalcNodeName = 'input' 
							  or $docToCalcNodeName = 'inputseries'
							  or contains($docToCalcNodeName, 'devpack') 
							  or contains($docToCalcNodeName, 'linkedview'))">
				<xsl:if test="(@ActualServingsPerContainer >= 0)">
          <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d">
          <h4 class="ui-bar-b"><strong>Nutrition Facts Based On Actual Serving Size</strong></h4>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Gender of Serving Person</strong>: 
							<xsl:value-of select="@GenderOfServingPerson"/>
						</div>
						<div class="ui-block-b">
							<strong>Weight and Units of Serving Person</strong>: 
							<xsl:value-of select="@WeightOfServingPerson"/>
							&#xA0;&#xA0;<xsl:value-of select="@WeightUnitsOfServingPerson"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Actual Servings Per Container</strong>: 
							<xsl:value-of select="@ActualServingsPerContainer"/>
						</div>
						<div class="ui-block-b">
							<strong>Nutrition Facts Servings Per Container</strong>: 
							<xsl:value-of select="@ServingsPerContainer"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Actual Serving Size</strong>: 
							<xsl:value-of select="@InputPrice1Amount"/>
						</div>
						<div class="ui-block-b">
							<strong>Serving Units</strong>: 
							<xsl:value-of select="@InputUnit1"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Total Cost Per Actual Serving</strong>: 
							<xsl:value-of select="@ServingCost"/>
						</div>
						<div class="ui-block-b">
							<strong>Container Cost</strong>: 
							<xsl:value-of select="@InputPrice3"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Calories Per Actual Serving</strong>: 
							<xsl:value-of select="@CaloriesPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Calories From Fat Per Actual Serving</strong>: 
							<xsl:value-of select="@CaloriesFromFatPerActualServing"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Total Fat Per Actual Serving</strong>: 
							<xsl:value-of select="@TotalFatPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Total Fat Percent Daily Value</strong>: 
							<xsl:value-of select="@TotalFatActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Saturated Fat Per Actual Serving</strong>: 
							<xsl:value-of select="@SaturatedFatPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Saturated Fat Percent Daily Value</strong>: 
							<xsl:value-of select="@SaturatedFatActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Trans Fat Per Actual Serving</strong>: 
							<xsl:value-of select="@TransFatPerActualServing"/>
						</div>
						<div class="ui-block-b">
									
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Cholesterol Per Actual Serving</strong>:
							<xsl:value-of select="@CholesterolPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Cholesterol Percent Daily Value</strong>: 
							<xsl:value-of select="@CholesterolActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Sodium Per Actual Serving</strong>: 
							<xsl:value-of select="@SodiumPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Sodium Percent Daily Value</strong>: 
							<xsl:value-of select="@SodiumActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Potassium Per Actual Serving</strong>: 
							<xsl:value-of select="@PotassiumPerActualServing"/>
						</div>
						<div class="ui-block-b">
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Total Carbohydrate Per Actual Serving</strong>: 
							<xsl:value-of select="@TotalCarbohydratePerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Total Carbohydrate Daily Value</strong>: 
							<xsl:value-of select="@TotalCarbohydrateActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Other Carbohydrate Per Actual Serving</strong>: 
							<xsl:value-of select="@OtherCarbohydratePerActualServing"/>
						</div>
						<div class="ui-block-b">
                  
            </div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Dietary Fiber Per Actual Serving</strong>: 
							<xsl:value-of select="@DietaryFiberPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Dietary Fiber Daily Value</strong>: 
							<xsl:value-of select="@DietaryFiberActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Sugars Per Actual Serving</strong>: 
							<xsl:value-of select="@SugarsPerActualServing"/>
						</div>
						<div class="ui-block-b">
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Protein Per Actual Serving</strong>: 
							<xsl:value-of select="@ProteinPerActualServing"/>
						</div>
						<div class="ui-block-b">
							<strong>Protein Daily Value</strong>: 
							<xsl:value-of select="@ProteinActualDailyPercent"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Vitamin A Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@VitaminAPercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Vitamin C Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@VitaminCPercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Vitamin D Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@VitaminDPercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Calcium Daily Actual Value</strong>: 
							<xsl:value-of select="@CalciumPercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Iron Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@IronPercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Thiamin Percent Actual Daily Value</strong>: 
							<xsl:value-of select="ThiaminPercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Folate Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@FolatePercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Riboflavin Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@RiboflavinPercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Niacin Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@NiacinPercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>VitaminB6 Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@VitaminB6PercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>VitaminB12 Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@VitaminB12PercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Phosphorous Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@PhosphorousPercentActualDailyValue"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Magnesium Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@MagnesiumPercentActualDailyValue"/>
						</div>
						<div class="ui-block-b">
							<strong>Zinc Percent Actual Daily Value</strong>: 
							<xsl:value-of select="@ZincPercentActualDailyValue"/>
						</div>
					</div>
          </div>
				</xsl:if>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
        <h4 class="ui-bar-b"><strong>Nutrition Calculator Variables</strong></h4>
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
						<label for="lblGender" >
							<strong>Gender of Person</strong>
						</label>
						<select class="Select225" id="lblGender">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;GenderOfServingPerson;string;10</xsl:attribute>
							</xsl:if>
                <option>
								<xsl:attribute name="value">female</xsl:attribute>
								<xsl:if test="(@GenderOfServingPerson = 'female')">
									<xsl:attribute name="selected"/>
								</xsl:if>female
							</option>
							<option>
								<xsl:attribute name="value">male</xsl:attribute>
								<xsl:if test="(@GenderOfServingPerson = 'male')">
									<xsl:attribute name="selected"/>
								</xsl:if>male
							</option>
						</select>
					</div>
					<div class="ui-block-b">
						<label for="lblActualCaloriesPerDay" >
							<strong>Actual Calories Per Day</strong>
						</label>
						<select class="Select225" id="lblActualCaloriesPerDay">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ActualCaloriesPerDay;decimal;8</xsl:attribute>
							</xsl:if>
                <option>
								<xsl:attribute name="value">2000</xsl:attribute>
								<xsl:if test="(contains(@ActualCaloriesPerDay, '2000'))">
									<xsl:attribute name="selected"/>
								</xsl:if>2000
							</option>
							<option>
								<xsl:attribute name="value">2500</xsl:attribute>
								<xsl:if test="(contains(@ActualCaloriesPerDay, '2500'))">
									<xsl:attribute name="selected"/>
								</xsl:if>2500
							</option>
						</select>
					</div>
        </div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblActualServingsPerContainer"><strong>Actual Servings Per Container</strong></label>
						<input type="text" id="lblActualServingsPerContainer">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ActualServingsPerContainer;decimal;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@ActualServingsPerContainer"/></xsl:attribute>
						</input>
					</div>
					<div class="ui-block-b">
						<label for="lblContainerSize">
							<strong>Container Size (units: <xsl:value-of select="@InputUnit1"/>)</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblContainerSize">
								<xsl:attribute name="value">
									<xsl:value-of select="@ContainerSize"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblContainerSize">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ContainerSize;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ContainerSize"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblWeightOfServingPerson">
							<strong>Weight Of Serving Person</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblWeightOfServingPerson">
								<xsl:attribute name="value"><xsl:value-of select="@WeightOfServingPerson"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblWeightOfServingPerson">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;WeightOfServingPerson;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@WeightOfServingPerson"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblWeightUnitsOfServingPerson">
							<strong>Weight Units Of Serving Person</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblWeightUnitsOfServingPerson">
								<xsl:attribute name="value"><xsl:value-of select="@WeightUnitsOfServingPerson"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblWeightUnitsOfServingPerson">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;WeightUnitsOfServingPerson;string;25</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@WeightUnitsOfServingPerson"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblUnitPrice" >Unit Price</label>
					<xsl:choose>
					<xsl:when test="($docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries')">
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice1', @InputPrice1, 'decimal', '8')"/>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice1', @InputPrice1, 'decimal', '8')"/>
							</xsl:if>
					</xsl:when>
					<xsl:otherwise>
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input id="lblUnitPrice" type="text" >
									<xsl:attribute name="value">
										<xsl:value-of select="@InputPrice1"/>
									</xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input id="lblUnitPrice" type="text" >
									<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;InputPrice1;decimal;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@InputPrice1"/></xsl:attribute>
								</input>
							</xsl:if>
					</xsl:otherwise>
					</xsl:choose>
          </div>
					<div class="ui-block-b">
						<label for="lblMarketValue" >Container Price</label>
					<xsl:choose>
					<xsl:when test="($docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries')">
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice3', @InputPrice3, 'decimal', '8')"/>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice3', @InputPrice3, 'decimal', '8')"/>
							</xsl:if>
					</xsl:when>
					<xsl:otherwise>
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input  id="lblMarketValue" type="text" >
									<xsl:attribute name="value">
										<xsl:value-of select="@InputPrice3"/>
									</xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input id="lblMarketValue" type="text" >
									<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;InputPrice3;decimal;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@InputPrice3"/></xsl:attribute>
								</input>
							</xsl:if>
					</xsl:otherwise>
					</xsl:choose>
				</div>
        </div>
        <h4 class="ui-bar-b"><strong>Enter the exact numbers (no adjustments) found on Nutrition Facts.</strong></h4>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblServingsPerContainer">
							<strong>Servings Per Container</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblServingsPerContainer">
								<xsl:attribute name="value">
									<xsl:value-of select="@ServingsPerContainer"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblServingsPerContainer">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ServingsPerContainer;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ServingsPerContainer"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblCaloriesPerServing"><strong>Calories Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblCaloriesPerServing">
								<xsl:attribute name="value"><xsl:value-of select="@CaloriesPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblCaloriesPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;CaloriesPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@CaloriesPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblCaloriesFromFatPerServing"><strong>Calories From Fat Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblCaloriesFromFatPerServing">
								<xsl:attribute name="value"><xsl:value-of select="@CaloriesFromFatPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblCaloriesFromFatPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;CaloriesFromFatPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@CaloriesFromFatPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblTotalFatPerServing"><strong>Total Fat Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblTotalFatPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@TotalFatPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblTotalFatPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TotalFatPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@TotalFatPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblSaturatedFatPerServing"><strong>Saturated Fat Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblSaturatedFatPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@SaturatedFatPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblSaturatedFatPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SaturatedFatPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@SaturatedFatPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblTransFatPerServing"><strong>Trans Fat Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblTransFatPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@TransFatPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblTransFatPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TransFatPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@TransFatPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblCholesterolPerServing"><strong>Cholesterol Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblCholesterolPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@CholesterolPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblCholesterolPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;CholesterolPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@CholesterolPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblSodiumPerServing"><strong>Sodium Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblSodiumPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@SodiumPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblSodiumPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SodiumPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@SodiumPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblPotassiumPerServing"><strong>Potassium Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblPotassiumPerServing">
								<xsl:attribute name="value">
									<xsl:value-of select="@PotassiumPerServing"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblPotassiumPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;PotassiumPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@PotassiumPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblTotalCarbohydratePerServing"><strong>Total Carbohydrate Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblTotalCarbohydratePerServing">
								<xsl:attribute name="value"><xsl:value-of select="@TotalCarbohydratePerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblTotalCarbohydratePerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TotalCarbohydratePerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@TotalCarbohydratePerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblOtherCarbohydratePerServing">
							<strong>Other Carbohydrate Per Serving</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblOtherCarbohydratePerServing">
								<xsl:attribute name="value"><xsl:value-of select="@OtherCarbohydratePerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblOtherCarbohydratePerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;OtherCarbohydratePerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OtherCarbohydratePerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblDietaryFiberPerServing"><strong>Dietary Fiber Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblDietaryFiberPerServing">
								<xsl:attribute name="value"><xsl:value-of select="@DietaryFiberPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblDietaryFiberPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;DietaryFiberPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@DietaryFiberPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblSugarsPerServing"><strong>Sugars Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblSugarsPerServing">
								<xsl:attribute name="value"><xsl:value-of select="@SugarsPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblSugarsPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SugarsPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@SugarsPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblProteinPerServing"><strong>Protein Per Serving</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblProteinPerServing">
								<xsl:attribute name="value"><xsl:value-of select="@ProteinPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblProteinPerServing">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ProteinPerServing;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ProteinPerServing"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblVitaminAPercentDailyValue"><strong>Vitamin A Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblVitaminAPercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@VitaminAPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblVitaminAPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;VitaminAPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@VitaminAPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblVitaminCPercentDailyValue"><strong>Vitamin C Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblVitaminCPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@VitaminCPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblVitaminCPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;VitaminCPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@VitaminCPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblVitaminDPercentDailyValue"><strong>Vitamin D Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblVitaminDPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@VitaminDPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblVitaminDPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;VitaminDPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@VitaminDPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblCalciumPercentDailyValue"><strong>Calcium Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblCalciumPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@CalciumPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblCalciumPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;CalciumPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@CalciumPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblIronPercentDailyValue"><strong>Iron Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblIronPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@IronPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblIronPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;IronPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@IronPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblThiaminPercentDailyValue"><strong>Thiamin Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblThiaminPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@ThiaminPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblThiaminPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ThiaminPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ThiaminPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblFolatePercentDailyValue"><strong>Folate Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblFolatePercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@FolatePercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblFolatePercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;FolatePercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@FolatePercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblRiboflavinPercentDailyValue"><strong>Riboflavin Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblRiboflavinPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@RiboflavinPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblRiboflavinPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;RiboflavinPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@RiboflavinPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblNiacinPercentDailyValue"><strong>Niacin Percent Daily Value</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblNiacinPercentDailyValue">
								<xsl:attribute name="value">
									<xsl:value-of select="@NiacinPercentDailyValue"/>
								</xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblNiacinPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;NiacinPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@NiacinPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblVitaminB6PercentDailyValue">
							<strong>VitaminB6 Percent Daily Value</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblVitaminB6PercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@VitaminB6PercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblVitaminB6PercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;VitaminB6PercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@VitaminB6PercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblVitaminB12PercentDailyValue">
							<strong>VitaminB12 Percent Daily Value</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblVitaminB12PercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@VitaminB12PercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblVitaminB12PercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;VitaminB12PercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@VitaminB12PercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblPhosphorousPercentDailyValue">
							<strong>Phosphorous Percent Daily Value</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblPhosphorousPercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@PhosphorousPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblPhosphorousPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;PhosphorousPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@PhosphorousPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblMagnesiumPercentDailyValue">
							<strong>Magnesium Percent Daily Value</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblMagnesiumPercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@MagnesiumPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblMagnesiumPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;MagnesiumPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@MagnesiumPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblZincPercentDailyValue">
							<strong>Zinc Percent Daily Value</strong>
						</label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input  type="text" id="lblZincPercentDailyValue">
								<xsl:attribute name="value"><xsl:value-of select="@ZincPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input  type="text" id="lblZincPercentDailyValue">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ZincPercentDailyValue;decimal;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ZincPercentDailyValue"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
								
					</div>
				</div>
				<div class="ui-grid-a"></div>
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
			</xsl:when>
			<xsl:otherwise>
				<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
				sure this is the right calculator?</h3>
			</xsl:otherwise>
		</xsl:choose>
		</div>
    <div id="divstepthree">
			<xsl:variable name="filexttype"><xsl:value-of select="DisplayDevPacks:GetSubString($selectedFileURIPattern,'/','5')" /></xsl:variable>
			<xsl:if test="($lastStepNumber != 'stepfour')">
        <h4 class="ui-bar-b"><strong>Step 3 of 3. Save</strong></h4>
				<xsl:if test="$filexttype = 'temp' or contains($docToCalcNodeName, 'linkedview')">
          <p>
							<strong>Temporary Calculations.</strong> Calculations are temporarily saved when temporary calculations are run.
					</p>
				</xsl:if>
				<xsl:if test="($filexttype != 'temp') and (contains($docToCalcNodeName, 'linkedview') = false)">
					<xsl:variable name="calcParams4a">'&amp;step=stepfour&amp;savemethod=calcs<xsl:value-of select="$calcParams"/>'</xsl:variable>
          <p>
							<strong>Method 1.</strong> Do you wish to save step 2's calculations? These calculations are viewed by opening this particular calculator addin.
					</p>
					<xsl:if test="($viewEditType = 'full')">
						<xsl:value-of select="DisplayDevPacks:MakeDevTreksButton('savecalculation', 'SubmitButton1Enabled150', 'Save Calcs', $contenturipattern, $selectedFileURIPattern, 'prepaddin', 'linkedviews', 'runaddin', 'none', $calcParams4a)" />
					</xsl:if>
				</xsl:if>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepfour')
				or ($lastStepNumber = 'stepthree' and $filexttype = 'temp')
				or ($lastStepNumber = 'stepthree' and contains($docToCalcNodeName, 'linkedview'))">
						<h3>
							<xsl:if test="($saveMethod = 'calcs'
									or $filexttype = 'temp'
									or contains($docToCalcNodeName, 'linkedview'))">
								Your calculations have been saved. The calculations can be viewed whenever
								this calculator addin is opened.
							</xsl:if>
						</h3>
			</xsl:if>
      <br /><br />
		</div>
		<div id="divsteplast">
			<h4 class="ui-bar-b"><strong>Instructions (beta)</strong></h4>
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
				<li><strong>Step 2. Actual Servings Per Container:</strong> Reflects the actual number of servings consumed by the user using this food container.</li>
				<li><strong>Step 2. Food Nutrition Facts:</strong> The food facts found on the back of food containers in the USA.</li>
			</ul>
    </div> 
		</div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>

