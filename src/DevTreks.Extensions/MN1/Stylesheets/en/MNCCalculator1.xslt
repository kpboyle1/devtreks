<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8"/>
	<!-- pass in params -->
	<!--array holding references to constants, locals, lists ...-->
	<xsl:param name="linkedListsArray"/>
	<!-- calcs -->
	<xsl:param name="saveMethod"/>
	<!-- default linked view -->
	<xsl:param name="defaultLinkedViewId"/>
	<!-- the last step is used to display different parts of the calculator-->
	<xsl:param name="lastStepNumber"/>
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType"/>
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType"/>
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub"/>
	<!-- which node to start with? -->
	<xsl:param name="nodeName"/>
	<!-- which view to use? -->
	<xsl:param name="viewEditType"/>
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole"/>
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern"/>
	<!-- the addin being used -->
	<xsl:param name="calcDocURI"/>
	<!-- the node being calculated (custom docs' nodename can be a devpack node, while this might be budgetgroup) -->
	<xsl:param name="docToCalcNodeName"/>
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams"/>
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName"/>
	<!-- which network is this doc from? -->
	<xsl:param name="networkId"/>
	<!-- what is the start row? -->
	<xsl:param name="startRow"/>
	<!-- what is the end row? -->
	<xsl:param name="endRow"/>
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams"/>
	<!-- what is the club's email? -->
	<xsl:param name="clubEmail"/>
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath"/>
	<!-- init html -->
	<xsl:template match="@*|/|node()"/>
	<xsl:template match="/">
		<div class="box">
			<xsl:if test="($serverActionType = 'linkedviews')">
				<div id="stepsmenu">
					<xsl:value-of select="DisplayDevPacks:WriteMenuSteps('3')"/>
				</div>
			</xsl:if>
			<xsl:apply-templates select="root"/>
      <div>
				<br />
				<strong>Current view of document</strong>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="root">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
		<div id="divstepzero">
      <h4 class="ui-bar-b"><strong>Food Nutrition USDA Standard Reference Input Calculation View</strong></h4>
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
      <h4 class="ui-bar-b">
					<strong>
						<xsl:value-of select="linkedview[@Id=$linkedviewid]/@CalculatorName" />
					</strong>
			</h4>
			<p>
					<strong>Introduction</strong>
					<br />
					This calculator calculates the nutritional value of inputs per actual serving size 
							based on the USDA National Nutrient Database for Standard Reference, Release 24.
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
		<xsl:apply-templates select="linkedview"/>
	</xsl:template>
	<xsl:template match="linkedview">
		<xsl:variable name="linkedviewid"><xsl:value-of select="DisplayDevPacks:GetURIPatternPart($calcDocURI,'id')" /></xsl:variable>
	<xsl:if test="(@Id = $linkedviewid) or (@Id = 1)">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@CalculatorName,@Id,$networkId,local-name(),'')"/></xsl:variable>
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
			<xsl:value-of select="DisplayDevPacks:WriteSelectListsForLocals(
						$linkedListsArray, $searchurl, $serverSubActionType, 'full', 
						@RealRate, @NominalRate, @UnitGroupId, @CurrencyGroupId,
						@RealRateId, @NominalRateId, @RatingGroupId)"/>
		</div>
		<div id="divsteptwo">
			<h4 class="ui-bar-b"><strong>Step 2 of 3. Calculate</strong></h4>
			<xsl:variable name="calcParams3">'&amp;step=stepthree<xsl:value-of select="$calcParams"/>'</xsl:variable>
      <xsl:if test="($viewEditType = 'full')">
				<xsl:value-of select="DisplayDevPacks:WriteCalculateButtons($selectedFileURIPattern, $contenturipattern, $calcParams3)"/>
			</xsl:if>
			<xsl:if test="($lastStepNumber = 'stepthree')">
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
      <xsl:value-of select="DisplayDevPacks:WriteAlternatives($searchurl, $viewEditType,
            @AlternativeType, @TargetType)"/>
      </div>
		  <xsl:choose>
				<xsl:when test="($docToCalcNodeName = 'inputgroup')">
						<div>
              <label for="CalculatorName" class="ui-hidden-accessible"></label>
					    <input id="CalculatorName" type="text" data-mini="true">
                <xsl:if test="($viewEditType = 'full')">
							    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
                </xsl:if>
						    <xsl:attribute name="value"><xsl:value-of select="@CalculatorName" /></xsl:attribute>
					    </input>
				  </div>
          <div >
								<label for="lblSRDescription">Label and Description: </label>
								<xsl:value-of select="@SRLabel" /> : <xsl:value-of select="@SRDescription" />
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
					</xsl:when>
					<xsl:when test="($docToCalcNodeName = 'input' 
							  or $docToCalcNodeName = 'inputseries'
							  or contains($docToCalcNodeName, 'devpack') 
							  or contains($docToCalcNodeName, 'linkedview'))">
						<xsl:if test="(@ActualServingSize >= 0)">
              <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d">
                <h4 class="ui-bar-b">
                  <strong>Actual Serving Size</strong>
                </h4>
                <div class="ui-grid-a">
                  <div class="ui-block-a">
                    <strong>Actual Servings Per Container</strong>:
                    <xsl:value-of select="@ActualServingsPerContainer"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>USDA Servings Per Container</strong>:
                    <xsl:value-of select="@TypicalServingsPerContainer"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Serving Size</strong>:
                    <xsl:value-of select="@ActualServingSize"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Serving Units</strong>:
                    <xsl:value-of select="@ServingSizeUnit"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Total Cost Per Actual Serving</strong>:
                    <xsl:value-of select="@ServingCost"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Container Cost</strong>:
                    <xsl:value-of select="@ContainerPrice"/>
                  </div>
                </div>
                <h4 class="ui-bar-b">
                  <strong>Nutritional Composition of Actual Serving Size</strong>
                </h4>
                <div class="ui-grid-a">
                  <div class="ui-block-a">
                    <strong>Actual Water g</strong>:
                    <xsl:value-of select="@ActualWater_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Energy Kcal</strong>:
                    <xsl:value-of select="@ActualEnerg_Kcal"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Protein g</strong>:
                    <xsl:value-of select="@ActualProtein_g"/>
                  </div>
                  <div class="ui-block-b">
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Lipid Tot g</strong>:
                    <xsl:value-of select="@ActualLipid_Tot_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Ash g</strong>:
                    <xsl:value-of select="@ActualAsh_g"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Carbohydrate g</strong>:
                    <xsl:value-of select="@ActualCarbohydrt_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Fiber (TD) g</strong>:
                    <xsl:value-of select="@ActualFiber_TD_g"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Sugar (Tot) g</strong>:
                    <xsl:value-of select="@ActualSugar_Tot_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Calcium mg</strong>:
                    <xsl:value-of select="@ActualCalcium_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Iron mg</strong>:
                    <xsl:value-of select="@ActualIron_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Magnesium mg</strong>:
                    <xsl:value-of select="@ActualMagnesium_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Phosphorus mg</strong>:
                    <xsl:value-of select="@ActualPhosphorus_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Potassium mg</strong>:
                    <xsl:value-of select="@ActualPotassium_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Sodium mg</strong>:
                    <xsl:value-of select="@ActualSodium_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Zinc mg</strong>:
                    <xsl:value-of select="@ActualZinc_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Copper mg</strong>:
                    <xsl:value-of select="@ActualCopper_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Manganese mg</strong>:
                    <xsl:value-of select="@ActualManganese_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Selenium pg</strong>:
                    <xsl:value-of select="@ActualSelenium_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Vitamin C mg</strong>:
                    <xsl:value-of select="@ActualVit_C_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Thiamin mg</strong>:
                    <xsl:value-of select="@ActualThiamin_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Riboflavin mg</strong>:
                    <xsl:value-of select="@ActualRiboflavin_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Niacin mg</strong>:
                    <xsl:value-of select="@ActualNiacin_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Panto mg</strong>:
                    <xsl:value-of select="@ActualPanto_Acid_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Vitamin B6 mg</strong>:
                    <xsl:value-of select="@ActualVit_B6_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Folate (Tot) pg</strong>:
                    <xsl:value-of select="@ActualFolate_Tot_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Folic Acid pg</strong>:
                    <xsl:value-of select="@ActualFolic_Acid_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Food Folate pg</strong>:
                    <xsl:value-of select="@ActualFood_Folate_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Folate (DFE) pg</strong>:
                    <xsl:value-of select="@ActualFolate_DFE_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Choline (Tot) mg</strong>:
                    <xsl:value-of select="@ActualCholine_Tot_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Vitamin B12 pg</strong>:
                    <xsl:value-of select="@ActualVit_B12_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Vitamin A (IU)</strong>:
                    <xsl:value-of select="@ActualVit_A_IU"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Vitamin A (RAE)</strong>:
                    <xsl:value-of select="@ActualVit_A_RAE"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Retinol pg</strong>:
                    <xsl:value-of select="@ActualRetinol_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Alpha Carotene pg</strong>:
                    <xsl:value-of select="@ActualAlpha_Carot_pg"/>
                  </div>
                  <div class="ui-block-b">
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Beta Carotene pg</strong>:
                    <xsl:value-of select="@ActualBeta_Carot_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Beta Crypt pg</strong>:
                    <xsl:value-of select="@ActualBeta_Crypt_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Lycopene pg</strong>:
                    <xsl:value-of select="@ActualLycopene_pg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Lut Zea pg</strong>:
                    <xsl:value-of select="@ActualLut_Zea_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Vitamin E mg</strong>:
                    <xsl:value-of select="@ActualVit_E_mg"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Vitamin D pg</strong>:
                    <xsl:value-of select="@ActualVit_D_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual ViVitamin D (IU)</strong>:
                    <xsl:value-of select="@ActualViVit_D_IU"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Vitamin K pg</strong>:
                    <xsl:value-of select="@ActualVit_K_pg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Fatty Acid Sat g</strong>:
                    <xsl:value-of select="@ActualFA_Sat_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Fatty Acid Mono g</strong>:
                    <xsl:value-of select="@ActualFA_Mono_g"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Fatty Acid Poly g</strong>:
                    <xsl:value-of select="@ActualFA_Poly_g"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Cholesterol mg</strong>:
                    <xsl:value-of select="@ActualCholestrl_mg"/>
                  </div>
                  <div class="ui-block-a">
                    <strong>Actual Extra 1</strong>:
                    <xsl:value-of select="@ActualExtra1"/>
                  </div>
                  <div class="ui-block-b">
                    <strong>Actual Extra 2</strong>:
                    <xsl:value-of select="@ActualExtra2"/>
                  </div>
                </div>
              </div>
						</xsl:if>
            <div data-role="collapsible"  data-theme="b" data-content-theme="d">
              <h4 class="ui-bar-b">
                <strong>Nutrition Calculator Variables</strong>
              </h4>
              <div>
                <label for="CalculatorName" class="ui-hidden-accessible"></label>
                <input id="CalculatorName" type="text" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorName;string;75</xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="value"> <xsl:value-of select="@CalculatorName" /></xsl:attribute>
                </input>
              </div>
              <div >
                <label for="lblSRDescription">Label and Description: </label>
                <xsl:value-of select="@SRLabel" /> : <xsl:value-of select="@SRDescription" />
              </div>
              <div >
                <label for="lblDescription">Description</label>
                <textarea class="Text75H100PCW" id="lblDescription" data-mini="true">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;CalculatorDescription;string;255 </xsl:attribute>
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
              <div>
                <label for="lblWeightToUseType" >
                  <strong>Typical USDA Serving Size and Unit</strong>
                </label>
                <select class="Select225" id="lblWeightToUseType">
                  <xsl:if test="($viewEditType = 'full')">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;WeightToUseType;string;15</xsl:attribute>
                  </xsl:if>
                  <option>
                    <xsl:attribute name="value">weight1</xsl:attribute>
                    <xsl:if test="(@WeightToUseType = 'weight1')">
                      <xsl:attribute name="selected"/>
                    </xsl:if>weight1 (<xsl:value-of select="@TypWt1_Amount"/> - <xsl:value-of select="@TypWt1_Unit"/>)
                  </option>
                  <option>
                    <xsl:attribute name="value">weight1metric</xsl:attribute>
                    <xsl:if test="(@WeightToUseType = 'weight1metric')">
                      <xsl:attribute name="selected"/>
                    </xsl:if>weight1metric (<xsl:value-of select="@GmWt_1"/> - grams)
                  </option>
                  <option>
                    <xsl:attribute name="value">weight2</xsl:attribute>
                    <xsl:if test="(@WeightToUseType = 'weight2')">
                      <xsl:attribute name="selected"/>
                    </xsl:if>weight2 (<xsl:value-of select="@TypWt2_Amount"/> - <xsl:value-of select="@TypWt2_Unit"/>)
                  </option>
                  <option>
                    <xsl:attribute name="value">weight2metric</xsl:attribute>
                    <xsl:if test="(@WeightToUseType = 'weight2metric')">
                      <xsl:attribute name="selected"/>
                    </xsl:if>weight2metric (<xsl:value-of select="@GmWt_2"/> - grams)
                  </option>
                </select>
              </div>
              <div>
                <label for="lblActualServingSize">
                  <strong>Actual Serving Size</strong>
                </label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="lblActualServingSize">
                    <xsl:attribute name="value"><xsl:value-of select="@ActualServingSize"/></xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="lblActualServingSize">
                    <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ActualServingSize;decimal;8</xsl:attribute>
                    <xsl:attribute name="value"><xsl:value-of select="@ActualServingSize"/></xsl:attribute>
                  </input>
                </xsl:if>
              </div>
              <div>
                <label for="ContainerSizeInSSUnits">
                  <strong>
                    Container Size in USDA Serving Size Units
                  </strong>
                </label>
                <xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
                  <input type="text" id="ContainerSizeInSSUnits">
                    <xsl:attribute name="value"><xsl:value-of select="@ContainerSizeInSSUnits"/></xsl:attribute>
                  </input>
                </xsl:if>
                <xsl:if test="($viewEditType = 'full')">
                  <input type="text" id="ContainerSizeInSSUnits">
                    <xsl:attribute name="name">
                      <xsl:value-of select="$searchurl"/>;ContainerSizeInSSUnits;decimal;8</xsl:attribute>
                    <xsl:attribute name="value"> <xsl:value-of select="@ContainerSizeInSSUnits"/></xsl:attribute>
                  </input>
                </xsl:if>
              </div>
              <div class="ui-grid-a">
                <div class="ui-block-a">
                  <label for="ContainerPrice">
                    <strong>
                      Container Price
                    </strong>
                  </label>
                    <input type="text" id="ContainerPrice">
                      <xsl:if test="($viewEditType = 'full')">
                        <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ContainerPrice;decimal;8</xsl:attribute>
                      </xsl:if>
                      <xsl:attribute name="value"><xsl:value-of select="@ContainerPrice"/></xsl:attribute>
                    </input>
                  
                </div>
                <div class="ui-block-b">
                  <label for="ContainerUnit">
                    <strong>
                      Container Unit
                    </strong>
                  </label>
                  <input type="text" id="ContainerUnit">
                    <xsl:if test="($viewEditType = 'full')">
                      <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;ContainerUnit;string;25</xsl:attribute>
                    </xsl:if>
                      <xsl:attribute name="value"><xsl:value-of select="@ContainerUnit"/></xsl:attribute>
                  </input>
                </div>
                <div class="ui-block-a">
                  <label for="Extra1">
                    <strong>
                      Extra 1
                    </strong>
                  </label>
                  <input type="text" id="Extra1">
                    <xsl:if test="($viewEditType = 'full')">
                      <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Extra1;double;8</xsl:attribute>
                    </xsl:if>
                    <xsl:attribute name="value"><xsl:value-of select="@Extra1"/></xsl:attribute>
                  </input>
                </div>
                <div class="ui-block-b">
                  <label for="Extra2">
                    <strong>
                      Extra 2
                    </strong>
                  </label>
                  <input type="text" id="Extra2">
                    <xsl:if test="($viewEditType = 'full')">
                      <xsl:attribute name="name"><xsl:value-of select="$searchurl"/>;Extra2;double;8</xsl:attribute>
                    </xsl:if>
                    <xsl:attribute name="value"><xsl:value-of select="@Extra2"/></xsl:attribute>
                  </input>
                </div>
              </div>
            </div>
					</xsl:when>
					<xsl:otherwise>
						<h3>This input calculator does not appear appropriate for the document being analyzed. Are you 
						sure this is the right calculator?</h3>
					</xsl:otherwise>
				</xsl:choose>
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
					<xsl:variable name="calcParams4a">'&amp;step=stepfour&amp;savemethod=calcs<xsl:value-of select="$calcParams"/>'</xsl:variable>
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
      <br /><br />
		</div>
		<div id="divsteplast">
			<h4 class="ui-bar-b"><strong>Instructions </strong></h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
			<h4>Step 1</h4>
			<ul data-role="listview">
				<li><strong>Step 1. Real and Nominal Rates:</strong> The nominal rate includes inflation while the real rate does not. In the USA, DevTreks recommends 
					using Office of Management and Budget rates for the same year as the date of the input.</li>
			</ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
        <h4>Step 2</h4>
        <ul data-role="listview">
          <li>
            <strong>Step 2. Use Same Calculator Pack In Descendants?:</strong> True to insert or update this same calculator in children.
          </li>
          <li>
            <strong>Step 2. Overwrite Descendants?:</strong> True to insert or update all of the attributes of this same calculator in all children. False only updates children attributes that are controlled by the developer of this calculator (i.e. version, stylehsheet name, relatedcalculatorstype ...)
          </li>
          <li>
            <strong>Step 2. What If Tag Name:</strong> Instructional videos explaining the use of what-if scenario calculations should be viewed before changing or using this parameter.
          </li>
          <li>
            <strong>Step 2. Related Calculators Type:</strong> When the Use Same Calculator Pack in Descendant is true, uses this value to determine which descendant calculator to update. Inserts a new descendant when no descendant has this same name. Updates the descendant that has this same name.
          </li>
          <li>
            <strong>Step 3. Typical USDA Serving Size and Unit:</strong> The USDA food nutrient database contains two typical household portion, or serving, sizes. Either size can be chosen using metric or standard USA imperial units of measurement. Weight 1 is larger, and contains more measurements, than weight 2. Use weight 1 as the default.
          </li>
          <li>
            <strong>Step 3. Actual Serving Size:</strong> Adjust the Typical USDA Serving Size to the actual serving size consumed in a typical meal. For example, if the typical serving size is 3 oz, but 4 oz are actually consumed in a typical meal, enter 4. In order to keep this a unit cost, the typical meal should not be tied to one particular individual or age group.
          </li>
          <li>
            <strong>Step 3. Container Size and Unit:</strong> Enter the size and unit of measurement of the food container holding this food item. Convert the containter units of measurement to the same units found in the typical serving size units.
          </li>
          <li>
            <strong>Step 3. Container Price and CAP Price:</strong>  The container price is the grocery store price of the container holding this food item. It will be added to the input's CAP Price after saving the calculations.
          </li>
          <li>
            <strong>Step 3. Extra 1 and 2:</strong> Optional, extra nutritional properties of this food item (see the accompanying reference).
          </li>
          <li>
            <strong>Step 3. OC Amount:</strong> The per unit amount of the food item will be 1 (unit serving size portion). This quantity can be adjusted when the input is added to budgets.
          </li>
          <li>
            <strong>Step 3. OC Price:</strong> The per unit cost of the food item is calculated automatically.
          </li>
          <li>
            <strong>Step 3. OC Unit:</strong> This unit is entered automatically from the ActualServingSize and the Typical USDA Serving Size selection.
          </li>
          <li>
            <strong>Step 3. Serving Cost:</strong> The cost of the food item uses the actual serving size in the calculation: ActualServingSize * OCPrice.
          </li>
          <li>
            <strong>Step 3. Food Nutrition Composition:</strong> The starting food nutrients come directly from the USDA National Nutrient Database for Standard
            Reference, Release 24. The food nutrient composition of the 'Actual Serving Size' is computed by the calculator. The calculations use formulas found in the reference document for this tutorial.
          </li>
        </ul>
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d">
        <h4 class="ui-bar-b">
          <strong>References</strong>
        </h4>
        <ul data-role="listview">
          <li>
            <strong>USDA, ARS</strong> Composition of Foods Raw, Processed, Prepared USDA National Nutrient Database for Standard
            Reference, Release 24. September, 2011. (http://www.ars.usda.gov/nutrientdata)
          </li>
          <li>
            <strong>USDA and United States Department of Health and Human Services.</strong> Dietary Guidelines for Americans, 2010. 7th Edition. Washington DC. US GPO, December, 2010
          </li>
        </ul>
      </div>
		</div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>
