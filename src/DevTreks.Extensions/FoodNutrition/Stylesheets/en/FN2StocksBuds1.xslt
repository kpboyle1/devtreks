<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
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
	<!-- the node being calculated-->
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
	<!-- what is the guide's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<xsl:apply-templates select="root" />
	</xsl:template>
	<xsl:template match="root">
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="budgetgroup" />
			<div>
				<a id="aFeedback" name="Feedback">
					<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
					Feedback About <xsl:value-of select="$selectedFileURIPattern" />
				</a>
      </div>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<h4 class="ui-bar-b">
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
		<h4>
      <strong>Budget Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<h4>
      <strong>Budget</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@EnterpriseName" />
    </h4>
    <!--<div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="budgetoutcomes" />-->
    <div>
      <strong>Operations</strong>
    </div>
		<xsl:apply-templates select="budgetoperations" />
    <h4>
      Time Period Totals
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
    <h4>
      <strong>Operation</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Benefits</strong>
        </h4>
      </div>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Costs</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Container Size : <xsl:value-of select="@TContainerSizeUsingServingSizeUnit" />
          </div>
          <div class="ui-block-b">
            Container Cost : <xsl:value-of select="@TMarketValue" />
          </div>
          <div class="ui-block-a">
            USDA Servings Per Container : <xsl:value-of select="@TTypicalServingsPerContainer" />
          </div>
          <div class="ui-block-b">
            Actual Servings Per Container : <xsl:value-of select="@TActualServingsPerContainer" />
          </div>
          <div class="ui-block-a">
           Serving Size Unit : <xsl:value-of select="@TServingSizeUnit" />
          </div>
          <div class="ui-block-b">
           Actual Serving Size : <xsl:value-of select="@TActualServingSize" />
          </div>
          <div class="ui-block-a">
           Actual Water : <xsl:value-of select="@TWater_g" />
          </div>
          <div class="ui-block-b">
           Actual Energy (calories) : <xsl:value-of select="@TEnerg_Kcal" />
          </div>
          <div class="ui-block-a">
            Actual Lipid : <xsl:value-of select="@TLipid_Tot_g" />
          </div>
          <div class="ui-block-b">
            Actual Ash : <xsl:value-of select="@TAsh_g" />
          </div>
          <div class="ui-block-a">
            Actual Carbohydrate :  <xsl:value-of select="@TCarbohydrt_g" />
          </div>
          <div class="ui-block-b">
           Actual Fiber (TD) : <xsl:value-of select="@TFiber_TD_g" />
          </div>
          <div class="ui-block-a">
            Actual Sugar (Tot) : <xsl:value-of select="@TSugar_Tot_g" />
          </div>
          <div class="ui-block-b">
            Actual Calcium : <xsl:value-of select="@TCalcium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Iron : <xsl:value-of select="@TIron_mg" />
          </div>
          <div class="ui-block-b">
           Actual Magnesium : <xsl:value-of select="@TMagnesium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Phosphorus : <xsl:value-of select="@TPhosphorus_mg" />
          </div>
          <div class="ui-block-b">
            Actual Potassium : <xsl:value-of select="@TPotassium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Sodium : <xsl:value-of select="@TSodium_mg" />
          </div>
          <div class="ui-block-b">
            Actual Zinc : <xsl:value-of select="@TZinc_mg" />
          </div>
          <div class="ui-block-a">
            Actual Copper : <xsl:value-of select="@TCopper_mg" />
          </div>
          <div class="ui-block-b">
           Actual Manganese : <xsl:value-of select="@TManganese_mg" />
          </div>
          <div class="ui-block-a">
           Actual Selenium : <xsl:value-of select="@TSelenium_pg" />
          </div>
          <div class="ui-block-b">
           Actual Vitamin C : <xsl:value-of select="@TVit_C_mg" />
          </div>
          <div class="ui-block-a">
            Actual Thiamin : <xsl:value-of select="@TThiamin_mg" />
          </div>
          <div class="ui-block-b">
            Actual Riboflavin : <xsl:value-of select="@TRiboflavin_mg" />
          </div>
          <div class="ui-block-a">
            Actual Niacin : <xsl:value-of select="@TNiacin_mg" />
          </div>
          <div class="ui-block-b">
           Actual Panto : <xsl:value-of select="@TPanto_Acid_mg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin B6 : <xsl:value-of select="@TVit_B6_mg" />
          </div>
          <div class="ui-block-b">
            Actual Folate (Tot) : <xsl:value-of select="@TFolate_Tot_pg" />
          </div>
          <div class="ui-block-a">
           Actual Folic Acid : <xsl:value-of select="@TFolic_Acid_pg" />
          </div>
          <div class="ui-block-b">
           Actual Food Folate : <xsl:value-of select="@TFood_Folate_pg" />
          </div>
          <div class="ui-block-a">
            Actual Folate (DFE) : <xsl:value-of select="@TFolate_DFE_pg" />
          </div>
          <div class="ui-block-b">
           Actual Choline (Tot) : <xsl:value-of select="@TCholine_Tot_mg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin B12 : <xsl:value-of select="@TVit_B12_pg" />
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
           Actual Vitamin A (IU) : <xsl:value-of select="@TVit_A_IU" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin A (RAE) : <xsl:value-of select="@TVit_A_RAE" />
          </div>
          <div class="ui-block-a">
            Actual Retinol : <xsl:value-of select="@TRetinol_pg" />
          </div>
          <div class="ui-block-b">
           Actual Alpha Carotene : <xsl:value-of select="@TAlpha_Carot_pg" />
          </div>
          <div class="ui-block-a">
            Actual Beta Carotene : <xsl:value-of select="@TBeta_Carot_pg" />
          </div>
          <div class="ui-block-b">
            Actual Beta Crypt : <xsl:value-of select="@TBeta_Crypt_pg" />
          </div>
          <div class="ui-block-a">
           Actual Lycopene : <xsl:value-of select="@TLycopene_pg" />
          </div>
          <div class="ui-block-b">
            Actual Lut Zea : <xsl:value-of select="@TLut_Zea_pg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin E : <xsl:value-of select="@TVit_E_mg" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin D : <xsl:value-of select="@TVit_D_pg" />
          </div>
          <div class="ui-block-a">
           Actual ViVitamin D (IU) : <xsl:value-of select="@TViVit_D_IU" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin K : <xsl:value-of select="@TVit_K_pg" />
          </div>
          <div class="ui-block-a">
            Actual Fatty Acid Sat : <xsl:value-of select="@TFA_Sat_g" />
          </div>
          <div class="ui-block-b">
            Actual Fatty Acid Mono : <xsl:value-of select="@TFA_Mono_g" />
          </div>
          <div class="ui-block-a">
           Actual Fatty Acid Poly : <xsl:value-of select="@TFA_Poly_g" />
          </div>
          <div class="ui-block-b">
            Actual Cholesterol : <xsl:value-of select="@TCholestrl_mg" />
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Container Size : <xsl:value-of select="@ContainerSizeUsingServingSizeUnit" />
          </div>
          <div class="ui-block-b">
            Container Cost : <xsl:value-of select="@MarketValue" />
          </div>
          <div class="ui-block-a">
            USDA Servings Per Container : <xsl:value-of select="@TypicalServingsPerContainer" />
          </div>
          <div class="ui-block-b">
            Actual Servings Per Container : <xsl:value-of select="@ActualServingsPerContainer" />
          </div>
          <div class="ui-block-a">
           Serving Size Unit : <xsl:value-of select="@ServingSizeUnit" />
          </div>
          <div class="ui-block-b">
           Actual Serving Size : <xsl:value-of select="@ActualServingSize" />
          </div>
          <div class="ui-block-a">
           Actual Water : <xsl:value-of select="@ActualWater_g" />
          </div>
          <div class="ui-block-b">
           Actual Energy (calories) : <xsl:value-of select="@ActualEnerg_Kcal" />
          </div>
          <div class="ui-block-a">
            Actual Lipid : <xsl:value-of select="@ActualLipid_Tot_g" />
          </div>
          <div class="ui-block-b">
            Actual Ash : <xsl:value-of select="@ActualAsh_g" />
          </div>
          <div class="ui-block-a">
            Actual Carbohydrate :  <xsl:value-of select="@ActualCarbohydrt_g" />
          </div>
          <div class="ui-block-b">
           Actual Fiber (TD) : <xsl:value-of select="@ActualFiber_TD_g" />
          </div>
          <div class="ui-block-a">
            Actual Sugar (Tot) : <xsl:value-of select="@ActualSugar_Tot_g" />
          </div>
          <div class="ui-block-b">
            Actual Calcium : <xsl:value-of select="@ActualCalcium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Iron : <xsl:value-of select="@ActualIron_mg" />
          </div>
          <div class="ui-block-b">
           Actual Magnesium : <xsl:value-of select="@ActualMagnesium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Phosphorus : <xsl:value-of select="@ActualPhosphorus_mg" />
          </div>
          <div class="ui-block-b">
            Actual Potassium : <xsl:value-of select="@ActualPotassium_mg" />
          </div>
          <div class="ui-block-a">
            Actual Sodium : <xsl:value-of select="@ActualSodium_mg" />
          </div>
          <div class="ui-block-b">
            Actual Zinc : <xsl:value-of select="@ActualZinc_mg" />
          </div>
          <div class="ui-block-a">
            Actual Copper : <xsl:value-of select="@ActualCopper_mg" />
          </div>
          <div class="ui-block-b">
           Actual Manganese : <xsl:value-of select="@ActualManganese_mg" />
          </div>
          <div class="ui-block-a">
           Actual Selenium : <xsl:value-of select="@ActualSelenium_pg" />
          </div>
          <div class="ui-block-b">
           Actual Vitamin C : <xsl:value-of select="@ActualVit_C_mg" />
          </div>
          <div class="ui-block-a">
            Actual Thiamin : <xsl:value-of select="@ActualThiamin_mg" />
          </div>
          <div class="ui-block-b">
            Actual Riboflavin : <xsl:value-of select="@ActualRiboflavin_mg" />
          </div>
          <div class="ui-block-a">
            Actual Niacin : <xsl:value-of select="@ActualNiacin_mg" />
          </div>
          <div class="ui-block-b">
           Actual Panto : <xsl:value-of select="@ActualPanto_Acid_mg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin B6 : <xsl:value-of select="@ActualVit_B6_mg" />
          </div>
          <div class="ui-block-b">
            Actual Folate (Tot) : <xsl:value-of select="@ActualFolate_Tot_pg" />
          </div>
          <div class="ui-block-a">
           Actual Folic Acid : <xsl:value-of select="@ActualFolic_Acid_pg" />
          </div>
          <div class="ui-block-b">
           Actual Food Folate : <xsl:value-of select="@ActualFood_Folate_pg" />
          </div>
          <div class="ui-block-a">
            Actual Folate (DFE) : <xsl:value-of select="@ActualFolate_DFE_pg" />
          </div>
          <div class="ui-block-b">
           Actual Choline (Tot) : <xsl:value-of select="@ActualCholine_Tot_mg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin B12 : <xsl:value-of select="@ActualVit_B12_pg" />
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
           Actual Vitamin A (IU) : <xsl:value-of select="@ActualVit_A_IU" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin A (RAE) : <xsl:value-of select="@ActualVit_A_RAE" />
          </div>
          <div class="ui-block-a">
            Actual Retinol : <xsl:value-of select="@ActualRetinol_pg" />
          </div>
          <div class="ui-block-b">
           Actual Alpha Carotene : <xsl:value-of select="@ActualAlpha_Carot_pg" />
          </div>
          <div class="ui-block-a">
            Actual Beta Carotene : <xsl:value-of select="@ActualBeta_Carot_pg" />
          </div>
          <div class="ui-block-b">
            Actual Beta Crypt : <xsl:value-of select="@ActualBeta_Crypt_pg" />
          </div>
          <div class="ui-block-a">
           Actual Lycopene : <xsl:value-of select="@ActualLycopene_pg" />
          </div>
          <div class="ui-block-b">
            Actual Lut Zea : <xsl:value-of select="@ActualLut_Zea_pg" />
          </div>
          <div class="ui-block-a">
            Actual Vitamin E : <xsl:value-of select="@ActualVit_E_mg" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin D : <xsl:value-of select="@ActualVit_D_pg" />
          </div>
          <div class="ui-block-a">
           Actual ViVitamin D (IU) : <xsl:value-of select="@ActualViVit_D_IU" />
          </div>
          <div class="ui-block-b">
            Actual Vitamin K : <xsl:value-of select="@ActualVit_K_pg" />
          </div>
          <div class="ui-block-a">
            Actual Fatty Acid Sat : <xsl:value-of select="@ActualFA_Sat_g" />
          </div>
          <div class="ui-block-b">
            Actual Fatty Acid Mono : <xsl:value-of select="@ActualFA_Mono_g" />
          </div>
          <div class="ui-block-a">
           Actual Fatty Acid Poly : <xsl:value-of select="@ActualFA_Poly_g" />
          </div>
          <div class="ui-block-b">
            Actual Cholesterol : <xsl:value-of select="@ActualCholestrl_mg" />
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
	      </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
