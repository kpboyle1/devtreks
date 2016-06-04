<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, November -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Input"
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
	<xsl:template match="inputgroup">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
      <xsl:variable name="title">Input Group : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" /></xsl:variable>
      <xsl:value-of select="DisplayComps:writeStringFullColumnTH($title, $fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Input
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="inputseries">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Input Series
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
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
			  <td scope="row" colspan="1"><strong>OC Total </strong></td>
			  <td>
				  <xsl:value-of select="@TOCCost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TOCCost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>OC AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TOCAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TOCAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>OC PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TOCPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TOCPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>OC BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TOCBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TOCBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>OC BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TOCBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TOCBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>AOH Total </strong></td>
			  <td>
				  <xsl:value-of select="@TAOHCost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAOHCost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>AOH AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAOHAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAOHAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>AOH PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAOHPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAOHPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>AOH BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAOHBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAOHBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>AOH BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TAOHBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TAOHBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>CAP Total </strong></td>
			  <td>
				  <xsl:value-of select="@TCAPCost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TCAPCost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>CAP AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TCAPAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TCAPAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>CAP PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TCAPPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TCAPPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>CAP BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TCAPBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TCAPBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>CAP BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TCAPBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TCAPBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCC Total </strong></td>
			  <td>
				  <xsl:value-of select="@TLCCCost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCCCost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCC AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TLCCAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCCAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCC PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TLCCPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCCPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCC BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TLCCBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCCBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCC BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TLCCBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCCBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>EAA Total </strong></td>
			  <td>
				  <xsl:value-of select="@TEAACost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TEAACost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>EAA AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TEAAAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TEAAAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>EAA PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TEAAPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TEAAPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>EAA BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TEAABaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TEAABaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>EAA BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TEAABasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TEAABasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Unit Total </strong></td>
			  <td>
				  <xsl:value-of select="@TUnitCost"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TUnitCost_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Unit AmountChange</strong></td>
			  <td>
				  <xsl:value-of select="@TUnitAmountChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TUnitAmountChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Unit PercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TUnitPercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TUnitPercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Unit BaseChange</strong></td>
			  <td>
				  <xsl:value-of select="@TUnitBaseChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TUnitBaseChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Unit BasePercentChange</strong></td>
			  <td>
				  <xsl:value-of select="@TUnitBasePercentChange"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TUnitBasePercentChange_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
    <tr>
      <td colspan="10"><strong>SubCost Totals</strong></td>
    </tr>
      <xsl:if test="(@TSubP1Name1_0 != '' and @TSubP1Name1_0 != 'none') or (@TSubP1Name1_1 != '' and @TSubP1Name1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubCost 1 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount1_0 != '' and @TSubP1Amount1_0 != 'none') or (@TSubP1Amount1_1 != '' and @TSubP1Amount1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubCost 1 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit1_0 != '' and @TSubP1Unit1_0 != 'none') or (@TSubP1Unit1_1 != '' and @TSubP1Unit1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubCost 1 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price1_0 != '' and @TSubP1Price1_0 != 'none') or (@TSubP1Price1_1 != '' and @TSubP1Price1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubCost 1 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total1_0 != '' and @TSubP1Total1_0 != 'none') or (@TSubP1Total1_1 != '' and @TSubP1Total1_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 1 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit1_0 != '' and @TSubP1TotalPerUnit1_0 != 'none') or (@TSubP1TotalPerUnit1_1 != '' and @TSubP1TotalPerUnit1_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 1 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name2_0 != '' and @TSubP1Name2_0 != 'none') or (@TSubP1Name2_1 != '' and @TSubP1Name2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount2_0 != '' and @TSubP1Amount2_0 != 'none') or (@TSubP1Amount2_1 != '' and @TSubP1Amount2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit2_0 != '' and @TSubP1Unit2_0 != 'none') or (@TSubP1Unit2_1 != '' and @TSubP1Unit2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price2_0 != '' and @TSubP1Price2_0 != 'none') or (@TSubP1Price2_1 != '' and @TSubP1Price2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total2_0 != '' and @TSubP1Total2_0 != 'none') or (@TSubP1Total2_1 != '' and @TSubP1Total2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit2_0 != '' and @TSubP1TotalPerUnit2_0 != 'none') or (@TSubP1TotalPerUnit2_1 != '' and @TSubP1TotalPerUnit2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 2 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name3_0 != '' and @TSubP1Name3_0 != 'none') or (@TSubP1Name3_1 != '' and @TSubP1Name3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount3_0 != '' and @TSubP1Amount3_0 != 'none') or (@TSubP1Amount3_1 != '' and @TSubP1Amount3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit3_0 != '' and @TSubP1Unit3_0 != 'none') or (@TSubP1Unit3_1 != '' and @TSubP1Unit3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price3_0 != '' and @TSubP1Price3_0 != 'none') or (@TSubP1Price3_1 != '' and @TSubP1Price3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total3_0 != '' and @TSubP1Total3_0 != 'none') or (@TSubP1Total3_1 != '' and @TSubP1Total3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit3_0 != '' and @TSubP1TotalPerUnit3_0 != 'none') or (@TSubP1TotalPerUnit3_1 != '' and @TSubP1TotalPerUnit3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 3 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name4_0 != '' and @TSubP1Name4_0 != 'none') or (@TSubP1Name4_1 != '' and @TSubP1Name4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount4_0 != '' and @TSubP1Amount4_0 != 'none') or (@TSubP1Amount4_1 != '' and @TSubP1Amount4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit4_0 != '' and @TSubP1Unit4_0 != 'none') or (@TSubP1Unit4_1 != '' and @TSubP1Unit4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price4_0 != '' and @TSubP1Price4_0 != 'none') or (@TSubP1Price4_1 != '' and @TSubP1Price4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total4_0 != '' and @TSubP1Total4_0 != 'none') or (@TSubP1Total4_1 != '' and @TSubP1Total4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit4_0 != '' and @TSubP1TotalPerUnit4_0 != 'none') or (@TSubP1TotalPerUnit4_1 != '' and @TSubP1TotalPerUnit4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 4 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name5_0 != '' and @TSubP1Name5_0 != 'none') or (@TSubP1Name5_1 != '' and @TSubP1Name5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount5_0 != '' and @TSubP1Amount5_0 != 'none') or (@TSubP1Amount5_1 != '' and @TSubP1Amount5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit5_0 != '' and @TSubP1Unit5_0 != 'none') or (@TSubP1Unit5_1 != '' and @TSubP1Unit5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price5_0 != '' and @TSubP1Price5_0 != 'none') or (@TSubP1Price5_1 != '' and @TSubP1Price5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total5_0 != '' and @TSubP1Total5_0 != 'none') or (@TSubP1Total5_1 != '' and @TSubP1Total5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit5_0 != '' and @TSubP1TotalPerUnit5_0 != 'none') or (@TSubP1TotalPerUnit5_1 != '' and @TSubP1TotalPerUnit5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 5 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name6_0 != '' and @TSubP1Name6_0 != 'none') or (@TSubP1Name6_1 != '' and @TSubP1Name6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount6_0 != '' and @TSubP1Amount6_0 != 'none') or (@TSubP1Amount6_1 != '' and @TSubP1Amount6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit6_0 != '' and @TSubP1Unit6_0 != 'none') or (@TSubP1Unit6_1 != '' and @TSubP1Unit6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price6_0 != '' and @TSubP1Price6_0 != 'none') or (@TSubP1Price6_1 != '' and @TSubP1Price6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total6_0 != '' and @TSubP1Total6_0 != 'none') or (@TSubP1Total6_1 != '' and @TSubP1Total6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit6_0 != '' and @TSubP1TotalPerUnit6_0 != 'none') or (@TSubP1TotalPerUnit6_1 != '' and @TSubP1TotalPerUnit6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 6 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name7_0 != '' and @TSubP1Name7_0 != 'none') or (@TSubP1Name7_1 != '' and @TSubP1Name7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount7_0 != '' and @TSubP1Amount7_0 != 'none') or (@TSubP1Amount7_1 != '' and @TSubP1Amount7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit7_0 != '' and @TSubP1Unit7_0 != 'none') or (@TSubP1Unit7_1 != '' and @TSubP1Unit7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price7_0 != '' and @TSubP1Price7_0 != 'none') or (@TSubP1Price7_1 != '' and @TSubP1Price7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total7_0 != '' and @TSubP1Total7_0 != 'none') or (@TSubP1Total7_1 != '' and @TSubP1Total7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit7_0 != '' and @TSubP1TotalPerUnit7_0 != 'none') or (@TSubP1TotalPerUnit7_1 != '' and @TSubP1TotalPerUnit7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 7 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name8_0 != '' and @TSubP1Name8_0 != 'none') or (@TSubP1Name8_1 != '' and @TSubP1Name8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount8_0 != '' and @TSubP1Amount8_0 != 'none') or (@TSubP1Amount8_1 != '' and @TSubP1Amount8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit8_0 != '' and @TSubP1Unit8_0 != 'none') or (@TSubP1Unit8_1 != '' and @TSubP1Unit8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price8_0 != '' and @TSubP1Price8_0 != 'none') or (@TSubP1Price8_1 != '' and @TSubP1Price8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total8_0 != '' and @TSubP1Total8_0 != 'none') or (@TSubP1Total8_1 != '' and @TSubP1Total8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit8_0 != '' and @TSubP1TotalPerUnit8_0 != 'none') or (@TSubP1TotalPerUnit8_1 != '' and @TSubP1TotalPerUnit8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 8 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name9_0 != '' and @TSubP1Name9_0 != 'none') or (@TSubP1Name9_1 != '' and @TSubP1Name9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount9_0 != '' and @TSubP1Amount9_0 != 'none') or (@TSubP1Amount9_1 != '' and @TSubP1Amount9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit9_0 != '' and @TSubP1Unit9_0 != 'none') or (@TSubP1Unit9_1 != '' and @TSubP1Unit9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price9_0 != '' and @TSubP1Price9_0 != 'none') or (@TSubP1Price9_1 != '' and @TSubP1Price9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total9_0 != '' and @TSubP1Total9_0 != 'none') or (@TSubP1Total9_1 != '' and @TSubP1Total9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit9_0 != '' and @TSubP1TotalPerUnit9_0 != 'none') or (@TSubP1TotalPerUnit9_1 != '' and @TSubP1TotalPerUnit9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 9 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name10_0 != '' and @TSubP1Name10_0 != 'none') or (@TSubP1Name10_1 != '' and @TSubP1Name10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Name10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Amount10_0 != '' and @TSubP1Amount10_0 != 'none') or (@TSubP1Amount10_1 != '' and @TSubP1Amount10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Amount10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Unit10_0 != '' and @TSubP1Unit10_0 != 'none') or (@TSubP1Unit10_1 != '' and @TSubP1Unit10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Unit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Price10_0 != '' and @TSubP1Price10_0 != 'none') or (@TSubP1Price10_1 != '' and @TSubP1Price10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Price10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Total10_0 != '' and @TSubP1Total10_0 != 'none') or (@TSubP1Total10_1 != '' and @TSubP1Total10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1Total10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1TotalPerUnit10_0 != '' and @TSubP1TotalPerUnit10_0 != 'none') or (@TSubP1TotalPerUnit10_1 != '' and @TSubP1TotalPerUnit10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubCost 10 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
