<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
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
					<xsl:apply-templates select="outcomegroup" />
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<tr>
			<th scope="col" colspan="10">
				Outcome Group
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
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
    <tr>
			<th scope="col" colspan="10"><strong>Outcome</strong></th>
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
    <xsl:variable name="count" select="count(outcomeoutput)"/>
    <xsl:if test="($count > 0)">
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
      <xsl:apply-templates select="outcomeoutput">
			  <xsl:sort select="@OutputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'outcomeoutput')">
      <tr>
			  <td colspan="10">
				  Date : <xsl:value-of select="@Date"/> ; Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  Ben
			  </td>
			  <td>
				  <xsl:value-of select="@TAMR"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMRBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Output Q
			  </td>
			  <td>
				  <xsl:value-of select="@TRAmount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmountPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmountBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRAmountBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Output P
			  </td>
			  <td>
				  <xsl:value-of select="@TRPrice"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPriceAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPricePercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPriceBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRPriceBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  RIncent
			  </td>
			  <td>
				  <xsl:value-of select="@TAMRINCENT"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRIncentAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRIncentPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRIncentBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMRIncentBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
      <tr>
        <td>
          Total Benefit:
			  </td>
			  <td>
				  <xsl:value-of select="@TAMR"/>
			  </td>
        <td>
          Output Q:
			  </td>
			  <td>
				  <xsl:value-of select="@TRAmount"/>
			  </td>
        <td>
          Output P:
			  </td>
			  <td>
					  <xsl:value-of select="@TRPrice"/>
			  </td>
        <td>
          Incentive:
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRINCENT"/>
			  </td>
        <td colspan="2">
			  </td>
		  </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
