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
    <xsl:variable name="count" select="count(outcomeoutput)"/>
    <xsl:if test="($count > 0)">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'outcomeoutput')">
      <tr>
			  <td colspan="10">
				  Date : <xsl:value-of select="@Date"/> ; Observations: <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
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
            <xsl:value-of select="@TAMRPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRPPPercent"/> ; <xsl:value-of select="@TAMRPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRPFPercent"/>
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
            <xsl:value-of select="@TRAmountPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountPPPercent"/> ; <xsl:value-of select="@TRAmountPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TRAmountPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            Incentives
          </td>
          <td>
            <xsl:value-of select="@TAMRINCENT"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentPPPercent"/> ; <xsl:value-of select="@TAMRIncentPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TAMRIncentPFPercent"/>
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
            <xsl:value-of select="@TRPricePFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRPricePCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRPriceAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRPriceACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRPriceAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRPriceACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRPricePPPercent"/> ; <xsl:value-of select="@TRPricePCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TRPricePFPercent"/>
          </td>
        </tr>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
      <tr>
        <td>
			  </td>
			  <td>
          Total: <xsl:value-of select="@TAMR"/>
			  </td>
			  <td>
				  Output Q: <xsl:value-of select="@TRAmount"/>
			  </td>
			  <td>
          Output P: <xsl:value-of select="@TRPrice"/>
			  </td>
        <td>
          Incent: <xsl:value-of select="@TAMRINCENT"/>
			  </td>
			  <td colspan="5">
			  </td>
		  </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
