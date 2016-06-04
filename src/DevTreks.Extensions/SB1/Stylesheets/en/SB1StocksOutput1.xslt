<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2015, December -->
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
	<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
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
    <div id="divstepzero">
      <xsl:choose>
        <xsl:when test="($docToCalcNodeName = 'outputgroup' 
							or $docToCalcNodeName = 'output' 
							or $docToCalcNodeName = 'outputseries'
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
				</xsl:when>
				<xsl:otherwise>
					<h3>This output analyzer does not appear appropriate for the document being analyzed. Are you 
					sure this is the right analyzer?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>Resource Stock Output Analyzer View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool generates a variety of basic resource stock statistics for outputs.
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
				  <label for="lblFileToAnalysisExtensionType" >Base Calculations To Analyze Type:</label>
				  <select id="lblFileToAnalysisExtensionType" data-mini="true">
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
              <xsl:attribute name="value">output</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'output')">
								<xsl:attribute name="selected" />
							</xsl:if>Base Outputs
						</option>
				  </select>
			  </div>
        <div>
				  <label for="lblAnalyzerType" >Analysis Type:</label>
				  <select id="lblAnalyzerType" data-mini="true">
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
							<xsl:attribute name="value">sbtotal1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbtotal1')">
								<xsl:attribute name="selected" />
							</xsl:if>Totals
						</option>
						<option>
							<xsl:attribute name="value">sbstat1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbstat1')">
								<xsl:attribute name="selected" />
							</xsl:if>Statistics 1
						</option>
            <option>
							<xsl:attribute name="value">sbchangeyr</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbchangeyr')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Year
						</option>
            <option>
							<xsl:attribute name="value">sbchangeid</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbchangeid')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Id
						</option>
            <option>
							<xsl:attribute name="value">sbchangealt</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbchangealt')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Alternative
						</option>
            <option>
							<xsl:attribute name="value">sbprogress1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'sbprogress1')">
								<xsl:attribute name="selected" />
							</xsl:if>Progress 1
						</option>
				  </select>
			  </div>
        <xsl:value-of select="DisplayDevPacks:WriteStandardAnalysisParams2($searchurl, $viewEditType,
            $docToCalcNodeName, @Option5)"/>
        <xsl:if test="(@AnalyzerType != 'sbtotal1')">
          <div data-role="collapsible"  data-theme="b" data-content-theme="d">
            <h4 class="ui-bar-b">
              <strong>Stocks to Analyze</strong>
            </h4>
            <div class="ui-field-contain">
              <fieldset data-role="controlgroup">
                <legend>Choose up to 10:</legend>
                <!--all are dynamically set to either true or false in IsJQUpdated().js-->
                <input type="checkbox" id="SB1Label1" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label1;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label1 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label1">Indicator 1</label>
                <input type="checkbox" id="SB1Label2" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label2;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label2 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label2">Indicator 2</label>
                <input type="checkbox" id="SB1Label3" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label3;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label3 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label3">Indicator 3</label>
                <input type="checkbox" id="SB1Label4" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label4;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label4 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label4">Indicator 4</label>
                <input type="checkbox" id="SB1Label5" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label5;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label5 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label5">Indicator 5</label>
                <input type="checkbox" id="SB1Label6" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label6;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label6 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label6">Indicator 6</label>
                <input type="checkbox" id="SB1Label7" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label7;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label7 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label7">Indicator 7</label>
                <input type="checkbox" id="SB1Label8" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label8;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label8 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label8">Indicator 8</label>
                <input type="checkbox" id="SB1Label9" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label9;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label9 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label9">Indicator 9</label>
                <input type="checkbox" id="SB1Label10" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label10;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label10 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label10">Indicator 10</label>
                <input type="checkbox" id="SB1Label11" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label11;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label11 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label11">Indicator 11</label>
                <input type="checkbox" id="SB1Label12" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label12;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label12 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label12">Indicator 12</label>
                <input type="checkbox" id="SB1Label13" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label13;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label13 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label13">Indicator 13</label>
                <input type="checkbox" id="SB1Label14" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label14;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label14 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label14">Indicator 14</label>
                <input type="checkbox" id="SB1Label15" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label15;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label15 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label15">Indicator 15</label>
                <input type="checkbox" id="SB1Label16" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label16;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label16 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label16">Indicator 16</label>
                <input type="checkbox" id="SB1Label17" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label17;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label17 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label17">Indicator 17</label>
                <input type="checkbox" id="SB1Label18" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label18;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label18 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label18">Indicator 18</label>
                <input type="checkbox" id="SB1Label19" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label19;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label19 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label19">Indicator 19</label>
                <input type="checkbox" id="SB1Label20" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;SB1Label20;string;15</xsl:attribute>
                  <xsl:if test="(@SB1Label20 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="SB1Label20">Indicator 20</label>
              </fieldset>
            </div>
          </div>
        </xsl:if>
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
        <xsl:if test="(($docToCalcNodeName != 'outputseries') or contains($docToCalcNodeName, 'devpack') or contains($docToCalcNodeName, 'linkedview'))">
          <div data-role="collapsible"  data-theme="b" data-content-theme="d">
          <h4>Relations</h4>
	        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams1($searchurl, $viewEditType,
                @UseSameCalculator, @Overwrite)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@AnalyzerType = 'sbchangeyr' or @AnalyzerType = 'sbchangeid' 
          or @AnalyzerType = 'sbchangealt')">
          <div data-role="collapsible"  data-theme="b" data-content-theme="d">
            <h4 class="ui-bar-b">
              <strong>Math Tests</strong>
            </h4>
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <label for="MathType"> Math Type</label>
              <select class="Select225" id="MathType" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MathType;string;25</xsl:attribute>
                </xsl:if>
                <option>
                  <xsl:attribute name="value">makeselection</xsl:attribute>make selection
                </option>
                <option>
                  <xsl:attribute name="value">none</xsl:attribute>
                  <xsl:if test="(@MathType = 'none')">
                    <xsl:attribute name="selected" />
                  </xsl:if>none
                </option>
                <option>
                  <xsl:attribute name="value">algorithm1</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm1')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm1
                </option>
                <option>
                  <xsl:attribute name="value">algorithm2</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm2')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm2
                </option>
               <option>
                  <xsl:attribute name="value">algorithm3</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm3')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm3
                </option>
                <option>
                  <xsl:attribute name="value">algorithm4</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm4')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm4
                </option>
                <option>
                  <xsl:attribute name="value">algorithm5</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm5')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm5
                </option>
                <option>
                  <xsl:attribute name="value">algorithm6</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm6')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm6
                </option>
                <option>
                  <xsl:attribute name="value">algorithm7</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm7')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm7
                </option>
                <option>
                  <xsl:attribute name="value">algorithm8</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm8')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm8
                </option>
                <option>
                  <xsl:attribute name="value">algorithm9</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm9')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm9
                </option>
                <option>
                  <xsl:attribute name="value">algorithm10</xsl:attribute>
                  <xsl:if test="(@MathType = 'algorithm10')">
                    <xsl:attribute name="selected" />
                  </xsl:if>algorithm10
                </option>
              </select>
            </div>
            <div class="ui-block-b">
              <label for="MathSubType"> Math Sub Type</label>
              <input id="MathSubType" type="text" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
						      <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MathSubType;string;25</xsl:attribute>
					      </xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@MathSubType" /></xsl:attribute>
				      </input>
            </div>
            <div class="ui-block-a">
              <label for="MathCILevel">Confidence Interval</label>
				      <input id="MathCILevel" type="text" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
						      <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MathCILevel;integer;4</xsl:attribute>
					      </xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@MathCILevel" /></xsl:attribute>
				      </input>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            <label for="MathExpression">Math Expression</label>
            <textarea id="MathExpression" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
					      <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MathExpression;string;250</xsl:attribute>
				      </xsl:if>
              <xsl:value-of select="@MathExpression" />
			      </textarea>
          </div>
          <div>
				    <label for="MathResult">Math Result</label>
				    <textarea id="MathResult" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
						    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MathResult;string;1500</xsl:attribute>
              </xsl:if>
					    <xsl:value-of select="@MathResult" />
				    </textarea>
			    </div>
          </div>
        </xsl:if>
			  <div>
				  <label for="lblDescription">Description</label>
				  <textarea id="lblDescription" data-mini="true">
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
        <xsl:value-of select="DisplayDevPacks:WriteStandardCalculatorParams2($searchurl, $viewEditType,
          @WhatIfTagName, '')"/>
        <div>
				  <label for="lblRelatedCalculatorType" >Base Resource (Output) Calculations To Analyze Type:</label>
				  <select id="lblRelatedCalculatorType" data-mini="true">
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
							<xsl:attribute name="value">sb101</xsl:attribute>
							<xsl:if test="(@RelatedCalculatorType = 'sb101')">
								<xsl:attribute name="selected" />
							</xsl:if>Resource Stock Calculator 1
						</option>
				  </select>
			  </div>
        <xsl:if test="(@AnalyzerType = 'sbstat1' or @AnalyzerType = 'sbprogress1')">
          <xsl:value-of select="DisplayDevPacks:WriteStandardAnalysisParams1($searchurl, $viewEditType,
            $docToCalcNodeName, @Option1, @Option2, @Option4)"/>
        </xsl:if>
        <xsl:if test="(@AnalyzerType = 'sbchangeyr' or @AnalyzerType = 'sbchangeid' or @AnalyzerType = 'sbchangealt')">
          <xsl:value-of select="DisplayDevPacks:WriteStandardComparisons($searchurl, $viewEditType,
            $docToCalcNodeName, @Option1)"/>
        </xsl:if>
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
			  <h4 class="ui-bar-b"><strong>Instructions </strong></h4>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 1</h4>
			  <ul data-role="listview">
				  <li><strong>Step 1. Base Calculations To Analyze:</strong> Choose the type of base element to analyze.</li>
					<li><strong>Step 1. Analysis Type:</strong> Choose the type of analysis to run. Ensure that it coincides with the name of the analyzer being used.  </li>
          <li><strong>Step 1. Stocks to Analyze:</strong> Choose up to 10 resource stock indicators to analyze. Run a Totals Analysis to find out the number associated with indicator.</li>
			  </ul>
			  </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 2</h4>
			  <ul data-role="listview">
				  <li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
          <li><strong>Step 2. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
          <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				  <li><strong>Step 2. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
          <li><strong>Step 2. Alternative:</strong> Used for comparisons.</li>
          <li><strong>Step 2. Target Type:</strong> Used for progress analysis and to set benchmarks and actuals.</li>
          <li><strong>Step 2. Math Properties:</strong> Choose optional algorithms and subalgorithms. Refer to the CTA tutorial for options.</li>
          <li><strong>Step 2. Media URL:</strong> Delimited string of Media URLs that explain the analysis.</li>
			  </ul>
        </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 3</h4>
          <ul data-role="listview">
				    <li><strong>Step 3. Save Using:</strong> The Save command saves xml data the Save Text command saves csv data.</li>
			    </ul>
        </div>
        <br /><br />
        <h4>Reminder: The 'Resource Stock Totals' analyzer requires running the underlying stock calculator after inserting, deleting, 
								or updating, this linked view (that keeps the stock calculator's linked views synchronized with these linkedviews).
        </h4>
		  </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
