<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Operation"
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
					<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		<tr>
			<th scope="col" colspan="10">
				Operation Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="operation">
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
	<xsl:template match="operation">
    <xsl:variable name="nodeCount" select="count(operationinput/root/linkedview[@CalculatorType='irrpower'])"/>
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
					  <label for="lblCapitalRecoveryCost" >
              Capital Recovery Cost (per acre inch or m3) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblTaxesHousingInsuranceCost" >
              Taxes, H, I Cost (per acre inch or m3) :
            </label>
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
				  <strong>
					  <label for="lblFuelAmount" >
              Fuel Amount (per acre inch or m3):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelPrice" >Fuel Price</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelCost" >
              Fuel Cost (per acre inch or m3):
            </label>
				  </strong>
			  </td>
		  </tr>
		  <tr>
			  <td>
				  <strong>
					  <label for="lblEnergyExtraCost" >
              Extra Energy (standby) Cost (per acre inch or per m3) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborAmount" >
              Labor Amount (per acre or hectare):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborPrice" >Labor Price (per hour):</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLaborCost" >
              Labor Cost (per acre inch or m3):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblIrrigationTimes" >Irrigation Times</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblIrrigationDurationPerSet" >Irrigation Duration Per Set</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblIrrigationDurationLaborHoursPerSet" >Irrigation Duration Labor Hours Per Set</label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblEquipmentLaborAmount" >
              Equipment Labor Amount (per acre or hectare) :
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblPriceMachineryLabor" >
              Equipment Labor Price (per hour):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblEquipmentLaborCost" >
              Equipment Labor Cost (per acre inch or m3):
            </label>
				  </strong>
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>
					  <label for="lblSeasonWaterNeed" >
              Season Water Need ((per acre inch or m3):
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblSeasonWaterExtraCredit" >Season Water Extra Credit</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblSeasonWaterExtraDebit" >Season Water Extra Debit</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblDistributionUniformity" >Distribution Uniformity</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblSeasonWaterApplied" >
              Season Applied Amount (per acre inch pr m3) :
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblWaterPrice" >
              Water Price (per acre inch pr m3):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblWaterCost" >
              Water Cost (per acre inch or m3):
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblLubeOilAmount" >
              Lube Amount (gallons or liters) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblPriceOil" >Lube Oil Price</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblLubeOilCost" >
              Lube Oil Cost (per acre inch or m3) :
            </label>
				  </strong>
			  </td>
		  </tr>
		  <tr>
			  <td>
				  <strong>
					  <label for="lblWaterHP" >
              Water Horsepower (hp or kW) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblBrakeHP" >
              Brake Horsepower (hp or kW) :
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblEngineFlywheelPower" >
              Engine Flywheel Power (hp or kW) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFlowRate" >
              Flow Rate (gpm or l/s)
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblStaticHead" >
              Static Head (feet or meters)
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblPressureHead" >
              Pressure Head (psi or kPa)
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblPumpCapacity" >
              Water pumped (acre inches/hour or m3/hour) :
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lbPumpEfficiency" >Pump Efficiency</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelAmountRequired" >
              Required Fuel Amount (per acre inch or m3):
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblPumpingPlantPerformance" >Pump Performance</label>
				  </strong>
			  </td>
		  </tr>
      <tr>
        <td>
				  <strong>
					  <label for="lblUnits" >
              Unit of Measurement
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblRepairCost" >
              Repair Cost (per acre inch or m3) :
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblPumpHoursPerUnitArea" >
              Pump Hours Needed per Season (per acre or hectare) :
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblFrictionHead" >
              Friction Head (feet or meters)
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblOtherHead" >
              Other Head (feet or meters)
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblExtraPower1" >
              Extra Power 1 (hp or kW)
            </label>
				  </strong>
			  </td>
        <td>
				  <strong>
					  <label for="lblExtraPower2" >
              Extra Power 2 (hp or kW)
            </label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelType" >Fuel Type</label>
				  </strong>
			  </td>
			  <td>
				  <strong>
					  <label for="lblFuelUnit" >Fuel Unit</label>
				  </strong>
			  </td>
			  <td>
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
		  <xsl:apply-templates select="operationinput">
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
	<xsl:template match="operationinput">
    <xsl:variable name="nodeCount" select="count(root/linkedview[@CalculatorType='irrpower'])"/>
    <xsl:if test="($nodeCount != '0')">
		  <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@Name" /></strong>
			  </td>
		  </tr>
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
				  <xsl:value-of select="@TOC"/>
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
				  <xsl:value-of select="@TAOH"/>
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
      <xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		  <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			  <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'operationinput')">
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
        <xsl:value-of select="@TFuelAmount" />
			</td>
			<td>
        <xsl:value-of select="@TFuelPrice" />
			</td>
			<td>
        <xsl:value-of select="@TFuelCost" />
			</td>
		</tr>
		<tr>
			<td>
        <xsl:value-of select="@TEnergyExtraCost" />
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
        <xsl:value-of select="@TIrrigationTimes" />
			</td>
			<td>
        <xsl:value-of select="@TIrrigationDurationPerSet" />
			</td>
			<td>
        <xsl:value-of select="@TIrrigationDurationLaborHoursPerSet" />
			</td>
      <td>
        <xsl:value-of select="@TEquipmentLaborAmount" />
			</td>
			<td>
        <xsl:value-of select="@TPriceMachineryLabor" />
			</td>
      <td>
        <xsl:value-of select="@TEquipmentLaborCost" />
			</td>
		</tr>
    <tr>
			<td>
        <xsl:value-of select="@TSeasonWaterNeed" />
			</td>
      <td>
        <xsl:value-of select="@TSeasonWaterExtraCredit" />
			</td>
			<td>
        <xsl:value-of select="@TSeasonWaterExtraDebit" />
			</td>
			<td>
        <xsl:value-of select="@TDistributionUniformity" />
			</td>
			<td>
        <xsl:value-of select="@TSeasonWaterApplied" />
			</td>
      <td>
        <xsl:value-of select="@TWaterPrice" />
			</td>
			<td>
        <xsl:value-of select="@TWaterCost" />
			</td>
      <td>
        <xsl:value-of select="@TLubeOilAmount" />
			</td>
			<td>
        <xsl:value-of select="@TPriceOil" />
			</td>
			<td>
        <xsl:value-of select="@TLubeOilCost" />
			</td>
		</tr>
		<tr>
			<td>
        <xsl:value-of select="@TWaterHP" />
			</td>
			<td>
        <xsl:value-of select="@TBrakeHP" />
			</td>
      <td>
        <xsl:value-of select="@TEngineFlywheelPower" />
			</td>
			<td>
        <xsl:value-of select="@TFlowRate" />
			</td>
			<td>
        <xsl:value-of select="@TStaticHead" />
			</td>
			<td>
        <xsl:value-of select="@TPressureHead" />
			</td>
			<td>
        <xsl:value-of select="@TPumpCapacity" />
			</td>
			<td>
        <xsl:value-of select="@TPumpEfficiency" />
			</td>
			<td>
        <xsl:value-of select="@TFuelAmountRequired" />
			</td>
			<td>
        <xsl:value-of select="@TPumpingPlantPerformance" />
			</td>
		</tr>
      <tr>
       <td>
        see inputs
			</td>
			<td>
        <xsl:value-of select="@TRepairCost" />
			</td>
			<td>
        <xsl:value-of select="@TPumpHoursPerUnitArea" />
			</td>
      <td>
        <xsl:value-of select="@TFrictionHead" />
			</td>
			<td>
        <xsl:value-of select="@TOtherHead" />
			</td>
			<td>
        <xsl:value-of select="@TExtraPower1" />
			</td>
			<td>
        <xsl:value-of select="@TExtraPower2" />
			</td>
			<td>
        <xsl:value-of select="@FuelType" />
			</td>
			<td>
        <xsl:value-of select="@FuelUnit" />
			</td>
			<td>
			</td>
		</tr>
		</xsl:if>
		<xsl:if test="($localName = 'operationinput')">
      <tr>
			    <td colspan="10"><strong>Irrigation Technology</strong></td>
		  </tr>
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
        <xsl:value-of select="@FuelAmount" />
			</td>
			<td>
        <xsl:value-of select="@FuelPrice" />
			</td>
			<td>
        <xsl:value-of select="@FuelCost" />
			</td>
		</tr>
		<tr>
			<td>
        <xsl:value-of select="@EnergyExtraCost" />
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
        <xsl:value-of select="@IrrigationTimes" />
			</td>
			<td>
        <xsl:value-of select="@IrrigationDurationPerSet" />
			</td>
			<td>
        <xsl:value-of select="@IrrigationDurationLaborHoursPerSet" />
			</td>
      <td>
        <xsl:value-of select="@EquipmentLaborAmount" />
			</td>
      <td>
        <xsl:value-of select="@PriceMachineryLabor" />
			</td>
			<td>
        <xsl:value-of select="@EquipmentLaborCost" />
			</td>
		</tr>
    <tr>
			<td>
        <xsl:value-of select="@SeasonWaterNeed" />
			</td>
      <td>
        <xsl:value-of select="@SeasonWaterExtraCredit" />
			</td>
			<td>
        <xsl:value-of select="@SeasonWaterExtraDebit" />
			</td>
			<td>
        <xsl:value-of select="@DistributionUniformity" />
			</td>
			<td>
        <xsl:value-of select="@SeasonWaterApplied" />
			</td>
      <td>
        <xsl:value-of select="@WaterPrice" />
			</td>
			<td>
        <xsl:value-of select="@WaterCost" />
			</td>
      <td>
        <xsl:value-of select="@LubeOilAmount" />
			</td>
			<td>
        <xsl:value-of select="@PriceOil" />
			</td>
			<td>
        <xsl:value-of select="@LubeOilCost" />
			</td>
		</tr>
		<tr>
			<td>
        <xsl:value-of select="@WaterHP" />
			</td>
			<td>
        <xsl:value-of select="@BrakeHP" />
			</td>
      <td>
        <xsl:value-of select="@EngineFlywheelPower" />
			</td>
			<td>
        <xsl:value-of select="@FlowRate" />
			</td>
			<td>
        <xsl:value-of select="@StaticHead" />
			</td>
			<td>
        <xsl:value-of select="@PressureHead" />
			</td>
			<td>
        <xsl:value-of select="@PumpCapacity" />
			</td>
			<td>
        <xsl:value-of select="@PumpEfficiency" />
			</td>
			<td>
        <xsl:value-of select="@FuelAmountRequired" />
			</td>
			<td>
        <xsl:value-of select="@PumpingPlantPerformance" />
			</td>
		</tr>
      <tr>
        <td>
        <xsl:if test="(@UnitGroupId = '1001')">
					Imperial (USA)
				</xsl:if>
				<xsl:if test="(@UnitGroupId != '1001')">
					Metric
				</xsl:if>
			</td>
			<td>
        <xsl:value-of select="@RepairCost" />
			</td>
			<td>
        <xsl:value-of select="@PumpHoursPerUnitArea" />
			</td>
      <td>
        <xsl:value-of select="@FrictionHead" />
			</td>
			<td>
        <xsl:value-of select="@OtherHead" />
			</td>
			<td>
        <xsl:value-of select="@ExtraPower1" />
			</td>
			<td>
        <xsl:value-of select="@ExtraPower2" />
			</td>
			<td>
        <xsl:value-of select="@FuelType" />
			</td>
			<td>
        <xsl:value-of select="@FuelUnit" />
			</td>
			<td>
			</td>
		</tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>