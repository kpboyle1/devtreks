<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="outcomegroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<tr>
			<th scope="col" colspan="10">
				Outcome Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
      <tr>
			  <th scope="col" colspan="10"><strong>Outcome</strong></th>
		  </tr>
		  <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@Name" /></strong>
			  </td>
		  </tr>
    <tr>
      <td>
        <strong>
          Container Size
        </strong>
      </td>
      <td>
        <strong>
          Serving Cost
        </strong>
      </td>
      <td>
        <strong>
          USDA Servings Per Cont
        </strong>
      </td>
      <td>
        <strong>
          Servings Per Cont
        </strong>
      </td>
      <td>
        <strong>
          Serving Size Unit
        </strong>
      </td>
      <td>
        <strong>
          Serving Size
        </strong>
      </td>
      <td>
        <strong>
          Water g
        </strong>
      </td>
      <td>
        <strong>
          Energy Kcal
        </strong>
      </td>
      <td>
        <strong>
          Lipid g -  Ash g
        </strong>
      </td>
      <td>
        <strong>
          Protein g
        </strong>
      </td>
    </tr>
    <tr>
      <td>
        <strong>
          Carbohydrate g
        </strong>
      </td>
      <td>
        <strong>
          Fiber (TD) g
        </strong>
      </td>
      <td>
        <strong>
          Sugar (Tot) g
        </strong>
      </td>
      <td>
        <strong>
          Calcium mg
        </strong>
      </td>
      <td>
        <strong>
          Iron mg
        </strong>
      </td>
      <td>
        <strong>
          Magnesium mg
        </strong>
      </td>
      <td>
        <strong>
          Phosphorus mg
        </strong>
      </td>
      <td>
        <strong>
          Potassium mg
        </strong>
      </td>
      <td>
        <strong>
          Sodium mg
        </strong>
      </td>
      <td>
        <strong>
          Zinc mg
        </strong>
      </td>
    </tr>
    <tr>
      <td>
        <strong>
          Copper mg
        </strong>
      </td>
      <td>
        <strong>
          Manganese mg
        </strong>
      </td>
      <td>
        <strong>
          Selenium pg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin C mg
        </strong>
      </td>
      <td>
        <strong>
          Thiamin mg
        </strong>
      </td>
      <td>
        <strong>
          Riboflavin mg
        </strong>
      </td>
      <td>
        <strong>
          Niacin mg
        </strong>
      </td>
      <td>
        <strong>
          Panto mg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin B6 mg
        </strong>
      </td>
      <td>
        <strong>
          Folate (Tot) pg
        </strong>
      </td>
    </tr>
    <tr>
      <td>
        <strong>
          Folic Acid pg
        </strong>
      </td>
      <td>
        <strong>
          Food Folate pg
        </strong>
      </td>
      <td>
        <strong>
          Folate (DFE) pg
        </strong>
      </td>
      <td>
        <strong>
          Choline (Tot) mg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin B12 pg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin A (IU)
        </strong>
      </td>
      <td>
        <strong>
          Vitamin A (RAE)
        </strong>
      </td>
      <td>
        <strong>
          Retinol pg
        </strong>
      </td>
      <td>
        <strong>
          Alpha Carotene pg
        </strong>
      </td>
      <td>
        <strong>
          Beta Carotene pg
        </strong>
      </td>
    </tr>
    <tr>
      <td>
        <strong>
          Beta Crypt pg
        </strong>
      </td>
      <td>
        <strong>
          Lycopene pg
        </strong>
      </td>
      <td>
        <strong>
          Lut Zea pg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin E mg
        </strong>
      </td>
      <td>
        <strong>
          Vitamin D pg - IU
        </strong>
      </td>
      <td>
        <strong>
          Vitamin K pg
        </strong>
      </td>
      <td>
        <strong>
          Fatty Acid Sat g
        </strong>
      </td>
      <td>
        <strong>
          Fatty Acid Mono g - Poly g
        </strong>
      </td>
      <td>
        <strong>
          Cholesterol mg
        </strong>
      </td>
      <td>
        <strong>
          Extra 1 - Extra 2
        </strong>
      </td>
    </tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mntotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="root/linkedview">
    <xsl:param name="localName" />
    <tr>
      <td>
        <xsl:value-of select="@TContainerSizeInSSUnits" />
      </td>
      <td>
        <xsl:value-of select="@TServingCost" />
      </td>
      <td>
        <xsl:value-of select="@TTypicalServingsPerContainer" />
      </td>
      <td>
        <xsl:value-of select="@TActualServingsPerContainer" />
      </td>
      <td>
        <xsl:value-of select="@TServingSizeUnit" />
      </td>
      <td>
        <xsl:value-of select="@TActualServingSize" />
      </td>
      <td>
        <xsl:value-of select="@TWater_g" />
      </td>
      <td>
        <xsl:value-of select="@TEnerg_Kcal" />
      </td>
      <td>
        <xsl:value-of select="@TLipid_Tot_g" /> - <xsl:value-of select="@TAsh_g" />
        </td>
        <td>
          <xsl:value-of select="@TProtein_g" />
        </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TCarbohydrt_g" />
      </td>
      <td>
        <xsl:value-of select="@TFiber_TD_g" />
      </td>
      <td>
        <xsl:value-of select="@TSugar_Tot_g" />
      </td>
      <td>
        <xsl:value-of select="@TCalcium_mg" />
      </td>
      <td>
        <xsl:value-of select="@TIron_mg" />
      </td>
      <td>
        <xsl:value-of select="@TMagnesium_mg" />
      </td>
      <td>
        <xsl:value-of select="@TPhosphorus_mg" />
      </td>
      <td>
        <xsl:value-of select="@TPotassium_mg" />
      </td>
      <td>
        <xsl:value-of select="@TSodium_mg" />
      </td>
      <td>
        <xsl:value-of select="@TZinc_mg" />
      </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TCopper_mg" />
      </td>
      <td>
        <xsl:value-of select="@TManganese_mg" />
      </td>
      <td>
        <xsl:value-of select="@TSelenium_pg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_C_mg" />
      </td>
      <td>
        <xsl:value-of select="@TThiamin_mg" />
      </td>
      <td>
        <xsl:value-of select="@TRiboflavin_mg" />
      </td>
      <td>
        <xsl:value-of select="@TNiacin_mg" />
      </td>
      <td>
        <xsl:value-of select="@TPanto_Acid_mg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_B6_mg" />
      </td>
      <td>
        <xsl:value-of select="@TFolate_Tot_pg" />
      </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TFolic_Acid_pg" />
      </td>
      <td>
        <xsl:value-of select="@TFood_Folate_pg" />
      </td>
      <td>
        <xsl:value-of select="@TFolate_DFE_pg" />
      </td>
      <td>
        <xsl:value-of select="@TCholine_Tot_mg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_B12_pg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_A_IU" />
      </td>
      <td>
        <xsl:value-of select="@TVit_A_RAE" />
      </td>
      <td>
        <xsl:value-of select="@TRetinol_pg" />
      </td>
      <td>
        <xsl:value-of select="@TAlpha_Carot_pg" />
      </td>
      <td>
        <xsl:value-of select="@TBeta_Carot_pg" />
      </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TBeta_Crypt_pg" />
      </td>
      <td>
        <xsl:value-of select="@TLycopene_pg" />
      </td>
      <td>
        <xsl:value-of select="@TLut_Zea_pg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_E_mg" />
      </td>
      <td>
        <xsl:value-of select="@TVit_D_pg" />-<xsl:value-of select="@TViVit_D_IU" />
      </td>
      <td>
        <xsl:value-of select="@TVit_K_pg" />
      </td>
      <td>
        <xsl:value-of select="@TFA_Sat_g" />
      </td>
      <td>
        <xsl:value-of select="@TFA_Mono_g" />-<xsl:value-of select="@TFA_Poly_g" />
      </td>
      <td>
        <xsl:value-of select="@TCholestrl_mg" />
      </td>
      <td>
        <xsl:value-of select="@TExtra1" />-<xsl:value-of select="@TExtra2" />
      </td>
    </tr>
    <tr>
      <td colspan="10">
        <strong>Description : </strong>
        <xsl:value-of select="@CalculatorDescription" />
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>