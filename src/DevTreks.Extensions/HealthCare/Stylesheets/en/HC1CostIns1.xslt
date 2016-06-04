<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
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
					<xsl:value-of select="DisplayDevPacks:WriteMenuSteps('5')"/>
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
      <h4 class="ui-bar-b"><strong>Health Care Cost Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This calculator calculates the health care cost of inputs based on insurance provider, health care provider, 
              and health care recipient costs. It also calculates a qualitative rating of health care inputs.
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
      <h4 class="ui-bar-b"><strong>Step 1 of 5. Make Selections</strong></h4>
		  <xsl:variable name="calcParams1">'&amp;step=steptwo<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams1)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'steptwo')">
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
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
		</div>
    <div id="divsteptwo">
      <h4 class="ui-bar-b"><strong>Step 2 of 5. Make Selections</strong></h4>
		  <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
			  <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <div>
					<label for="lblHealthCareProvider"><strong>Health Care Provider</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblHealthCareProvider">
							<xsl:attribute name="value"><xsl:value-of select="@HealthCareProvider"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblHealthCareProvider">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;HealthCareProvider;string;150</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@HealthCareProvider"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
				<label for="lblInsuranceProvider"><strong>Insurance Provider</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblInsuranceProvider">
						<xsl:attribute name="value"><xsl:value-of select="@InsuranceProvider"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblInsuranceProvider">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;InsuranceProvider;string;150</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@InsuranceProvider"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblPackageType"><strong>Package Type (i.e. High-Family, Low-Self)</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblPackageType">
						<xsl:attribute name="value"><xsl:value-of select="@PackageType"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblPackageType">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;PackageType;string;150</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@PackageType"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
					<label for="lblPostalCode" ><strong>Postal Code of Health Care Provider</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblPostalCode">
							<xsl:attribute name="value"><xsl:value-of select="@PostalCode" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblPostalCode">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PostalCode;string;25</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@PostalCode" /></xsl:attribute>
						</input>
					</xsl:if>
      </div>
      <div class="ui-field-contain">
         <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
          <legend>Health Care Input Is In Network?</legend>
						<input   type="radio" id="lblIsInNetwork1">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IsInNetwork;boolean;1</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value">1</xsl:attribute>
							<xsl:if test="(@IsInNetwork = '1')">
								<xsl:attribute name="checked">true</xsl:attribute>
							</xsl:if>
						</input>
            <label for="lblIsInNetwork1" >True</label>
						<input   type="radio" id="lblIsInNetwork2">
							<xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IsInNetwork;boolean;1</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value">0</xsl:attribute>
							<xsl:if test="(@IsInNetwork != '1')">
								<xsl:attribute name="checked">true</xsl:attribute>
							</xsl:if>
						</input>
            <label for="lblIsInNetwork2" >False</label>
			  </fieldset>
			</div>
      <div>
				<label for="lblConditionSeverity" ><strong>Severity of Health Condition</strong></label>
				<select class="Select225" id="lblConditionSeverity" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ConditionSeverity;string;50</xsl:attribute>
          </xsl:if>
            <option>
						<xsl:attribute name="value">notsevere</xsl:attribute>
						<xsl:if test="(@ConditionSeverity = 'notsevere')">
							<xsl:attribute name="selected" />
						</xsl:if>Not Severe
					</option>
          <option>
						<xsl:attribute name="value">slightlysevere</xsl:attribute>
						<xsl:if test="(@ConditionSeverity = 'slightlysevere')">
							<xsl:attribute name="selected" />
						</xsl:if>Slightly Severe
					</option>
					<option>
						<xsl:attribute name="value">moderatelysevere</xsl:attribute>
						<xsl:if test="(@ConditionSeverity = 'moderatelysevere')">
							<xsl:attribute name="selected" />
						</xsl:if>Moderately Severe
					</option>
          <option>
						<xsl:attribute name="value">extremelysevere</xsl:attribute>
						<xsl:if test="(@ConditionSeverity = 'extremelysevere')">
							<xsl:attribute name="selected" />
						</xsl:if>Extremely Severe
					</option>
				</select>
			</div>
      <h4 class="ui-bar-b"><strong>Optional Additional Costs (Opportunity Costs)</strong></h4>
      <div class="ui-field-contain">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
          <legend>Add Additional Prices To Base Input Price?</legend>
            <input   type="radio" id="lblUseAddedCostsInInput1">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;UseAddedCostsInInput;boolean;1</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">1</xsl:attribute>
              <xsl:if test="(@UseAddedCostsInInput = '1')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <label for="lblUseAddedCostsInInput1" >True</label>
            <input   type="radio" id="lblUseAddedCostsInInput2">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;UseAddedCostsInInput;boolean;1</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">0</xsl:attribute>
              <xsl:if test="(@UseAddedCostsInInput != '1')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <label for="lblUseAddedCostsInInput2" >False</label>
        </fieldset>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblAdditionalName1" ><strong>Additional Price 1 Name</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalName1">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalName1" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalName1">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalName1;string;25</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalName1" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblAdditionalName2" ><strong>Additional Price 2 Name</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalName2">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalName2" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalName2">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalName2;string;25</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalName2" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblAdditionalUnit1" ><strong>Additional Unit 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalUnit1">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalUnit1" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalUnit1">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalUnit1;string;25</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalUnit1" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblAdditionalUnit2" ><strong>Additional Unit 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalUnit2">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalUnit2" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalUnit2">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalUnit2;string;25</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalUnit2" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblAdditionalPrice1" ><strong>Additional Price 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalPrice1">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalPrice1" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalPrice1">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalPrice1;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalPrice1" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblAdditionalPrice2" ><strong>Additional Price 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalPrice2">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalPrice2" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalPrice2">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalPrice2;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalPrice2" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblAdditionalAmount1" ><strong>Additional Amount 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalAmount1">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalAmount1" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalAmount1">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalAmount1;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalAmount1" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblAdditionalAmount2" ><strong>Additional Amount 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAdditionalAmount2">
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalAmount2" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAdditionalAmount2">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalAmount2;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AdditionalAmount2" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
			<div>
				<label for="lblAdditionalCostsDescription"><strong>Additional Prices Description</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<textarea class="Text75H100PCW" id="lblAdditionalCostsDescription">
						<xsl:value-of select="@AdditionalCostsDescription" />
					</textarea>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<textarea class="Text75H100PCW" id="lblAdditionalCostsDescription">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AdditionalCostsDescription;string;255</xsl:attribute>
						<xsl:value-of select="@AdditionalCostsDescription" />
					</textarea>
				</xsl:if>
			</div>
