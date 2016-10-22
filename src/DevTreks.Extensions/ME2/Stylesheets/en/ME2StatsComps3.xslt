<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Component"
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
					<xsl:apply-templates select="componentgroup" />
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
		<xsl:apply-templates select="componentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentgroup">
		<tr>
			<th scope="col" colspan="10">
				Component Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="component">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="component">
    <tr>
			<th scope="col" colspan="10"><strong>Component</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(componentinput)"/>
    <xsl:if test="($count > 0)">
      <tr>
			  <th scope="col" colspan="10"><strong>Inputs</strong></th>
		  </tr>
      <xsl:apply-templates select="componentinput">
			  <xsl:sort select="@InputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="componentinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input:&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TotalME2Type1 != '' and @TotalME2Type1 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
          <strong>Monitoring and Evaluation Type: </strong><xsl:value-of select="@TotalME2Type1" />
			  </td>
		  </tr>
    </xsl:if>
    <tr>
      <th>
				Total Type
			</th>
      <th>
				Unit
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
      <th colspan="3">
			</th>
		</tr>
    <xsl:if test="(@TME2Name1 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name1" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N1" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label1" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit1"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev1"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev1"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev1"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description1" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name2 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name2" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N2" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label2" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit2"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev2"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev2"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev2"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description2" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name3 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name3" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N3" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label3" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit3"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev3"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev3"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev3"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description3" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name4 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name4" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N4" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label4" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit4"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev4"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev4"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev4"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description4" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name5 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name5" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N5" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label5" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit5"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev5"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev5"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev5"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description5" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name6 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name6" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N6" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label6" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit6"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev6"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev6"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev6"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description6" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name7 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name7" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N7" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label7" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit7"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev7"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev7"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev7"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description7" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name8 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name8" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N8" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label8" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit8"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev8"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev8"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev8"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description8" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name9 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name9" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N9" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label9" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit9"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev9"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev9"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev9"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description9" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name10 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name10" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N10" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label10" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Total
			  </td>
        <td>
          <xsl:value-of select="@TME2Unit10"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Mean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Median10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Variance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2StandDev10"/>
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
          Q1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Q1Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Mean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Median10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1Variance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1StandDev10"/>
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
          Q2
			  </td>
			  <td>
				    <xsl:value-of select="@TME2Q2Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Mean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Median10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Variance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2StandDev10"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description10" />
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>