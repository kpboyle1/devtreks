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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
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
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="root/linkedview">
    <xsl:param name="localName" />
    <xsl:if test="(@SB1Name1 != '' or @TSB1Name1 != '')">
      <tr>
        <th>
          Score
        </th>
        <th>
          Score Unit
        </th>
        <th>
          Score D1 Amount
        </th>
        <th>
          Score D1 Unit
        </th>
        <th>
          Score D2 Amount
        </th>
        <th>
          Score D2 Unit
        </th>
        <th>
          Iterations
        </th>
        <th>
          Confid Int
        </th>
        <th>
          Random Seed
        </th>
        <th>
          Base IO
        </th>
      </tr>
      <tr>
        <th>
          Score Most Amount
        </th>
        <th>
          Score Most Unit
        </th>
        <th>
          Score Low Amount
        </th>
        <th>
          Score Low Unit
        </th>
        <th>
          Score High Amount
        </th>
        <th>
          Score High Unit
        </th>
        <th>
          Distribution Type
        </th>
        <th>
          Math Type
        </th>
        <th>
          Math Sub Type
        </th>
        <th>
          Observations
        </th>
      </tr>
      <xsl:if test="(@SB1ScoreUnit != '')">
        <tr>
          <td>
            <xsl:value-of select="@SB1Score" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreUnit" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreD1Amount"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreD1Unit" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreD2Amount"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreD2Unit"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Iterations"/>
          </td>
          <td>
            <xsl:value-of select="@SB1CILevel"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Random"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1ScoreM" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreMUnit" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreLAmount"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreLUnit" />
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreUAmount"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreUUnit"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreDistType"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreMathType"/>
          </td>
          <td>
            <xsl:value-of select="@SB1ScoreMathSubType" />
          </td>
          <td>
            1
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1ScoreMathExpression"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1ScoreMathResult" />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1ScoreUnit != '')">
        <tr>
          <td>
            <xsl:value-of select="@TSB1Score" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreUnit" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreD1Amount"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreD1Unit" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreD2Amount"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreD2Unit"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Iterations"/>
          </td>
          <td colspan="3">

          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1ScoreM" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreMUnit" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreLAmount"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreLUnit" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreUAmount"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreUUnit"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreDistType"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreMathType"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreMathSubType" />
          </td>
          <td>
            <xsl:value-of select="@TSB1ScoreN" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1ScoreMathExpression"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1ScoreMathResult" />
          </td>
        </tr>
      </xsl:if>
      <tr>
        <th colspan="2">
          Name (N)
        </th>
        <th>
          Label
        </th>
        <th>
          Date
        </th>
        <th>
          Rel Label
        </th>
        <th>
          Math Type
        </th>
        <th>
          Dist Type
        </th>
        <th>
          Base IO
        </th>
        <th>
          Math Operator
        </th>
        <th>
          Math Sub Type
        </th>
      </tr>
      <tr>
        <th>
          Q1 Amount
        </th>
        <th>
          Q1 Unit
        </th>
        <th>
          Q2 Amount
        </th>
        <th>
          Q2 Unit
        </th>
        <th>
          Q3 Amount
        </th>
        <th>
          Q3 Unit
        </th>
        <th>
          Q4 Amount
        </th>
        <th>
          Q4 Unit
        </th>
        <th>
          Q5 Amount
        </th>
        <th>
          Q5 Unit
        </th>
      </tr>
      <tr>
        <th>
          QT Amount
        </th>
        <th>
          QT Unit
        </th>
        <th>
          QT D1 Amount
        </th>
        <th>
          QT D2 Amount
        </th>
        <th>
          QT Most Amount
        </th>
        <th>
          QT Most Unit
        </th>
        <th>
          QT Low Amount
        </th>
        <th>
          QT Low Unit
        </th>
        <th>
          QT High Amount
        </th>
        <th>
          QT High Unit
        </th>
      </tr>
      <xsl:if test="(@SB1Name1 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name1" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label1" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel1" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType1"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit1"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit1"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description1" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression1"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name2 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name2" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label2" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel2" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType2"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit2"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit2"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description2" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression2"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name3 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name3" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label3" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel3" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType3"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit3"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit3"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description3" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression3"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name4 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name4" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label4" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel4" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType4"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit4"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit4"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description4" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression4"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name5 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name5" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label5" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel5" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType5"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit5"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit5"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description5" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression5"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name6 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name6" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label6" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel6" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType6"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit6"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit6"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description6" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression6"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name7 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name7" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label7" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel7" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType7"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit7"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit7"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description7" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression7"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name8 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name8" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label8" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel8" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType8"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit8"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit8"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description8" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression8"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name9 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name9" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label9" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel9" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType9"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit9"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit9"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description9" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression9"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name10 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name10" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label10" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel10" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType10"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit10"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit10"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description10" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression10"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name11 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name11" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label11" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel11" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType11"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit11"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit11"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description11" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression11"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name12 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name12" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label12" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel12" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType12"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit12"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit12"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description12" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression12"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name13 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name13" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label13" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel13" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType13"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit13"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit13"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description13" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression13"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name14 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name14" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label14" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel14" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType14"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit14"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit14"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description14" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression14"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@SB1Name15 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@SB1Name15" />
            </strong>
          </td>
          <td>
            <xsl:value-of select="@SB1Label15" />
          </td>
          <td>
            <xsl:value-of select="@SB1Date15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1RelLabel15" />
          </td>
          <td>
            <xsl:value-of select="@SB1MathType15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1Type15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1BaseIO15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathOperator15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1MathSubType15"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB11Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB11Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB12Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB13Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB14Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB15Unit15"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@SB1TAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD1Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TD2Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TMUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TLUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@SB1TUUnit15"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1Description15" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SB1MathExpression15"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name1 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name1" />
            </strong> (<xsl:value-of select="@TSB1N1" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label1" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel1" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1N1" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType1"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit1"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount1"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit1"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description1" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression1"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name2 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name2" />
            </strong>  (<xsl:value-of select="@TSB1N2" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label2" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel2" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType2"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit2"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount2"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit2"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description2" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression2"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name3 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name3" />
            </strong>  (<xsl:value-of select="@TSB1N3" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label3" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel3" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType3"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit3"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount3"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit3"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description3" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression3"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name4 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name4" />
            </strong>  (<xsl:value-of select="@TSB1N4" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label4" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel4" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType4"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit4"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount4"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit4"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description4" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression4"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name5 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name5" />
            </strong>  (<xsl:value-of select="@TSB1N5" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label5" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel5" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType5"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit5"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount5"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit5"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description5" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression5"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name6 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name6" />
            </strong>  (<xsl:value-of select="@TSB1N6" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label6" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel6" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType6"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit6"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount6"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit6"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description6" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression6"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name7 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name7" />
            </strong>  (<xsl:value-of select="@TSB1N7" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label7" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel7" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType7"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit7"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount7"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit7"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description7" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression7"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name8 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name8" />
            </strong>  (<xsl:value-of select="@TSB1N8" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label8" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel8" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType8"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit8"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount8"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit8"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description8" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression8"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name9 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name9" />
            </strong>  (<xsl:value-of select="@TSB1N9" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label9" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel9" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType9"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit9"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount9"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit9"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description9" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression9"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name10 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name10" />
            </strong>  (<xsl:value-of select="@TSB1N10" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label10" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel10" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType10"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit10"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount10"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit10"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description10" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression10"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name11 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name11" />
            </strong>  (<xsl:value-of select="@TSB1N11" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label11" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel11" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType11"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit11"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount11"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit11"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description11" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression11"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name12 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name12" />
            </strong>  (<xsl:value-of select="@TSB1N12" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label12" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel12" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType12"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit12"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount12"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit12"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description12" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression12"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name13 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name13" />
            </strong>  (<xsl:value-of select="@TSB1N13" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label13" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel13" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType13"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit13"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount13"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit13"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description13" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression13"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name14 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name14" />
            </strong>  (<xsl:value-of select="@TSB1N14" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label14" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel14" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType14"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit14"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount14"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit14"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description14" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression14"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSB1Name15 != '')">
        <tr>
          <td colspan="2">
            <strong>
              <xsl:value-of select="@TSB1Name15" />
            </strong> (<xsl:value-of select="@TSB1N15" />)
          </td>
          <td>
            <xsl:value-of select="@TSB1Label15" />
          </td>
          <td>
            <xsl:value-of select="@TSB1Date15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1RelLabel15" />
          </td>
          <td>
            <xsl:value-of select="@TSB1MathType15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1Type15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1BaseIO15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathOperator15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1MathSubType15"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB11Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB11Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB12Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB13Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB14Unit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB15Unit15"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@TSB1TAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD1Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TD2Amount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TMUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TLUnit15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUAmount15"/>
          </td>
          <td>
            <xsl:value-of select="@TSB1TUUnit15"/>
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1Description15" />
          </td>
        </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSB1MathExpression15"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>