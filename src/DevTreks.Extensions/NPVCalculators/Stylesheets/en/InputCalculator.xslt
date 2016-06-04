<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, October -->
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
					<xsl:value-of select="DisplayDevPacks:WriteMenuSteps('3')"/>
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
      <h4 class="ui-bar-b"><strong>Input Calculation View</strong></h4>
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
					This calculator calculates discounted input totals.
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
	    <xsl:if test="(($docToCalcNodeName != 'inputseries') or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				<xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
          @UseSameCalculator, @Overwrite)"/>
		  </xsl:if>
      <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, @RelatedCalculatorsType)"/>
      </div>
      <xsl:if test="($lastStepNumber = 'stepthree')
					or ($viewEditType != 'full')
					or (@TOC >= 0)
					or (@TAOH >= 0)
					or (@TCAP >= 0)">
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblDate" >Date Applied</label>: 
						<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputDate', @InputDate, 'datetime', '8')"/>
					</div>
					<div class="ui-block-b">
						<label for="lblInputDate" >End Of Period Date</label>: 
						<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('Date', @Date, 'datetime', '8')"/>
					</div>
				</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputPrice1">OC Price</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice1', @InputPrice1, 'decimal', '8')"/>
						</div>
						<div class="ui-block-b">
							<label for="lblInputInputPrice1Amount">OC Amount</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice1Amount', @InputPrice1Amount, 'double', '8')"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputUnit1">OC Unit</label>: 
							<xsl:value-of select="@InputUnit1" />
						</div>
						<div class="ui-block-b">
										
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputPrice2">AOH Price</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice2', @InputPrice2, 'decimal', '8')"/>
						</div>
						<div class="ui-block-b">
							<label for="lblInputInputPrice2Amount">AOH Amount</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice2Amount', @InputPrice2Amount, 'double', '8')"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputUnit2">AOH Unit</label>: 
							<xsl:value-of select="@InputUnit2" />
						</div>
						<div class="ui-block-b">
										
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputPrice3">Market Value</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice3', @InputPrice3, 'decimal', '8')"/>
						</div>
						<div class="ui-block-b">
							<label for="lblInputInputPrice3Amount">CAP Amount</label>: 
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('InputPrice3Amount', @InputPrice3Amount, 'double', '8')"/>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputUnit3">CAP Unit</label>: 
							<xsl:value-of select="@InputUnit3" />
						</div>
						<div class="ui-block-b">
						</div>
					</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						Operating Cost per unit: 
						<xsl:value-of select="@TOC" />
					</div>
					<div class="ui-block-b">
						Operating Cost Interest: 
						<xsl:value-of select="@TOC_INT" />
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<strong>Discounted Operating Cost</strong>
					</div>
					<div class="ui-block-b">
						<xsl:value-of select="format-number((@TOC + @TOC_INT), '#,###.00')"/>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						Allocated Overhead Cost per unit: 
						<xsl:value-of select="@TAOH" />
					</div>
					<div class="ui-block-b">
						Allocated Overhead Cost Interest: 
						<xsl:value-of select="@TAOH_INT" />
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<strong>Discounted Allocated Overhead Cost</strong>
					</div>
					<div class="ui-block-b">
						<xsl:value-of select="format-number((@TAOH + @TAOH_INT), '#,###.00')"/>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						Capital Cost per unit: 
						<xsl:value-of select="@TCAP" />
					</div>
					<div class="ui-block-b">
						Capital Cost Interest: 
						<xsl:value-of select="@TCAP_INT" />
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<strong>Discounted Capital Cost</strong>
					</div>
					<div class="ui-block-b">
						<xsl:value-of select="format-number((@TCAP + @TCAP_INT), '#,###.00')"/>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<strong>Incentive Adjusted Total Costs</strong>
					</div>
					<div class="ui-block-b">
						<xsl:value-of select="@TINCENT" />
					</div>
				</div>
				<xsl:if test="(@EffectiveLife > 1)">
					<div class="ui-grid-a">
						<div class="ui-block-a">
							Amortized Allocated OH Cost per unit: 
							<xsl:value-of select="@TAMAOH" />
						</div>
						<div class="ui-block-b">
							Amortized Allocated OH Cost Interest: 
							<xsl:value-of select="@TAMAOH_INT" />
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Incentive Adjusted Amortized Costs</strong>
						</div>
						<div class="ui-block-b">
							<xsl:value-of select="@TAMINCENT" />
						</div>
					</div>
				</xsl:if>
        </div>
			</xsl:if>
			<div>
        <label for="CalculatorName" class="ui-hidden-accessible"></label>
				<input id="CalculatorName" type="text" class="Input400Bold" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
          </xsl:if>
					<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
				</input>
		</div>
		<div class="ui-field-contain">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
          <legend>Is Discounted?</legend>
          <input class="Input25"  type="radio" id="lblDiscountYorN1">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;DiscountYorN;boolean;1</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value">1</xsl:attribute>
            <xsl:if test="(@DiscountYorN = '1')">
              <xsl:attribute name="checked">true</xsl:attribute>
            </xsl:if>
          </input>
          <label for="lblDiscountYorN1">True</label>
          <input class="Input25"  type="radio" id="lblDiscountYorN2">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;DiscountYorN;boolean;1</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value">0</xsl:attribute>
            <xsl:if test="(@DiscountYorN != '1')">
              <xsl:attribute name="checked">true</xsl:attribute>
            </xsl:if>
          </input>
          <label for="lblDiscountYorN2">False</label>
        </fieldset>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblEOPDate" >End of Period Date</label>
					<input type="text" class="Input75Center" id="lblEOPDate">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Date;datetime;8</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@Date" /></xsl:attribute>
					</input>
				</div>
        <div class="ui-block-b">
					<label for="lblInputTimes">Times Applied</label>
					<input class="Input75" type="text" id="lblInputTimes">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputTimes;real;8</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@InputTimes" /></xsl:attribute>
					</input>
				</div>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblEffectiveLife">Effective Life (years)</label>								
          <input class="Input75" type="text" id="lblEffectiveLife">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;EffectiveLife;int;4</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@EffectiveLife" /></xsl:attribute>
					</input>
				</div>
				<div class="ui-block-b">
					<label for="lblSalvageValue">Salvage Value</label>
					<input class="Input75" type="text" id="lblSalvageValue">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SalvageValue;decimal;8</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SalvageValue" /></xsl:attribute>
					</input>
				</div>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblOutputIncentiveAmount" >Incentive Amount</label>
					<input class="Input75" id="lblOutputIncentiveAmount">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IncentiveAmount;double;8</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('IncentiveAmount', @IncentiveAmount, 'double', '8')"/></xsl:attribute>
					</input>
				</div>
				<div class="ui-block-b">
					<label for="lblOutputIncentiveRate" >Incentive Rate</label>
						<input class="Input75" id="lblOutputIncentiveRate">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IncentiveRate;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('IncentiveRate', @IncentiveRate, 'double', '8')"/></xsl:attribute>
						</input>
				</div>
			</div>
			<xsl:if test="($docToCalcNodeName = 'inputgroup'
					or contains($docToCalcNodeName, 'linkedview'))">
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputPrice">Input OC Price</label>
							<input class="Input75" type="text" id="lblInputPrice">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice1;decimal;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice1" /></xsl:attribute>
							</input>
						</div>
						<div class="ui-block-b">
							<label for="lblInputAmount">Input OC Amount</label>
							<input class="Input75" type="text" id="lblInputAmount">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice1Amount;double;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice1Amount" /></xsl:attribute>
							</input>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputDate">Date Applied</label>
							<input class="Input75" type="text" id="lblInputDate">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputDate;datetime;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputDate" /></xsl:attribute>
							</input>
						</div>
						<div class="ui-block-b">
							<label for="lblInputInputUnit1">Input OC Unit</label>
							<input class="Input75" type="text" id="lblInputInputUnit1">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputUnit1;string;25</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputUnit1" /></xsl:attribute>
							</input>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputPrice2">Input AOH Price</label>
							<input class="Input75" type="text" id="lblInputPrice2">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice2;decimal;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice2" /></xsl:attribute>
							</input>
						</div>
						<div class="ui-block-b">
							<label for="lblInputAmount2">Input AOH Amount</label>
							<input class="Input75" type="text" id="lblInputAmount2">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice2Amount;double;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice2Amount" /></xsl:attribute>
							</input>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputUnit2">Input AOH Unit</label>										
              <input class="Input75" type="text" id="lblInputInputUnit2">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputUnit2;string;25</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputUnit2" /></xsl:attribute>
							</input>
						</div>
						<div class="ui-block-b">
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputPrice3">Input Market Value</label>
							<input class="Input75" type="text" id="lblInputPrice3">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice3;decimal;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice3" /></xsl:attribute>
							</input>
										
						</div>
						<div class="ui-block-a">
							<label for="lblInputAmount3">Input Market Value Amount</label>
							<input class="Input75" type="text" id="lblInputAmount3">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputPrice3Amount;double;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputPrice3Amount" /></xsl:attribute>
							</input>
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<label for="lblInputInputUnit3">Input Market Value Unit</label>
							<input class="Input75" type="text" id="lblInputInputUnit3">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputUnit3;string;25</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@InputUnit3" /></xsl:attribute>
							</input>
						</div>
						<div class="ui-block-b">
						</div>
					</div>
				</xsl:if>
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
				<li><strong>Step 2. Input Variables:</strong> These are the same input variables found in operating and capital budgets and are used in the same manner.</li>
			</ul>
		  </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>References</h4>
    </div>
  </div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>
