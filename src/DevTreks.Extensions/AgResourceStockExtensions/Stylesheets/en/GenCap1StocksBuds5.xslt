<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, October -->
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
		<!--this release does not include resource stock output calculators-->
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
    <xsl:variable name="nodeCount" select="count(budgetinput/root/linkedview[@CalculatorType='gencapital'])"/>
    <xsl:if test="($nodeCount != '0')">
		  <tr>
			  <th scope="col" colspan="10"><strong>Operation</strong></th>
		  </tr>
		  <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@Name" /></strong>
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>
					  <label for="lblMarketValue" >Market Value</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblSalvageValue" >Salvage Value</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblCapitalRecoveryCost" >Capital Recovery Cost</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblTaxesHousingInsuranceCost" >THI Cost</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblStartingHrs" >Starting Hrs</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblPlannedUseHrs" >Planned Use Hrs</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblUsefulLifeHrs" >Useful Life Hrs</label>
				  </strong>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
		  </tr>
		  <tr>
			  <td>
				  <strong>
					  <label for="lblFuelAmount" >Fuel Amount</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelPrice" >Fuel Price</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelCost" >Fuel Cost</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborAmount" >Labor Amount</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborPrice" >Labor Price</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborCost" >Labor Cost</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblEnergyUseHr" >Energy Use Hr</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblEnergyEffTypical" >Energy Efficiency</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblRandMPercent" >R and M Percent</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblRepairCost" >Repair Cost</label>
				  </strong>
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($nodeCount != '0') and ($viewEditType != 'summary')">
			<tr>
				<th scope="col" colspan="10">Inputs (cost = cost * operation.amount * input.times * input.amount)</th>
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
					  <label for="lblAOHAmount" >AOH Amount</label>
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
					  <label for="lblTAOH" >AOH Cost</label>
				  </strong>
			  </td>
		  </tr>
      <tr>
        <td>
				  <strong>
					  <label for="lblCAPAmount" >CAP Amount</label>
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
			  <td>
				  <strong>
					  <label for="lblTCAP" >CAP Cost</label>
				  </strong>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
		</xsl:if>
    <xsl:if test="($nodeCount != '0')">
		  <xsl:apply-templates select="budgetinput">
			  <xsl:sort select="@InputDate"/>
		  </xsl:apply-templates>
		  <tr>
			  <td colspan="10"><strong>Operation Totals&#xA0;&#xA0;(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong></td>
		  </tr>
		  <xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		  <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			  <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="budgetinput">
    <xsl:variable name="nodeCount" select="count(root/linkedview[@CalculatorType='gencapital'])"/>
    <xsl:if test="($nodeCount != '0')">
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
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('AOHAmount', @InputPrice2Amount, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit2" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice2" />
			  </td>
			  <td>
				   <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('TAOH', @InputPrice2Amount * @InputPrice2, 'double', '8')"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('CAPAmount', @InputPrice3Amount, 'double', '8')"/>
			  </td>
			  <td>
				  <xsl:value-of select="@InputUnit3" />
			  </td>
			  <td>
				  <xsl:value-of select="@InputPrice3" />
			  </td>
			  <td>
				  <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('TCAP', @InputPrice3Amount * @InputPrice3, 'double', '8')"/>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
        <td>
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput')">
			<tr>
				<td>
					<xsl:value-of select="@TMarketValue" />
				</td>
				<td>
					<xsl:value-of select="@TSalvageValue" />
				</td>
				<td>
					<xsl:value-of select="@TCapitalRecoveryCost" />
				</td>
				<td>
					<xsl:value-of select="@TTaxesHousingInsuranceCost" />
				</td>
				<td>
					<xsl:value-of select="@TStartingHrs" />
				</td>
				<td>
					<xsl:value-of select="@TPlannedUseHrs" />
				</td>
				<td>
					<xsl:value-of select="@TUsefulLifeHrs" />
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@TFuelAmount" />
				</td>
				<td>
					<xsl:value-of select="@TFuelPrice" />
				</td>
				<td>
					<xsl:value-of select="@TFuelCost" />
				</td>
				<td>
					<xsl:value-of select="@TLaborAmount" />
				</td>
				<td>
					<xsl:value-of select="@TLaborPrice" />
				</td>
				<td>
					<xsl:value-of select="@TLaborCost" />
				</td>
				<td>
					<xsl:value-of select="@TEnergyUseHr" />
				</td>
				<td>
					<xsl:value-of select="@TEnergyEffTypical" />
				</td>
				<td>
					<xsl:value-of select="@TRandMPercent" />
				</td>
				<td>
					<xsl:value-of select="@TRepairCost" />
				</td>
			</tr>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
			<tr>
				<td>
					<xsl:value-of select="@MarketValue" />
				</td>
				<td>
					<xsl:value-of select="@SalvageValue" />
				</td>
				<td>
					<xsl:value-of select="@CapitalRecoveryCost" />
				</td>
				<td>
					<xsl:value-of select="@TaxesHousingInsuranceCost" />
				</td>
				<td>
					<xsl:value-of select="@StartingHrs" />
				</td>
				<td>
					<xsl:value-of select="@PlannedUseHrs" />
				</td>
				<td>
					<xsl:value-of select="@UsefulLifeHrs" />
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:value-of select="@FuelAmount" />
				</td>
				<td>
					<xsl:value-of select="@FuelPrice" />
				</td>
				<td>
					<xsl:value-of select="@FuelCost" />
				</td>
				<td>
					<xsl:value-of select="@LaborAmount" />
				</td>
				<td>
					<xsl:value-of select="@LaborPrice" />
				</td>
				<td>
					<xsl:value-of select="@LaborCost" />
				</td>
				<td>
					<xsl:value-of select="@EnergyUseHr" />
				</td>
				<td>
					<xsl:value-of select="@EnergyEffTypical" />
				</td>
				<td>
					<xsl:value-of select="@RandMPercent" />
				</td>
				<td>
					<xsl:value-of select="@RepairCost" />
				</td>
			</tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
