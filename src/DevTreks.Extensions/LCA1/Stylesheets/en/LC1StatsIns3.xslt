<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, November -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
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
    <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcastat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
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
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcastat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(inputseries)"/>
    <xsl:if test="($count > 0)">
      <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
      <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="inputseries">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input Series: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcastat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'inputgroup')">
		  <tr>
			  <td colspan="10">
				  Cost Observations : <xsl:value-of select="@TCostN"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  OC
			  </td>
			  <td>
				  <xsl:value-of select="@TOCCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TOCStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  AOH
			  </td>
			  <td>
				  <xsl:value-of select="@TAOHCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAOHStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  CAP
			  </td>
			  <td>
				  <xsl:value-of select="@TCAPCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TCAPStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  LCC
			  </td>
			  <td>
				  <xsl:value-of select="@TLCCCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TLCCStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Unit
			  </td>
			  <td>
				  <xsl:value-of select="@TUnitCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TUnitStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  EAA
			  </td>
			  <td>
				  <xsl:value-of select="@TEAACost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TEAAStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName != 'inputgroup')">
		  <tr>
			  <td colspan="10">
				  Cost Observations : <xsl:value-of select="@TCostN"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  OC
			  </td>
			  <td>
				  <xsl:value-of select="@TOCCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TOCVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TOCStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  AOH
			  </td>
			  <td>
				  <xsl:value-of select="@TAOHCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAOHVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAOHStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  CAP
			  </td>
			  <td>
				  <xsl:value-of select="@TCAPCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TCAPStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  LCC
			  </td>
			  <td>
				  <xsl:value-of select="@TLCCCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TLCCStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Unit
			  </td>
			  <td>
				  <xsl:value-of select="@TUnitCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TUnitVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TUnitStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  EAA
			  </td>
			  <td>
				  <xsl:value-of select="@TEAACost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAMean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TEAAVariance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TEAAStandDev"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
			  </td>
      </tr>
		</xsl:if>
    <tr>
      <td>
				<strong>
					SubBOrC Name
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Amount
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Price
				</strong>
			</td>
			<td>
				<strong>
					SubBOrC Total
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit Total
				</strong>
			</td>
      <td>
        <strong>
					SubBOrC Label
				</strong>
      </td>
			<td colspan="3">
			</td>
		</tr>
    <xsl:if test="(@TSubP1Name1 != '' and @TSubP1Name1 != 'none')">
		  <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit1"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label1"/>
			  </td>
		    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description1"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name2 != '' and @TSubP1Name2 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit2"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label2"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description2"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name3 != '' and @TSubP1Name3 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit3"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label3"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description3"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name4 != '' and @TSubP1Name4 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit4"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label4"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description4"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name5 != '' and @TSubP1Name5 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit5"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label5"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description5"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name6 != '' and @TSubP1Name6 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit6"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label6"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description6"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name7 != '' and @TSubP1Name7 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit7"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label7"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description7"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name8 != '' and @TSubP1Name8 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit8"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label8"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description8"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name9 != '' and @TSubP1Name9 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit9"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label9"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description9"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSubP1Name10 != '' and @TSubP1Name10 != 'none')">
      <tr>
        <td>
					  <xsl:value-of select="@TSubP1Name10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSubP1Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Price10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSubP1TotalPerUnit10"/>
			  </td>
			  <td>
            <xsl:value-of select="@TSubP1Label10"/>
			  </td>
			    <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TSubP1Description10"/>
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
