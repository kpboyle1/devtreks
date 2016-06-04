<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, October -->
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
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					Market Value
				</strong>
			</td>
			<td>
				<strong>
					Salvage Value
				</strong>
			</td>
			<td>
				<strong>
					Capital Recovery Cost
				</strong>
			</td>
			<td>
				<strong>
					THI Cost
				</strong>
			</td>
			<td>
				<strong>
					Starting Hrs
				</strong>
			</td>
			<td>
				<strong>
					Planned Use Hrs
				</strong>
			</td>
			<td>
				<strong>
					Useful Life Hrs
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
					Fuel Amount
				</strong>
			</td>
			<td>
				<strong>
					Fuel Price
				</strong>
			</td>
			<td>
				<strong>
					Fuel Cost
				</strong>
			</td>
			<td>
				<strong>
					Labor Amount
				</strong>
			</td>
			<td>
				<strong>
					Labor Price
				</strong>
			</td>
			<td>
				<strong>
					Labor Cost
				</strong>
			</td>
			<td>
				<strong>
					Energy Use Hr
				</strong>
			</td>
			<td>
				<strong>
					Energy Efficiency
				</strong>
			</td>
			<td>
				<strong>
					R and M Percent
				</strong>
			</td>
			<td>
				<strong>
					Repair Cost
				</strong>
			</td>
		</tr>
		<tr>
			<th scope="col" colspan="10">Inputs (cost = cost * input.amount)</th>
		</tr>
    <tr>
			<td>
				<strong>
					Date
				</strong>
			</td>
			<td>
				<strong>
					OC Amount
				</strong>
			</td>
			<td>
				<strong>
					OC Units
				</strong>
			</td>
			<td>
				<strong>
					OC Price
				</strong>
			</td>
			<td>
				<strong>
					OC Cost
				</strong>
			</td>
			<td>
				<strong>
					AOH Unit
				</strong>
			</td>
			<td>
				<strong>
					AOH Price
				</strong>
			</td>
      <td>
				<strong>
				  CAP Unit
				</strong>
			</td>
			<td>
				<strong>
					CAP Price
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
    </xsl:if>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'inputgroup')">
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
		<xsl:if test="($localName != 'inputgroup')">
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
