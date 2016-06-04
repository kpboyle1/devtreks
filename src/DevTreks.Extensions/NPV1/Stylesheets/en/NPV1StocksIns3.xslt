<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
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
				Input Group : <xsl:value-of select="@Name" />
			</th>
		</tr>
		<tr>
        <td colspan="3">
				  <strong>
					  Name
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TOC Unit
				  </strong>
			  </td>
        <td>
          <strong>
					  TOC Amount
				  </strong>
			  </td>
        <td>
          <strong>
					  TOC Price
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TOC
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TAOH
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TCAP
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TCost
				  </strong>
			  </td>
		  </tr>
			<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			  <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		  </xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
		<tr>
			<th scope="col" colspan="10"><strong>Input: <xsl:value-of select="@Name" /></strong></th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="incount" select="count(inputseries)"/>
    <xsl:if test="($incount > 0)">
		  <tr>
			  <td scope="row" colspan="10">
				  <strong>Input Series</strong>
			  </td>
		  </tr>
      <tr>
        <td colspan="3">
				  <strong>
					  Name
				  </strong>
			  </td>
			  <td>
				  <strong>
					  OC Unit
				  </strong>
			  </td>
        <td>
          <strong>
					  TOC Amount
				  </strong>
			  </td>
        <td>
          <strong>
					  TOC Price
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TOC
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TAOH
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TCAP
				  </strong>
			  </td>
			  <td>
				  <strong>
					  TCost
				  </strong>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputseries">
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
      <tr>
        <td colspan="3">
				  <xsl:value-of select="@TOCName"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TOCUnit"/>
			  </td>
        <td>
					 <xsl:value-of select="@TOCAmount"/>
			  </td>
        <td>
					  <xsl:value-of select="@TOCPrice"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL"/>
			  </td>
		  </tr>
	</xsl:template>
</xsl:stylesheet>
