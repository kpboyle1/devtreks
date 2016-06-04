<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, April -->
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
      <h4 class="ui-bar-b"><strong>Machinery Calculation View</strong></h4>
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This tool calculates totals for input machinery uris. The discounted totals include operating,
					allocated overhead, and capital costs. Operating costs include fuel, repair and maintenance, and labor costs. 
					Allocated overhead costs include capital recovery, and taxes, housing and insurance costs. When added to 
          an operation or component, power inputs set maximum PTO horsepower properties while nonpower inputs 
          set equivalent PTO horsepower and field capacity properties.
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
			<xsl:value-of select="DisplayDevPacks:WriteSelectListForNewView(
						$linkedListsArray, $searchurl, 'machineryconstant', 
						@MachConstantId, $serverSubActionType, 'full', 
						'lstMachineryConstants', 'Machinery Constant', 
						';MachConstantId;integer;4')"/>
      <p>
				  Examples:
				  <br />
					Field Efficiency: <xsl:value-of select="format-number(@FieldEffTypical, '#,###.000')"/>&#xA0;
					Speed: <xsl:value-of select="format-number(@FieldSpeedTypical, '#,###.00')"/>&#xA0;
					Width: <xsl:value-of select="format-number(@Width, '#,###.00')"/>
		  </p>
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
				$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
				@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
				@RealRateId, @NominalRateId, @RatingGroupId)"/>
		</div>
		<div id="divsteptwo">
      <h4 class="ui-bar-b"><strong>Step 2 of 4. Make Selections</strong></h4>
		  <xsl:variable name="calcParams2">'&amp;step=stepthree<xsl:value-of select="$calcParams" />'</xsl:variable>
			<xsl:if test="($viewEditType = 'full')">
			  <xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams2)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
        <h4 class="ui-bar-b"><strong>Success. Please review the selections below.</strong></h4>
			</xsl:if>
      <br />
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true"> 
        <h4 class="ui-bar-b"><strong>Prices</strong></h4>
			  <xsl:value-of select="DisplayDevPacks:WriteSelectListsForPrices(
				  @UnitGroupId, $searchurl, $serverSubActionType, 'full', 
				  @PriceGas, @PriceDiesel, @PriceLP, @PriceNG,
				  @PriceElectric, @PriceOil, @PriceRegularLabor, 
				  @PriceMachineryLabor, @PriceSupervisorLabor, @TaxPercent, 
				  @InsurePercent, @HousingPercent)"/>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true"> 
        <h4 class="ui-bar-b"><strong>Optional Machinery Selection and Scheduling Size Range</strong></h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="size1">Size (Width) 1 </label>
            <xsl:if test="($viewEditType != 'full')">
						  <xsl:value-of select="@SizeRange1" />
					  </xsl:if>
					  <xsl:if test="($viewEditType = 'full')">
						  <input id="size1" type="text" data-mini="true">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeRange1;double;8</xsl:attribute>
							  <xsl:attribute name="value"><xsl:value-of select="@SizeRange1" /></xsl:attribute>
						  </input>
					  </xsl:if>
          </div>
          <div class="ui-block-b">
            <label for="lp1">List Price 1 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizePrice1" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="lp1" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizePrice1;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizePrice1" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="max1">HP (Max PTO HP) 1 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarD1" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="max1" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarD1;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarD1" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="p1">Speed 1 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarA1" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="p1" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarA1;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarA1" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="fe1">Field Efficiency 1 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarB1" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="fe1" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarB1;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarB1" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="epto1">Equiv PTO HP 1 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarC1" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="epto1" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarC1;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarC1" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
      </div>
		  <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="size2">Size (Width) 2 </label>
            <xsl:if test="($viewEditType != 'full')">
						  <xsl:value-of select="@SizeRange2" />
					  </xsl:if>
					  <xsl:if test="($viewEditType = 'full')">
						  <input id="size2" type="text" data-mini="true">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeRange2;double;8</xsl:attribute>
							  <xsl:attribute name="value"><xsl:value-of select="@SizeRange2" /></xsl:attribute>
						  </input>
					  </xsl:if>
          </div>
          <div class="ui-block-b">
            <label for="lp2">List Price 2 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizePrice2" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="lp2" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizePrice2;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizePrice2" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="max2">HP (Max PTO HP) 2 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarD2" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="max2" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarD2;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarD2" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="p2">Speed 2 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarA2" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="p2" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarA2;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarA2" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="fe2">Field Efficiency 2 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarB2" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="fe2" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarB2;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarB2" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="epto2">Equiv PTO HP 2 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarC2" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="epto2" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarC2;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarC2" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
      </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="size3">Size (Width) 3 </label>
            <xsl:if test="($viewEditType != 'full')">
						  <xsl:value-of select="@SizeRange3" />
					  </xsl:if>
					  <xsl:if test="($viewEditType = 'full')">
						  <input id="size3" type="text" data-mini="true">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeRange3;double;8</xsl:attribute>
							  <xsl:attribute name="value"><xsl:value-of select="@SizeRange3" /></xsl:attribute>
						  </input>
					  </xsl:if>
          </div>
          <div class="ui-block-b">
            <label for="lp3">List Price 3 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizePrice3" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="lp3" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizePrice3;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizePrice3" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="max3">HP (Max PTO HP) 3 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarD3" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="max3" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarD3;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarD3" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="p3">Speed 3 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarA3" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="p3" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarA3;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarA3" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="fe3">Field Efficiency 3 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarB3" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="fe3" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarB3;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarB3" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="epto3">Equiv PTO HP 3 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarC3" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="epto3" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarC3;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarC3" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
      </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="size4">Size (Width) 4 </label>
            <xsl:if test="($viewEditType != 'full')">
						  <xsl:value-of select="@SizeRange4" />
					  </xsl:if>
					  <xsl:if test="($viewEditType = 'full')">
						  <input id="size4" type="text" data-mini="true">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeRange4;double;8</xsl:attribute>
							  <xsl:attribute name="value"><xsl:value-of select="@SizeRange4" /></xsl:attribute>
						  </input>
					  </xsl:if>
          </div>
          <div class="ui-block-b">
            <label for="lp4">List Price 4 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizePrice4" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="lp4" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizePrice4;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizePrice4" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="max4">HP (Max PTO HP) 4 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarD4" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="max4" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarD4;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarD4" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="p4">Speed 4 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarA4" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="p4" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarA4;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarA4" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="fe4">Field Efficiency 4 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarB4" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="fe4" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarB4;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarB4" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="epto4">Equiv PTO HP 4 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarC4" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="epto4" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarC4;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarC4" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
      </div>
      <div class="ui-grid-a">
          <div class="ui-block-a">
            <label for="size5">Size (Width) 5 </label>
            <xsl:if test="($viewEditType != 'full')">
						  <xsl:value-of select="@SizeRange5" />
					  </xsl:if>
					  <xsl:if test="($viewEditType = 'full')">
						  <input id="size5" type="text" data-mini="true">
							  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeRange5;double;8</xsl:attribute>
							  <xsl:attribute name="value"><xsl:value-of select="@SizeRange5" /></xsl:attribute>
						  </input>
					  </xsl:if>
          </div>
          <div class="ui-block-b">
            <label for="lp5">List Price 5 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizePrice5" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="lp5" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizePrice5;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizePrice5" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="max5">HP (Max PTO HP) 5 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarD5" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="max5" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarD5;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarD5" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="p5">Speed 5 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarA5" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="p5" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarA5;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarA5" /></xsl:attribute>
							  </input>
						  </xsl:if>
          </div>
          <div class="ui-block-a">
            <label for="fe5">Field Efficiency 5 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarB5" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="fe5" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarB5;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarB5" /></xsl:attribute>
							  </input>
						  </xsl:if>
            </div>
          <div class="ui-block-b">
            <label for="epto5">Equiv PTO HP 5 </label>
            <xsl:if test="($viewEditType != 'full')">
							  <xsl:value-of select="@SizeVarC5" />
						  </xsl:if>
						  <xsl:if test="($viewEditType = 'full')">
							  <input id="epto5" type="text" data-mini="true">
								  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SizeVarC5;double;8</xsl:attribute>
								  <xsl:attribute name="value"><xsl:value-of select="@SizeVarC5" /></xsl:attribute>
							  </input>
						  </xsl:if>
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
			<xsl:choose>
				<xsl:when test="($docToCalcNodeName = 'inputgroup' 
							or $docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries'
							or contains($docToCalcNodeName, 'devpack') 
							or contains($docToCalcNodeName, 'linkedview'))">
					<xsl:if test="(@RepairCost >= 0)
							or (@CapitalRecoveryCost >= 0)">
            <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" data-mini="true">
              <h4 class="ui-bar-b">
                <strong>Operating Costs</strong>
              </h4>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Area <xsl:value-of select="@InputUnit1" /> :
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Area <xsl:value-of select="@InputUnit1" /> :
                  </xsl:if>
                  <xsl:value-of select="@ServiceCapacity" />
                </div>
                <div class="ui-block-b">
                  <xsl:if test="(@UnitGroupId = '1001')">
                    Fuel (gal/hr):
                  </xsl:if>
                  <xsl:if test="(@UnitGroupId != '1001')">
                    Fuel (liters/hr):
                  </xsl:if>
                  <xsl:value-of select="@FuelAmount" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  Fuel Cost: <xsl:value-of select="@FuelCost" />
                </div>
                <div class="ui-block-b">
                  Lube Oil Cost: <xsl:value-of select="@LubeOilCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  Repair Cost: <xsl:value-of select="@RepairCost" />
                </div>
                <div class="ui-block-b">
                  Labor Cost: <xsl:value-of select="@LaborCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <strong>Total Operating Cost ($/hour): </strong>
                </div>
                <div class="ui-block-b">
                  <strong>
                    <xsl:value-of select="@InputPrice1" />
                  </strong>
                </div>
              </div>
              <h4 class="ui-bar-b">
                <strong>Allocated Overhead Costs</strong>
              </h4>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  Capital Recovery Cost: <xsl:value-of select="@CapitalRecoveryCost" />
                </div>
                <div class="ui-block-b">
                  Taxes, Housing, Insurance: <xsl:value-of select="@TaxesHousingInsuranceCost" />
                </div>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <strong>Total Allocated Overhead Cost ($/hour): </strong>
                </div>
                <div class="ui-block-b">
                  <strong>
                    <xsl:value-of select="@InputPrice2" />
                  </strong>
                </div>
              </div>
              <div class="ui-grid-a">
                <!--In order to set a db attribute in a calulator, without 
							having the db property overwrite it, change its name (i.e. MarketValue) 
							and then set the db name (i.e. CAPPrice = MarketValue) when the calculation is run-->
                <div class="ui-block-a">
                  <strong>Capital Cost:</strong>
                  <xsl:value-of select="@InputPrice3" />
                </div>
                <div class="ui-block-b">
                  Capital Unit: <xsl:value-of select="@InputUnit3" />
                </div>
              </div>
            </div>
					</xsl:if>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>A. Select options</strong></h4>
				<div class="ui-field-contain">
          <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Capacity Options:</legend>
            <input type="radio" id="CapacityOptions1" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForCapacity;integer;4</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">1</xsl:attribute>
              <xsl:if test="(@OptionForCapacity = '1')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <xsl:if test="(@UnitGroupId = '1001')">
              <label for="CapacityOptions1" >Area (hours/acre)</label>
            </xsl:if>
            <xsl:if test="(@UnitGroupId != '1001')">
              <label for="CapacityOptions1" >Area (hours/hectare)</label>
            </xsl:if>
            <input type="radio" id="CapacityOptions2" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForCapacity;integer;4</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">2</xsl:attribute>
              <xsl:if test="(@OptionForCapacity = '2')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <xsl:if test="(@UnitGroupId = '1001')">
              <label for="CapacityOptions2" >Material (hours/ton)</label>
            </xsl:if>
            <xsl:if test="(@UnitGroupId != '1001')">
              <label for="CapacityOptions2" >Material (hours/metric ton)</label>
            </xsl:if>
            <input type="radio" id="CapacityOptions3" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForCapacity;integer;4</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">3</xsl:attribute>
              <xsl:if test="(@OptionForCapacity = '3')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <xsl:if test="(@UnitGroupId = '1001')">
              <label for="CapacityOptions3" >Area (acres/hour)</label>
            </xsl:if>
            <xsl:if test="(@UnitGroupId != '1001')">
              <label for="CapacityOptions3" >Area (hectares/hour)</label>
            </xsl:if>
            <input type="radio" id="CapacityOptions4" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForCapacity;integer;4</xsl:attribute>
              </xsl:if>
              <xsl:attribute name="value">4</xsl:attribute>
              <xsl:if test="(@OptionForCapacity = '4')">
                <xsl:attribute name="checked">true</xsl:attribute>
              </xsl:if>
            </input>
            <xsl:if test="(@UnitGroupId = '1001')">
              <label for="CapacityOptions4" >Material (tons/hour)</label>
            </xsl:if>
            <xsl:if test="(@UnitGroupId != '1001')">
              <label for="CapacityOptions4" >Material (metric tons/hour)</label>
            </xsl:if>
          </fieldset>
					</div>
				<div class="ui-field-contain">
          <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Vary Time Options</legend>
						<input type="radio" id="TimeandOutputOptions1" data-mini="true">
							<xsl:if test="($viewEditType = 'full')">
                <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForTime;integer;4</xsl:attribute>
							</xsl:if>
              <xsl:attribute name="value">1</xsl:attribute>
							<xsl:if test="(@OptionForTime = '1')">
								<xsl:attribute name="checked">true</xsl:attribute>
							</xsl:if>
						</input>
						<label for="TimeandOutputOptions1" >Costs Vary Over Time</label>
						<input type="radio" id="TimeandOutputOptions2" data-mini="true">
							<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForTime;integer;4</xsl:attribute>
								</xsl:if>
							<xsl:attribute name="value">2</xsl:attribute>
							<xsl:if test="(@OptionForTime = '2')">
								<xsl:attribute name="checked">true</xsl:attribute>
							</xsl:if>
						</input>
						<label for="TimeandOutputOptions2" >Costs Do Not VOT</label>
				  </fieldset>
        </div>
				<div class="ui-field-contain">
          <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Inflation Options</legend>
							<input type="radio" id="InflationOptions1" data-mini="true">
								<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value">1</xsl:attribute>
								<xsl:if test="(@OptionForInflation = '1')">
									<xsl:attribute name="checked">true</xsl:attribute>
								</xsl:if>
							</input>
							<label for="InflationOptions1" >First Year</label>
							<input type="radio" id="InflationOptions2" data-mini="true">
								<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="value">2</xsl:attribute>
								<xsl:if test="(@OptionForInflation = '2')">
									<xsl:attribute name="checked">true</xsl:attribute>
								</xsl:if>
							</input>
							<label for="InflationOptions2" >All Years</label>
							<input type="radio" id="InflationOptions3" data-mini="true">
								<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForInflation;integer;4</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="value">3</xsl:attribute>
								<xsl:if test="(@OptionForInflation = '3')">
									<xsl:attribute name="checked">true</xsl:attribute>
								</xsl:if>
							</input>
							<label for="InflationOptions3" >Do Not Use</label>
					</fieldset>
        </div>
				<div class="ui-field-contain">
          <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Fuel Options</legend>
							<input type="radio" id="FuelOptions1" data-mini="true">
								<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForFuel;integer;4</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value">1</xsl:attribute>
								<xsl:if test="(@OptionForFuel = '1')">
									<xsl:attribute name="checked">true</xsl:attribute>
								</xsl:if>
							</input>
							<label for="FuelOptions1" >Base on Operation</label>
							<input type="radio" id="FuelOptions2" data-mini="true">
								<xsl:if test="($viewEditType = 'full')">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OptionForFuel;integer;4</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="value">2</xsl:attribute>
								<xsl:if test="(@OptionForFuel = '2')">
									<xsl:attribute name="checked">true</xsl:attribute>
								</xsl:if>
							</input>
							<label for="FuelOptions2" >Base on Enterprise</label>
					</fieldset>
				</div>
        </div>
        <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b"><strong>B. Fill in machinery variables</strong></h4>
				<div>
          <label for="CalculatorName" class="ui-hidden-accessible"></label>
					<input id="CalculatorName" type="text" data-mini="true">
            <xsl:if test="($viewEditType = 'full')">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
            </xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					</input>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblFuelType" >Fuel Type</label>
							<select class="Select225" id="lblFuelType" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FuelType;string;50</xsl:attribute>
								</xsl:if>
                <option>
									<xsl:attribute name="value">none</xsl:attribute>
									<xsl:if test="(@FuelType = 'none')">
										<xsl:attribute name="selected" />
									</xsl:if>none
								</option>
								<option>
									<xsl:attribute name="value">diesel</xsl:attribute>
									<xsl:if test="(@FuelType = 'diesel')">
										<xsl:attribute name="selected" />
									</xsl:if>diesel
								</option>
								<option>
									<xsl:attribute name="value">gas</xsl:attribute>
									<xsl:if test="(@FuelType = 'gas')">
										<xsl:attribute name="selected" />
									</xsl:if>gas
								</option>
								<option>
									<xsl:attribute name="value">lpg</xsl:attribute>
									<xsl:if test="(@FuelType = 'lpg')">
										<xsl:attribute name="selected" />
									</xsl:if>lpg
								</option>
							</select>
					</div>
          <div class="ui-block-b">
          </div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblMarketValue" >Market Value (input.CAPPrice)</label>
					<xsl:choose>
					<xsl:when test="($docToCalcNodeName = 'input' 
							or $docToCalcNodeName = 'inputseries')">
							<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('MarketValue', @MarketValue, 'decimal', '8')"/>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<xsl:choose>
								<xsl:when test="(contains($docToCalcNodeName, 'devpack')
									or contains($docToCalcNodeName, 'linkedview')
									or contains($selectedFileURIPattern, 'temp'))">
									<input id="lblMarketValue" type="text" data-mini="true">
										<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MarketValue;double;8</xsl:attribute>
										<xsl:attribute name="value"><xsl:value-of select="@MarketValue" /></xsl:attribute>
									</input>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('MarketValue', @MarketValue, 'decimal', '8')"/>
								</xsl:otherwise>
							</xsl:choose>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<input id="lblMarketValue" type="text" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;MarketValue;double;8</xsl:attribute>
								</xsl:if>
                <xsl:attribute name="value"><xsl:value-of select="@MarketValue" /></xsl:attribute>
							</input>
						</xsl:otherwise>
						</xsl:choose>
          </div>
          <div class="ui-block-b">
            <label for="lblPlannedUseHours" >Planned Use Hours</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
								<input id="lblPlannedUseHours" type="text" data-mini="true">
									<xsl:attribute name="value"><xsl:value-of select="@PlannedUseHrs" /></xsl:attribute>
								</input>
							</xsl:if>
							<xsl:if test="($viewEditType = 'full')">
								<input id="lblPlannedUseHours" type="text" data-mini="true">
									<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;PlannedUseHrs;integer;4</xsl:attribute>
									<xsl:attribute name="value"><xsl:value-of select="@PlannedUseHrs" /></xsl:attribute>
								</input>
							</xsl:if>
          </div>
        </div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblSalvageValue" >Salvage Value</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblSalvageValue" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@SalvageValue" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblSalvageValue" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;SalvageValue;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@SalvageValue" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblStartingHours" >Starting Hours</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblStartingHours" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@StartingHrs" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblStartingHours" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;StartingHrs;integer;4</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@StartingHrs" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <label for="lblHorsePower" >Horse Power</label>
              <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                <input type="text" id="lblHorsePower" data-mini="true">
                  <xsl:attribute name="value"><xsl:value-of select="@HP" /></xsl:attribute>
                </input>
              </xsl:if>
              <xsl:if test="($viewEditType = 'full')">
                <input type="text" id="lblHorsePower" data-mini="true">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;HP;integer;4</xsl:attribute>
                  <xsl:attribute name="value"><xsl:value-of select="@HP" /></xsl:attribute>
                </input>
              </xsl:if>
            </div>
            <div class="ui-block-b">
              <label for="lblUsefulLifeHours" >Useful Life Hours</label>
              <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                <input type="text" id="lblUsefulLifeHours" data-mini="true">
                  <xsl:attribute name="value"><xsl:value-of select="@UsefulLifeHrs" /></xsl:attribute>
                </input>
              </xsl:if>
              <xsl:if test="($viewEditType = 'full')">
                <input type="text" id="lblUsefulLifeHours" data-mini="true">
                  <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;UsefulLifeHrs;integer;4</xsl:attribute>
                  <xsl:attribute name="value"><xsl:value-of select="@UsefulLifeHrs" /></xsl:attribute>
                </input>
              </xsl:if>
            </div>
          </div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblHPPTOMax" >Max PTO HP</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblHPPTOMax" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@HPPTOMax" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblHPPTOMax" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;HPPTOMax;integer;4</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@HPPTOMax" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblServiceCapacity" >Service Capacity (area covered)</label>
            <input type="text" id="lblServiceCapacity" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@ServiceCapacity" /></xsl:attribute>
							</input>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblHPPTOEquiv" >Equiv PTO HP</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblHPPTOEquiv" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@HPPTOEquiv" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblHPPTOEquiv" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;HPPTOEquiv;integer;4</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@HPPTOEquiv" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblFieldSpeedTypical" >Field Speed Typical</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblFieldSpeedTypical" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@FieldSpeedTypical" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblFieldSpeedTypical" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FieldSpeedTypical;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@FieldSpeedTypical" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblListPriceAdj" >List Price Adj (+)</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblListPriceAdj" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@ListPriceAdj" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblListPriceAdj" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;ListPriceAdj;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@ListPriceAdj" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblWidth" >Width</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblWidth" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="@Width" /></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblWidth" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Width;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="@Width" /></xsl:attribute>
							</input>
						</xsl:if>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblFieldEffTypical" >Field Eff Typical</label>
            <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<input type="text" id="lblFieldEffTypical" data-mini="true">
								<xsl:attribute name="value"><xsl:value-of select="format-number(@FieldEffTypical, '#,###.000')"/></xsl:attribute>
							</input>
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<input type="text" id="lblFieldEffTypical" data-mini="true">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FieldEffTypical;double;8</xsl:attribute>
								<xsl:attribute name="value"><xsl:value-of select="format-number(@FieldEffTypical, '#,###.000')"/></xsl:attribute>
							</input>
						</xsl:if>
					</div>
					<div class="ui-block-b">
						<label for="lblDate" >Date</label>
            <input type="text" id="lblDate" name="lblDate" data-mini="true" disabled="true">
								<xsl:attribute name="value"><xsl:value-of select="@InputDate"/></xsl:attribute>
							</input>
					</div>
				</div>
				<div class="ui-grid-a">
					<div class="ui-block-a">
						<label for="lblLaborType" >Labor Type</label>
            <select class="Select225" id="lblLaborType" data-mini="true">
              <xsl:if test="($viewEditType = 'full')">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborType;string;50</xsl:attribute>
              </xsl:if>
							<option>
								<xsl:attribute name="value">none</xsl:attribute>
								<xsl:if test="(@LaborType = 'none')">
									<xsl:attribute name="selected" />
								</xsl:if>none
							</option>
							<option>
								<xsl:attribute name="value">regular</xsl:attribute>
								<xsl:if test="(@LaborType = 'regular')">
									<xsl:attribute name="selected" />
								</xsl:if>regular
							</option>
							<option>
								<xsl:attribute name="value">machinery</xsl:attribute>
								<xsl:if test="(@LaborType = 'machinery')">
									<xsl:attribute name="selected" />
								</xsl:if>machinery
							</option>
							<option>
								<xsl:attribute name="value">supervisory</xsl:attribute>
								<xsl:if test="(@LaborType = 'supervisory')">
									<xsl:attribute name="selected" />
								</xsl:if>supervisory
							</option>
						</select>
				</div>
				<div class="ui-block-b">
          <label for="lblLaborAmountAdj" >Labor Amount Adj</label>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<input type="text" id="lblLaborAmountAdj" data-mini="true">
							<xsl:attribute name="value"><xsl:value-of select="@LaborAmountAdj" /></xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" id="lblLaborAmountAdj" data-mini="true">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LaborAmountAdj;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="@LaborAmountAdj" /></xsl:attribute>
						</input>
					</xsl:if>
				</div>
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
			</xsl:when>
			<xsl:otherwise>
				<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
				sure this is the right calculator?</h3>
			</xsl:otherwise>
			</xsl:choose>
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
				<li><strong>Step 1. Machinery Constants:</strong> The constants derive from ASABE publication ASAE D497.7 listed below.</li>
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. In the USA, DevTreks recommends 
					using Office of Management and Budget rates for the same year as the date of the input.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 2</h4>
			<ul data-role="listview">
				<li><strong>Step 2. Fuel Prices:</strong> The prices should be as of the date of the input. Prices that won't be used, such as natural gas prices, do not need to be filled out.</li>
				<li><strong>Step 2. Labor Prices:</strong> These prices should reflect actual gross hourly wages for each category of labor.</li>
				<li><strong>Step 2. Tax Percent:</strong> A percent that is multiplied by the market value to derive a tax cost. Similar to mill rates (percent tax per $1000 of assessed value). </li>
				<li><strong>Step 2. Housing Percent:</strong> A percent that is multiplied by the market value to derive a housing cost. </li>
				<li><strong>Step 2. Insure Percent:</strong> A percent that is multiplied by the market value to derive an insurance cost. </li>
        <li><strong>Step 2. Size Ranges:</strong> This information is optional. Machinery selection and scheduling analyzers use these ranges to determine feasible combinations 
          of machinery (i.e. least cost) in operations and components. The numbers entered should be reasonably related, but not equal, to the associated numbers entered in step 3. 
          The size column is width.  The HP column should be maximum PTO horsepower, or horsepower, for power inputs. For nonpower inputs, 
          the number should reflect an associated, typical, power input. The speed and efficiency columns are used with the width column to determine field, or service, capacity. 
          The Equivalent PTO HP is used to determine fuel amounts and costs. If the last three columns are left blank or zero, ratios derived from step 3 will be used in calculations.
          The references below explain how these parameters are used in machinery selection and scheduling.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 3</h4>
			<ul data-role="listview">
				<li><strong>Step 3. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.</li>
        <li><strong>Step 3. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)</li>
        <li><strong>Step 3. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.</li>
				<li><strong>Step 3. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.</li>
				<li><strong>Step 3. Capacity Options:</strong> The materials option is only used when machinery, such as balers, do not cover each square foot of a field.</li>
				<li><strong>Step 3. Vary Time and Output Options:</strong> These options change the the operating costs (fuel, repair, labor, lube). The 'Costs Vary Over Time' option calculates an amortized average annual repair cost adjusted for inflation (see Hallam et al equation 5.18). The 'Costs Do Not Vary Over Time' option calculates repair costs, with no inflation adjustments, using ASABE repair cost equations.</li>
				<li><strong>Step 3. Inflation Options:</strong> These options change the operating and capital recovery costs.</li>
				<li><strong>Step 3. Fuel Options:</strong> Machinery that is being used for specific operations, such as planting, should use the 'Operation' option. Machinery that may be used in several operations should use the 'Enterprise' option.</li>
        <li><strong>Step 3. Market Value:</strong> Before a calculation is run for the first time, this number will be zero. When the calculation is run the parent input's CAPPrice is used as the market value.</li>
				<li><strong>Step 3. List Price Adjustment:</strong> A multiplier that is used to increase the market value to the list price. For example, if the list price of an item of machinery is ten percent more that the market value, enter 10. This number is divided by 100 in the calculations.</li>
				<li><strong>Step 3. Scheduling Efficiency and Labor Adjustment:</strong> A multiplier that is used to adjust the amount of labor needed to use the machinery for the specified area (i.e. acre or hectare). For example, if the time spent setting up the machinery or transporting the machinery adds ten percent to the total amount of time it takes to complete the specified area, enter 10. If the scheduling of the machinery's use often causes the equipment operator to be idle, add an additional adjustment. This number is divided by 100 in the calculations.</li>
        <li><strong>Step 3. Field Efficiency Typical:</strong> "The ratio between the productivity of a machine under field conditions and the theoretical maximum productivity" (see ASABE). In general, we recommend using the default ASABE value that comes from selecting a machinery constant. This number is divided by 100 in the calculations. Note that all fields requiring a percent in DevTreks' calculators should be entered as a whole number (unless the percent is less than 1%).</li>
			</ul>
      <ul data-role="listview">
        <li><strong>Step 3. Area:</strong> Also known as service capacity or effective field capacity, is the number of hours used by the machine per acre (hectare). This number is used to set operating and allocated overhead cost amounts.</li>
        <li><strong>Step 3. Per Hour Costs:</strong> With the exception of the capital cost, all costs are per hour of machinery use.  Total cost per acre is calculated at cost (per hour) x area covered (hours per acre). For example, if an implement is used for .10 hours per acre and the repair cost is $10 per hour, the total repair cost per acre is $1 per acre. </li>
				<li><strong>Step 3. Fuel Cost:</strong> The quantity of fuel consumed per hour (by the power input) multiplied by the fuel price.</li>
				<li><strong>Step 3. Repair Cost:</strong> 'Includes replacement parts, materials, shop expenses, and labor for maintaining a machine in good working order.'(ASABE)</li>
        <li><strong>Step 3. Labor Cost:</strong> The quantity of labor used per hour multiplied by the labor price.</li>
        <li><strong>Step 3. Lube Cost:</strong> 'Oil consumption is defined as the volume per hour of engine crankcase oil replaced at the manufacturers recommended change interval' (ASABE).</li>
        <li><strong>Step 3. Operating Cost:</strong> Costs associated with the resources expended (expendables) during the production cycle.</li>
        <li><strong>Step 3. Capital Recovery Cost:</strong> Annual ownership cost, adjusted for inflation (unless the the no inflation option is chosen). 'This cost reflects the reduction in value of an asset with use and time.' (ASABE)</li>
        <li><strong>Step 3. Taxes, Housing and Insurance Cost:</strong> These are calculated as a percent of the market value of the machinery.</li>
        <li><strong>Step 3. Allocated Overhead Cost:</strong> All costs that are not operating costs (that are not expendables), such as capital costs. </li>
        <li><strong>Step 3. Capital Cost:</strong> Initial market value (purchase price) of the machinery.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
      <h4 class="ui-bar-b"><strong>References</strong></h4>
      <ul data-role="listview">
        <li><strong>American Society of Agricultural and Biological Engineers, ASAE D497.7 MAR2011</strong> Agricultural Machinery Management Data</li>
        <li><strong>American Society of Agricultural and Biological Engineers, ASAE EP496.3 FEB2006 (R2011)</strong> Agricultural Machinery Management</li>
				<li><strong>Hallam, Eidman, Morehart and Klonsky (editors) </strong> Commodity Cost and Returns Estimation Handbook, Staff General Research Papers, Iowa State University, Department of Economics, 1999</li>
			</ul>
      </div>
		</div>
		</xsl:if>
</xsl:template>
</xsl:stylesheet>
  