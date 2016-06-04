<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
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
					<h3>This operation and component analyzer does not appear appropriate for the document being analyzed. Are you 
					sure this is the right analyzer?</h3>
				</xsl:otherwise>
			</xsl:choose>
      <h4 class="ui-bar-b"><strong>Food Nutrition Operation or Component Analyzer View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool generates a variety of basic food nutrition cost stock statistics for DevTreks components and operations.
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
							<xsl:attribute name="value">component</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'component')">
								<xsl:attribute name="selected" />
							</xsl:if>NPV Components
						</option>
            <option>
							<xsl:attribute name="value">operation</xsl:attribute>
							<xsl:if test="(@FilesToAnalyzeExtensionType = 'operation')">
								<xsl:attribute name="selected" />
							</xsl:if>NPV Operations
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
							<xsl:attribute name="value">mntotal1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mntotal1')">
								<xsl:attribute name="selected" />
							</xsl:if>Totals
						</option>
						<option>
							<xsl:attribute name="value">mnstat1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mnstat1')">
								<xsl:attribute name="selected" />
							</xsl:if>Statistics 1
						</option>
            <option>
							<xsl:attribute name="value">mnchangeyr</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mnchangeyr')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Year
						</option>
            <option>
							<xsl:attribute name="value">mnchangeid</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mnchangeid')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Id
						</option>
            <option>
							<xsl:attribute name="value">mnchangealt</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mnchangealt')">
								<xsl:attribute name="selected" />
							</xsl:if>Change By Alternative
						</option>
            <option>
							<xsl:attribute name="value">mnprogress1</xsl:attribute>
							<xsl:if test="(@AnalyzerType = 'mnprogress1')">
								<xsl:attribute name="selected" />
							</xsl:if>Progress 1
						</option>
				  </select>
			  </div>
        <p>Date: <xsl:value-of select="@Date" /></p>
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
							<xsl:attribute name="value">foodnutSR01</xsl:attribute>
							<xsl:if test="(@RelatedCalculatorType = 'foodnutSR01')">
								<xsl:attribute name="selected" />
							</xsl:if>Food Nutrition ARS SR
						</option>
				  </select>
			  </div>
        <xsl:if test="(@AnalyzerType = 'mnstat1' or @AnalyzerType = 'mnprogress1')">
          <xsl:value-of select="DisplayDevPacks:WriteStandardAnalysisParams1($searchurl, $viewEditType,
            $docToCalcNodeName, @Option1, @Option2, @Option4)"/>
        </xsl:if>
        <xsl:if test="(@AnalyzerType = 'mnchangeyr' or @AnalyzerType = 'mnchangeid' or @AnalyzerType = 'mnchangealt')">
          <xsl:value-of select="DisplayDevPacks:WriteStandardComparisons($searchurl, $viewEditType,
            $docToCalcNodeName, @Option1)"/>
        </xsl:if>
        <xsl:value-of select="DisplayDevPacks:WriteStandardAnalysisParams2($searchurl, $viewEditType,
            $docToCalcNodeName, @Option5)"/>
        <xsl:if test="(@AnalyzerType != 'mntotal1')">
          <div data-role="collapsible"  data-theme="b" data-content-theme="d">
            <h4 class="ui-bar-b">
              <strong>Nutrients to Analyze</strong>
            </h4>
            <div class="ui-field-contain">
              <fieldset data-role="controlgroup">
                <legend>Choose up to 10:</legend>
                <!--all are dynamically set to either true or false in IsJQUpdated().js-->
                <input type="checkbox" id="ContainerPrice" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ContainerPrice;string;25</xsl:attribute>
                  <xsl:if test="(@ContainerPrice = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ContainerPrice">ContainerPrice</label>
                <input type="checkbox" id="ContainerSizeInSSUnits" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ContainerSizeInSSUnits;string;25</xsl:attribute>
                  <xsl:if test="(@ContainerSizeInSSUnits = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ContainerSizeInSSUnits">ContainerSizeInSSUnits</label>
                <input type="checkbox" id="ServingCost" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ServingCost;string;25</xsl:attribute>
                  <xsl:if test="(@ServingCost = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ServingCost">ServingCost</label>
                <input type="checkbox" id="ActualServingSize" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ActualServingSize;string;25</xsl:attribute>
                  <xsl:if test="(@ActualServingSize = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ActualServingSize">ActualServingSize</label>
                <input type="checkbox" id="TypicalServingSize" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TypicalServingSize;string;25</xsl:attribute>
                  <xsl:if test="(@TypicalServingSize = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="TypicalServingSize">TypicalServingSize</label>
                <input type="checkbox" id="TypicalServingsPerContainer" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;TypicalServingsPerContainer;string;25</xsl:attribute>
                  <xsl:if test="(@TypicalServingsPerContainer = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="TypicalServingsPerContainer">TypicalServingsPerContainer</label>
                <input type="checkbox" id="ActualServingsPerContainer" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ActualServingsPerContainer;string;25</xsl:attribute>
                  <xsl:if test="(@ActualServingsPerContainer = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ActualServingsPerContainer">ActualServingsPerContainer</label>
                <input type="checkbox" id="Water_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Water_g;string;25</xsl:attribute>
                  <xsl:if test="(@Water_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Water_g">Water_g</label>
                <input type="checkbox" id="Energ_Kcal" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Energ_Kcal;string;25</xsl:attribute>
                  <xsl:if test="(@Energ_Kcal = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Energ_Kcal">Energ_Kcal</label>
                <input type="checkbox" id="Protein_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Protein_g;string;25</xsl:attribute>
                  <xsl:if test="(@Protein_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Protein_g">Protein_g</label>
                <input type="checkbox" id="Lipid_Tot_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Lipid_Tot_g;string;25</xsl:attribute>
                  <xsl:if test="(@Lipid_Tot_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Lipid_Tot_g">Lipid_Tot_g</label>
                <input type="checkbox" id="Ash_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Ash_g;string;25</xsl:attribute>
                  <xsl:if test="(@Ash_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Ash_g">Ash_g</label>
                <input type="checkbox" id="Carbohydrt_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Carbohydrt_g;string;25</xsl:attribute>
                  <xsl:if test="(@Carbohydrt_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Carbohydrt_g">Carbohydrt_g</label>
                <input type="checkbox" id="Fiber_TD_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Fiber_TD_g;string;25</xsl:attribute>
                  <xsl:if test="(@Fiber_TD_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Fiber_TD_g">Fiber_TD_g</label>
                <input type="checkbox" id="Sugar_Tot_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Sugar_Tot_g;string;25</xsl:attribute>
                  <xsl:if test="(@Sugar_Tot_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Sugar_Tot_g">Sugar_Tot_g</label>
                <input type="checkbox" id="Calcium_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Calcium_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Calcium_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Calcium_mg">Calcium_mg</label>
                <input type="checkbox" id="Iron_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Iron_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Iron_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Iron_mg">Iron_mg</label>
                <input type="checkbox" id="Magnesium_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Magnesium_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Magnesium_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Magnesium_mg">Magnesium_mg</label>
                <input type="checkbox" id="Phosphorus_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Phosphorus_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Phosphorus_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Phosphorus_mg">Phosphorus_mg</label>
                <input type="checkbox" id="Potassium_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Potassium_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Potassium_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Potassium_mg">Potassium_mg</label>
                <input type="checkbox" id="Sodium_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Sodium_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Sodium_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Sodium_mg">Sodium_mg</label>
                <input type="checkbox" id="Zinc_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Zinc_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Zinc_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Zinc_mg">Zinc_mg</label>
                <input type="checkbox" id="Copper_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Copper_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Copper_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Copper_mg">Copper_mg</label>
                <input type="checkbox" id="Manganese_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Manganese_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Manganese_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Manganese_mg">Manganese_mg</label>
                <input type="checkbox" id="Selenium_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Selenium_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Selenium_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Selenium_pg">Selenium_pg</label>
                <input type="checkbox" id="Vit_C_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_C_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_C_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_C_mg">Vit_C_mg</label>
                <input type="checkbox" id="Thiamin_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Thiamin_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Thiamin_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Thiamin_mg">Thiamin_mg</label>
                <input type="checkbox" id="Riboflavin_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Riboflavin_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Riboflavin_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Riboflavin_mg">Riboflavin_mg</label>
                <input type="checkbox" id="Niacin_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Niacin_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Niacin_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Niacin_mg">Niacin_mg</label>
                <input type="checkbox" id="Panto_Acid_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Panto_Acid_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Panto_Acid_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Panto_Acid_mg">Panto_Acid_mg</label>
                <input type="checkbox" id="Vit_B6_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_B6_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_B6_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_B6_mg">Vit_B6_mg</label>
                <input type="checkbox" id="Folate_Tot_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Folate_Tot_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Folate_Tot_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Folate_Tot_pg">Folate_Tot_pg</label>
                <input type="checkbox" id="Folic_Acid_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Folic_Acid_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Folic_Acid_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Folic_Acid_pg">Folic_Acid_pg</label>
                <input type="checkbox" id="Food_Folate_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Food_Folate_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Food_Folate_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Food_Folate_pg">Food_Folate_pg</label>
                <input type="checkbox" id="Folate_DFE_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Folate_DFE_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Folate_DFE_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Folate_DFE_pg">Folate_DFE_pg</label>
                <input type="checkbox" id="Choline_Tot_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Choline_Tot_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Choline_Tot_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Choline_Tot_mg">Choline_Tot_mg</label>
                <input type="checkbox" id="Vit_B12_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_B12_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_B12_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_B12_pg">Vit_B12_pg</label>
                <input type="checkbox" id="Vit_A_IU" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_A_IU;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_A_IU = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_A_IU">Vit_A_IU</label>
                <input type="checkbox" id="Vit_A_RAE" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_A_RAE;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_A_RAE = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_A_RAE">Vit_A_RAE</label>
                <input type="checkbox" id="Retinol_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Retinol_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Retinol_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Retinol_pg">Retinol_pg</label>
                <input type="checkbox" id="Alpha_Carot_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Alpha_Carot_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Alpha_Carot_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Alpha_Carot_pg">Alpha_Carot_pg</label>
                <input type="checkbox" id="Beta_Carot_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Beta_Carot_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Beta_Carot_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Beta_Carot_pg">Beta_Carot_pg</label>
                <input type="checkbox" id="Beta_Crypt_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Beta_Crypt_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Beta_Crypt_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Beta_Crypt_pg">Beta_Crypt_pg</label>
                <input type="checkbox" id="Lycopene_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Lycopene_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Lycopene_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Lycopene_pg">Lycopene_pg</label>
                <input type="checkbox" id="Lut_Zea_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Lut_Zea_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Lut_Zea_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Lut_Zea_pg">Lut_Zea_pg</label>
                <input type="checkbox" id="Vit_E_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_E_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_E_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_E_mg">Vit_E_mg</label>
                <input type="checkbox" id="Vit_D_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_D_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_D_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_D_pg">Vit_D_pg</label>
                <input type="checkbox" id="ViVit_D_IU" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ViVit_D_IU;string;25</xsl:attribute>
                  <xsl:if test="(@ViVit_D_IU = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="ViVit_D_IU">ViVit_D_IU</label>
                <input type="checkbox" id="Vit_K_pg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Vit_K_pg;string;25</xsl:attribute>
                  <xsl:if test="(@Vit_K_pg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Vit_K_pg">Vit_K_pg</label>
                <input type="checkbox" id="FA_Sat_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;FA_Sat_g;string;25</xsl:attribute>
                  <xsl:if test="(@FA_Sat_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="FA_Sat_g">FA_Sat_g</label>
                <input type="checkbox" id="FA_Mono_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;FA_Mono_g;string;25</xsl:attribute>
                  <xsl:if test="(@FA_Mono_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="FA_Mono_g">FA_Mono_g</label>
                <input type="checkbox" id="FA_Poly_g" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;FA_Poly_g;string;25</xsl:attribute>
                  <xsl:if test="(@FA_Poly_g = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="FA_Poly_g">FA_Poly_g</label>
                <input type="checkbox" id="Cholestrl_mg" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Cholestrl_mg;string;25</xsl:attribute>
                  <xsl:if test="(@Cholestrl_mg = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Cholestrl_mg">Cholestrl_mg</label>
                <input type="checkbox" id="Extra1" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Extra1;string;25</xsl:attribute>
                  <xsl:if test="(@Extra1 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Extra1">Extra1</label>
                <input type="checkbox" id="Extra2" class="custom" >
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Extra2;string;25</xsl:attribute>
                  <xsl:if test="(@Extra2 = 'true')">
                    <xsl:attribute name="checked">true</xsl:attribute>
                  </xsl:if>
                </input>
                <label for="Extra2">Extra2</label>
              </fieldset>
            </div>
          </div>
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
				  <li><strong>Step 1. Base Calculations To Analyze:</strong> Operations go into operating budgets. Components go into capital budgets.</li>
					<li><strong>Step 1. Analysis Type:</strong> Totals add together the resource stocks being analyzed. Mean and Standard deviationis not available yet.</li>
			  </ul>
			  </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 2</h4>
			  <ul data-role="listview">
				  <li><strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
				  <li><strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
          <li><strong>Step 2. Base Resource Calculations To Analyze Type:</strong> The calculator chosen should actually be used, and used fairly frequently, in the underlying data being analyzed.</li>
					<li><strong>Step 2. Related Calculators Type:</strong> Name of the "Related Calculator Type" found in descendants. Runs calculations using the data found in those descendant linked views. 
            This property takes precedence over the "Base Resoure Calculations to Analyze Type" property. </li>
          <li><strong>Step 2. Nutrients to Analyze:</strong> Choose up to 10 food nutrient properties to analyze.</li>
			  </ul>
        </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			  <h4>Step 3</h4>
          <ul data-role="listview">
				    <li><strong>Step 3. Save Using:</strong> The Save command saves xml data the Save Text command saves csv data.</li>
			    </ul>
        </div>
        <br /><br />
        <h4>Reminder: The 'Resource Stock Totals' analyzer requires running the underlying NPV calculator after inserting, deleting, 
								or updating, this linked view (that keeps the NPV calculator's linked views synchronized with these linkedviews).
        </h4>
		  </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
