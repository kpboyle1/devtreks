<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2015, November -->
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"  encoding="UTF-8"/>
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
        <xsl:when test="(contains($docToCalcNodeName, 'input')
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This input calculator does not appear appropriate for the document being calculated. Are you 
					sure this is the right calculator?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>Stock Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool tracks resource stock indicators for input uris. Up to 15 new 
          indicators can be added for each input. 
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
     <h4 class="ui-bar-b"><strong>Stock Indicators</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 1</strong></h4>
        <div>
          <label for="SB1Name1" class="ui-hidden-accessible">Name 1</label>
          <input id="SB1Name1" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name1;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name1" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description1">Indicator 1 Description</label>
				  <textarea id="SB1Description1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description1;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description1" />
				  </textarea>
			  </div>
        <div >
          <label for="SB1URL1">Indicator 1 URL</label>
          <textarea id="SB1URL1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL1;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL1" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label1">Label 1 </label>
            <input id="SB1Label1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel1">Rel Label 1 </label>
            <input id="SB1RelLabel1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel1;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date1">Date 1 </label>
            <input id="SB1Date1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date1;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type1">Dist Type 1</label>
            <select class="Select225" id="SB1Type1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type1 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount1">Q1 1 </label>
						<input id="SB11Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount1;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit1">Q1 Unit 1 </label>
						<input id="SB11Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount1">Q2 1 </label>
						<input id="SB12Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit1">Q2 Unit 1</label>
						<input id="SB12Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit1;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount1">Q3 1 </label>
						<input id="SB13Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount1;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit1">Q3 Unit 1 </label>
						<input id="SB13Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount1">Q4 1 </label>
						<input id="SB14Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit1">Q4 Unit 1</label>
						<input id="SB14Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit1;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount1">Q5 1 </label>
						<input id="SB15Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit1">Q5 Unit 1 </label>
						<input id="SB15Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator1">Math Operator 1</label>
            <select class="Select225" id="SB1MathOperator1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator1 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO1">BaseIO 1</label>
            <select class="Select225" id="SB1BaseIO1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO1 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount1">QT 1 </label>
						<input id="SB1TAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit1">QT Unit 1 </label>
						<input id="SB1TUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType1">Math Type 1</label>
            <select class="Select225" id="SB1MathType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType1 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType1">Math Sub Type 1</label>
						<input id="SB1MathSubType1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD1Amount1">QT D1 1 </label>
						<input id="SB1TD1Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit1">QT D1 Unit 1 </label>
						<input id="SB1TD1Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount1">QT D2 1 </label>
						<input id="SB1TD2Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit1">QT D2 Unit 1 </label>
						<input id="SB1TD2Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount1">QT Most 1 </label>
						<input id="SB1TMAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit1">QT Most Unit 1 </label>
						<input id="SB1TMUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit1" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount1">QT Low 1 </label>
						<input id="SB1TLAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit1">QT Low Unit 1 </label>
						<input id="SB1TLUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount1">QT High 1 </label>
						<input id="SB1TUAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit1">QT High Unit 1 </label>
						<input id="SB1TUUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit1" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression1">Math Expression 1</label>
          <input id="SB1MathExpression1" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression1;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression1" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult1">Math Result 1</label>
				  <textarea id="SB1MathResult1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult1;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult1" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 2</strong></h4>
        <div>
          <label for="SB1Name2" class="ui-hidden-accessible">Name 2</label>
          <input id="SB1Name2" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name2;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name2" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description2">Indicator 2 Description</label>
				  <textarea id="SB1Description2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description2;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description2" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL2">Indicator 2 URL</label>
          <textarea id="SB1URL2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL2;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL2" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label2">Label 2</label>
            <input id="SB1Label2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel2">Rel Label 2 </label>
            <input id="SB1RelLabel2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel2;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date2">Date 2 </label>
            <input id="SB1Date2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date2;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type2">Dist Type 2</label>
            <select class="Select225" id="SB1Type2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type2 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount2">Q1 2 </label>
						<input id="SB11Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount2;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit2">Q1 Unit 2 </label>
						<input id="SB11Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount2">Q2 2 </label>
						<input id="SB12Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit2">Q2 Unit 2</label>
						<input id="SB12Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit2;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount2">Q3 2 </label>
						<input id="SB13Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount2;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit2">Q3 Unit 2 </label>
						<input id="SB13Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount2">Q4 2 </label>
						<input id="SB14Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit2">Q4 Unit 2</label>
						<input id="SB14Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit2;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount2">Q5 2 </label>
						<input id="SB15Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit2">Q5 Unit 2 </label>
						<input id="SB15Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathOperator2">Math Operator 2</label>
            <select class="Select225" id="SB1MathOperator2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator2 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO2">BaseIO 2</label>
            <select class="Select225" id="SB1BaseIO2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO2 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount2">QT 2 </label>
						<input id="SB1TAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit2">QT Unit 2</label>
						<input id="SB1TUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType2">Math Type 2</label>
            <select class="Select225" id="SB1MathType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType2;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType2 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType2">Math Sub Type 2</label>
						<input id="SB1MathSubType2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType2" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount2">QT D1 2 </label>
						<input id="SB1TD1Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit2">QT D1 Unit 2 </label>
						<input id="SB1TD1Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount2">QT D2 2 </label>
						<input id="SB1TD2Amount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit2">QT D2 Unit 2 </label>
						<input id="SB1TD2Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount2">QT Most 2 </label>
						<input id="SB1TMAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit2">QT Most Unit 2 </label>
						<input id="SB1TMUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit2" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount2">QT Low 2 </label>
						<input id="SB1TLAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit2">QT Low Unit 2 </label>
						<input id="SB1TLUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount2">QT High 2 </label>
						<input id="SB1TUAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit2">QT High Unit 2 </label>
						<input id="SB1TUUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit2" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression2">Math Expression 2</label>
          <input id="SB1MathExpression2" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression2;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression2" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult2">Math Result 2</label>
				  <textarea id="SB1MathResult2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult2;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult2" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 3</strong></h4>
        <div>
          <label for="SB1Name3" class="ui-hidden-accessible">Name 3</label>
          <input id="SB1Name3" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name3;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name3" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description3">Description 3</label>
				  <textarea id="SB1Description3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description3;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description3" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL3">Indicator 3 URL</label>
          <textarea id="SB1URL3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL3;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL3" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label3">Label 3</label>
            <input id="SB1Label3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel3">Rel Label 3 </label>
            <input id="SB1RelLabel3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel3;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date3">Date 3</label>
            <input id="SB1Date3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date3;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type3">Dist Type 3</label>
            <select class="Select225" id="SB1Type3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type3 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount3">Q1 3 </label>
						<input id="SB11Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount3;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit3">Q1 Unit 3 </label>
						<input id="SB11Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount3">Q2 3 </label>
						<input id="SB12Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit3">Q2 Unit 3</label>
						<input id="SB12Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit3;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount3">Q3 3 </label>
						<input id="SB13Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount3;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit3">Q3 Unit 3 </label>
						<input id="SB13Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount3">Q4 3 </label>
						<input id="SB14Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit3">Q4 Unit 3</label>
						<input id="SB14Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit3;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount3">Q5 3 </label>
						<input id="SB15Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit3">Q5 Unit 3 </label>
						<input id="SB15Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator3">Math Operator 3</label>
            <select class="Select225" id="SB1MathOperator3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator3 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO3">BaseIO 3</label>
            <select class="Select225" id="SB1BaseIO3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO3 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount3">QT 3</label>
						<input id="SB1TAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit3">QT Unit 3 </label>
						<input id="SB1TUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType3">Math Type 3</label>
            <select class="Select225" id="SB1MathType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType3;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType3 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType3">Math Sub Type 3</label>
						<input id="SB1MathSubType3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType3" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount3">QT D1 3 </label>
						<input id="SB1TD1Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit3">QT D1 Unit 3 </label>
						<input id="SB1TD1Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount3">QT D2 3 </label>
						<input id="SB1TD2Amount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit3">QT D2 Unit 3 </label>
						<input id="SB1TD2Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount3">QT Most 3 </label>
						<input id="SB1TMAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit3">QT Most Unit 3 </label>
						<input id="SB1TMUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit3" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount3">QT Low 3 </label>
						<input id="SB1TLAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit3">QT Low Unit 3 </label>
						<input id="SB1TLUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount3">QT High 3 </label>
						<input id="SB1TUAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit3">QT High Unit 3 </label>
						<input id="SB1TUUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit3" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression3">Math Expression 3</label>
          <input id="SB1MathExpression3" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression3;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression3" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult3">Math Result 3</label>
				  <textarea id="SB1MathResult3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult3;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult3" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 4</strong></h4>
        <div>
          <label for="SB1Name4" class="ui-hidden-accessible">Name 4</label>
          <input id="SB1Name4" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name4;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name4" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description4">Description 4</label>
				  <textarea id="SB1Description4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description4;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description4" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL4">Indicator 4 URL</label>
          <textarea id="SB1URL4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL4;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL4" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label4">Label 4</label>
            <input id="SB1Label4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label4;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel4">Rel Label 4 </label>
            <input id="SB1RelLabel4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel4;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date4">Date 4</label>
            <input id="SB1Date4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date4;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type4">Dist Type 4</label>
            <select class="Select225" id="SB1Type4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type4 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount4">Q1 4 </label>
						<input id="SB11Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount4;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit4">Q1 Unit 4 </label>
						<input id="SB11Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount4">Q2 4</label>
						<input id="SB12Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit4">Q2 Unit 4</label>
						<input id="SB12Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit4;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount4">Q3 4 </label>
						<input id="SB13Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount4;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit4">Q3 Unit 4 </label>
						<input id="SB13Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount4">Q4 4 </label>
						<input id="SB14Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit4">Q4 Unit 4</label>
						<input id="SB14Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit4;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount4">Q5 4</label>
						<input id="SB15Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit4">Q5 Unit 4</label>
						<input id="SB15Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator4">Math Operator 4</label>
            <select class="Select225" id="SB1MathOperator4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator4 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO4">BaseIO 4</label>
            <select class="Select225" id="SB1BaseIO4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO4 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount4">QT 4</label>
						<input id="SB1TAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit4">QT Unit 4 </label>
						<input id="SB1TUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType4">Math Type 4</label>
            <select class="Select225" id="SB1MathType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType4;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType4 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType4">Math Sub Type 4</label>
						<input id="SB1MathSubType4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType4;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType4" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount4">QT D1 4 </label>
						<input id="SB1TD1Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit4">QT D1 Unit 4 </label>
						<input id="SB1TD1Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount4">QT D2 4 </label>
						<input id="SB1TD2Amount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit4">QT D2 Unit 4 </label>
						<input id="SB1TD2Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount4">QT Most 4 </label>
						<input id="SB1TMAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit4">QT Most Unit 4 </label>
						<input id="SB1TMUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit4" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount4">QT Low 4 </label>
						<input id="SB1TLAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit4">QT Low Unit 4 </label>
						<input id="SB1TLUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount4">QT High 4 </label>
						<input id="SB1TUAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit4">QT High Unit 4 </label>
						<input id="SB1TUUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit4" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression4">Math Expression 4</label>
          <input id="SB1MathExpression4" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression4;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression4" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult4">Math Result 4</label>
				  <textarea id="SB1MathResult4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult4;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult4" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 5</strong></h4>
        <div>
          <label for="SB1Name5" class="ui-hidden-accessible">Name 5</label>
          <input id="SB1Name5" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name5;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name5" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description5">Description 5</label>
				  <textarea id="SB1Description5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description5;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description5" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL5">Indicator 5 URL</label>
          <textarea id="SB1URL5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL5;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL5" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label5">Label 5</label>
            <input id="SB1Label5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label5;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel5">Rel Label 5 </label>
            <input id="SB1RelLabel5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel5;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date5">Date 5</label>
            <input id="SB1Date5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date5;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type5">Dist Type 5</label>
            <select class="Select225" id="SB1Type5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type5 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount5">Q1 5 </label>
						<input id="SB11Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit5">Q1 Unit 5 </label>
						<input id="SB11Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount5">Q2 5</label>
						<input id="SB12Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit5">Q2 Unit 5</label>
						<input id="SB12Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit5;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount5">Q3 5 </label>
						<input id="SB13Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit5">Q3 Unit 5 </label>
						<input id="SB13Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount5">Q4 5 </label>
						<input id="SB14Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit5">Q4 Unit 5</label>
						<input id="SB14Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit5;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount5">Q5 5</label>
						<input id="SB15Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit5">Q5 Unit 5</label>
						<input id="SB15Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator5">Math Operator 5</label>
            <select class="Select225" id="SB1MathOperator5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator5 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO5">BaseIO 5</label>
            <select class="Select225" id="SB1BaseIO5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO5 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount5">QT 5</label>
						<input id="SB1TAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit5">QT Unit 5 </label>
						<input id="SB1TUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType5">Math Type 5</label>
            <select class="Select225" id="SB1MathType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType5;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType5 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType5">Math Sub Type 5</label>
						<input id="SB1MathSubType5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType5;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType5" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount5">QT D1 5 </label>
						<input id="SB1TD1Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit5">QT D1 Unit 5 </label>
						<input id="SB1TD1Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount5">QT D2 5 </label>
						<input id="SB1TD2Amount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit5">QT D2 Unit 5 </label>
						<input id="SB1TD2Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount5">QT Most 5 </label>
						<input id="SB1TMAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit5">QT Most Unit 5 </label>
						<input id="SB1TMUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit5" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount5">QT Low 5 </label>
						<input id="SB1TLAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit5">QT Low Unit 5 </label>
						<input id="SB1TLUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount5">QT High 5 </label>
						<input id="SB1TUAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit5">QT High Unit 5 </label>
						<input id="SB1TUUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit5" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression5">Math Expression 5</label>
          <input id="SB1MathExpression5" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression5;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression5" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult5">Math Result 5</label>
				  <textarea id="SB1MathResult5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult5;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult5" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 6</strong></h4>
        <div>
          <label for="SB1Name6" class="ui-hidden-accessible">Name 6</label>
          <input id="SB1Name6" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name6;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name6" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description6">Description 6</label>
				  <textarea id="SB1Description6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description6;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description6" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL6">Indicator 6 URL</label>
          <textarea id="SB1URL6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL6;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL6" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label6">Label 6</label>
            <input id="SB1Label6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label6;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel6">Rel Label 6 </label>
            <input id="SB1RelLabel6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel6;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date6">Date 6</label>
            <input id="SB1Date6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date6;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type6">Dist Type 6</label>
            <select class="Select225" id="SB1Type6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type6 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount6">Q1 6 </label>
						<input id="SB11Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount6;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit6">Q1 Unit 6 </label>
						<input id="SB11Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount6">Q2 6</label>
						<input id="SB12Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit6">Q2 Unit 6</label>
						<input id="SB12Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit6;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount6">Q3 6 </label>
						<input id="SB13Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount6;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit6">Q3 Unit 6 </label>
						<input id="SB13Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount6">Q4 6 </label>
						<input id="SB14Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit6">Q4 Unit 6</label>
						<input id="SB14Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit6;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount6">Q5 6</label>
						<input id="SB15Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit6">Q5 Unit 6</label>
						<input id="SB15Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator6">Math Operator 6</label>
            <select class="Select225" id="SB1MathOperator6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator6 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO6">BaseIO 6</label>
            <select class="Select225" id="SB1BaseIO6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO6 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount6">QT 6</label>
						<input id="SB1TAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit6">QT Unit 6 </label>
						<input id="SB1TUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType6">Math Type 6</label>
            <select class="Select225" id="SB1MathType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType6;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType6 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType6">Math Sub Type 6</label>
						<input id="SB1MathSubType6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType6;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType6" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount6">QT D1 6 </label>
						<input id="SB1TD1Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit6">QT D1 Unit 6 </label>
						<input id="SB1TD1Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount6">QT D2 6 </label>
						<input id="SB1TD2Amount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit6">QT D2 Unit 6 </label>
						<input id="SB1TD2Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount6">QT Most 6 </label>
						<input id="SB1TMAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit6">QT Most Unit 6 </label>
						<input id="SB1TMUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit6" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount6">QT Low 6 </label>
						<input id="SB1TLAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit6">QT Low Unit 6 </label>
						<input id="SB1TLUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount6">QT High 6 </label>
						<input id="SB1TUAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit6">QT High Unit 6 </label>
						<input id="SB1TUUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit6" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression6">Math Expression 6</label>
          <input id="SB1MathExpression6" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression6;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression6" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult6">Math Result 6</label>
				  <textarea id="SB1MathResult6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult6;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult6" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 7</strong></h4>
        <div>
          <label for="SB1Name7" class="ui-hidden-accessible">Name 7</label>
          <input id="SB1Name7" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name7;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name7" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description7">Description 7</label>
				  <textarea id="SB1Description7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description7;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description7" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL7">Indicator 7 URL</label>
          <textarea id="SB1URL7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL7;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL7" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label7">Label 7</label>
            <input id="SB1Label7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label7;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel7">Rel Label 7 </label>
            <input id="SB1RelLabel7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel7;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date7">Date 7</label>
            <input id="SB1Date7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date7;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type7">Dist Type 7</label>
            <select class="Select225" id="SB1Type7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type7 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount7">Q1 7 </label>
						<input id="SB11Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount7;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit7">Q1 Unit 7 </label>
						<input id="SB11Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount7">Q2 7</label>
						<input id="SB12Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit7">Q2 Unit 7</label>
						<input id="SB12Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit7;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount7">Q3 7 </label>
						<input id="SB13Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount7;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit7">Q3 Unit 7 </label>
						<input id="SB13Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount7">Q4 7 </label>
						<input id="SB14Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit7">Q4 Unit 7</label>
						<input id="SB14Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit7;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount7">Q5 7</label>
						<input id="SB15Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit7">Q5 Unit 7</label>
						<input id="SB15Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator7">Math Operator 7</label>
            <select class="Select225" id="SB1MathOperator7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator7 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO7">BaseIO 7</label>
            <select class="Select225" id="SB1BaseIO7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO7 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount7">QT 7</label>
						<input id="SB1TAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit7">QT Unit 7 </label>
						<input id="SB1TUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType7">Math Type 7</label>
            <select class="Select225" id="SB1MathType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType7;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType7 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType7">Math Sub Type 7</label>
						<input id="SB1MathSubType7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType7;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType7" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount7">QT D1 7 </label>
						<input id="SB1TD1Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit7">QT D1 Unit 7 </label>
						<input id="SB1TD1Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount7">QT D2 7 </label>
						<input id="SB1TD2Amount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit7">QT D2 Unit 7 </label>
						<input id="SB1TD2Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount7">QT Most 7 </label>
						<input id="SB1TMAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit7">QT Most Unit 7 </label>
						<input id="SB1TMUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit7" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount7">QT Low 7 </label>
						<input id="SB1TLAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit7">QT Low Unit 7 </label>
						<input id="SB1TLUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount7">QT High 7 </label>
						<input id="SB1TUAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit7">QT High Unit 7 </label>
						<input id="SB1TUUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit7" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression7">Math Expression 7</label>
          <input id="SB1MathExpression7" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression7;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression7" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult7">Math Result 7</label>
				  <textarea id="SB1MathResult7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult7;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult7" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 8</strong></h4>
        <div>
          <label for="SB1Name8" class="ui-hidden-accessible">Name 8</label>
          <input id="SB1Name8" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name8;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name8" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description8">Description 8</label>
				  <textarea id="SB1Description8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description8;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description8" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL8">Indicator 8 URL</label>
          <textarea id="SB1URL8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL8;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL8" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label8">Label 8</label>
            <input id="SB1Label8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label8;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel8">Rel Label 8 </label>
            <input id="SB1RelLabel8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel8;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date8">Date 8</label>
            <input id="SB1Date8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date8;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type8">Dist Type 8</label>
            <select class="Select225" id="SB1Type8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type8 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount8">Q1 8 </label>
						<input id="SB11Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount8;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit8">Q1 Unit 8 </label>
						<input id="SB11Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount8">Q2 8</label>
						<input id="SB12Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit8">Q2 Unit 8</label>
						<input id="SB12Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit8;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount8">Q3 8 </label>
						<input id="SB13Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount8;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit8">Q3 Unit 8 </label>
						<input id="SB13Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount8">Q4 8 </label>
						<input id="SB14Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit8">Q4 Unit 8</label>
						<input id="SB14Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit8;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount8">Q5 8</label>
						<input id="SB15Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit8">Q5 Unit 8</label>
						<input id="SB15Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator8">Math Operator 8</label>
            <select class="Select225" id="SB1MathOperator8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator8 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO8">BaseIO 8</label>
            <select class="Select225" id="SB1BaseIO8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO8 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount8">QT 8</label>
						<input id="SB1TAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit8">QT Unit 8 </label>
						<input id="SB1TUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType8">Math Type 8</label>
            <select class="Select225" id="SB1MathType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType8;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType8 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType8">Math Sub Type 8</label>
						<input id="SB1MathSubType8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType8;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType8" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount8">QT D1 8 </label>
						<input id="SB1TD1Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit8">QT D1 Unit 8 </label>
						<input id="SB1TD1Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount8">QT D2 8 </label>
						<input id="SB1TD2Amount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit8">QT D2 Unit 8 </label>
						<input id="SB1TD2Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount8">QT Most 8 </label>
						<input id="SB1TMAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit8">QT Most Unit 8 </label>
						<input id="SB1TMUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit8" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount8">QT Low 8 </label>
						<input id="SB1TLAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit8">QT Low Unit 8 </label>
						<input id="SB1TLUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount8">QT High 8 </label>
						<input id="SB1TUAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit8">QT High Unit 8 </label>
						<input id="SB1TUUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit8" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression8">Math Expression 8</label>
          <input id="SB1MathExpression8" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression8;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression8" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult8">Math Result 8</label>
				  <textarea id="SB1MathResult8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult8;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult8" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 9</strong></h4>
        <div>
          <label for="SB1Name9" class="ui-hidden-accessible">Name 9</label>
          <input id="SB1Name9" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name9;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name9" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description9">Description 9</label>
				  <textarea id="SB1Description9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description9;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description9" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL9">Indicator 9 URL</label>
          <textarea id="SB1URL9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL9;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL9" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label9">Label 9</label>
            <input id="SB1Label9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label9;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel9">Rel Label 9 </label>
            <input id="SB1RelLabel9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel9;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date9">Date 9</label>
            <input id="SB1Date9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date9;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type9">Dist Type 9</label>
            <select class="Select225" id="SB1Type9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type9 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount9">Q1 9 </label>
						<input id="SB11Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount9;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit9">Q1 Unit 9 </label>
						<input id="SB11Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount9">Q2 9</label>
						<input id="SB12Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit9">Q2 Unit 9</label>
						<input id="SB12Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit9;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount9">Q3 9 </label>
						<input id="SB13Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount9;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit9">Q3 Unit 9 </label>
						<input id="SB13Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount9">Q4 9 </label>
						<input id="SB14Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit9">Q4 Unit 9</label>
						<input id="SB14Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit9;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount9">Q5 9</label>
						<input id="SB15Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit9">Q5 Unit 9</label>
						<input id="SB15Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator9">Math Operator 9</label>
            <select class="Select225" id="SB1MathOperator9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator9 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO9">BaseIO 9</label>
            <select class="Select225" id="SB1BaseIO9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO9 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount9">QT 9</label>
						<input id="SB1TAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit9">QT Unit 9 </label>
						<input id="SB1TUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType9">Math Type 9</label>
            <select class="Select225" id="SB1MathType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType9;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType9 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType9">Math Sub Type 9</label>
						<input id="SB1MathSubType9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType9;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType9" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount9">QT D1 9 </label>
						<input id="SB1TD1Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit9">QT D1 Unit 9 </label>
						<input id="SB1TD1Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount9">QT D2 9 </label>
						<input id="SB1TD2Amount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit9">QT D2 Unit 9 </label>
						<input id="SB1TD2Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount9">QT Most 9 </label>
						<input id="SB1TMAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit9">QT Most Unit 9 </label>
						<input id="SB1TMUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit9" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount9">QT Low 9 </label>
						<input id="SB1TLAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit9">QT Low Unit 9 </label>
						<input id="SB1TLUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount9">QT High 9 </label>
						<input id="SB1TUAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit9">QT High Unit 9 </label>
						<input id="SB1TUUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit9" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression9">Math Expression 9</label>
          <input id="SB1MathExpression9" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression9;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression9" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult9">Math Result 9</label>
				  <textarea id="SB1MathResult9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult9;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult9" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 10</strong></h4>
        <div>
          <label for="SB1Name10" class="ui-hidden-accessible">Name 10</label>
          <input id="SB1Name10" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name10;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name10" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description10">Description 10</label>
				  <textarea id="SB1Description10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description10;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description10" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL10">Indicator 10 URL</label>
          <textarea id="SB1URL10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL10;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL10" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label10">Label 10</label>
            <input id="SB1Label10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label10;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel10">Rel Label 10 </label>
            <input id="SB1RelLabel10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel10;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date10">Date 10</label>
            <input id="SB1Date10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date10;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type10">Dist Type 10</label>
            <select class="Select225" id="SB1Type10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type10 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount10">Q1 10 </label>
						<input id="SB11Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount10;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit10">Q1 Unit 10 </label>
						<input id="SB11Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount10">Q2 10</label>
						<input id="SB12Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit10">Q2 Unit 10</label>
						<input id="SB12Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit10;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount10">Q3 10 </label>
						<input id="SB13Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount10;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit10">Q3 Unit 10 </label>
						<input id="SB13Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount10">Q4 10 </label>
						<input id="SB14Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit10">Q4 Unit 10</label>
						<input id="SB14Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit10;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount10">Q5 10</label>
						<input id="SB15Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit10">Q5 Unit 10</label>
						<input id="SB15Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator10">Math Operator 10</label>
            <select class="Select225" id="SB1MathOperator10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator10 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO10">BaseIO 10</label>
            <select class="Select225" id="SB1BaseIO10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO10 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount10">QT 10</label>
						<input id="SB1TAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit10">QT Unit 10 </label>
						<input id="SB1TUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType10">Math Type 10</label>
            <select class="Select225" id="SB1MathType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType10;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType10 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType10">Math Sub Type 10</label>
						<input id="SB1MathSubType10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType10;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType10" /></xsl:attribute>
						</input>
          </div>
         <div class="ui-block-a">
            <label for="SB1TD1Amount10">QT D1 10 </label>
						<input id="SB1TD1Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit10">QT D1 Unit 10 </label>
						<input id="SB1TD1Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount10">QT D2 10 </label>
						<input id="SB1TD2Amount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit10">QT D2 Unit 10 </label>
						<input id="SB1TD2Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount10">QT Most 10 </label>
						<input id="SB1TMAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit10">QT Most Unit 10 </label>
						<input id="SB1TMUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit10" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount10">QT Low 10 </label>
						<input id="SB1TLAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit10">QT Low Unit 10 </label>
						<input id="SB1TLUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount10">QT High 10 </label>
						<input id="SB1TUAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit10">QT High Unit 10 </label>
						<input id="SB1TUUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit10" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression10">Math Expression 10</label>
          <input id="SB1MathExpression10" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression10;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression10" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult10">Math Result 10</label>
				  <textarea id="SB1MathResult10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult10;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult10" />
				  </textarea>
			  </div>
      </div>
    </div>
		<div id="divsteptwo">
      <h4 class="ui-bar-b"><strong>Step 2 of 3. Enter Stock Indicators</strong></h4>
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
      <h4 class="ui-bar-b"><strong>More Stock Indicators</strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 11</strong></h4>
        <div>
          <label for="SB1Name11" class="ui-hidden-accessible">Name 11</label>
          <input id="SB1Name11" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name11;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name11" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description11">Indicator 11 Description</label>
				  <textarea id="SB1Description11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description11;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description11" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL11">Indicator 11 URL</label>
          <textarea id="SB1URL11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL11;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL11" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label11">Label 11</label>
            <input id="SB1Label11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label11;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel11">Rel Label 11 </label>
            <input id="SB1RelLabel11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel11;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date11">Date 11 </label>
            <input id="SB1Date11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date11;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type11">Dist Type 11</label>
            <select class="Select225" id="SB1Type11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type11 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount11">Q1 11 </label>
						<input id="SB11Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount11;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit11">Q1 Unit 11 </label>
						<input id="SB11Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount11">Q2 11 </label>
						<input id="SB12Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit11">Q2 Unit 11</label>
						<input id="SB12Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit11;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount11">Q3 11 </label>
						<input id="SB13Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount11;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit11">Q3 Unit 11 </label>
						<input id="SB13Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount11">Q4 11 </label>
						<input id="SB14Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit11">Q4 Unit 11</label>
						<input id="SB14Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit11;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount11">Q5 11 </label>
						<input id="SB15Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit11">Q5 Unit 11 </label>
						<input id="SB15Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator11">Math Operator 11</label>
            <select class="Select225" id="SB1MathOperator11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator11 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO11">BaseIO 11</label>
            <select class="Select225" id="SB1BaseIO11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO11 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount11">QT 11</label>
						<input id="SB1TAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit11">QT Unit 11 </label>
						<input id="SB1TUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType11">Math Type 11</label>
            <select class="Select225" id="SB1MathType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType11;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType11 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType11">Math Sub Type 11</label>
						<input id="SB1MathSubType11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType11;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType11" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount11">QT D1 11 </label>
						<input id="SB1TD1Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit11">QT D1 Unit 11 </label>
						<input id="SB1TD1Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount11">QT D2 11 </label>
						<input id="SB1TD2Amount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit11">QT D2 Unit 11 </label>
						<input id="SB1TD2Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount11">QT Most 11 </label>
						<input id="SB1TMAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit11">QT Most Unit 11 </label>
						<input id="SB1TMUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit11" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount11">QT Low 11 </label>
						<input id="SB1TLAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit11">QT Low Unit 11 </label>
						<input id="SB1TLUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount11">QT High 11 </label>
						<input id="SB1TUAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit11">QT High Unit 11 </label>
						<input id="SB1TUUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit11" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression11">Math Expression 11</label>
          <input id="SB1MathExpression11" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression11;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression11" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult11">Math Result 11</label>
				  <textarea id="SB1MathResult11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult11;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult11" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 12</strong></h4>
        <div>
          <label for="SB1Name12" class="ui-hidden-accessible">Name 12</label>
          <input id="SB1Name12" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name12;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name12" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description12">Indicator 12 Description</label>
				  <textarea id="SB1Description12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description12;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description12" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL12">Indicator 12 URL</label>
          <textarea id="SB1URL12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL12;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL12" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label12">Label 12</label>
            <input id="SB1Label12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label12;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel12">Rel Label 12 </label>
            <input id="SB1RelLabel12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel12;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date12">Date 12 </label>
            <input id="SB1Date12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date12;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type12">Dist Type 12</label>
            <select class="Select225" id="SB1Type12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type12 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount12">Q1 12 </label>
						<input id="SB11Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount12;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit12">Q1 Unit 12 </label>
						<input id="SB11Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount12">Q2 12 </label>
						<input id="SB12Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit12">Q2 Unit 12</label>
						<input id="SB12Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit12;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount12">Q3 12 </label>
						<input id="SB13Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount12;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit12">Q3 Unit 12 </label>
						<input id="SB13Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount12">Q4 12 </label>
						<input id="SB14Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit12">Q4 Unit 12</label>
						<input id="SB14Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit12;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount12">Q5 12 </label>
						<input id="SB15Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit12">Q5 Unit 12 </label>
						<input id="SB15Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator12">Math Operator 12</label>
            <select class="Select225" id="SB1MathOperator12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator12 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO12">BaseIO 12</label>
            <select class="Select225" id="SB1BaseIO12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO12 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount12">QT 12</label>
						<input id="SB1TAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit12">QT Unit 12 </label>
						<input id="SB1TUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType12">Math Type 12</label>
            <select class="Select225" id="SB1MathType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType12;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType12 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType12">Math Sub Type 12</label>
						<input id="SB1MathSubType12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType12;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType12" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount12">QT D1 12 </label>
						<input id="SB1TD1Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit12">QT D1 Unit 12 </label>
						<input id="SB1TD1Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount12">QT D2 12 </label>
						<input id="SB1TD2Amount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit12">QT D2 Unit 12 </label>
						<input id="SB1TD2Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount12">QT Most 12 </label>
						<input id="SB1TMAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit12">QT Most Unit 12 </label>
						<input id="SB1TMUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit12" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount12">QT Low 12 </label>
						<input id="SB1TLAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit12">QT Low Unit 12 </label>
						<input id="SB1TLUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount12">QT High 12 </label>
						<input id="SB1TUAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit12">QT High Unit 12 </label>
						<input id="SB1TUUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit12" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression12">Math Expression 12</label>
          <input id="SB1MathExpression12" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression12;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression12" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult12">Math Result 12</label>
				  <textarea id="SB1MathResult12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult12;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult12" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 13</strong></h4>
        <div>
          <label for="SB1Name13" class="ui-hidden-accessible">Name 13</label>
          <input id="SB1Name13" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name13;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name13" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description13">Description 13</label>
				  <textarea id="SB1Description13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description13;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description13" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL13">Indicator 13 URL</label>
          <textarea id="SB1URL13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL13;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL13" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label13">Label 13</label>
            <input id="SB1Label13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label13;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel13">Rel Label 13 </label>
            <input id="SB1RelLabel13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel13;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date13">Date 13</label>
            <input id="SB1Date13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date13;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type13">Dist Type 13</label>
            <select class="Select225" id="SB1Type13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type13 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount13">Q1 13 </label>
						<input id="SB11Amount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount13;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit13">Q1 Unit 13 </label>
						<input id="SB11Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount13">Q2 1 </label>
						<input id="SB12Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit13">Q2 Unit 13</label>
						<input id="SB12Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit13;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount13">Q3 13 </label>
						<input id="SB13Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount13;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit13">Q3 Unit 13 </label>
						<input id="SB13Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount13">Q4 13 </label>
						<input id="SB14Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit13">Q4 Unit 13</label>
						<input id="SB14Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit13;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount13">Q5 13 </label>
						<input id="SB15Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit13">Q5 Unit 13 </label>
						<input id="SB15Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator13">Math Operator 13</label>
            <select class="Select225" id="SB1MathOperator13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator13 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO13">BaseIO 13</label>
            <select class="Select225" id="SB1BaseIO13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO13 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount13">QT 13</label>
						<input id="SB1TAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit13">QT Unit 13 </label>
						<input id="SB1TUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType13">Math Type 13</label>
            <select class="Select225" id="SB1MathType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType13;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType13 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType13">Math Sub Type 13</label>
						<input id="SB1MathSubType13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType13;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType13" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount13">QT D1 13 </label>
						<input id="SB1TD1Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit13">QT D1 Unit 13 </label>
						<input id="SB1TD1Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount13">QT D2 13 </label>
						<input id="SB1TD2Amount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit13">QT D2 Unit 13 </label>
						<input id="SB1TD2Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount13">QT Most 13 </label>
						<input id="SB1TMAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit13">QT Most Unit 13 </label>
						<input id="SB1TMUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit13" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount13">QT Low 13 </label>
						<input id="SB1TLAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit13">QT Low Unit 13 </label>
						<input id="SB1TLUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount13">QT High 13 </label>
						<input id="SB1TUAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit13">QT High Unit 13 </label>
						<input id="SB1TUUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit13" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression13">Math Expression 13</label>
          <input id="SB1MathExpression13" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression13;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression13" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult13">Math Result 13</label>
				  <textarea id="SB1MathResult13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult13;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult13" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 14</strong></h4>
        <div>
          <label for="SB1Name14" class="ui-hidden-accessible">Name 14</label>
          <input id="SB1Name14" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name14;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name14" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description14">Description 14</label>
				  <textarea id="SB1Description14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description14;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description14" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL14">Indicator 14 URL</label>
          <textarea id="SB1URL14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL14;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL14" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label14">Label 14</label>
            <input id="SB1Label14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label14;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel14">Rel Label 14 </label>
            <input id="SB1RelLabel14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel14;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date14">Date 14</label>
            <input id="SB1Date14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date14;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type14">Dist Type 14</label>
            <select class="Select225" id="SB1Type14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type14 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount14">Q1 14 </label>
						<input id="SB11Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount14;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit14">Q1 Unit 14 </label>
						<input id="SB11Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount14">Q2 14</label>
						<input id="SB12Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit14">Q2 Unit 14</label>
						<input id="SB12Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit14;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount14">Q3 14 </label>
						<input id="SB13Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount14;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit14">Q3 Unit 14 </label>
						<input id="SB13Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount14">Q4 14 </label>
						<input id="SB14Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit14">Q4 Unit 14</label>
						<input id="SB14Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit14;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount14">Q5 14</label>
						<input id="SB15Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit14">Q5 Unit 14</label>
						<input id="SB15Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator14">Math Operator 14</label>
            <select class="Select225" id="SB1MathOperator14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator14 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO14">BaseIO 14</label>
            <select class="Select225" id="SB1BaseIO14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO14 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount14">QT 14</label>
						<input id="SB1TAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit14">QT Unit 14 </label>
						<input id="SB1TUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType14">Math Type 14</label>
            <select class="Select225" id="SB1MathType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType14;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType14 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType14">Math Sub Type 14</label>
						<input id="SB1MathSubType14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType14;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType14" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount14">QT D1 14 </label>
						<input id="SB1TD1Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit14">QT D1 Unit 14 </label>
						<input id="SB1TD1Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount14">QT D2 14 </label>
						<input id="SB1TD2Amount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit14">QT D2 Unit 14 </label>
						<input id="SB1TD2Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount14">QT Most 14 </label>
						<input id="SB1TMAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit14">QT Most Unit 14 </label>
						<input id="SB1TMUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit14" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount14">QT Low 14 </label>
						<input id="SB1TLAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit14">QT Low Unit 14 </label>
						<input id="SB1TLUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount14">QT High 14 </label>
						<input id="SB1TUAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit14">QT High Unit 14 </label>
						<input id="SB1TUUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit14" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression14">Math Expression 14</label>
          <input id="SB1MathExpression14" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression14;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression14" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult14">Math Result 14</label>
				  <textarea id="SB1MathResult14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult14;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult14" />
				  </textarea>
			  </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 15</strong></h4>
        <div>
          <label for="SB1Name15" class="ui-hidden-accessible">Name 15</label>
          <input id="SB1Name15" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Name15;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1Name15" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="SB1Description15">Description 15</label>
				  <textarea id="SB1Description15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Description15;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1Description15" />
				  </textarea>
			  </div>
        <div>
          <label for="SB1URL15">Indicator 15 URL</label>
          <textarea id="SB1URL15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1URL15;string;500</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="@SB1URL15" />
          </textarea>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="SB1Label15">Label 15</label>
            <input id="SB1Label15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Label15;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Label15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1RelLabel15">Rel Label 15 </label>
            <input id="SB1RelLabel15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1RelLabel15;string;50</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1RelLabel15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1Date15">Date 15</label>
            <input id="SB1Date15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Date15;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Date15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Type15">Dist Type 15</label>
            <select class="Select225" id="SB1Type15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Type15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1Type15 = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB11Amount15">Q1 15 </label>
						<input id="SB11Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Amount15;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB11Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB11Unit15">Q1 Unit 15 </label>
						<input id="SB11Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB11Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB11Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB12Amount15">Q2 15</label>
						<input id="SB12Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB12Unit15">Q2 Unit 15</label>
						<input id="SB12Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB12Unit15;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB12Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB13Amount15">Q3 15 </label>
						<input id="SB13Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Amount15;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@SB13Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB13Unit15">Q3 Unit 15 </label>
						<input id="SB13Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB13Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB13Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB14Amount15">Q4 15 </label>
						<input id="SB14Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB14Unit15">Q4 Unit 15</label>
						<input id="SB14Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB14Unit15;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB14Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB15Amount15">Q5 15</label>
						<input id="SB15Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB15Unit15">Q5 Unit 15</label>
						<input id="SB15Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB15Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB15Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
				    <label for="SB1MathOperator15">Math Operator 15</label>
            <select class="Select225" id="SB1MathOperator15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathOperator15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">equalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'equalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>equalto
              </option>
              <option>
                <xsl:attribute name="value">lessthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'lessthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthan
              </option>
              <option>
                <xsl:attribute name="value">greaterthan</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'greaterthan')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthan
              </option>
              <option>
                <xsl:attribute name="value">lessthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'lessthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>lessthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">greaterthanorequalto</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'greaterthanorequalto')">
                  <xsl:attribute name="selected" />
                </xsl:if>greaterthanorequalto
              </option>
              <option>
                <xsl:attribute name="value">specific</xsl:attribute>
                <xsl:if test="(@SB1MathOperator15 = 'specific')">
                  <xsl:attribute name="selected" />
                </xsl:if>specific
              </option>
            </select>
			    </div>
          <div class="ui-block-b">
            <label for="SB1BaseIO15">BaseIO 15</label>
            <select class="Select225" id="SB1BaseIO15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO15 = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="SB1TAmount15">QT 15</label>
						<input id="SB1TAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUnit15">QT Unit 15 </label>
						<input id="SB1TUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUnit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1MathType15">Math Type 15</label>
            <select class="Select225" id="SB1MathType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathType15;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1MathType15 = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1MathSubType15">Math Sub Type 15</label>
						<input id="SB1MathSubType15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathSubType15;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1MathSubType15" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TD1Amount15">QT D1 15 </label>
						<input id="SB1TD1Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD1Unit15">QT D1 Unit 15 </label>
						<input id="SB1TD1Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD1Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD1Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TD2Amount15">QT D2 15 </label>
						<input id="SB1TD2Amount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Amount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Amount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TD2Unit15">QT D2 Unit 15 </label>
						<input id="SB1TD2Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TD2Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TD2Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TMAmount15">QT Most 15 </label>
						<input id="SB1TMAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TMUnit15">QT Most Unit 15 </label>
						<input id="SB1TMUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TMUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TMUnit15" /></xsl:attribute>
						</input>
          </div>
        <div class="ui-block-a">
            <label for="SB1TLAmount15">QT Low 15 </label>
						<input id="SB1TLAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TLUnit15">QT Low Unit 15 </label>
						<input id="SB1TLUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TLUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TLUnit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="SB1TUAmount15">QT High 15 </label>
						<input id="SB1TUAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="SB1TUUnit15">QT High Unit 15 </label>
						<input id="SB1TUUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1TUUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1TUUnit15" /></xsl:attribute>
						</input>
          </div>
        </div>
        <div>
          <label for="SB1MathExpression15">Math Expression 15</label>
          <input id="SB1MathExpression15" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathExpression15;string;250</xsl:attribute>
						</xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@SB1MathExpression15" /></xsl:attribute>
					</input>
        </div>
        <div>
				  <label for="SB1MathResult15">Math Result 15</label>
				  <textarea id="SB1MathResult15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1MathResult15;string;2000</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@SB1MathResult15" />
				  </textarea>
			  </div>
      </div>
      <xsl:value-of select="DisplayDevPacks:WriteAlternatives($searchurl, $viewEditType,
            @AlternativeType, @TargetType)"/>
      <div>
        <label for="SB1ScoreMathExpression">Score Math Expression</label>
        <textarea id="SB1ScoreMathExpression" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreMathExpression;string;250</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@SB1ScoreMathExpression" />
			  </textarea>
      </div>
      <div class="ui-grid-a">
         <div class="ui-block-a">
            <label for="SB1Score">Score</label>
				    <input id="SB1Score" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Score;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Score" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreUnit">Score Unit</label>
				    <input id="SB1ScoreUnit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreUnit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreUnit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreD1Amount">Score D1</label>
				    <input id="SB1ScoreD1Amount" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreD1Amount;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreD1Amount" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreD1Unit">Score D1 Unit</label>
				    <input id="SB1ScoreD1Unit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreD1Unit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreD1Unit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreD2Amount">Score D2</label>
				    <input id="SB1ScoreD2Amount" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreD2Amount;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreD2Amount" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreD2Unit">Score D2 Unit</label>
				    <input id="SB1ScoreD2Unit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreD2Unit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreD2Unit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreDistType">Score Dist Type</label>
            <select class="Select225" id="SB1ScoreDistType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreDistType;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">normal</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'normal')">
                  <xsl:attribute name="selected" />
                </xsl:if>normal
              </option>
              <option>
                <xsl:attribute name="value">triangle</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'triangle')">
                  <xsl:attribute name="selected" />
                </xsl:if>triangle
              </option>
              <option>
                <xsl:attribute name="value">uniform</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'uniform')">
                  <xsl:attribute name="selected" />
                </xsl:if>uniform
              </option>
              <option>
                <xsl:attribute name="value">bernoulli</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'bernoulli')">
                  <xsl:attribute name="bernoulli" />
                </xsl:if>bernoulli
              </option>
              <option>
                <xsl:attribute name="value">beta</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'beta')">
                  <xsl:attribute name="selected" />
                </xsl:if>beta
              </option>
              <option>
                <xsl:attribute name="value">lognormal</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'lognormal')">
                  <xsl:attribute name="selected" />
                </xsl:if>lognormal
              </option>
              <option>
                <xsl:attribute name="value">weibull</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'weibull')">
                  <xsl:attribute name="selected" />
                </xsl:if>weibull
              </option>
              <option>
                <xsl:attribute name="value">poisson</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'poisson')">
                  <xsl:attribute name="selected" />
                </xsl:if>poisson
              </option>
              <option>
                <xsl:attribute name="value">binomial</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'binomial')">
                  <xsl:attribute name="selected" />
                </xsl:if>binomial
              </option>
              <option>
                <xsl:attribute name="value">pareto</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'pareto')">
                  <xsl:attribute name="selected" />
                </xsl:if>pareto
              </option>
              <option>
                <xsl:attribute name="value">gamma</xsl:attribute>
                <xsl:if test="(@SB1ScoreDistType = 'gamma')">
                  <xsl:attribute name="selected" />
                </xsl:if>gamma
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1Iterations">Iterations</label>
				    <input id="SB1Iterations" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Iterations;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Iterations" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1CILevel">Confidence Interval</label>
				    <input id="SB1CILevel" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1CILevel;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1CILevel" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1Random">Random Seed</label>
				    <input id="SB1Random" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1Random;integer;4</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1Random" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1BaseIO">Score BaseIO</label>
            <select class="Select225" id="SB1BaseIO" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1BaseIO;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">quantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'quantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>quantity
              </option>
              <option>
                <xsl:attribute name="value">times</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'times')">
                  <xsl:attribute name="selected" />
                </xsl:if>times
              </option>
              <option>
                <xsl:attribute name="value">ocprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'ocprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>ocprice
              </option>
              <option>
                <xsl:attribute name="value">aohprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'aohprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>aohprice
              </option>
              <option>
                <xsl:attribute name="value">capprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'capprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>capprice
              </option>
              <option>
                <xsl:attribute name="value">benprice</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'benprice')">
                  <xsl:attribute name="selected" />
                </xsl:if>benprice
              </option>
              <option>
                <xsl:attribute name="value">composquantity</xsl:attribute>
                <xsl:if test="(@SB1BaseIO = 'composquantity')">
                  <xsl:attribute name="selected" />
                </xsl:if>composquantity
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreM">Score Most Likely</label>
				    <input id="SB1ScoreM" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreM;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreM" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreMUnit">Score Most Unit</label>
				    <input id="SB1ScoreMUnit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreMUnit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreMUnit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreLAmount">Score Low Estimate</label>
				    <input id="SB1ScoreLAmount" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreLAmount;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreLAmount" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreLUnit">Score Low Unit</label>
				    <input id="SB1ScoreLUnit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreLUnit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreLUnit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreUAmount">Score High Estimate</label>
				    <input id="SB1ScoreUAmount" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreUAmount;double;8</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreUAmount" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreUUnit">Score High Unit</label>
				    <input id="SB1ScoreUUnit" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreUUnit;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreUUnit" /></xsl:attribute>
				    </input>
          </div>
          <div class="ui-block-a">
            <label for="SB1ScoreMathType">Score Math Type</label>
            <select class="Select225" id="SB1ScoreMathType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreMathType;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">algorithm1</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm1')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm1
              </option>
              <option>
                <xsl:attribute name="value">algorithm2</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm2')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm2
              </option>
             <option>
                <xsl:attribute name="value">algorithm3</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm3')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm3
              </option>
              <option>
                <xsl:attribute name="value">algorithm4</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm4')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm4
              </option>
              <option>
                <xsl:attribute name="value">algorithm5</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm5')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm5
              </option>
              <option>
                <xsl:attribute name="value">algorithm6</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm6')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm6
              </option>
              <option>
                <xsl:attribute name="value">algorithm7</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm7')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm7
              </option>
              <option>
                <xsl:attribute name="value">algorithm8</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm8')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm8
              </option>
              <option>
                <xsl:attribute name="value">algorithm9</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm9')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm9
              </option>
              <option>
                <xsl:attribute name="value">algorithm10</xsl:attribute>
                <xsl:if test="(@SB1ScoreMathType = 'algorithm10')">
                  <xsl:attribute name="selected" />
                </xsl:if>algorithm10
              </option>
            </select>
          </div>
          <div class="ui-block-b">
            <label for="SB1ScoreMathSubType">Score Math Sub Type</label>
            <input id="SB1ScoreMathSubType" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreMathSubType;string;25</xsl:attribute>
					    </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@SB1ScoreMathSubType" /></xsl:attribute>
				    </input>
          </div>
       </div>
      <div>
        <label for="SB1ScoreMathResult">Score Math Result</label>
			  <textarea id="SB1ScoreMathResult" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1ScoreMathResult;string;2000</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@SB1ScoreMathResult" />
			  </textarea>
      </div>
      <div>
        <label for="SB1JointDataURL">Joint Data</label>
			  <textarea id="SB1JointDataURL" data-mini="true">
          <xsl:if test="($viewEditType = 'full')">
					  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SB1JointDataURL;string;1500</xsl:attribute>
				  </xsl:if>
          <xsl:value-of select="@SB1JointDataURL" />
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
        <li><strong>Step 2. Score Amount and Unit:</strong> The Unit must be manually entered. The Amount will be the result of the Math Expression calculation.</li>
        <li><strong>Step 2. ScoreD1 Amount and Unit:</strong> First distribution variable for Score.</li>
        <li><strong>Step 2. ScoreD2 Amount and Unit:</strong> Second distribution for Score.</li>
        <li><strong>Step 2. Distribution Type:</strong> The numeric distribution of Score. Refer to the Stock Calculation 1 reference.</li>
        <li><strong>Step 2. Score Math Type and Math Sub Type:</strong> Mathematical algorithm and subalgorithm to use with Distribution Type, Score, ScoreD1, and ScoreD2 to solve for ScoreM, ScoreL, and ScoreU. Refer to the Stock Calculation 1 reference for the algorithms.</li>
        <li><strong>Step 2. Score Most Likely, Score Low, Score High, Amounts and Units:</strong> Results of Distribution Type and Math Type calculations.</li>
        <li><strong>Step 2. Iterations:</strong> Number of iterations to use when drawing random number samples for some algorithms.  </li>
        <li><strong>Step 2. Confidence Interval:</strong> Level of confidence interval to use when reporting all Score and Indicator high and low amounts. Should be an integer such as 95, 90, or 40.</li>
        <li><strong>Step 2. Random Seed:</strong> Any positive integer, except 0, will result in the same set of random variables being used each time a calculation is run.</li>
        <li><strong>Step 2. Score BaseIO:</strong> Base input or output property to update with the Score Most Likely property. </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>Refer to the Stock Calculation 1 reference.</strong></li>
			</ul>
      </div>
		</div>
		</xsl:if>
</xsl:template>
</xsl:stylesheet>
  