</div>
<div id="divstepthree">
	<h4 class="ui-bar-b"><strong>Step 3 of 5. Input Quality Rating</strong></h4>
		<xsl:variable name="calcParams3">'&amp;step=stepfour<xsl:value-of select="$calcParams" />'</xsl:variable>
		<xsl:if test="($viewEditType = 'full')">
			<xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
		</xsl:if>
		<xsl:if test="($lastStepNumber = 'stepfour')">
        <h4 class="ui-bar-b"><strong>Success. Please review the calculations below.</strong></h4>
		</xsl:if>
  <h4 class="ui-bar-b"><strong>Input Quality of Health Care Rating.</strong></h4>
  <p><strong>Only complete this step for the primary inputs or services used for your health care condition.</strong></p>
    <h4 class="ui-bar-b">
		  <strong>Using a scale of 0 to 10, rate the quality of this input or service. 
      0 = not applicable or don't know. Ratings can be whole numbers, such as 10, or single digits, such as 5.5.</strong>
	  </h4>
      <div>
				<label for="lblDiagnosisQualityRating"><strong>Diagnosis Quality.</strong> How well did the provider use this input or service to diagnose your heath care condition? (0 = not applicable or not known, 1 = very poorly, 10 = excellently)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblDiagnosisQualityRating">
						<xsl:attribute name="value"><xsl:value-of select="@DiagnosisQualityRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblDiagnosisQualityRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;DiagnosisQualityRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@DiagnosisQualityRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTreatmentQualityRating"><strong>Treatment Quality.</strong> How well did the provider use this input or service to treat your heath care condition?  (0 = not applicable or not known, 1 = very poorly, 10 = excellently)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblTreatmentQualityRating">
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentQualityRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblTreatmentQualityRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TreatmentQualityRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentQualityRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTreatmentBenefitRating"><strong>Treatment Benefits.</strong> How much did this input or service contribute to the resolution of your health care condition (i.e. did it lead to you getting fully well)? (0 = not applicable or not known, 1 = not all all, 10 = a great deal)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblTreatmentBenefitRating">
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentBenefitRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblTreatmentBenefitRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TreatmentBenefitRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentBenefitRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTreatmentCostRating"><strong>Treatment Costs.</strong> Did the costs of this input or service justify the benefits? (0 = not applicable or not known; 1 = no, not at all; 10 = yes, fully)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblTreatmentCostRating">
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentCostRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblTreatmentCostRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TreatmentCostRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TreatmentCostRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblKnowledgeTransferRating"><strong>Knowledge Transfer.</strong> Did you receive enough correct information about this input or service to make an informed decision about the need for using it and whether or not it worked.? (0 = not applicable or not known; 1 = no, not at all; 10 = yes, fully)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblKnowledgeTransferRating">
						<xsl:attribute name="value"><xsl:value-of select="@KnowledgeTransferRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblKnowledgeTransferRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;KnowledgeTransferRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@KnowledgeTransferRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblConstrainedChoiceRating"><strong>Constrained Choice.</strong> Is this the input, service, and/or provider you would have chosen for your health condition if you faced no constraints (such as money, copayment limitations, coverage restrictions, health care knowledge, availability of care providers).? (0 = not applicable or not known; 1 = no, not at all; 10 = yes, fully)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblConstrainedChoiceRating">
						<xsl:attribute name="value"><xsl:value-of select="@ConstrainedChoiceRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblConstrainedChoiceRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ConstrainedChoiceRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@ConstrainedChoiceRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblInsuranceCoverageRating"><strong>Insurance Coverage.</strong> Divide the percentage of the cost of this input or service paid by your insurance company by 10. (0 = not applicable or not known; 0.1 = 1% / 10; 10 = 100% / 10)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblInsuranceCoverageRating">
						<xsl:attribute name="value"><xsl:value-of select="@InsuranceCoverageRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblInsuranceCoverageRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;InsuranceCoverageRating;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@InsuranceCoverageRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <p>Are you willing to participate in a more comprehensive survey about the benefits and costs of your health care?</p>
      <div class="ui-field-contain">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
          <legend>
            <strong>Survey Participation.</strong> 
          </legend>
          <input   type="radio" id="lblWillDoSurvey1">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;WillDoSurvey;boolean;1</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value">1</xsl:attribute>
            <xsl:if test="(@WillDoSurvey = '1')">
              <xsl:attribute name="checked">true</xsl:attribute>
            </xsl:if>
          </input>
          <label for="lblWillDoSurvey1">True</label>
          <input   type="radio" id="lblWillDoSurvey2">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;WillDoSurvey;boolean;1</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value">0</xsl:attribute>
            <xsl:if test="(@WillDoSurvey != '1')">
              <xsl:attribute name="checked">true</xsl:attribute>
            </xsl:if>
          </input>
          <label for="lblWillDoSurvey2">False</label>
        </fieldset>
			</div>
      <div>
				<label for="lblBCAssessment"><strong>Input Quality Assessment</strong> (explain your ratings)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<textarea class="Text200H100PCW" id="lblBCAssessment">
						<xsl:value-of select="@InputQualityAssessment" />
					</textarea>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<textarea class="Text200H100PCW" id="lblBCAssessment">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;InputQualityAssessment;string;500</xsl:attribute>
						<xsl:value-of select="@InputQualityAssessment" />
					</textarea>
				</xsl:if>
			</div>
