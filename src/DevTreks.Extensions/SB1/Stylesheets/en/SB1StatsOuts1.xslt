<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, December -->
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="outputgroup" />
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
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<h4>
      <strong>Output Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		 <h4>
      <strong>Output Series </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TSB1ScoreMUnit != '' or @TSB1Name1 != '')">
    <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicators</strong>
      </h4>
      <div class="ui-grid-a">
        <xsl:if test="(string-length(@TSB1ScoreMUnit) > 0)">
          <div class="ui-block-a">
            Observations 1: <xsl:value-of select="@TSB1ScoreN"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Score Most Likely: <xsl:value-of select="@TSB1ScoreM"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@TSB1ScoreMUnit"/>
          </div>
          <div class="ui-block-a">
            Score Mean: <xsl:value-of select="@TSBScoreMean"/>
          </div>
          <div class="ui-block-b">
            Score Median: <xsl:value-of select="@TSBScoreMedian"/>
          </div>
          <div class="ui-block-a">
            Score Variance: <xsl:value-of select="@TSBScoreVariance"/>
          </div>
          <div class="ui-block-b">  
            Score Std Dev: <xsl:value-of select="@TSBScoreStandDev"/>
          </div>
          <div class="ui-block-a">
            Score Low: <xsl:value-of select="@TSB1ScoreLAmount"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@TSB1ScoreLUnit"/>
          </div>
          <div class="ui-block-a">
            Score Low Mean: <xsl:value-of select="@TSBScoreLMean"/>
          </div>
          <div class="ui-block-b">
            Score Low Median: <xsl:value-of select="@TSBScoreLMedian"/>
          </div>
          <div class="ui-block-a">
            Score Low Variance: <xsl:value-of select="@TSBScoreLVariance"/>
          </div>
          <div class="ui-block-b">  
            Score Low Std Dev: <xsl:value-of select="@TSBScoreLStandDev"/>
          </div>
          <div class="ui-block-a">
            Score High: <xsl:value-of select="@TSB1ScoreUAmount"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@TSB1ScoreUUnit"/>
          </div>
          <div class="ui-block-a">
            Score High Mean: <xsl:value-of select="@TSBScoreUMean"/>
          </div>
          <div class="ui-block-b">
            Score High Median: <xsl:value-of select="@TSBScoreUMedian"/>
          </div>
          <div class="ui-block-a">
            Score High Variance: <xsl:value-of select="@TSBScoreUVariance"/>
          </div>
          <div class="ui-block-b">  
            Score High Std Dev: <xsl:value-of select="@TSBScoreUStandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name1) > 0)">
          <div class="ui-block-a">
            Name 1: <strong><xsl:value-of select="@TSB1Name1"/></strong>
          </div>
          <div class="ui-block-b">
            Label 1: <xsl:value-of select="@TSB1Label1"/>
          </div>
          <div class="ui-block-a">
            Observations 1: <xsl:value-of select="@TSB1N1"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 1: <xsl:value-of select="@TSB1TMAmount1"/>
          </div>
          <div class="ui-block-b">
            Unit 1: <xsl:value-of select="@TSB1TMUnit1"/>
          </div>
          <div class="ui-block-a">
            Mean 1: <xsl:value-of select="@TSB1Mean"/>
          </div>
          <div class="ui-block-b">
            Median 1: <xsl:value-of select="@TSB1Median"/>
          </div>
          <div class="ui-block-a">
            Variance 1: <xsl:value-of select="@TSB1Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 1: <xsl:value-of select="@TSB1StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name2) > 0)">
          <div class="ui-block-a">
            Name 2: <strong><xsl:value-of select="@TSB1Name2"/></strong>
          </div>
          <div class="ui-block-b">
            Label 2: <xsl:value-of select="@TSB1Label2"/>
          </div>
          <div class="ui-block-a">
            Observations 2: <xsl:value-of select="@TSB1N2"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 2: <xsl:value-of select="@TSB1TMAmount2"/>
          </div>
          <div class="ui-block-b">
            Unit 2: <xsl:value-of select="@TSB1TMUnit2"/>
          </div>
          <div class="ui-block-a">
            Mean 2: <xsl:value-of select="@TSB2Mean"/>
          </div>
          <div class="ui-block-b">
            Median 2: <xsl:value-of select="@TSB2Median"/>
          </div>
          <div class="ui-block-a">
            Variance 2: <xsl:value-of select="@TSB2Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 2: <xsl:value-of select="@TSB2StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name3) > 0)">
          <div class="ui-block-a">
            Name 3: <strong><xsl:value-of select="@TSB1Name3"/></strong>
          </div>
          <div class="ui-block-b">
            Label 3: <xsl:value-of select="@TSB1Label3"/>
          </div>
          <div class="ui-block-a">
            Observations 3: <xsl:value-of select="@TSB1N3"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 3: <xsl:value-of select="@TSB1TMAmount3"/>
          </div>
          <div class="ui-block-b">
            Unit 3: <xsl:value-of select="@TSB1TMUnit3"/>
          </div>
          <div class="ui-block-a">
            Mean 3: <xsl:value-of select="@TSB3Mean"/>
          </div>
          <div class="ui-block-b">
            Median 3: <xsl:value-of select="@TSB3Median"/>
          </div>
          <div class="ui-block-a">
            Variance 3: <xsl:value-of select="@TSB3Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 3: <xsl:value-of select="@TSB3StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name4) > 0)">
          <div class="ui-block-a">
            Name 4: <strong><xsl:value-of select="@TSB1Name4"/></strong>
          </div>
          <div class="ui-block-b">
            Label 4: <xsl:value-of select="@TSB1Label4"/>
          </div>
          <div class="ui-block-a">
            Observations 4: <xsl:value-of select="@TSB1N4"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 4: <xsl:value-of select="@TSB1TMAmount4"/>
          </div>
          <div class="ui-block-b">
            Unit 4: <xsl:value-of select="@TSB1TMUnit4"/>
          </div>
          <div class="ui-block-a">
            Mean 4: <xsl:value-of select="@TSB4Mean"/>
          </div>
          <div class="ui-block-b">
            Median 4: <xsl:value-of select="@TSB4Median"/>
          </div>
          <div class="ui-block-a">
            Variance 4: <xsl:value-of select="@TSB4Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 4: <xsl:value-of select="@TSB4StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name5) > 0)">
          <div class="ui-block-a">
            Name 5: <strong><xsl:value-of select="@TSB1Name5"/></strong>
          </div>
          <div class="ui-block-b">
            Label 5: <xsl:value-of select="@TSB1Label5"/>
          </div>
          <div class="ui-block-a">
            Observations 5: <xsl:value-of select="@TSB1N5"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 5: <xsl:value-of select="@TSB1TMAmount5"/>
          </div>
          <div class="ui-block-b">
            Unit 5: <xsl:value-of select="@TSB1TMUnit5"/>
          </div>
          <div class="ui-block-a">
            Mean 5: <xsl:value-of select="@TSB5Mean"/>
          </div>
          <div class="ui-block-b">
            Median 5: <xsl:value-of select="@TSB5Median"/>
          </div>
          <div class="ui-block-a">
            Variance 5: <xsl:value-of select="@TSB5Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 5: <xsl:value-of select="@TSB5StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name6) > 0)">
          <div class="ui-block-a">
            Name 6: <strong><xsl:value-of select="@TSB1Name6"/></strong>
          </div>
          <div class="ui-block-b">
            Label 6: <xsl:value-of select="@TSB1Label6"/>
          </div>
          <div class="ui-block-a">
            Observations 6: <xsl:value-of select="@TSB1N6"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 6: <xsl:value-of select="@TSB1TMAmount6"/>
          </div>
          <div class="ui-block-b">
            Unit 6: <xsl:value-of select="@TSB1TMUnit6"/>
          </div>
          <div class="ui-block-a">
            Mean 6: <xsl:value-of select="@TSB6Mean"/>
          </div>
          <div class="ui-block-b">
            Median 6: <xsl:value-of select="@TSB6Median"/>
          </div>
          <div class="ui-block-a">
            Variance 6: <xsl:value-of select="@TSB6Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 6: <xsl:value-of select="@TSB6StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name7) > 0)">
          <div class="ui-block-a">
            Name 7: <strong><xsl:value-of select="@TSB1Name7"/></strong>
          </div>
          <div class="ui-block-b">
            Label 7: <xsl:value-of select="@TSB1Label7"/>
          </div>
          <div class="ui-block-a">
            Observations 7: <xsl:value-of select="@TSB1N7"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 7: <xsl:value-of select="@TSB1TMAmount7"/>
          </div>
          <div class="ui-block-b">
            Unit 7: <xsl:value-of select="@TSB1TMUnit7"/>
          </div>
          <div class="ui-block-a">
            Mean 7: <xsl:value-of select="@TSB7Mean"/>
          </div>
          <div class="ui-block-b">
            Median 7: <xsl:value-of select="@TSB7Median"/>
          </div>
          <div class="ui-block-a">
            Variance 7: <xsl:value-of select="@TSB7Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 7: <xsl:value-of select="@TSB7StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name8) > 0)">
          <div class="ui-block-a">
            Name 8: <strong><xsl:value-of select="@TSB1Name8"/></strong>
          </div>
          <div class="ui-block-b">
            Label 8: <xsl:value-of select="@TSB1Label8"/>
          </div>
          <div class="ui-block-a">
            Observations 8: <xsl:value-of select="@TSB1N8"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 8: <xsl:value-of select="@TSB1TMAmount8"/>
          </div>
          <div class="ui-block-b">
            Unit 8: <xsl:value-of select="@TSB1TMUnit8"/>
          </div>
          <div class="ui-block-a">
            Mean 8: <xsl:value-of select="@TSB8Mean"/>
          </div>
          <div class="ui-block-b">
            Median 8: <xsl:value-of select="@TSB8Median"/>
          </div>
          <div class="ui-block-a">
            Variance 8: <xsl:value-of select="@TSB8Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 8: <xsl:value-of select="@TSB8StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name9) > 0)">
          <div class="ui-block-a">
            Name 9: <strong><xsl:value-of select="@TSB1Name9"/></strong>
          </div>
          <div class="ui-block-b">
            Label 9: <xsl:value-of select="@TSB1Label9"/>
          </div>
          <div class="ui-block-a">
            Observations 9: <xsl:value-of select="@TSB1N9"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 9: <xsl:value-of select="@TSB1TMAmount9"/>
          </div>
          <div class="ui-block-b">
            Unit 9: <xsl:value-of select="@TSB1TMUnit9"/>
          </div>
          <div class="ui-block-a">
            Mean 9: <xsl:value-of select="@TSB9Mean"/>
          </div>
          <div class="ui-block-b">
            Median 9: <xsl:value-of select="@TSB9Median"/>
          </div>
          <div class="ui-block-a">
            Variance 9: <xsl:value-of select="@TSB9Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 9: <xsl:value-of select="@TSB9StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TSB1Name10) > 0)">
          <div class="ui-block-a">
            Name 10: <strong><xsl:value-of select="@TSB1Name10"/></strong>
          </div>
          <div class="ui-block-b">
            Label 10: <xsl:value-of select="@TSB1Label10"/>
          </div>
          <div class="ui-block-a">
            Observations 10: <xsl:value-of select="@TSB1N10"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total 10: <xsl:value-of select="@TSB1TMAmount10"/>
          </div>
          <div class="ui-block-b">
            Unit 10: <xsl:value-of select="@TSB1TMUnit10"/>
          </div>
          <div class="ui-block-a">
            Mean 10: <xsl:value-of select="@TSB10Mean"/>
          </div>
          <div class="ui-block-b">
            Median 10: <xsl:value-of select="@TSB10Median"/>
          </div>
          <div class="ui-block-a">
            Variance 10: <xsl:value-of select="@TSB10Variance"/>
          </div>
          <div class="ui-block-b">  
            Std Dev 10: <xsl:value-of select="@TSB10StandDev"/>
          </div>
        </xsl:if>
      </div>
    </div>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
