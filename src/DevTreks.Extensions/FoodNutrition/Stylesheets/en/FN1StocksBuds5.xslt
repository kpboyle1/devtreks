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
					<label for="lblContainerSize" >Container Size</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblServingCost" >Serving Cost</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblServingsPerContainer" >Servings Per Container</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblActualServingsPerContainer" >Actual Servings Per Container</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblGenderOfServingPerson" >Gender Of Serving Person</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lbWeightOfServingPerson" >Weight Of Serving Person</label>
				</strong>
			</td>
      <td>
				<strong>
					<label for="lblActualCaloriesPerDay" >Actual Calories Per Day</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblCaloriesPerActualServing" >Calories Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblCaloriesFromFatPerActualServing" >Calories From Fat Per Actual Serving</label>
				</strong>
			</td>
      <td>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblTotalFatPerActualServing" >Total Fat Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblTotalFatActualDailyPercent" >Total Fat Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblSaturatedFatPerActualServing" >Saturated Fat Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblSaturatedFatActualDailyPercent" >Saturated Fat Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblTransFatPerActualServing" >Trans Fat Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblCholesterolPerActualServing" >Cholesterol Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblCholesterolActualDailyPercent" >Cholesterol Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblSodiumPerActualServing" >Sodium Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblSodiumActualDailyPercent" >Sodium Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblPotassiumPerActualServing" >Potassium Per Actual Serving</label>
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<strong>
					<label for="lblTotalCarbohydratePerActualServing" >Total Carbohydrate Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblTotalCarbohydrateActualDailyPercent" >Total Carbohydrate Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblOtherCarbohydratePerActualServing" >Other Carbohydrate Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblOtherCarbohydrateActualDailyPercent" >Other Carbohydrate Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblDietaryFiberPerActualServing" >Dietary Fiber Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblDietaryFiberActualDailyPercent" >Dietary Fiber Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblSugarsPerActualServing" >Sugars Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblProteinPerActualServing" >Protein Per Actual Serving</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblProteinActualDailyPercent" >Protein Actual Daily Percent</label>
				</strong>
			</td>
			<td>
				<strong>
					<label for="lblVitaminAPercentActualDailyValue" >Vitamin A Percent Actual Daily Value</label>
				</strong>
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
					<xsl:value-of select="@TContainerSize" />
				</td>
				<td>
					<xsl:value-of select="@TServingCost" />
				</td>
				<td>
					<xsl:value-of select="@TServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@TActualServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@TGenderOfServingPerson" />
				</td>
				<td>
					<xsl:value-of select="@TWeightOfServingPerson" />
				</td>
        <td>
					<xsl:value-of select="@TActualCaloriesPerDay" />
				</td>
				<td>
					<xsl:value-of select="@TCaloriesPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TCaloriesFromFatPerActualServing" />
				</td>
        <td>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TTotalFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TTotalFatActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TSaturatedFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TSaturatedFatActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TTransFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TCholesterolPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TCholesterolActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TSodiumPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TSodiumActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TPotassiumPerActualServing" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TTotalCarbohydratePerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TTotalCarbohydrateActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TOtherCarbohydratePerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TOtherCarbohydrateActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TDietaryFiberPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TDietaryFiberActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TSugarsPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TProteinPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TProteinActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TVitaminAPercentActualDailyValue" />
				</td>
			</tr>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
			<tr>
				<td>
					<xsl:value-of select="@ContainerSize" />
				</td>
				<td>
					<xsl:value-of select="@ServingCost" />
				</td>
				<td>
					<xsl:value-of select="@ServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@ActualServingsPerContainer" />
				</td>
				<td>
					<xsl:value-of select="@GenderOfServingPerson" />
				</td>
				<td>
					<xsl:value-of select="@WeightOfServingPerson" />
				</td>
        <td>
					<xsl:value-of select="@ActualCaloriesPerDay" />
				</td>
				<td>
					<xsl:value-of select="@CaloriesPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@CaloriesFromFatPerActualServing" />
				</td>
        <td>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TotalFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TotalFatActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@SaturatedFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@SaturatedFatActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@TransFatPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@CholesterolPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@CholesterolActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@SodiumPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@SodiumActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@PotassiumPerActualServing" />
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TotalCarbohydratePerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@TotalCarbohydrateActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@OtherCarbohydratePerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@OtherCarbohydrateActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@DietaryFiberPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@DietaryFiberActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@SugarsPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@ProteinPerActualServing" />
				</td>
				<td>
					<xsl:value-of select="@ProteinActualDailyPercent" />
				</td>
				<td>
					<xsl:value-of select="@VitaminAPercentActualDailyValue" />
				</td>
			</tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
