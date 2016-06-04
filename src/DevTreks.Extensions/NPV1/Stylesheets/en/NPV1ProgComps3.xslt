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
        C or B Type
      </th>
      <th>
        Plan Period
      </th>
      <th>
        Plan Full
      </th>
      <th>
        Plan Cumul
      </th>
      <th>
        Actual Period
      </th>
      <th>
        Actual Cumul
      </th>
      <th>
        Actual Period Change
      </th>
      <th>
        Actual Cumul Change
      </th>
      <th>
        Plan P Percent ; Plan C Percent
      </th>
      <th>
        Plan Full Percent
      </th>
    </tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
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
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />&#xA0;Label:&#xA0;<xsl:value-of select="@Num" /></strong>
			</td>
		</tr>
    <tr>
      <th>
        C or B Type
      </th>
      <th>
        Plan Period
      </th>
      <th>
        Plan Full
      </th>
      <th>
        Plan Cumul
      </th>
      <th>
        Actual Period
      </th>
      <th>
        Actual Cumul
      </th>
      <th>
        Actual Period Change
      </th>
      <th>
        Actual Cumul Change
      </th>
      <th>
        Plan P Percent ; Plan C Percent
      </th>
      <th>
        Plan Full Percent
      </th>
    </tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'componentinput')">
      <tr>
			  <td colspan="10">
				  Date : <xsl:value-of select="@Date"/> ; Observations: <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
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
          <xsl:value-of select="@TAMOCPFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCPCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCAPTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCAPChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCPPPercent"/> ; <xsl:value-of select="@TAMOCPCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOCPFPercent"/>
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
          <xsl:value-of select="@TAMAOHPFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHPCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHAPTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHAPChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHPPPercent"/> ; <xsl:value-of select="@TAMAOHPCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOHPFPercent"/>
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
          <xsl:value-of select="@TAMCAPPFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPPCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPAPTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPAPChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPPPPercent"/> ; <xsl:value-of select="@TAMCAPPCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAPPFPercent"/>
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
          <xsl:value-of select="@TAMPFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMPCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAPTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAPChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMPPPercent"/> ; <xsl:value-of select="@TAMPCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TAMPFPercent"/>
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
          <xsl:value-of select="@TAMIncentPFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentPCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentAPTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentAPChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentPPPercent"/> ; <xsl:value-of select="@TAMIncentPCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TAMIncentPFPercent"/>
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
