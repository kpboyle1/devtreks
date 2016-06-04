<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
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
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
    <tr>
      <th>
				Cost or B Type
			</th>
      <th>
				Total
			</th>
			<th>
				Amount Change
			</th>
			<th>
				Percent Change
			</th>
      <th>
				Base Change
			</th>
			<th>
				Base Percent Change
			</th>
      <th colspan="4">
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
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
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
    <tr>
      <th>
				Cost or B Type
			</th>
      <th>
				Total
			</th>
			<th>
				Amount Change
			</th>
			<th>
				Percent Change
			</th>
      <th>
				Base Change
			</th>
			<th>
				Base Percent Change
			</th>
      <th colspan="4">
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(componentinput)"/>
    <xsl:if test="($count > 0)">
      <xsl:apply-templates select="componentinput">
			  <xsl:sort select="@InputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="componentinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'componentinput')">
      <tr>
			  <td colspan="10">
				  Date : <xsl:value-of select="@Date"/> ; Observations: <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  OC
			  </td>
			  <td>
				  <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOCAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOCPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOCBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMOCBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  AOH
			  </td>
			  <td>
				  <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOHAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOHPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOHBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMAOHBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  CAP
			  </td>
			  <td>
				  <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAPAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAPPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAPBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMCAPBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Total
			  </td>
			  <td>
				  <xsl:value-of select="@TAMTOTAL"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Incent
			  </td>
			  <td>
				  <xsl:value-of select="@TAMINCENT"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMIncentAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMIncentPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMIncentBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMIncentBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName = 'componentinput')">
      <tr>
        <td>
          Totals
			  </td>
			  <td>
				  OC: <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
				  AOH: <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					CAP: <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					Total: <xsl:value-of select="@TAMTOTAL"/>
			  </td>
        <td>
					 
			  </td>
        <td>
					 Incent: <xsl:value-of select="@TAMINCENT"/>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
