<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
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
		<tr>
			<th scope="col" colspan="10">
				Output Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
    <tr>
			<th scope="col" colspan="10"><strong>Output</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
    <tr>
			<th scope="col" colspan="10"><strong>Output Series</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TME2Stage != '' and @TME2Stage != 'none')">
      <tr>
			  <td scope="row" colspan="10">
          <strong>Monitoring and Evaluation Type: </strong><xsl:value-of select="@TME2Stage" />
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
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name0" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N0" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label0" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit0"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev0"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev0"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev0"/>
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
				  <xsl:value-of select="@TME2Description0" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name1" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N1" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label1" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit1"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev1"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev1"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev1"/>
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
    <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name2" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N2" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label2" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit2"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev2"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev2"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev2"/>
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
    <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name3" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N3" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label3" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit3"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev3"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev3"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev3"/>
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
    <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name4" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N4" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label4" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit4"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev4"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev4"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev4"/>
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
    <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name5" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N5" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label5" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit5"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev5"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev5"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev5"/>
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
    <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name6" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N6" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label6" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit6"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev6"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev6"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev6"/>
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
    <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name7" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N7" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label7" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit7"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev7"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev7"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev7"/>
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
    <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name8" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N8" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label8" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit8"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev8"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev8"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev8"/>
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
    <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name9" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N9" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label9" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit9"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev9"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev9"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev9"/>
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
    <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name10" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N10" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label10" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit10"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev10"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev10"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev10"/>
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
    <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name11" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N11" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label11" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit11"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev11"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev11"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev11"/>
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
				  <xsl:value-of select="@TME2Description11" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name12" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N12" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label12" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit12"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev12"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev12"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev12"/>
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
				  <xsl:value-of select="@TME2Description12" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name13" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N13" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label13" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit13"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev13"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev13"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev13"/>
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
				  <xsl:value-of select="@TME2Description14" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name14" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N14" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label14" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit14"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev14"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev14"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev14"/>
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
				  <xsl:value-of select="@TME2Description14" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name15" /></strong>
          <br />
          <strong>Observations: </strong><xsl:value-of select="@TME2N15" />&#xA0;:&#xA0;<xsl:value-of select="@TME2Label15" />
			  </td>
		  </tr>
			<tr>
        <td>
					  Most Likely
			  </td>
        <td>
          <xsl:value-of select="@TME2TMUnit15"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMean15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MMedian15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MVariance15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MStandDev15"/>
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
          Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMean15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LMedian15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LVariance15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LStandDev15"/>
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
          Upper
			  </td>
			  <td>
				    <xsl:value-of select="@TME2TUUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMean15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UMedian15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UVariance15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UStandDev15"/>
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
				  <xsl:value-of select="@TME2Description15" />
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>