</div>
<div id="divstepfour">
			<h4 class="ui-bar-b"><strong>Step 4 of 5. Calculate</strong></h4>
			<xsl:variable name="calcParams4">'&amp;step=stepfive<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams4)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepfive')">
        <h4 class="ui-bar-b">
          <strong>Success. Please review the calculations below.</strong>
        </h4>
			</xsl:if>
				<xsl:if test="@ReceiverCost >= 0">
          <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d">
            <h4 class="ui-bar-b">
              <strong>Costs</strong>
            </h4>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <strong>Cost Rating</strong>:
                <xsl:value-of select="@CostRating" />
              </div>
              <div class="ui-block-b">
                <strong>Recipient Cost (=input.OCPrice)</strong>:
                <xsl:value-of select="@ReceiverCost" />
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <strong>Incentives Cost</strong>:
                <xsl:value-of select="@IncentivesCost" />
              </div>
              <div class="ui-block-b">
                <strong>Insurance Provider Cost</strong>:
                <xsl:value-of select="@InsuranceProviderCost" />
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <strong>Adjusted Cost</strong>:
                <xsl:value-of select="@AdjustedPrice" />
              </div>
              <div class="ui-block-b">
                <strong>Health Care Provider Cost</strong>:
                <xsl:value-of select="@HealthCareProviderCost"/>
              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <strong>Additional Cost (Add. Cost 1 plus 2)</strong>:
                <xsl:value-of select="@AdditionalCost" />
              </div>
              <div class="ui-block-b">

              </div>
            </div>
            <div class="ui-grid-a">
              <div class="ui-block-a">
                <strong>Additional Cost 1</strong>:
                <xsl:value-of select="@AdditionalCost1" />
              </div>
              <div class="ui-block-b">
                <strong>Additional Cost 2</strong>:
                <xsl:value-of select="@AdditionalCost2" />
              </div>
            </div>
          </div>
			</xsl:if>
      <div>
        <label for="CalculatorName" class="ui-hidden-accessible"></label>
				<input id="CalculatorName" type="text" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
          </xsl:if>
					<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
				</input>
			</div>
			<div>
					<label for="lblHC1PriceType" ><strong>Price Type To Use To Set input.OCPrice</strong></label>
					<select class="Select225" id="lblHC1PriceType" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;HC1PriceType;string;50</xsl:attribute>
						</xsl:if>
            <option>
							<xsl:attribute name="value">contractedprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'contractedprice')">
								<xsl:attribute name="selected" />
							</xsl:if>Contracted Price
						</option>
            <option>
							<xsl:attribute name="value">baseprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'baseprice')">
								<xsl:attribute name="selected" />
							</xsl:if>Base Price
						</option>
						<option>
							<xsl:attribute name="value">adjustedprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'adjustedprice')">
								<xsl:attribute name="selected" />
							</xsl:if>Adjusted Price
						</option>
            <option>
							<xsl:attribute name="value">listprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'listprice')">
								<xsl:attribute name="selected" />
							</xsl:if>List Price
						</option>
            <option>
							<xsl:attribute name="value">marketprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'marketprice')">
								<xsl:attribute name="selected" />
							</xsl:if>Market Price
						</option>
						<option>
							<xsl:attribute name="value">productioncostprice</xsl:attribute>
							<xsl:if test="(@HC1PriceType = 'productioncostprice')">
								<xsl:attribute name="selected" />
							</xsl:if>Production Cost Price
						</option>
					</select>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblBasePrice" ><strong>Base Price (input.CAPPrice)</strong></label>
				<xsl:choose>
				<xsl:when test="($docToCalcNodeName = 'input' 
						or $docToCalcNodeName = 'inputseries')">
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('BasePrice', @BasePrice, 'decimal', '8')"/>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<xsl:choose>
							<xsl:when test="(contains($docToCalcNodeName, 'devpack')
								or contains($docToCalcNodeName, 'linkedview')
								or contains($selectedFileURIPattern, 'temp'))">
								<input id="lblBasePrice" type="text">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;BasePrice;double;8</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@BasePrice" /></xsl:attribute>
								</input>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('BasePrice', @BasePrice, 'decimal', '8')"/>
							</xsl:otherwise>
						</xsl:choose>
						</xsl:if>
				</xsl:when>
				<xsl:otherwise>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input id="lblBasePrice" type="text">
								<xsl:attribute name="value"><xsl:value-of select="@BasePrice" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input id="lblBasePrice" type="text">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;BasePrice;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@BasePrice" /></xsl:attribute>
							</input>
						</xsl:if>
				</xsl:otherwise>
				</xsl:choose>
        </div>
				<div class="ui-block-a">
					<label for="lblContractedPrice" ><strong>Contracted Price</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblContractedPrice">
							<xsl:attribute name="value"><xsl:value-of select="@ContractedPrice" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblContractedPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ContractedPrice;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@ContractedPrice" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblBasePriceAdjustment" ><strong>Base Price Adjustment</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblBasePriceAdjustment">
							<xsl:attribute name="value"><xsl:value-of select="@BasePriceAdjustment" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblBasePriceAdjustment">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;BasePriceAdjustment;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@BasePriceAdjustment" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblProductionCostPrice" ><strong>Production Cost Price</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblProductionCostPrice">
							<xsl:attribute name="value"><xsl:value-of select="@ProductionCostPrice" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblProductionCostPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ProductionCostPrice;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@ProductionCostPrice" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblListPrice" ><strong>List Price</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblListPrice">
							<xsl:attribute name="value"><xsl:value-of select="@ListPrice" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblListPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ListPrice;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@ListPrice" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblMarketPrice" ><strong>Market Price (discounted price)</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblMarketPrice">
							<xsl:attribute name="value"><xsl:value-of select="@MarketPrice" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblMarketPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MarketPrice;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@MarketPrice" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblAnnualPremium1" ><strong>Annual Premium Paid By Recipient</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAnnualPremium1">
							<xsl:attribute name="value"><xsl:value-of select="@AnnualPremium1" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAnnualPremium1">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AnnualPremium1;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AnnualPremium1" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
        <div class="ui-block-b">
					<label for="lblAnnualPremium2" ><strong>Annual Premium Paid By Other</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAnnualPremium2">
							<xsl:attribute name="value"><xsl:value-of select="@AnnualPremium2" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAnnualPremium2">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AnnualPremium2;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AnnualPremium2" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div>
				<label for="lblAssignedPremiumCost" ><strong>Annual Premiums Assigned to this Input</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblAssignedPremiumCost">
						<xsl:attribute name="value"><xsl:value-of select="@AssignedPremiumCost" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblAssignedPremiumCost">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AssignedPremiumCost;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@AssignedPremiumCost" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblCoPay1Amount" ><strong>CoPayment Amount 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblCoPay1Amount">
							<xsl:attribute name="value"><xsl:value-of select="@CoPay1Amount" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblCoPay1Amount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CoPay1Amount;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@CoPay1Amount" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblCoPay2Amount" ><strong>CoPayment Amount 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblCoPay2Amount">
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Amount" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblCoPay2Amount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CoPay2Amount;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Amount" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblCoPay2Rate" ><strong>CoPayment Rate 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblCoPay2Rate">
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Rate" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblCoPay2Rate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CoPay2Rate;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Rate" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblCoPay2Rate" ><strong>CoPayment Rate 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblCoPay2Rate">
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Rate" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblCoPay2Rate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CoPay2Rate;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@CoPay2Rate" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblIncentive1Amount" ><strong>Incentive Amount 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblIncentive1Amount">
							<xsl:attribute name="value"><xsl:value-of select="@Incentive1Amount" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblIncentive1Amount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Incentive1Amount;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@Incentive1Amount" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblIncentive2Amount" ><strong>Incentive Amount 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblIncentive2Amount">
							<xsl:attribute name="value"><xsl:value-of select="@Incentive2Amount" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblIncentive2Amount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Incentive2Amount;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@Incentive2Amount" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblIncentive1Rate" ><strong>Incentive Rate 1</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblIncentive1Rate">
							<xsl:attribute name="value"><xsl:value-of select="@Incentive1Rate" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblIncentive1Rate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Incentive1Rate;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@Incentive1Rate" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblIncentive2Rate" ><strong>Incentive Rate 2</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblIncentive2Rate">
							<xsl:attribute name="value"><xsl:value-of select="@Incentive2Rate" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblIncentive2Rate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Incentive2Rate;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@Incentive2Rate" /></xsl:attribute>
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
		</div>
		<div id="divstepfive">
			<xsl:variable name="filexttype"><xsl:value-of select="DisplayDevPacks:GetSubString($selectedFileURIPattern,'/','5')" /></xsl:variable>
			<xsl:if test="($lastStepNumber != 'stepsix')">
				<h4 class="ui-bar-b"><strong>Step 5 of 5. Save</strong></h4>
				<xsl:if test="$filexttype = 'temp' or contains($docToCalcNodeName, 'linkedview')">
          <p>
							<strong>Temporary Calculations.</strong> Calculations are temporarily saved when temporary calculations are run.
					</p>
				</xsl:if>
				<xsl:if test="($filexttype != 'temp') and (contains($docToCalcNodeName, 'linkedview') = false)">
					<xsl:variable name="calcParams5a">'&amp;step=stepsix&amp;savemethod=calcs<xsl:value-of select="$calcParams" />'</xsl:variable>
					<h4 class="ui-bar-b"><strong>Method 1.</strong> Do you wish to save these calculations? These calculations are viewed by opening this particular calculator addin.</h4>
					<xsl:if test="($viewEditType = 'full')">
						<xsl:value-of select="DisplayDevPacks:MakeDevTreksButton('savecalculation', 'SubmitButton1Enabled150', 'Save Calcs', $contenturipattern, $selectedFileURIPattern, 'prepaddin', 'linkedviews', 'runaddin', 'none', $calcParams5a)" />
					</xsl:if>
				</xsl:if>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepsix')
				or ($lastStepNumber = 'stepsix' and $filexttype = 'temp')
				or ($lastStepNumber = 'stepsix' and contains($docToCalcNodeName, 'linkedview'))">
				<div>
						<h3>
							<xsl:if test="($saveMethod = 'calcs'
									or $filexttype = 'temp'
									or contains($docToCalcNodeName, 'linkedview'))">
								Your calculations have been saved. The calculations can be viewed whenever
								this calculator addin is opened.
							</xsl:if>
						</h3>
				</div>
			</xsl:if>
      <br /><br />
		</div>
		<div id="divsteplast">
			<h4 class="ui-bar-b"><strong>Instructions (beta)</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
        <h4>Step 1</h4>
        <ul data-role="listview">
          <li>
            <strong>Step 1. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.
          </li>
          <li>
            <strong>Step 1. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylesheet name, relatedcalculatorstype ...)
          </li>
          <li>
            <strong>Step 1. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.
          </li>
          <li>
            <strong>Step 1. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.
          </li>
          <li>
            <strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. Note that when this
            input is added to a budget, the rates chosen in the budget calculator or analyzer can override this rate. In the USA, DevTreks recommends
            using Office of Management and Budget rates for the same year as the date of the input.
          </li>
        </ul>
        <ul data-role="listview">
          <li>
            <strong>Cost Perspective:</strong> Some economists believe that the 'ideal' entity completing the cost estimate is a 'perfect' insurance provider who acts as a 'perfect' agent for the recipient
            (who pays out all payments collected as benefits and is not motivated by making profits).
            In practice, the perspective (and price type below) can be chosen based on the requirements of the economic evaluation. Costs and Benefits should use the same perspective.
          </li>
          <li>
            <strong>Before Treatment Comparator:</strong> Many health care organizations recommend using 'routine care' as the 'before treatment' comparator.
            In practice, the comparator can be chosen based on the requirements of the economic evaluation. Please try to keep the comparators consistent within a network.
          </li>
        </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
        <li><strong>Step 2. Step 3 should only be completed for the primary inputs or services affecting your health care condition.</strong> </li>
				<li><strong>Step 2. Health Care Provider:</strong> Name of the individual or organization providing the health care input.</li>
        <li><strong>Step 2. Insurance Provider:</strong> Name of the company or organization providing health care insurance for the input.</li>
				<li><strong>Step 2. Package Type:</strong> Type of package received in insurance plan. Examples include high-family, low-self ...</li>
        <li><strong>Step 2. Postal Code:</strong> Zip, or postal, code of the health care provider.</li>
        <li><strong>Step 2. Is In Network Option:</strong> If false, adds together two costs to calculate Recipient Costs. The first cost is computed the same as the 'In Network' cost (see Example 1 or 2 below). The second cost is the difference between the adjusted price and the 'price type to use' price (see Example 3 below)</li>
        <li><strong>Step 2. Severity of Condition:</strong> The severity of the main health care condition resulting in the use of this input or service plays an important part in the cost of health care received. Choose one of the severity options. If needed, ask a medical professional for their opinion.</li>
        <li><strong>Step 2. Add Additional Prices to Input?:</strong> Yes means add both additional costs (price x amount) to the Recipient Cost (the Recipient Cost becomes the base input.OCPrice).</li>
				<li><strong>Step 2. Additional Prices:</strong> Unit costs for additional costs incurred in the use of the input such as productivity loss, informal care, travel expenses, new clothes, new furniture, etc.</li>
        <li><strong>Step 2. Additional Price Description:</strong> Explanation for how the additional prices and amounts were calculated.</li>
      </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
        <li><strong>Step 3. Ratings:</strong> Ratings can be 0 to 10, with decimals, such as 7.5 acceptable. Use a 0 rating when the rating is not known or not applicable.  </li>
				<li><strong>Step 3. Diagnosis Quality Rating:</strong> Rate how well the provider used this input or service to diagnose your heath care condition.  </li>
				<li><strong>Step 3. Treatment Quality Rating:</strong>  Rate how well the provider used this input or service to treat your heath care condition</li>
				<li><strong>Step 3. Treatment Benefit Rating:</strong> Rate how much this input or service contributed to the resolution of your health care condition</li>
        <li><strong>Step 3. Treatment Cost Rating:</strong> Rate whether or not the costs of this input or service justified the benefits.</li>
        <li><strong>Step 3. Knowledge Transfer Rating:</strong> Rate how much information you received about the need for this input or service and its efficacy. </li>
        <li><strong>Step 3. Constrained Choice Rating:</strong> Rate the degree to which you would have chosen to use this input, service, and/or provider, if no constraints were faced when making the decision. </li>
        <li><strong>Step 3. Insurance Coverage Rating:</strong> Requires dividing the percentage of the cost of this input paid by the insurance company by 10 (6.5 = 65% / 10). </li>
        <li><strong>Step 3. Will Do Survey:</strong> True means that you are willing to complete a more comprensive survey about the benefits and costs of health care. </li>
        <li><strong>Step 3. Input Quality Assessment:</strong> Provide an explanation for the ratings made above. </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 4</h4>
      <ul data-role="listview">
        <li><strong>Step 4. Price Type To Use:</strong> The price type can be chosen based on the requirements of the economic evaluation (see 'Cost Perspective', above) . 
          Efforts should be made to collect enough data to calculate sound recipient costs, insurance provider costs, and health care provider costs. Efforts should also be 
          made to ensure that all of the clubs found in a DevTreks network follow the same price collection strategy.
          </li>
        <li><strong>Step 4. Base Price:</strong> 'Base Price' is the health care input price used by health care providers as an initial, benchmark, price for calculating the full costs of health care inputs. 
          Many organizations use Medicare prices, or usual and customary rates (UCRs), as the base price. This calculator uses the input's CAP Price as the 'Base Price'. </li>
				<li><strong>Step 4. Contracted Price:</strong> The 'Contracted Price' is the health care input price negotiated by insurance companies, or governments, with health care providers. </li>
        <li><strong>Step 4. Base Price Adjustment and Adjusted Price:</strong> The 'Adjusted Price' is calculated by multiplying the 'Base Price' by the 'Base Price Adjustment'.
          The Base Price Adjustment usually ranges from 140 to 250 percent of the base price.
          Adjusted Prices are often used as 'Out of Network' price. Example: $1300 Adjusted Price = $1000 Base Price * 130% (Base Price Adjustment / 100). 
          This number is divided by 100 in the Adjusted Price calculation. The Adjusted Price derived using this multiplier is considered the full 'market' price for this input. </li>
        <li><strong>Step 4. List Price :</strong> The List Price is the Health Care Provider's full input price. </li>
        <li><strong>Step 4. Market Price:</strong> The Market Price is the Health Care Provider's discounted List price.  </li>
        <li><strong>Step 4. Production Cost Price:</strong> The Production Cost Price is the Health Care Provider's cost of production for this input. This price is seldom available (but can be 
          readily calculated using DevTreks) but closely matches the economics science definition of costs.</li>
        <li><strong>Step 4. Annual Premium Paid By Recipient:</strong> Annual amount of money spent on health care insurance by the health care recipient.</li>
        <li><strong>Step 4. Annual Premium Paid By Other:</strong> Annual amount of money spent on health care insurance on behalf of health care recipients by employers or governments. </li>
        <li><strong>Step 4. Annual Premiums Assigned to this Input:</strong> Amount of both annual premiums to assign to this specific input. Generally will be equal to what the insurance company pays, but the amount should not 
          exceed the summation of both premiums. The final recipient cost will include this amount. </li>
        <li><strong>Step 4. CoPayment Amounts:</strong> Lump sum payments made by health care recipients to receive this health care input. Deductibles should be included as copayments.</li>
        <li><strong>Step 4. CoPayment Rates:</strong> Payment rates, calculated as percentage of health care input costs, paid by health care recipients. For example, a value of 
          30 for an input with a price of $100 would mean that a $30 ($100 * 30%) payment was needed. Don't enter the character '%' and don't enter a number less than one unless the rate is really a fraction less than one.</li>
        <li><strong>Step 4. Incentive Amounts:</strong> Lump sum incentives, or subsidies, received by health care recipients to receive this health care input. For example, some low income recipients may receive subsidy payments to help pay the cost of the input.</li>
        <li><strong>Step 4. Incentive Rates:</strong> Subsidy rates, calculated as percentage of health care input costs, received by health care recipients to reduce the cost of this health care input. For example, some recipients may receive incentives based on an assesment of their risk in needing this input. 
          Don't enter the character '%' and don't enter a number less than one unless the rate is really a fraction less than one.</li>
        <li><strong>Step 4. Insurance Provider Price ProRates:</strong> When the insurance provider lumps inputs together when calculating their prices (i.e. a colonoscopy input service with a sedation iput service), each input's insurance company prices (copays, contracted price) should be prorated among the inputs. 
          The prorate formula could be based on the combined inputs' total costs (input1 prorate = inputs total cost / input1price). Alternatively, an operation health care cost calculator can be used instead of an input cost calculator to set the insurance company costs.</li>
        <li><strong>Step 4. Description:</strong> Explanation for the prices, copayments and incentives.</li>
			</ul>
      <ul data-role="listview">
        <li><strong>Step 4. Input Quality Rating: </strong> Sum of step 2's ratings multiplied by 10 and divided by the number of nonzero ratings. Ratings of 0 are left out of the calculation. The scientific significance of this 
          rating system has not been established yet, but recent research suggests that rating the quality of health care received, and health care management, plays an important role in achieving health care efficiency.</li>
        <li><strong>Step 4. Recipient Cost:</strong> Cost paid by the health care recipient for using this input. The calculation does not include incentives. This price is used to set the associated input.OCPrice. 
          The formula is: Recipient Cost = FeeToUse - InsuranceProviderCost - IncentivesCost + AssignedPremiumCost + (AdditionalCost, if useinanlaysis = true).
          Note that copays and incentives can be set to zero to cause this cost to be any of the price types. For example, a cost analysis 
          that preferred using cost of production prices would choose a 'Price To Use Type' of 'Production Cost' and set all of the copays 
          to zero. We recommend that economic analyses use uniform 'Price To Use Types' for all of the inputs used in an economic analysis. </li>
				<li><strong>Step 4. Incentives Cost:</strong> Incentives paid on behalf of the health care recipient, usually by governments, for using this input.</li>
        <li><strong>Step 4. Insurance Provider Cost:</strong> Cost paid by the insurance provider for the health care recipient's use of this input. 
          The formula is: InsuranceProviderCost = ContractedPrice - CoPay1Amount - CoPay2Amount- (ContractedPrice * CoPay1Rate) - (ContractedPrice * CoPay2Rate);</li>
        <li><strong>Step 4. Health Care Provider Cost:</strong> Cost paid to the health care provider for the health care recipient's use of this input. The 
          formula is: HealthCareProviderCost = InsuranceProviderCost + ReceiverCost - AssignedPremiumCost</li>
        <li><strong>Step 4. Additional Costs:</strong> Additional costs associated with the use of this input or service, often incurred by the health care recipient (i.e. productivity loss, informal care) .</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Examples</h4>
      <ul data-role="listview">
        <li><strong>Example 1. Typical Insurance Recipient Who Pays a CoPayment Amount In Network:</strong>
        Base Price (Medicare-derived price) = $1000; Price To Use = Contract Price; Contract Price = $1000; CoPayment Amount 1 = $100; Recipient Cost = $100; Insurance Provider Cost = $900 = $1000 (Contracted Price) - $100 (Recipient Cost).</li>
        <li><strong>Example 2. Typical Insurance Recipient Who Pays a CoPayment Rate In Network:</strong>
        Base Price (Medicare-derived price) = $1000; Price To Use = Contract Price; Contract Price = $1000; CoPayment Rate 1 = 10; Recipient Cost = $100 = $1000 (Contracted Price) * 10% (CoPayment Rate 1); Insurance Provider Cost = $900 = $1000 (Contracted Price) - $100 (Recipient Cost).</li>
         <li><strong>Example 3. Typical Insurance Recipient Who Pays for an Out Of Network Service at List Price:</strong>
        Price To Use = List Price ($1000); Contract Price = $700; CoPayment Amount 1 = $100; Insurance Provider Cost = $600 = $700 (Contracted Price) - $100 (CoPayment). Additional Recipient Cost = $300 = $1000 (List Price) - $700 (Contracted Price). Recipient Full Cost = $400 = $100 (CoPayment Amount 1) + $300 (Additional Recipient Cost)</li>
       </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>This calculator needs vetting in the field before it can be used to provide full decision support.</strong> </li>
        <li><strong>Access Economics. </strong>  An improved HTA economic evaluation framework for Australia.
          A (report for the) Medical Technology Association of Australia. May, 2009. </li>
        <li><strong>Ernst R. Berndt, David M. Cutler, Richard G. Frank, Zvi Griliches, Joseph P. Newhouse, and Jack E. Triplett.  </strong>
          Price Indexes for Medical Care Good and Services: An Overview of Measurement Issues. NBER Working Paper No. 6817. JEL No. 11, C8. November, 1998.</li>
          <li><strong>Alan M. Garber.</strong> Advances in Cost-Effectiveness Analysis of Health Interventions. Working Paper 7198. 
          National Bureau of Economic Research. June 1999.</li>
        <li><strong>New York Times.</strong> Insurers Alter Cost Formula. Patients Pay. April 24, 2012</li>
        <li><strong>National Institute for Health and Clinical Excellence (NICE, England). </strong> Guide to the methods
          of technology appraisal. June, 2008.  </li>
        <li><strong>National Institute for Health and Clinical Excellence (NICE, England).</strong> 
          Briefing papers for the update to the Methods Guide (2008 Technology Appraisals Methods Guide), January, 2012</li>
        <li><strong>Smith MW, Barnett PG, Phibbs CS, Wagner TH. </strong> 
          Microcost methods of determining VA (Veterans Administration) healthcare costs. Menlo Park, CA: Health Economics Resource Center. 2010.</li>
        <li><strong>Zsolt Magyorosy, Peter Smith.</strong>  The main methodological issues in costing medical health care services. 
          A literature review. Center for Health Economics, University of York, CHE Research Paper 7.  2005.</li>
      </ul>
      </div>
		</div>
	</xsl:if>
</xsl:template>
</xsl:stylesheet>

