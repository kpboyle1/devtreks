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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		<h4>
      <strong>Operation Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <h4>
      <strong>Operation </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operationinput">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationinput">
    <h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbchangeyr' 
      or @AnalyzerType='sbchangeid' or @AnalyzerType='sbchangealt']">
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
            Alternative : <xsl:value-of select="@AlternativeType"/> 
          </div>
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
              Score Amount C: <xsl:value-of select="@TSBScoreAmountChange"/>
            </div>
            <div class="ui-block-b">
              Score Percent C: <xsl:value-of select="@TSBScorePercentChange"/>
            </div>
            <div class="ui-block-a">
              Score Base C: <xsl:value-of select="@TSBScoreBaseChange"/>
            </div>
            <div class="ui-block-b">  
              Score Base Percent C: <xsl:value-of select="@TSBScoreBasePercentChange"/>
            </div>
            <div class="ui-block-a">
              Score Low: <xsl:value-of select="@TSB1ScoreLAmount"/>
            </div>
            <div class="ui-block-b">
              Unit: <xsl:value-of select="@TSB1ScoreLUnit"/>
            </div>
            <div class="ui-block-a">
              Score Low Amount C: <xsl:value-of select="@TSBScoreLAmountChange"/>
            </div>
            <div class="ui-block-b">
              Score Low Percent C: <xsl:value-of select="@TSBScoreLPercentChange"/>
            </div>
            <div class="ui-block-a">
              Score Low Base C: <xsl:value-of select="@TSBScoreLBaseChange"/>
            </div>
            <div class="ui-block-b">  
              Score Low Base Percent C: <xsl:value-of select="@TSBScoreLBasePercentChange"/>
            </div>
            <div class="ui-block-a">
              Score High: <xsl:value-of select="@TSB1ScoreUAmount"/>
            </div>
            <div class="ui-block-b">
              Unit: <xsl:value-of select="@TSB1ScoreUUnit"/>
            </div>
            <div class="ui-block-a">
              Score High Amount C: <xsl:value-of select="@TSBScoreUAmountChange"/>
            </div>
            <div class="ui-block-b">
              Score High Percent C: <xsl:value-of select="@TSBScoreUPercentChange"/>
            </div>
            <div class="ui-block-a">
              Score High Base C: <xsl:value-of select="@TSBScoreUBaseChange"/>
            </div>
            <div class="ui-block-b">  
              Score High Base Percent C: <xsl:value-of select="@TSBScoreUBasePercentChange"/>
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
              Total 1: <xsl:value-of select="@TSB1TMAmount1"/>
            </div>
            <div class="ui-block-b">
              Unit 1: <xsl:value-of select="@TSB1TMUnit1"/>
            </div>
            <div class="ui-block-a">
              Amount Change 1: <xsl:value-of select="@TSB1AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 1: <xsl:value-of select="@TSB1PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 1: <xsl:value-of select="@TSB1BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 1: <xsl:value-of select="@TSB1BasePercentChange"/>
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
              Total 2: <xsl:value-of select="@TSB1TMAmount2"/>
            </div>
            <div class="ui-block-b">
              Unit 2: <xsl:value-of select="@TSB1TMUnit2"/>
            </div>
            <div class="ui-block-a">
              Amount Change 2: <xsl:value-of select="@TSB2AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 2: <xsl:value-of select="@TSB2PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 2: <xsl:value-of select="@TSB2BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 2: <xsl:value-of select="@TSB2BasePercentChange"/>
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
              Total 3: <xsl:value-of select="@TSB1TMAmount3"/>
            </div>
            <div class="ui-block-b">
              Unit 3: <xsl:value-of select="@TSB1TMUnit3"/>
            </div>
            <div class="ui-block-a">
              Amount Change 3: <xsl:value-of select="@TSB3AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 3: <xsl:value-of select="@TSB3PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 3: <xsl:value-of select="@TSB3BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 3: <xsl:value-of select="@TSB3BasePercentChange"/>
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
              Total 4: <xsl:value-of select="@TSB1TMAmount4"/>
            </div>
            <div class="ui-block-b">
              Unit 4: <xsl:value-of select="@TSB1TMUnit4"/>
            </div>
            <div class="ui-block-a">
              Amount Change 4: <xsl:value-of select="@TSB4AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 4: <xsl:value-of select="@TSB4PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 4: <xsl:value-of select="@TSB4BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 4: <xsl:value-of select="@TSB4BasePercentChange"/>
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
              Total 5: <xsl:value-of select="@TSB1TMAmount5"/>
            </div>
            <div class="ui-block-b">
              Unit 5: <xsl:value-of select="@TSB1TMUnit5"/>
            </div>
            <div class="ui-block-a">
              Amount Change 5: <xsl:value-of select="@TSB5AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 5: <xsl:value-of select="@TSB5PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 5: <xsl:value-of select="@TSB5BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 5: <xsl:value-of select="@TSB5BasePercentChange"/>
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
              Total 6: <xsl:value-of select="@TSB1TMAmount6"/>
            </div>
            <div class="ui-block-b">
              Unit 6: <xsl:value-of select="@TSB1TMUnit6"/>
            </div>
            <div class="ui-block-a">
              Amount Change 6: <xsl:value-of select="@TSB6AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 6: <xsl:value-of select="@TSB6PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 6: <xsl:value-of select="@TSB6BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 6: <xsl:value-of select="@TSB6BasePercentChange"/>
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
              Total 7: <xsl:value-of select="@TSB1TMAmount7"/>
            </div>
            <div class="ui-block-b">
              Unit 7: <xsl:value-of select="@TSB1TMUnit7"/>
            </div>
            <div class="ui-block-a">
              Amount Change 7: <xsl:value-of select="@TSB7AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 7: <xsl:value-of select="@TSB7PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 7: <xsl:value-of select="@TSB7BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 7: <xsl:value-of select="@TSB7BasePercentChange"/>
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
              Total 8: <xsl:value-of select="@TSB1TMAmount8"/>
            </div>
            <div class="ui-block-b">
              Unit 8: <xsl:value-of select="@TSB1TMUnit8"/>
            </div>
            <div class="ui-block-a">
              Amount Change 8: <xsl:value-of select="@TSB8AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 8: <xsl:value-of select="@TSB8PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 8: <xsl:value-of select="@TSB8BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 8: <xsl:value-of select="@TSB8BasePercentChange"/>
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
              Total 9: <xsl:value-of select="@TSB1TMAmount9"/>
            </div>
            <div class="ui-block-b">
              Unit 9: <xsl:value-of select="@TSB1TMUnit9"/>
            </div>
            <div class="ui-block-a">
              Amount Change 9: <xsl:value-of select="@TSB9AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 9: <xsl:value-of select="@TSB9PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 9: <xsl:value-of select="@TSB9BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 9: <xsl:value-of select="@TSB9BasePercentChange"/>
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
              Total 10: <xsl:value-of select="@TSB1TMAmount10"/>
            </div>
            <div class="ui-block-b">
              Unit 10: <xsl:value-of select="@TSB1TMUnit10"/>
            </div>
            <div class="ui-block-a">
              Amount Change 10: <xsl:value-of select="@TSB10AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change 10: <xsl:value-of select="@TSB10PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change 10: <xsl:value-of select="@TSB10BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change 10: <xsl:value-of select="@TSB10BasePercentChange"/>
            </div>
          </xsl:if>
        </div>
      </div>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
