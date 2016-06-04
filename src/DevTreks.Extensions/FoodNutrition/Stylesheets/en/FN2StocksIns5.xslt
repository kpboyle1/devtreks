<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Input"
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
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="y0:servicebase" />
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="inputgroup" />
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
	<xsl:template match="y0:servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputgroup">
		<tr>
			<th scope="col" colspan="10">
				Input Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Group Totals</strong></td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
		<tr>
			<th scope="col" colspan="10"><strong>Input</strong></th>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblContainerSizeUsingServingSizeUnit" >Container Size</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblMarketValue" >Container Cost</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblTypicalServingsPerContainer" >USDA Servings Per Container</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualServingsPerContainer" >Actual Servings Per Container</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblServingSizeUnit" >Serving Size Unit</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lbActualServingSize" >Actual Serving Size</label>
				</strong>
			</td>
      <td>
				<strong>
					<label for="lblActualWater_g" >Actual Water</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualEnerg_Kcal" >Actual Energy (calories)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualLipid_Tot_g" >Actual Lipid</label>
				</strong>
			</td>
      <td>
				<strong>
					<label for="lblActualAsh_g" >Actual Ash</label>
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblActualCarbohydrt_g" >Actual Carbohydrate</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFiber_TD_g" >Actual Fiber (TD)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualSugar_Tot_g" >Actual Sugar (Tot)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualCalcium_mg" >Actual Calcium</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualIron_mg" >Actual Iron</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualMagnesium_mg" >Actual Magnesium</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualPhosphorus_mg" >Actual Phosphorus</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualPotassium_mg" >Actual Potassium</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualSodium_mg" >Actual Sodium</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualZinc_mg" >Actual Zinc</label>
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblActualCopper_mg" >Actual Copper</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualManganese_mg" >Actual Manganese</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualSelenium_pg" >Actual Selenium</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_C_mg" >Actual Vitamin C</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualThiamin_mg" >Actual Thiamin</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualRiboflavin_mg" >Actual Riboflavin</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualNiacin_mg" >Actual Niacin</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualPanto_Acid_mg" >Actual Panto</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_B6_mg" >Actual Vitamin B6</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFolate_Tot_pg" >Actual Folate (Tot)</label>
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblActualFolic_Acid_pg" >Actual Folic Acid</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFood_Folate_pg" >Actual Food Folate</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFolate_DFE_pg" >Actual Folate (DFE)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualCholine_Tot_mg" >Actual Choline (Tot)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_B12_pg" >Actual Vitamin B12</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_A_IU" >Actual Vitamin A (IU)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_A_RAE" >Actual Vitamin A (RAE)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualRetinol_pg" >Actual Retinol</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualAlpha_Carot_pg" >Actual Alpha Carotene</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualBeta_Carot_pg" >Actual Beta Carotene</label>
				</strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					<label for="lblActualBeta_Crypt_pg" >Actual Beta Crypt</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualLycopene_pg" >Actual Lycopene</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualLut_Zea_pg" >Actual Lut Zea</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_E_mg" >Actual Vitamin E</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_D_pg" >Actual Vitamin D</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualViVit_D_IU" >Actual ViVitamin D (IU)</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualVit_K_pg" >Actual Vitamin K</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFA_Sat_g" >Actual Fatty Acid Sat</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFA_Mono_g" >Actual Fatty Acid Mono</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualFA_Poly_g" >Actual Fatty Acid Poly</label>
				</strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					<label for="lblActualCholestrl_mg" >Actual Cholesterol</label>
				</strong>
			</td>
			<td colspan="9">
				
			</td>
     </tr>
		<tr>
			<th scope="col" colspan="10">Inputs (cost = cost * input.amount)</th>
		</tr>
    <tr>
			<td>
				<strong>
					<label for="lblDate" >Date</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblOCAmount" >OC Amount</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblOCUnit" >OC Units</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblOCPrice" >OC Price</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblTOC" >OC Cost</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblAOHUnit" >AOH Units</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblAOHPrice" >AOH Price</label>
				</strong>
			</td>
      <td>
				<strong>
					<label for="lblCAPUnit" >CAP Units</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblCAPPrice" >CAP Price</label>
				</strong>
			</td>
      <td></td>
		</tr>
		<xsl:variable name="nodeCount" select="count(inputseries)"/>
		<xsl:if test="($nodeCount != '0')">
			<tr>
				<th scope="col" colspan="10">Input Series</th>
			</tr>
		</xsl:if>
		<xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Input</strong></td>
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
		<tr>
			  <td>
				  <xsl:value-of select="@InputDate" />
			  </td>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OCAmount', @InputPrice1Amount, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit1" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice1" />
			  </td>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OCAmount', @InputPrice1Amount * @InputPrice1, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit2" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice2" />
			  </td>
			   <td>
				  <xsl:value-of select="@InputUnit3" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice3" />
			  </td>
        <td></td>
		  </tr>
	</xsl:template>
	<xsl:template match="inputseries">
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<tr>
			  <td>
				  <xsl:value-of select="@InputDate" />
			  </td>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OCAmount', @InputPrice1Amount, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit1" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice1" />
			  </td>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OCAmount', @InputPrice1Amount * @InputPrice1, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit2" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice2" />
			  </td>
			   <td>
				  <xsl:value-of select="@InputUnit3" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice3" />
			  </td>
        <td></td>
		  </tr>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:choose>
		<xsl:when test="($localName = 'inputgroup') and ($nodeName != 'linkedview')">
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
					<xsl:value-of select="@TActualWater_g" />
				</td>
				<td>
					<xsl:value-of select="@TActualEnerg_Kcal" />
				</td>
				<td>
					<xsl:value-of select="@TActualLipid_Tot_g" />
				</td>
        <td>
          <xsl:value-of select="@TActualAsh_g" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TActualCarbohydrt_g" />
				</td>
				<td>
					<xsl:value-of select="@TActualFiber_TD_g" />
				</td>
				<td>
					<xsl:value-of select="@TActualSugar_Tot_g" />
				</td>
				<td>
					<xsl:value-of select="@TActualCalcium_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualIron_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualMagnesium_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualPhosphorus_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualPotassium_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualSodium_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualZinc_mg" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TActualCopper_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualManganese_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualSelenium_pg" />
				</td>
				<td>
					<xsl:value-of select="@TActualVit_C_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualThiamin_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualRiboflavin_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualNiacin_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualPanto_Acid_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualVit_B6_mg" />
				</td>
				<td>
					<xsl:value-of select="@TActualFolate_Tot_pg" />
				</td>
			</tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TActualFolic_Acid_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualFood_Folate_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualFolate_DFE_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualCholine_Tot_mg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_B12_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_A_IU" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_A_RAE" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualRetinol_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualAlpha_Carot_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualBeta_Carot_pg" />
			  </td>
		  </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TActualBeta_Crypt_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualLycopene_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualLut_Zea_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_E_mg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_D_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualViVit_D_IU" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualVit_K_pg" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualFA_Sat_g" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualFA_Mono_g" />
			  </td>
			  <td>
					  <xsl:value-of select="@TActualFA_Poly_g" />
			  </td>
		  </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TActualCholestrl_mg" />
			  </td>
			  <td colspan="9">
				
			  </td>
     </tr>
		</xsl:when>
		<xsl:otherwise>
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
		</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>