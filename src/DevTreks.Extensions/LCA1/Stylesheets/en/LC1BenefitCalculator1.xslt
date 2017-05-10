<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2017, May -->
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
        <xsl:when test="($docToCalcNodeName = 'outputgroup' 
							or $docToCalcNodeName = 'output' 
							or $docToCalcNodeName = 'outputseries'
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This output calculator does not appear appropriate for the document being analyzed. Are you 
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
					This tool calculates life cycle benefits totals for output uris. 
							The discounted totals can include up to 10 subbenefits and impacts. 
              SubBenefits from health care investments might include earned income, government benefits, and business returns.
              SubBenefits from building investments might include commodity production, commercial rental, retail sales, and revenue-sharing returns.
							Impacts, such as environmental impacts, can be taken from associated life cycle analyses or other calculators. 
          <br />
          The calculator updates the base output's Price with the calculated totals. 
              The calculator does not change the base output's Units or Amounts.
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
      <h4 class="ui-bar-b"><strong>Step 2 of 4. Define SubBenefits and Impacts</strong></h4>
		  <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
			  <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <h4 class="ui-bar-b"><strong>SubBenefits and Impacts</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 1</strong></h4>
        <div>
          <label for="SubPName1" class="ui-hidden-accessible">Name 1</label>
          <input id="SubPName1" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName1;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName1" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription1">Description 1</label>
				  <textarea id="SubPDescription1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription1;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription1" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType1">Price Type 1</label>
            <select id="SubPType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType1;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType1 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount1">Amount 1 </label>
           <input id="SubPAmount1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit1">Unit 1 </label>
            <input id="SubPUnit1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice1">Price 1 </label>
            <input id="SubPPrice1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate1">Escalate Rate 1 </label>
            <input id="SubPEscRate1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType1">Escalate Type 1 </label>
            <input id="SubPEscType1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType1;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor1">Discount Factor 1</label>
            <input id="SubPFactor1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears1">Discount Years 1</label>
            <input id="SubPYears1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel1">Label 1 </label>
            <input id="SubPLabel1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes1">Discount Year Times 1</label>
            <input id="SubPYearTimes1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType1">Price Basis Type 1 </label>
            <select id="SubPOtherType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType1;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType1 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue1">Salvage Value 1</label>
            <input id="SubPSalvValue1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal1">Total Benefit 1 </label>
            <input id="SubPTotal1" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal1" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit1">Unit Benefit 1 </label>
            <input id="SubPTotalPerUnit1" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit1" /></xsl:attribute>
              </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 2</strong></h4>
        <div>
          <label for="SubPName2" class="ui-hidden-accessible">Name 2</label>
          <input id="SubPName2" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName2;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName2" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription2">Description 2</label>
				  <textarea id="SubPDescription2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription2;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription2" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType2">Price Type 2 </label>
            <select id="SubPType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType2;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType2 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount2">Amount 2 </label>
            <input id="SubPAmount2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit2">Unit 2 </label>
            <input id="SubPUnit2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice2">Price 2 </label>
            <input id="SubPPrice2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
               <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate2">Escalate Rate 2 </label>
            <input id="SubPEscRate2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
               <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType2">Escalate Type 2 </label>
            <input id="SubPEscType2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType2;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor2">Discount Factor 2</label>
            <input id="SubPFactor2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears2">Discount Years 2</label>
            <input id="SubPYears2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel2">Label 2 </label>
            <input id="SubPLabel2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes2">Discount Year Times 2</label>
            <input id="SubPYearTimes2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType2">Price Basis Type 2 </label>
            <select id="SubPOtherType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType2;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType2 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue2">Salvage Value 2</label>
            <input id="SubPSalvValue2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal2">Total Benefit 2 </label>
            <input id="SubPTotal2" type="text"  data-mini="true">
              <xsl:attribute name="value"><xsl:value-of select="@SubPTotal2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit2">Unit Benefit 2 </label>
            <input id="SubPTotalPerUnit2" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit2" /></xsl:attribute>
              </input>
          </div>
      </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 3</strong></h4>
        <div>
          <label for="SubPName3" class="ui-hidden-accessible">Name 3</label>
          <input id="SubPName3" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName3;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName3" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription3">Description 3</label>
				  <textarea id="SubPDescription3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription3;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription3" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType3">Price Type 3 </label>
            <select id="SubPType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"> <xsl:value-of select="$searchurl" />;SubPType3;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType3 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount3">Amount 3 </label>
            <input id="SubPAmount3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"> <xsl:value-of select="$searchurl" />;SubPAmount3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit3">Unit 3 </label>
            <input id="SubPUnit3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice3">Price 3 </label>
            <input id="SubPPrice3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"> <xsl:value-of select="@SubPPrice3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate3">Escalate Rate 3 </label>
            <input id="SubPEscRate3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType3">Escalate Type 3 </label>
            <input id="SubPEscType3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType3;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor3">Discount Factor 3</label>
            <input id="SubPFactor3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears3">Discount Years 3</label>
            <input id="SubPYears3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel3">Label 3 </label>
            <input id="SubPLabel3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes3">Discount Year Times 3</label>
            <input id="SubPYearTimes3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType3">Price Basis Type 3 </label>
            <select id="SubPOtherType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType3;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType3 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue3">Salvage Value 3</label>
            <input id="SubPSalvValue3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal3">Total Benefit 3 </label>
            <input id="SubPTotal3" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal3" /></xsl:attribute>
              </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit3">Unit Benefit 3 </label>
            <input id="SubPTotalPerUnit3" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit3" /></xsl:attribute>
             </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 4</strong></h4>
        <div>
          <label for="SubPName4" class="ui-hidden-accessible">Name 4</label>
          <input id="SubPName4" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName4;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName4" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription4">Description 4</label>
				  <textarea id="SubPDescription4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription4;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription4" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType4">Price Type 4 </label>
            <select id="SubPType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"> <xsl:value-of select="$searchurl" />;SubPType4;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType4 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount4">Amount 4 </label>
            <input id="SubPAmount4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit4">Unit 4 </label>
            <input id="SubPUnit4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit4;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice4">Price 4 </label>
            <input id="SubPPrice4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate4">Escalate Rate 4 </label>
            <input id="SubPEscRate4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType4">Escalate Type 4 </label>
            <input id="SubPEscType4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType4;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor4">Discount Factor 4</label>
            <input id="SubPFactor4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears4">Discount Years 4</label>
            <input id="SubPYears4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel4">Label 4 </label>
            <input id="SubPLabel4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel4;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes4">Discount Year Times 4</label>
            <input id="SubPYearTimes4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType4">Price Basis Type 4 </label>
            <select id="SubPOtherType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType4;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType4 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue4">Salvage Value 4</label>
            <input id="SubPSalvValue4" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal4">Total Benefit 4 </label>
            <input id="SubPTotal4" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit4">Unit Benefit 4 </label>
            <input id="SubPTotalPerUnit4" type="text"  data-mini="true">
               <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit4" /></xsl:attribute>
            </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true"> 
        <h4 class="ui-bar-b"><strong>SubBenefit 5</strong></h4>
        <div>
          <label for="SubPName5" class="ui-hidden-accessible">Name 5</label>
					<input id="SubPName5" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName5;string;75</xsl:attribute>
            </xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@SubPName5" /></xsl:attribute>
					</input>
				</div>
        <div >
				  <label for="SubPDescription5">Description 5</label>
				  <textarea id="SubPDescription5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription5;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription5" />
				  </textarea>
			  </div>
      <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType5">Price Type 5 </label>
            <select id="SubPType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType5;string;50</xsl:attribute>
              </xsl:if>
							<option>
								<xsl:attribute name="value">none</xsl:attribute>
								<xsl:if test="(@SubPType5 = 'none')">
									<xsl:attribute name="selected" />
								</xsl:if>none
							</option>
							<option>
								<xsl:attribute name="value">rev</xsl:attribute>
								<xsl:if test="(@SubPType5 = 'rev')">
									<xsl:attribute name="selected" />
								</xsl:if>revenue
							</option>
						</select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount5">Amount 5 </label>
            <input id="SubPAmount5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit5">Unit 5 </label>
            <input id="SubPUnit5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit5;string;25</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPUnit5" /></xsl:attribute>
						</input>
            </div>
          <div class="ui-block-b">
            <label for="SubPPrice5">Price 5 </label>
           <input id="SubPPrice5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPPrice5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate5">Escalate Rate 5 </label>
            <input id="SubPEscRate5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPEscRate5" /></xsl:attribute>
						</input>
            </div>
          <div class="ui-block-b">
            <label for="SubPEscType5">Escalate Type 5 </label>
            <input id="SubPEscType5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType5;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor5">Discount Factor 5</label>
            <input id="SubPFactor5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPFactor5" /></xsl:attribute>
						</input>
            </div>
          <div class="ui-block-b">
            <label for="SubPYears5">Discount Years 5</label>
            <input id="SubPYears5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SubPYears5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel5">Label 5 </label>
            <input id="SubPLabel5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel5;string;15</xsl:attribute>
              </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPLabel5" /></xsl:attribute>
          </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes5">Discount Year Times 5</label>
            <input id="SubPYearTimes5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes5;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType5">Price Basis Type 5 </label>
            <select id="SubPOtherType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType5;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType5 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue5">Salvage Value 5</label>
            <input id="SubPSalvValue5" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue5;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal5">Total Benefit 5</label>
            <input id="SubPTotal5" type="text"  data-mini="true">
								  <xsl:attribute name="value"><xsl:value-of select="@SubPTotal5" /></xsl:attribute>
							  </input>
            </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit5">Unit Benefit 5</label>
            <input id="SubPTotalPerUnit5" type="text"  data-mini="true">
							<xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit5" /></xsl:attribute>
						</input>
          </div>
      </div>
     </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 6</strong></h4>
        <div>
          <label for="SubPName6" class="ui-hidden-accessible">Name 6</label>
          <input id="SubPName6" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName6;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName6" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription6">Description 6</label>
				  <textarea id="SubPDescription6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription6;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription6" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType6">Price Type 6</label>
            <select id="SubPType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType6;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType6 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount6">Amount 6 </label>
           <input id="SubPAmount6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit6">Unit 6 </label>
            <input id="SubPUnit6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit6;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice6">Price 6 </label>
            <input id="SubPPrice6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate6">Escalate Rate 6 </label>
            <input id="SubPEscRate6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType6">Escalate Type 6 </label>
            <input id="SubPEscType6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType6;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor6">Discount Factor 6</label>
            <input id="SubPFactor6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears6">Discount Years 6</label>
            <input id="SubPYears6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel6">Label 1 </label>
            <input id="SubPLabel6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel6;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes6">Discount Year Times 6</label>
            <input id="SubPYearTimes6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType6">Price Basis Type 6 </label>
            <select id="SubPOtherType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType6;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType6 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue6">Salvage Value 6</label>
            <input id="SubPSalvValue6" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal6">Total Benefit 6</label>
            <input id="SubPTotal6" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal6" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit6">Unit Benefit 6</label>
            <input id="SubPTotalPerUnit6" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit6" /></xsl:attribute>
              </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 7</strong></h4>
        <div>
          <label for="SubPName7" class="ui-hidden-accessible">Name 7</label>
          <input id="SubPName7" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName7;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName7" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription7">Description 7</label>
				  <textarea id="SubPDescription7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription7;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription7" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType7">Price Type 7</label>
            <select id="SubPType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType7;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType7 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount7">Amount 7</label>
           <input id="SubPAmount7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit7">Unit 7</label>
            <input id="SubPUnit7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit7;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice7">Price 7</label>
            <input id="SubPPrice7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate7">Escalate Rate 7</label>
            <input id="SubPEscRate7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType7">Escalate Type 7 </label>
            <input id="SubPEscType7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType7;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor7">Discount Factor 7</label>
            <input id="SubPFactor7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears7">Discount Years 7</label>
            <input id="SubPYears7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel7">Label 7</label>
            <input id="SubPLabel7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel7;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes7">Discount Year Times 7</label>
            <input id="SubPYearTimes7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType7">Price Basis Type 7 </label>
            <select id="SubPOtherType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType7;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType7 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue7">Salvage Value 7</label>
            <input id="SubPSalvValue7" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal7">Total Benefit 7</label>
            <input id="SubPTotal7" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal7" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit7">Unit Benefit 7</label>
            <input id="SubPTotalPerUnit7" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit7" /></xsl:attribute>
              </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 8</strong></h4>
        <div>
          <label for="SubPName8" class="ui-hidden-accessible">Name 8</label>
          <input id="SubPName8" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName8;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName8" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription8">Description 8</label>
				  <textarea id="SubPDescription8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription8;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription8" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType8">Price Type 8</label>
            <select id="SubPType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType8;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType8 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount8">Amount 8</label>
           <input id="SubPAmount8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit8">Unit 8</label>
            <input id="SubPUnit8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit8;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice8">Price 8</label>
            <input id="SubPPrice8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate8">Escalate Rate 8</label>
            <input id="SubPEscRate8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType8">Escalate Type 8 </label>
            <input id="SubPEscType8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType8;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor8">Discount Factor 8</label>
            <input id="SubPFactor8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears8">Discount Years 8</label>
            <input id="SubPYears8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel8">Label 8</label>
            <input id="SubPLabel8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel8;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes8">Discount Year Times 8</label>
            <input id="SubPYearTimes8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType8">Price Basis Type 8 </label>
            <select id="SubPOtherType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType8;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType8 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue8">Salvage Value 8</label>
            <input id="SubPSalvValue8" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal8">Total Benefit 8</label>
            <input id="SubPTotal8" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal8" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit8">Unit Benefit 8</label>
            <input id="SubPTotalPerUnit8" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit8" /></xsl:attribute>
              </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 9</strong></h4>
        <div>
          <label for="SubPName9" class="ui-hidden-accessible">Name 9</label>
          <input id="SubPName9" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName9;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName9" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription9">Description 9</label>
				  <textarea id="SubPDescription9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription9;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription9" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType9">Price Type 9</label>
            <select id="SubPType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType9;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType9 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount9">Amount 9</label>
           <input id="SubPAmount9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit9">Unit 9</label>
            <input id="SubPUnit9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit9;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice9">Price 9</label>
            <input id="SubPPrice9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate9">Escalate Rate 9</label>
            <input id="SubPEscRate9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType9">Escalate Type 9 </label>
            <input id="SubPEscType9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType9;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor9">Discount Factor 9</label>
            <input id="SubPFactor9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears9">Discount Years 9</label>
            <input id="SubPYears9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel9">Label 9</label>
            <input id="SubPLabel9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel9;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes9">Discount Year Times 9</label>
            <input id="SubPYearTimes9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType9">Price Basis Type 9 </label>
            <select id="SubPOtherType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType9;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType9 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue9">Salvage Value 9</label>
            <input id="SubPSalvValue9" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal9">Total Benefit 9</label>
            <input id="SubPTotal9" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal9" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit9">Unit Benefit 9</label>
            <input id="SubPTotalPerUnit9" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit9" /></xsl:attribute>
              </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>SubBenefit 10</strong></h4>
        <div>
          <label for="SubPName10" class="ui-hidden-accessible">Name 10</label>
          <input id="SubPName10" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPName10;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SubPName10" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SubPDescription10">Description 10</label>
				  <textarea id="SubPDescription10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPDescription10;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SubPDescription10" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SubPType10">Price Type 10</label>
            <select id="SubPType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPType10;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPType10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@SubPType10 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPAmount10">Amount 10</label>
           <input id="SubPAmount10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPAmount10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPAmount10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPUnit10">Unit 10</label>
            <input id="SubPUnit10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPUnit10;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPUnit10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPPrice10">Price 10</label>
            <input id="SubPPrice10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPPrice10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPPrice10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPEscRate10">Escalate Rate 10</label>
            <input id="SubPEscRate10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscRate10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscRate10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPEscType10">Escalate Type 10 </label>
            <input id="SubPEscType10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPEscType10;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPEscType10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPFactor10">Discount Factor 10</label>
            <input id="SubPFactor10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPFactor10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPFactor10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYears10">Discount Years 10</label>
            <input id="SubPYears10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYears10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYears10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPLabel10">Label 10 </label>
            <input id="SubPLabel10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPLabel10;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPLabel10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPYearTimes10">Discount Year Times 10</label>
            <input id="SubPYearTimes10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPYearTimes10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPYearTimes10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPOtherType10">Price Basis Type 10 </label>
            <select id="SubPOtherType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPOtherType10;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">market</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'market')">
                  <xsl:attribute name="selected" />
                </xsl:if>market
              </option>
              <option>
                <xsl:attribute name="value">list</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'list')">
                  <xsl:attribute name="selected" />
                </xsl:if>list
              </option>
              <option>
                <xsl:attribute name="value">contracted</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'contracted')">
                  <xsl:attribute name="selected" />
                </xsl:if>contracted
              </option>
              <option>
                <xsl:attribute name="value">government</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'government')">
                  <xsl:attribute name="selected" />
                </xsl:if>government
              </option>
              <option>
                <xsl:attribute name="value">production</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'production')">
                  <xsl:attribute name="selected" />
                </xsl:if>production
              </option>
              <option>
                <xsl:attribute name="value">copay</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'copay')">
                  <xsl:attribute name="selected" />
                </xsl:if>copay
              </option>
              <option>
                <xsl:attribute name="value">premium</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'premium')">
                  <xsl:attribute name="selected" />
                </xsl:if>premium
              </option>
              <option>
                <xsl:attribute name="value">incentive</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'incentive')">
                  <xsl:attribute name="selected" />
                </xsl:if>incentive
              </option>
              <option>
                <xsl:attribute name="value">penalty</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'penalty')">
                  <xsl:attribute name="selected" />
                </xsl:if>penalty
              </option>
              <option>
                <xsl:attribute name="value">fee</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'fee')">
                  <xsl:attribute name="selected" />
                </xsl:if>fee
              </option>
              <option>
                <xsl:attribute name="value">engineered</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'engineered')">
                  <xsl:attribute name="selected" />
                </xsl:if>engineered
              </option>
              <option>
                <xsl:attribute name="value">consensus</xsl:attribute>
                <xsl:if test="(@SubPOtherType10 = 'consensus')">
                  <xsl:attribute name="selected" />
                </xsl:if>consensus
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SubPSalvValue10">Salvage Value 10</label>
            <input id="SubPSalvValue10" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SubPSalvValue10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SubPSalvValue10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SubPTotal10">Total Benefit 10</label>
            <input id="SubPTotal10" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotal10" /></xsl:attribute>
             </input>
          </div>
          <div class="ui-block-b">
            <label for="SubPTotalPerUnit10">Unit Benefit 10</label>
            <input id="SubPTotalPerUnit10" type="text"  data-mini="true">
                <xsl:attribute name="value"><xsl:value-of select="@SubPTotalPerUnit10" /></xsl:attribute>
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
	    <xsl:if test="($docToCalcNodeName != 'outputseries' or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
				<xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
          @UseSameCalculator, @Overwrite)"/>
		  </xsl:if>
      <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, @RelatedCalculatorsType)"/>
      </div>
					<xsl:if test="($lastStepNumber = 'stepfour')
						or ($viewEditType != 'full')
						or (@LCBTotalBenefit != 0)">
            <div class="ui-grid-a">
              <div class="ui-block-a">
								SubBenefit 1: 
								<xsl:value-of select="@SubPName1" />: <xsl:value-of select="@SubPTotal1" />
							</div>
							<div class="ui-block-b">
								SubBenefit 2: 
								<xsl:value-of select="@SubPName2" />: <xsl:value-of select="@SubPTotal2" />
							</div>
							<div class="ui-block-a">
								SubBenefit 3: 
								<xsl:value-of select="@SubPName3" />: <xsl:value-of select="@SubPTotal3" />
							</div>
							<div class="ui-block-b">
								SubBenefit 4: 
								<xsl:value-of select="@SubPName4" />: <xsl:value-of select="@SubPTotal4" />
							</div>
							<div class="ui-block-a">
								SubBenefit 5: 
								<xsl:value-of select="@SubPName5" />: <xsl:value-of select="@SubPTotal5" />
							</div>
							<div class="ui-block-b">
							</div>
							<div class="ui-block-a">
								SubBenefit 6: 
								<xsl:value-of select="@SubPName6" />; <xsl:value-of select="@SubPTotal6" />
							</div>
							<div class="ui-block-b">
								SubBenefit 7: 
								<xsl:value-of select="@SubPName7" />; <xsl:value-of select="@SubPTotal7" /> 
							</div>
							<div class="ui-block-a">
								SubBenefit 8: 
								<xsl:value-of select="@SubPName8" />; <xsl:value-of select="@SubPTotal8" />
							</div>
							<div class="ui-block-b">
								SubBenefit 9: 
								<xsl:value-of select="@SubPName9" />; <xsl:value-of select="@SubPTotal9" />
							</div>
							<div class="ui-block-a">
								SubBenefit 10: 
								<xsl:value-of select="@SubPName10" />; <xsl:value-of select="@SubPTotal10" />
							</div>
							<div class="ui-block-b">
							</div>
							<div class="ui-block-b">
								<strong>Output.Price: </strong>
								<xsl:value-of select="@RTotalBenefit" />
							</div>
							<div class="ui-block-a">
								<strong>Total LCB: </strong>
								<xsl:value-of select="@LCBTotalBenefit" />
							</div>
							<div class="ui-block-b">
								<strong>Total EAA: </strong>
								<xsl:value-of select="@EAATotalBenefit" />
							</div>
							<div class="ui-block-a">
								<strong>Total Unit LCB: </strong>
								<xsl:value-of select="@UnitTotalBenefit" />
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
			<h4 class="ui-bar-b"><strong>Instructions </strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 1</h4>
			<ul data-role="listview">
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. Only the real rate (with constant dollars) is used in this version of the calculator.</li>
			</ul>
			</div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. SubBenefits: </strong> Up to 10 subbenefits can be defined. </li>
				<li><strong>SubBenefit Aggregation: </strong> The label is used to aggregate subbenefit. When possible, use labels contained in industry-standard Work Breakdown Schedules, or Classification systems. Because of display limitations, only 10 subbenefit can be displayed. </li>
        <li><strong>Name: </strong> Name of the subbenefit. Examples include sales, rentals, revenue-sharing returns, and subsidies ...</li>
        <li><strong>Label: </strong> Work Breakdown Structure label used to aggregate subcosts and subbenefits. </li>
        <li><strong>Price Type: </strong> Type of parent Input or Output price to update when the calculators are saved. Options include oc (Operating Costs), aoh (Allocated Overhead Costs), cap (Capital Costs), or rev (Revenues).</li>
        <li><strong>Amount: </strong> The quantity of the LCA element to be used to determine costs or benefits. Unit costs and benefits often will use an amount of 1.</li>
        <li><strong>Unit: </strong> The unit of measurement of the LCA element.</li>
        <li><strong>Price: </strong> The unit price of the LCA element to be used to determine costs or benefits.</li>
        <li><strong>Escalate Rate: </strong> Rate of price increase or decrease over the life span of the output. Will be divided by 100 in the calculations. The initial subcost or subbenefit will be multiplied by this rate and used with the linear or geometric Escalate Type property. </li>
        <li><strong>Escalate Type: </strong>Type of price escalation. Type of price escalation used to calculate life cycle costs and benefits. Use the ‘none’ option, along with the Discount Years and Salvage Value properties to determine a present value subcost or subbenefit. 
          Use the 'upvtable' option, or uniform present value, if NIST 135-style price indexes are being used to compute escalated benefits (and complete the Discount Factor property). Use the 'spv', or single present value, option to calculate a discounted benefit (and complete the Discount Years property). 
          Use the 'caprecovery' option to calculate an amortized annual benefit (and complete the Discount Years and Salvage Value properties; in addition, the Discount Times property can be used to compute 'cost/benefit per hour' of use style calculations). Use the ‘uniform’ option along with Step’s 3 properties 
          to calculate a uniform price escalation. Use the ‘linear’ or ‘geometric’ options, along the Escalate Rate property, to calculate linear or geometric series price escalation.</li>
        <li><strong>Discount Factor: </strong>A present value taken from NIST 135-style price escalation indexes. The index should be for a period that sums together the service life of the project and the planning construction years. See table 5-4 in the NIST 135 reference.  </li>
        <li><strong>Discount Years:  </strong>The number of years to use in single present value and capital recovery discount formulas. Used when the Escalation Type property is 'spv' (Single Present Value) or "caprecovery" (Capital Recovery). </li>
        <li><strong>Discount Year Times:  </strong>The number of times to use the Discount Years property to calculate recurrent benefits. Only used when the Escalate Type property is 'spv', or Single Present Value, and the Discount Years property is greater than zero. Total number of times will not exceed the service life of the investment.</li>
        <li><strong>Other Price Type: </strong> The type of price used to calculate subcosts or subbenefits. Options include none, market, list, contracted, government, production, copay, premium, incentive, penalty, fee, engineered, and consensus. When possible, use market prices.</li>
         <li><strong>Salvage Value:  </strong>Used with an Escalation Type of spv and Service Life Years > 0 to calculate a discounted salvage, or residual, value. Also used with an Escalation Type of caprecovery and DiscountYears > 0 to calculate an annual capital recovery benefit for operating budgets.</li>
        <li><strong>Total Cost: </strong> Not a data entry field. The final discounted benefit.</li>
        <li><strong>Per Unit Cost: </strong> Not a data entry field. The final discounted benefit divided by the Per Unit Amount.</li>
				</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Use Same Calculator Pack In Descendants?: </strong> True to insert or update this same calculator in children.</li>
				<li><strong>Step 3. Overwrite Descendants?: </strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 3. What If Tag Name: </strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 3. Related Calculators Type: </strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 3. Service Life Years: </strong> The life span of the output.</li>
				<li><strong>Step 3. Planning Construction Years: </strong> Number of years in the planning and construction period. Also known of the preproduction period. </li>
        <li><strong>Step 3. Years From Base Date: </strong> The base date is the input or output’s date. Enter an integer that specifies the specific year within the Planning Construction Years when the input is installed. The NIST reference uses examples that are installed in 1 year and given a value of 1 for this property. A project with a Planning Construction Years greater than 1 would insert whichever year within the period when the input is actually installed.</li>
				<li><strong>Step 3. Target Type: </strong> The Benchmark option is used to define a baseline, or benchmark, output. 
          The Actual option is used to define the actual results for an output. 
          The Full Target and Partial Target options are used to carry out progress and goal-related analyses.</li>
        <li><strong>Step 3. Alternative Type: </strong> Type of alternative used in the comparison of different outputs. </li>
        <li><strong>Step 3. Per Unit Amount: </strong> An amount used to derive per unit benefits. For example, a 1000 ft2 (m2) house would use a value of 1000.</li>
        <li><strong>Step 3. Per Unit Amount: </strong> A unit used in per unit benefits. For example, a 1000 ft2 (m2) house would use a value of ft2 (m2).</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>Summary Benefits</strong></h4>
      <ul data-role="listview">
        <li><strong>Individual SubBenefits: </strong> The total discounted benefit derived from running the calculator.</li>
        <li><strong>Total Discounted Benefit price: </strong> Sum of the discounted subbenefits that will be added to parent output.</li>
        <li><strong>Total Life Cycle Benefit: </strong> Sum of the Total Discounted Benefits.</li>
        <li><strong>Total Equivalent Annual Annuity: </strong> Amortizes the life cycle benefit over the service life years, using an equivalent annual annuity discounting formula.</li>
        <li><strong>Total Per Unit Benefit: </strong> Total Life Cycle Benefit divided by the Per Unit Amount.</li>
        <li><strong>Per Unit Unit: </strong> Unit of measurement for the Total Per Unit Benefit.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Hallam, Eidman, Morehart and Klonsky (editors): </strong> Commodity Benefit and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
			  <li><strong>Lippiatt: </strong> BEES 4.0, Building for Environmental and Economic Sustainability Technical Manual and User Guide. US Department of Commerce, National Institute of Standards and Technology. NIST 7423. 2007 </li>
        <li><strong>National Institute for Standards and Technology Handbook 135: </strong>Life-Cycle Costing Manual. 1996 Edition. US Department of Commerce</li>
        <li><strong>United States Government Accountability Office: </strong> Applied Research and Methods. GAO Cost Estimating and Assessment Guide. Best Practices for Developing and Managing Capital Program Costs. March, 2009.</li>
      </ul>
      </div>
		</div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>