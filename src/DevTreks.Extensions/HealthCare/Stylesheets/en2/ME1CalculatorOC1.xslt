<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, May -->
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
        <xsl:when test="(contains($docToCalcNodeName, 'operation')
              or contains($docToCalcNodeName, 'component')
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This operation/component calculator does not appear appropriate for the document being calculated. Are you 
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
					This tool tracks monitoring and evaluation indicators for operation or component uris. Up to 20 new 
          M and E indicators can be added for each operation or component. In general, benchmark and target 
          indicators are added to one calculator to track baseline conditions. Actual indicators are 
          added to track realtime, midterm, expost, or midterm conditions.
          Indicator labels are used to identify the same indicator in different calculators (use WBSs for the labels).
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
				  <textarea class="Text75H100PCW" id="IndDescription1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription1;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription1" />
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
            <label for="IndAltType1">Indicator Type 1</label>
            <select class="Select225" id="IndAltType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType1;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType1 = 'actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType1 = 'benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType1 = 'partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType1 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType1">Price Type 1</label>
            <select class="Select225" id="IndPriceType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType1;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType1 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType1 = 'oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType1 = 'aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType1 = 'cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight1">Weight 1</label>
            <input id="IndWeight1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight1;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType1">Math Type 1</label>
            <select class="Select225" id="IndMathType1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType1;string;50</xsl:attribute>
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
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType1 = 'Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount1">Amount A 1 </label>
						<input id="Ind1aAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount1;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit1">Unit A 1 </label>
						<input id="Ind1Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount1">Amount B 1 </label>
						<input id="Ind2aAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit1">Unit B 1</label>
						<input id="Ind2Unit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit1;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal1">Total 1 </label>
						<input id="IndTotal1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal1;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal1" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit1">Total Unit 1 </label>
						<input id="IndUnit1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit1;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit1" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription2;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription2" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel2">Label 2 </label>
            <input id="IndLabel2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType2">Indicator Type 2</label>
            <select class="Select225" id="IndAltType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType2;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType2 = 'actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType2 = 'benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType2 = 'partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType2 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType2">Price Type 2</label>
            <select class="Select225" id="IndPriceType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType2;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType2 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType2 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType2 = 'oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType2 = 'aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType2 = 'cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight2">Weight 2</label>
            <input id="IndWeight2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight2;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType2">Math Type 2</label>
            <select class="Select225" id="IndMathType2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType2;string;50</xsl:attribute>
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
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType2 = 'Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount2">Amount A 2 </label>
						<input id="Ind1aAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount2;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit2">Unit A 2 </label>
						<input id="Ind1Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount2">Amount B 2 </label>
						<input id="Ind2aAmount2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit2">Unit B 2</label>
						<input id="Ind2Unit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit2;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal2">Total 2 </label>
						<input id="IndTotal2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal2;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal2" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit2">Total Unit 2 </label>
						<input id="IndUnit2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit2;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit2" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription3;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription3" />
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
            <label for="IndAltType3">Indicator Type 3</label>
            <select class="Select225" id="IndAltType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType3;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType3 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType3 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType3 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType3 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType3 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType3">Price Type 3</label>
            <select class="Select225" id="IndPriceType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType3;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType3 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType3 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType3 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType3 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType3 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight3">Weight 3</label>
            <input id="IndWeight3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight3;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType3">Math Type 3</label>
            <select class="Select225" id="IndMathType3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType3;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType3 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType3 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType3 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType3 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType3 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount3">Amount A 3 </label>
						<input id="Ind1aAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount3;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit3">Unit A 3 </label>
						<input id="Ind1Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount3">Amount B 1 </label>
						<input id="Ind2aAmount3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit3">Unit B 3</label>
						<input id="Ind2Unit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit3;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal3">Total 3 </label>
						<input id="IndTotal3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal3;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal3" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit3">Total Unit 3 </label>
						<input id="IndUnit3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit3;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit3" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription4" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription4;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription4" />
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
            <label for="IndAltType4">Indicator Type 4</label>
            <select class="Select225" id="IndAltType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType4;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType4 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType4 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType4 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType4 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType4 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType4">Price Type 4</label>
            <select class="Select225" id="IndPriceType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType4;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType4 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType4 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType4 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType4 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType4 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight4">Weight 4</label>
            <input id="IndWeight4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight4;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight4" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType4">Math Type 4</label>
            <select class="Select225" id="IndMathType4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType4;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType4 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType4 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType4 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType4 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType4 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount4">Amount A 4 </label>
						<input id="Ind1aAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount4;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit4">Unit A 4 </label>
						<input id="Ind1Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount4">Amount B 4</label>
						<input id="Ind2aAmount4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit4">Unit B 4</label>
						<input id="Ind2Unit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit4;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal4">Total 4</label>
						<input id="IndTotal4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal4;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal4" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit4">Total Unit 4</label>
						<input id="IndUnit4" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit4;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit4" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription5" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription5;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription5" />
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
            <label for="IndAltType5">Indicator Type 5</label>
            <select class="Select225" id="IndAltType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType5;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType5 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType5 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType5 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType5 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType5 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType5">Price Type 5</label>
            <select class="Select225" id="IndPriceType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType5;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType5 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType5 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType5 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType5 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType5 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight5">Weight 5</label>
            <input id="IndWeight5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight5;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight5" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType5">Math Type 5</label>
            <select class="Select225" id="IndMathType5" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType5;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType5 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType5 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType5 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType5 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType5 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount5">Amount A 5 </label>
						<input id="Ind1aAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount5;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit5">Unit A 5 </label>
						<input id="Ind1Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount5">Amount B 5</label>
						<input id="Ind2aAmount5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit5">Unit B 5</label>
						<input id="Ind2Unit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit5;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal5">Total 5</label>
						<input id="IndTotal5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal5;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal5" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit5">Total Unit 5</label>
						<input id="IndUnit5" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit5;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit5" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription6" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription6;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription6" />
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
            <label for="IndAltType6">Indicator Type 6</label>
            <select class="Select225" id="IndAltType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType6;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType6 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType6 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType6 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType6 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType6 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType6">Price Type 6</label>
            <select class="Select225" id="IndPriceType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType6;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType6 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType6 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType6 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType6 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType6 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight6">Weight 6</label>
            <input id="IndWeight6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight6;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight6" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType6">Math Type 6</label>
            <select class="Select225" id="IndMathType6" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType6;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType6 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType6 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType6 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType6 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType6 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount6">Amount A 6 </label>
						<input id="Ind1aAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount6;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit6">Unit A 6 </label>
						<input id="Ind1Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount6">Amount B 6</label>
						<input id="Ind2aAmount6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit6">Unit B 6</label>
						<input id="Ind2Unit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit6;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal6">Total 6</label>
						<input id="IndTotal6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal6;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal6" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit6">Total Unit 6</label>
						<input id="IndUnit6" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit6;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit6" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription7" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription7;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription7" />
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
            <label for="IndAltType7">Indicator Type 7</label>
            <select class="Select225" id="IndAltType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType7;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType7 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType7 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType7 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType7 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType7 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType7">Price Type 7</label>
            <select class="Select225" id="IndPriceType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType7;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType7 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType7 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType7 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType7 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType7 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight7">Weight 7</label>
            <input id="IndWeight7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight7;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight7" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType7">Math Type 7</label>
            <select class="Select225" id="IndMathType7" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType7;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType7 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType7 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType7 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType7 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType7 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount7">Amount A 7 </label>
						<input id="Ind1aAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount7;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit7">Unit A 7 </label>
						<input id="Ind1Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount7">Amount B 7</label>
						<input id="Ind2aAmount7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit7">Unit B 7</label>
						<input id="Ind2Unit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit7;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal7">Total 7</label>
						<input id="IndTotal7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal7;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal7" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit7">Total Unit 7</label>
						<input id="IndUnit7" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit7;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit7" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription8" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription8;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription8" />
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
            <label for="IndAltType8">Indicator Type 8</label>
            <select class="Select225" id="IndAltType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType8;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType8 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType8 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType8 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType8 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType8 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType8">Price Type 8</label>
            <select class="Select225" id="IndPriceType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType8;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType8 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType8 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType8 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType8 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType8 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight8">Weight 8</label>
            <input id="IndWeight8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight8;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight8" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType8">Math Type 8</label>
            <select class="Select225" id="IndMathType8" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType8;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType8 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType8 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType8 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType8 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType8 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount8">Amount A 8 </label>
						<input id="Ind1aAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount8;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit8">Unit A 8 </label>
						<input id="Ind1Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount8">Amount B 8</label>
						<input id="Ind2aAmount8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit8">Unit B 8</label>
						<input id="Ind2Unit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit8;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal8">Total 8</label>
						<input id="IndTotal8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal8;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal8" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit8">Total Unit 8</label>
						<input id="IndUnit8" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit8;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit8" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription9" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription9;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription9" />
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
            <label for="IndAltType9">Indicator Type 9</label>
            <select class="Select225" id="IndAltType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType9;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType9 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType9 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType9 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType9 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType9 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType9">Price Type 9</label>
            <select class="Select225" id="IndPriceType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType9;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType9 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType9 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType9 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType9 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType9 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight9">Weight 9</label>
            <input id="IndWeight9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight9;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight9" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType9">Math Type 9</label>
            <select class="Select225" id="IndMathType9" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType9;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType9 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType9 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType9 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType9 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType9 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount9">Amount A 9 </label>
						<input id="Ind1aAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount9;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit9">Unit A 9 </label>
						<input id="Ind1Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount9">Amount B 9</label>
						<input id="Ind2aAmount9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit9">Unit B 9</label>
						<input id="Ind2Unit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit9;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal9">Total 9</label>
						<input id="IndTotal9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal9;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal9" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit9">Total Unit 9</label>
						<input id="IndUnit9" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit9;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit9" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription10" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription10;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription10" />
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
            <label for="IndAltType10">Indicator Type 10</label>
            <select class="Select225" id="IndAltType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType10;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType10 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType10 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType10 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType10 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType10 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType10">Price Type 10</label>
            <select class="Select225" id="IndPriceType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType10;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType10 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType10 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType10 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType10 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType10 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight10">Weight 10</label>
            <input id="IndWeight10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight10;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight10" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType10">Math Type 10</label>
            <select class="Select225" id="IndMathType10" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType10;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType10 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType10 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType10 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType10 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType10 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount10">Amount A 10 </label>
						<input id="Ind1aAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount10;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit10">Unit A 10 </label>
						<input id="Ind1Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount10">Amount B 10</label>
						<input id="Ind2aAmount10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit10">Unit B 10</label>
						<input id="Ind2Unit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit10;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal10">Total 10</label>
						<input id="IndTotal10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal10;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal10" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit10">Total Unit 10</label>
						<input id="IndUnit10" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit10;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit10" /></xsl:attribute>
						</input>
          </div>
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
	      <xsl:if test="(($docToCalcNodeName != 'operation' and $docToCalcNodeName != 'component') or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
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
				  <textarea class="Text75H100PCW" id="IndDescription11" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription11;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription11" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel11">Label 11 </label>
            <input id="IndLabel11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel11;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType11">Indicator Type 11</label>
            <select class="Select225" id="IndAltType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType11;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType11 = 'actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType11 = 'benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType11 = 'partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType11 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType11">Price Type 11</label>
            <select class="Select225" id="IndPriceType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType11;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType11 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType11 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType11 = 'oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType11 = 'aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType11 = 'cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight11">Weight 11</label>
            <input id="IndWeight11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight11;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight11" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType11">Math Type 11</label>
            <select class="Select225" id="IndMathType11" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType11;string;50</xsl:attribute>
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
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType11 = 'Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount11">Amount A 11 </label>
						<input id="Ind1aAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount11;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit11">Unit A 11 </label>
						<input id="Ind1Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount11">Amount B 11 </label>
						<input id="Ind2aAmount11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit11">Unit B 11</label>
						<input id="Ind2Unit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit11;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal11">Total 11 </label>
						<input id="IndTotal11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal11;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal11" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit11">Total Unit 11 </label>
						<input id="IndUnit11" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit11;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit11" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription12" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription12;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription12" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel12">Label 12 </label>
            <input id="IndLabel12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel12;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType12">Indicator Type 12</label>
            <select class="Select225" id="IndAltType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType12;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType12 = 'actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType12 = 'benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType12 = 'partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType12 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType12">Price Type 12</label>
            <select class="Select225" id="IndPriceType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType12;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType12 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType12 = 'rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType12 = 'oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType12 = 'aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType12 = 'cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight12">Weight 12</label>
            <input id="IndWeight12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight12;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight12" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType12">Math Type 12</label>
            <select class="Select225" id="IndMathType12" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType12;string;50</xsl:attribute>
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
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType12 = 'Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount12">Amount A 12 </label>
						<input id="Ind1aAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount12;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit12">Unit A 12 </label>
						<input id="Ind1Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount12">Amount B 12 </label>
						<input id="Ind2aAmount12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit12">Unit B 12</label>
						<input id="Ind2Unit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit12;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal12">Total 12 </label>
						<input id="IndTotal12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal12;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal12" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit12">Total Unit 12 </label>
						<input id="IndUnit12" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit12;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit12" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription13" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription13;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription13" />
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
            <label for="IndAltType13">Indicator Type 13</label>
            <select class="Select225" id="IndAltType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType13;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType13 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType13 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType13 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType13 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType13 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType13">Price Type 13</label>
            <select class="Select225" id="IndPriceType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType13;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType13 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType13 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType13 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType13 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType13 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight13">Weight 13</label>
            <input id="IndWeight13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight13;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight13" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType13">Math Type 13</label>
            <select class="Select225" id="IndMathType13" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType13;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType13 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType13 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType13 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType13 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType13 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount13">Amount A 13 </label>
						<input id="Ind1aAmount1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount13;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit13">Unit A 13 </label>
						<input id="Ind1Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount13">Amount B 1 </label>
						<input id="Ind2aAmount13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit13">Unit B 13</label>
						<input id="Ind2Unit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit13;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal13">Total 13 </label>
						<input id="IndTotal13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal13;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal13" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit13">Total Unit 13 </label>
						<input id="IndUnit13" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit13;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit13" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription14" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription14;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription14" />
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
            <label for="IndAltType14">Indicator Type 14</label>
            <select class="Select225" id="IndAltType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType14;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType14 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType14 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType14 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType14 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType14 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType14">Price Type 14</label>
            <select class="Select225" id="IndPriceType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType14;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType14 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType14 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType14 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType14 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType14 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight14">Weight 14</label>
            <input id="IndWeight14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight14;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight14" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType14">Math Type 14</label>
            <select class="Select225" id="IndMathType14" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType14;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType14 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType14 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType14 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType14 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType14 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount14">Amount A 14 </label>
						<input id="Ind1aAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount14;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit14">Unit A 14 </label>
						<input id="Ind1Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount14">Amount B 14</label>
						<input id="Ind2aAmount14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit14">Unit B 14</label>
						<input id="Ind2Unit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit14;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal14">Total 14</label>
						<input id="IndTotal14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal14;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal14" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit14">Total Unit 14</label>
						<input id="IndUnit14" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit14;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit14" /></xsl:attribute>
						</input>
          </div>
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
				  <textarea class="Text75H100PCW" id="IndDescription15" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription15;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription15" />
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
            <label for="IndAltType15">Indicator Type 15</label>
            <select class="Select225" id="IndAltType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType15;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType15 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType15 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType15 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType15 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType15 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
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
            <label for="IndPriceType15">Price Type 15</label>
            <select class="Select225" id="IndPriceType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType15;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType15 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType15 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType15 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType15 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType15 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight15">Weight 15</label>
            <input id="IndWeight15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight15;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight15" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType15">Math Type 15</label>
            <select class="Select225" id="IndMathType15" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType15;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType15 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType15 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType15 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType15 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType15 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount15">Amount A 15 </label>
						<input id="Ind1aAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount15;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit15">Unit A 15 </label>
						<input id="Ind1Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount15">Amount B 15</label>
						<input id="Ind2aAmount15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit15">Unit B 15</label>
						<input id="Ind2Unit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit15;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal15">Total 15</label>
						<input id="IndTotal15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal15;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal15" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit15">Total Unit 15</label>
						<input id="IndUnit15" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit15;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit15" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 16</strong></h4>
        <div>
          <label for="IndName16" class="ui-hidden-accessible">Name 16</label>
          <input id="IndName16" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName16;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName16" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription16">Description 16</label>
				  <textarea class="Text75H100PCW" id="IndDescription16" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription16;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription16" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel16">Label 16</label>
            <input id="IndLabel16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel16;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel16" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType16">Indicator Type 16</label>
            <select class="Select225" id="IndAltType16" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType16;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType16 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType16 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType16 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType16 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType16 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndDate16">Date 16</label>
            <input id="IndDate16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate16;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate16" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndPriceType16">Price Type 16</label>
            <select class="Select225" id="IndPriceType16" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType16;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType16 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType16 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType16 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType16 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType16 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight16">Weight 16</label>
            <input id="IndWeight16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight16;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight16" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType16">Math Type 16</label>
            <select class="Select225" id="IndMathType16" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType16;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType16 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType16 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType16 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType16 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType16 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount16">Amount A 16 </label>
						<input id="Ind1aAmount16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount16;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount16" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit16">Unit A 16 </label>
						<input id="Ind1Unit16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit16;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit16" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount16">Amount B 16</label>
						<input id="Ind2aAmount16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount16;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount16" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit16">Unit B 16</label>
						<input id="Ind2Unit16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit16;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit16" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal16">Total 16</label>
						<input id="IndTotal16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal16;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal16" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit16">Total Unit 16</label>
						<input id="IndUnit16" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit16;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit16" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 17</strong></h4>
        <div>
          <label for="IndName17" class="ui-hidden-accessible">Name 17</label>
          <input id="IndName17" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName17;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName117" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription17">Description 17</label>
				  <textarea class="Text75H100PCW" id="IndDescription17" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription17;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription17" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel17">Label 17</label>
            <input id="IndLabel17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel17;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel17" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType17">Indicator Type 17</label>
            <select class="Select225" id="IndAltType17" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType17;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType17 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType17 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType17 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType17 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType17 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndDate17">Date 17</label>
            <input id="IndDate17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate17;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate17" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndPriceType17">Price Type 17</label>
            <select class="Select225" id="IndPriceType17" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType17;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType17 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType17 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType17 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType17 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType17 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight17">Weight 17</label>
            <input id="IndWeight17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight17;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight17" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType17">Math Type 17</label>
            <select class="Select225" id="IndMathType17" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType17;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType17 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType17 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType17 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType17 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType17 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount17">Amount A 17 </label>
						<input id="Ind1aAmount17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount17;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount17" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit17">Unit A 17 </label>
						<input id="Ind1Unit17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit17;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit17" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount17">Amount B 17</label>
						<input id="Ind2aAmount17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount17;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount17" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit17">Unit B 17</label>
						<input id="Ind2Unit17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit17;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit17" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal17">Total 17</label>
						<input id="IndTotal17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal17;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal17" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit17">Total Unit 17</label>
						<input id="IndUnit17" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit17;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit17" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 18</strong></h4>
        <div>
          <label for="IndName18" class="ui-hidden-accessible">Name 18</label>
          <input id="IndName18" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName18;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName18" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription18">Description 18</label>
				  <textarea class="Text75H100PCW" id="IndDescription18" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription18;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription18" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel18">Label 18</label>
            <input id="IndLabel18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel18;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel18" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType18">Indicator Type 18</label>
            <select class="Select225" id="IndAltType18" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType18;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType18 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType18 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType18 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType18 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType18 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndDate18">Date 18</label>
            <input id="IndDate18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate18;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate18" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndPriceType18">Price Type 18</label>
            <select class="Select225" id="IndPriceType18" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType18;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType18 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType18 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType18 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType18 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType18 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight18">Weight 18</label>
            <input id="IndWeight18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight18;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight18" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType18">Math Type 18</label>
            <select class="Select225" id="IndMathType18" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType18;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType18 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType18 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType18 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType18 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType18 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount18">Amount A 18 </label>
						<input id="Ind1aAmount18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount18;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount18" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit18">Unit A 18 </label>
						<input id="Ind1Unit18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit18;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit18" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount18">Amount B 18</label>
						<input id="Ind2aAmount18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount18;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount18" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit18">Unit B 18</label>
						<input id="Ind2Unit18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit18;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit18" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal18">Total 18</label>
						<input id="IndTotal18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal18;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal18" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit18">Total Unit 18</label>
						<input id="IndUnit18" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit18;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit18" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 19</strong></h4>
        <div>
          <label for="IndName19" class="ui-hidden-accessible">Name 19</label>
          <input id="IndName19" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName19;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName19" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription19">Description 19</label>
				  <textarea class="Text75H100PCW" id="IndDescription19" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription19;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription19" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel19">Label 19</label>
            <input id="IndLabel19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel19;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel19" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType19">Indicator Type 19</label>
            <select class="Select225" id="IndAltType19" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType19;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType19 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType19 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType19 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType19 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType19 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndDate19">Date 19</label>
            <input id="IndDate19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate19;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate19" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndPriceType19">Price Type 19</label>
            <select class="Select225" id="IndPriceType19" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType19;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType19 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType19 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType19 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType19 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType19 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight19">Weight 19</label>
            <input id="IndWeight19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight19;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight19" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType19">Math Type 19</label>
            <select class="Select225" id="IndMathType19" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType19;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType19 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType19 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType19 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType19 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType19 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount19">Amount A 19 </label>
						<input id="Ind1aAmount19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount19;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount19" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit19">Unit A 19 </label>
						<input id="Ind1Unit19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit19;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit19" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount19">Amount B 19</label>
						<input id="Ind2aAmount19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount19;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount19" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit19">Unit B 19</label>
						<input id="Ind2Unit19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit19;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit19" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal19">Total 19</label>
						<input id="IndTotal19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal19;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal19" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit19">Total Unit 19</label>
						<input id="IndUnit19" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit19;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit19" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Indicator 20</strong></h4>
        <div>
          <label for="IndName20" class="ui-hidden-accessible">Name 20</label>
          <input id="IndName20" type="text"  data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndName20;string;75</xsl:attribute>
            </xsl:if>
            <xsl:attribute name="value"><xsl:value-of select="@IndName20" /></xsl:attribute>
          </input>
        </div>
        <div >
				  <label for="IndDescription20">Description 20</label>
				  <textarea class="Text75H100PCW" id="IndDescription20" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDescription20;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@IndDescription20" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="IndLabel20">Label 20</label>
            <input id="IndLabel20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndLabel20;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndLabel20" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndAltType20">Indicator Type 20</label>
            <select class="Select225" id="IndAltType20" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndAltType20;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndAltType20 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">actual</xsl:attribute>
                <xsl:if test="(@IndAltType20 ='actual')">
                  <xsl:attribute name="selected" />
                </xsl:if>actual
              </option>
              <option>
                <xsl:attribute name="value">benchmark</xsl:attribute>
                <xsl:if test="(@IndAltType20 ='benchmark')">
                  <xsl:attribute name="selected" />
                </xsl:if>benchmark
              </option>
              <option>
                <xsl:attribute name="value">partialtarget</xsl:attribute>
                <xsl:if test="(@IndAltType20 ='partialtarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>partialtarget
              </option>
              <option>
                <xsl:attribute name="value">fulltarget</xsl:attribute>
                <xsl:if test="(@IndAltType20 = 'fulltarget')">
                  <xsl:attribute name="selected" />
                </xsl:if>fulltarget
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndDate20">Date 20</label>
            <input id="IndDate20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndDate20;datetime;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndDate20" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndPriceType20">Price Type 20</label>
            <select class="Select225" id="IndPriceType20" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndPriceType20;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndPriceType20 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">rev</xsl:attribute>
                <xsl:if test="(@IndPriceType20 ='rev')">
                  <xsl:attribute name="selected" />
                </xsl:if>revenue
              </option>
              <option>
                <xsl:attribute name="value">oc</xsl:attribute>
                <xsl:if test="(@IndPriceType20 ='oc')">
                  <xsl:attribute name="selected" />
                </xsl:if>operating
              </option>
              <option>
                <xsl:attribute name="value">aoh</xsl:attribute>
                <xsl:if test="(@IndPriceType20 ='aoh')">
                  <xsl:attribute name="selected" />
                </xsl:if>overhead
              </option>
              <option>
                <xsl:attribute name="value">cap</xsl:attribute>
                <xsl:if test="(@IndPriceType20 ='cap')">
                  <xsl:attribute name="selected" />
                </xsl:if>capital
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="IndWeight20">Weight 20</label>
            <input id="IndWeight20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndWeight20;double;8</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndWeight20" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="IndMathType20">Math Type 20</label>
            <select class="Select225" id="IndMathType20" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndMathType20;string;50</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@IndMathType20 ='none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">Q1_add_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType20 ='Q1_add_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_add_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_subtract_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType20 ='Q1_subtract_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_subtract_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_divide_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType20 ='Q1_divide_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_divide_Q2
              </option>
              <option>
                <xsl:attribute name="value">Q1_multiply_Q2</xsl:attribute>
                <xsl:if test="(@IndMathType20 ='Q1_multiply_Q2')">
                  <xsl:attribute name="selected" />
                </xsl:if>Q1_multiply_Q2
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="Ind1aAmount20">Amount A 20 </label>
						<input id="Ind1aAmount20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1aAmount20;double;8</xsl:attribute>
              </xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@Ind1aAmount20" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind1Unit20">Unit A 20 </label>
						<input id="Ind1Unit20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind1Unit20;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind1Unit20" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="Ind2aAmount20">Amount B 20</label>
						<input id="Ind2aAmount20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2aAmount20;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2aAmount20" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="Ind2Unit20">Unit B 20</label>
						<input id="Ind2Unit20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Ind2Unit20;string;25</xsl:attribute>
								</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@Ind2Unit20" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-a">
            <label for="IndTotal20">Total 20</label>
						<input id="IndTotal20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndTotal20;double;8</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndTotal20" /></xsl:attribute>
						</input>
          </div>
          <div class="ui-block-b">
            <label for="IndUnit20">Total Unit 20</label>
						<input id="IndUnit20" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;IndUnit20;string;25</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@IndUnit20" /></xsl:attribute>
						</input>
          </div>
        </div>
      </div>
      <div >
				<label for="CalculatorDescription">Calculations Description</label>
				<textarea class="Text75H100PCW" id="CalculatorDescription" data-mini="true">
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
				<li><strong>Step 1. Indicators:</strong> Up to 10 indicators can be entered in this step and up to 10 indicators can be included in the next step.</li>
				<li><strong>Step 1. Indicator x Name and Description:</strong> Name and description for each indicator.</li>
        <li><strong>Step 1. Indicator x Label:</strong> Labels are used to group together benchmark, target and actual indicators. DevTreks recommends using a WBS to 
          classify the indicators.</li>
				<li><strong>Step 1. Indicator x Indicator Type:</strong> At least one benchmark, one full target, one partial target, and one actual, indicator 
          must be entered for each group of indicators. They must have the same label and the label must be unique. As mentioned in Step 1, the indicators 
          do not need to be in the same linked view. For example, benchmark and targets may be added to one operation calculator while 
          the actual results (i.e. quarterly) are kept in a separate calculator.</li>
				<li><strong>Step 1. Indicator x Date:</strong> Make sure that the benchmark, targets, actual, indicators have distinct dates.</li>
        <li><strong>Step 1. Indicator x Price Type:</strong> If the indicator is a benefit or cost (i.e. substitute inputs), enter the price type. </li>
        <li><strong>Step 1. Indicator x Weight:</strong> A multiplier used in weighted milestone calculations. The Total will be multiplied by this value. Use the description to explain further.</li>
        <li><strong>Step 1. Quantity 1 and 2:</strong> Fill in an amount and unit for at least Quantity 1. Fill in Quantity 2 when two quantities are needed to derive 
          a total for the indicator. For example, a substitute input would derive a total cost by including a price for Quantity 1, a quantity for Quantity 2, 
          and a Math Type of Q1_multiply_Q2. A rate can be derived by filling in both Quantities and then specifying Q1_divide_Q2. Use the description field 
          to further explain.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
        <li><strong>Step 2. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 2. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 2. Indicators:</strong> Up to 10 indicators can be included in this step.</li>
        <li><strong>Step 2. Indicator x Total and Unit:</strong> For example, a substitute input would derive a total cost by including a price for Quantity 1, a quantity for Quantity 2, 
          and a Math Type of Q1_multiply_Q2. </li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>International Federation of Red Cross and Red Crescent Societies</strong> Project/programme monitoring and evaluation (MandE) guide. 2011. (www.ifcr.org)</li>
        <li><strong>International Institute for Educational Planning</strong> Manual for Monitoring and Evaluating Education Partnerships. UNESCO 2009. (www.iiep.unesco.org)</li>
				<li><strong>US Government Accountability Office</strong> Applied Research and Methods. GAO Cost Estimating and Assessment Guide. Best Practices for Developing and Managing Capital Program Costs. March, 2009.</li>
        <li><strong>US Agency for International Development</strong> Nutrition by Design, Design and Measuring for Nutrition Impacts. Presentation made December, 2012 for Agriculture and Nutrition Global Learning and Evidence Exchange (NGLEE).</li>
				<li><strong>United Nations Development Programme</strong> Handbook on Planning, Monitoring and Evaluating for Development Results. 2009</li>
			</ul>
      </div>
		</div>
		</xsl:if>
</xsl:template>
</xsl:stylesheet>
  