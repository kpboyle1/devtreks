<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
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
      <xsl:choose>
        <xsl:when test="(contains($docToCalcNodeName, 'outcome')
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This outcome calculator does not appear appropriate for the document being calculated. Are you 
					sure this is the right calculator?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>M and E Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool tracks monitoring and evaluation indicators for outcome uris. Up to 20 new 
          M and E indicators can be added for each outcome. 
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
      <div>
        <label for="CalculatorName">Calculator Name</label>
				<input id="CalculatorName" type="text" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
          </xsl:if>
					<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
				</input>
			</div>
     <h4 class="ui-bar-b"><strong>M and E Indicators</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 1</strong></h4>
        <div>
          <label for="IndName1" class="ui-hidden-accessible">Name 1</label>
          <input id="IndName1" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName1;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName1" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription1">Indicator 1 Description</label>
				  <textarea id="IndDescription1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription1;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription1" />
				  </textarea>
			  </div>
        <div >
          <label for="IndURL1">Indicator 1 URL</label>
          <textarea id="IndURL1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL1;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL1" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel1">Label 1 </label>
            <input id="IndLabel1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel1">Rel Label 1 </label>
            <input id="IndRelLabel1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel1;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate1">Date 1 </label>
            <input id="IndDate1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate1;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType1">Dist Type 1</label>
            <select class="Select225" id="IndType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType1 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType1 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType1 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType1 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType1 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType1 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType1 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType1 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType1 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType1 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType1 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount1">Q1 1 </label>
						<input id="Ind1Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount1;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit1">Q1 Unit 1 </label>
						<input id="Ind1Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount1">Q2 1 </label>
						<input id="Ind2Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit1">Q2 Unit 1</label>
						<input id="Ind2Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit1;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount1">Q3 1 </label>
						<input id="Ind3Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount1;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit1">Q3 Unit 1 </label>
						<input id="Ind3Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount1">Q4 1 </label>
						<input id="Ind4Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit1">Q4 Unit 1</label>
						<input id="Ind4Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit1;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount1">Q5 1 </label>
						<input id="Ind5Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit1">Q5 Unit 1 </label>
						<input id="Ind5Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator1">Math Operator 1</label>
            <select class="Select225" id="IndMathOperator1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator1 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO1">BaseIO 1</label>
            <select class="Select225" id="IndBaseIO1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO1 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount1">QT 1 </label>
						<input id="IndTAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit1">QT Unit 1 </label>
						<input id="IndTUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType1">Math Type 1</label>
            <select class="Select225" id="IndMathType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType1">Math Sub Type 1</label>
						<input id="IndMathSubType1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD1Amount1">QT D1 1 </label>
						<input id="IndTD1Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit1">QT D1 Unit 1 </label>
						<input id="IndTD1Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount1">QT D2 1 </label>
						<input id="IndTD2Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit1">QT D2 Unit 1 </label>
						<input id="IndTD2Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount1">QT Most 1 </label>
						<input id="IndTMAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit1">QT Most Unit 1 </label>
						<input id="IndTMUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit1" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount1">QT Low 1 </label>
						<input id="IndTLAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit1">QT Low Unit 1 </label>
						<input id="IndTLUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount1">QT High 1 </label>
						<input id="IndTUAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit1">QT High Unit 1 </label>
						<input id="IndTUUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit1" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression1">Math Expression 1</label>
          <input id="IndMathExpression1" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression1;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression1" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult1">Math Result 1</label>
				  <textarea id="IndMathResult1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult1;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult1" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 2</strong></h4>
        <div>
          <label for="IndName2" class="ui-hidden-accessible">Name 2</label>
          <input id="IndName2" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName2;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName2" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription2">Indicator 2 Description</label>
				  <textarea id="IndDescription2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription2;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription2" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL2">Indicator 2 URL</label>
          <textarea id="IndURL2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL2;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL2" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel2">Label 2</label>
            <input id="IndLabel2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel2">Rel Label 2 </label>
            <input id="IndRelLabel2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel2;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate2">Date 2 </label>
            <input id="IndDate2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate2;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType2">Dist Type 2</label>
            <select class="Select225" id="IndType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType2 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType2 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType2 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType2 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType2 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType2 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType2 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType2 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType2 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType2 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType2 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount2">Q1 2 </label>
						<input id="Ind1Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount2;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit2">Q1 Unit 2 </label>
						<input id="Ind1Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount2">Q2 2 </label>
						<input id="Ind2Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit2">Q2 Unit 2</label>
						<input id="Ind2Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit2;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount2">Q3 2 </label>
						<input id="Ind3Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount2;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit2">Q3 Unit 2 </label>
						<input id="Ind3Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount2">Q4 2 </label>
						<input id="Ind4Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit2">Q4 Unit 2</label>
						<input id="Ind4Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit2;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount2">Q5 2 </label>
						<input id="Ind5Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit2">Q5 Unit 2 </label>
						<input id="Ind5Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathOperator2">Math Operator 2</label>
            <select class="Select225" id="IndMathOperator2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator2 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO2">BaseIO 2</label>
            <select class="Select225" id="IndBaseIO2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO2 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount2">QT 2 </label>
						<input id="IndTAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit2">QT Unit 2</label>
						<input id="IndTUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType2">Math Type 2</label>
            <select class="Select225" id="IndMathType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType2">Math Sub Type 2</label>
						<input id="IndMathSubType2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType2" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount2">QT D1 2 </label>
						<input id="IndTD1Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit2">QT D1 Unit 2 </label>
						<input id="IndTD1Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount2">QT D2 2 </label>
						<input id="IndTD2Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit2">QT D2 Unit 2 </label>
						<input id="IndTD2Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount2">QT Most 2 </label>
						<input id="IndTMAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit2">QT Most Unit 2 </label>
						<input id="IndTMUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit2" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount2">QT Low 2 </label>
						<input id="IndTLAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit2">QT Low Unit 2 </label>
						<input id="IndTLUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount2">QT High 2 </label>
						<input id="IndTUAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit2">QT High Unit 2 </label>
						<input id="IndTUUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit2" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression2">Math Expression 2</label>
          <input id="IndMathExpression2" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression2;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression2" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult2">Math Result 2</label>
				  <textarea id="IndMathResult2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult2;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult2" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 3</strong></h4>
        <div>
          <label for="IndName3" class="ui-hidden-accessible">Name 3</label>
          <input id="IndName3" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName3;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName3" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription3">Description 3</label>
				  <textarea id="IndDescription3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription3;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription3" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL3">Indicator 3 URL</label>
          <textarea id="IndURL3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL3;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL3" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel3">Label 3</label>
            <input id="IndLabel3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel3">Rel Label 3 </label>
            <input id="IndRelLabel3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel3;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate3">Date 3</label>
            <input id="IndDate3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate3;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType3">Dist Type 3</label>
            <select class="Select225" id="IndType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType3 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType3 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType3 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType3 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType3 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType3 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType3 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType3 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType3 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType3 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType3 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount3">Q1 3 </label>
						<input id="Ind1Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount3;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit3">Q1 Unit 3 </label>
						<input id="Ind1Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount3">Q2 3 </label>
						<input id="Ind2Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit3">Q2 Unit 3</label>
						<input id="Ind2Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit3;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount3">Q3 3 </label>
						<input id="Ind3Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount3;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit3">Q3 Unit 3 </label>
						<input id="Ind3Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount3">Q4 3 </label>
						<input id="Ind4Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit3">Q4 Unit 3</label>
						<input id="Ind4Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit3;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount3">Q5 3 </label>
						<input id="Ind5Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit3">Q5 Unit 3 </label>
						<input id="Ind5Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator3">Math Operator 3</label>
            <select class="Select225" id="IndMathOperator3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator3 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO3">BaseIO 3</label>
            <select class="Select225" id="IndBaseIO3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO3 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount3">QT 3</label>
						<input id="IndTAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit3">QT Unit 3 </label>
						<input id="IndTUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType3">Math Type 3</label>
            <select class="Select225" id="IndMathType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType3 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType3">Math Sub Type 3</label>
						<input id="IndMathSubType3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType3" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount3">QT D1 3 </label>
						<input id="IndTD1Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit3">QT D1 Unit 3 </label>
						<input id="IndTD1Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount3">QT D2 3 </label>
						<input id="IndTD2Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit3">QT D2 Unit 3 </label>
						<input id="IndTD2Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount3">QT Most 3 </label>
						<input id="IndTMAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit3">QT Most Unit 3 </label>
						<input id="IndTMUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit3" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount3">QT Low 3 </label>
						<input id="IndTLAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit3">QT Low Unit 3 </label>
						<input id="IndTLUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount3">QT High 3 </label>
						<input id="IndTUAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit3">QT High Unit 3 </label>
						<input id="IndTUUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit3" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression3">Math Expression 3</label>
          <input id="IndMathExpression3" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression3;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression3" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult3">Math Result 3</label>
				  <textarea id="IndMathResult3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult3;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult3" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 4</strong></h4>
        <div>
          <label for="IndName4" class="ui-hidden-accessible">Name 4</label>
          <input id="IndName4" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName4;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName4" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription4">Description 4</label>
				  <textarea id="IndDescription4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription4;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription4" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL4">Indicator 4 URL</label>
          <textarea id="IndURL4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL4;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL4" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel4">Label 4</label>
            <input id="IndLabel4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel4;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel4">Rel Label 4 </label>
            <input id="IndRelLabel4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel4;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate4">Date 4</label>
            <input id="IndDate4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate4;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType4">Dist Type 4</label>
            <select class="Select225" id="IndType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType4 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType4 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType4 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType4 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType4 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType4 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType4 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType4 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType4 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType4 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType4 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount4">Q1 4 </label>
						<input id="Ind1Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount4;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit4">Q1 Unit 4 </label>
						<input id="Ind1Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount4">Q2 4</label>
						<input id="Ind2Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit4">Q2 Unit 4</label>
						<input id="Ind2Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit4;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount4">Q3 4 </label>
						<input id="Ind3Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount4;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit4">Q3 Unit 4 </label>
						<input id="Ind3Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount4">Q4 4 </label>
						<input id="Ind4Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit4">Q4 Unit 4</label>
						<input id="Ind4Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit4;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount4">Q5 4</label>
						<input id="Ind5Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit4">Q5 Unit 4</label>
						<input id="Ind5Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator4">Math Operator 4</label>
            <select class="Select225" id="IndMathOperator4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator4 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO4">BaseIO 4</label>
            <select class="Select225" id="IndBaseIO4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO4 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount4">QT 4</label>
						<input id="IndTAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit4">QT Unit 4 </label>
						<input id="IndTUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType4">Math Type 4</label>
            <select class="Select225" id="IndMathType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType4 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType4">Math Sub Type 4</label>
						<input id="IndMathSubType4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType4;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType4" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount4">QT D1 4 </label>
						<input id="IndTD1Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit4">QT D1 Unit 4 </label>
						<input id="IndTD1Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount4">QT D2 4 </label>
						<input id="IndTD2Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit4">QT D2 Unit 4 </label>
						<input id="IndTD2Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount4">QT Most 4 </label>
						<input id="IndTMAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit4">QT Most Unit 4 </label>
						<input id="IndTMUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit4" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount4">QT Low 4 </label>
						<input id="IndTLAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit4">QT Low Unit 4 </label>
						<input id="IndTLUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount4">QT High 4 </label>
						<input id="IndTUAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit4">QT High Unit 4 </label>
						<input id="IndTUUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit4" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression4">Math Expression 4</label>
          <input id="IndMathExpression4" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression4;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression4" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult4">Math Result 4</label>
				  <textarea id="IndMathResult4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult4;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult4" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 5</strong></h4>
        <div>
          <label for="IndName5" class="ui-hidden-accessible">Name 5</label>
          <input id="IndName5" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName5;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName5" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription5">Description 5</label>
				  <textarea id="IndDescription5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription5;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription5" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL5">Indicator 5 URL</label>
          <textarea id="IndURL5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL5;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL5" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel5">Label 5</label>
            <input id="IndLabel5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel5;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel5">Rel Label 5 </label>
            <input id="IndRelLabel5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel5;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate5">Date 5</label>
            <input id="IndDate5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate5;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType5">Dist Type 5</label>
            <select class="Select225" id="IndType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType5 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType5 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType5 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType5 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType5 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType5 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType5 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType5 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType5 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType5 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType5 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount5">Q1 5 </label>
						<input id="Ind1Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit5">Q1 Unit 5 </label>
						<input id="Ind1Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount5">Q2 5</label>
						<input id="Ind2Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit5">Q2 Unit 5</label>
						<input id="Ind2Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit5;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount5">Q3 5 </label>
						<input id="Ind3Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit5">Q3 Unit 5 </label>
						<input id="Ind3Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount5">Q4 5 </label>
						<input id="Ind4Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit5">Q4 Unit 5</label>
						<input id="Ind4Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit5;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount5">Q5 5</label>
						<input id="Ind5Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit5">Q5 Unit 5</label>
						<input id="Ind5Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator5">Math Operator 5</label>
            <select class="Select225" id="IndMathOperator5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator5 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO5">BaseIO 5</label>
            <select class="Select225" id="IndBaseIO5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO5 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount5">QT 5</label>
						<input id="IndTAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit5">QT Unit 5 </label>
						<input id="IndTUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType5">Math Type 5</label>
            <select class="Select225" id="IndMathType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType5 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType5">Math Sub Type 5</label>
						<input id="IndMathSubType5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType5;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType5" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount5">QT D1 5 </label>
						<input id="IndTD1Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit5">QT D1 Unit 5 </label>
						<input id="IndTD1Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount5">QT D2 5 </label>
						<input id="IndTD2Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit5">QT D2 Unit 5 </label>
						<input id="IndTD2Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount5">QT Most 5 </label>
						<input id="IndTMAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit5">QT Most Unit 5 </label>
						<input id="IndTMUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit5" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount5">QT Low 5 </label>
						<input id="IndTLAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit5">QT Low Unit 5 </label>
						<input id="IndTLUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount5">QT High 5 </label>
						<input id="IndTUAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit5">QT High Unit 5 </label>
						<input id="IndTUUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit5" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression5">Math Expression 5</label>
          <input id="IndMathExpression5" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression5;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression5" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult5">Math Result 5</label>
				  <textarea id="IndMathResult5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult5;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult5" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 6</strong></h4>
        <div>
          <label for="IndName6" class="ui-hidden-accessible">Name 6</label>
          <input id="IndName6" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName6;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName6" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription6">Description 6</label>
				  <textarea id="IndDescription6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription6;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription6" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL6">Indicator 6 URL</label>
          <textarea id="IndURL6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL6;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL6" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel6">Label 6</label>
            <input id="IndLabel6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel6;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel6">Rel Label 6 </label>
            <input id="IndRelLabel6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel6;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate6">Date 6</label>
            <input id="IndDate6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate6;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType6">Dist Type 6</label>
            <select class="Select225" id="IndType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType6 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType6 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType6 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType6 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType6 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType6 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType6 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType6 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType6 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType6 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType6 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount6">Q1 6 </label>
						<input id="Ind1Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount6;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit6">Q1 Unit 6 </label>
						<input id="Ind1Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount6">Q2 6</label>
						<input id="Ind2Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit6">Q2 Unit 6</label>
						<input id="Ind2Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit6;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount6">Q3 6 </label>
						<input id="Ind3Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount6;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit6">Q3 Unit 6 </label>
						<input id="Ind3Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount6">Q4 6 </label>
						<input id="Ind4Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit6">Q4 Unit 6</label>
						<input id="Ind4Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit6;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount6">Q5 6</label>
						<input id="Ind5Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit6">Q5 Unit 6</label>
						<input id="Ind5Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator6">Math Operator 6</label>
            <select class="Select225" id="IndMathOperator6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator6 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO6">BaseIO 6</label>
            <select class="Select225" id="IndBaseIO6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO6 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount6">QT 6</label>
						<input id="IndTAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit6">QT Unit 6 </label>
						<input id="IndTUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType6">Math Type 6</label>
            <select class="Select225" id="IndMathType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType6 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType6">Math Sub Type 6</label>
						<input id="IndMathSubType6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType6;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType6" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount6">QT D1 6 </label>
						<input id="IndTD1Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit6">QT D1 Unit 6 </label>
						<input id="IndTD1Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount6">QT D2 6 </label>
						<input id="IndTD2Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit6">QT D2 Unit 6 </label>
						<input id="IndTD2Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount6">QT Most 6 </label>
						<input id="IndTMAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit6">QT Most Unit 6 </label>
						<input id="IndTMUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit6" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount6">QT Low 6 </label>
						<input id="IndTLAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit6">QT Low Unit 6 </label>
						<input id="IndTLUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount6">QT High 6 </label>
						<input id="IndTUAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit6">QT High Unit 6 </label>
						<input id="IndTUUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit6" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression6">Math Expression 6</label>
          <input id="IndMathExpression6" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression6;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression6" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult6">Math Result 6</label>
				  <textarea id="IndMathResult6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult6;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult6" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 7</strong></h4>
        <div>
          <label for="IndName7" class="ui-hidden-accessible">Name 7</label>
          <input id="IndName7" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName7;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName7" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription7">Description 7</label>
				  <textarea id="IndDescription7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription7;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription7" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL7">Indicator 7 URL</label>
          <textarea id="IndURL7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL7;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL7" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel7">Label 7</label>
            <input id="IndLabel7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel7;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel7">Rel Label 7 </label>
            <input id="IndRelLabel7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel7;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate7">Date 7</label>
            <input id="IndDate7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate7;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType7">Dist Type 7</label>
            <select class="Select225" id="IndType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType7 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType7 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType7 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType7 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType7 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType7 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType7 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType7 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType7 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType7 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType7 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount7">Q1 7 </label>
						<input id="Ind1Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount7;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit7">Q1 Unit 7 </label>
						<input id="Ind1Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount7">Q2 7</label>
						<input id="Ind2Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit7">Q2 Unit 7</label>
						<input id="Ind2Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit7;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount7">Q3 7 </label>
						<input id="Ind3Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount7;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit7">Q3 Unit 7 </label>
						<input id="Ind3Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount7">Q4 7 </label>
						<input id="Ind4Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit7">Q4 Unit 7</label>
						<input id="Ind4Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit7;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount7">Q5 7</label>
						<input id="Ind5Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit7">Q5 Unit 7</label>
						<input id="Ind5Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator7">Math Operator 7</label>
            <select class="Select225" id="IndMathOperator7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator7 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO7">BaseIO 7</label>
            <select class="Select225" id="IndBaseIO7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO7 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount7">QT 7</label>
						<input id="IndTAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit7">QT Unit 7 </label>
						<input id="IndTUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType7">Math Type 7</label>
            <select class="Select225" id="IndMathType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType7 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType7">Math Sub Type 7</label>
						<input id="IndMathSubType7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType7;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType7" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount7">QT D1 7 </label>
						<input id="IndTD1Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit7">QT D1 Unit 7 </label>
						<input id="IndTD1Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount7">QT D2 7 </label>
						<input id="IndTD2Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit7">QT D2 Unit 7 </label>
						<input id="IndTD2Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount7">QT Most 7 </label>
						<input id="IndTMAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit7">QT Most Unit 7 </label>
						<input id="IndTMUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit7" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount7">QT Low 7 </label>
						<input id="IndTLAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit7">QT Low Unit 7 </label>
						<input id="IndTLUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount7">QT High 7 </label>
						<input id="IndTUAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit7">QT High Unit 7 </label>
						<input id="IndTUUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit7" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression7">Math Expression 7</label>
          <input id="IndMathExpression7" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression7;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression7" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult7">Math Result 7</label>
				  <textarea id="IndMathResult7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult7;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult7" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 8</strong></h4>
        <div>
          <label for="IndName8" class="ui-hidden-accessible">Name 8</label>
          <input id="IndName8" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName8;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName8" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription8">Description 8</label>
				  <textarea id="IndDescription8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription8;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription8" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL8">Indicator 8 URL</label>
          <textarea id="IndURL8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL8;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL8" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel8">Label 8</label>
            <input id="IndLabel8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel8;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel8">Rel Label 8 </label>
            <input id="IndRelLabel8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel8;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate8">Date 8</label>
            <input id="IndDate8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate8;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType8">Dist Type 8</label>
            <select class="Select225" id="IndType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType8 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType8 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType8 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType8 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType8 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType8 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType8 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType8 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType8 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType8 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType8 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount8">Q1 8 </label>
						<input id="Ind1Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount8;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit8">Q1 Unit 8 </label>
						<input id="Ind1Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount8">Q2 8</label>
						<input id="Ind2Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit8">Q2 Unit 8</label>
						<input id="Ind2Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit8;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount8">Q3 8 </label>
						<input id="Ind3Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount8;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit8">Q3 Unit 8 </label>
						<input id="Ind3Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount8">Q4 8 </label>
						<input id="Ind4Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit8">Q4 Unit 8</label>
						<input id="Ind4Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit8;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount8">Q5 8</label>
						<input id="Ind5Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit8">Q5 Unit 8</label>
						<input id="Ind5Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator8">Math Operator 8</label>
            <select class="Select225" id="IndMathOperator8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator8 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO8">BaseIO 8</label>
            <select class="Select225" id="IndBaseIO8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO8 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount8">QT 8</label>
						<input id="IndTAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit8">QT Unit 8 </label>
						<input id="IndTUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType8">Math Type 8</label>
            <select class="Select225" id="IndMathType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType8 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType8">Math Sub Type 8</label>
						<input id="IndMathSubType8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType8;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType8" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount8">QT D1 8 </label>
						<input id="IndTD1Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit8">QT D1 Unit 8 </label>
						<input id="IndTD1Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount8">QT D2 8 </label>
						<input id="IndTD2Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit8">QT D2 Unit 8 </label>
						<input id="IndTD2Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount8">QT Most 8 </label>
						<input id="IndTMAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit8">QT Most Unit 8 </label>
						<input id="IndTMUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit8" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount8">QT Low 8 </label>
						<input id="IndTLAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit8">QT Low Unit 8 </label>
						<input id="IndTLUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount8">QT High 8 </label>
						<input id="IndTUAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit8">QT High Unit 8 </label>
						<input id="IndTUUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit8" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression8">Math Expression 8</label>
          <input id="IndMathExpression8" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression8;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression8" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult8">Math Result 8</label>
				  <textarea id="IndMathResult8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult8;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult8" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 9</strong></h4>
        <div>
          <label for="IndName9" class="ui-hidden-accessible">Name 9</label>
          <input id="IndName9" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName9;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName9" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription9">Description 9</label>
				  <textarea id="IndDescription9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription9;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription9" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL9">Indicator 9 URL</label>
          <textarea id="IndURL9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL9;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL9" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel9">Label 9</label>
            <input id="IndLabel9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel9;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel9">Rel Label 9 </label>
            <input id="IndRelLabel9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel9;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate9">Date 9</label>
            <input id="IndDate9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate9;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType9">Dist Type 9</label>
            <select class="Select225" id="IndType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType9 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType9 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType9 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType9 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType9 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType9 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType9 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType9 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType9 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType9 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType9 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount9">Q1 9 </label>
						<input id="Ind1Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount9;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit9">Q1 Unit 9 </label>
						<input id="Ind1Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount9">Q2 9</label>
						<input id="Ind2Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit9">Q2 Unit 9</label>
						<input id="Ind2Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit9;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount9">Q3 9 </label>
						<input id="Ind3Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount9;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit9">Q3 Unit 9 </label>
						<input id="Ind3Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount9">Q4 9 </label>
						<input id="Ind4Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit9">Q4 Unit 9</label>
						<input id="Ind4Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit9;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount9">Q5 9</label>
						<input id="Ind5Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit9">Q5 Unit 9</label>
						<input id="Ind5Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator9">Math Operator 9</label>
            <select class="Select225" id="IndMathOperator9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator9 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO9">BaseIO 9</label>
            <select class="Select225" id="IndBaseIO9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO9 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount9">QT 9</label>
						<input id="IndTAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit9">QT Unit 9 </label>
						<input id="IndTUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType9">Math Type 9</label>
            <select class="Select225" id="IndMathType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType9 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType9">Math Sub Type 9</label>
						<input id="IndMathSubType9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType9;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType9" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount9">QT D1 9 </label>
						<input id="IndTD1Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit9">QT D1 Unit 9 </label>
						<input id="IndTD1Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount9">QT D2 9 </label>
						<input id="IndTD2Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit9">QT D2 Unit 9 </label>
						<input id="IndTD2Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount9">QT Most 9 </label>
						<input id="IndTMAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit9">QT Most Unit 9 </label>
						<input id="IndTMUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit9" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount9">QT Low 9 </label>
						<input id="IndTLAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit9">QT Low Unit 9 </label>
						<input id="IndTLUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount9">QT High 9 </label>
						<input id="IndTUAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit9">QT High Unit 9 </label>
						<input id="IndTUUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit9" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression9">Math Expression 9</label>
          <input id="IndMathExpression9" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression9;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression9" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult9">Math Result 9</label>
				  <textarea id="IndMathResult9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult9;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult9" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 10</strong></h4>
        <div>
          <label for="IndName10" class="ui-hidden-accessible">Name 10</label>
          <input id="IndName10" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName10;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName10" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription10">Description 10</label>
				  <textarea id="IndDescription10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription10;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription10" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL10">Indicator 10 URL</label>
          <textarea id="IndURL10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL10;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL10" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel10">Label 10</label>
            <input id="IndLabel10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel10;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel10">Rel Label 10 </label>
            <input id="IndRelLabel10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel10;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate10">Date 10</label>
            <input id="IndDate10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate10;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType10">Dist Type 10</label>
            <select class="Select225" id="IndType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType10 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType10 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType10 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType10 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType10 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType10 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType10 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType10 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType10 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType10 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType10 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount10">Q1 10 </label>
						<input id="Ind1Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount10;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit10">Q1 Unit 10 </label>
						<input id="Ind1Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount10">Q2 10</label>
						<input id="Ind2Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit10">Q2 Unit 10</label>
						<input id="Ind2Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit10;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount10">Q3 10 </label>
						<input id="Ind3Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount10;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit10">Q3 Unit 10 </label>
						<input id="Ind3Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount10">Q4 10 </label>
						<input id="Ind4Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit10">Q4 Unit 10</label>
						<input id="Ind4Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit10;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount10">Q5 10</label>
						<input id="Ind5Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit10">Q5 Unit 10</label>
						<input id="Ind5Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator10">Math Operator 10</label>
            <select class="Select225" id="IndMathOperator10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator10 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO10">BaseIO 10</label>
            <select class="Select225" id="IndBaseIO10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO10 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount10">QT 10</label>
						<input id="IndTAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit10">QT Unit 10 </label>
						<input id="IndTUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType10">Math Type 10</label>
            <select class="Select225" id="IndMathType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType10 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType10">Math Sub Type 10</label>
						<input id="IndMathSubType10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType10;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType10" /></xsl:attribute>
						</input>
          </div>
         <div class="ui-block-a">
            <label for="IndTD1Amount10">QT D1 10 </label>
						<input id="IndTD1Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit10">QT D1 Unit 10 </label>
						<input id="IndTD1Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount10">QT D2 10 </label>
						<input id="IndTD2Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit10">QT D2 Unit 10 </label>
						<input id="IndTD2Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount10">QT Most 10 </label>
						<input id="IndTMAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit10">QT Most Unit 10 </label>
						<input id="IndTMUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit10" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount10">QT Low 10 </label>
						<input id="IndTLAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit10">QT Low Unit 10 </label>
						<input id="IndTLUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount10">QT High 10 </label>
						<input id="IndTUAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit10">QT High Unit 10 </label>
						<input id="IndTUUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit10" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression10">Math Expression 10</label>
          <input id="IndMathExpression10" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression10;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression10" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult10">Math Result 10</label>
				  <textarea id="IndMathResult10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult10;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult10" />
				  </textarea>
			  </div>
      </div>
    </div>
		<div id="divsteptwo">
      <h4 class="ui-bar-b"><strong>Step 2 of 3. Enter M and E Indicators</strong></h4>
		  <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
        <xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
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
      <h4 class="ui-bar-b"><strong>More M and E Indicators</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 11</strong></h4>
        <div>
          <label for="IndName11" class="ui-hidden-accessible">Name 11</label>
          <input id="IndName11" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName11;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName11" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription11">Indicator 11 Description</label>
				  <textarea id="IndDescription11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription11;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription11" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL11">Indicator 11 URL</label>
          <textarea id="IndURL11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL11;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL11" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel11">Label 11</label>
            <input id="IndLabel11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel11;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel11">Rel Label 11 </label>
            <input id="IndRelLabel11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel11;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate11">Date 11 </label>
            <input id="IndDate11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate11;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType11">Dist Type 11</label>
            <select class="Select225" id="IndType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType11 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType11 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType11 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType11 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType11 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType11 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType11 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType11 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType11 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType11 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType11 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount11">Q1 11 </label>
						<input id="Ind1Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount11;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit11">Q1 Unit 11 </label>
						<input id="Ind1Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount11">Q2 11 </label>
						<input id="Ind2Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit11">Q2 Unit 11</label>
						<input id="Ind2Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit11;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount11">Q3 11 </label>
						<input id="Ind3Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount11;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit11">Q3 Unit 11 </label>
						<input id="Ind3Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount11">Q4 11 </label>
						<input id="Ind4Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit11">Q4 Unit 11</label>
						<input id="Ind4Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit11;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount11">Q5 11 </label>
						<input id="Ind5Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit11">Q5 Unit 11 </label>
						<input id="Ind5Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator11">Math Operator 11</label>
            <select class="Select225" id="IndMathOperator11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator11 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO11">BaseIO 11</label>
            <select class="Select225" id="IndBaseIO11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO11 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount11">QT 11</label>
						<input id="IndTAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit11">QT Unit 11 </label>
						<input id="IndTUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType11">Math Type 11</label>
            <select class="Select225" id="IndMathType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType11">Math Sub Type 11</label>
						<input id="IndMathSubType11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType11;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType11" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount11">QT D1 11 </label>
						<input id="IndTD1Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit11">QT D1 Unit 11 </label>
						<input id="IndTD1Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount11">QT D2 11 </label>
						<input id="IndTD2Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit11">QT D2 Unit 11 </label>
						<input id="IndTD2Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount11">QT Most 11 </label>
						<input id="IndTMAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit11">QT Most Unit 11 </label>
						<input id="IndTMUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit11" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount11">QT Low 11 </label>
						<input id="IndTLAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit11">QT Low Unit 11 </label>
						<input id="IndTLUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount11">QT High 11 </label>
						<input id="IndTUAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit11">QT High Unit 11 </label>
						<input id="IndTUUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit11" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression11">Math Expression 11</label>
          <input id="IndMathExpression11" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression11;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression11" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult11">Math Result 11</label>
				  <textarea id="IndMathResult11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult11;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult11" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 12</strong></h4>
        <div>
          <label for="IndName12" class="ui-hidden-accessible">Name 12</label>
          <input id="IndName12" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName12;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName12" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription12">Indicator 12 Description</label>
				  <textarea id="IndDescription12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription12;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription12" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL12">Indicator 12 URL</label>
          <textarea id="IndURL12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL12;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL12" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel12">Label 12</label>
            <input id="IndLabel12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel12;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel12">Rel Label 12 </label>
            <input id="IndRelLabel12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel12;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate12">Date 12 </label>
            <input id="IndDate12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate12;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType12">Dist Type 12</label>
            <select class="Select225" id="IndType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType12 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType12 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType12 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType12 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType12 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType12 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType12 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType12 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType12 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType12 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType12 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount12">Q1 12 </label>
						<input id="Ind1Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount12;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit12">Q1 Unit 12 </label>
						<input id="Ind1Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount12">Q2 12 </label>
						<input id="Ind2Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit12">Q2 Unit 12</label>
						<input id="Ind2Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit12;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount12">Q3 12 </label>
						<input id="Ind3Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount12;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit12">Q3 Unit 12 </label>
						<input id="Ind3Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount12">Q4 12 </label>
						<input id="Ind4Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit12">Q4 Unit 12</label>
						<input id="Ind4Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit12;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount12">Q5 12 </label>
						<input id="Ind5Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit12">Q5 Unit 12 </label>
						<input id="Ind5Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator12">Math Operator 12</label>
            <select class="Select225" id="IndMathOperator12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator12 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO12">BaseIO 12</label>
            <select class="Select225" id="IndBaseIO12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO12 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount12">QT 12</label>
						<input id="IndTAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit12">QT Unit 12 </label>
						<input id="IndTUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType12">Math Type 12</label>
            <select class="Select225" id="IndMathType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType12">Math Sub Type 12</label>
						<input id="IndMathSubType12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType12;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType12" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount12">QT D1 12 </label>
						<input id="IndTD1Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit12">QT D1 Unit 12 </label>
						<input id="IndTD1Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount12">QT D2 12 </label>
						<input id="IndTD2Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit12">QT D2 Unit 12 </label>
						<input id="IndTD2Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount12">QT Most 12 </label>
						<input id="IndTMAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit12">QT Most Unit 12 </label>
						<input id="IndTMUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit12" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount12">QT Low 12 </label>
						<input id="IndTLAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit12">QT Low Unit 12 </label>
						<input id="IndTLUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount12">QT High 12 </label>
						<input id="IndTUAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit12">QT High Unit 12 </label>
						<input id="IndTUUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit12" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression12">Math Expression 12</label>
          <input id="IndMathExpression12" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression12;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression12" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult12">Math Result 12</label>
				  <textarea id="IndMathResult12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult12;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult12" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 13</strong></h4>
        <div>
          <label for="IndName13" class="ui-hidden-accessible">Name 13</label>
          <input id="IndName13" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName13;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName13" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription13">Description 13</label>
				  <textarea id="IndDescription13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription13;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription13" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL13">Indicator 13 URL</label>
          <textarea id="IndURL13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL13;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL13" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel13">Label 13</label>
            <input id="IndLabel13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel13;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel13">Rel Label 13 </label>
            <input id="IndRelLabel13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel13;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate13">Date 13</label>
            <input id="IndDate13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate13;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType13">Dist Type 13</label>
            <select class="Select225" id="IndType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType13 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType13 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType13 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType13 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType13 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType13 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType13 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType13 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType13 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType13 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType13 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount13">Q1 13 </label>
						<input id="Ind1Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount13;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit13">Q1 Unit 13 </label>
						<input id="Ind1Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount13">Q2 1 </label>
						<input id="Ind2Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit13">Q2 Unit 13</label>
						<input id="Ind2Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit13;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount13">Q3 13 </label>
						<input id="Ind3Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount13;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit13">Q3 Unit 13 </label>
						<input id="Ind3Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount13">Q4 13 </label>
						<input id="Ind4Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit13">Q4 Unit 13</label>
						<input id="Ind4Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit13;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount13">Q5 13 </label>
						<input id="Ind5Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit13">Q5 Unit 13 </label>
						<input id="Ind5Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator13">Math Operator 13</label>
            <select class="Select225" id="IndMathOperator13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator13 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO13">BaseIO 13</label>
            <select class="Select225" id="IndBaseIO13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO13 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount13">QT 13</label>
						<input id="IndTAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit13">QT Unit 13 </label>
						<input id="IndTUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType13">Math Type 13</label>
            <select class="Select225" id="IndMathType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType13 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType13">Math Sub Type 13</label>
						<input id="IndMathSubType13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType13;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType13" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount13">QT D1 13 </label>
						<input id="IndTD1Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit13">QT D1 Unit 13 </label>
						<input id="IndTD1Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount13">QT D2 13 </label>
						<input id="IndTD2Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit13">QT D2 Unit 13 </label>
						<input id="IndTD2Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount13">QT Most 13 </label>
						<input id="IndTMAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit13">QT Most Unit 13 </label>
						<input id="IndTMUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit13" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount13">QT Low 13 </label>
						<input id="IndTLAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit13">QT Low Unit 13 </label>
						<input id="IndTLUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount13">QT High 13 </label>
						<input id="IndTUAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit13">QT High Unit 13 </label>
						<input id="IndTUUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit13" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression13">Math Expression 13</label>
          <input id="IndMathExpression13" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression13;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression13" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult13">Math Result 13</label>
				  <textarea id="IndMathResult13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult13;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult13" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 14</strong></h4>
        <div>
          <label for="IndName14" class="ui-hidden-accessible">Name 14</label>
          <input id="IndName14" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName14;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName14" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription14">Description 14</label>
				  <textarea id="IndDescription14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription14;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription14" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL14">Indicator 14 URL</label>
          <textarea id="IndURL14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL14;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL14" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel14">Label 14</label>
            <input id="IndLabel14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel14;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel14">Rel Label 14 </label>
            <input id="IndRelLabel14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel14;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate14">Date 14</label>
            <input id="IndDate14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate14;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType14">Dist Type 14</label>
            <select class="Select225" id="IndType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType14 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType14 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType14 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType14 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType14 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType14 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType14 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType14 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType14 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType14 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType14 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount14">Q1 14 </label>
						<input id="Ind1Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount14;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit14">Q1 Unit 14 </label>
						<input id="Ind1Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount14">Q2 14</label>
						<input id="Ind2Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit14">Q2 Unit 14</label>
						<input id="Ind2Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit14;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount14">Q3 14 </label>
						<input id="Ind3Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount14;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit14">Q3 Unit 14 </label>
						<input id="Ind3Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount14">Q4 14 </label>
						<input id="Ind4Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit14">Q4 Unit 14</label>
						<input id="Ind4Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit14;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount14">Q5 14</label>
						<input id="Ind5Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit14">Q5 Unit 14</label>
						<input id="Ind5Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator14">Math Operator 14</label>
            <select class="Select225" id="IndMathOperator14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator14 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO14">BaseIO 14</label>
            <select class="Select225" id="IndBaseIO14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO14 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount14">QT 14</label>
						<input id="IndTAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit14">QT Unit 14 </label>
						<input id="IndTUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType14">Math Type 14</label>
            <select class="Select225" id="IndMathType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType14 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType14">Math Sub Type 14</label>
						<input id="IndMathSubType14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType14;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType14" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount14">QT D1 14 </label>
						<input id="IndTD1Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit14">QT D1 Unit 14 </label>
						<input id="IndTD1Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount14">QT D2 14 </label>
						<input id="IndTD2Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit14">QT D2 Unit 14 </label>
						<input id="IndTD2Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount14">QT Most 14 </label>
						<input id="IndTMAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit14">QT Most Unit 14 </label>
						<input id="IndTMUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit14" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount14">QT Low 14 </label>
						<input id="IndTLAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit14">QT Low Unit 14 </label>
						<input id="IndTLUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount14">QT High 14 </label>
						<input id="IndTUAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit14">QT High Unit 14 </label>
						<input id="IndTUUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit14" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression14">Math Expression 14</label>
          <input id="IndMathExpression14" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression14;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression14" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult14">Math Result 14</label>
				  <textarea id="IndMathResult14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult14;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult14" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 15</strong></h4>
        <div>
          <label for="IndName15" class="ui-hidden-accessible">Name 15</label>
          <input id="IndName15" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName15;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName15" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription15">Description 15</label>
				  <textarea id="IndDescription15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription15;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription15" />
				  </textarea>
			  </div>
        <div>
          <label for="IndURL15">Indicator 15 URL</label>
          <textarea id="IndURL15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL15;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@IndURL15" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel15">Label 15</label>
            <input id="IndLabel15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel15;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel15">Rel Label 15 </label>
            <input id="IndRelLabel15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel15;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate15">Date 15</label>
            <input id="IndDate15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate15;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType15">Dist Type 15</label>
            <select class="Select225" id="IndType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType15 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType15 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType15 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType15 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType15 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType15 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType15 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType15 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType15 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType15 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType15 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1Amount15">Q1 15 </label>
						<input id="Ind1Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Amount15;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit15">Q1 Unit 15 </label>
						<input id="Ind1Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2Amount15">Q2 15</label>
						<input id="Ind2Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit15">Q2 Unit 15</label>
						<input id="Ind2Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit15;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind3Amount15">Q3 15 </label>
						<input id="Ind3Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Amount15;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind3Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind3Unit15">Q3 Unit 15 </label>
						<input id="Ind3Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind3Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind3Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind4Amount15">Q4 15 </label>
						<input id="Ind4Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind4Unit15">Q4 Unit 15</label>
						<input id="Ind4Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind4Unit15;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind4Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind5Amount15">Q5 15</label>
						<input id="Ind5Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind5Unit15">Q5 Unit 15</label>
						<input id="Ind5Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind5Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind5Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="IndMathOperator15">Math Operator 15</label>
            <select class="Select225" id="IndMathOperator15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator15 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="IndBaseIO15">BaseIO 15</label>
            <select class="Select225" id="IndBaseIO15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO15 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTAmount15">QT 15</label>
						<input id="IndTAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit15">QT Unit 15 </label>
						<input id="IndTUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType15">Math Type 15</label>
            <select class="Select225" id="IndMathType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType15 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType15">Math Sub Type 15</label>
						<input id="IndMathSubType15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType15;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType15" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTD1Amount15">QT D1 15 </label>
						<input id="IndTD1Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit15">QT D1 Unit 15 </label>
						<input id="IndTD1Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount15">QT D2 15 </label>
						<input id="IndTD2Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit15">QT D2 Unit 15 </label>
						<input id="IndTD2Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount15">QT Most 15 </label>
						<input id="IndTMAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit15">QT Most Unit 15 </label>
						<input id="IndTMUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit15" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="IndTLAmount15">QT Low 15 </label>
						<input id="IndTLAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit15">QT Low Unit 15 </label>
						<input id="IndTLUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount15">QT High 15 </label>
						<input id="IndTUAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit15">QT High Unit 15 </label>
						<input id="IndTUUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit15" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="IndMathExpression15">Math Expression 15</label>
          <input id="IndMathExpression15" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression15;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndMathExpression15" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="IndMathResult15">Math Result 15</label>
				  <textarea id="IndMathResult15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult15;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndMathResult15" />
				  </textarea>
			  </div>
      </div>
      <xsl:value-of select="DisplayDevPacks:WriteAlternatives($searchurl, $viewEditType,
            @AlternativeType, @TargetType)"/>
      <h4 class="ui-bar-b"><strong>Score</strong></h4>
        <div>
          <label for="IndName0" class="ui-hidden-accessible">Name</label>
          <input id="IndName0" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName0;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName0" /></xsl:attribute>
          </input>
        </div>
      <div>
        <label for="IndMathExpression0">Score Math Expression</label>
        <textarea id="IndMathExpression0" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathExpression0;string;250</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@IndMathExpression0" />
			  </textarea>
      </div>
      <div class="ui-grid-a">
         <div class="ui-block-a">
            <label for="IndLabel0">Label</label>
            <input id="IndLabel0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel0;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel0" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndRelLabel0">Rel Label</label>
            <input id="IndRelLabel0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRelLabel0;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRelLabel0" /></xsl:attribute>
            </input>
          </div>
         <div class="ui-block-a">
            <label for="IndTAmount0">Total Score</label>
				    <input id="IndTAmount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTAmount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTAmount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUnit0">Score Unit</label>
				    <input id="IndTUnit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUnit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUnit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD1Amount0">D1</label>
				    <input id="IndTD1Amount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Amount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Amount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD1Unit0">D1 Unit</label>
				    <input id="IndTD1Unit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD1Unit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD1Unit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndTD2Amount0">D2</label>
				    <input id="IndTD2Amount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Amount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Amount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTD2Unit0">D2 Unit</label>
				    <input id="IndTD2Unit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTD2Unit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTD2Unit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndDate0">Date</label>
            <input id="IndDate0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate0;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate0" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndType0">Dist Type</label>
            <select class="Select225" id="IndType0" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndType0;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndType0 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@IndType0 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@IndType0 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@IndType0 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@IndType0 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@IndType0 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@IndType0 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@IndType0 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@IndType0 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@IndType0 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@IndType0 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@IndType0 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndIterations0">Iterations</label>
				    <input id="IndIterations0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndIterations0;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndIterations0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndCILevel0">Confidence Interval</label>
				    <input id="IndCILevel0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndCILevel0;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndCILevel0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndRandom0">Random Seed</label>
				    <input id="IndRandom0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndRandom0;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndRandom0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndBaseIO0">BaseIO</label>
            <select class="Select225" id="IndBaseIO0" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndBaseIO0;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@IndBaseIO0 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathOperator0">Math Operator</label>
            <select class="Select225" id="IndMathOperator0" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathOperator0;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@IndMathOperator0 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndTMAmount0">Most Likely</label>
				    <input id="IndTMAmount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMAmount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMAmount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTMUnit0">Most Unit</label>
				    <input id="IndTMUnit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTMUnit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTMUnit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndTLAmount0">Low Estimate</label>
				    <input id="IndTLAmount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLAmount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLAmount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTLUnit0">Low Unit</label>
				    <input id="IndTLUnit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTLUnit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTLUnit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndTUAmount0">High Estimate</label>
				    <input id="IndTUAmount0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUAmount0;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUAmount0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="IndTUUnit0">High Unit</label>
				    <input id="IndTUUnit0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTUUnit0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTUUnit0" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="IndMathType0">Math Type</label>
            <select class="Select225" id="IndMathType0" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType0;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@IndMathType0 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="IndMathSubType0">Math Sub Type</label>
            <input id="IndMathSubType0" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathSubType0;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndMathSubType0" /></xsl:attribute>
				    </input>
          </div>
       </div>
      <div>
        <label for="IndMathResult0">Math Result</label>
			  <textarea id="IndMathResult0" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathResult0;string;2000</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@IndMathResult0" />
			  </textarea>
      </div>
      <div>
        <label for="IndURL0">Score URL</label>
			  <textarea id="IndURL0" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndURL0;string;1500</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@IndURL0" />
			  </textarea>
      </div>
      <div>
				<label for="CalculatorDescription">Calculations Description</label>
				<textarea id="CalculatorDescription" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorDescription;string;255</xsl:attribute>
          </xsl:if>
					<xsl:value-of select="@CalculatorDescription" />
				</textarea>
			</div>
      <div >
				<label for="lblMediaURL">Media URL</label>
				<textarea id="lblMediaURL" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MediaURL;string;600</xsl:attribute>
          </xsl:if>
					<xsl:value-of select="@MediaURL" />
				</textarea>
			</div>
      <div>
        <label for="DataURL">Data URL</label>
			  <textarea id="DataURL" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;DataURL;string;1500</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@DataURL" />
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
					<xsl:variable name="calcParams4a">'&amp;step=stepfour&amp;savemethod=calcs<xsl:value-of select="$calcParams" />'</xsl:variable>
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
		</div>
		<div id="divsteplast">
			<h4 class="ui-bar-b"><strong>Instructions</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 1</h4>
			<ul data-role="listview">
				<li><strong>Step 1. Indicators:</strong> Enter up to 10 indicators.</li>
				<li><strong>Step 1. Indicator Name and Description:</strong> Name and description for each indicator.</li>
				<li><strong>Step 1. Indicator Date:</strong> Make sure that the benchmark, targets, actual, indicators have distinct dates.</li>
        <li><strong>Step 1. Distribution Type:</strong> The numeric distribution of QT. Refer to the Stock Calculation 1 reference.</li>
        <li><strong>Step 1. Math Expression:</strong>A mathematical expression containing one or more of the Q1 to Q5 variables and/or sibling indicator Q1 to QTM variables. Use strings that identify both the indicator (I1, I2, … In) and the Qx property (Q … QTM), with a period separator between them. Examples include:((I1.Q1 + I1.Q2) * I1.Q3) + I1.Q4)) - (2 * I1.Q5)</li>
        <li><strong>Step 1. Math Operator Type:</strong> Mathematical operation to use with QT. MathTypes include: equalto, lessthan, greaterthan, lessthanorequalto, and greaterthanorequalto. Refer to the Stock Calculation 1 reference for the algorithms.</li>
        <li><strong>Step 1. Math Type and Math Sub Type:</strong> Mathematical algorithm and subalgorithm to use with Distribution Type, QT, QTD1, and QTD2 to solve for QTM, QTL, and QTU. Refer to the Stock Calculation 1 reference for the algorithms.</li>
        <li><strong>Step 1. QT Amount and Unit:</strong> The Unit must be manually entered. The Amount will be the result of the mathematical calculation.</li>
        <li><strong>Step 1. QTD1 Amount and Unit:</strong> First distribution, or shape, parameter for QT.</li>
        <li><strong>Step 1. QTD2 Amount and Unit:</strong> Second distribution, or scale, for QT.</li>
        <li><strong>Step 1. BaseIO:</strong> Base input or output property to update with this indicator's QTM property. </li>
        <li><strong>Step 1. QTM Amount and Unit:</strong> Most Likely Estimate for QT. The Unit must be manually entered. The Amount will be the result of the mathematical algorithm.</li>
        <li><strong>Step 1. QTL Amount and Unit:</strong> Low Estimate or QT. The Unit must be manually entered. The Amount will be the result of the mathematical algorithm.</li>
        <li><strong>Step 1. QTU Amount and Unit:</strong> High Estimate for QT. The Unit must be manually entered. The Amount will be the result of the mathematical algorithm.</li>
        <li><strong>Step 1. Math Result:</strong> TEXT string holding results of calculation.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
        <li><strong>Step 2. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 2. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 2. Indicators:</strong> Enter up to 5 indicators.</li>
        <li><strong>Step 2. Target Type:</strong> Used with Progress analyzers to identify benchmark and actual indicators.</li>
        <li><strong>Step 2. Altern Type:</strong> Used with Change by Alternative analyzers to identify alternatives to compare.</li>
        <li><strong>Step 2. Score Math Expression:</strong> A mathematical expression containing one or more of the children indicator Q1 to QTM variables. Use strings that identify both the indicator (I1, I2, … In) and the Qx property (Q … QTM), with a period separator between them. Examples include:((I1.QTM + I2.QTM) * I3.Q3) + I4.QTM)) - (2 * I5.QTM)</li>
        <li><strong>Step 2. Score Total and Unit:</strong> The Unit must be manually entered. The Amount will be the result of the Math Expression calculation.</li>
        <li><strong>Step 2. D1 Amount and Unit:</strong> First distribution variable for Score.</li>
        <li><strong>Step 2. D2 Amount and Unit:</strong> Second distribution for Score.</li>
        <li><strong>Step 2. Distribution Type:</strong> The numeric distribution of Score. Refer to the Stock Calculation 1 reference.</li>
        <li><strong>Step 2. Math Type and Math Sub Type:</strong> Mathematical algorithm and subalgorithm to use with Distribution Type, Score, ScoreD1, and ScoreD2 to solve for ScoreM, ScoreL, and ScoreU. Refer to the Stock Calculation 1 reference for the algorithms.</li>
        <li><strong>Step 2. Most Likely, Low, High, Amounts and Units:</strong> Results of Distribution Type and Math Type calculations.</li>
        <li><strong>Step 2. Iterations:</strong> Number of iterations to use when drawing random number samples for some algorithms.  </li>
        <li><strong>Step 2. Confidence Interval:</strong> Level of confidence interval to use when reporting all Score and Indicator high and low amounts. Should be an integer such as 95, 90, or 40.</li>
        <li><strong>Step 2. Random Seed:</strong> Any positive integer, except 0, will result in the same set of random variables being used each time a calculation is run.</li>
        <li><strong>Step 2. BaseIO:</strong> Base input or output property to update with the Score Most Likely property. </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Refer to the M and E Introduction and Resource Stock Calculation references.</strong></li>
			</ul>
      </div>
		</div>
		</xsl:if>
</xsl:template>
</xsl:stylesheet>
  