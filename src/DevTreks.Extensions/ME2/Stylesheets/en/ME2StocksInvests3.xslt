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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
		<tr>
			<th scope="col" colspan="10">
				Investment Group
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
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<tr>
			<th scope="col" colspan="10">
				Investment
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
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<tr>
			<th scope="col" colspan="10"><strong>Time Period</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@PracticeName" /></strong>
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
		<xsl:apply-templates select="investmentoutcomes" />
    <tr>
			<th scope="col" colspan="10">Components</th>
		</tr>
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
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
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
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
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
    <tr>
			<th scope="col" colspan="10"><strong>Component</strong></th>
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
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
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
    <xsl:if test="(@IndName1 != '' or @IndName2 != '')">
      <tr>
        <th>
				     Type
			  </th>
			  <th>
					  Weight
			  </th>
			  <th>
					  Date
			  </th>
			  <th>
					  Math Type
			  </th>
        <th>
					  Amount 1
			  </th>
			  <th>
					  Unit 1
			  </th>
			  <th>
					  Amount 2
			  </th>
        <th>
					  Unit 2
			  </th>
        <th>
					  Total
			  </th>
        <th>
            Unit
			  </th>
		  </tr>
    </xsl:if>
    <xsl:if test="(@IndName1 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 1: </strong> <xsl:value-of select="@IndName1" />&#xA0;<xsl:value-of select="@IndLabel1" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType1"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal1"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit1"/>
			  </td>
		  </tr>
      <tr>
      <td colspan="10">
        <strong>Description : </strong>
        <xsl:value-of select="@IndDescription1" />
			</td>
    </tr>
    </xsl:if>
    <xsl:if test="(@IndName2 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 2: </strong> <xsl:value-of select="@IndName2" />&#xA0;<xsl:value-of select="@IndLabel2" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType2"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal2"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit2"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription2" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName3 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 3: </strong> <xsl:value-of select="@IndName3" />&#xA0;<xsl:value-of select="@IndLabel3" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType3"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal3"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit3"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription3" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName4 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 4: </strong> <xsl:value-of select="@IndName4" />&#xA0;<xsl:value-of select="@IndLabel4" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType4"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal4"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit4"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription4" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName5 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 5: </strong> <xsl:value-of select="@IndName5" />&#xA0;<xsl:value-of select="@IndLabel5" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType5"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal5"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit5"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription5" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName6 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 6: </strong> <xsl:value-of select="@IndName6" />&#xA0;<xsl:value-of select="@IndLabel6" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType6"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal6"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit6"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription6" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName7 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 7: </strong> <xsl:value-of select="@IndName7" />&#xA0;<xsl:value-of select="@IndLabel7" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType7"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal7"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit7"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription7" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName8 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 8: </strong> <xsl:value-of select="@IndName8" />&#xA0;<xsl:value-of select="@IndLabel8" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType8"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal8"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit8"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription8" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName9 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 9: </strong> <xsl:value-of select="@IndName9" />&#xA0;<xsl:value-of select="@IndLabel9" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType9"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal9"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit9"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription9" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName10 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 10: </strong> <xsl:value-of select="@IndName10" />&#xA0;<xsl:value-of select="@IndLabel10" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType10"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal10"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit10"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription10" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName11 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 11: </strong> <xsl:value-of select="@IndName11" />&#xA0;<xsl:value-of select="@IndLabel11" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType11"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal11"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit11"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription11" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName12 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 12: </strong> <xsl:value-of select="@IndName12" />&#xA0;<xsl:value-of select="@IndLabel12" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType12"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal12"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit12"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription12" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName13 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 13: </strong> <xsl:value-of select="@IndName13" />&#xA0;<xsl:value-of select="@IndLabel13" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType13"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal13"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit13"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription13" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName14 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 14: </strong> <xsl:value-of select="@IndName14" />&#xA0;<xsl:value-of select="@IndLabel14" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType14"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal14"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit14"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription14" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName15 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 15: </strong> <xsl:value-of select="@IndName15" />&#xA0;<xsl:value-of select="@IndLabel15" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType15"/>
			  </td>
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
					  <xsl:value-of select="@IndTotal15"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit15"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription15" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName16 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 16: </strong> <xsl:value-of select="@IndName16" />&#xA0;<xsl:value-of select="@IndLabel16" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType16"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight16"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate16"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType16"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind1Amount16"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit16"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount16"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit16"/>
			  </td>   
        <td>
					  <xsl:value-of select="@IndTotal16"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit16"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription16" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName17 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 17: </strong> <xsl:value-of select="@IndName17" />&#xA0;<xsl:value-of select="@IndLabel17" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType17"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight17"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate17"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType17"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind1Amount17"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit17"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount17"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit17"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTotal17"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit17"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription17" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName18 != '')">
      <tr>
        <td colspan="10">
				    <strong>Indicator 18: </strong> <xsl:value-of select="@IndName18" />&#xA0;<xsl:value-of select="@IndLabel18" />
			  </td>
      </tr>
		  <tr>
        <td>
					  <xsl:value-of select="@IndType18"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight18"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate18"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType18"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind1Amount18"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit18"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount18"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit18"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTotal18"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit18"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription18" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@IndName19 != '')">
        <tr>
          <td colspan="10">
				      <strong>Indicator 19: </strong> <xsl:value-of select="@IndName19" />&#xA0;<xsl:value-of select="@IndLabel19" />
			    </td>
        </tr>
		    <tr>
        <td>
					  <xsl:value-of select="@IndType19"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight19"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate19"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType19"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind1Amount19"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit19"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount19"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit19"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTotal19"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit19"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription19" />
			  </td>
      </tr>
      </xsl:if>
      <xsl:if test="(@IndName20 != '')">
        <tr>
          <td colspan="10">
				      <strong>Indicator 20: </strong> <xsl:value-of select="@IndName20" />&#xA0;<xsl:value-of select="@IndLabel20" />
			    </td>
        </tr>
		    <tr>
        <td>
					  <xsl:value-of select="@IndType20"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndWeight20"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndDate20"/>
			  </td>
			  <td>
					  <xsl:value-of select="@IndMathType20"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind1Amount20"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind1Unit20"/>
			  </td>
			  <td>
					  <xsl:value-of select="@Ind2Amount20"/>
			  </td>
        <td>
					  <xsl:value-of select="@Ind2Unit20"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndTotal20"/>
			  </td>
        <td>
					  <xsl:value-of select="@IndUnit20"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@IndDescription20" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' or @TME2Name2 != '')">
      <tr>
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
					  Q1 Total
			  </th>
			  <th>
					  Q1 Unit
			  </th>
        <th>
					  Q2 Total
			  </th>
			  <th>
					  Q2 Unit
			  </th>
			  <th>
			  </th>
        <th>
			  </th>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit1"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description1" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name2 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit2"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description2" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name3 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit3"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description3" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name4 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit4"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description4" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name5 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit5"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description5" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name6 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit6"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description6" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name7 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit7"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description7" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name8 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit8"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description8" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name9 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit9"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description9" />
			  </td>
      </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name10 != '')">
		  <tr>
        <td>
					  <xsl:value-of select="@TME2Name10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Label10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Unit10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1Unit10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2Unit10"/>
			  </td>
			  <td>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong>
          <xsl:value-of select="@TME2Description10" />
			  </td>
      </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
