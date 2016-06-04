<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2015, September -->
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="outcomegroup" />
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<h4>
      <strong>Outcome Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
    <h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="root/linkedview">
    <xsl:param name="localName" />
    <xsl:if test="(@SB1Name1 != '' or @TSB1Name1 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="a" >
        <h4 class="ui-bar-b">
          <strong>Indicators</strong>
        </h4>
        <xsl:if test="(@SB1ScoreUnit != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              Math Expression: <xsl:value-of select="@SB1ScoreMathExpression"/>
            </div>
            <div class="ui-block-b">

            </div>
            <div class="ui-block-a">
              Score Amount: <xsl:value-of select="@SB1Score"/>
            </div>
            <div class="ui-block-b">
              Score Unit: <xsl:value-of select="@SB1ScoreUnit"/>
            </div>
            <div class="ui-block-a">
              Score D1 Amount: <xsl:value-of select="@SB1ScoreD1Amount"/>
            </div>
            <div class="ui-block-b">
              Score D1 Unit: <xsl:value-of select="@SB1ScoreD1Unit"/>
            </div>
            <div class="ui-block-a">
              Score D2 Amount: <xsl:value-of select="@SB1ScoreD2Amount"/>
            </div>
            <div class="ui-block-b">
              Score D2 Unit: <xsl:value-of select="@SB1ScoreD2Unit"/>
            </div>
            <div class="ui-block-a">
              Distribution Type: <xsl:value-of select="@SB1ScoreDistType"/>
            </div>
            <div class="ui-block-b">
              Math Type: <xsl:value-of select="@SB1ScoreMathType"/>
            </div>
            <div class="ui-block-a">
              Score Most Amount: <xsl:value-of select="@SB1ScoreM"/>
            </div>
            <div class="ui-block-b">
              Score Most Unit: <xsl:value-of select="@SB1ScoreMUnit"/>
            </div>
            <div class="ui-block-a">
              Score Low Amount: <xsl:value-of select="@SB1ScoreLAmount"/>
            </div>
            <div class="ui-block-b">
              Score Low Unit: <xsl:value-of select="@SB1ScoreLUnit"/>
            </div>
            <div class="ui-block-a">
              Score High Amount: <xsl:value-of select="@SB1ScoreUAmount"/>
            </div>
            <div class="ui-block-b">
              Score High Unit: <xsl:value-of select="@SB1ScoreUUnit"/>
            </div>
            <div class="ui-block-a">
              Iterations: <xsl:value-of select="@SB1Iterations"/>
            </div>
            <div class="ui-block-b">
              Math Sub Type: <xsl:value-of select="@SB1ScoreMathSubType"/>
            </div>
            <div class="ui-block-a">
              Confid Int: <xsl:value-of select="@SB1CILevel"/>
            </div>
            <div class="ui-block-b">
              Random Seed: <xsl:value-of select="@SB1Random"/>
            </div>
            <div class="ui-block-a">
              Base IO: <xsl:value-of select="@SB1BaseIO"/>
            </div>
            <div class="ui-block-b">

            </div>
          </div>
          <div>
            Score Math Result: <xsl:value-of select="@SB1ScoreMathResult" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name1 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 1 Name: </strong>
              <xsl:value-of select="@SB1Name1"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label1"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date1"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel1"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType1"/>
            </div>
            <div class="ui-block-b">
              Dist Type: <xsl:value-of select="@SB1Type1"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount1"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit1"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount1"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit1"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount1"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit1"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount1"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit1"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount1"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit1"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression1"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator1"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit1"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount1"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit1"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount1"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit1"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit1"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit1"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount1"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit1"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType1"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO1"/>
            </div>
          </div>
          <div>
            Indic 1 Description: <xsl:value-of select="@SB1Description1" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name2 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 2 Name: </strong>
              <xsl:value-of select="@SB1Name2"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label2"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date2"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel2"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType2"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type2"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount2"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit2"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount2"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit2"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount2"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit2"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount2"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit2"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount2"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit2"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression2"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator2"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit2"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount2"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit2"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount2"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit2"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit2"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit2"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount2"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit2"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType2"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO2"/>
            </div>
          </div>
          <div>
            Indic 2 Description: <xsl:value-of select="@SB1Description2" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name3 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 3 Name: </strong>
              <xsl:value-of select="@SB1Name3"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label3"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date3"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel3"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType3"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type3"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount3"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit3"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount3"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit3"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount3"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit3"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount3"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit3"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount3"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit3"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression3"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator3"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit3"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount3"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit3"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount3"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit3"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit3"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit3"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount3"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit3"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType3"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO3"/>
            </div>
          </div>
          <div>
            Indic 3 Description: <xsl:value-of select="@SB1Description3" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name4 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 4 Name: </strong>
              <xsl:value-of select="@SB1Name4"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label4"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date4"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel4"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType4"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type4"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount4"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit4"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount4"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit4"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount4"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit4"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount4"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit4"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount4"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit4"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression4"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator4"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit4"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount4"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit4"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount4"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit4"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit4"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit4"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount4"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit4"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType4"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO4"/>
            </div>
          </div>
          <div>
            Indic 4 Description: <xsl:value-of select="@SB1Description4" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name5 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 5 Name: </strong>
              <xsl:value-of select="@SB1Name5"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label5"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date5"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel5"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType5"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type5"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount5"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit5"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount5"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit5"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount5"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit5"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount5"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit5"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount5"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit5"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression5"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator5"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit5"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount5"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit5"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount5"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit5"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit5"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit5"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount5"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit5"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType5"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO5"/>
            </div>
          </div>
          <div>
            Indic 5 Description: <xsl:value-of select="@SB1Description5" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name6 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 6 Name: </strong>
              <xsl:value-of select="@SB1Name6"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label6"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date6"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel6"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType6"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type6"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount6"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit6"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount6"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit6"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount6"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit6"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount6"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit6"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount6"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit6"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression6"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator6"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit6"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount6"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit6"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount6"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit6"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit6"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit6"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount6"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit6"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType6"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO6"/>
            </div>
          </div>
          <div>
            Indic 6 Description: <xsl:value-of select="@SB1Description6" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name7 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 7 Name: </strong>
              <xsl:value-of select="@SB1Name7"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label7"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date7"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel7"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType7"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type7"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount7"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit7"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount7"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit7"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount7"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit7"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount7"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit7"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount7"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit7"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression7"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator7"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit7"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount7"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit7"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount7"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit7"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit7"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit7"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount7"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit7"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType7"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO7"/>
            </div>
          </div>
          <div>
            Indic 7 Description: <xsl:value-of select="@SB1Description7" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name8 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 8 Name: </strong>
              <xsl:value-of select="@SB1Name8"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label8"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date8"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel8"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType8"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type8"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount8"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit8"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount8"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit8"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount8"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit8"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount8"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit8"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount8"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit8"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression8"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator8"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit8"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount8"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit8"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount8"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit8"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit8"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit8"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount8"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit8"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType8"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO8"/>
            </div>
          </div>
          <div>
            Indic 8 Description: <xsl:value-of select="@SB1Description8" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name9 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 9 Name: </strong>
              <xsl:value-of select="@SB1Name9"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label9"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date9"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel9"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType9"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type9"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount9"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit9"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount9"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit9"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount9"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit9"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount9"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit9"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount9"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit9"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression9"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator9"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit9"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount9"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit9"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount9"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit9"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit9"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit9"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount9"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit9"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType9"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO9"/>
            </div>
          </div>
          <div>
            Indic 9 Description: <xsl:value-of select="@SB1Description9" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name10 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 10 Name: </strong>
              <xsl:value-of select="@SB1Name10"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label10"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date10"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel10"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType10"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type10"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount10"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit10"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount10"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit10"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount10"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit10"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount10"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit10"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount10"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit10"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression10"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator10"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit10"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount10"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit10"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount10"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit10"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit10"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit10"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount10"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit10"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType10"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO10"/>
            </div>
          </div>
          <div>
            Indic 10 Description: <xsl:value-of select="@SB1Description10" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name11 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 11 Name: </strong>
              <xsl:value-of select="@SB1Name11"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label11"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date11"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel11"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType11"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type11"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount11"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit11"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount11"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit11"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount11"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit11"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount11"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit11"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount11"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit11"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression11"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator11"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit11"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount11"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit11"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount11"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit11"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit11"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit11"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount11"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit11"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType11"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO11"/>
            </div>
          </div>
          <div>
            Indic 11 Description: <xsl:value-of select="@SB1Description11" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name12 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 12 Name: </strong>
              <xsl:value-of select="@SB1Name12"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label12"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date12"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel12"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType12"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type12"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount12"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit12"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount12"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit12"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount12"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit12"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount12"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit12"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount12"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit12"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression12"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator12"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit12"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount12"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit12"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount12"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit12"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit12"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit12"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount12"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit12"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType12"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO12"/>
            </div>
          </div>
          <div>
            Indic 12 Description: <xsl:value-of select="@SB1Description12" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name13 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 13 Name: </strong>
              <xsl:value-of select="@SB1Name13"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label13"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date13"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel13"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType13"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type13"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount13"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit13"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount13"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit13"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount13"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit13"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount13"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit13"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount13"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit13"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression13"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator13"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit13"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount13"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit13"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount13"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit13"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit13"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit13"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount13"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit13"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType13"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO13"/>
            </div>
          </div>
          <div>
            Indic 13 Description: <xsl:value-of select="@SB1Description13" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name14 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 14 Name: </strong>
              <xsl:value-of select="@SB1Name14"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label14"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date14"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel14"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType14"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type14"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount14"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit14"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount14"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit14"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount14"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit14"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount14"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit14"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount14"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit14"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression14"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator14"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit14"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount14"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit14"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount14"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit14"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit14"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit14"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount14"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit14"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType14"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO14"/>
            </div>
          </div>
          <div>
            Indic 14 Description: <xsl:value-of select="@SB1Description14" />
          </div>
        </xsl:if>
        <xsl:if test="(@SB1Name15 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 15 Name: </strong>
              <xsl:value-of select="@SB1Name15"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@SB1Label15"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@SB1Date15"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@SB1RelLabel15"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@SB1MathType15"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@SB1Type15"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@SB11Amount15"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@SB11Unit15"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@SB12Amount15"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@SB12Unit15"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@SB13Amount15"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@SB13Unit15"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@SB14Amount15"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@SB14Unit15"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@SB15Amount15"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@SB15Unit15"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@SB1MathExpression15"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@SB1MathOperator15"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@SB1TAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@SB1TUnit15"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@SB1TD1Amount15"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@SB1TD1Unit15"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@SB1TD2Amount15"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@SB1TD2Unit15"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@SB1TMAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@SB1TMUnit15"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@SB1TLAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@SB1TLUnit15"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@SB1TUAmount15"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@SB1TUUnit15"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@SB1MathSubType15"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@SB1BaseIO15"/>
            </div>
          </div>
          <div>
            Indic 15 Description: <xsl:value-of select="@SB1Description15" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1ScoreUnit != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              Math Expression: <xsl:value-of select="@TSB1ScoreMathExpression"/>
            </div>
            <div class="ui-block-b">
              Observations: <xsl:value-of select="@TSB1ScoreN"/>
            </div>
            <div class="ui-block-a">
              Score Amount: <xsl:value-of select="@TSB1Score"/>
            </div>
            <div class="ui-block-b">
              Score Unit: <xsl:value-of select="@TSB1ScoreUnit"/>
            </div>
            <div class="ui-block-a">
              Score D1 Amount: <xsl:value-of select="@TSB1ScoreD1Amount"/>
            </div>
            <div class="ui-block-b">
              Score D1 Unit: <xsl:value-of select="@TSB1ScoreD1Unit"/>
            </div>
            <div class="ui-block-a">
              Score D2 Amount: <xsl:value-of select="@TSB1ScoreD2Amount"/>
            </div>
            <div class="ui-block-b">
              Score D2 Unit: <xsl:value-of select="@TSB1ScoreD2Unit"/>
            </div>
            <div class="ui-block-a">
              Distribution Type: <xsl:value-of select="@TSB1ScoreDistType"/>
            </div>
            <div class="ui-block-b">
              Math Type: <xsl:value-of select="@TSB1ScoreMathType"/>
            </div>
            <div class="ui-block-a">
              Score Most Amount: <xsl:value-of select="@TSB1ScoreM"/>
            </div>
            <div class="ui-block-b">
              Score Most Unit: <xsl:value-of select="@TSB1ScoreMUnit"/>
            </div>
            <div class="ui-block-a">
              Score Low Amount: <xsl:value-of select="@TSB1ScoreLAmount"/>
            </div>
            <div class="ui-block-b">
              Score Low Unit: <xsl:value-of select="@TSB1ScoreLUnit"/>
            </div>
            <div class="ui-block-a">
              Score High Amount: <xsl:value-of select="@TSB1ScoreUAmount"/>
            </div>
            <div class="ui-block-b">
              Score High Unit: <xsl:value-of select="@TSB1ScoreUUnit"/>
            </div>
            <div class="ui-block-a">
              Iterations: <xsl:value-of select="@TSB1Iterations"/>
            </div>
            <div class="ui-block-b">
              Score Math Sub Type: <xsl:value-of select="@TSB1ScoreMathSubType" />
            </div>
          </div>
          <div>
            Score Math Result: <xsl:value-of select="@TSB1ScoreMathResult" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name1 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 1 Name: </strong>
              <xsl:value-of select="@TSB1Name1"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label1"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date1"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel1"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType1"/>
            </div>
            <div class="ui-block-b">
              Dist Type: <xsl:value-of select="@TSB1Type1"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount1"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit1"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount1"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit1"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount1"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit1"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount1"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit1"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount1"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit1"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression1"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator1"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit1"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount1"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit1"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount1"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit1"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit1"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount1"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit1"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount1"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit1"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType1"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO1"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N1"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 1 Description: <xsl:value-of select="@TSB1Description1" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name2 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 2 Name: </strong>
              <xsl:value-of select="@TSB1Name2"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label2"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date2"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel2"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType2"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type2"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount2"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit2"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount2"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit2"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount2"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit2"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount2"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit2"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount2"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit2"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression2"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator2"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit2"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount2"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit2"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount2"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit2"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit2"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount2"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit2"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount2"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit2"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType2"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO2"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N2"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 2 Description: <xsl:value-of select="@TSB1Description2" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name3 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 3 Name: </strong>
              <xsl:value-of select="@TSB1Name3"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label3"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date3"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel3"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType3"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type3"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount3"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit3"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount3"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit3"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount3"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit3"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount3"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit3"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount3"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit3"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression3"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator3"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit3"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount3"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit3"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount3"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit3"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit3"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount3"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit3"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount3"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit3"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType3"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO3"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N3"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 3 Description: <xsl:value-of select="@TSB1Description3" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name4 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 4 Name: </strong>
              <xsl:value-of select="@TSB1Name4"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label4"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date4"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel4"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType4"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type4"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount4"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit4"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount4"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit4"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount4"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit4"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount4"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit4"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount4"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit4"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression4"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator4"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit4"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount4"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit4"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount4"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit4"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit4"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount4"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit4"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount4"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit4"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType4"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO4"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N4"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 4 Description: <xsl:value-of select="@TSB1Description4" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name5 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 5 Name: </strong>
              <xsl:value-of select="@TSB1Name5"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label5"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date5"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel5"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType5"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type5"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount5"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit5"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount5"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit5"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount5"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit5"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount5"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit5"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount5"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit5"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression5"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator5"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit5"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount5"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit5"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount5"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit5"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit5"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount5"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit5"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount5"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit5"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType5"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO5"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N5"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 5 Description: <xsl:value-of select="@TSB1Description5" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name6 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 6 Name: </strong>
              <xsl:value-of select="@TSB1Name6"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label6"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date6"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel6"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType6"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type6"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount6"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit6"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount6"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit6"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount6"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit6"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount6"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit6"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount6"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit6"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression6"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator6"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit6"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount6"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit6"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount6"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit6"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit6"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount6"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit6"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount6"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit6"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType6"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO6"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N6"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 6 Description: <xsl:value-of select="@TSB1Description6" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name7 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 7 Name: </strong>
              <xsl:value-of select="@TSB1Name7"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label7"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date7"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel7"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType7"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type7"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount7"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit7"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount7"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit7"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount7"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit7"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount7"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit7"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount7"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit7"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression7"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator7"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit7"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount7"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit7"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount7"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit7"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit7"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount7"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit7"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount7"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit7"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType7"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO7"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N7"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 7 Description: <xsl:value-of select="@TSB1Description7" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name8 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 8 Name: </strong>
              <xsl:value-of select="@TSB1Name8"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label8"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date8"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel8"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType8"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type8"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount8"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit8"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount8"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit8"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount8"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit8"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount8"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit8"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount8"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit8"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression8"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator8"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit8"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount8"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit8"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount8"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit8"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit8"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount8"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit8"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount8"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit8"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType8"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO8"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N8"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 8 Description: <xsl:value-of select="@TSB1Description8" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name9 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 9 Name: </strong>
              <xsl:value-of select="@TSB1Name9"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label9"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date9"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel9"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType9"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type9"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount9"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit9"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount9"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit9"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount9"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit9"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount9"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit9"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount9"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit9"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression9"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator9"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit9"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount9"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit9"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount9"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit9"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit9"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount9"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit9"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount9"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit9"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType9"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO9"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N9"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 9 Description: <xsl:value-of select="@TSB1Description9" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name10 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 10 Name: </strong>
              <xsl:value-of select="@TSB1Name10"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label10"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date10"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel10"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType10"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type10"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount10"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit10"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount10"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit10"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount10"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit10"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount10"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit10"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount10"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit10"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression10"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator10"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit10"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount10"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit10"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount10"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit10"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit10"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount10"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit10"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount10"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit10"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType10"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO10"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N10"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 10 Description: <xsl:value-of select="@TSB1Description10" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name11 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 11 Name: </strong>
              <xsl:value-of select="@TSB1Name11"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label11"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date11"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel11"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType11"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type11"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount11"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit11"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount11"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit11"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount11"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit11"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount11"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit11"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount11"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit11"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression11"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator11"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit11"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount11"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit11"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount11"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit11"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit11"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount11"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit11"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount11"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit11"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType11"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO11"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N11"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 11 Description: <xsl:value-of select="@TSB1Description11" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name12 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 12 Name: </strong>
              <xsl:value-of select="@TSB1Name12"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label12"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date12"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel12"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType12"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type12"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount12"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit12"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount12"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit12"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount12"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit12"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount12"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit12"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount12"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit12"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression12"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator12"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit12"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount12"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit12"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount12"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit12"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit12"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount12"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit12"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount12"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit12"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType12"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO12"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N12"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 12 Description: <xsl:value-of select="@TSB1Description12" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name13 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 13 Name: </strong>
              <xsl:value-of select="@TSB1Name13"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label13"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date13"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel13"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType13"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type13"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount13"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit13"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount13"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit13"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount13"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit13"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount13"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit13"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount13"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit13"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression13"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator13"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit13"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount13"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit13"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount13"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit13"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit13"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount13"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit13"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount13"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit13"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType13"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO13"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N13"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 13 Description: <xsl:value-of select="@TSB1Description13" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name14 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 14 Name: </strong>
              <xsl:value-of select="@TSB1Name14"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label14"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date14"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel14"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType14"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type14"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount14"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit14"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount14"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit14"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount14"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit14"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount14"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit14"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount14"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit14"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression14"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator14"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit14"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount14"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit14"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount14"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit14"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit14"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount14"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit14"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount14"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit14"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType14"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO14"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N14"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 14 Description: <xsl:value-of select="@TSB1Description14" />
          </div>
        </xsl:if>
        <xsl:if test="(@TSB1Name15 != '')">
          <div class="ui-grid-a">
            <div class="ui-block-a">
              <strong>Indic 15 Name: </strong>
              <xsl:value-of select="@TSB1Name15"/>
            </div>
            <div class="ui-block-b">
              Label:  <xsl:value-of select="@TSB1Label15"/>
            </div>
            <div class="ui-block-a">
              Date: <xsl:value-of select="@TSB1Date15"/>
            </div>
            <div class="ui-block-b">
              Rel Label:  <xsl:value-of select="@TSB1RelLabel15"/>
            </div>
            <div class="ui-block-a">
              Math Type:  <xsl:value-of select="@TSB1MathType15"/>
            </div>
            <div class="ui-block-b">
              Type: <xsl:value-of select="@TSB1Type15"/>
            </div>
            <div class="ui-block-a">
              Q1 Amount:  <xsl:value-of select="@TSB11Amount15"/>
            </div>
            <div class="ui-block-b">
              Q1 Unit: <xsl:value-of select="@TSB11Unit15"/>
            </div>
            <div class="ui-block-a">
              Q2 Amount:  <xsl:value-of select="@TSB12Amount15"/>
            </div>
            <div class="ui-block-b">
              Q2 Unit: <xsl:value-of select="@TSB12Unit15"/>
            </div>
            <div class="ui-block-a">
              Q3 Amount:  <xsl:value-of select="@TSB13Amount15"/>
            </div>
            <div class="ui-block-b">
              Q3 Unit: <xsl:value-of select="@TSB13Unit15"/>
            </div>
            <div class="ui-block-a">
              Q4 Amount:  <xsl:value-of select="@TSB14Amount15"/>
            </div>
            <div class="ui-block-b">
              Q4 Unit: <xsl:value-of select="@TSB14Unit15"/>
            </div>
            <div class="ui-block-a">
              Q5 Amount: <xsl:value-of select="@TSB15Amount15"/>
            </div>
            <div class="ui-block-b">
              Q5 Unit: <xsl:value-of select="@TSB15Unit15"/>
            </div>
            <div class="ui-block-a">
              Math Express: <xsl:value-of select="@TSB1MathExpression15"/>
            </div>
            <div class="ui-block-b">
              Math Operator: <xsl:value-of select="@TSB1MathOperator15"/>
            </div>
            <div class="ui-block-a">
              QT Amount: <xsl:value-of select="@TSB1TAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Unit: <xsl:value-of select="@TSB1TUnit15"/>
            </div>
            <div class="ui-block-a">
              QT D1 Amount: <xsl:value-of select="@TSB1TD1Amount15"/>
            </div>
            <div class="ui-block-b">
              QT D1 Unit: <xsl:value-of select="@TSB1TD1Unit15"/>
            </div>
            <div class="ui-block-a">
              QT D2 Amount: <xsl:value-of select="@TSB1TD2Amount15"/>
            </div>
            <div class="ui-block-b">
              QT D2 Unit: <xsl:value-of select="@TSB1TD2Unit15"/>
            </div>
            <div class="ui-block-a">
              QT Most Amount: <xsl:value-of select="@TSB1TMAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Most Unit: <xsl:value-of select="@TSB1TMUnit15"/>
            </div>
            <div class="ui-block-a">
              QT Low Amount: <xsl:value-of select="@TSB1TLAmount15"/>
            </div>
            <div class="ui-block-b">
              QT Low Unit: <xsl:value-of select="@TSB1TLUnit15"/>
            </div>
            <div class="ui-block-a">
              QT High Amount: <xsl:value-of select="@TSB1TUAmount15"/>
            </div>
            <div class="ui-block-b">
              QT High Unit: <xsl:value-of select="@TSB1TUUnit15"/>
            </div>
            <div class="ui-block-a">
              Math Sub Type: <xsl:value-of select="@TSB1MathSubType15"/>
            </div>
            <div class="ui-block-b">
              Base IO: <xsl:value-of select="@TSB1BaseIO15"/>
            </div>
            <div class="ui-block-a">
              Observations: <xsl:value-of select="@TSB1N15"/>
            </div>
            <div class="ui-block-b">
            </div>
          </div>
          <div>
            Indic 15 Description: <xsl:value-of select="@TSB1Description15" />
          </div>
        </xsl:if>
      </div>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>