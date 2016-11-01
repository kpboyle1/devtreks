<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
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
				Budget Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
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
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
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
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10"><strong>Time Period</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@EnterpriseName" /></strong>
			</td>
		</tr>
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
		<tr>
			<th scope="col" colspan="10">Outcomes</th>
		</tr>
		<xsl:apply-templates select="budgetoutcomes" />
    <tr>
			<th scope="col" colspan="10">Operations</th>
		</tr>
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
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
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
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
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
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none') or (@IndName1 != '' and @IndName1 != 'none')">
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
				  Base IO or Obs
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
    </xsl:if>
    <xsl:if test="(@IndName0 != '' and @IndName0 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName0" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel0" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate0"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel0" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType0"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType0"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount0"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount0"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount0"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit0"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression0"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName1 != '' and @IndName1 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName1" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel1" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate1"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel1" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType1"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType1"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit1"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit1"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit1"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount1"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit1"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription1" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression1"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName2 != '' and @IndName2 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName2" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel2" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate2"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel2" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType2"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType2"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit2"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit2"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit2"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount2"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit2"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription2" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression2"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName3 != '' and @IndName3 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName3" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel3" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate3"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel3" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType3"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType3"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit3"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit3"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit3"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount3"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit3"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription3" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression3"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName4 != '' and @IndName4 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName4" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel4" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate4"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel4" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType4"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType4"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit4"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit4"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit4"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount4"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit4"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription4" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression4"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName5 != '' and @IndName5 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName5" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel5" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate5"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel5" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType5"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType5"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit5"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit5"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit5"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount5"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit5"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription5" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression5"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName6 != '' and @IndName6 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName6" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel6" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate6"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel6" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType6"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType6"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit6"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit6"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit6"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount6"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit6"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription6" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression6"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName7 != '' and @IndName7 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName7" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel7" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate7"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel7" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType7"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType7"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit7"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit7"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit7"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount7"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit7"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription7" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression7"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName8 != '' and @IndName8 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName8" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel8" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate8"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel8" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType8"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType8"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit8"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit8"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit8"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount8"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit8"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription8" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression8"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName9 != '' and @IndName9 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName9" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel9" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate9"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel9" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType9"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType9"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit9"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit9"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit9"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount9"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit9"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription9" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression9"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName10 != '' and @IndName10 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName10" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel10" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate10"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel10" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType10"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType10"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit10"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit10"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit10"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount10"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit10"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription10" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression10"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName11 != '' and @IndName11 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName11" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel11" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate11"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel11" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType11"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType11"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit11"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit11"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit11"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount11"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit11"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription11" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression11"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName12 != '' and @IndName12 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName12" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel12" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate12"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel12" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType12"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType12"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit12"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit12"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit12"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount12"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit12"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription12" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression12"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName13 != '' and @IndName13 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName13" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel13" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate13"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel13" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType13"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType13"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit13"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit13"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit13"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount13"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit13"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription13" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression13"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName14 != '' and @IndName14 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName14" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel14" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate14"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel14" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType14"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType14"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit14"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit14"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit14"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount14"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit14"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription14" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression14"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName15 != '' and @IndName15 != 'none')">
		  <tr>
        <td colspan="2">
          <strong><xsl:value-of select="@IndName15" /></strong> 
			  </td>
			  <td>
					  <xsl:value-of select="@IndLabel15" />
			  </td>
        <td>
					  <xsl:value-of select="@IndDate15"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndRelLabel15" />
			  </td>
        <td>
					  <xsl:value-of select="@IndMathType15"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndType15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndBaseIO15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathOperator15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndMathSubType15"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@Ind1Amount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit15"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind3Amount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind3Unit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind4Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind4Unit15"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind5Unit15"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@IndTAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTD1Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTD2Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTMAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTMUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndTLAmount15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTLUnit15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUAmount15"/>
			  </td>
        <td>
          <xsl:value-of select="@IndTUUnit15"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndDescription15" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@IndMathExpression15"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name0" /></strong> (<xsl:value-of select="@TME2N0" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label0" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel0" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type0"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N0" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator0"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType0"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount0"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit0"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount0"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit0"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description0" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression0"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name1" /></strong> (<xsl:value-of select="@TME2N1" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label1" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel1" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type1"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N1" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator1"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType1"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit1"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount1"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit1"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount1"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit1"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description1" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression1"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name2" /></strong> (<xsl:value-of select="@TME2N2" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label2" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel2" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type2"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N2" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator2"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType2"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit2"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount2"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit2"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount2"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit2"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description2" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression2"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name3" /></strong> (<xsl:value-of select="@TME2N3" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label3" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel3" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type3"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N3" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator3"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType3"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit3"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount3"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit3"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount3"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit3"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description3" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression3"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name4" /></strong> (<xsl:value-of select="@TME2N4" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label4" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel4" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type4"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N4" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator4"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType4"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit4"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount4"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit4"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount4"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit4"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description4" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression4"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name5" /></strong> (<xsl:value-of select="@TME2N5" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label5" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel5" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type5"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N5" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator5"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType5"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit5"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount5"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit5"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount5"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit5"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description5" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression5"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name6" /></strong> (<xsl:value-of select="@TME2N6" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label6" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel6" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type6"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N6" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator6"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType6"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit6"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount6"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit6"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount6"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit6"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description6" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression6"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name7" /></strong> (<xsl:value-of select="@TME2N7" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label7" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel7" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type7"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N7" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator7"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType7"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit7"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount7"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit7"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount7"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit7"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description7" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression7"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name8" /></strong> (<xsl:value-of select="@TME2N8" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label8" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel8" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type8"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N8" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator8"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType8"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit8"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount8"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit8"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount8"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit8"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description8" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression8"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name9" /></strong> (<xsl:value-of select="@TME2N9" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label9" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel9" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type9"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N9" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator9"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType9"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit9"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount9"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit9"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount9"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit9"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description9" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression9"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name10" /></strong> (<xsl:value-of select="@TME2N10" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label10" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel10" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type10"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N10" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator10"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType10"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit10"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount10"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit10"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount10"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit10"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description10" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression10"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name11" /></strong> (<xsl:value-of select="@TME2N11" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label11" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel11" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type11"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N11" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator11"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType11"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit11"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount11"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit11"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount11"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit11"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description11" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression11"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name12" /></strong> (<xsl:value-of select="@TME2N12" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label12" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel12" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type12"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N12" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator12"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType12"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit12"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount12"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit12"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount12"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit12"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description12" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression12"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name13" /></strong> (<xsl:value-of select="@TME2N13" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label13" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel13" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type13"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N13" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator13"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType13"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit13"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount13"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit13"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount13"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit13"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description13" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression13"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name14" /></strong> (<xsl:value-of select="@TME2N14" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label14" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel14" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type14"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N14" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator14"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType14"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit14"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount14"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit14"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount14"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit14"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description15" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression15"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
      <tr>
        <td colspan="2">
					   <strong><xsl:value-of select="@TME2Name15" /></strong> (<xsl:value-of select="@TME2N15" />)
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label15" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2Date15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2RelLabel15" />
			  </td>
        <td>
					  <xsl:value-of select="@TME2MathType15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Type15"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2N15" />
			  </td>
        <td>
          <xsl:value-of select="@TME2MathOperator15"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2MathSubType15"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME21Amount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME21Unit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME22Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME22Unit15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME23Amount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME23Unit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME24Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME24Unit15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME25Unit15"/>
			  </td>
		  </tr>
      <tr>
        <td>
					  <xsl:value-of select="@TME2TAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TD1Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TD2Amount15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2TMAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TLAmount15"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TLUnit15"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUAmount15"/>
			  </td>
        <td>
          <xsl:value-of select="@TME2TUUnit15"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2Description15" />
        </td>
      </tr>
      <tr>
        <td colspan="10">
          <xsl:value-of select="@TME2MathExpression15"/>
        </td>
      </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
