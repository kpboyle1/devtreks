<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
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
			<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <h4>
      <strong>Investment Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@Name" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="investmentoutcomes" />
    <div>
      <strong>Components</strong>
    </div>
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<h4>
      <strong>Component</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="root/linkedview">
    <xsl:param name="localName" />
    <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Nutrition Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Container Size : <xsl:value-of select="@TContainerSizeInSSUnits" />
        </div>
        <div class="ui-block-b">
          Serving Cost : <xsl:value-of select="@TServingCost" />
        </div>
        <div class="ui-block-a">
          USDA Servings Per Cont : <xsl:value-of select="@TTypicalServingsPerContainer" />
        </div>
        <div class="ui-block-b">
          Servings Per Cont : <xsl:value-of select="@TActualServingsPerContainer" />
        </div>
        <div class="ui-block-a">
          Serving Size Unit : <xsl:value-of select="@TServingSizeUnit" />
        </div>
        <div class="ui-block-b">
          Serving Size : <xsl:value-of select="@TActualServingSize" />
        </div>
        <div class="ui-block-a">
          Water g : <xsl:value-of select="@TWater_g" />
        </div>
        <div class="ui-block-b">
          Energy Kcal : <xsl:value-of select="@TEnerg_Kcal" />
        </div>
        <div class="ui-block-a">
          Protein g : <xsl:value-of select="@TProtein_g" />
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lipid g : <xsl:value-of select="@TLipid_Tot_g" />
        </div>
        <div class="ui-block-b">
          Ash g : <xsl:value-of select="@TAsh_g" />
        </div>
        <div class="ui-block-a">
          Carbohydrate g :  <xsl:value-of select="@TCarbohydrt_g" />
        </div>
        <div class="ui-block-b">
          Fiber (TD) : <xsl:value-of select="@TFiber_TD_g" />
        </div>
        <div class="ui-block-a">
          Sugar (Tot) g : <xsl:value-of select="@TSugar_Tot_g" />
        </div>
        <div class="ui-block-b">
          Calcium mg : <xsl:value-of select="@TCalcium_mg" />
        </div>
        <div class="ui-block-a">
          Iron mg : <xsl:value-of select="@TIron_mg" />
        </div>
        <div class="ui-block-b">
          Magnesium mg : <xsl:value-of select="@TMagnesium_mg" />
        </div>
        <div class="ui-block-a">
          Phosphorus mg : <xsl:value-of select="@TPhosphorus_mg" />
        </div>
        <div class="ui-block-b">
          Potassium mg : <xsl:value-of select="@TPotassium_mg" />
        </div>
        <div class="ui-block-a">
          Sodium mg : <xsl:value-of select="@TSodium_mg" />
        </div>
        <div class="ui-block-b">
          Zinc mg : <xsl:value-of select="@TZinc_mg" />
        </div>
        <div class="ui-block-a">
          Copper mg : <xsl:value-of select="@TCopper_mg" />
        </div>
        <div class="ui-block-b">
          Manganese mg : <xsl:value-of select="@TManganese_mg" />
        </div>
        <div class="ui-block-a">
          Selenium pg : <xsl:value-of select="@TSelenium_pg" />
        </div>
        <div class="ui-block-b">
          Vitamin C mg : <xsl:value-of select="@TVit_C_mg" />
        </div>
        <div class="ui-block-a">
          Thiamin mg : <xsl:value-of select="@TThiamin_mg" />
        </div>
        <div class="ui-block-b">
          Riboflavin mg : <xsl:value-of select="@TRiboflavin_mg" />
        </div>
        <div class="ui-block-a">
          Niacin mg : <xsl:value-of select="@TNiacin_mg" />
        </div>
        <div class="ui-block-b">
          Panto mg : <xsl:value-of select="@TPanto_Acid_mg" />
        </div>
        <div class="ui-block-a">
          Vitamin B6 mg : <xsl:value-of select="@TVit_B6_mg" />
        </div>
        <div class="ui-block-b">
          Folate (Tot) pg : <xsl:value-of select="@TFolate_Tot_pg" />
        </div>
        <div class="ui-block-a">
          Folic Acid pg  : <xsl:value-of select="@TFolic_Acid_pg" />
        </div>
        <div class="ui-block-b">
          Food Folate pg : <xsl:value-of select="@TFood_Folate_pg" />
        </div>
        <div class="ui-block-a">
          Folate (DFE) pg : <xsl:value-of select="@TFolate_DFE_pg" />
        </div>
        <div class="ui-block-b">
          Choline (Tot) mg : <xsl:value-of select="@TCholine_Tot_mg" />
        </div>
        <div class="ui-block-a">
          Vitamin B12 pg : <xsl:value-of select="@TVit_B12_pg" />
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Vitamin A (IU) : <xsl:value-of select="@TVit_A_IU" />
        </div>
        <div class="ui-block-b">
          Vitamin A (RAE) : <xsl:value-of select="@TVit_A_RAE" />
        </div>
        <div class="ui-block-a">
          Retinol pg : <xsl:value-of select="@TRetinol_pg" />
        </div>
        <div class="ui-block-b">
          Alpha Carotene pg : <xsl:value-of select="@TAlpha_Carot_pg" />
        </div>
        <div class="ui-block-a">
          Beta Carotene pg : <xsl:value-of select="@TBeta_Carot_pg" />
        </div>
        <div class="ui-block-b">
          Beta Crypt pg : <xsl:value-of select="@TBeta_Crypt_pg" />
        </div>
        <div class="ui-block-a">
          Lycopene pg : <xsl:value-of select="@TLycopene_pg" />
        </div>
        <div class="ui-block-b">
          Lut Zea pg : <xsl:value-of select="@TLut_Zea_pg" />
        </div>
        <div class="ui-block-a">
          Vitamin E mg : <xsl:value-of select="@TVit_E_mg" />
        </div>
        <div class="ui-block-b">
          Vitamin D pg : <xsl:value-of select="@TVit_D_pg" />
        </div>
        <div class="ui-block-a">
          ViVitamin D (IU) : <xsl:value-of select="@TViVit_D_IU" />
        </div>
        <div class="ui-block-b">
          Vitamin K pg : <xsl:value-of select="@TVit_K_pg" />
        </div>
        <div class="ui-block-a">
          Fatty Acid Sat g : <xsl:value-of select="@TFA_Sat_g" />
        </div>
        <div class="ui-block-b">
          Fatty Acid Mono g : <xsl:value-of select="@TFA_Mono_g" />
        </div>
        <div class="ui-block-a">
          Fatty Acid Poly g : <xsl:value-of select="@TFA_Poly_g" />
        </div>
        <div class="ui-block-b">
          Cholesterol mg : <xsl:value-of select="@TCholestrl_mg" />
        </div>
        <div class="ui-block-a">
          Extra 1 : <xsl:value-of select="@TExtra1" />
        </div>
        <div class="ui-block-b">
          Extra 2 : <xsl:value-of select="@TExtra2" />
        </div>
      </div>
      <div >
        <strong>Description : </strong>
        <xsl:value-of select="@CalculatorDescription" />
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>
