﻿<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
	xmlns:DisplayComps="urn:displaycomps">
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
					<xsl:apply-templates select="outputgroup" />
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
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
      <xsl:variable name="title">Output Group : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" /></xsl:variable>
      <xsl:value-of select="DisplayComps:writeStringFullColumnTH($title, $fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Output
			</th>
			<xsl:value-of select="DisplayComps:writeTitles($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Name</strong></td>
			<td>
				<!--Total-->
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Label</strong></td>
			<td>
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="outputseries">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Output Series
			</th>
			<xsl:value-of select="DisplayComps:writeTitles($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Name</strong></td>
			<td>
				<!--Total-->
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Label</strong></td>
			<td>
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
  </xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
      <tr>
			  <td scope="row" colspan="1"><strong>Observations</strong></td>
			  <td>
				  <xsl:value-of select="@Observations"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('Observations_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Alternative</strong></td>
			  <td>
            <xsl:value-of select="@AlternativeType"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('AlternativeType_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Total </strong></td>
			  <td>
				  <xsl:value-of select="@TAMR"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAMR_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAMRAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAMRAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAMRPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAMRPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAMRBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAMRBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAMRBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAMRBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Output Q Total </strong></td>
			  <td>
				  <xsl:value-of select="@TRAmount"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRAmount_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Output Q AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Output Q PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRAmountPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRAmountPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Output Q BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRAmountBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRAmountBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Output Q BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRAmountBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRAmountBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>R Price Total </strong></td>
			  <td>
				  <xsl:value-of select="@TRPrice"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRPrice_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>R Price AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRPriceAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRPriceAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>R Price PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRPricePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRPricePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>R Price BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRPriceBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRPriceBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>R Price BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TRPriceBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRPriceBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
	</xsl:template>
</xsl:stylesheet>
