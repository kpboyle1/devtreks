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
	<!-- the node being calculated (custom docs' nodename can be a devpack, while this might be budgetgroup) -->
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
				<br />
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
					<h3>This operation and component analyzer does not appear appropriate for the document being analyzed. Are you 
					sure this is the right analyzer?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>Operation and Component M and E Analyzer Views</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool generates a variety of basic statistics for DevTreks standard monitoring and evaluation uris. 
          The analyses include basic progress in achieving actual versus target levels for monitoring and evaluation indicators.
          The people targeted by this monitoring and evaluation analysis can be described by using the optional Demographic properties.
			</p>
			<p>
				<strong>Analysis View Description</strong>
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
        <h4 class="ui-bar-b"><strong>Step 1 of 3. Analyze</strong></h4>
		    <xsl:variable name="calcParams1">'&amp;step=steptwo<xsl:value-of select="$calcParams" />'</xsl:variable>
			  <xsl:if test="($viewEditType = 'full')">
				  <xsl:value-of select="DisplayDevPacks:WriteGetConstantsButtons($selectedFileURIPattern, $contenturipattern, $calcParams1)"/>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'steptwo')">
          <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			  </xsl:if>
        <br />
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
        <label for="MandEType">Monitoring and Evaluation Stage</label>
				<select class="Select225" id="MandEType" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
              <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MandEType;string;25</xsl:attribute>
            </xsl:if>
            <option>
              <xsl:attribute name="value">makeselection</xsl:attribute>make selection
            </option>
            <option>
              <xsl:attribute name="value">none</xsl:attribute>
              <xsl:if test="(@MandEType = 'none')">
                <xsl:attribute name="selected" />
              </xsl:if>none
            </option>
            <option>
              <xsl:attribute name="value">baseline</xsl:attribute>
              <xsl:if test="(@MandEType = 'baseline')">
                <xsl:attribute name="selected" />
              </xsl:if>baseline
            </option>
            <option>
              <xsl:attribute name="value">realtime</xsl:attribute>
              <xsl:if test="(@MandEType = 'realtime')">
                <xsl:attribute name="selected" />
              </xsl:if>realtime
            </option>
            <option>
              <xsl:attribute name="value">midterm</xsl:attribute>
              <xsl:if test="(@MandEType = 'midterm')">
                <xsl:attribute name="selected" />
              </xsl:if>midterm
            </option>
            <option>
              <xsl:attribute name="value">final</xsl:attribute>
              <xsl:if test="(@MandEType = 'final')">
                <xsl:attribute name="selected" />
              </xsl:if>final
            </option>
            <option>
              <xsl:attribute name="value">expost</xsl:attribute>
              <xsl:if test="(@MandEType = 'expost')">
                <xsl:attribute name="selected" />
              </xsl:if>expost
            </option>
          </select>
			  </div>
        <div>
				  <label for="lblFileToAnalysisExtensionType" >Base Calculations To Analyze Type:</label>
				  <select class="Select225" id="lblFileToAnalysisExtensionType" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FilesToAnalyzeExtensionType;string;25</xsl:attribute>
            </xsl:if>
            <option>
							<xsl:attribute name="value">none</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'none' or @FilesToAnalyzeExtensionType = '')">
								<xsl:attribute name="selected" />
							</xsl:if>Needs Selection
						</option>
						<option>
							<xsl:attribute name="value">operation</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'operation')">
								<xsl:attribute name="selected" />
							</xsl:if>NPV Operations
						</option>
						<option>
							<xsl:attribute name="value">component</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'component')">
								<xsl:attribute name="selected" />
							</xsl:if>NPV Components
						</option>
				  </select>
			  </div>
        <div>
				  <label for="lblAnalyzerType" >Analysis Type:</label>
				  <select class="Select225" id="lblAnalyzerType" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;AnalyzerType;string;25</xsl:attribute>
            </xsl:if>
            <option>
							<xsl:attribute name="value">none</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'none' or @AnalyzerType = '')">
								<xsl:attribute name="selected" />
							</xsl:if>Needs Selection
						</option>
						<option>
							<xsl:attribute name="value">resources01</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'resources01')">
								<xsl:attribute name="selected" />
							</xsl:if>Monitoring and Evaluation 1 Progress
						</option>
				  </select>
			  </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Demographics 1</strong></h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4Name1">Name</label>
            <input id="D4Name1" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Name1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Name1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4Label1">Label</label>
            <input id="D4Label1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Label1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Label1" /></xsl:attribute>
            </input>
          </div>
        </div>
        <div >
				  <label for="D4Description1">Description</label>
				  <textarea class="Text75H100PCW" id="D4Description1" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Description1;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@D4Description1" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4NameA1">Name A</label>
            <input id="D4NameA1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameA1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameA1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountA1">Amount A</label>
            <select class="Select225" id="D4AmountA1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountA1;string;25</xsl:attribute>
              </xsl:if>
              <option>
                <xsl:attribute name="value">makeselection</xsl:attribute>make selection
              </option>
              <option>
                <xsl:attribute name="value">none</xsl:attribute>
                <xsl:if test="(@D4AmountA1 = 'none')">
                  <xsl:attribute name="selected" />
                </xsl:if>none
              </option>
              <option>
                <xsl:attribute name="value">female</xsl:attribute>
                <xsl:if test="(@D4AmountA1 = 'female')">
                  <xsl:attribute name="selected" />
                </xsl:if>female
              </option>
              <option>
                <xsl:attribute name="value">male</xsl:attribute>
                <xsl:if test="(@D4AmountA1 = 'male')">
                  <xsl:attribute name="selected" />
                </xsl:if>male
              </option>
            </select>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitA1">Unit A</label>
            <input id="D4UnitA1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitA1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitA1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopA1">Population A</label>
            <input id="D4PopA1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopA1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopA1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameB1">Name B</label>
            <input id="D4NameB1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameB1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameB1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountB1">Amount B</label>
            <input id="D4AmountB1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountB1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountB1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitB1">Unit B</label>
            <input id="D4UnitB1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitB1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitB1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopB1">Population B</label>
            <input id="D4PopB1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopB1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopB1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameC1">Name C</label>
            <input id="D4NameC1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameC1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameC1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountC1">Amount C</label>
            <input id="D4AmountC1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountC1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountC1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitC1">Unit C</label>
            <input id="D4UnitC1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitC1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitC1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopC1">Population C</label>
            <input id="D4PopC1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopC1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopC1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameD1">Name D</label>
            <input id="D4NameD1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameD1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameD1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountD1">Amount D</label>
            <input id="D4AmountD1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountD1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountD1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitD1">Unit D</label>
            <input id="D4UnitD1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitD1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitD1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopD1">Population D</label>
            <input id="D4PopD1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopD1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopD1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameE1">Name E</label>
            <input id="D4NameE1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameE1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameE1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountE1">Amount E</label>
            <input id="D4AmountE1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountE1;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountE1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitE1">Unit E</label>
            <input id="D4UnitE1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitE1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitE1" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopE1">Population E</label>
            <input id="D4PopE1" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopE1;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopE1" /></xsl:attribute>
            </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Demographics 2</strong></h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4Name2">Name</label>
            <input id="D4Name2" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Name2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Name2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4Label2">Label</label>
            <input id="D4Label2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Label2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Label2" /></xsl:attribute>
            </input>
          </div>
        </div>
        <div >
				  <label for="D4Description2">Description</label>
				  <textarea class="Text75H100PCW" id="D4Description2" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Description2;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@D4Description2" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4NameA2">Name A</label>
            <input id="D4NameA2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameA2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameA2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountA2">Amount A</label>
            <input id="D4AmountA2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountA2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountA2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitA2">Unit A</label>
            <input id="D4UnitA2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitA2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitA2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopA2">Population A</label>
            <input id="D4PopA2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopA2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopA2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameB2">Name B</label>
            <input id="D4NameB2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameB2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameB2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountB2">Amount B</label>
            <input id="D4AmountB2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountB2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountB2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitB2">Unit B</label>
            <input id="D4UnitB2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitB2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitB2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopB2">Population B</label>
            <input id="D4PopB2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopB2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopB2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameC2">Name C</label>
            <input id="D4NameC2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameC2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameC2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountC2">Amount C</label>
            <input id="D4AmountC2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountC2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountC2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitC2">Unit C</label>
            <input id="D4UnitC2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitC2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitC2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopC2">Population C</label>
            <input id="D4PopC2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopC2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopC2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameD2">Name D</label>
            <input id="D4NameD2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameD2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameD2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountD2">Amount D</label>
            <input id="D4AmountD2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountD2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountD2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitD2">Unit D</label>
            <input id="D4UnitD2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitD2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitD2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopD2">Population D</label>
            <input id="D4PopD2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopD2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopD2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameE2">Name E</label>
            <input id="D4NameE2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameE2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameE2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountE2">Amount E</label>
            <input id="D4AmountE2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountE2;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountE2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitE2">Unit E</label>
            <input id="D4UnitE2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitE2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitE2" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopE2">Population E</label>
            <input id="D4PopE2" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopE2;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopE2" /></xsl:attribute>
            </input>
          </div>
        </div>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>Demographics 3</strong></h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4Name3">Name</label>
            <input id="D4Name3" type="text"  data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Name3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Name3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4Label3">Label</label>
            <input id="D4Label3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Label3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4Label3" /></xsl:attribute>
            </input>
          </div>
        </div>
        <div >
				  <label for="D4Description3">Description</label>
				  <textarea class="Text75H100PCW" id="D4Description3" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4Description3;string;255</xsl:attribute>
            </xsl:if>
					  <xsl:value-of select="@D4Description3" />
				  </textarea>
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="D4NameA3">Name A</label>
            <input id="D4NameA3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameA3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameA3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountA3">Amount A</label>
            <input id="D4AmountA3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountA3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountA3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitA3">Unit A</label>
            <input id="D4UnitA3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitA3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitA3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopA3">Population A</label>
            <input id="D4PopA3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopA3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopA3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameB3">Name B</label>
            <input id="D4NameB3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameB3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameB3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountB3">Amount B</label>
            <input id="D4AmountB3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountB3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountB3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitB3">Unit B</label>
            <input id="D4UnitB3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitB3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitB3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopB3">Population B</label>
            <input id="D4PopB3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopB3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopB3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameC3">Name C</label>
            <input id="D4NameC3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameC3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameC3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountC3">Amount C</label>
            <input id="D4AmountC3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountC3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountC3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitC3">Unit C</label>
            <input id="D4UnitC3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitC3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitC3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopC3">Population C</label>
            <input id="D4PopC3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopC3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopC3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameD3">Name D</label>
            <input id="D4NameD3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameD3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameD3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountD3">Amount D</label>
            <input id="D4AmountD3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountD3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountD3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitD3">Unit D</label>
            <input id="D4UnitD3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitD3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitD3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopD3">Population D</label>
            <input id="D4PopD3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopD3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopD3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4NameE3">Name E</label>
            <input id="D4NameE3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4NameE3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4NameE3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4AmountE3">Amount E</label>
            <input id="D4AmountE3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4AmountE3;string;25</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4AmountE3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-a">
            <label for="D4UnitE3">Unit E</label>
            <input id="D4UnitE3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4UnitE3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4UnitE3" /></xsl:attribute>
            </input>
          </div>
          <div class="ui-block-b">
            <label for="D4PopE3">Population E</label>
            <input id="D4PopE3" type="text" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;D4PopE3;string;15</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value"><xsl:value-of select="@D4PopE3" /></xsl:attribute>
            </input>
          </div>
        </div>
      </div>
		  </div>
     <div id="divsteptwo">
			  <h4 class="ui-bar-b"><strong>Step 2 of 3. Analyze</strong></h4>
			  <xsl:variable name="calcParams3">'&amp;step=stepthree<xsl:value-of select="$calcParams"/>'</xsl:variable>
        <xsl:if test="($viewEditType = 'full')">
				  <xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'stepthree')">
            <h4 class="ui-bar-b"><strong>Success. Please review the calculations below.</strong></h4>
			  </xsl:if>
        <xsl:if test="(($docToCalcNodeName != 'operation' and $docToCalcNodeName != 'component') or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
          <div data-role="collapsible"  data-theme="b" data-content-theme="d">
          <h4>Relations</h4>
	        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
                @UseSameCalculator, @Overwrite)"/>
          </div>
        </xsl:if>
        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, '')"/>
        <div>
				  <label for="lblRelatedCalculatorType" >Base Resource (Input) Calculations To Analyze Type:</label>
				  <select class="Select225" id="lblRelatedCalculatorType" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
						 <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;RelatedCalculatorType;string;25</xsl:attribute>
            </xsl:if>
            <option>
							<xsl:attribute name="value">none</xsl:attribute>
							<xsl:if test="(@RelatedCalculatorType = 'none' or @RelatedCalculatorType = '')">
								<xsl:attribute name="selected" />
							</xsl:if>Needs Selection
						</option>
						<option>
							<xsl:attribute name="value">me1</xsl:attribute>
							<xsl:if test="(@RelatedCalculatorType = 'me1')">
								<xsl:attribute name="selected" />
							</xsl:if>Monitoring and Evaluation 1
						</option>
				  </select>
			  </div>
        <xsl:value-of select="DisplayDevPacks:WriteStandardAnalysisParams1($searchurl, $viewEditType,
          $docToCalcNodeName, @Option1, @Option2, @Option4)"/>
        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, '')"/>
        <div>
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
					  <xsl:variable name="calcParams4a">'&amp;step=stepfour&amp;savemethod=analyses<xsl:value-of select="$calcParams"/>'</xsl:variable>
            <p>
							  <strong>Method 1.</strong> Do you wish to save step 2's calculations? These calculations are viewed by opening this particular calculator addin.
					  </p>
					  <xsl:if test="($viewEditType = 'full')">
						  <xsl:value-of select="DisplayDevPacks:MakeDevTreksButton('savecalculation', 'SubmitButton1Enabled150', 'Save Calcs', $contenturipattern, $selectedFileURIPattern, 'prepaddin', 'linkedviews', 'runaddin', 'none', $calcParams4a)" />
					  </xsl:if>
				  </xsl:if>
			  </xsl:if>
			  <xsl:if test="($lastStepNumber = 'stepfour')
				  or ($lastStepNumber = 'stepthree' and $filexttype = 'temp')">
						  <h3>
								<xsl:if test="($saveMethod != 'saveastext'
									or $filexttype = 'temp')">
									Your analysis has been saved. The analysis can be viewed whenever
									this analyzer addin is opened.
								</xsl:if>
								<xsl:if test="($saveMethod = 'saveastext')">
									The analysis has been saved. The text document can be downloaded by clicking on the 
									Add To Package link and downloading a package.
								</xsl:if>
							</h3>
			  </xsl:if>
		  </div>
			<div id="divsteplast">
			  <h4 class="ui-bar-b"><strong>Instructions</strong></h4>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 1</h4>
			  <ul data-role="listview">
           <li><strong>Step 1. Monitoring and Evaluation Stage:</strong> 
              Some groups may prefer to store all of their baseline, target, and actual indicators results in one analysis. The advantage 
              is that one operation and one input can be used to store benchmark, target, and actual indicator measurements.
              Other groups may prefer to break the analysis into stages, such as baseline, midterm, and final. The latter requires 
              adding stage-related indicators to specific operations with specific inputs. </li>
				  <li><strong>Step 1. Base Calculations To Analyze:</strong>  Make sure that the data being analyzed corresponds to either operations or components.</li>
					<li><strong>Step 1. Analysis Type:</strong> A video overview of these options will be available.</li>
				  <li><strong>Step 1. Demographics:</strong> Up to three sets of demographics can be entered. 
              Each set includes a name, unit, quantity or description, and population amount, for up to five demographic properties. 
              Examples include income, height, weight, race, gender, and postal code. Because of the generic nature of this calculator, 
              the five demographic properties had to be kept generic. More specific demographics can be entered in accompanying 
              calculators and analyzers (i.e. food nutrition-related demographics can be entered in an accompanying food 
              nutrition calculator or analyzer). </li>
			  </ul>
        </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 2 and 3</h4>
          <ul data-role="listview">
            <li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
            <li><strong>Step 2. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
            <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
            <li><strong>Step 1. Base Resource Calculations To Analyze:</strong>  Make sure that the calculator chosen has been used with the data being analyzed.</li>
            <li><strong>Step 2. Compare Using:</strong> The Compare Only option is not available in this version.</li>
					  <li><strong>Step 2. Aggregate Using:</strong> Types is the least detailed analysis. Groups is the next most detailed analysis. Labels can be the most detailed, but all of the data being analyzed must have been classified using labels and Work Breakdown Structures.</li>
				    <li><strong>Step 3. Save Using:</strong> The Save command saves xml data the Save Text command saves csv data.</li>
			    </ul>
        </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Results</h4>
			  <ul data-role="listview">
          <li><strong>Partial Target Totals Only: </strong> The results show the cumulative totals for each indicator's partial targets. Each indicator is identified by a unique label (preferably a WBS label).</li>
				  <li><strong>Type:</strong> The indicator's Alternative Type: benchmark, actual, partialtarget, fulltarget.</li>
          <li><strong>Actual Total:</strong> The actual quantity of the indicator completed during this partial target period. </li>
          <li><strong>Actual Date:</strong> The date of the last 'actual' indicator included in the Actual Total.</li>
				  <li><strong>Benchmark Total:</strong> The starting quantity of the indicator prior to any project intervention.</li>
				  <li><strong>Benchmark Percent:</strong> (Actual Total / Benchmark Total) * 100. Zero if the Benchmark Total is zero.</li>
          <li><strong>Partial Target Date:</strong> The date of this 'partial target' indicator.</li>
          <li><strong>Partial Target Total:</strong> The target quantity of the indicator for this partial period.</li>
				  <li><strong>Partial Target Percent:</strong> (Actual Total / Partial Target Total) * 100. Zero if the Partial Target Total is zero.</li>
          <li><strong>Full Total:</strong> The target quantity of the indicator for the full project period.</li>
				  <li><strong>Full Percent:</strong> (Actual Total / Full Total) * 100. Zero if the Full Total is zero. Note that the Actual Total is only for this partial period. Derive the percent for the full target period by adding together all of this indicator's Full Percents.</li>
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
        <h4>Reminder: Analyzers require running the underlying NPV calculator after inserting, deleting, 
								or updating, this linked view (that keeps the NPV calculator's linked views synchronized with these linkedviews).
        </h4>
		  </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>