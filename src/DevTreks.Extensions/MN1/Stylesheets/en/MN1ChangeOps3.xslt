<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
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
    <tr>
      <th>
				Name
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
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
    <tr>
      <th>
				Name
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
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(operationinput)"/>
    <xsl:if test="($count > 0)">
      <xsl:apply-templates select="operationinput">
			  <xsl:sort select="@InputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="operationinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<tr>
			<td>
				<xsl:value-of select="@TMN1Name"/>
			</td>
			<td>
				<xsl:value-of select="@TMN1Q"/>
			</td>
			<td>
					<xsl:value-of select="@TMN1AmountChange"/>
			</td>
			<td>
					<xsl:value-of select="@TMN1PercentChange"/>
			</td>
			<td>
					<xsl:value-of select="@TMN1BaseChange"/>
			</td>
      <td>
					<xsl:value-of select="@TMN1BasePercentChange"/>
			</td>
      <td colspan="4">
			</td>
		</tr>
    <tr>
			<td>
				<xsl:value-of select="@TMN2Name"/>
			</td>
			<td>
				<xsl:value-of select="@TMN2Q"/>
			</td>
			<td>
					<xsl:value-of select="@TMN2AmountChange"/>
			</td>
			<td>
					<xsl:value-of select="@TMN2PercentChange"/>
			</td>
			<td>
					<xsl:value-of select="@TMN2BaseChange"/>
			</td>
      <td>
					<xsl:value-of select="@TMN2BasePercentChange"/>
			</td>
      <td colspan="4">
			</td>
		</tr>
    <xsl:if test="(string-length(@TMN3Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN3Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN3Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN3AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN3PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN3BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN3BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN4Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN4Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN4Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN4AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN4PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN4BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN4BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN5Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN5Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN5Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN5AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN5PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN5BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN5BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN6Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN6Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN6Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN6AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN6PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN6BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN6BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN7Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN7Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN7Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN7AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN7PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN7BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN7BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN8Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN8Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN8Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN8AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN8PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN8BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN8BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN9Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN9Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN9Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN9AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN9PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN9BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN9BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN10Name) > 0)">
      <tr>
			  <td>
				  <xsl:value-of select="@TMN10Name"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TMN10Q"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN10AmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN10PercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TMN10BaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TMN10BasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
