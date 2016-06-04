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
      <h4 class="ui-bar-b"><strong>Operation and Component Calculation View</strong></h4>
      <xsl:choose>
        <xsl:when test="($docToCalcNodeName = 'operationgroup' 
							or $docToCalcNodeName = 'operation' 
							or $docToCalcNodeName = 'operationinput'
              or $docToCalcNodeName = 'componentgroup' 
							or $docToCalcNodeName = 'component' 
							or $docToCalcNodeName = 'componentinput'
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This operation and component calculator does not appear appropriate for the document being analyzed. Are you 
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
					This tool calculates net present value totals for operation and component uris. The discounted totals include operating, 
							allocated overhead, capital, and incentive-adjusted totals. Operations or components that have an effective life different than 1 period 
							include annual totals. Resource stock analyzers use the scheduling and timeliness parameters for labor and capital stock planning.
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
      <br />
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
		</div>
    <div id="divsteptwo">
			<h4 class="ui-bar-b"><strong>Optional Step 2 of 4. Scheduling and Selection</strong></h4>
			<xsl:variable name="calcParams3">'&amp;step=stepthree<xsl:value-of select="$calcParams"/>'</xsl:variable>
      <xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
          <h4 class="ui-bar-b"><strong>Success. Please review the calculations below.</strong></h4>
			</xsl:if>
      <div>
					<label for="lblPlannedStartDate" ><strong>Start Date</strong></label>
					<xsl:if test="($viewEditType != full)">
						<input type="text">
							<xsl:attribute name="value"><xsl:value-of select="@PlannedStartDate" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PlannedStartDate;datetime;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@PlannedStartDate" /></xsl:attribute>
						</input>
					</xsl:if>
			</div>
			<div>
				<label for="lblLaborAvailable" ><strong>Labor Available (hours per day)</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@LaborAvailable" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborAvailable;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@LaborAvailable" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
			<div>
				<label for="lblWorkdayProbability" ><strong>Workday Completion Probability</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@WorkdayProbability" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;WorkdayProbability;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@WorkdayProbability" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTimelinessPenalty1" ><strong>Timeliness Penalty Percent</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenalty1" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;TimelinessPenalty1;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenalty1" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTimelinessPenaltyDaysFromStart1" ><strong>Number of Days From Start for Timeliness Penalty</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenaltyDaysFromStart1" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;TimelinessPenaltyDaysFromStart1;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenaltyDaysFromStart1" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTimelinessPenalty2" ><strong>Additional Penalty Percent</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenalty2" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;TimelinessPenalty2;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenalty2" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTimelinessPenaltyDaysFromStart2" ><strong>Additional Number of Days From First Penalty</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenaltyDaysFromStart2" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;TimelinessPenaltyDaysFromStart2;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TimelinessPenaltyDaysFromStart2" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblWorkdaysLimit" ><strong>Total Number of Workdays Limit</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@WorkdaysLimit" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;WorkdaysLimit;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@WorkdaysLimit" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblOutputName" ><strong>Output Name</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@OutputName" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputName;string;75</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@OutputName" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblOutputUnit" ><strong>Output Unit</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@OutputUnit" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputUnit;string;25</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@OutputUnit" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblOutputPrice" ><strong>Output Price</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@OutputPrice" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputPrice;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@OutputPrice" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblOutputYield" ><strong>Output Yield</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@OutputYield" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputYield;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@OutputYield" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblCompositionUnit" ><strong>Composition Unit</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@CompositionUnit" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CompositionUnit;string;25</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@CompositionUnit" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblCompositionAmount" ><strong>Composition Amount</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@CompositionAmount" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CompositionAmount;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@CompositionAmount" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblOutputTimes" ><strong>Output Times</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text">
						<xsl:attribute name="value"><xsl:value-of select="@OutputTimes" /></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputTimes;double;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@OutputTimes" /></xsl:attribute>
					</input>
				</xsl:if>
			</div>
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
	    <xsl:if test="(($docToCalcNodeName != 'operation' and $docToCalcNodeName != 'component') or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				<xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
          @UseSameCalculator, @Overwrite)"/>
		  </xsl:if>
      <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, @RelatedCalculatorsType)"/>
      <xsl:value-of select="DisplayDevPacks:WriteAlternatives($searchurl, $viewEditType,
            @AlternativeType, @TargetType)"/>
      </div>
      <xsl:if test="(($lastStepNumber = 'stepfour') or (@FieldCapacity > 0))
            and ($docToCalcNodeName != 'operationgroup' and $docToCalcNodeName != 'componentgroup')">
        <div>
						<label for="lblOpCompAmount" ><strong>Operation or Component Amount</strong></label>: 
						<xsl:value-of select="@Amount" />
				</div>
        <div>
						<label for="lblOpCompUnit" ><strong>Operation or Component Unit</strong></label>: 
						<xsl:value-of select="@Unit" />
				</div>
        <div>
						<label for="lblOCAmount" ><strong>Field Capacity per hour)</strong></label>: 
						<xsl:value-of select="@FieldCapacity" />
				</div>
        <div>
						<label for="lblAreaCovered" ><strong>Area Covered Per Day</strong></label>: 
						<xsl:value-of select="@AreaCovered" />
				</div>
        <div>
						<label for="lblFieldDays" ><strong>Field Days Needed</strong></label>: 
						<xsl:value-of select="@FieldDays" />
				</div>
        <div>
						<label for="lblProbableFieldDays" ><strong>Probable Field Days Needed</strong></label>: 
						<xsl:value-of select="@ProbableFieldDays" />
				</div>
        <div>
						<label for="lblProbableFinishDate" ><strong>Probable Finish Date</strong></label>:
						<xsl:value-of select="@ProbableFinishDate" />
				</div>
        <div>
          <label for="lblTR" >
            <strong>Total Revenue</strong>
          </label>:
          <xsl:value-of select="@TR" />
        </div>
        <div>
						<label for="lblTimelinessPenaltyCost" ><strong>Timeliness Penalty Cost</strong></label>: 
						<xsl:value-of select="@TimelinessPenaltyCost" />
				</div>
        <div>
						<label for="lblTimelinessPenaltyCostPerHour" ><strong>Timeliness Penalty Cost Per Hour</strong></label>: 
						<xsl:value-of select="@TimelinessPenaltyCostPerHour" />
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
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. In the USA, DevTreks recommends 
					using Office of Management and Budget rates for the same year as the date of the input.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. Start Date:</strong> Earliest starting date of the operation or component. The date is adjusted in the timeliness calculations to the same date as the children machinery inputs (so be careful with input dates).</li>
				<li><strong>Step 2. Labor Available (hours per day):</strong> The total hours of labor available each day of the operation or component.</li>
				<li><strong>Step 2. Workday Probability (percent):</strong> The percent of days of the operation or component with conditions, such as weather, that allow work to be completed. This number will be divided by 100 in the calculation. (the ASABE reference includes representative workday probabilities)</li>
        <li><strong>Step 2. Timeliness Penalty (percent output loss per day):</strong> The percent of output loss per day if an operation or component is not completed on time. This number will be divided by 100 in the calculation. An additional penalty (plus or minus) can be added when losses change significantly for later operations or components. (the ASABE reference includes representative penalties)</li>
        <li><strong>Step 2. Timeliness Penalty Days From Start Date (number of days):</strong> The number of days (i.e. 35) from the Start Date of an operation or component when the timeliness penalty starts.</li>
        <li><strong>Step 2. Additional Timeliness Penalty and Additional Days:</strong> These two numbers will be added to their respective first penalties to derive an additional penalty. </li>
        <li><strong>Step 2. Timeliness Penalty Output Name:</strong> The name of the output to use for the timeliness penalty. Note that, when this operation/component is added to a budget, the budget's actual outputs will be substituted for these outputs. The substitution will either look for an output with the same name (i.e. corn) or choose the output with the highest revenue. Keep the output naming conventions simple.</li>
        <li><strong>Step 2. Timeliness Penalty Output Unit:</strong> The unit of the output to use for the timeliness penalty. Note that units must be consistent among the inputs, operations and components (i.e. metric, hours per acre).</li>
        <li><strong>Step 2. Timeliness Penalty Output Price Per Unit:</strong> The price per unit of the output to use for the timeliness penalty.</li>
        <li><strong>Step 2. Timeliness Penalty Output Yield:</strong> The yield of the output to use for the timeliness penalty.</li>
        <li><strong>Step 2. Timeliness Penalty Composition Amount and Unit:</strong> Composition of output, generally used for livestock (i.e. 100 head of cattle).</li>
        <li><strong>Step 2. Timeliness Penalty Output Times:</strong> Number of times this yield of output is harvested (can also be used as a general multiplier).</li>
			</ul>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
				<li><strong>Step 3. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 3. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 3. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 3. Operation or Component Timeliness Penalty, Operating, and Allocated Overhead Costs:</strong> These numbers do not appear when running calculations at the group level.
          The timeliness penalty is calculated using yield losses alone. The operating and allocated overhead cost calculations do not include the timeliness penalty. Add the two numbers together to get the full operation or component cost. 
          Further explanation of timeliness penalties can be found in the references below.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>References</h4>
        <ul data-role="listview">
          <li><strong>American Society of Agricultural and Biological Engineers, ASAE D497.7 MAR2011</strong> Agricultural Machinery Management Data</li>
          <li><strong>American Society of Agricultural and Biological Engineers, ASAE EP496.3 FEB2006 (R2011)</strong> Agricultural Machinery Management</li>
				  <li><strong>John Siemens, University of Illinois</strong> User Guide Farm Machinery and Selection Program, Version S10, 1988 ( http://devtreks.cloudapp.net/commontreks/preview/commons/resourcepack/DevTreks%20Machinery%20Costs/437/none ) (last accessed 2012-04-05</li>
				  <li><strong>William Edwards. Iowa State University</strong> Farm Machinery Selection, File A3-28, www.extension.iastate.edu/agdm (last accessed February, 2012)</li>
          <li><strong>Hallam, Eidman, Morehart and Klonsky (editors) </strong> Commodity Cost and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
			  </ul>
      </div>
		</div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
