<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, December -->
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="componentgroup" />
				<div>
					<a id="aFeedback" name="Feedback">
						<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
						Feedback About <xsl:value-of select="$selectedFileURIPattern" />
					</a>
        </div>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<h4 class="ui-bar-b">
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="componentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentgroup">
		<h4>
      <strong>Component Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="component">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="component">
    <h4>
      <strong>Component </strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="componentinput">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentinput">
    <h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TSB1ScoreMUnit != '' or @TSB1Name1 != '')">
		<div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Indicators</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <xsl:if test="(string-length(@TSB1ScoreMUnit) > 0)">
            <div class="ui-block-a">
              Observations 1: <xsl:value-of select="@TSB1ScoreN"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Score Planned Period: <xsl:value-of select="@TSB1ScoreM"/>
            </div>
            <div class="ui-block-b">
              Score Unit: <xsl:value-of select="@TSB1ScoreMUnit"/>
            </div>
            <div class="ui-block-a">
              Score Plan Full: <xsl:value-of select="@TSBScorePFTotal"/>
            </div>
            <div class="ui-block-b">
              Score Plan Cumul: <xsl:value-of select="@TSBScorePCTotal"/>
            </div>
            <div class="ui-block-a">
              Score Actual Period: <xsl:value-of select="@TSBScoreAPTotal"/>
            </div>
            <div class="ui-block-b">  
              Score Actual Cumul: <xsl:value-of select="@TSBScoreACTotal"/>
            </div>
            <div class="ui-block-a">
              Score Actual Period Change: <xsl:value-of select="@TSBScoreAPChange"/>
            </div>
            <div class="ui-block-b">
              Score Actual Cumul Change: <xsl:value-of select="@TSBScoreACChange"/>
            </div>
            <div class="ui-block-a">
              Score Planned Period Percent: <xsl:value-of select="@TSBScorePPPercent"/>
            </div>
            <div class="ui-block-b">
              Score Planned Cumul Percent: <xsl:value-of select="@TSBScorePCPercent"/>
            </div>
            <div class="ui-block-a">
              Score Planned Full Percent: <xsl:value-of select="@TSBScorePFPercent"/>
            </div>
            <div class="ui-block-a">
              Score Low Planned Period: <xsl:value-of select="@TSB1ScoreLAmount"/>
            </div>
            <div class="ui-block-b">
              Score Low Unit: <xsl:value-of select="@TSB1ScoreLUnit"/>
            </div>
            <div class="ui-block-a">
              Score Low Plan Full: <xsl:value-of select="@TSBScoreLPFTotal"/>
            </div>
            <div class="ui-block-b">
              Score Low Plan Cumul: <xsl:value-of select="@TSBScoreLPCTotal"/>
            </div>
            <div class="ui-block-a">
              Score Low Actual Period: <xsl:value-of select="@TSBScoreLAPTotal"/>
            </div>
            <div class="ui-block-b">  
              Score Low Actual Cumul: <xsl:value-of select="@TSBScoreLACTotal"/>
            </div>
            <div class="ui-block-a">
              Score Low Actual Period Change: <xsl:value-of select="@TSBScoreLAPChange"/>
            </div>
            <div class="ui-block-b">
              Score Low Actual Cumul Change: <xsl:value-of select="@TSBScoreLACChange"/>
            </div>
            <div class="ui-block-a">
              Score Low Planned Period Percent: <xsl:value-of select="@TSBScoreLPPPercent"/>
            </div>
            <div class="ui-block-b">
              Score Low Planned Cumul Percent: <xsl:value-of select="@TSBScoreLPCPercent"/>
            </div>
            <div class="ui-block-a">
              Score Low Planned Full Percent: <xsl:value-of select="@TSBScoreLPFPercent"/>
            </div>
            <div class="ui-block-a">
              Score High Planned Period: <xsl:value-of select="@TSB1ScoreUAmount"/>
            </div>
            <div class="ui-block-b">
              Score High Unit: <xsl:value-of select="@TSB1ScoreUUnit"/>
            </div>
            <div class="ui-block-a">
              Score High Plan Full: <xsl:value-of select="@TSBScoreUPFTotal"/>
            </div>
            <div class="ui-block-b">
              Score High Plan Cumul: <xsl:value-of select="@TSBScoreUPCTotal"/>
            </div>
            <div class="ui-block-a">
              Score High Actual Period: <xsl:value-of select="@TSBScoreUAPTotal"/>
            </div>
            <div class="ui-block-b">  
              Score High Actual Cumul: <xsl:value-of select="@TSBScoreUACTotal"/>
            </div>
            <div class="ui-block-a">
              Score High Actual Period Change: <xsl:value-of select="@TSBScoreUAPChange"/>
            </div>
            <div class="ui-block-b">
              Score High Actual Cumul Change: <xsl:value-of select="@TSBScoreUACChange"/>
            </div>
            <div class="ui-block-a">
              Score High Planned Period Percent: <xsl:value-of select="@TSBScoreUPPPercent"/>
            </div>
            <div class="ui-block-b">
              Score High Planned Cumul Percent: <xsl:value-of select="@TSBScoreUPCPercent"/>
            </div>
            <div class="ui-block-a">
              Score High Planned Full Percent: <xsl:value-of select="@TSBScoreUPFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name1) > 0)">
            <div class="ui-block-a">
              Name 1: <strong><xsl:value-of select="@TSB1Name1"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 1: <xsl:value-of select="@TSB1N1"/>
            </div>
            <div class="ui-block-b">
              Label 1: <xsl:value-of select="@TSB1Label1"/>
            </div>
            <div class="ui-block-a">
              Planned Period 1: <xsl:value-of select="@TSB1TMAmount1"/>
            </div>
            <div class="ui-block-b">
              Unit 1: <xsl:value-of select="@TSB1TMUnit1"/>
            </div>
            <div class="ui-block-b">
              Plan Full 1: <xsl:value-of select="@TSB1PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 1: <xsl:value-of select="@TSB1PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 1: <xsl:value-of select="@TSB1APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 1: <xsl:value-of select="@TSB1ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 1: <xsl:value-of select="@TSB1APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 1: <xsl:value-of select="@TSB1ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 1: <xsl:value-of select="@TSB1PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 1: <xsl:value-of select="@TSB1PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 1: <xsl:value-of select="@TSB1PFPercent"/>
            </div>
          </xsl:if>
           <xsl:if test="(string-length(@TSB1Name2) > 0)">
            <div class="ui-block-a">
              Name 2: <strong><xsl:value-of select="@TSB1Name2"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 2: <xsl:value-of select="@TSB1N2"/>
            </div>
            <div class="ui-block-b">
              Label 2: <xsl:value-of select="@TSB1Label2"/>
            </div>
            <div class="ui-block-a">
              Planned Period 2: <xsl:value-of select="@TSB1TMAmount2"/>
            </div>
            <div class="ui-block-b">
              Unit 2: <xsl:value-of select="@TSB1TMUnit2"/>
            </div>
            <div class="ui-block-b">
              Plan Full 2: <xsl:value-of select="@TSB2PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 2: <xsl:value-of select="@TSB2PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 2: <xsl:value-of select="@TSB2APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 2: <xsl:value-of select="@TSB2ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 2: <xsl:value-of select="@TSB2APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 2: <xsl:value-of select="@TSB2ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 2: <xsl:value-of select="@TSB2PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 2: <xsl:value-of select="@TSB2PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 2: <xsl:value-of select="@TSB2PFPercent"/>
            </div>
          </xsl:if>
           <xsl:if test="(string-length(@TSB1Name3) > 0)">
            <div class="ui-block-a">
              Name 3: <strong><xsl:value-of select="@TSB1Name3"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 3: <xsl:value-of select="@TSB1N3"/>
            </div>
            <div class="ui-block-b">
              Label 3: <xsl:value-of select="@TSB1Label3"/>
            </div>
            <div class="ui-block-a">
              Planned Period 3: <xsl:value-of select="@TSB1TMAmount3"/>
            </div>
            <div class="ui-block-b">
              Unit 3: <xsl:value-of select="@TSB1TMUnit3"/>
            </div>
            <div class="ui-block-b">
              Plan Full 3: <xsl:value-of select="@TSB3PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 3: <xsl:value-of select="@TSB3PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 3: <xsl:value-of select="@TSB3APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 3: <xsl:value-of select="@TSB3ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 3: <xsl:value-of select="@TSB3APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 3: <xsl:value-of select="@TSB3ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 3: <xsl:value-of select="@TSB3PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 3: <xsl:value-of select="@TSB3PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 3: <xsl:value-of select="@TSB3PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name4) > 0)">
            <div class="ui-block-a">
              Name 4: <strong><xsl:value-of select="@TSB1Name4"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 4: <xsl:value-of select="@TSB1N4"/>
            </div>
            <div class="ui-block-b">
              Label 4: <xsl:value-of select="@TSB1Label4"/>
            </div>
            <div class="ui-block-a">
              Planned Period 4: <xsl:value-of select="@TSB1TMAmount4"/>
            </div>
            <div class="ui-block-b">
              Unit 4: <xsl:value-of select="@TSB1TMUnit4"/>
            </div>
            <div class="ui-block-b">
              Plan Full 4: <xsl:value-of select="@TSB4PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 4: <xsl:value-of select="@TSB4PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 4: <xsl:value-of select="@TSB4APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 4: <xsl:value-of select="@TSB4ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 4: <xsl:value-of select="@TSB4APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 4: <xsl:value-of select="@TSB4ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 4: <xsl:value-of select="@TSB4PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 4: <xsl:value-of select="@TSB4PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 4: <xsl:value-of select="@TSB4PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name5) > 0)">
            <div class="ui-block-a">
              Name 5: <strong><xsl:value-of select="@TSB1Name5"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 5: <xsl:value-of select="@TSB1N5"/>
            </div>
            <div class="ui-block-b">
              Label 5: <xsl:value-of select="@TSB1Label5"/>
            </div>
            <div class="ui-block-a">
              Planned Period 5: <xsl:value-of select="@TSB1TMAmount5"/>
            </div>
            <div class="ui-block-b">
              Unit 5: <xsl:value-of select="@TSB1TMUnit5"/>
            </div>
            <div class="ui-block-b">
              Plan Full 5: <xsl:value-of select="@TSB5PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 5: <xsl:value-of select="@TSB5PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 5: <xsl:value-of select="@TSB5APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 5: <xsl:value-of select="@TSB5ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 5: <xsl:value-of select="@TSB5APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 5: <xsl:value-of select="@TSB5ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 5: <xsl:value-of select="@TSB5PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 5: <xsl:value-of select="@TSB5PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 5: <xsl:value-of select="@TSB5PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name6) > 0)">
            <div class="ui-block-a">
              Name 6: <strong><xsl:value-of select="@TSB1Name6"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 6: <xsl:value-of select="@TSB1N6"/>
            </div>
            <div class="ui-block-b">
              Label 6: <xsl:value-of select="@TSB1Label6"/>
            </div>
            <div class="ui-block-a">
              Planned Period 6: <xsl:value-of select="@TSB1TMAmount6"/>
            </div>
            <div class="ui-block-b">
              Unit 6: <xsl:value-of select="@TSB1TMUnit6"/>
            </div>
            <div class="ui-block-b">
              Plan Full 6: <xsl:value-of select="@TSB6PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 6: <xsl:value-of select="@TSB6PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 6: <xsl:value-of select="@TSB6APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 6: <xsl:value-of select="@TSB6ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 6: <xsl:value-of select="@TSB6APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 6: <xsl:value-of select="@TSB6ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 6: <xsl:value-of select="@TSB6PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 6: <xsl:value-of select="@TSB6PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 6: <xsl:value-of select="@TSB6PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name7) > 0)">
            <div class="ui-block-a">
              Name 7: <strong><xsl:value-of select="@TSB1Name7"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 7: <xsl:value-of select="@TSB1N7"/>
            </div>
            <div class="ui-block-b">
              Label 7: <xsl:value-of select="@TSB1Label7"/>
            </div>
            <div class="ui-block-a">
              Planned Period 7: <xsl:value-of select="@TSB1TMAmount7"/>
            </div>
            <div class="ui-block-b">
              Unit 7: <xsl:value-of select="@TSB1TMUnit7"/>
            </div>
            <div class="ui-block-b">
              Plan Full 7: <xsl:value-of select="@TSB7PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 7: <xsl:value-of select="@TSB7PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 7: <xsl:value-of select="@TSB7APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 7: <xsl:value-of select="@TSB7ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 7: <xsl:value-of select="@TSB7APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 7: <xsl:value-of select="@TSB7ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 7: <xsl:value-of select="@TSB7PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 7: <xsl:value-of select="@TSB7PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 7: <xsl:value-of select="@TSB7PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name8) > 0)">
            <div class="ui-block-a">
              Name 8: <strong><xsl:value-of select="@TSB1Name8"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 8: <xsl:value-of select="@TSB1N8"/>
            </div>
            <div class="ui-block-b">
              Label 8: <xsl:value-of select="@TSB1Label8"/>
            </div>
            <div class="ui-block-a">
              Planned Period 8: <xsl:value-of select="@TSB1TMAmount8"/>
            </div>
            <div class="ui-block-b">
              Unit 8: <xsl:value-of select="@TSB1TMUnit8"/>
            </div>
            <div class="ui-block-b">
              Plan Full 8: <xsl:value-of select="@TSB8PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 8: <xsl:value-of select="@TSB8PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 8: <xsl:value-of select="@TSB8APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 8: <xsl:value-of select="@TSB8ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 8: <xsl:value-of select="@TSB8APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 8: <xsl:value-of select="@TSB8ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 8: <xsl:value-of select="@TSB8PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 8: <xsl:value-of select="@TSB8PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 8: <xsl:value-of select="@TSB8PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name9) > 0)">
            <div class="ui-block-a">
              Name 9: <strong><xsl:value-of select="@TSB1Name9"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 9: <xsl:value-of select="@TSB1N9"/>
            </div>
            <div class="ui-block-b">
              Label 9: <xsl:value-of select="@TSB1Label9"/>
            </div>
            <div class="ui-block-a">
              Planned Period 9: <xsl:value-of select="@TSB1TMAmount9"/>
            </div>
            <div class="ui-block-b">
              Unit 9: <xsl:value-of select="@TSB1TMUnit9"/>
            </div>
            <div class="ui-block-b">
              Plan Full 9: <xsl:value-of select="@TSB9PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 9: <xsl:value-of select="@TSB9PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 9: <xsl:value-of select="@TSB9APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 9: <xsl:value-of select="@TSB9ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 9: <xsl:value-of select="@TSB9APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 9: <xsl:value-of select="@TSB9ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 9: <xsl:value-of select="@TSB9PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 9: <xsl:value-of select="@TSB9PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 9: <xsl:value-of select="@TSB9PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TSB1Name10) > 0)">
            <div class="ui-block-a">
              Name 10: <strong><xsl:value-of select="@TSB1Name10"/></strong>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Observations 10: <xsl:value-of select="@TSB1N10"/>
            </div>
            <div class="ui-block-b">
              Label 10: <xsl:value-of select="@TSB1Label10"/>
            </div>
            <div class="ui-block-a">
              Planned Period 10: <xsl:value-of select="@TSB1TMAmount10"/>
            </div>
            <div class="ui-block-b">
              Unit 10: <xsl:value-of select="@TSB1TMUnit10"/>
            </div>
            <div class="ui-block-b">
              Plan Full 10: <xsl:value-of select="@TSB10PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul 10: <xsl:value-of select="@TSB10PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period 10: <xsl:value-of select="@TSB10APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul 10: <xsl:value-of select="@TSB10ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change 10: <xsl:value-of select="@TSB10APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change 10: <xsl:value-of select="@TSB10ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent 10: <xsl:value-of select="@TSB10PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent 10: <xsl:value-of select="@TSB10PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent 10: <xsl:value-of select="@TSB10PFPercent"/>
            </div>
          </xsl:if>
        </div>
     </div>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
