<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
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
					<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
    <tr>
			<th scope="col" colspan="10">
				Budget Group : <xsl:value-of select="@Name" /> 
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget : <xsl:value-of select="@Name" /> 
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10">
				Time Period : <xsl:value-of select="@Name" /> 
			</th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="budgetoutcomes" />
		<xsl:apply-templates select="budgetoperations" />
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<tr>
			<th scope="col" colspan="10"><strong>Outcome</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
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
          Score Amount Change
			  </th>
        <th>
				  Score Percent Change
			  </th>
        <th>
          Score Base Change
			  </th>
			  <th>
				  Score Base Percent Change
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
          Score Low Amount Change
			  </th>
        <th>
				  Score Low Percent Change
			  </th>
        <th>
          Score Low Base Change
			  </th>
			  <th>
				  Score Low Base Percent Change
			  </th>
			  <th>
				  Score High
			  </th>
        <th>
          Score High Amount Change
			  </th>
        <th>
				  Score High Percent Change
			  </th>
        <th>
          Score High Base Change
			  </th>
			  <th>
				  Score High Base Percent Change
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
				  <xsl:value-of select="@TSBScoreAmountChange"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScorePercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreBaseChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreBasePercentChange"/>
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
				  <xsl:value-of select="@TSBScoreLAmountChange"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreLPercentChange"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreLBaseChange"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreLBasePercentChange"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreUAmount"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreUAmountChange"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreUPercentChange"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreUBaseChange"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreUBasePercentChange"/>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TSB1Name1 != '')">
      <tr>
        <th>
				  Name
			  </th>
        <th>
          Observations
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
		  </tr>
      <tr>
			  <td colspan="10">
				   Date : <xsl:value-of select="@Date"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
			  </td>
      </tr>
      <xsl:if test="(string-length(@TSB1Name1) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name1"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N1"/>
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
					    <xsl:value-of select="@TSB1AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB1PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB1BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB1BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name2) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name2"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N2"/>
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
					    <xsl:value-of select="@TSB2AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB2PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB2BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB2BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name3) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name3"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N3"/>
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
					    <xsl:value-of select="@TSB3AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB3PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB3BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB3BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name4) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name4"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N4"/>
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
					    <xsl:value-of select="@TSB4AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB4PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB4BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB4BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name5) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name5"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N5"/>
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
					    <xsl:value-of select="@TSB5AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB5PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB5BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB5BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name6) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name6"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N6"/>
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
					    <xsl:value-of select="@TSB6AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB6PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB6BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB6BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name7) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name7"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N7"/>
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
					    <xsl:value-of select="@TSB7AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB7PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB7BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB7BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name8) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name8"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N8"/>
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
					    <xsl:value-of select="@TSB8AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB8PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB8BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB8BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name9) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name9"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N9"/>
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
					    <xsl:value-of select="@TSB9AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB9PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB9BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB9BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(string-length(@TSB1Name10) > 0)">
		    <tr>
			    <td>
				    <xsl:value-of select="@TSB1Name10"/>
			    </td>
          <td>
				    <xsl:value-of select="@TSB1N10"/>
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
					    <xsl:value-of select="@TSB10AmountChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB10PercentChange"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSB10BaseChange"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSB10BasePercentChange"/>
			    </td>
          <td>
			    </td>
		    </tr>
       </xsl:if>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
