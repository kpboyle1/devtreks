<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, December -->
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TSB1ScoreMUnit != '')">
      <tr>
        <th>
          Observations
			  </th>
        <th>
				  Score Most Likely
			  </th>
        <th>
          Unit
			  </th>
        <th>
          Score Mean
			  </th>
        <th>
				  Score Median
			  </th>
        <th>
          Score Variance
			  </th>
			  <th>
				  Score Std Dev
			  </th>
			  <th>
          Score Low Unit
			  </th>
        <th>
          Score High Unit
			  </th>
			  <th>
			  </th>
		  </tr>
      <tr>
        <th>
				  Score Low
			  </th>
        <th>
          Score Low Mean
			  </th>
        <th>
				  Score Low Median
			  </th>
        <th>
          Score Low Variance
			  </th>
			  <th>
				  Score Low Std Dev
			  </th>
			  <th>
				  Score High
			  </th>
        <th>
          Score High Mean
			  </th>
        <th>
				  Score High Median
			  </th>
        <th>
          Score High Variance
			  </th>
			  <th>
				  Score High Std Dev
			  </th>
		  </tr>
      <tr>
        <td>
				  <xsl:value-of select="@TSB1ScoreN"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreM"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1ScoreMUnit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreMean"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreMedian"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreVariance"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreStandDev"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB1ScoreLUnit"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB1ScoreUUnit"/>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td>
				  <xsl:value-of select="@TSB1ScoreLAmount"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreLMean"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreLMedian"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreLVariance"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreLStandDev"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreUAmount"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreUMean"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreUMedian"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreUVariance"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreUStandDev"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSB1Name1 != '')">
    <tr>
      <th>
        Observations
			</th>
      <th>
				Name
			</th>
      <th>
        Label
			</th>
      <th>
				Total
			</th>
      <th>
        Unit
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
			<th>
			</th>
		</tr>
    <xsl:if test="(string-length(@TSB1Name1) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N1"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name1"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label1"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount1"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB1Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB1Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB1Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB1StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="(string-length(@TSB1Name2) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N2"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name2"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label2"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount2"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB2Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB2Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB2Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB2StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
      <xsl:if test="(string-length(@TSB1Name3) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N3"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name3"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label3"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount3"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB3Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB3Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB3Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB3StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="(string-length(@TSB1Name4) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N4"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name4"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label4"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount4"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB4Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB4Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB4Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB4StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
      <xsl:if test="(string-length(@TSB1Name5) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N5"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name5"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label5"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount5"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB5Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB5Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB5Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB5StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="(string-length(@TSB1Name6) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N6"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name6"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label6"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount6"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB6Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB6Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB6Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB6StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
      <xsl:if test="(string-length(@TSB1Name7) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N7"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name7"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label7"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount7"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB7Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB7Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB7Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB7StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="(string-length(@TSB1Name8) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N8"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name8"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label8"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount8"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB8Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB8Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB8Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB8StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
      <xsl:if test="(string-length(@TSB1Name9) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N9"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name9"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label9"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount9"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB9Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB9Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB9Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB9StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="(string-length(@TSB1Name10) > 0)">
			<tr>
        <td>
				  <xsl:value-of select="@TSB1N10"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1Name10"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1Label10"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1TMAmount10"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSB1TMUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB10Mean"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB10Median"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSB10Variance"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSB10StandDev"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
