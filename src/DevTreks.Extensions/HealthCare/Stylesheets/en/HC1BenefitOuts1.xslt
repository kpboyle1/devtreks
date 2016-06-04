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
       <xsl:when test="($docToCalcNodeName = 'outputgroup' 
							    or $docToCalcNodeName = 'output' 
							    or $docToCalcNodeName = 'outputseries'
							    or contains($docToCalcNodeName, 'devpack') 
							    or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
					sure this is the right calculator?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>Health Care Benefit Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This calculator calculates the benefits of health care outputs. 
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
	    <xsl:if test="($docToCalcNodeName != 'outputseries' or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
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
					<label for="lblAge"><strong>How old are you now? Use years.</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAge">
							<xsl:attribute name="value"><xsl:value-of select="@Age"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAge">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Age;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@Age"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
					<label for="lblLocationId"><strong>What is your postal code?</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblLocationId">
							<xsl:attribute name="value"><xsl:value-of select="@LocationId"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblLocationId">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;LocationId;string;15</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@LocationId"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
					<label for="lblGender"><strong>Are you male or female?</strong></label>
					<select class="Select225" id="lblGender" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Gender;string;50</xsl:attribute>
						</xsl:if>
            <option>
							<xsl:attribute name="value">female</xsl:attribute>
							<xsl:if test="(@Gender = 'female')">
								<xsl:attribute name="selected" />
							</xsl:if>Female
						</option>
            <option>
							<xsl:attribute name="value">male</xsl:attribute>
							<xsl:if test="(@Gender = 'male')">
								<xsl:attribute name="selected" />
							</xsl:if>Male
						</option>
					</select>
			</div>
      <div>
					<label for="lblEducationYears"><strong>How many years in all did you spend studying in school,
            college or university?</strong></label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblEducationYears">
							<xsl:attribute name="value"><xsl:value-of select="@EducationYears"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblEducationYears">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;EducationYears;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@EducationYears"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
					<label for="lblHousing"><strong>What type of housing are you living in?</strong></label>
					<select class="Select225" id="lblHousing" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Housing;string;25</xsl:attribute>
						</xsl:if>
            <option>
							<xsl:attribute name="value">independentliving</xsl:attribute>
							<xsl:if test="(@Housing = 'independentliving')">
								<xsl:attribute name="selected" />
							</xsl:if>Independent Living
						</option>
            <option>
							<xsl:attribute name="value">assistedlivingfacility</xsl:attribute>
							<xsl:if test="(@Housing = 'assistedlivingfacility')">
								<xsl:attribute name="selected" />
							</xsl:if>Assisted Living Facility
						</option>
						<option>
							<xsl:attribute name="value">hospitalized</xsl:attribute>
							<xsl:if test="(@Housing = 'hospitalized')">
								<xsl:attribute name="selected" />
							</xsl:if>Hospitalized
						</option>
					</select>
			</div>
      <div>
					<label for="lblMaritalStatus" ><strong>What is your current marital status?</strong></label>
					<select class="Select225" id="lblMaritalStatus" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MaritalStatus;string;25</xsl:attribute>
						</xsl:if>
            <option>
							<xsl:attribute name="value">nevermarried</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'nevermarried')">
								<xsl:attribute name="selected" />
							</xsl:if>Never Married
						</option>
            <option>
							<xsl:attribute name="value">currentlymarried</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'currentlymarried')">
								<xsl:attribute name="selected" />
							</xsl:if>Currently Married
						</option>
						<option>
							<xsl:attribute name="value">separated</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'separated')">
								<xsl:attribute name="selected" />
							</xsl:if>Separated
						</option>
            <option>
							<xsl:attribute name="value">divorced</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'divorced')">
								<xsl:attribute name="selected" />
							</xsl:if>Divorced
						</option>
            <option>
							<xsl:attribute name="value">widowed</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'widowed')">
								<xsl:attribute name="selected" />
							</xsl:if>Widowed
						</option>
            <option>
							<xsl:attribute name="value">cohabiting</xsl:attribute>
							<xsl:if test="(@MaritalStatus = 'cohabiting')">
								<xsl:attribute name="selected" />
							</xsl:if>Cohabiting
						</option>
					</select>
      </div>
      <div>
				  <label for="lblRace" ><strong>What is your predominant race?</strong></label>
          <select class="Select225" id="lblRace" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Race;string;25</xsl:attribute>
            </xsl:if>
						<option>
							<xsl:attribute name="value">indigenousornative</xsl:attribute>
							<xsl:if test="(@Race = 'indigenousornative')">
								<xsl:attribute name="selected" />
							</xsl:if>Indigenous Or Native (i.e. American Indian)
						</option>
            <option>
							<xsl:attribute name="value">asian</xsl:attribute>
							<xsl:if test="(@Race = 'asian')">
								<xsl:attribute name="selected" />
							</xsl:if>Asian
						</option>
						<option>
							<xsl:attribute name="value">blackorafrican</xsl:attribute>
							<xsl:if test="(@Race = 'blackorafrican')">
								<xsl:attribute name="selected" />
							</xsl:if>Black or African
						</option>
            <option>
							<xsl:attribute name="value">hispanicorlatino</xsl:attribute>
							<xsl:if test="(@Race = 'hispanicorlatino')">
								<xsl:attribute name="selected" />
							</xsl:if>Hispanic Or Latino
						</option>
            <option>
							<xsl:attribute name="value">white</xsl:attribute>
							<xsl:if test="(@Race = 'white')">
								<xsl:attribute name="selected" />
							</xsl:if>White
						</option>
            <option>
							<xsl:attribute name="value">mixed</xsl:attribute>
							<xsl:if test="(@Race = 'mixed')">
								<xsl:attribute name="selected" />
							</xsl:if>Mixed (not more than 50% of any of the above)
						</option>
					</select>
      </div>
      <div>
				  <label for="lblWorkStatus" ><strong>Which describes your main work status best?</strong></label>
					<select class="Select225" id="lblWorkStatus" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;WorkStatus;string;25</xsl:attribute>
						</xsl:if>
              <option>
							<xsl:attribute name="value">paidwork</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'paidwork')">
								<xsl:attribute name="selected" />
							</xsl:if>Paid Work
						</option>
            <option>
							<xsl:attribute name="value">selfemployed</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'selfemployed')">
								<xsl:attribute name="selected" />
							</xsl:if>Self Employed
						</option>
						<option>
							<xsl:attribute name="value">nonpaidwork</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'nonpaidwork')">
								<xsl:attribute name="selected" />
							</xsl:if>Nonpaid Work (volunteer)
						</option>
            <option>
							<xsl:attribute name="value">student</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'student')">
								<xsl:attribute name="selected" />
							</xsl:if>Student
						</option>
            <option>
							<xsl:attribute name="value">homemaker</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'homemaker')">
								<xsl:attribute name="selected" />
							</xsl:if>Homemaker
						</option>
            <option>
							<xsl:attribute name="value">retired</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'retired')">
								<xsl:attribute name="selected" />
							</xsl:if>Retired
						</option>
						<option>
							<xsl:attribute name="value">unemployedforhealth</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'unemployedforhealth')">
								<xsl:attribute name="selected" />
							</xsl:if>Unemployed For Health Reasons
						</option>
            <option>
							<xsl:attribute name="value">unemployedforother</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'unemployedforother')">
								<xsl:attribute name="selected" />
							</xsl:if>Unemployed For Other Reasons
						</option>
            <option>
							<xsl:attribute name="value">other</xsl:attribute>
							<xsl:if test="(@WorkStatus = 'other')">
								<xsl:attribute name="selected" />
							</xsl:if>Other
						</option>
					</select>
      </div>
      <h4 class="ui-bar-b"><strong>Optional Cost Benefit Monetary Assessment</strong></h4>
			<div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblOutputCost" ><strong>Output Cost. </strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblOutputCost">
								<xsl:attribute name="value"><xsl:value-of select="@OutputCost" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblOutputCost">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputCost;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OutputCost" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				<div class="ui-block-b">
					<label for="lblBenefitAdjustment" ><strong>Benefit Adj. </strong> </label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblBenefitAdjustment">
							<xsl:attribute name="value"><xsl:value-of select="@BenefitAdjustment" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblBenefitAdjustment">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;BenefitAdjustment;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@BenefitAdjustment" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</div>
      <h4 class="ui-bar-b"><strong>Optional Cost Effectiveness Outcome Measure (Output Effects)</strong></h4>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblOutputEffect1Name" ><strong>Out. Effect Name</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblOutputEffect1Name">
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Name" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblOutputEffect1Name">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputEffect1Name;string;75</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Name" /></xsl:attribute>
							</input>
						</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblOutputEffect1Unit" ><strong>Effect Unit</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblOutputEffect1Unit">
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Unit" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblOutputEffect1Unit">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputEffect1Unit;string;25</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Unit" /></xsl:attribute>
							</input>
						</xsl:if>
				</div>
			</div>
      <div class="ui-grid-a">
				<div class="ui-block-a">
					<label for="lblOutputEffect1Amount" ><strong>Effect Amount</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblOutputEffect1Amount">
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Amount" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblOutputEffect1Amount">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputEffect1Amount;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Amount" /></xsl:attribute>
							</input>
						</xsl:if>
				</div>
				<div class="ui-block-b">
					<label for="lblOutputEffect1Price" ><strong>Effect Price</strong></label>
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblOutputEffect1Price">
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Price" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblOutputEffect1Price">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputEffect1Price;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@OutputEffect1Price" /></xsl:attribute>
							</input>
						</xsl:if>
				</div>
			</div>
		</div>
		<div id="divstepthree">
      <h4 class="ui-bar-b"><strong>Step 3 of 5. Health Improvement Rating</strong></h4>
			<xsl:variable name="calcParams3">'&amp;step=stepfour<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepfour')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>Quality of of Health Ratings</strong></h4>
      <p><strong>Only complete this step for the primary outputs or outcomes affecting your health care condition.  </strong></p>
      <ul data-role="listview" data-theme="a">
        <li data-role="list-divider">
					<strong>Using the following scale of -100 to 100, and the relative degree of changes, rate how each of the five dimension of health listed below got better or worse AS A RESULT OF
          the health care treatment you received. Ratings that are considered not important, not known, or not applicable, should be rated 0.
          Use whole numbers such as -55 or 25.</strong>
         </li>
          <li>
            -100 = this dimension of health quality got completely worse
          </li>
          <li>
            -75 = this dimension of health quality got a lot worse
          </li>
          <li>
            -50 = this dimension of health quality got moderately worse
          </li>
          <li>
            -25 = this dimension of health quality got a little worse
          </li>
          <li>
            0 = do not include this dimension of health quality in the rating
          </li>
          <li>
            25 = this dimension of health quality got a little better
          </li>
          <li>
            50 = this dimension of health quality got moderately better
          </li>
          <li>
            75 = this dimension of health quality got a lot better
          </li>
          <li>
            100 = this dimension of health quality got completely better
          </li>
			</ul>
      </div>
      <br />
      <div>
					<label for="lblPhysicalHealthRating"><strong>Physical Health Rating.</strong> Rate how your physical health changed as a result of the health care you received for this health outcome.</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblPhysicalHealthRating">
							<xsl:attribute name="value"><xsl:value-of select="@PhysicalHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblPhysicalHealthRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;PhysicalHealthRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@PhysicalHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <br />
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4><strong>Physical Health.</strong></h4>
      <ul data-role="listview">
         <li data-role="list-divider">This rating includes:</li>
        <li>
          the degree and frequency of pain and discomfort; 
        </li>
        <li>
          the performance of daily living activities, such as dressing, washing, shopping, cooking, and driving;
        </li>
        <li>
          the ability to walk around, exercise, and move about;
        </li>
        <li>
          the ability to see, hear, talk, and communicate;
        </li>
        <li>
          the ability to relax and sleep.
        </li>
      </ul>
      </div>
      <br />
      <div>
					<label for="lblEmotionalHealthRating"><strong>Emotional Health Rating.</strong> Rate how your emotional health changed as a result of the health care you received for this health outcome.</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblEmotionalHealthRating">
							<xsl:attribute name="value"><xsl:value-of select="@EmotionalHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblEmotionalHealthRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;EmotionalHealthRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@EmotionalHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d">
    <h4><strong>Emotional Health.</strong></h4>
    <ul data-role="listview">
       <li data-role="list-divider">This rating includes:</li>
      <li>
      the degree and frequency of anxiety, sadness, depression, and anger; 
      </li>
      <li>
      the degree of happiness and peace of mind;
      </li>
      <li>
      the ability to cope with life;
      </li>
      <li>
      the level of energy;
      </li>
      <li>
      the degree of spirituality.
      </li>
	  </ul>
    </div>
      <br />
      <div>
					<label for="lblSocialHealthRating"><strong>Social Health Rating.</strong> Rate how your social health changed as a result of the health care you received for this health outcome.</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblSocialHealthRating">
							<xsl:attribute name="value"><xsl:value-of select="@SocialHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblSocialHealthRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SocialHealthRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@SocialHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d">
    <h4><strong>Social Health.</strong></h4>
    <ul data-role="listview">
      <li data-role="list-divider">This rating includes:</li>
      <li>
      the way relations with relatives and friends changed; 
      </li>
      <li>
      the way burdens on persons who helped with this health care condition changed 
        (i.e. did caregivers have to spend less time taking care of you?);
      </li>
      <li>
      the degree of participation in social and community activities;
      </li>
      <li>
      the degree to which intimate relationships, including sexual relations, got better or worse;
      </li>
      <li>
      the way memory and the ability to learn new things changed.
      </li>
    </ul>
    </div>
      <br />
      <div>
					<label for="lblEconomicHealthRating"><strong>Economic Health Rating.</strong> Rate how your economic health changed as a result of the health care you received for this health outcome.</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblEconomicHealthRating">
							<xsl:attribute name="value"><xsl:value-of select="@EconomicHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblEconomicHealthRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;EconomicHealthRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@EconomicHealthRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <br />
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4><strong>Economic Health.</strong></h4>
      <ul data-role="listview">
         <li data-role="list-divider">This rating includes:</li>
        <li>
          the degree of participation in leisure actities (such as golf or travel); 
        </li>
        <li>
          the ability to engage in productive work (go back to a job or work as a volunteer);
        </li>
        <li>
          the way income and wealth changed (amount paid out of pocket for the health care, reductions in savings accounts);
        </li>
        <li>
          the amount of time saved (i.e. visiting offices, travelling for health care);
        </li>
        <li>
          the amount of productive knowledge gained (about medicines to take, life style changes needed, and other health-related knowledge).
        </li>
	    </ul>
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
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>Quality of of Health Care Delivery Ratings.</strong></h4>
      <ul data-role="listview" data-theme="a">
        <li data-role="list-divider">
					<strong>Using the following scale of -100 to 100, and the relative degree of changes, rate the quality of the health care you received DURING THE TREATMENT that resulted in
          this health care outcome. Ratings that are considered not important, not known, or not applicable, should be rated 0.
          Use whole numbers such as -55 or 25.</strong>
         </li>
      <li>
        -100 = this dimension of health care delivery was absolutely terrible
      </li>
      <li>
        -75 = this dimension of health care delivery was very bad
      </li>
      <li>
        -50 = this dimension of health care delivery was moderately bad
      </li>
      <li>
        -25 = this dimension of health care delivery was a little bad
      </li>
      <li>
        0 = do not include this dimension of health care delivery in the rating
      </li>
      <li>
        25 = this dimension of health care delivery was a little good
      </li>
      <li>
        50 = this dimension of health care delivery was moderately good
      </li>
      <li>
        75 = this dimension of health care delivery was very good
      </li>
      <li>
        100 = this dimension of health care delivery was absolutely excellent
      </li>
		</ul>
    </div>
      <br />
      <div>
					<label for="lblHealthCareDeliveryRating"><strong>Health Care Delivery Rating.</strong> 
            Rate the quality of the health care you received for this health outcome.</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblHealthCareDeliveryRating">
							<xsl:attribute name="value"><xsl:value-of select="@HealthCareDeliveryRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblHealthCareDeliveryRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;HealthCareDeliveryRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@HealthCareDeliveryRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4><strong>Health Care Delivery.</strong></h4>
      <ul data-role="listview">
        <li data-role="list-divider">This rating includes:</li>
        <li>
          the satisfaction with the health care treatment received; 
        </li>
        <li>
          the appropriateness of the health care treatment received;
        </li>
        <li>
          the competency of the health care providers who delivered this health care treatment;
        </li>
        <li>
          the accessability of the health care providers who delivered this health care treatment;
        </li>
        <li>
          the timeliness of the health care treatment;
         </li>
        <li>
          the fairness in the way the health care recipient was treated.
        </li>
			</ul>
      </div>
      <br />
      <div>
				<label for="lblBCAssessment"><strong>Benefit Assessment</strong> (explain your ratings)</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<textarea class="Text200H100PCW" id="lblBCAssessment">
						<xsl:value-of select="@BenefitAssessment" />
					</textarea>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<textarea class="Text200H100PCW" id="lblBCAssessment">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;BenefitAssessment;string;500</xsl:attribute>
						<xsl:value-of select="@BenefitAssessment" />
					</textarea>
				</xsl:if>
			</div>
		</div>
		<div id="divstepfour">
      <h4 class="ui-bar-b"><strong>Step 4 of 5. Calculate:</strong></h4>
			<xsl:variable name="calcParams4">'&amp;step=stepfive<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
						<xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams4)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepfive')">
        <h4 class="ui-bar-b"><strong>Success. Please review the calculations below</strong></h4>
			</xsl:if>
				<xsl:if test="(@AdjustedBenefit >= 0)">
          <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d">
          <h4 class="ui-bar-b"><strong>Health Quality Benefits</strong></h4>
          <div class="ui-grid-a">
            <div class="ui-block-a">
							<strong>After Treatment QALY</strong>: 
							<xsl:value-of select="@QALY" />
						</div>
						<div class="ui-block-b">
							<strong>Incremental QALY (After - Before)</strong>: 
							<xsl:value-of select="@ICERQALY" />
						</div>
					</div>
          <div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>TTO Quality Adjusted Life Years</strong>: 
							<xsl:value-of select="@TTOQALY" />
						</div>
						<div class="ui-block-b">
							<strong>Adjusted Benefits (=output.Price)</strong>
						</div>
						<div class="ui-block-a">
							<xsl:value-of select="@AdjustedBenefit" />
						</div>
					</div>
					<div class="ui-grid-a">
						<div class="ui-block-a">
							<strong>Average Health Quality Rating</strong>: 
							<xsl:value-of select="@AverageBenefitRating" />
						</div>
						<div class="ui-block-b">
							<strong>Treatment Effect Amount (<xsl:value-of select="@OutputEffect1Name" />)</strong>: 
							<xsl:value-of select="@OutputEffect1Amount" />
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
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>2. 'Before Treatment' Health-Related Quality of Life Ratings.</strong></h4>
      <ul data-role="listview" data-theme="a">
        <li data-role="list-divider">
					<strong>Given the changes in the five dimensions of health quality completed in the last step,  
          use the following scale to rate the state of your overall health BEFORE you received this treatment for this health outcome. 
          Use whole numbers such as -20 or 45.
          </strong>
				</li>
        <li>
        -25 = this health condition caused the overall quality of my health to be worse than death
        </li>
        <li>
        0 = this health condition caused the overall quality of my health to be equivalent to death (or death actually occurred)
        </li>
        <li>
        25 = this health condition caused the overall quality of my health to be poor
        </li>
        <li>
        50 = this health condition caused the overall quality of my health to be good
        </li>
        <li>
        75 = this health condition caused the overall quality of my health to be excellent
        </li>
        <li>
        100 = this health condition caused the overall quality of my health to be perfect
      </li>
			</ul>
      </div>
      <br />
      <div>
					<label for="lblBeforeQOLRating"><strong>2. 'Before Treatment' Health-Related Quality of Life Rating.</strong> </label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblBeforeQOLRating">
							<xsl:attribute name="value"><xsl:value-of select="@BeforeQOLRating"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblBeforeQOLRating">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;BeforeQOLRating;integer;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@BeforeQOLRating"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>2. 'After Treatment' Health-Related Quality of Life Ratings.</strong></h4>
      <ul data-role="listview" data-theme="a">
        <li data-role="list-divider">
					<strong>Given the changes in the five dimensions of health quality completed in the last step,  
          use the following scale to rate the state of your overall health AFTER you received this treatment for this health outcome.
          </strong>
				</li>
        <li>
        -25 = this health outcome caused the overall quality of my health to be worse than death
        </li>
        <li>
        0 = this health outcome caused the overall quality of my health to be equivalent to death (or death actually occurred)
        </li>
        <li>
        25 = this health outcome caused the overall quality of my health to be poor
        </li>
        <li>
        50 = this health outcome caused the overall quality of my health to be good
        </li>
        <li>
        75 = this health outcome caused the overall quality of my health to be excellent
        </li>
        <li>
        100 = this health outcome caused the overall quality of my health to be perfect
      </li>
			</ul>
      </div>
      <br />
      <div>
				<label for="lblAfterQOLRating"><strong>3. 'After Treatment' Health-Related Quality of Life Rating.</strong> 
          Use whole negative or positive numbers such as -5 or 55.</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblAfterQOLRating">
						<xsl:attribute name="value"><xsl:value-of select="@AfterQOLRating"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblAfterQOLRating">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;AfterQOLRating;integer;4</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@AfterQOLRating"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
					<label for="lblBeforeYears"><strong>4. Number of Years Duration for 'Before Treatment'</strong> 
          How many years, using decimals, were you going to live in the 'before' health quality state?</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblBeforeYears">
							<xsl:attribute name="value"><xsl:value-of select="@BeforeYears"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblBeforeYears">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;BeforeYears;decimal;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@BeforeYears"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
					<label for="lblAfterYears"><strong>5. Number of Years Duration for 'After Treatment'</strong> 
          How many years, using decimals, were you going to live in the 'after' health quality state?</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblAfterYears">
							<xsl:attribute name="value"><xsl:value-of select="@AfterYears"/></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblAfterYears">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;AfterYears;decimal;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@AfterYears"/></xsl:attribute>
						</input>
					</xsl:if>
			</div>
      <div>
				<label for="lblAfterYearsProb"><strong>6. Probability of Number of Years Duration for 'After Treatment'</strong> 
        What is the probability, using whole numbers between 1 and 100, of living in the 'after' health quality state for the full number of 'after treatment' years?</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblAfterYearsProb">
						<xsl:attribute name="value"><xsl:value-of select="@AfterYearsProb"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblAfterYearsProb">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;AfterYearsProb;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@AfterYearsProb"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblTimeTradeoffYears"><strong>7. Time Tradeoff Years (optional)</strong> 
        How many years in 'after treatment' health years is equally attractive to living in your full number of 'before treatment' health years.</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblTimeTradeoffYears">
						<xsl:attribute name="value"><xsl:value-of select="@TimeTradeoffYears"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblTimeTradeoffYears">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TimeTradeoffYears;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@TimeTradeoffYears"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblEquityMultiplier"><strong>8. Equity Multiplier. (optional)</strong>
        Enter a multiplier to adjust the overall quality of health 
        rating based on an equity consideration, such as age. The multiplier should be a whole number between -100 and 100 and will be 
        divided by 100 to derive a percentage multiplier for the final calculation.</label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<input type="text" id="lblEquityMultiplier">
						<xsl:attribute name="value"><xsl:value-of select="@EquityMultiplier"/></xsl:attribute>
					</input>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<input type="text" id="lblEquityMultiplier">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;EquityMultiplier;decimal;8</xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select="@EquityMultiplier"/></xsl:attribute>
					</input>
				</xsl:if>
			</div>
      <div>
				<label for="lblDescription"><strong>Description</strong></label>
				<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
					<textarea class="Text75H100PCW" id="lblDescription">
						<xsl:value-of select="@CalculatorDescription" />
					</textarea>
				</xsl:if>
				<xsl:if test="($viewEditType = 'full')">
					<textarea class="Text75H100PCW" id="lblDescription">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorDescription;string;255</xsl:attribute>
						<xsl:value-of select="@CalculatorDescription" />
					</textarea>
				</xsl:if>
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
				<li><strong>Step 1. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
        <li><strong>Step 1. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylesheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 1. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 1. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. The QALY will be discounted by the real rate. Note that when this 
          output is added to a budget, the rates chosen in the budget calculator or analyzer can override this rate.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
      <ul data-role="listview">
				<li><strong>Step 2. Demographics:</strong> Fill in the information about the health care recipient.</li>
        <li><strong>Step 2. Output Cost (optional):</strong> Total cost of the inputs used to produce this outcome. Should be taken from a related DevTreks cost, or budget, calculator (using the cost perspective used in the calculations).</li>
				<li><strong>Step 2. Benefit Adjustment:</strong> How much you are willing to pay to cover #7. Output Cost. Express the value as 
          a percent of the total costs (i.e. 50, 200). Use only whole numbers with no '%' sign. This number is divided by 100 and multiplied by the Output Cost to derive the Adjusted Benefit calculation. 
          It provides a quantitative assessment of benefits and can include indirect, or related, benefits. Documentation should be included in the Description.</li>
        <li><strong>Step 2. Output Effects (optional):</strong> Supports cost effectiveness analysis by allowing costs to be divided by this output effect, 
          such as 'strokes averted', 'infections reduced', or 'deaths avoided', per dollar cost.</li>
      </ul>
			<ul data-role="listview">
          <li><strong>Benefit Perspective:</strong> Some economists believe that the 'ideal' entity completing the benefit estimate is a 'perfect' insurance provider who acts as a 'perfect' agent for the recipient 
          (who pays out all payments collected as benefits and is not motivated by making profits). 
          In practice, the perspective can be chosen based on the requirements of the economic evaluation. Costs and Benefits should use the same perspective.</li>
        <li><strong>Before Treatment Comparator:</strong> Many health care organizations recommend using 'routine care' as the 'before treatment' comparator. 
          In practice, the comparator can be chosen based on the requirements of the economic evaluation. Please try to keep the comparators consistent within a network.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
      <ul data-role="listview">
        <li><strong>Step 3. Ratings:</strong> Health care economists (Sinnott, 2007) note that new outcome-based (or specific disease) HRQol instruments with new scoring systems may be appropriate, and provide better results than more generic instruments,
          when health care preferences are based on specific inputs and outcomes (i.e. on specific diseases). 
          Therefore, this new rating system was developed as one potential instrument. Ratings can be -100 to 100, with only whole numbers acceptable. Ratings that are considered not important, not known, or not applicable, should be rated 0.  
          Efforts should be made to ensure that factors considered as costs (i.e. lost productivity, time savings) should be specifically considered for benefits as well (i.e. considered in the Economic Health Dimension). 
          Other HRQol instruments have demonstrated that instruments with fewer factors can be largely as effective as instruments with a large number of factors. 
          Some governments (England, Ireland) recommend that the 'before treatment' condition (or comparator) should be 'routine care'. </li>
				<li><strong>Step 3. Physical Health Rating:</strong> Rates the degree to which the health care recipient's physical health changed as a result of the health care inputs (treatment) used with this health care outcome.  </li>
				<li><strong>Step 3. Emotional Health Rating:</strong>  Rates the degree to which the health care recipient's emotional health changed as a result of the health care inputs (treatment) used with this health care outcome.  </li>
				<li><strong>Step 3. Social Health Rating:</strong> Rates the degree to which the health care recipient's social health changed as a result of the health care inputs (treatment) used with this health care outcome.  </li>
        <li><strong>Step 3. Economic Health Rating:</strong> Rates the degree to which the health care recipient's economic health changed as a result of the health care inputs (treatment) used with this health care outcome.  </li>
        <li><strong>Step 3. Health Care Delivery Rating:</strong> Rates the performance of the health care delivery system used to treat the recipient's health condition and that resulted in this specific outcome (see Access Economics, 2009 and Chandra, 2011).  </li>
        <li><strong>Step 3. Will Do Survey:</strong> True means that you are willing to complete a more comprensive survey about the benefits and costs of health care. A logical followup survey would include the individual factors within each rating.</li>
        <li><strong>Step 3. Benefit Assessment:</strong> Provide an explanation for the ratings made above. </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 4</h4>
      <ul data-role="listview">
         <li><strong>Step 4. Before and After Health-Related Quality of Life Ratings:</strong> Rates the 'before and after treatment' health-related quality of life state of the health care recipient. 
          This is the only factor used in the final calculated QALY and allows the previous five health quality dimensions to be weighted differently. The whole number rating system means that 126 health states are possible (-25 to 100). Each health state 
          is a self-weighted blend of the five health care dimensions rated in the previous step. The rating is divided by 100 to derive the final QALY weight and to adapt to the convention of using 0 to 1 health status states.</li>
        <li><strong>Step 4. Before Treatment Health-Related Quality of Life Ratings:</strong>  Please consider all of the five health quality dimensions 
            included in the previous step.  You can place greater importance on some dimensions over other ones. 
            See the 'Before Treatment' Comparator section in the Instructions. </li>
        <li><strong>Step 4. After Treatment Health-Related Quality of Life Ratings:</strong> The simplest way to come up with an 'after' rating is to 
            add the average points of the combined five dimension to the 'before' rating.  However, you may want to place greater 
            importance on some dimensions over other ones. For example, if the average change in the five dimension is +25, 
            but you place greater weight on a dimension that is higher than 25, you might want to add more than 25 points 
            to the 'before' rating to derive the 'after' rating. You can increase or decrease the rating because of positive 
            or negative interrelationships among the five dimension. </li>
        <li><strong>Step 4. Preference-based health status weights for populations :</strong> Many health economists (Sinnott, 2007) 
          recommend that population utility values be used as preference, or QALY, weights and that these values be drawn from representative population samples. 
          Some economists allow that, when this was not possible, the values of individual patients, like this rating system, may be used. 
          Like everything else in DevTreks, the instrument is new and hasn't been tested to meet the validity, feasibility, reliability, and responsiveness criteria needed by HRQol instruments. 
          </li>
        <li><strong>Step 4. Duration in Before Condition:</strong> Enter the number of years you expected to live with your health condition if this health care outcome was not achieved. 
          Use decimals to deal with periods less than one year (i.e. 3 months = .25 years). You should obtain this information from a health 
          care professional or reference.</li>
        <li><strong>Step 4. Duration in After Condition:</strong> Enter the number of years you expect to live with this health care outcome (in the 'after treatment' state). </li>
        <li><strong>Step 4. Probability of Duration in After Condition:</strong> Enter the probability of living with this health care outcome. If the probability is less than 
          100%, the remainder of the time will be calculated using the 'before treatment' health state. If the probability of living
          for the full number 'after treatment' years is 90%, enter 90. You should obtain this information from a health 
          care professional or reference.</li>
        <li><strong>Step 4. Time Tradeoff (optional):</strong> This question is based on the assumption that you would 
            be willing to live fewer years in the 'after treatment' state of health in order not to have to live more years 
            in the 'before treatment' state of health. For example, some people may prefer to live 10 disease-free years rather than 
            17 years with a disease (in this case the Time Tradeoff Years = 10, 10 disease-free years means the same to you as 17 disease-laden years). 
            The question is not asking you about the scientific probablilty of these years, it is strictly asking about your preference for living 'illness-free' versus 'illness-borne'.
            The formula used to derive a QALY weight = time tradeoff years / 'before treatment' years, scaled 0-1 . Optional because some recipients are not willing to give up any time (i.e. handicapped persons).</li>
        <li><strong>Step 4. Equity Multiplier (optional):</strong> Existing quality of life ratings have been criticized for not including any 
            consideration for equity (i.e. every QALY is equal to every other QALY). The result is that children's QALY's are identical to elderly persons even 
            though some people consider increases in children's health to have greater overall benefits. This multplier allows an equity adjustment to be included in the 
            final QALY. The multiplier should come from a formal, fully documented, and accepted, QALY-equity multiplier system.</li>
        <li><strong>Step 4. Description:</strong> Explanation for the information entered in this step.</li>
			</ul>
      <ul data-role="listview">
        <li><strong>Step 4. Average Benefit Rating:</strong> Sum of step 3's ratings divided by the number of nonzero ratings. Ratings of 0 are left out of the calculation.  </li>
        <li><strong>Step 4. QALY: </strong> The formula used to determine the initial QALY is: QALY = (AfterQOLRating / 100) * AfterYears * DiscountFactor(realrate, afteryears). 
          The next calculation is: QALY = (QALY * (AfterYearsProb / 100)) + (BeforeQALY * ((100 - AfterYearsProb) / 100)). 
          The final calculation is QALY = QALY * (EquityMultiplier / 100)</li>
        <li><strong>Step 4. Time Tradeoff QALY:</strong> The formula used to determine this QALY is: TTOQALY = (TimeTradeoffYears / BeforeYears) * AfterYears * DiscountFactor(realrate, afteryears). 
          The final calculation is TTOQALY = TTOQALY * (EquityMultiplier / 100)</li>
        <li><strong>Step 4. Adjusted Benefit:</strong> Example: $110 = $100 (inputs costs) * (110% (Benefit Adjustment) / 100) </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">  
        <li><strong>This calculator needs vetting in the field before it can be fully used to provide health care decision support.</strong> </li>
        <li><strong>Access Economics. </strong>  An improved HTA economic evaluation framework for Australia. 
          A (report for the) Medical Technology Association of Australia. May, 2009. </li>
          <li><strong>J. Brazier, M. Deverill, C. Green, R. Harper A. Booth.</strong> A review of the use of health status
          measures in economic evaluation. Health Technology Assessment 1999; Vol. 3: No. 9</li>
        <li><strong>Amitabh Chandra, Anupam B. Jena, Jonathan S. Skinner.</strong> The Pragmatist’s Guide to Comparative Effectiveness Research. 
          NBER Working Paper No. 16990, April 2011, JEL No. H51,I1 </li>
        <li><strong>Alan M. Garber.</strong> Advances in Cost-Effectiveness Analysis of Health Interventions. Working Paper 7198. 
          National Bureau of Economic Research. June 1999.</li>
        <li><strong>National Institute for Health and Clinical Excellence (NICE, England). </strong> Guide to the methods
          of technology appraisal. June, 2008. </li>
        <li><strong>National Institute for Health and Clinical Excellence (NICE, England).</strong> 
          Briefing papers for the update to the Methods Guide (2008 Technology Appraisals Methods Guide), January, 2012</li>
        <li><strong>National Health and Medical Research Council. (NHRMC, Australia)</strong> How to compare the costs and benefits:
          evaluation of the economic evidence. Handbook series on preparing clinical practice guidelines. July 2001</li>
        <li><strong>Franco Sassi.</strong> Calculating QALYs, comparing QALY and DALY calculations.
          Department of Social Policy, The London School of Economics and Political Science, London, UK. Advance Access publication. July, 2006</li>
        <li><strong>Sinnott PL, Joyce VR, Barnett PG.</strong> Preference Measurement in Economic Analysis. 
          Guidebook. Menlo Park CA. VA Palo Alto, Health Economics Resource Center; 2007.</li>
        <li><strong>Health Information and Quality Authority (Ireland).  Guidelines for the Economic Evaluation of 
          Health Technologies in Ireland. November 2010.</strong> Guidelines for the Economic Evaluation of Health Technologies in Ireland. www.hiqa.ie</li>
			</ul>
		</div>
    </div>
		</xsl:if>
</xsl:template>
</xsl:stylesheet>
