<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
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
			<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
		<h4>
      <strong>Investment Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@PracticeName" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="investmentoutcomes" />
    <div>
      <strong>Components</strong>
    </div>
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
    <h4>
      <strong>Component</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicators Details</strong>
      </h4>
      <xsl:if test="(@IndName0 != '' and @IndName0 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 0 Name: </strong><xsl:value-of select="@IndName0"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel0"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate0"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel0"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType0"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType0"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType0"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO0"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression0"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator0"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit0"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount0"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit0"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount0"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit0"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit0"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit0"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount0"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit0"/>
          </div>
        </div>
        <div>
			    Score Math Result: <xsl:value-of select="@IndMathResult0" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName1 != '' and @IndName1 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 1 Name: </strong><xsl:value-of select="@IndName1"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel1"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate1"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel1"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType1"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType1"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType1"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO1"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount1"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit1"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount1"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit1"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount1"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit1"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount1"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit1"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount1"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit1"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression1"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator1"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit1"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount1"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit1"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount1"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit1"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit1"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit1"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount1"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit1"/>
          </div>
        </div>
        <div>
			    Indic 1 Description: <xsl:value-of select="@IndDescription1" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName2 != '' and @IndName2 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 2 Name: </strong><xsl:value-of select="@IndName2"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel2"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate2"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel2"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType2"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType2"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType2"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO2"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount2"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit2"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount2"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit2"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount2"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit2"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount2"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit2"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount2"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit2"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression2"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator2"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit2"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount2"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit2"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount2"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit2"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit2"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit2"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount2"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit2"/>
          </div>
        </div>
        <div>
			    Indic 2 Description: <xsl:value-of select="@IndDescription2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName3 != '' and @IndName3 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 3 Name: </strong><xsl:value-of select="@IndName3"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel3"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate3"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel3"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType3"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType3"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType3"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO3"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount3"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit3"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount3"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit3"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount3"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit3"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount3"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit3"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount3"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit3"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression3"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator3"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit3"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount3"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit3"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount3"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit3"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit3"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit3"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount3"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit3"/>
          </div>
        </div>
        <div>
			    Indic 3 Description: <xsl:value-of select="@IndDescription3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName4 != '' and @IndName4 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 4 Name: </strong><xsl:value-of select="@IndName4"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel4"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate4"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel4"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType4"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType4"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType4"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO4"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount4"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit4"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount4"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit4"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount4"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit4"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount4"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit4"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount4"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit4"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression4"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator4"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit4"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount4"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit4"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount4"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit4"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit4"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit4"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount4"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit4"/>
          </div>
        </div>
        <div>
			    Indic 4 Description: <xsl:value-of select="@IndDescription4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName5 != '' and @IndName5 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 5 Name: </strong><xsl:value-of select="@IndName5"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel5"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate5"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel5"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType5"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType5"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType5"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO5"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount5"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit5"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount5"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit5"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount5"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit5"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount5"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit5"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount5"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit5"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression5"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator5"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit5"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount5"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit5"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount5"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit5"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit5"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit5"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount5"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit5"/>
          </div>
        </div>
        <div>
			    Indic 5 Description: <xsl:value-of select="@IndDescription5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName6 != '' and @IndName6 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 6 Name: </strong><xsl:value-of select="@IndName6"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel6"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate6"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel6"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType6"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType6"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType6"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO6"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount6"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit6"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount6"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit6"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount6"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit6"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount6"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit6"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount6"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit6"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression6"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator6"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit6"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount6"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit6"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount6"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit6"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit6"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit6"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount6"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit6"/>
          </div>
        </div>
        <div>
			    Indic 6 Description: <xsl:value-of select="@IndDescription6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName7 != '' and @IndName7 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 7 Name: </strong><xsl:value-of select="@IndName7"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel7"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate7"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel7"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType7"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType7"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType7"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO7"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount7"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit7"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount7"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit7"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount7"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit7"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount7"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit7"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount7"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit7"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression7"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator7"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit7"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount7"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit7"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount7"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit7"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit7"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit7"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount7"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit7"/>
          </div>
        </div>
        <div>
			    Indic 7 Description: <xsl:value-of select="@IndDescription7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName8 != '' and @IndName8 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 8 Name: </strong><xsl:value-of select="@IndName8"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel8"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate8"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel8"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType8"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType8"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType8"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO8"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount8"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit8"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount8"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit8"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount8"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit8"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount8"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit8"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount8"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit8"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression8"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator8"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit8"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount8"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit8"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount8"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit8"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit8"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit8"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount8"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit8"/>
          </div>
        </div>
        <div>
			    Indic 8 Description: <xsl:value-of select="@IndDescription8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName9 != '' and @IndName9 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 9 Name: </strong><xsl:value-of select="@IndName9"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel9"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate9"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel9"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType9"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType9"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType9"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO9"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount9"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit9"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount9"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit9"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount9"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit9"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount9"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit9"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount9"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit9"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression9"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator9"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit9"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount9"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit9"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount9"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit9"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit9"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit9"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount9"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit9"/>
          </div>
        </div>
        <div>
			    Indic 9 Description: <xsl:value-of select="@IndDescription9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName10 != '' and @IndName10 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 10 Name: </strong><xsl:value-of select="@IndName10"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel10"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate10"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel10"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType10"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType10"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType10"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO10"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount10"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit10"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount10"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit10"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount10"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit10"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount10"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit10"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount10"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit10"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression10"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator10"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit10"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount10"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit10"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount10"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit10"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit10"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit10"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount10"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit10"/>
          </div>
        </div>
        <div>
			    Indic 10 Description: <xsl:value-of select="@IndDescription10" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName11 != '' and @IndName11 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 11 Name: </strong><xsl:value-of select="@IndName11"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel11"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate11"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel11"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType11"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType11"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType11"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO11"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount11"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit11"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount11"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit11"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount11"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit11"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount11"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit11"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount11"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit11"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression11"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator11"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit11"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount11"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit11"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount11"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit11"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit11"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit11"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount11"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit11"/>
          </div>
        </div>
        <div>
			    Indic 11 Description: <xsl:value-of select="@IndDescription11" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName12 != '' and @IndName12 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 12 Name: </strong><xsl:value-of select="@IndName12"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel12"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate12"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel12"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType12"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType12"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType12"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO12"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount12"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit12"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount12"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit12"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount12"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit12"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount12"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit12"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount12"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit12"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression12"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator12"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit12"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount12"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit12"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount12"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit12"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit12"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit12"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount12"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit12"/>
          </div>
        </div>
        <div>
			    Indic 12 Description: <xsl:value-of select="@IndDescription12" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName13 != '' and @IndName13 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 13 Name: </strong><xsl:value-of select="@IndName13"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel13"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate13"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel13"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType13"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType13"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType13"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO13"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount13"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit13"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount13"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit13"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount13"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit13"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount13"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit13"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount13"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit13"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression13"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator13"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit13"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount13"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit13"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount13"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit13"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit13"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit13"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount13"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit13"/>
          </div>
        </div>
        <div>
			    Indic 13 Description: <xsl:value-of select="@IndDescription13" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName14 != '' and @IndName14 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 14 Name: </strong><xsl:value-of select="@IndName14"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel14"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate14"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel14"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType14"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType14"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType14"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO14"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount14"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit14"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount14"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit14"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount14"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit14"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount14"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit14"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount14"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit14"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression14"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator14"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit14"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount14"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit14"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount14"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit14"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit14"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit14"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount14"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit14"/>
          </div>
        </div>
        <div>
			    Indic 14 Description: <xsl:value-of select="@IndDescription14" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName15 != '' and @IndName15 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 15 Name: </strong><xsl:value-of select="@IndName15"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel15"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@IndDate15"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@IndRelLabel15"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType15"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@IndType15"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@IndMathSubType15"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@IndBaseIO15"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount15"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit15"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount15"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit15"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@Ind3Amount15"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@Ind3Unit15"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@Ind4Amount15"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@Ind4Unit15"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@Ind5Amount15"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@Ind5Unit15"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@IndMathExpression15"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@IndMathOperator15"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@IndTAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@IndTUnit15"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@IndTD1Amount15"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@IndTD1Unit15"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@IndTD2Amount15"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@IndTD2Unit15"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@IndTMAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@IndTMUnit15"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@IndTLAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@IndTLUnit15"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@IndTUAmount15"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@IndTUUnit15"/>
          </div>
        </div>
        <div>
			    Indic 15 Description: <xsl:value-of select="@IndDescription15" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Stage != '' and @TME2Stage != 'none')">
        <div>
			    M and E Stage: <strong><xsl:value-of select="@TME2Stage"/></strong>
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 0 Name: </strong><xsl:value-of select="@TME2Name0"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label0"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date0"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel0"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType0"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type0"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType0"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO0"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression0"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator0"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit0"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount0"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit0"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount0"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit0"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit0"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount0"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit0"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount0"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit0"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N0"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 0 Description: <xsl:value-of select="@TME2Description0" />
	      </div>
        <div>
			    Score Math Result: <xsl:value-of select="@TME2MathResult0" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 1 Name: </strong><xsl:value-of select="@TME2Name1"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label1"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date1"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel1"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType1"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type1"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType1"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO1"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount1"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit1"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount1"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit1"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount1"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit1"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount1"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit1"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount1"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit1"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression1"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator1"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit1"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount1"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit1"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount1"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit1"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit1"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount1"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit1"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount1"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit1"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N1"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 1 Description: <xsl:value-of select="@TME2Description1" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 2 Name: </strong><xsl:value-of select="@TME2Name2"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label2"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date2"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel2"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType2"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type2"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType2"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO2"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount2"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit2"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount2"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit2"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount2"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit2"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount2"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit2"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount2"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit2"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression2"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator2"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit2"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount2"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit2"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount2"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit2"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit2"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount2"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit2"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount2"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit2"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N2"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 2 Description: <xsl:value-of select="@TME2Description2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 3 Name: </strong><xsl:value-of select="@TME2Name3"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label3"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date3"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel3"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType3"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type3"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType3"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO3"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount3"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit3"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount3"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit3"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount3"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit3"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount3"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit3"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount3"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit3"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression3"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator3"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit3"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount3"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit3"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount3"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit3"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit3"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount3"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit3"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount3"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit3"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N3"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 3 Description: <xsl:value-of select="@TME2Description3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 4 Name: </strong><xsl:value-of select="@TME2Name4"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label4"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date4"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel4"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType4"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type4"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType4"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO4"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount4"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit4"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount4"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit4"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount4"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit4"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount4"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit4"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount4"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit4"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression4"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator4"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit4"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount4"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit4"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount4"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit4"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit4"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount4"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit4"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount4"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit4"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N4"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 4 Description: <xsl:value-of select="@TME2Description4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 5 Name: </strong><xsl:value-of select="@TME2Name5"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label5"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date5"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel5"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType5"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type5"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType5"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO5"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount5"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit5"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount5"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit5"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount5"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit5"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount5"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit5"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount5"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit5"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression5"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator5"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit5"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount5"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit5"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount5"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit5"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit5"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount5"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit5"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount5"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit5"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N5"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 5 Description: <xsl:value-of select="@TME2Description5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 6 Name: </strong><xsl:value-of select="@TME2Name6"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label6"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date6"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel6"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType6"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type6"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType6"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO6"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount6"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit6"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount6"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit6"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount6"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit6"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount6"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit6"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount6"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit6"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression6"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator6"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit6"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount6"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit6"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount6"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit6"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit6"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount6"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit6"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount6"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit6"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N6"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 6 Description: <xsl:value-of select="@TME2Description6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 7 Name: </strong><xsl:value-of select="@TME2Name7"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label7"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date7"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel7"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType7"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type7"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType7"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO7"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount7"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit7"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount7"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit7"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount7"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit7"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount7"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit7"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount7"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit7"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression7"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator7"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit7"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount7"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit7"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount7"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit7"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit7"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount7"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit7"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount7"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit7"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N7"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 7 Description: <xsl:value-of select="@TME2Description7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 8 Name: </strong><xsl:value-of select="@TME2Name8"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label8"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date8"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel8"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType8"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type8"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType8"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO8"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount8"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit8"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount8"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit8"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount8"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit8"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount8"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit8"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount8"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit8"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression8"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator8"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit8"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount8"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit8"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount8"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit8"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit8"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount8"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit8"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount8"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit8"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N8"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 8 Description: <xsl:value-of select="@TME2Description8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 9 Name: </strong><xsl:value-of select="@TME2Name9"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label9"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date9"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel9"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType9"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type9"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType9"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO9"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount9"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit9"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount9"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit9"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount9"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit9"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount9"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit9"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount9"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit9"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression9"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator9"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit9"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount9"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit9"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount9"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit9"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit9"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount9"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit9"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount9"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit9"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N9"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 9 Description: <xsl:value-of select="@TME2Description9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 10 Name: </strong><xsl:value-of select="@TME2Name10"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label10"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date10"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel10"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType10"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type10"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType10"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO10"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount10"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit10"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount10"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit10"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount10"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit10"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount10"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit10"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount10"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit10"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression10"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator10"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit10"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount10"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit10"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount10"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit10"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit10"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount10"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit10"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount10"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit10"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N10"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 10 Description: <xsl:value-of select="@TME2Description10" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 11 Name: </strong><xsl:value-of select="@TME2Name11"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label11"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date11"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel11"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType11"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type11"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType11"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO11"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount11"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit11"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount11"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit11"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount11"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit11"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount11"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit11"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount11"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit11"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression11"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator11"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit11"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount11"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit11"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount11"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit11"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit11"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount11"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit11"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount11"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit11"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N11"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 11 Description: <xsl:value-of select="@TME2Description11" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 12 Name: </strong><xsl:value-of select="@TME2Name12"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label12"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date12"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel12"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType12"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type12"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType12"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO12"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount12"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit12"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount12"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit12"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount12"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit12"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount12"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit12"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount12"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit12"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression12"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator12"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit12"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount12"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit12"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount12"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit12"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit12"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount12"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit12"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount12"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit12"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N12"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 12 Description: <xsl:value-of select="@TME2Description12" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 13 Name: </strong><xsl:value-of select="@TME2Name13"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label13"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date13"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel13"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType13"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type13"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType13"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO13"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount13"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit13"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount13"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit13"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount13"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit13"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount13"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit13"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount13"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit13"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression13"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator13"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit13"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount13"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit13"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount13"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit13"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit13"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount13"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit13"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount13"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit13"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N13"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 13 Description: <xsl:value-of select="@TME2Description13" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 14 Name: </strong><xsl:value-of select="@TME2Name14"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label14"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date14"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel14"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType14"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type14"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType14"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO14"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount14"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit14"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount14"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit14"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount14"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit14"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount14"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit14"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount14"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit14"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression14"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator14"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit14"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount14"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit14"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount14"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit14"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit14"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount14"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit14"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount14"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit14"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N14"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 14 Description: <xsl:value-of select="@TME2Description14" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 15 Name: </strong><xsl:value-of select="@TME2Name15"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@TME2Label15"/>
          </div>
          <div class="ui-block-a">
            Date: <xsl:value-of select="@TME2Date15"/>
          </div>
          <div class="ui-block-b">
            Rel Label:  <xsl:value-of select="@TME2RelLabel15"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@TME2MathType15"/>
          </div>
          <div class="ui-block-b">
            Dist Type: <xsl:value-of select="@TME2Type15"/>
          </div>
          <div class="ui-block-a">
            Math Sub Type: <xsl:value-of select="@TME2MathSubType15"/>
          </div>
          <div class="ui-block-b">
            Base IO: <xsl:value-of select="@TME2BaseIO15"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@TME21Amount15"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@TME21Unit15"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@TME22Amount15"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@TME22Unit15"/>
          </div>
          <div class="ui-block-a">
            Q3 Amount:  <xsl:value-of select="@TME23Amount15"/>
          </div>
          <div class="ui-block-b">
            Q3 Unit: <xsl:value-of select="@TME23Unit15"/>
          </div>
          <div class="ui-block-a">
            Q4 Amount:  <xsl:value-of select="@TME24Amount15"/>
          </div>
          <div class="ui-block-b">
            Q4 Unit: <xsl:value-of select="@TME24Unit15"/>
          </div>
          <div class="ui-block-a">
            Q5 Amount: <xsl:value-of select="@TME25Amount15"/>
          </div>
          <div class="ui-block-b">
            Q5 Unit: <xsl:value-of select="@TME25Unit15"/>
          </div>
          <div class="ui-block-a">
            Math Express: <xsl:value-of select="@TME2MathExpression15"/>
          </div>
          <div class="ui-block-b">
            Math Operator: <xsl:value-of select="@TME2MathOperator15"/>
          </div>
          <div class="ui-block-a">
            QT Amount: <xsl:value-of select="@TME2TAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Unit: <xsl:value-of select="@TME2TUnit15"/>
          </div>
          <div class="ui-block-a">
            QT D1 Amount: <xsl:value-of select="@TME2TD1Amount15"/>
          </div>
          <div class="ui-block-b">
            QT D1 Unit: <xsl:value-of select="@TME2TD1Unit15"/>
          </div>
          <div class="ui-block-a">
            QT D2 Amount: <xsl:value-of select="@TME2TD2Amount15"/>
          </div>
          <div class="ui-block-b">
            QT D2 Unit: <xsl:value-of select="@TME2TD2Unit15"/>
          </div>
          <div class="ui-block-a">
            QT Most Amount: <xsl:value-of select="@TME2TMAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Most Unit: <xsl:value-of select="@TME2TMUnit15"/>
          </div>
          <div class="ui-block-a">
            QT Low Amount: <xsl:value-of select="@TME2TLAmount15"/>
          </div>
          <div class="ui-block-b">
            QT Low Unit: <xsl:value-of select="@TME2TLUnit15"/>
          </div>
          <div class="ui-block-a">
            QT High Amount: <xsl:value-of select="@TME2TUAmount15"/>
          </div>
          <div class="ui-block-b">
            QT High Unit: <xsl:value-of select="@TME2TUUnit15"/>
          </div>
          <div class="ui-block-a">
            Observations: <xsl:value-of select="@TME2N15"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
        <div>
			    Indic 15 Description: <xsl:value-of select="@TME2Description15" />
	      </div>
      </xsl:if>
    </div>
	</xsl:template>
</xsl:stylesheet>