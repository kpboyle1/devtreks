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
      <h4 class="ui-bar-b"><strong>Life Cycle Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
          This tool calculates life cycle cost totals for input uris.   The calculator updates the base input's OCPrice, 
          AOHPrice, and CAPPrice with the calculated totals. 
          The calculator does not change the base input's Units or Amounts.
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
      <h4 class="ui-bar-b"><strong>Step 2 of 4. Define SubCosts and Impacts</strong></h4>
		  <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
			  <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubCost</strong></h4>
        <div>
          <label for="SubPName" class="ui-hidden-accessible">Name</label>
          <input id="SubPName" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription">Description</label>
				  <textarea id="SubPDescription" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType">Price Type</label>
            <select id="SubPType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@SubPType = 'oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@SubPType = 'aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@SubPType = 'cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount">Amount </label>
           <input id="SubPAmount" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit">Unit </label>
            <input id="SubPUnit" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice">Price </label>
            <input id="SubPPrice" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate">Escalate Rate </label>
            <input id="SubPEscRate" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType">Escalate Type </label>
            <select id="SubPEscType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">linear</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'linear')">
                  <xsl:attribute name="selected" />
                </xsl:if>linear
              </option>
              <option>
                <xsl:attribute name="value">geometric</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'geometric')">
                  <xsl:attribute name="selected" />
                </xsl:if>geometric
              </option>
              <option>
                <xsl:attribute name="value">upvtable</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'upvtable')">
                  <xsl:attribute name="selected" />
                </xsl:if>upvtable
              </option>
              <option>
                <xsl:attribute name="value">spv</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'spv')">
                  <xsl:attribute name="selected" />
                </xsl:if>spv
              </option>
              <option>
                <xsl:attribute name="value">caprecovery</xsl:attribute>
                <xsl:if test="(@SubPEscType = 'caprecovery')">
                  <xsl:attribute name="selected" />
                </xsl:if>caprecovery
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor">Discount Factor</label>
            <input id="SubPFactor" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears">Discount Years</label>
            <input id="SubPYears" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel">Label </label>
            <input id="SubPLabel" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes">Discount Year Times</label>
            <input id="SubPYearTimes" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType">Price Basis Type </label>
            <select id="SubPOtherType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue">Salvage Value</label>
            <input id="SubPSalvValue" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal">Total Cost </label>
            <input id="SubPTotal" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit">Unit Cost</label>
            <input id="SubPTotalPerUnit" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit" /></xsl:attribute>
              </input>
          </div>
        </div>
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
	      <xsl:if test="($docToCalcNodeName != 'inputseries' or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				  <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
            @UseSameCalculator, @Overwrite)"/>
		    </xsl:if>
        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
            @WhatIfTagName, @RelatedCalculatorsType)"/>
      </div>
				<xsl:if test="($lastStepNumber = 'stepfour')
					or ($viewEditType != 'full')
					or (@LCCTotalCost != 0)">
          <div class="ui-grid-a">
						<div class="ui-block-a">
							SubCost: 
							<xsl:value-of select="@SubPName" />: <xsl:value-of select="@SubPTotal" />
						</div>
						<div class="ui-block-b">
							<strong>Input.OCPrice: </strong> 
							<xsl:value-of select="@OCTotalCost" />
						</div>
						<div class="ui-block-a">
							<strong>Input.AOHPrice: </strong>
							<xsl:value-of select="@AOHTotalCost" />
						</div>
						<div class="ui-block-b">
							<strong>Input.CAPPrice: </strong>
							<xsl:value-of select="@CAPTotalCost" />
						</div>
						<div class="ui-block-a">
							<strong>Total LCC: </strong>
							<xsl:value-of select="@LCCTotalCost" />
						</div>
						<div class="ui-block-b">
							<strong>Total EAA: </strong>
							<xsl:value-of select="@EAATotalCost" />
						</div>
						<div class="ui-block-a">
							<strong>Total Unit LCC: </strong>
							<xsl:value-of select="@UnitTotalCost" />
						</div>
						<div class="ui-block-b">
							Unit:
							<xsl:value-of select="@PerUnitUnit" />
						</div>
						<div class="ui-block-a">
							<strong>Unit Total Amount: </strong>
							<xsl:value-of select="@PerUnitAmount" />
						</div>
						<div class="ui-block-b">
						</div>
					</div>
				</xsl:if>
			  <h4 class="ui-bar-b"><strong>Fill in investment variables</strong></h4>
				<div>
          <label for="CalculatorName" class="ui-hidden-accessible"></label>
					<input id="CalculatorName" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
            </xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					</input>
				</div>
				<div class="ui-grid-a">
					
					<div class="ui-block-a">
						<label for="ServiceLifeYears" >Service Life</label>
            <input type="text" id="ServiceLifeYears" data-mini="true">
               <xsl:if test="($viewEditType = 'full')">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ServiceLifeYears;double;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@ServiceLifeYears" /></xsl:attribute>
							</input>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
						<label for="PlanningConstructionYears" >Planning Construction Years</label>
						<input type="text" id="PlanningConstructionYears">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PlanningConstructionYears;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@PlanningConstructionYears" /></xsl:attribute>
						</input>
					</div>
					<div class="ui-block-b">
						<label for="YearsFromBaseDate" >Years From Base Date</label>
						<input type="text" id="YearsFromBaseDate">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;YearsFromBaseDate;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@YearsFromBaseDate" /></xsl:attribute>
						</input>
					</div>
					<div class="ui-block-a">
						<label for="PerUnitAmount" >Per Unit Amount</label>
						<input type="text" id="PerUnitAmount">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PerUnitAmount;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@PerUnitAmount" /></xsl:attribute>
						</input>
					</div>
					<div class="ui-block-b">
						<label for="PerUnitUnit" >Per Unit Unit</label>
						<input type="text" id="PerUnitUnit">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PerUnitUnit;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@PerUnitUnit" /></xsl:attribute>
						</input>
					</div>
				</div>
        <xsl:value-of select="DisplayDevPacks:WriteAlternatives($searchurl, $viewEditType,
            @AlternativeType, @TargetType)"/>
			  <div >
				  <label for="Description">Description</label>
				  <textarea id="Description" data-mini="true">
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
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. Only the real rate (with constant dollars) is used in this version of the calculator.</li>
			</ul>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. SubCosts: </strong> Up to 1 subcosts can be defined. These definitions refer to these subcosts as LCA elements.</li>
				<li><strong>SubCost Aggregation: </strong> The label is used to aggregate subcost. When possible, use labels contained in industry-standard Work Breakdown Schedules, or Classification systems. Because of display limitations, only 10 subcost can be displayed.</li>
				<li><strong>Name: </strong> Name of the subcost. Examples include, electricity, water, gas, taxes, contingencies ...</li>
        <li><strong>Label: </strong> Work Breakdown Structure label used to aggregate subcosts and subbenefits. </li>
        <li><strong>Price Type: </strong> Type of parent Input or Output price to update when the calculators are saved. Options include oc (Operating Costs), aoh (Allocated Overhead Costs), cap (Capital Costs), or rev (Revenues).</li>
        <li><strong>Amount: </strong> The quantity of the LCA element to be used to determine costs or benefits. Unit costs and benefits often will use an amount of 1.</li>
        <li><strong>Unit: </strong> The unit of measurement of the LCA element.</li>
        <li><strong>Price: </strong> The unit price of the LCA element to be used to determine costs or benefits.</li>
        <li><strong>Escalate Rate: </strong> Rate of price increase or decrease over the life span of the input. Will be divided by 100 in the calculations. The initial subcost or subbenefit will be multiplied by this rate and used with the linear or geometric Escalate Type property. </li>
        <li><strong>Escalate Type: </strong>Type of price escalation. Type of price escalation used to calculate life cycle costs and benefits. Use the ‘none’ option, along with the Discount Years and Salvage Value properties to determine a present value subcost or subbenefit. 
          Use the 'upvtable' option, or uniform present value, if NIST 135-style price indexes are being used to compute escalated costs or benefits (and complete the Discount Factor property). Use the 'spv', or single present value, option to calculate a discounted cost/benefit (and complete the Discount Years property). 
          Use the 'caprecovery' option to calculate an amortized annual cost (and complete the Discount Years and Salvage Value properties; in addition, the Discount Times property can be used to compute 'cost/benefit per hour' of use style calculations). Use the ‘uniform’ option along with Step’s 3 properties 
          to calculate a uniform price escalation. Use the ‘linear’ or ‘geometric’ options, along the Escalate Rate property, to calculate linear or geometric series price escalation.</li>
        <li><strong>Discount Factor: </strong>A present value taken from NIST 135-style price escalation indexes. The index should be for a period that sums together the service life of the project and the planning construction years. See table 5-4 in the NIST 135 reference.  </li>
        <li><strong>Discount Years:  </strong>The number of years to use in single present value and capital recovery discount formulas. Used when the Escalation Type property is 'spv' (Single Present Value) or "caprecovery" (Capital Recovery). </li>
        <li><strong>Discount Year Times:  </strong>The number of times to use the Discount Years property to calculate recurrent costs. Only used when the Escalate Type property is 'spv', or Single Present Value, and the Discount Years property is greater than zero. Total number of times will not exceed the service life of the investment.</li>
        <li><strong>Other Price Type: </strong> The type of price used to calculate subcosts or subbenefits. Options include none, market, list, contracted, government, production, copay, premium, incentive, penalty, fee, engineered, and consensus. When possible, use market prices.</li>
         <li><strong>Salvage Value:  </strong>Used with an Escalation Type of spv and Service Life Years > 0 to calculate a discounted salvage, or residual, value. Also used with an Escalation Type of caprecovery and DiscountYears > 0 to calculate an annual capital recovery cost for operating budgets.</li>
        <li><strong>Total Cost: </strong> Not a data entry field. The final discounted cost.</li>
        <li><strong>Per Unit Cost: </strong> Not a data entry field. The final discounted cost divided by the Per Unit Amount.</li>
      </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Use Same Calculator Pack In Descendants?: </strong> True to insert or update this same calculator in children.</li>
				<li><strong>Step 3. Overwrite Descendants?: </strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 3. What If Tag Name: </strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 3. Related Calculators Type: </strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
        <li><strong>Step 3. Service Life Years: </strong> The life span of the input.</li>
				<li><strong>Step 3. Planning Construction Years: </strong> Number of years in the planning and construction period. Also known of the preproduction period. </li>
        <li><strong>Step 3. Years From Base Date: </strong> The base date is the input or output’s date. Enter an integer that specifies the specific year within the Planning Construction Years when the input is installed. The NIST reference uses examples that are installed in 1 year and given a value of 1 for this property. A project with a Planning Construction Years greater than 1 would insert whichever year within the period when the input is actually installed.</li>
				<li><strong>Step 3. Target Type: </strong> The Benchmark option is used to define a baseline, or benchmark, input. 
          The Actual option is used to define the actual results for an input. 
          The Full Target and Partial Target options are used to carry out progress and goal-related analyses.</li>
        <li><strong>Step 3. Alternative Type: </strong> Type of alternative used in the comparison of different inputs. </li>
        <li><strong>Step 3. Per Unit Amount: </strong> An amount used to derive per unit costs. For example, a 1000 ft2 (m2) house would use a value of 1000.</li>
        <li><strong>Step 3. Per Unit Amount: </strong> A unit used in per unit costs. For example, a 1000 ft2 (m2) house would use a value of ft2 (m2).</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>Summary Costs</strong></h4>
      <ul data-role="listview">
        <li><strong>Individual SubCost or SubBenefit: </strong> The total discounted cost or benefit derived from running the calculator.</li>
        <li><strong>Total Discounted OC, AOH, and CAP prices: </strong> Sum of the discounted subcosts that will be added to parent input.</li>
        <li><strong>Total Life Cycle Cost: </strong> Sum of the Total OC, AOH, and CAP costs.</li>
        <li><strong>Total Equivalent Annual Annuity: </strong> Amortizes the life cycle cost over the service life years, using an equivalent annual annuity discounting formula.</li>
        <li><strong>Total Per Unit Cost: </strong> Total Life Cycle Cost divided by the Per Unit Amount.</li>
        <li><strong>Per Unit Unit: </strong> Unit of measurement for the Total Per Unit Cost.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Hallam, Eidman, Morehart and Klonsky (editors): </strong> Commodity Cost and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
			  <li><strong>Lippiatt: </strong> BEES 4.0, Building for Environmental and Economic Sustainability Technical Manual and User Guide. US Department of Commerce, National Institute of Standards and Technology. NIST 7423. 2007 </li>
        <li><strong>National Institute for Standards and Technology Handbook 135: </strong>Life-Cycle Costing Manual. 1996 Edition. US Department of Commerce</li>
        <li><strong>United States Government Accountability Office: </strong> Applied Research and Methods. GAO Cost Estimating and Assessment Guide. Best Practices for Developing and Managing Capital Program Costs. March, 2009.</li>
      </ul>
      </div>
		</div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>