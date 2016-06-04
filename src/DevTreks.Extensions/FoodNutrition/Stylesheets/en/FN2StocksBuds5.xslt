<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, July -->
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
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="y0:servicebase" />
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
	<xsl:template match="y0:servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
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
		<xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Group Totals</strong></td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
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
		<xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Budget Totals&#xA0;&#xA0;(Date:&#xA0;<xsl:value-of select="@Date" /></strong></td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
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
		<xsl:variable name="outputCount" select="count(budgetoutcomes/budgetoutcome)"/>
		<xsl:if test="($outputCount != '0')">
			<tr>
				<th scope="col" colspan="10">Outcomes</th>
			</tr>
			<xsl:apply-templates select="budgetoutcomes" />
		</xsl:if>
		<tr>
			<th scope="col" colspan="10">Operations</th>
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
			<td>
				<strong>
					<label for="lblTimes" >Times</label>
				</strong>
			</td>
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
					<label for="lblCAPPrice" >CAP Price (Container Cost)</label>
				</strong>
			</td>
			<td>
				
			</td>
			<td>
				
			</td>
			<td>
				
			</td>
		</tr>
		<xsl:apply-templates select="budgetoperations" />
		<tr>
			<th scope="col" colspan="10">Time Period Totals&#xA0;&#xA0;(Amount:&#xA0;<xsl:value-of select="@EnterpriseAmount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></th>
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
    <tr>
			<th scope="col" colspan="10"><strong>Outputs</strong></th>
		</tr>
		<xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Outcome Totals&#xA0;&#xA0;(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong></td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
		<!--this release does not include resource stock budgetoutput calculators-->
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
		<xsl:apply-templates select="budgetinput"></xsl:apply-templates>
		<tr>
			<td colspan="10"><strong>Operation Totals&#xA0;&#xA0;(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong></td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
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
				<xsl:value-of select="@InputTimes" />
			</td>
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
				<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('TOC', @InputPrice1Amount * @InputPrice1, 'double', '8')"/>
			</td>
			<td>
				<xsl:value-of select="@InputPrice3" />
			</td>
			<td>
				
			</td>
			<td>
				
			</td>
			<td>
				
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput')">
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
		<xsl:if test="($localName = 'budgetinput')">
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
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
