<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, January -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
		<tr>
			<th scope="col" colspan="10">
				Budget Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10"><strong>Time Period</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@EnterpriseName" /></strong>
			</td>
		</tr>
		<!--<tr>
			<th scope="col" colspan="10">Outcomes</th>
		</tr>
    <xsl:apply-templates select="budgetoutcomes" />-->
		<tr>
			<th scope="col" colspan="10">Operations</th>
		</tr>
		<tr>
			<td>
				<strong>
					Container Size
				</strong>
			</td>
			<td>
				<strong>
					Container Cost
				</strong>
			</td>
			<td>
				<strong>
					USDA Servings Per Container
				</strong>
			</td>
			<td>
				<strong>
					Actual Servings Per Container
				</strong>
			</td>
			<td>
				<strong>
					Serving Size Unit
				</strong>
			</td>
			<td>
				<strong>
					Actual Serving Size
				</strong>
			</td>
      <td>
				<strong>
					Actual Water
				</strong>
			</td>
			<td>
				<strong>
					Actual Energy (calories)
				</strong>
			</td>
			<td>
				<strong>
					Actual Lipid
				</strong>
			</td>
      <td>
				<strong>
					Actual Ash
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					Actual Carbohydrate
				</strong>
			</td>
			<td>
				<strong>
					Actual Fiber (TD)
				</strong>
			</td>
			<td>
				<strong>
					Actual Sugar (Tot)
				</strong>
			</td>
			<td>
				<strong>
					Actual Calcium
				</strong>
			</td>
			<td>
				<strong>
					Actual Iron
				</strong>
			</td>
			<td>
				<strong>
					Actual Magnesium
				</strong>
			</td>
			<td>
				<strong>
					Actual Phosphorus
				</strong>
			</td>
			<td>
				<strong>
					Actual Potassium
				</strong>
			</td>
			<td>
				<strong>
					Actual Sodium
				</strong>
			</td>
			<td>
				<strong>
					Actual Zinc
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					Actual Copper
				</strong>
			</td>
			<td>
				<strong>
					Actual Manganese
				</strong>
			</td>
			<td>
				<strong>
					Actual Selenium
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin C
				</strong>
			</td>
			<td>
				<strong>
					Actual Thiamin
				</strong>
			</td>
			<td>
				<strong>
					Actual Riboflavin
				</strong>
			</td>
			<td>
				<strong>
					Actual Niacin
				</strong>
			</td>
			<td>
				<strong>
					Actual Panto
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin B6
				</strong>
			</td>
			<td>
				<strong>
					Actual Folate (Tot)
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					Actual Folic Acid
				</strong>
			</td>
			<td>
				<strong>
					Actual Food Folate
				</strong>
			</td>
			<td>
				<strong>
					Actual Folate (DFE)
				</strong>
			</td>
			<td>
				<strong>
					Actual Choline (Tot)
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin B12
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin A (IU)
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin A (RAE)
				</strong>
			</td>
			<td>
				<strong>
					Actual Retinol
				</strong>
			</td>
			<td>
				<strong>
					Actual Alpha Carotene
				</strong>
			</td>
			<td>
				<strong>
					Actual Beta Carotene
				</strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					Actual Beta Crypt
				</strong>
			</td>
			<td>
				<strong>
					Actual Lycopene
				</strong>
			</td>
			<td>
				<strong>
					Actual Lut Zea
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin E
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin D
				</strong>
			</td>
			<td>
				<strong>
					Actual ViVitamin D (IU)
				</strong>
			</td>
			<td>
				<strong>
					Actual Vitamin K
				</strong>
			</td>
			<td>
				<strong>
					Actual Fatty Acid Sat
				</strong>
			</td>
			<td>
				<strong>
					Actual Fatty Acid Mono
				</strong>
			</td>
			<td>
				<strong>
					Actual Fatty Acid Poly
				</strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					Actual Cholesterol
				</strong>
			</td>
			<td colspan="9">
				
			</td>
     </tr>
		<xsl:apply-templates select="budgetoperations" />
		<tr>
			<th scope="col" colspan="10"><strong>Time Period Totals</strong></th>
		</tr>
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
		<tr>
			<th scope="col" colspan="10"><strong>Outcome</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
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
		<tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <tr>
		        <td scope="col" colspan="10"><strong>Benefits</strong></td>
	      </tr>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <tr>
		        <td scope="col" colspan="10"><strong>Costs</strong></td>
	      </tr>
			 <tr>
				<td>
					<xsl:value-of select="@TContainerSizeUsingServingSizeUnit" />
				</td>
				<td>
					<xsl:value-of select="@TMarketValue" />
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
					<xsl:value-of select="@TLipid_Tot_g" />
				</td>
        <td>
          <xsl:value-of select="@TAsh_g" />
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
					  <xsl:value-of select="@TVit_D_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TViVit_D_IU" />
			  </td>
			  <td>
					  <xsl:value-of select="@TVit_K_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TFA_Sat_g" />
			  </td>
			  <td>
					  <xsl:value-of select="@TFA_Mono_g" />
			  </td>
			  <td>
					  <xsl:value-of select="@TFA_Poly_g" />
			  </td>
		  </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TCholestrl_mg" />
			  </td>
			  <td colspan="9">
				
			  </td>
     </tr>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <tr>
		    <td scope="col" colspan="10"><strong>Costs</strong></td>
	    </tr>
			 <tr>
				<td>
					<xsl:value-of select="@ContainerSizeUsingServingSizeUnit" />
				</td>
				<td>
					<xsl:value-of select="@MarketValue" />
				</td>
				<td>
					<xsl:value-of select="@TypicalServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@ActualServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@ServingSizeUnit" />
				</td>
				<td>
					<xsl:value-of select="@ActualServingSize" />
				</td>
        <td>
					<xsl:value-of select="@ActualWater_g" />
				</td>
				<td>
					<xsl:value-of select="@ActualEnerg_Kcal" />
				</td>
				<td>
					<xsl:value-of select="@ActualLipid_Tot_g" />
				</td>
        <td>
          <xsl:value-of select="@ActualAsh_g" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@ActualCarbohydrt_g" />
				</td>
				<td>
					<xsl:value-of select="@ActualFiber_TD_g" />
				</td>
				<td>
					<xsl:value-of select="@ActualSugar_Tot_g" />
				</td>
				<td>
					<xsl:value-of select="@ActualCalcium_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualIron_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualMagnesium_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualPhosphorus_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualPotassium_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualSodium_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualZinc_mg" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@ActualCopper_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualManganese_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualSelenium_pg" />
				</td>
				<td>
					<xsl:value-of select="@ActualVit_C_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualThiamin_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualRiboflavin_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualNiacin_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualPanto_Acid_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualVit_B6_mg" />
				</td>
				<td>
					<xsl:value-of select="@ActualFolate_Tot_pg" />
				</td>
			</tr>
      <tr>
			<td>
					<xsl:value-of select="@ActualFolic_Acid_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualFood_Folate_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualFolate_DFE_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualCholine_Tot_mg" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_B12_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_A_IU" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_A_RAE" />
			</td>
			<td>
					<xsl:value-of select="@ActualRetinol_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualAlpha_Carot_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualBeta_Carot_pg" />
			</td>
		</tr>
    <tr>
			<td>
					<xsl:value-of select="@ActualBeta_Crypt_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualLycopene_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualLut_Zea_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_E_mg" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_D_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualViVit_D_IU" />
			</td>
			<td>
					<xsl:value-of select="@ActualVit_K_pg" />
			</td>
			<td>
					<xsl:value-of select="@ActualFA_Sat_g" />
			</td>
			<td>
					<xsl:value-of select="@ActualFA_Mono_g" />
			</td>
			<td>
					<xsl:value-of select="@ActualFA_Poly_g" />
			</td>
		</tr>
    <tr>
			<td>
					<xsl:value-of select="@ActualCholestrl_mg" />
			</td>
			<td colspan="9">
				
			</td>
     </tr>
      <tr>
        <td colspan="10">
					<strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
				</td>
			</tr>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
			
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
